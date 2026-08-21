namespace IcecastStreamMonitor.Models;

public class StreamSnapshotLog
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public Guid ChannelId { get; set; }
    public bool IsOnline { get; set; }
    public int Listeners { get; set; }
    public int BitRateKbps { get; set; }
    public long LatencyMs { get; set; }
    public double MegaBytesTransferred { get; set; }
    public string SongTitle { get; set; } = string.Empty;
    public string StatusMessage { get; set; } = string.Empty;
}

public class Stream24HourMetrics
{
    public int PeakListeners { get; set; }
    public double AverageListeners { get; set; }
    public int MinListeners { get; set; }
    public double TotalMegaBytesTransferred { get; set; }
    public double UptimePercentage { get; set; }
    public double AverageLatencyMs { get; set; }
    public int TotalSnapshots { get; set; }
}
