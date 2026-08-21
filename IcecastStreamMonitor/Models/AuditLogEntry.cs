namespace IcecastStreamMonitor.Models;

public enum AuditCategory
{
    ConfigChange,
    AuthChange,
    ChannelManagement,
    StreamAlert,
    System
}

public class AuditLogEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public AuditCategory Category { get; set; } = AuditCategory.ConfigChange;
    public string ChannelName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;

    public string FormattedTimestamp => Timestamp.ToString("yyyy-MM-dd HH:mm:ss");

    public string CategoryDisplayName => Category switch
    {
        AuditCategory.ConfigChange => "CONFIG",
        AuditCategory.AuthChange => "AUTH",
        AuditCategory.ChannelManagement => "CHANNEL",
        AuditCategory.StreamAlert => "ALERT",
        AuditCategory.System => "SYSTEM",
        _ => "INFO"
    };

    public override string ToString()
    {
        var target = string.IsNullOrWhiteSpace(ChannelName) ? "" : $" [{ChannelName}]";
        return $"[{FormattedTimestamp}] [{CategoryDisplayName}]{target} {Action}: {Details}";
    }
}
