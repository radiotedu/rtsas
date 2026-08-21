using System.Reflection;
using IcecastStreamMonitor.RemoteControl.Models;
using IcecastStreamMonitor.RemoteControl.Services;

namespace IcecastStreamMonitor.RemoteControl.Forms;

public partial class RemoteAnalyticsDialog : Form
{
    private readonly RemoteStreamItem _channel;
    private readonly RemoteApiClient _apiClient;

    public RemoteAnalyticsDialog(RemoteStreamItem channel, RemoteApiClient apiClient)
    {
        InitializeComponent();
        _channel = channel;
        _apiClient = apiClient;

        EnableDoubleBuffering(gridLogs);

        Text = $"24-Hour Remote Analytics - {_channel.Name} ({_channel.MountPath})";
        lblTitle.Text = $"24-Hour Analytics: {_channel.Name}";
        lblStreamInfo.Text = $"Server: {_channel.Host}:{_channel.Port} | Mount: {_channel.MountPath} | Format: {_channel.AudioFormat}";
    }

    private static void EnableDoubleBuffering(DataGridView dgv)
    {
        typeof(DataGridView).InvokeMember(
            "DoubleBuffered",
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
            null,
            dgv,
            new object[] { true });
    }

    private async void RemoteAnalyticsDialog_Load(object? sender, EventArgs e)
    {
        bool isDark = BackColor.R < 100;
        tabCharts.IsDarkMode = isDark;
        chartListeners.IsDarkMode = isDark;
        chartData.IsDarkMode = isDark;
        chartLatency.IsDarkMode = isDark;
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        btnRefresh.Enabled = false;
        var (metrics, history) = await _apiClient.GetHistoryAsync(_channel.Id);
        btnRefresh.Enabled = true;

        lblValPeak.Text = metrics.PeakListeners.ToString("N0");
        lblValAvg.Text = metrics.AverageListeners.ToString("F1");
        lblValData.Text = metrics.TotalMegaBytesTransferred >= 1024
            ? $"{(metrics.TotalMegaBytesTransferred / 1024.0):F2} GB"
            : $"{metrics.TotalMegaBytesTransferred:F1} MB";
        lblValUptime.Text = $"{metrics.UptimePercentage:F1} %";
        lblValLatency.Text = $"{metrics.AverageLatencyMs:F0} ms";

        chartListeners.SetData(history);
        chartData.SetData(history);
        chartLatency.SetData(history);

        gridLogs.Rows.Clear();
        foreach (var log in history.OrderByDescending(h => h.Timestamp))
        {
            var idx = gridLogs.Rows.Add();
            var row = gridLogs.Rows[idx];

            row.Cells[colTime.Index].Value = log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
            row.Cells[colLogStatus.Index].Value = log.IsOnline ? "ONLINE" : "OFFLINE";
            row.Cells[colLogListeners.Index].Value = log.Listeners.ToString("N0");
            row.Cells[colLogBitrate.Index].Value = log.BitRateKbps > 0 ? $"{log.BitRateKbps} kbps" : "-";
            row.Cells[colLogData.Index].Value = log.MegaBytesTransferred.ToString("F2");
            row.Cells[colLogLatency.Index].Value = $"{log.LatencyMs} ms";
            row.Cells[colLogTitle.Index].Value = !string.IsNullOrEmpty(log.SongTitle) ? log.SongTitle : log.StatusMessage;

            if (!log.IsOnline)
            {
                row.DefaultCellStyle.ForeColor = Color.DarkRed;
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 245);
            }
        }
    }

    private async void BtnRefresh_Click(object? sender, EventArgs e)
    {
        await LoadDataAsync();
    }
}
