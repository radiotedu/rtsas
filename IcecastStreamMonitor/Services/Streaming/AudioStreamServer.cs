using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using IcecastStreamMonitor.Models;

namespace IcecastStreamMonitor.Services.Streaming;

public class AudioStreamServer
{
    private TcpListener? _listener;
    private CancellationTokenSource? _cts;
    private readonly ConcurrentDictionary<string, MountPointStream> _mounts = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _trustedProxies = new(StringComparer.OrdinalIgnoreCase) { "127.0.0.1", "::1", "localhost" };

    public int Port { get; private set; } = 11154;
    public string DefaultSourcePassword { get; set; } = "hackme";
    public string Hostname { get; set; } = "stream.radiotedu.com";
    public string AdminUser { get; set; } = "radiotedu";
    public string AdminPassword { get; set; } = "hackme";
    public bool IsRunning { get; private set; }

    public event Action<string, string>? OnSourceConnected;
    public event Action<string>? OnSourceDisconnected;
    public event Action<string, string>? OnClientConnected;
    public event Action<string, string>? OnClientDisconnected;

    public void SetTrustedProxies(IEnumerable<string> proxies)
    {
        _trustedProxies.Clear();
        foreach (var p in proxies)
        {
            if (!string.IsNullOrWhiteSpace(p))
            {
                _trustedProxies.Add(p.Trim());
            }
        }
    }

    public MountPointStream EnsureMount(string mountPath, string password = "", bool enabled = true)
    {
        var cleanPath = NormalizeMountPath(mountPath);
        return _mounts.GetOrAdd(cleanPath, path => new MountPointStream(path)
        {
            Password = password,
            Enabled = enabled
        });
    }

    public bool RemoveMount(string mountPath)
    {
        var cleanPath = NormalizeMountPath(mountPath);
        if (_mounts.TryRemove(cleanPath, out var mount))
        {
            mount.SetSourceDisconnected();
            return true;
        }
        return false;
    }

    public MountPointStream? GetMount(string mountPath)
    {
        var cleanPath = NormalizeMountPath(mountPath);
        _mounts.TryGetValue(cleanPath, out var mount);
        return mount;
    }

    public IEnumerable<MountPointStream> GetAllMounts() => _mounts.Values;

    public void Start(int port = 11154)
    {
        if (IsRunning) return;

        Port = port;
        _cts = new CancellationTokenSource();
        _listener = new TcpListener(IPAddress.Any, Port);
        _listener.Start(500);
        IsRunning = true;

        Task.Run(() => AcceptConnectionsLoopAsync(_cts.Token));
    }

    public void Stop()
    {
        if (!IsRunning) return;

        IsRunning = false;
        try { _cts?.Cancel(); } catch { }
        try { _listener?.Stop(); } catch { }

        foreach (var mount in _mounts.Values)
        {
            mount.SetSourceDisconnected();
        }
    }

    private async Task AcceptConnectionsLoopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested && _listener != null)
        {
            try
            {
                var client = await _listener.AcceptTcpClientAsync(ct);
                _ = Task.Run(() => HandleClientConnectionAsync(client, ct), ct);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch
            {
                if (!IsRunning) break;
            }
        }
    }

    private async Task HandleClientConnectionAsync(TcpClient client, CancellationToken ct)
    {
        client.NoDelay = true;
        using var stream = client.GetStream();

        try
        {
            // Read HTTP request header lines
            var reader = new StreamReader(stream, Encoding.Latin1, leaveOpen: true);
            var requestLine = await reader.ReadLineAsync(ct);
            if (string.IsNullOrWhiteSpace(requestLine)) return;

            var parts = requestLine.Split(' ', 3);
            if (parts.Length < 2) return;

            var method = parts[0].ToUpperInvariant();
            var rawUri = parts[1];

            // Read all headers
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string? line;
            while (!string.IsNullOrEmpty(line = await reader.ReadLineAsync(ct)))
            {
                var colonPos = line.IndexOf(':');
                if (colonPos > 0)
                {
                    var key = line.Substring(0, colonPos).Trim();
                    var val = line.Substring(colonPos + 1).Trim();
                    headers[key] = val;
                }
            }

            // Determine actual client IP (honor X-Forwarded-For or X-Real-IP if from trusted proxy)
            var remoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
            var socketIp = remoteEndPoint?.Address.ToString() ?? "127.0.0.1";
            var clientIp = ResolveClientIp(socketIp, headers);
            var userAgent = headers.TryGetValue("User-Agent", out var ua) ? ua : "Generic Player";

            // 1. DJ Audio Ingest (SOURCE or PUT)
            if (method == "SOURCE" || method == "PUT")
            {
                await HandleSourceIngestAsync(rawUri, headers, stream, clientIp, userAgent, ct);
                return;
            }

            // 2. Icecast 2 Compatible Status JSON (/status-json.xsl)
            if (rawUri.StartsWith("/status-json.xsl", StringComparison.OrdinalIgnoreCase) ||
                rawUri.StartsWith("/stats.json", StringComparison.OrdinalIgnoreCase))
            {
                await SendJsonStatsResponseAsync(stream);
                return;
            }

            // 3. Icecast 2 Compatible Admin XML Stats (/admin/stats.xml, /stats)
            if (rawUri.StartsWith("/admin/stats.xml", StringComparison.OrdinalIgnoreCase) ||
                rawUri.StartsWith("/stats", StringComparison.OrdinalIgnoreCase))
            {
                await SendXmlStatsResponseAsync(stream);
                return;
            }

            // 4. Admin Dynamic Metadata Update (/admin/metadata)
            if (rawUri.StartsWith("/admin/metadata", StringComparison.OrdinalIgnoreCase))
            {
                await HandleMetadataUpdateAsync(rawUri, headers, stream);
                return;
            }

            // 5. Admin Connected Listener List (/admin/listclients)
            if (rawUri.StartsWith("/admin/listclients", StringComparison.OrdinalIgnoreCase))
            {
                await HandleListClientsAsync(rawUri, stream);
                return;
            }

            // 6. Admin Kill / Kick Client (/admin/killclient)
            if (rawUri.StartsWith("/admin/killclient", StringComparison.OrdinalIgnoreCase))
            {
                await HandleKillClientAsync(rawUri, stream);
                return;
            }

            // 7. Listener Audio Playback (GET /mount or GET /mount.mp3)
            if (method == "GET" || method == "HEAD")
            {
                await HandleListenerPlaybackAsync(rawUri, headers, stream, clientIp, userAgent, method == "HEAD", ct);
                return;
            }

            // Fallback: 404
            await SendHttpResponseAsync(stream, 404, "Not Found", "text/plain", "404 Endpoint Not Found\r\n");
        }
        catch
        {
            // Connection closed
        }
        finally
        {
            try { client.Close(); } catch { }
        }
    }

    private string ResolveClientIp(string socketIp, Dictionary<string, string> headers)
    {
        if (_trustedProxies.Contains(socketIp))
        {
            if (headers.TryGetValue("X-Forwarded-For", out var xff) && !string.IsNullOrWhiteSpace(xff))
            {
                var firstIp = xff.Split(',')[0].Trim();
                if (!string.IsNullOrEmpty(firstIp)) return firstIp;
            }
            if (headers.TryGetValue("X-Real-IP", out var xrip) && !string.IsNullOrWhiteSpace(xrip))
            {
                return xrip.Trim();
            }
        }
        return socketIp;
    }

    private async Task HandleSourceIngestAsync(string rawUri, Dictionary<string, string> headers, NetworkStream stream, string clientIp, string userAgent, CancellationToken ct)
    {
        var mountPath = ExtractMountPath(rawUri);
        var mount = EnsureMount(mountPath);

        // Authenticate DJ Source
        if (!AuthenticateSource(headers, mount))
        {
            await SendHttpResponseAsync(stream, 401, "Unauthorized", "text/plain", "401 Unauthorized: Invalid Source Password\r\n");
            return;
        }

        // Extract broadcast parameters
        var contentType = headers.TryGetValue("Content-Type", out var ctHeader) ? ctHeader : "audio/mpeg";
        int bitrate = 320;
        if (headers.TryGetValue("ice-bitrate", out var brStr) && int.TryParse(brStr, out var br)) bitrate = br;
        else if (headers.TryGetValue("icy-br", out var icyBr) && int.TryParse(icyBr, out var ibr)) bitrate = ibr;

        var streamName = headers.TryGetValue("ice-name", out var iname) ? iname : (headers.TryGetValue("icy-name", out var yname) ? yname : mount.StreamName);
        var streamGenre = headers.TryGetValue("ice-genre", out var igenre) ? igenre : (headers.TryGetValue("icy-genre", out var ygenre) ? ygenre : mount.StreamGenre);
        var streamDesc = headers.TryGetValue("ice-description", out var idesc) ? idesc : mount.StreamDescription;

        mount.StreamDescription = streamDesc;
        mount.SetSourceConnected(clientIp, userAgent, contentType, bitrate, streamName, streamGenre);
        OnSourceConnected?.Invoke(mountPath, clientIp);

        // Send Icecast 2 handshake OK
        var okResponse = Encoding.Latin1.GetBytes("HTTP/1.0 200 OK\r\nServer: RTSAS/1.0\r\nConnection: Keep-Alive\r\n\r\n");
        await stream.WriteAsync(okResponse, 0, okResponse.Length, ct);

        // Read audio payload chunks continuously
        var buffer = new byte[16384];
        try
        {
            while (!ct.IsCancellationRequested)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, ct);
                if (bytesRead <= 0) break;

                mount.WriteAudioChunk(buffer, 0, bytesRead);
            }
        }
        catch
        {
            // DJ dropped
        }
        finally
        {
            mount.SetSourceDisconnected();
            OnSourceDisconnected?.Invoke(mountPath);
        }
    }

    private bool AuthenticateSource(Dictionary<string, string> headers, MountPointStream mount)
    {
        var targetPass = !string.IsNullOrEmpty(mount.Password) ? mount.Password : DefaultSourcePassword;

        // Check ice-password header
        if (headers.TryGetValue("ice-password", out var icePass) && icePass == targetPass)
        {
            return true;
        }

        // Check Basic Authorization header
        if (headers.TryGetValue("Authorization", out var auth) && auth.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                var base64 = auth.Substring(6).Trim();
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                var parts = credentials.Split(':', 2);
                var pass = parts.Length > 1 ? parts[1] : parts[0];
                if (pass == targetPass || pass == AdminPassword)
                {
                    return true;
                }
            }
            catch { }
        }

        return false;
    }

    private async Task HandleListenerPlaybackAsync(string rawUri, Dictionary<string, string> headers, NetworkStream stream, string clientIp, string userAgent, bool isHead, CancellationToken ct)
    {
        var mountPath = ExtractMountPath(rawUri);
        var mount = GetMount(mountPath);

        if (mount == null || !mount.IsOnline)
        {
            await SendHttpResponseAsync(stream, 404, "Mount Not Found", "text/plain", "404 Mount Offline or Not Found\r\n");
            return;
        }

        bool supportsIcy = headers.TryGetValue("Icy-MetaData", out var icyVal) && icyVal == "1";

        var sb = new StringBuilder();
        sb.Append("HTTP/1.0 200 OK\r\n");
        sb.Append($"Content-Type: {mount.ContentType}\r\n");
        sb.Append("Server: RTSAS/1.0 (Icecast 2 Compatible)\r\n");
        sb.Append("Connection: close\r\n");
        sb.Append("Pragma: no-cache\r\n");
        sb.Append("Cache-Control: no-cache, no-store, must-revalidate\r\n");
        sb.Append("Expires: Mon, 26 Jul 1997 05:00:00 GMT\r\n");
        sb.Append("Access-Control-Allow-Origin: *\r\n");
        sb.Append("Access-Control-Allow-Methods: GET, HEAD, OPTIONS\r\n");
        sb.Append($"icy-name: {mount.StreamName}\r\n");
        sb.Append($"icy-genre: {mount.StreamGenre}\r\n");
        sb.Append($"icy-br: {mount.BitRateKbps}\r\n");
        sb.Append($"icy-description: {mount.StreamDescription}\r\n");
        sb.Append("icy-pub: 1\r\n");

        if (supportsIcy)
        {
            sb.Append($"icy-metaint: {IcyMetadataInjector.DefaultMetaInterval}\r\n");
        }
        sb.Append("\r\n");

        var headerBytes = Encoding.Latin1.GetBytes(sb.ToString());
        await stream.WriteAsync(headerBytes, 0, headerBytes.Length, ct);

        if (isHead) return;

        var client = mount.RegisterClient(clientIp, userAgent, supportsIcy, stream);
        OnClientConnected?.Invoke(mountPath, clientIp);

        // Keep connection open and wait until disconnected or cancelled
        try
        {
            await Task.Delay(Timeout.Infinite, client.Cts.Token);
        }
        catch
        {
            // Disconnected
        }
        finally
        {
            mount.RemoveClient(client.Id);
            OnClientDisconnected?.Invoke(mountPath, clientIp);
        }
    }

    private async Task SendJsonStatsResponseAsync(NetworkStream stream)
    {
        var sources = _mounts.Values.Where(m => m.IsOnline).Select(m => new
        {
            listenurl = $"https://{Hostname}{m.MountPath}",
            listeners = m.ConcurrentListeners,
            listener_peak = m.PeakListeners,
            bitrate = m.BitRateKbps,
            title = m.StreamTitle,
            genre = m.StreamGenre,
            server_name = m.StreamName,
            server_description = m.StreamDescription,
            server_type = m.ContentType,
            stream_start = m.SourceConnectedAt?.ToString("s") ?? "",
            total_bytes = m.TotalBytesTransferred
        }).ToList();

        var jsonObj = new
        {
            icestats = new
            {
                admin = $"{AdminUser}@{Hostname}",
                host = Hostname,
                location = "RadioTEDU Cloud",
                server_id = "RTSAS/1.0 (Icecast 2.4.4 Compatible)",
                server_start = DateTime.Now.ToString("s"),
                source = sources.Count == 1 ? (object)sources[0] : sources
            }
        };

        var json = JsonSerializer.Serialize(jsonObj, new JsonSerializerOptions { WriteIndented = true });
        await SendHttpResponseAsync(stream, 200, "OK", "application/json", json);
    }

    private async Task SendXmlStatsResponseAsync(NetworkStream stream)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<icestats>");
        sb.AppendLine($"  <admin>{AdminUser}@{Hostname}</admin>");
        sb.AppendLine($"  <host>{Hostname}</host>");
        sb.AppendLine($"  <location>RadioTEDU Cloud</location>");
        sb.AppendLine($"  <server_id>RTSAS/1.0 (Icecast 2.4.4)</server_id>");

        foreach (var m in _mounts.Values.Where(m => m.IsOnline))
        {
            sb.AppendLine($"  <source mount=\"{m.MountPath}\">");
            sb.AppendLine($"    <listeners>{m.ConcurrentListeners}</listeners>");
            sb.AppendLine($"    <listener_peak>{m.PeakListeners}</listener_peak>");
            sb.AppendLine($"    <bitrate>{m.BitRateKbps}</bitrate>");
            sb.AppendLine($"    <title>{System.Security.SecurityElement.Escape(m.StreamTitle)}</title>");
            sb.AppendLine($"    <genre>{System.Security.SecurityElement.Escape(m.StreamGenre)}</genre>");
            sb.AppendLine($"    <server_name>{System.Security.SecurityElement.Escape(m.StreamName)}</server_name>");
            sb.AppendLine($"    <server_type>{m.ContentType}</server_type>");
            sb.AppendLine($"  </source>");
        }

        sb.AppendLine("</icestats>");
        await SendHttpResponseAsync(stream, 200, "OK", "application/xml", sb.ToString());
    }

    private async Task HandleMetadataUpdateAsync(string rawUri, Dictionary<string, string> headers, NetworkStream stream)
    {
        // /admin/metadata?mount=/lofi&mode=updinfo&song=Artist%20-%20Title
        var query = ParseQueryString(rawUri);
        if (query.TryGetValue("mount", out var mountPath) && query.TryGetValue("song", out var song))
        {
            var mount = GetMount(mountPath);
            if (mount != null)
            {
                mount.UpdateTitle(song);
                await SendHttpResponseAsync(stream, 200, "OK", "text/plain", "Metadata updated successfully\r\n");
                return;
            }
        }

        await SendHttpResponseAsync(stream, 400, "Bad Request", "text/plain", "Missing mount or song parameter\r\n");
    }

    private async Task HandleListClientsAsync(string rawUri, NetworkStream stream)
    {
        var query = ParseQueryString(rawUri);
        if (query.TryGetValue("mount", out var mountPath))
        {
            var mount = GetMount(mountPath);
            if (mount != null)
            {
                var listeners = mount.GetConnectedListeners();
                var json = JsonSerializer.Serialize(listeners, new JsonSerializerOptions { WriteIndented = true });
                await SendHttpResponseAsync(stream, 200, "OK", "application/json", json);
                return;
            }
        }

        await SendHttpResponseAsync(stream, 404, "Not Found", "text/plain", "Mount not found\r\n");
    }

    private async Task HandleKillClientAsync(string rawUri, NetworkStream stream)
    {
        var query = ParseQueryString(rawUri);
        if (query.TryGetValue("id", out var idStr) && long.TryParse(idStr, out var clientId))
        {
            foreach (var mount in _mounts.Values)
            {
                if (mount.KickClient(clientId))
                {
                    await SendHttpResponseAsync(stream, 200, "OK", "text/plain", "Client disconnected\r\n");
                    return;
                }
            }
        }

        await SendHttpResponseAsync(stream, 404, "Not Found", "text/plain", "Client ID not found\r\n");
    }

    private static async Task SendHttpResponseAsync(NetworkStream stream, int statusCode, string reason, string contentType, string body)
    {
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        var headers = $"HTTP/1.1 {statusCode} {reason}\r\n" +
                      $"Content-Type: {contentType}; charset=utf-8\r\n" +
                      $"Content-Length: {bodyBytes.Length}\r\n" +
                      "Access-Control-Allow-Origin: *\r\n" +
                      "Connection: close\r\n\r\n";

        var headerBytes = Encoding.Latin1.GetBytes(headers);
        await stream.WriteAsync(headerBytes, 0, headerBytes.Length);
        await stream.WriteAsync(bodyBytes, 0, bodyBytes.Length);
    }

    private static string NormalizeMountPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return "/live";
        var clean = path.Trim().Split('?')[0];
        return clean.StartsWith('/') ? clean : "/" + clean;
    }

    private static string ExtractMountPath(string rawUri)
    {
        var path = rawUri.Split('?')[0];
        // Strip .mp3/.ogg/.aac extensions if requested as stream aliases
        if (path.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".aac", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".flac", StringComparison.OrdinalIgnoreCase))
        {
            int dot = path.LastIndexOf('.');
            var subPath = path.Substring(0, dot);
            return NormalizeMountPath(subPath);
        }
        return NormalizeMountPath(path);
    }

    private static Dictionary<string, string> ParseQueryString(string rawUri)
    {
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        int q = rawUri.IndexOf('?');
        if (q < 0 || q >= rawUri.Length - 1) return dict;

        var query = rawUri.Substring(q + 1);
        var pairs = query.Split('&');
        foreach (var pair in pairs)
        {
            var kv = pair.Split('=', 2);
            if (kv.Length == 2)
            {
                dict[Uri.UnescapeDataString(kv[0])] = Uri.UnescapeDataString(kv[1]);
            }
        }
        return dict;
    }
}
