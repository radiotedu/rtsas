namespace IcecastStreamMonitor.Models;

public class StreamChannel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 8000;
    public string MountPath { get; set; } = "/stream";
    public ServerType ServerType { get; set; } = ServerType.Auto;
    public bool UseDefaultPassword { get; set; } = true;
    public string CustomPassword { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string FormattedUrl
    {
        get
        {
            var host = string.IsNullOrWhiteSpace(Host) ? "localhost" : Host.Trim();
            var path = MountPath.StartsWith('/') ? MountPath : "/" + MountPath;
            return $"http://{host}:{Port}{path}";
        }
    }

    public StreamChannel Clone()
    {
        return new StreamChannel
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Host = Host,
            Port = Port,
            MountPath = MountPath,
            ServerType = ServerType,
            UseDefaultPassword = UseDefaultPassword,
            CustomPassword = CustomPassword,
            Enabled = Enabled,
            CreatedAt = DateTime.UtcNow
        };
    }
}
