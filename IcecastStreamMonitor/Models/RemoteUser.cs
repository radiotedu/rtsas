namespace IcecastStreamMonitor.Models;

public class RemotePermissions
{
    public bool CanViewStatus { get; set; } = true;
    public bool CanViewLogs { get; set; } = true;
    public bool CanViewListenerIps { get; set; } = false;
    public bool CanRefreshStreams { get; set; } = true;
    public bool CanManageChannels { get; set; } = false;
    public bool CanViewAuditLogs { get; set; } = false;
}

public class RemoteUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PlainPassword { get; set; } = string.Empty;
    public string RoleName { get; set; } = "Viewer";
    public bool IsActive { get; set; } = true;
    public RemotePermissions Permissions { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
