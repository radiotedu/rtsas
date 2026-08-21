using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using IcecastStreamMonitor.RemoteControl.Models;

namespace IcecastStreamMonitor.RemoteControl.Services;

public class RemoteApiClient
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public string BaseUrl { get; private set; } = string.Empty;
    public RemoteUserSession? CurrentSession { get; private set; }

    public RemoteApiClient()
    {
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(6) };
    }

    public async Task<(bool Success, string ErrorMessage, RemoteUserSession? Session)> LoginAsync(
        string host, int port, string username, string password)
    {
        BaseUrl = $"http://{host.Trim()}:{port}/api";
        try
        {
            var payload = new { username, password };
            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync($"{BaseUrl}/auth/login", content);
            var resJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errDoc = JsonDocument.Parse(resJson);
                var errMsg = errDoc.RootElement.TryGetProperty("error", out var ep) ? ep.GetString() : "Login failed.";
                return (false, errMsg ?? "Authentication failed.", null);
            }

            var session = JsonSerializer.Deserialize<RemoteUserSession>(resJson, JsonOpts);
            CurrentSession = session;

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", session?.Token ?? "");

            return (true, string.Empty, session);
        }
        catch (Exception ex)
        {
            return (false, $"Connection error: {ex.Message}", null);
        }
    }

    public async Task<List<RemoteStreamItem>> GetStreamsAsync()
    {
        try
        {
            using var response = await _httpClient.GetAsync($"{BaseUrl}/streams");
            if (!response.IsSuccessStatusCode) return new List<RemoteStreamItem>();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("streams", out var sProp))
            {
                return JsonSerializer.Deserialize<List<RemoteStreamItem>>(sProp.GetRawText(), JsonOpts) ?? new List<RemoteStreamItem>();
            }
        }
        catch { }

        return new List<RemoteStreamItem>();
    }

    public async Task<(Remote24HourMetrics Metrics, List<RemoteSnapshotLog> History)> GetHistoryAsync(Guid channelId)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"{BaseUrl}/streams/{channelId}/history");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var metrics = root.TryGetProperty("metrics", out var mProp)
                    ? JsonSerializer.Deserialize<Remote24HourMetrics>(mProp.GetRawText(), JsonOpts) ?? new Remote24HourMetrics()
                    : new Remote24HourMetrics();

                var history = root.TryGetProperty("history", out var hProp)
                    ? JsonSerializer.Deserialize<List<RemoteSnapshotLog>>(hProp.GetRawText(), JsonOpts) ?? new List<RemoteSnapshotLog>()
                    : new List<RemoteSnapshotLog>();

                return (metrics, history);
            }
        }
        catch { }

        return (new Remote24HourMetrics(), new List<RemoteSnapshotLog>());
    }

    public async Task<(bool Allowed, string Message, List<RemoteListenerInfo> Listeners)> GetListenersAsync(Guid channelId)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"{BaseUrl}/streams/{channelId}/listeners");
            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                using var errDoc = JsonDocument.Parse(json);
                var msg = errDoc.RootElement.TryGetProperty("error", out var ep) ? ep.GetString() : "Permission denied.";
                return (false, msg ?? "Permission denied.", new List<RemoteListenerInfo>());
            }

            if (response.IsSuccessStatusCode)
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("listeners", out var lProp))
                {
                    var list = JsonSerializer.Deserialize<List<RemoteListenerInfo>>(lProp.GetRawText(), JsonOpts) ?? new List<RemoteListenerInfo>();
                    return (true, string.Empty, list);
                }
            }
        }
        catch (Exception ex)
        {
            return (false, ex.Message, new List<RemoteListenerInfo>());
        }

        return (false, "Failed to retrieve listeners.", new List<RemoteListenerInfo>());
    }

    public async Task<bool> TriggerRefreshAsync()
    {
        try
        {
            using var response = await _httpClient.PostAsync($"{BaseUrl}/streams/refresh", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AddChannelAsync(string name, string mount, string host, int port, int serverType)
    {
        try
        {
            var payload = new
            {
                action = "add",
                channel = new
                {
                    name,
                    mountPath = mount,
                    host,
                    port,
                    serverType,
                    useDefaultPassword = true,
                    enabled = true
                }
            };
            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync($"{BaseUrl}/streams/manage", content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteChannelAsync(Guid channelId)
    {
        try
        {
            var payload = new { action = "delete", id = channelId };
            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync($"{BaseUrl}/streams/manage", content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
