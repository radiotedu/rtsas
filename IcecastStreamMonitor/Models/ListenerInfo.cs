namespace IcecastStreamMonitor.Models;

public class ListenerInfo
{
    public int Id { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public int ConnectedSeconds { get; set; }
    public long BytesSent { get; set; }
    public string Location { get; set; } = "Unknown";

    public string FormattedConnectedTime
    {
        get
        {
            var ts = TimeSpan.FromSeconds(ConnectedSeconds);
            if (ts.TotalHours >= 1)
                return $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
            return $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }
    }

    public string CleanUserAgent
    {
        get
        {
            if (string.IsNullOrWhiteSpace(UserAgent)) return "Unknown Player";
            return UserAgent.Trim();
        }
    }
}
