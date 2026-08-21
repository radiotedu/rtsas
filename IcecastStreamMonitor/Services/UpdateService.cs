using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using IcecastStreamMonitor.Models;

namespace IcecastStreamMonitor.Services;

public class UpdateService
{
    private readonly HttpClient _httpClient;
    private readonly string _gitHubRepo;

    public string CurrentVersion { get; }

    public UpdateService(string gitHubRepo = "orcunkilic/IcecastStreamMonitor", HttpClient? httpClient = null)
    {
        _gitHubRepo = gitHubRepo;

        if (httpClient != null)
        {
            _httpClient = httpClient;
        }
        else
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("IcecastStreamMonitor", "1.0"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            _httpClient.Timeout = TimeSpan.FromSeconds(8);
        }

        var version = Assembly.GetExecutingAssembly().GetName().Version;
        CurrentVersion = version != null ? $"{version.Major}.{version.Minor}.{version.Build}" : "1.0.0";
    }

    public async Task<UpdateInfo> CheckForUpdatesAsync(CancellationToken cancellationToken = default)
    {
        var result = new UpdateInfo
        {
            CurrentVersion = CurrentVersion,
            LatestVersion = CurrentVersion
        };

        try
        {
            var url = $"https://api.github.com/repos/{_gitHubRepo}/releases/latest";
            using var response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                result.ErrorMessage = "No public GitHub releases found for this repository yet.";
                return result;
            }

            if (!response.IsSuccessStatusCode)
            {
                result.ErrorMessage = $"GitHub API returned status {(int)response.StatusCode}: {response.ReasonPhrase}";
                return result;
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            string tagName = root.TryGetProperty("tag_name", out var tProp) ? (tProp.GetString() ?? "") : "";
            string body = root.TryGetProperty("body", out var bProp) ? (bProp.GetString() ?? "") : "";
            string publishedAtStr = root.TryGetProperty("published_at", out var pProp) ? (pProp.GetString() ?? "") : "";
            string htmlUrl = root.TryGetProperty("html_url", out var hProp) ? (hProp.GetString() ?? "") : "";

            var cleanLatest = tagName.TrimStart('v', 'V').Trim();
            if (DateTime.TryParse(publishedAtStr, out var publishedDate))
            {
                result.PublishedAt = publishedDate;
            }

            result.LatestVersion = cleanLatest;
            result.ReleaseNotes = body;
            result.DownloadUrl = htmlUrl;

            // Check if installer asset exists
            if (root.TryGetProperty("assets", out var assets) && assets.ValueKind == JsonValueKind.Array)
            {
                foreach (var asset in assets.EnumerateArray())
                {
                    var name = asset.TryGetProperty("name", out var n) ? n.GetString() : "";
                    var downloadUrl = asset.TryGetProperty("browser_download_url", out var d) ? d.GetString() : "";
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(downloadUrl))
                    {
                        if (name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) || name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                        {
                            result.DownloadUrl = downloadUrl;
                            break;
                        }
                    }
                }
            }

            result.IsUpdateAvailable = IsNewerVersion(CurrentVersion, cleanLatest);
            return result;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.Message;
            return result;
        }
    }

    public static bool IsNewerVersion(string currentStr, string latestStr)
    {
        if (string.IsNullOrWhiteSpace(latestStr)) return false;

        if (Version.TryParse(currentStr, out var cur) && Version.TryParse(latestStr, out var lat))
        {
            return lat > cur;
        }

        return false;
    }

    public static void OpenUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return;
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch { }
    }
}
