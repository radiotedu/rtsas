namespace IcecastStreamMonitor.RemoteControl.Models;

public class RemotePermissions
{
    public bool CanViewStatus { get; set; } = true;
    public bool CanViewLogs { get; set; } = true;
    public bool CanViewListenerIps { get; set; } = false;
    public bool CanRefreshStreams { get; set; } = true;
    public bool CanManageChannels { get; set; } = false;
    public bool CanViewAuditLogs { get; set; } = false;
}

public class RemoteUserSession
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = "Viewer";
    public RemotePermissions Permissions { get; set; } = new();
}

public class RemoteStreamItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MountPath { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string ServerType { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public bool IsOnline { get; set; }
    public int ConcurrentListeners { get; set; }
    public int PeakListeners { get; set; }
    public int BitRateKbps { get; set; }
    public string AudioFormat { get; set; } = string.Empty;
    public string StreamTitle { get; set; } = string.Empty;
    public long LatencyMs { get; set; }
    public DateTime? LastChecked { get; set; }
}

public class RemoteSnapshotLog
{
    public DateTime Timestamp { get; set; }
    public Guid ChannelId { get; set; }
    public bool IsOnline { get; set; }
    public int Listeners { get; set; }
    public int BitRateKbps { get; set; }
    public long LatencyMs { get; set; }
    public double MegaBytesTransferred { get; set; }
    public string SongTitle { get; set; } = string.Empty;
    public string StatusMessage { get; set; } = string.Empty;
}

public class Remote24HourMetrics
{
    public int PeakListeners { get; set; }
    public double AverageListeners { get; set; }
    public int MinListeners { get; set; }
    public double TotalMegaBytesTransferred { get; set; }
    public double UptimePercentage { get; set; }
    public double AverageLatencyMs { get; set; }
    public int TotalSnapshots { get; set; }
}

public class RemoteListenerInfo
{
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public int ConnectedSeconds { get; set; }
    public string Location { get; set; } = "Remote Client";
    public string FormattedConnectedTime => TimeSpan.FromSeconds(ConnectedSeconds).ToString(@"hh\:mm\:ss");
}
