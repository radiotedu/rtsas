using System.Text.Json;
using IcecastStreamMonitor.Models;

namespace IcecastStreamMonitor.Services;

public class AuditLogService
{
    private readonly List<AuditLogEntry> _entries = new();
    private readonly string _storagePath;
    private readonly object _lock = new();

    public event Action<AuditLogEntry>? OnLogAdded;

    public AuditLogService(string? customPath = null)
    {
        if (!string.IsNullOrEmpty(customPath))
        {
            _storagePath = customPath;
        }
        else
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(appData, "IcecastStreamMonitor");
            Directory.CreateDirectory(appFolder);
            _storagePath = Path.Combine(appFolder, "audit_logs.json");
        }

        LoadLogs();

        if (_entries.Count == 0)
        {
            // Initial startup log
            Log(AuditCategory.System, "", "System Initialized", "Icecast & SHOUTcast Live Stream Monitor started.");
        }
    }

    public void Log(AuditCategory category, string channelName, string action, string details)
    {
        var entry = new AuditLogEntry
        {
            Timestamp = DateTime.Now,
            Category = category,
            ChannelName = channelName,
            Action = action,
            Details = details
        };

        lock (_lock)
        {
            _entries.Add(entry);
            // Cap at 2000 entries
            if (_entries.Count > 2000)
            {
                _entries.RemoveRange(0, _entries.Count - 2000);
            }
        }

        SaveLogs();
        OnLogAdded?.Invoke(entry);
    }

    public void LogChannelAdded(StreamChannel ch)
    {
        var auth = ch.UseDefaultPassword ? "Default Password" : "Custom Password";
        Log(AuditCategory.ChannelManagement, ch.Name, "Channel Created",
            $"Added new channel '{ch.Name}' (Mount: {ch.MountPath}, Server: {ch.Host}:{ch.Port}, Type: {ch.ServerType}, Auth: {auth})");
    }

    public void LogChannelModified(StreamChannel oldCh, StreamChannel newCh)
    {
        var changes = new List<string>();

        if (oldCh.Name != newCh.Name)
            changes.Add($"Label: '{oldCh.Name}' → '{newCh.Name}'");

        if (oldCh.MountPath != newCh.MountPath)
            changes.Add($"Mount Path: '{oldCh.MountPath}' → '{newCh.MountPath}'");

        if (oldCh.Host != newCh.Host || oldCh.Port != newCh.Port)
            changes.Add($"Server: '{oldCh.Host}:{oldCh.Port}' → '{newCh.Host}:{newCh.Port}'");

        if (oldCh.ServerType != newCh.ServerType)
            changes.Add($"Type: {oldCh.ServerType} → {newCh.ServerType}");

        if (oldCh.UseDefaultPassword != newCh.UseDefaultPassword || (!newCh.UseDefaultPassword && oldCh.CustomPassword != newCh.CustomPassword))
        {
            var oldAuth = oldCh.UseDefaultPassword ? "Default" : "Custom Password";
            var newAuth = newCh.UseDefaultPassword ? "Default" : "Custom Password (Updated)";
            changes.Add($"Authentication: {oldAuth} → {newAuth}");
        }

        if (oldCh.Enabled != newCh.Enabled)
            changes.Add($"Active State: {(oldCh.Enabled ? "Enabled" : "Disabled")} → {(newCh.Enabled ? "Enabled" : "Disabled")}");

        if (changes.Count > 0)
        {
            Log(AuditCategory.ConfigChange, newCh.Name, "Configuration Modified", string.Join("; ", changes));
        }
    }

    public void LogChannelDeleted(StreamChannel ch)
    {
        Log(AuditCategory.ChannelManagement, ch.Name, "Channel Deleted", $"Removed channel '{ch.Name}' ({ch.MountPath})");
    }

    public void LogChannelToggled(StreamChannel ch, bool newState)
    {
        Log(AuditCategory.ConfigChange, ch.Name, "Monitoring Toggled", $"Channel monitoring set to {(newState ? "ACTIVE" : "PAUSED")}");
    }

    public void LogGlobalSettingsChanged(AppSettings oldSettings, AppSettings newSettings)
    {
        var changes = new List<string>();

        if (oldSettings.DefaultPassword != newSettings.DefaultPassword)
            changes.Add("Global Default Password updated");

        if (oldSettings.PollingIntervalSeconds != newSettings.PollingIntervalSeconds)
            changes.Add($"Refresh interval: {oldSettings.PollingIntervalSeconds}s → {newSettings.PollingIntervalSeconds}s");

        if (oldSettings.RequestTimeoutSeconds != newSettings.RequestTimeoutSeconds)
            changes.Add($"Timeout: {oldSettings.RequestTimeoutSeconds}s → {newSettings.RequestTimeoutSeconds}s");

        if (changes.Count > 0)
        {
            Log(AuditCategory.ConfigChange, "Global", "Settings Updated", string.Join("; ", changes));
        }
    }

    public void LogStreamAlert(string channelName, string mountPath, string alertMessage)
    {
        Log(AuditCategory.StreamAlert, channelName, "STREAM ALERT", $"Mount '{mountPath}': {alertMessage}");
    }

    public List<AuditLogEntry> GetEntries()
    {
        lock (_lock)
        {
            return _entries.OrderByDescending(e => e.Timestamp).ToList();
        }
    }

    public void ClearLogs()
    {
        lock (_lock)
        {
            _entries.Clear();
        }
        SaveLogs();
        Log(AuditCategory.System, "", "Logs Cleared", "Audit log console history was cleared by user.");
    }

    private void SaveLogs()
    {
        try
        {
            lock (_lock)
            {
                var json = JsonSerializer.Serialize(_entries, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_storagePath, json);
            }
        }
        catch { }
    }

    private void LoadLogs()
    {
        try
        {
            if (File.Exists(_storagePath))
            {
                var json = File.ReadAllText(_storagePath);
                var list = JsonSerializer.Deserialize<List<AuditLogEntry>>(json);
                if (list != null)
                {
                    _entries.AddRange(list);
                }
            }
        }
        catch { }
    }
}
