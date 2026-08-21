using System.Text.Json;
using IcecastStreamMonitor.Models;

namespace IcecastStreamMonitor.Services;

public class TinyIceImportResult
{
    public int Port { get; set; } = 11154;
    public string Hostname { get; set; } = "127.0.0.1";
    public string AdminUser { get; set; } = "radiotedu";
    public List<StreamChannel> Channels { get; set; } = new();
    public List<string> TrustedProxies { get; set; } = new();
    public int TotalMountsCount => Channels.Count;
}

public static class TinyIceConfigImporter
{
    public static TinyIceImportResult ImportFromTinyIceJson(string jsonContent, string defaultHost = "127.0.0.1")
    {
        var result = new TinyIceImportResult();
        if (string.IsNullOrWhiteSpace(jsonContent))
        {
            return result;
        }

        try
        {
            using var doc = JsonDocument.Parse(jsonContent);
            var root = doc.RootElement;

            // 1. Port
            if (root.TryGetProperty("port", out var portProp))
            {
                if (portProp.ValueKind == JsonValueKind.Number && portProp.TryGetInt32(out var pNum))
                {
                    result.Port = pNum;
                }
                else if (portProp.ValueKind == JsonValueKind.String && int.TryParse(portProp.GetString(), out var pStr))
                {
                    result.Port = pStr;
                }
            }

            // 2. Hostname
            if (root.TryGetProperty("hostname", out var hostProp) && !string.IsNullOrWhiteSpace(hostProp.GetString()))
            {
                result.Hostname = hostProp.GetString()!;
            }

            // 3. Admin User
            if (root.TryGetProperty("admin_user", out var adminProp) && !string.IsNullOrWhiteSpace(adminProp.GetString()))
            {
                result.AdminUser = adminProp.GetString()!;
            }

            // 4. Trusted Proxies
            if (root.TryGetProperty("trusted_proxies", out var proxiesProp) && proxiesProp.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in proxiesProp.EnumerateArray())
                {
                    var proxy = item.GetString();
                    if (!string.IsNullOrWhiteSpace(proxy))
                    {
                        result.TrustedProxies.Add(proxy.Trim());
                    }
                }
            }

            // 5. Mounts
            if (root.TryGetProperty("mounts", out var mountsProp) && mountsProp.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in mountsProp.EnumerateObject())
                {
                    var rawMount = prop.Name.Trim();
                    if (string.IsNullOrEmpty(rawMount)) continue;

                    var mountPath = rawMount.StartsWith('/') ? rawMount : "/" + rawMount;
                    var friendlyName = FormatFriendlyChannelName(mountPath);

                    var channel = new StreamChannel
                    {
                        Id = Guid.NewGuid(),
                        Name = friendlyName,
                        Host = defaultHost,
                        Port = result.Port,
                        MountPath = mountPath,
                        ServerType = ServerType.Auto,
                        UseDefaultPassword = true,
                        CustomPassword = string.Empty,
                        Enabled = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    result.Channels.Add(channel);
                }
            }

            // Sort alphabetically by mount path
            result.Channels = result.Channels.OrderBy(c => c.MountPath).ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error parsing TinyIce config: {ex.Message}");
        }

        return result;
    }

    public static string FormatFriendlyChannelName(string mountPath)
    {
        var clean = mountPath.TrimStart('/').Replace(".mp3", "").Replace(".aac", "").Replace(".ogg", "").Replace(".flac", "");
        if (string.IsNullOrWhiteSpace(clean)) return "Main Stream";

        // e.g. "cazz-flac" -> "Cazz (FLAC)", "radio-high" -> "Radio (High)", "lofi" -> "Lofi Stream"
        var parts = clean.Split(new[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
        var formattedParts = parts.Select(p =>
        {
            var upper = p.ToUpperInvariant();
            if (upper is "FLAC" or "AAC" or "MP3" or "OGG" or "OPUS" or "WAV" or "AI" or "EN" or "FR")
            {
                return $"({upper})";
            }
            if (upper is "HIGH" or "LOW" or "NORMAL" or "320K" or "128K" or "64K" or "32K")
            {
                return $"({char.ToUpper(p[0]) + p[1..].ToLowerInvariant()})";
            }
            return char.ToUpper(p[0]) + p[1..].ToLowerInvariant();
        });

        return string.Join(" ", formattedParts);
    }
}
