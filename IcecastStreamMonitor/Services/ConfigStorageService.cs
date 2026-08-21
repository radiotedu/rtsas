using System.Text.Json;
using IcecastStreamMonitor.Models;

namespace IcecastStreamMonitor.Services;

public class ConfigStorageService
{
    private readonly string _configFilePath;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public ConfigStorageService(string? customPath = null)
    {
        if (!string.IsNullOrEmpty(customPath))
        {
            _configFilePath = customPath;
        }
        else
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(appData, "IcecastStreamMonitor");
            Directory.CreateDirectory(appFolder);
            _configFilePath = Path.Combine(appFolder, "config.json");
        }
    }

    public string ConfigFilePath => _configFilePath;

    public AppSettings LoadSettings()
    {
        try
        {
            if (File.Exists(_configFilePath))
            {
                var json = File.ReadAllText(_configFilePath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions);
                if (settings != null)
                {
                    settings.Channels ??= new List<StreamChannel>();
                    settings.RemoteUsers ??= new List<RemoteUser>();

                    // Decrypt encrypted password fields
                    settings.DefaultPassword = SecurityHelper.DecryptSecret(settings.DefaultPassword);
                    settings.MasterAdminPassword = SecurityHelper.DecryptSecret(settings.MasterAdminPassword);

                    if (string.IsNullOrEmpty(settings.MasterAdminPassword))
                    {
                        settings.MasterAdminPassword = !string.IsNullOrEmpty(settings.DefaultPassword) ? settings.DefaultPassword : "admin";
                    }

                    if (string.IsNullOrEmpty(settings.MasterAdminPasswordHash))
                    {
                        settings.MasterAdminPasswordHash = SecurityHelper.HashPassword(settings.MasterAdminPassword);
                    }

                    if (!string.IsNullOrEmpty(settings.DefaultSourcePassword))
                    {
                        settings.DefaultSourcePassword = SecurityHelper.DecryptSecret(settings.DefaultSourcePassword);
                    }
                    else
                    {
                        settings.DefaultSourcePassword = settings.DefaultPassword;
                    }

                    foreach (var ch in settings.Channels)
                    {
                        if (!string.IsNullOrEmpty(ch.CustomPassword))
                        {
                            ch.CustomPassword = SecurityHelper.DecryptSecret(ch.CustomPassword);
                        }
                    }

                    foreach (var user in settings.RemoteUsers)
                    {
                        if (!string.IsNullOrEmpty(user.PlainPassword))
                        {
                            user.PlainPassword = SecurityHelper.DecryptSecret(user.PlainPassword);
                        }
                    }

                    EnsureDefaultRemoteUsers(settings);
                    return settings;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to load settings: {ex.Message}");
        }

        var defaultSettings = GetDefaultSettings();
        EnsureDefaultRemoteUsers(defaultSettings);
        return defaultSettings;
    }

    public void SaveSettings(AppSettings settings)
    {
        try
        {
            var dir = Path.GetDirectoryName(_configFilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // Create a clone with encrypted secrets for serialization
            var clone = new AppSettings
            {
                DefaultPassword = SecurityHelper.EncryptSecret(settings.DefaultPassword),
                MasterAdminPassword = SecurityHelper.EncryptSecret(settings.MasterAdminPassword),
                MasterAdminPasswordHash = !string.IsNullOrEmpty(settings.MasterAdminPasswordHash)
                    ? settings.MasterAdminPasswordHash
                    : SecurityHelper.HashPassword(settings.MasterAdminPassword),
                DefaultSourcePassword = SecurityHelper.EncryptSecret(settings.DefaultSourcePassword),
                StreamingServerEnabled = settings.StreamingServerEnabled,
                StreamingServerPort = settings.StreamingServerPort,
                TrustedProxies = new List<string>(settings.TrustedProxies),
                PollingIntervalSeconds = settings.PollingIntervalSeconds,
                RequestTimeoutSeconds = settings.RequestTimeoutSeconds,
                AutoRefreshEnabled = settings.AutoRefreshEnabled,
                MinimizeToTray = settings.MinimizeToTray,
                CloseToTray = settings.CloseToTray,
                NotifyOnStreamOffline = settings.NotifyOnStreamOffline,
                RemoteServerEnabled = settings.RemoteServerEnabled,
                RemoteServerPort = settings.RemoteServerPort,
                WindowWidth = settings.WindowWidth,
                WindowHeight = settings.WindowHeight,
                WindowLeft = settings.WindowLeft,
                WindowTop = settings.WindowTop,
                IsMaximized = settings.IsMaximized,
                Channels = settings.Channels.Select(c =>
                {
                    var chClone = c.Clone();
                    chClone.CustomPassword = SecurityHelper.EncryptSecret(c.CustomPassword);
                    return chClone;
                }).ToList(),
                RemoteUsers = settings.RemoteUsers.Select(u => new RemoteUser
                {
                    Id = u.Id,
                    Username = u.Username,
                    PasswordHash = !string.IsNullOrEmpty(u.PasswordHash)
                        ? u.PasswordHash
                        : SecurityHelper.HashPassword(u.PlainPassword),
                    PlainPassword = SecurityHelper.EncryptSecret(u.PlainPassword),
                    RoleName = u.RoleName,
                    IsActive = u.IsActive,
                    Permissions = new RemotePermissions
                    {
                        CanViewStatus = u.Permissions.CanViewStatus,
                        CanViewLogs = u.Permissions.CanViewLogs,
                        CanViewListenerIps = u.Permissions.CanViewListenerIps,
                        CanRefreshStreams = u.Permissions.CanRefreshStreams,
                        CanManageChannels = u.Permissions.CanManageChannels,
                        CanViewAuditLogs = u.Permissions.CanViewAuditLogs
                    },
                    CreatedAt = u.CreatedAt
                }).ToList()
            };

            var json = JsonSerializer.Serialize(clone, JsonOptions);
            var tempPath = _configFilePath + ".tmp";
            File.WriteAllText(tempPath, json);
            File.Move(tempPath, _configFilePath, overwrite: true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
        }
    }

    private static void EnsureDefaultRemoteUsers(AppSettings settings)
    {
        if (settings.RemoteUsers.Count == 0)
        {
            // SuperAdmin user
            var adminUser = new RemoteUser
            {
                Username = "admin",
                PlainPassword = !string.IsNullOrEmpty(settings.MasterAdminPassword) ? settings.MasterAdminPassword : "admin",
                PasswordHash = SecurityHelper.HashPassword(!string.IsNullOrEmpty(settings.MasterAdminPassword) ? settings.MasterAdminPassword : "admin"),
                RoleName = "SuperAdmin",
                IsActive = true,
                Permissions = new RemotePermissions
                {
                    CanViewStatus = true,
                    CanViewLogs = true,
                    CanViewListenerIps = true,
                    CanRefreshStreams = true,
                    CanManageChannels = true,
                    CanViewAuditLogs = true
                }
            };

            // Standard Viewer user (Limited: cannot see IPs or manage channels)
            var viewerUser = new RemoteUser
            {
                Username = "viewer",
                PlainPassword = "viewer123",
                PasswordHash = SecurityHelper.HashPassword("viewer123"),
                RoleName = "Standard Viewer",
                IsActive = true,
                Permissions = new RemotePermissions
                {
                    CanViewStatus = true,
                    CanViewLogs = true,
                    CanViewListenerIps = false, // Restricted!
                    CanRefreshStreams = true,
                    CanManageChannels = false,  // Restricted!
                    CanViewAuditLogs = false
                }
            };

            settings.RemoteUsers.Add(adminUser);
            settings.RemoteUsers.Add(viewerUser);
        }
    }

    public static AppSettings GetDefaultSettings()
    {
        var defaultPass = "hackme";
        var settings = new AppSettings
        {
            DefaultPassword = defaultPass,
            MasterAdminPassword = defaultPass,
            MasterAdminPasswordHash = SecurityHelper.HashPassword(defaultPass),
            PollingIntervalSeconds = 5,
            RequestTimeoutSeconds = 4,
            AutoRefreshEnabled = true,
            RemoteServerEnabled = true,
            RemoteServerPort = 8080,
            Channels = new List<StreamChannel>()
        };

        EnsureDefaultRemoteUsers(settings);
        return settings;
    }
}
