using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using IcecastStreamMonitor.Models;

namespace IcecastStreamMonitor.Services;

public class StreamMonitorService : IStreamMonitorService, IDisposable
{
    private readonly HttpClient _httpClient;
    public Streaming.AudioStreamServer? LocalServer { get; set; }

    public StreamMonitorService(HttpClient? httpClient = null)
    {
        if (httpClient != null)
        {
            _httpClient = httpClient;
        }
        else
        {
            var handler = new SocketsHttpHandler
            {
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 3,
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                PooledConnectionIdleTimeout = TimeSpan.FromSeconds(30),
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("IcecastStreamMonitor/1.0 (Windows; classic)");
        }
    }

    public async Task<StreamStatus> CheckStreamAsync(
        StreamChannel channel,
        string defaultPassword,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        // 1. Direct in-memory check if matching local embedded streaming server
        if (LocalServer != null && LocalServer.IsRunning &&
            (channel.Host == "127.0.0.1" || channel.Host == "localhost" || channel.Host == "::1" || channel.Port == LocalServer.Port))
        {
            var mount = LocalServer.GetMount(channel.MountPath);
            if (mount != null)
            {
                return new StreamStatus
                {
                    ChannelId = channel.Id,
                    IsOnline = mount.IsOnline,
                    ConcurrentListeners = mount.ConcurrentListeners,
                    PeakListeners = mount.PeakListeners,
                    BitRateKbps = mount.BitRateKbps,
                    AudioFormat = AudioFormatHelper.NormalizeAudioFormat(mount.ContentType),
                    RawContentType = mount.ContentType,
                    StreamTitle = mount.StreamTitle,
                    ServerDescription = mount.StreamDescription,
                    LatencyMs = 0,
                    StatusMessage = mount.IsOnline ? "Online (Live Ingest)" : "Offline (Awaiting DJ Source)",
                    LastChecked = DateTime.Now
                };
            }
        }

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(timeout);

        var stopwatch = Stopwatch.StartNew();
        var effectivePassword = channel.UseDefaultPassword ? defaultPassword : channel.CustomPassword;

        try
        {
            switch (channel.ServerType)
            {
                case ServerType.Icecast:
                    return await QueryIcecastAsync(channel, effectivePassword, stopwatch, cts.Token);

                case ServerType.ShoutcastV1:
                    return await QueryShoutcastV1Async(channel, effectivePassword, stopwatch, cts.Token);

                case ServerType.ShoutcastV2:
                    return await QueryShoutcastV2Async(channel, effectivePassword, stopwatch, cts.Token);

                case ServerType.DirectStream:
                    return await ProbeDirectStreamAsync(channel, stopwatch, cts.Token);

                case ServerType.Auto:
                default:
                    // Try Icecast first -> SHOUTcast v2 -> SHOUTcast v1 -> Direct ICY probe
                    var status = await QueryIcecastAsync(channel, effectivePassword, stopwatch, cts.Token);
                    if (status.IsOnline) return status;

                    status = await QueryShoutcastV2Async(channel, effectivePassword, stopwatch, cts.Token);
                    if (status.IsOnline) return status;

                    status = await QueryShoutcastV1Async(channel, effectivePassword, stopwatch, cts.Token);
                    if (status.IsOnline) return status;

                    return await ProbeDirectStreamAsync(channel, stopwatch, cts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            stopwatch.Stop();
            return StreamStatus.Offline(channel.Id, "Request timed out", stopwatch.ElapsedMilliseconds);
        }
        catch (HttpRequestException ex)
        {
            stopwatch.Stop();
            var msg = ex.StatusCode.HasValue ? $"HTTP {(int)ex.StatusCode.Value} {ex.StatusCode.Value}" : ex.Message;
            return StreamStatus.Offline(channel.Id, msg, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return StreamStatus.Offline(channel.Id, ex.Message, stopwatch.ElapsedMilliseconds);
        }
    }

    public async Task<Dictionary<Guid, StreamStatus>> CheckAllStreamsAsync(
        IEnumerable<StreamChannel> channels,
        string defaultPassword,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        var channelList = channels.Where(c => c.Enabled).ToList();
        var results = new Dictionary<Guid, StreamStatus>();

        using var semaphore = new SemaphoreSlim(16); // Max 16 concurrent requests for high performance
        var tasks = channelList.Select(async channel =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                var status = await CheckStreamAsync(channel, defaultPassword, timeout, cancellationToken);
                return (channel.Id, status);
            }
            finally
            {
                semaphore.Release();
            }
        });

        var completed = await Task.WhenAll(tasks);
        foreach (var (id, status) in completed)
        {
            results[id] = status;
        }

        return results;
    }

    public async Task<StreamStatus> QueryIcecastAsync(
        StreamChannel channel,
        string password,
        Stopwatch stopwatch,
        CancellationToken ct)
    {
        var cleanMount = NormalizeMount(channel.MountPath);
        var host = string.IsNullOrWhiteSpace(channel.Host) ? "127.0.0.1" : channel.Host.Trim();

        // 1. Try public JSON status: http://host:port/status-json.xsl
        var jsonUrl = $"http://{host}:{channel.Port}/status-json.xsl";
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, jsonUrl);
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, ct);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(ct);
                var parsed = ParseIcecastJson(json, cleanMount, channel.Id, stopwatch.ElapsedMilliseconds);
                if (parsed != null && parsed.IsOnline)
                    return parsed;
            }
        }
        catch { /* fallback to admin XML */ }

        // 2. Try Admin XML stats: http://host:port/admin/stats.xml
        if (!string.IsNullOrEmpty(password))
        {
            var adminXmlUrl = $"http://{host}:{channel.Port}/admin/stats.xml";
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, adminXmlUrl);
                var authBytes = Encoding.UTF8.GetBytes($"admin:{password}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

                using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, ct);
                if (response.IsSuccessStatusCode)
                {
                    var xml = await response.Content.ReadAsStringAsync(ct);
                    var parsed = ParseIcecastAdminXml(xml, cleanMount, channel.Id, stopwatch.ElapsedMilliseconds);
                    if (parsed != null && parsed.IsOnline)
                        return parsed;
                }
            }
            catch { /* fallback to direct stream probe */ }
        }

        // 3. Fallback to direct stream header probe
        return await ProbeDirectStreamAsync(channel, stopwatch, ct);
    }

    public static StreamStatus? ParseIcecastJson(string json, string targetMount, Guid channelId, long latencyMs)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (!root.TryGetProperty("icestats", out var icestats))
                return null;

            if (!icestats.TryGetProperty("source", out var sourceProp))
                return null;

            var sources = new List<JsonElement>();
            if (sourceProp.ValueKind == JsonValueKind.Array)
            {
                foreach (var el in sourceProp.EnumerateArray())
                    sources.Add(el);
            }
            else if (sourceProp.ValueKind == JsonValueKind.Object)
            {
                sources.Add(sourceProp);
            }

            foreach (var src in sources)
            {
                string? listenUrl = GetJsonString(src, "listenurl");
                string? mount = GetJsonString(src, "mount");

                bool isMatch = false;
                if (!string.IsNullOrEmpty(mount) && NormalizeMount(mount).Equals(targetMount, StringComparison.OrdinalIgnoreCase))
                {
                    isMatch = true;
                }
                else if (!string.IsNullOrEmpty(listenUrl) && (listenUrl.EndsWith(targetMount, StringComparison.OrdinalIgnoreCase) || listenUrl.EndsWith("/" + targetMount.TrimStart('/'), StringComparison.OrdinalIgnoreCase)))
                {
                    isMatch = true;
                }
                else if (sources.Count == 1 && (string.IsNullOrEmpty(targetMount) || targetMount == "/"))
                {
                    isMatch = true;
                }

                if (isMatch)
                {
                    var listeners = GetJsonInt(src, "listeners");
                    var peak = GetJsonInt(src, "listener_peak");
                    var max = GetJsonInt(src, "max_listeners");
                    var serverType = GetJsonString(src, "server_type");
                    var audioInfo = GetJsonString(src, "audio_info");
                    var title = GetJsonString(src, "title") ?? GetJsonString(src, "server_name") ?? string.Empty;
                    var desc = GetJsonString(src, "server_description") ?? string.Empty;

                    var (aiBitrate, aiSampleRate, aiChannels) = AudioFormatHelper.ParseAudioInfo(audioInfo);
                    var directBitrate = GetJsonInt(src, "bitrate");
                    var bitrate = directBitrate > 0 ? directBitrate : aiBitrate;

                    var format = AudioFormatHelper.NormalizeAudioFormat(serverType, audioInfo);

                    return new StreamStatus
                    {
                        ChannelId = channelId,
                        IsOnline = true,
                        ConcurrentListeners = listeners,
                        PeakListeners = peak,
                        MaxListeners = max,
                        AudioFormat = format,
                        BitRateKbps = bitrate,
                        SampleRateHz = aiSampleRate,
                        Channels = aiChannels,
                        StreamTitle = title,
                        ServerDescription = desc,
                        LatencyMs = latencyMs,
                        LastChecked = DateTime.Now,
                        StatusMessage = $"Online ({latencyMs}ms)",
                        RawContentType = serverType ?? string.Empty
                    };
                }
            }
        }
        catch { }

        return null;
    }

    public static StreamStatus? ParseIcecastAdminXml(string xml, string targetMount, Guid channelId, long latencyMs)
    {
        try
        {
            var doc = XDocument.Parse(xml);
            var sources = doc.Descendants("source");

            foreach (var src in sources)
            {
                var mountAttr = src.Attribute("mount")?.Value;
                if (mountAttr != null && NormalizeMount(mountAttr).Equals(targetMount, StringComparison.OrdinalIgnoreCase))
                {
                    int listeners = int.TryParse(src.Element("listeners")?.Value, out var l) ? l : 0;
                    int peak = int.TryParse(src.Element("listener_peak")?.Value, out var p) ? p : 0;
                    int max = int.TryParse(src.Element("max_listeners")?.Value, out var m) ? m : 0;
                    var serverType = src.Element("server_type")?.Value;
                    var audioInfo = src.Element("audio_info")?.Value;
                    var title = src.Element("title")?.Value ?? src.Element("server_name")?.Value ?? string.Empty;
                    var desc = src.Element("server_description")?.Value ?? string.Empty;

                    var (aiBitrate, aiSampleRate, aiChannels) = AudioFormatHelper.ParseAudioInfo(audioInfo);
                    int directBitrate = int.TryParse(src.Element("bitrate")?.Value, out var br) ? br : 0;
                    int bitrate = directBitrate > 0 ? directBitrate : aiBitrate;

                    var format = AudioFormatHelper.NormalizeAudioFormat(serverType, audioInfo);

                    return new StreamStatus
                    {
                        ChannelId = channelId,
                        IsOnline = true,
                        ConcurrentListeners = listeners,
                        PeakListeners = peak,
                        MaxListeners = max,
                        AudioFormat = format,
                        BitRateKbps = bitrate,
                        SampleRateHz = aiSampleRate,
                        Channels = aiChannels,
                        StreamTitle = title,
                        ServerDescription = desc,
                        LatencyMs = latencyMs,
                        LastChecked = DateTime.Now,
                        StatusMessage = $"Online ({latencyMs}ms)",
                        RawContentType = serverType ?? string.Empty
                    };
                }
            }
        }
        catch { }

        return null;
    }

    public async Task<StreamStatus> QueryShoutcastV2Async(
        StreamChannel channel,
        string password,
        Stopwatch stopwatch,
        CancellationToken ct)
    {
        var host = string.IsNullOrWhiteSpace(channel.Host) ? "127.0.0.1" : channel.Host.Trim();
        var sid = ExtractSid(channel.MountPath);
        var statsUrl = $"http://{host}:{channel.Port}/stats?sid={sid}&json=1";
        if (!string.IsNullOrEmpty(password))
        {
            statsUrl += $"&pass={Uri.EscapeDataString(password)}";
        }

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, statsUrl);
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, ct);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(ct);
                var parsed = ParseShoutcastV2Json(json, channel.Id, stopwatch.ElapsedMilliseconds);
                if (parsed != null && parsed.IsOnline)
                    return parsed;
            }
        }
        catch { /* fallback to probe */ }

        return await ProbeDirectStreamAsync(channel, stopwatch, ct);
    }

    public static StreamStatus? ParseShoutcastV2Json(string json, Guid channelId, long latencyMs)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            int streamStatus = GetJsonInt(root, "streamstatus");
            if (streamStatus == 0 && root.TryGetProperty("streamstatus", out _))
            {
                return StreamStatus.Offline(channelId, "SHOUTcast stream inactive", latencyMs);
            }

            int listeners = GetJsonInt(root, "currentlisteners");
            int peak = GetJsonInt(root, "peaklisteners");
            int max = GetJsonInt(root, "maxlisteners");
            int bitrate = GetJsonInt(root, "bitrate");
            int samplerate = GetJsonInt(root, "samprate");
            string? songtitle = GetJsonString(root, "songtitle");
            string? servertitle = GetJsonString(root, "servertitle");
            string? content = GetJsonString(root, "content");

            var format = AudioFormatHelper.NormalizeAudioFormat(content);

            return new StreamStatus
            {
                ChannelId = channelId,
                IsOnline = true,
                ConcurrentListeners = listeners,
                PeakListeners = peak,
                MaxListeners = max,
                AudioFormat = format,
                BitRateKbps = bitrate,
                SampleRateHz = samplerate,
                StreamTitle = songtitle ?? string.Empty,
                ServerDescription = servertitle ?? string.Empty,
                LatencyMs = latencyMs,
                LastChecked = DateTime.Now,
                StatusMessage = $"Online ({latencyMs}ms)",
                RawContentType = content ?? string.Empty
            };
        }
        catch { }

        return null;
    }

    public async Task<StreamStatus> QueryShoutcastV1Async(
        StreamChannel channel,
        string password,
        Stopwatch stopwatch,
        CancellationToken ct)
    {
        var host = string.IsNullOrWhiteSpace(channel.Host) ? "127.0.0.1" : channel.Host.Trim();
        var url = $"http://{host}:{channel.Port}/7.html";

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, ct);

            if (response.IsSuccessStatusCode)
            {
                var html = await response.Content.ReadAsStringAsync(ct);
                var parsed = ParseShoutcastV1Html(html, channel.Id, stopwatch.ElapsedMilliseconds);
                if (parsed != null)
                    return parsed;
            }
        }
        catch { /* fallback to probe */ }

        return await ProbeDirectStreamAsync(channel, stopwatch, ct);
    }

    public static StreamStatus? ParseShoutcastV1Html(string html, Guid channelId, long latencyMs)
    {
        try
        {
            // Expected body: <html><body>15,1,42,100,14,128,Artist - Song</body></html>
            int bodyStart = html.IndexOf("<body>", StringComparison.OrdinalIgnoreCase);
            int bodyEnd = html.IndexOf("</body>", StringComparison.OrdinalIgnoreCase);

            string content;
            if (bodyStart >= 0 && bodyEnd > bodyStart)
            {
                content = html.Substring(bodyStart + 6, bodyEnd - (bodyStart + 6)).Trim();
            }
            else
            {
                content = html.Trim();
            }

            var parts = content.Split(',', 7);
            if (parts.Length >= 6)
            {
                int currentListeners = int.TryParse(parts[0], out var cl) ? cl : 0;
                int streamStatus = int.TryParse(parts[1], out var ss) ? ss : 0;
                int peakListeners = int.TryParse(parts[2], out var pl) ? pl : 0;
                int maxListeners = int.TryParse(parts[3], out var ml) ? ml : 0;
                int bitrate = int.TryParse(parts[5], out var br) ? br : 0;
                string songTitle = parts.Length > 6 ? parts[6] : string.Empty;

                if (streamStatus == 0)
                {
                    return StreamStatus.Offline(channelId, "Stream offline", latencyMs);
                }

                return new StreamStatus
                {
                    ChannelId = channelId,
                    IsOnline = true,
                    ConcurrentListeners = currentListeners,
                    PeakListeners = peakListeners,
                    MaxListeners = maxListeners,
                    AudioFormat = "MP3",
                    BitRateKbps = bitrate,
                    StreamTitle = songTitle,
                    LatencyMs = latencyMs,
                    LastChecked = DateTime.Now,
                    StatusMessage = $"Online ({latencyMs}ms)"
                };
            }
        }
        catch { }

        return null;
    }

    public async Task<StreamStatus> ProbeDirectStreamAsync(
        StreamChannel channel,
        Stopwatch stopwatch,
        CancellationToken ct)
    {
        var url = channel.FormattedUrl;

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Icy-MetaData", "1");
            request.Headers.Range = new RangeHeaderValue(0, 1024);

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

            stopwatch.Stop();
            var latency = stopwatch.ElapsedMilliseconds;

            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.PartialContent)
            {
                var contentType = response.Content.Headers.ContentType?.MediaType ?? string.Empty;
                var icyBrHeader = GetHeaderValue(response, "icy-br");
                var icyName = GetHeaderValue(response, "icy-name");
                var icyGenre = GetHeaderValue(response, "icy-genre");

                int bitrate = AudioFormatHelper.ParseBitrateString(icyBrHeader);
                var format = AudioFormatHelper.NormalizeAudioFormat(contentType);

                return new StreamStatus
                {
                    ChannelId = channel.Id,
                    IsOnline = true,
                    ConcurrentListeners = 0, // Not exposed in ICY stream headers
                    AudioFormat = format,
                    BitRateKbps = bitrate,
                    StreamTitle = icyName ?? channel.Name,
                    ServerDescription = icyGenre ?? string.Empty,
                    LatencyMs = latency,
                    LastChecked = DateTime.Now,
                    StatusMessage = $"Online ({latency}ms)",
                    RawContentType = contentType
                };
            }
            else
            {
                return StreamStatus.Offline(channel.Id, $"HTTP {(int)response.StatusCode} {response.StatusCode}", latency);
            }
        }
        catch (OperationCanceledException)
        {
            return StreamStatus.Offline(channel.Id, "Connection timed out", stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            return StreamStatus.Offline(channel.Id, ex.Message, stopwatch.ElapsedMilliseconds);
        }
    }

    private static string? GetHeaderValue(HttpResponseMessage response, string headerName)
    {
        if (response.Headers.TryGetValues(headerName, out var values))
            return values.FirstOrDefault();
        if (response.Content.Headers.TryGetValues(headerName, out var cValues))
            return cValues.FirstOrDefault();
        return null;
    }

    private static string NormalizeMount(string mount)
    {
        if (string.IsNullOrWhiteSpace(mount)) return "/";
        var trimmed = mount.Trim();
        return trimmed.StartsWith('/') ? trimmed : "/" + trimmed;
    }

    private static int ExtractSid(string mount)
    {
        var cleaned = mount.Trim().TrimStart('/');
        if (int.TryParse(cleaned, out var sid))
            return sid;
        return 1; // Default SHOUTcast stream ID
    }

    private static string? GetJsonString(JsonElement element, string propName)
    {
        if (element.TryGetProperty(propName, out var prop))
        {
            if (prop.ValueKind == JsonValueKind.String)
                return prop.GetString();
            if (prop.ValueKind == JsonValueKind.Number)
                return prop.ToString();
        }
        return null;
    }

    private static int GetJsonInt(JsonElement element, string propName)
    {
        if (element.TryGetProperty(propName, out var prop))
        {
            if (prop.ValueKind == JsonValueKind.Number && prop.TryGetInt32(out var val))
                return val;
            if (prop.ValueKind == JsonValueKind.String && int.TryParse(prop.GetString(), out var sVal))
                return sVal;
        }
        return 0;
    }

    public async Task<List<ListenerInfo>> GetListenersAsync(
        StreamChannel channel,
        string defaultPassword,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        // 1. Direct in-memory check if matching local embedded streaming server
        if (LocalServer != null && LocalServer.IsRunning &&
            (channel.Host == "127.0.0.1" || channel.Host == "localhost" || channel.Host == "::1" || channel.Port == LocalServer.Port))
        {
            var mount = LocalServer.GetMount(channel.MountPath);
            if (mount != null)
            {
                return mount.GetConnectedListeners();
            }
        }

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(timeout);

        var effectivePassword = channel.UseDefaultPassword ? defaultPassword : channel.CustomPassword;
        var host = string.IsNullOrWhiteSpace(channel.Host) ? "127.0.0.1" : channel.Host.Trim();
        var cleanMount = NormalizeMount(channel.MountPath);
        var sid = ExtractSid(channel.MountPath);

        // 1. Try Icecast listclients admin endpoint: http://host:port/admin/listclients?mount=/live
        var icecastUrl = $"http://{host}:{channel.Port}/admin/listclients?mount={Uri.EscapeDataString(cleanMount)}";
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, icecastUrl);
            if (!string.IsNullOrEmpty(effectivePassword))
            {
                var authBytes = Encoding.UTF8.GetBytes($"admin:{effectivePassword}");
                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
            }

            using var resp = await _httpClient.SendAsync(req, HttpCompletionOption.ResponseContentRead, cts.Token);
            if (resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync(cts.Token);
                var list = ParseIcecastListenersXml(body);
                if (list.Count > 0)
                    return list;
            }
        }
        catch { }

        // 2. Try Icecast listclients.xsl (HTML/XML)
        var icecastXslUrl = $"http://{host}:{channel.Port}/admin/listclients.xsl?mount={Uri.EscapeDataString(cleanMount)}";
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, icecastXslUrl);
            if (!string.IsNullOrEmpty(effectivePassword))
            {
                var authBytes = Encoding.UTF8.GetBytes($"admin:{effectivePassword}");
                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
            }

            using var resp = await _httpClient.SendAsync(req, HttpCompletionOption.ResponseContentRead, cts.Token);
            if (resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync(cts.Token);
                var list = ParseIcecastListenersXml(body);
                if (list.Count > 0)
                    return list;
            }
        }
        catch { }

        // 3. Try SHOUTcast admin listener list: /admin.cgi?mode=viewxml&page=3&sid=1
        var shoutcastUrl = $"http://{host}:{channel.Port}/admin.cgi?mode=viewxml&page=3&sid={sid}";
        if (!string.IsNullOrEmpty(effectivePassword))
        {
            shoutcastUrl += $"&pass={Uri.EscapeDataString(effectivePassword)}";
        }

        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, shoutcastUrl);
            if (!string.IsNullOrEmpty(effectivePassword))
            {
                var authBytes = Encoding.UTF8.GetBytes($"admin:{effectivePassword}");
                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
            }

            using var resp = await _httpClient.SendAsync(req, HttpCompletionOption.ResponseContentRead, cts.Token);
            if (resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync(cts.Token);
                var list = ParseShoutcastListenersXml(body);
                if (list.Count > 0)
                    return list;
            }
        }
        catch { }

        return new List<ListenerInfo>();
    }

    public static List<ListenerInfo> ParseIcecastListenersXml(string xmlOrHtml)
    {
        var result = new List<ListenerInfo>();
        try
        {
            var doc = XDocument.Parse(xmlOrHtml);
            var listenerNodes = doc.Descendants("listener");

            int idx = 1;
            foreach (var node in listenerNodes)
            {
                var ip = node.Element("IP")?.Value ?? node.Element("ip")?.Value ?? string.Empty;
                var ua = node.Element("UserAgent")?.Value ?? node.Element("useragent")?.Value ?? string.Empty;
                int connected = int.TryParse(node.Element("Connected")?.Value ?? node.Element("connected")?.Value, out var c) ? c : 0;
                int id = int.TryParse(node.Element("ID")?.Value ?? node.Element("id")?.Value, out var i) ? i : idx++;

                if (!string.IsNullOrWhiteSpace(ip))
                {
                    result.Add(new ListenerInfo
                    {
                        Id = id,
                        IpAddress = ip.Trim(),
                        UserAgent = string.IsNullOrWhiteSpace(ua) ? "Unknown Player" : ua.Trim(),
                        ConnectedSeconds = connected,
                        Location = ResolveIpLocation(ip.Trim())
                    });
                }
            }
        }
        catch
        {
            // If raw XML parse fails, fallback to regex HTML parsing for /admin/listclients.xsl
            var matches = System.Text.RegularExpressions.Regex.Matches(xmlOrHtml, @"(?:IP|ip)[\s:>]+([0-9a-fA-F\.:]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int id = 1;
            foreach (System.Text.RegularExpressions.Match m in matches)
            {
                if (m.Groups.Count > 1 && IPAddress.TryParse(m.Groups[1].Value, out _))
                {
                    var ip = m.Groups[1].Value;
                    result.Add(new ListenerInfo
                    {
                        Id = id++,
                        IpAddress = ip,
                        UserAgent = "Web Player / Stream Client",
                        ConnectedSeconds = 60,
                        Location = ResolveIpLocation(ip)
                    });
                }
            }
        }

        return result;
    }

    public static List<ListenerInfo> ParseShoutcastListenersXml(string xml)
    {
        var result = new List<ListenerInfo>();
        try
        {
            var doc = XDocument.Parse(xml);
            var listenerNodes = doc.Descendants("LISTENER");

            int idx = 1;
            foreach (var node in listenerNodes)
            {
                var ip = node.Element("HOSTNAME")?.Value ?? node.Element("IP")?.Value ?? string.Empty;
                var ua = node.Element("USERAGENT")?.Value ?? string.Empty;
                int connected = int.TryParse(node.Element("CONNECTTIME")?.Value, out var c) ? c : 0;
                int id = int.TryParse(node.Element("UID")?.Value ?? node.Element("POINTER")?.Value, out var i) ? i : idx++;

                if (!string.IsNullOrWhiteSpace(ip))
                {
                    result.Add(new ListenerInfo
                    {
                        Id = id,
                        IpAddress = ip.Trim(),
                        UserAgent = string.IsNullOrWhiteSpace(ua) ? "SHOUTcast Client" : ua.Trim(),
                        ConnectedSeconds = connected,
                        Location = ResolveIpLocation(ip.Trim())
                    });
                }
            }
        }
        catch { }

        return result;
    }

    private static string ResolveIpLocation(string ip)
    {
        if (ip == "127.0.0.1" || ip == "::1" || ip.StartsWith("192.168.") || ip.StartsWith("10.") || ip.StartsWith("172.16."))
            return "Local / LAN";
        return "Remote Client";
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
