namespace IcecastStreamMonitor.Models;

public class AppSettings
{
    public string DefaultPassword { get; set; } = "hackme";
    public string MasterAdminPasswordHash { get; set; } = string.Empty;
    public string MasterAdminPassword { get; set; } = "hackme";
    public int PollingIntervalSeconds { get; set; } = 5;
    public int RequestTimeoutSeconds { get; set; } = 4;
    public bool AutoRefreshEnabled { get; set; } = true;
    public bool MinimizeToTray { get; set; } = true;
    public bool CloseToTray { get; set; } = false;
    public bool NotifyOnStreamOffline { get; set; } = true;
    public bool RemoteServerEnabled { get; set; } = true;
    public int RemoteServerPort { get; set; } = 8080;
    public bool StreamingServerEnabled { get; set; } = true;
    public int StreamingServerPort { get; set; } = 11154;
    public string DefaultSourcePassword { get; set; } = "hackme";
    public List<string> TrustedProxies { get; set; } = new() { "127.0.0.1", "::1", "localhost" };
    public List<RemoteUser> RemoteUsers { get; set; } = new();
    public List<StreamChannel> Channels { get; set; } = new();

    public int WindowWidth { get; set; } = 950;
    public int WindowHeight { get; set; } = 560;
    public int WindowLeft { get; set; } = -1;
    public int WindowTop { get; set; } = -1;
    public bool IsMaximized { get; set; } = false;
}
