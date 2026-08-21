using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Text.Json;
using IcecastStreamMonitor.Models;

namespace IcecastStreamMonitor.Services;

public class RemoteApiService : IDisposable
{
    private readonly AppSettings _settings;
    private readonly Func<Dictionary<Guid, StreamStatus>> _getStatusCache;
    private readonly Func<Task> _triggerRefresh;
    private readonly StreamHistoryService _historyService;
    private readonly IStreamMonitorService _monitorService;
    private readonly AuditLogService _auditLogService;
    private readonly Action _saveSettingsCallback;

    private HttpListener? _listener;
    private CancellationTokenSource? _cts;
    private readonly ConcurrentDictionary<string, (RemoteUser User, DateTime Expiry)> _sessions = new();

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public bool IsRunning => _listener != null && _listener.IsListening;
    public int Port => _settings.RemoteServerPort;

    public RemoteApiService(
        AppSettings settings,
        Func<Dictionary<Guid, StreamStatus>> getStatusCache,
        Func<Task> triggerRefresh,
        StreamHistoryService historyService,
        IStreamMonitorService monitorService,
        AuditLogService auditLogService,
        Action saveSettingsCallback)
    {
        _settings = settings;
        _getStatusCache = getStatusCache;
        _triggerRefresh = triggerRefresh;
        _historyService = historyService;
        _monitorService = monitorService;
        _auditLogService = auditLogService;
        _saveSettingsCallback = saveSettingsCallback;
    }

    public void Start()
    {
        if (IsRunning || !_settings.RemoteServerEnabled) return;

        _cts = new CancellationTokenSource();

        try
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://*:{_settings.RemoteServerPort}/api/");
            _listener.Start();
        }
        catch
        {
            try
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add($"http://localhost:{_settings.RemoteServerPort}/api/");
                _listener.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to start Remote API Listener: {ex.Message}");
                return;
            }
        }

        Task.Run(() => ListenLoopAsync(_cts.Token));
        _auditLogService.Log(AuditCategory.System, "Remote API", "Service Started", $"Remote Control API server listening on port {_settings.RemoteServerPort}");
    }

    public void Stop()
    {
        _cts?.Cancel();
        try
        {
            _listener?.Stop();
            _listener?.Close();
        }
        catch { }
        _listener = null;
    }

    private async Task ListenLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested && _listener != null && _listener.IsListening)
        {
            try
            {
                var context = await _listener.GetContextAsync();
                _ = ProcessRequestAsync(context);
            }
            catch (HttpListenerException) { break; }
            catch (ObjectDisposedException) { break; }
            catch { }
        }
    }

    private async Task ProcessRequestAsync(HttpListenerContext context)
    {
        var req = context.Request;
        var res = context.Response;

        // Enable CORS
        res.AddHeader("Access-Control-Allow-Origin", "*");
        res.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization");
        res.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");

        if (req.HttpMethod == "OPTIONS")
        {
            res.StatusCode = 200;
            res.Close();
            return;
        }

        try
        {
            var rawPath = req.Url?.AbsolutePath.TrimEnd('/') ?? "";
            var path = rawPath.StartsWith("/api") ? rawPath.Substring(4) : rawPath;

            // 1. Public Auth Login
            if (path == "/auth/login" && req.HttpMethod == "POST")
            {
                await HandleLoginAsync(req, res);
                return;
            }

            // Authentication check for all other endpoints
            var user = AuthenticateRequest(req);
            if (user == null)
            {
                await SendJsonAsync(res, new { error = "Unauthorized: Invalid or expired authentication token." }, 401);
                return;
            }

            // 2. GET /streams (Live status)
            if (path == "/streams" && req.HttpMethod == "GET")
            {
                if (!user.Permissions.CanViewStatus)
                {
                    await SendJsonAsync(res, new { error = "Forbidden: You do not have permission to view stream status." }, 403);
                    return;
                }

                var statusCache = _getStatusCache();
                var list = _settings.Channels.Select(ch =>
                {
                    var st = statusCache.GetValueOrDefault(ch.Id);
                    return new
                    {
                        id = ch.Id,
                        name = ch.Name,
                        mountPath = ch.MountPath,
                        host = ch.Host,
                        port = ch.Port,
                        serverType = ch.ServerType.ToString(),
                        enabled = ch.Enabled,
                        isOnline = st?.IsOnline ?? false,
                        concurrentListeners = st?.ConcurrentListeners ?? 0,
                        peakListeners = st?.PeakListeners ?? 0,
                        bitRateKbps = st?.BitRateKbps ?? 0,
                        audioFormat = st?.AudioFormat ?? "",
                        streamTitle = st?.StreamTitle ?? "",
                        latencyMs = st?.LatencyMs ?? 0,
                        lastChecked = st?.LastChecked
                    };
                }).ToList();

                await SendJsonAsync(res, new { streams = list });
                return;
            }

            // 3. GET /streams/{id}/history (24h metrics & logs)
            if (path.StartsWith("/streams/") && path.EndsWith("/history") && req.HttpMethod == "GET")
            {
                if (!user.Permissions.CanViewLogs)
                {
                    await SendJsonAsync(res, new { error = "Forbidden: You do not have permission to view 24h history logs." }, 403);
                    return;
                }

                var parts = path.Split('/');
                if (parts.Length >= 3 && Guid.TryParse(parts[2], out var channelId))
                {
                    var history = _historyService.Get24HourHistory(channelId);
                    var metrics = _historyService.GetMetricsSummary(channelId);
                    await SendJsonAsync(res, new { metrics, history });
                    return;
                }
            }

            // 4. GET /streams/{id}/listeners (Connected listener IPs - RBAC protected!)
            if (path.StartsWith("/streams/") && path.EndsWith("/listeners") && req.HttpMethod == "GET")
            {
                if (!user.Permissions.CanViewListenerIps)
                {
                    await SendJsonAsync(res, new { error = "Access Denied: Your user role does not have permission to view connected client IPs." }, 403);
                    return;
                }

                var parts = path.Split('/');
                if (parts.Length >= 3 && Guid.TryParse(parts[2], out var channelId))
                {
                    var ch = _settings.Channels.FirstOrDefault(c => c.Id == channelId);
                    if (ch != null)
                    {
                        var listeners = await _monitorService.GetListenersAsync(ch, _settings.DefaultPassword, TimeSpan.FromSeconds(5));
                        await SendJsonAsync(res, new { channelName = ch.Name, mountPath = ch.MountPath, listeners });
                        return;
                    }
                }
            }

            // 5. POST /streams/refresh
            if (path == "/streams/refresh" && req.HttpMethod == "POST")
            {
                if (!user.Permissions.CanRefreshStreams)
                {
                    await SendJsonAsync(res, new { error = "Forbidden: Cannot refresh streams." }, 403);
                    return;
                }

                _ = _triggerRefresh();
                _auditLogService.Log(AuditCategory.System, user.Username, "Remote Refresh", "Triggered live refresh from Remote Control client.");
                await SendJsonAsync(res, new { success = true, message = "Refresh triggered." });
                return;
            }

            // 6. POST /streams/manage (SuperAdmin add/edit/delete)
            if (path == "/streams/manage" && req.HttpMethod == "POST")
            {
                if (!user.Permissions.CanManageChannels)
                {
                    await SendJsonAsync(res, new { error = "Forbidden: SuperAdmin privileges required to manage channels remotely." }, 403);
                    return;
                }

                using var reader = new StreamReader(req.InputStream, req.ContentEncoding);
                var body = await reader.ReadToEndAsync();
                using var doc = JsonDocument.Parse(body);
                var root = doc.RootElement;

                var action = root.GetProperty("action").GetString();
                if (action == "add")
                {
                    var chJson = root.GetProperty("channel");
                    var newCh = JsonSerializer.Deserialize<StreamChannel>(chJson.GetRawText(), JsonOpts);
                    if (newCh != null)
                    {
                        newCh.Id = Guid.NewGuid();
                        _settings.Channels.Add(newCh);
                        _saveSettingsCallback();
                        _auditLogService.LogChannelAdded(newCh);
                        _ = _triggerRefresh();
                        await SendJsonAsync(res, new { success = true, channel = newCh });
                        return;
                    }
                }
                else if (action == "delete")
                {
                    var targetId = root.GetProperty("id").GetGuid();
                    var existing = _settings.Channels.FirstOrDefault(c => c.Id == targetId);
                    if (existing != null)
                    {
                        _settings.Channels.Remove(existing);
                        _saveSettingsCallback();
                        _auditLogService.LogChannelDeleted(existing);
                        await SendJsonAsync(res, new { success = true });
                        return;
                    }
                }
            }

            // 7. GET /audit
            if (path == "/audit" && req.HttpMethod == "GET")
            {
                if (!user.Permissions.CanViewAuditLogs)
                {
                    await SendJsonAsync(res, new { error = "Forbidden: Cannot view audit logs." }, 403);
                    return;
                }

                var entries = _auditLogService.GetEntries();
                await SendJsonAsync(res, new { logs = entries });
                return;
            }

            await SendJsonAsync(res, new { error = "Endpoint not found." }, 404);
        }
        catch (Exception ex)
        {
            await SendJsonAsync(res, new { error = ex.Message }, 500);
        }
    }

    private async Task HandleLoginAsync(HttpListenerRequest req, HttpListenerResponse res)
    {
        using var reader = new StreamReader(req.InputStream, req.ContentEncoding);
        var body = await reader.ReadToEndAsync();

        try
        {
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;
            var username = root.GetProperty("username").GetString() ?? "";
            var password = root.GetProperty("password").GetString() ?? "";

            var user = _settings.RemoteUsers.FirstOrDefault(u =>
                u.IsActive && string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

            if (user == null || !SecurityHelper.VerifyPassword(password, user.PasswordHash))
            {
                await SendJsonAsync(res, new { error = "Invalid username or password." }, 401);
                return;
            }

            var token = Guid.NewGuid().ToString("N");
            _sessions[token] = (user, DateTime.UtcNow.AddHours(12));

            await SendJsonAsync(res, new
            {
                token,
                username = user.Username,
                role = user.RoleName,
                permissions = user.Permissions
            });
        }
        catch
        {
            await SendJsonAsync(res, new { error = "Malformed login request." }, 400);
        }
    }

    private RemoteUser? AuthenticateRequest(HttpListenerRequest req)
    {
        var authHeader = req.Headers["Authorization"];
        if (string.IsNullOrWhiteSpace(authHeader)) return null;

        var token = authHeader.Replace("Bearer ", "").Trim();
        if (_sessions.TryGetValue(token, out var session))
        {
            if (session.Expiry > DateTime.UtcNow)
            {
                return session.User;
            }
            _sessions.TryRemove(token, out _);
        }

        return null;
    }

    private static async Task SendJsonAsync(HttpListenerResponse res, object data, int statusCode = 200)
    {
        res.StatusCode = statusCode;
        res.ContentType = "application/json; charset=utf-8";
        var json = JsonSerializer.Serialize(data, JsonOpts);
        var bytes = Encoding.UTF8.GetBytes(json);
        res.ContentLength64 = bytes.Length;
        await res.OutputStream.WriteAsync(bytes);
        res.Close();
    }

    public void Dispose()
    {
        Stop();
    }
}
