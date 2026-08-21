namespace IcecastStreamMonitor.Models;

public class StreamStatus
{
    public Guid ChannelId { get; set; }
    public bool IsOnline { get; set; }
    public int ConcurrentListeners { get; set; }
    public int PeakListeners { get; set; }
    public int MaxListeners { get; set; }
    public string AudioFormat { get; set; } = "Unknown";
    public int BitRateKbps { get; set; }
    public int SampleRateHz { get; set; }
    public int Channels { get; set; }
    public string StreamTitle { get; set; } = string.Empty;
    public string ServerDescription { get; set; } = string.Empty;
    public long LatencyMs { get; set; }
    public DateTime LastChecked { get; set; } = DateTime.MinValue;
    public string StatusMessage { get; set; } = "Not checked";
    public string RawContentType { get; set; } = string.Empty;

    public static StreamStatus Offline(Guid channelId, string message, long latencyMs = 0)
    {
        return new StreamStatus
        {
            ChannelId = channelId,
            IsOnline = false,
            ConcurrentListeners = 0,
            AudioFormat = "-",
            BitRateKbps = 0,
            LatencyMs = latencyMs,
            LastChecked = DateTime.Now,
            StatusMessage = message
        };
    }
}
