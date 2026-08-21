using IcecastStreamMonitor.Models;
using IcecastStreamMonitor.Services;

namespace IcecastStreamMonitor.Forms;

public partial class UpdateCheckDialog : Form
{
    private readonly UpdateService _updateService;
    private UpdateInfo? _latestUpdateInfo;

    public UpdateCheckDialog(UpdateService updateService)
    {
        InitializeComponent();
        _updateService = updateService;
    }

    private async void UpdateCheckDialog_Load(object? sender, EventArgs e)
    {
        await PerformCheckAsync();
    }

    private async Task PerformCheckAsync()
    {
        btnCheckNow.Enabled = false;
        btnDownloadUpdate.Enabled = false;
        lblCurrentVersion.Text = $"Installed Version: v{_updateService.CurrentVersion}";
        lblLatestVersion.Text = "Latest Version: Checking GitHub...";
        lblStatusBadge.Text = "Checking...";
        lblStatusBadge.ForeColor = Color.DimGray;
        txtReleaseNotes.Text = "Querying GitHub Releases API...";

        _latestUpdateInfo = await _updateService.CheckForUpdatesAsync();

        lblCurrentVersion.Text = $"Installed Version: v{_latestUpdateInfo.CurrentVersion}";
        lblLatestVersion.Text = $"Latest Version on GitHub: v{_latestUpdateInfo.LatestVersion}";

        if (!string.IsNullOrEmpty(_latestUpdateInfo.ErrorMessage))
        {
            lblStatusBadge.Text = "Check Failed / No Releases";
            lblStatusBadge.ForeColor = Color.DarkGoldenrod;
            txtReleaseNotes.Text = $"Info: {_latestUpdateInfo.ErrorMessage}\n\nYou are on version v{_latestUpdateInfo.CurrentVersion}. As soon as releases are tagged on GitHub, new update installers will automatically be listed here.";
        }
        else if (_latestUpdateInfo.IsUpdateAvailable)
        {
            lblStatusBadge.Text = "🎉 New Update Available!";
            lblStatusBadge.ForeColor = Color.DarkGreen;
            btnDownloadUpdate.Enabled = true;

            var dateStr = _latestUpdateInfo.PublishedAt.HasValue
                ? _latestUpdateInfo.PublishedAt.Value.ToString("yyyy-MM-dd HH:mm")
                : "";

            txtReleaseNotes.Text = $"Version v{_latestUpdateInfo.LatestVersion} ({dateStr})\n\n{_latestUpdateInfo.ReleaseNotes}";
        }
        else
        {
            lblStatusBadge.Text = "✓ You are up to date!";
            lblStatusBadge.ForeColor = Color.ForestGreen;
            txtReleaseNotes.Text = $"You are using the latest version (v{_latestUpdateInfo.CurrentVersion}).\n\nNo newer releases found on GitHub.";
        }

        btnCheckNow.Enabled = true;
    }

    private async void BtnCheckNow_Click(object? sender, EventArgs e)
    {
        await PerformCheckAsync();
    }

    private void BtnDownloadUpdate_Click(object? sender, EventArgs e)
    {
        if (_latestUpdateInfo != null && !string.IsNullOrEmpty(_latestUpdateInfo.DownloadUrl))
        {
            UpdateService.OpenUrl(_latestUpdateInfo.DownloadUrl);
        }
        else
        {
            UpdateService.OpenUrl("https://github.com/orcunkilic/IcecastStreamMonitor/releases");
        }
    }
}
