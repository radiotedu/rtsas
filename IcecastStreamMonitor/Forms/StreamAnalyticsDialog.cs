using System.Reflection;
using System.Text;
using IcecastStreamMonitor.Models;
using IcecastStreamMonitor.Services;

namespace IcecastStreamMonitor.Forms;

public partial class StreamAnalyticsDialog : Form
{
    private readonly StreamChannel _channel;
    private readonly StreamHistoryService _historyService;
    private readonly StreamStatus? _currentStatus;

    public StreamAnalyticsDialog(StreamChannel channel, StreamHistoryService historyService, StreamStatus? currentStatus)
    {
        InitializeComponent();
        _channel = channel;
        _historyService = historyService;
        _currentStatus = currentStatus;

        EnableDoubleBuffering(gridLogs);

        Text = $"24h Analytics & Logs - {_channel.Name} ({_channel.MountPath})";
        lblTitle.Text = $"24-Hour Analytics & Logs: {_channel.Name}";

        var br = _currentStatus?.BitRateKbps > 0 ? $"{_currentStatus.BitRateKbps} kbps" : "N/A";
        var fmt = !string.IsNullOrEmpty(_currentStatus?.AudioFormat) ? _currentStatus.AudioFormat : _channel.ServerType.ToString();
        lblStreamInfo.Text = $"Server: {_channel.Host}:{_channel.Port} | Mount: {_channel.MountPath} | Format: {fmt} @ {br} | Active: {(_channel.Enabled ? "Yes" : "No")}";
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

    private void StreamAnalyticsDialog_Load(object? sender, EventArgs e)
    {
        bool isDark = BackColor.R < 100;
        chartListeners.IsDarkMode = isDark;
        chartData.IsDarkMode = isDark;
        chartLatency.IsDarkMode = isDark;
        LoadData();
    }

    private void LoadData()
    {
        var history = _historyService.Get24HourHistory(_channel.Id, _channel.Name, _currentStatus);
        var metrics = _historyService.GetMetricsSummary(_channel.Id);

        // Update Summary Cards
        lblValPeak.Text = metrics.PeakListeners.ToString("N0");
        lblValAvg.Text = metrics.AverageListeners.ToString("F1");
        lblValData.Text = metrics.TotalMegaBytesTransferred >= 1024
            ? $"{(metrics.TotalMegaBytesTransferred / 1024.0):F2} GB"
            : $"{metrics.TotalMegaBytesTransferred:F1} MB";
        lblValUptime.Text = $"{metrics.UptimePercentage:F1} %";
        lblValLatency.Text = $"{metrics.AverageLatencyMs:F0} ms";

        // Update Charts
        chartListeners.SetData(history);
        chartData.SetData(history);
        chartLatency.SetData(history);

        // Update Log Table (Descending)
        gridLogs.Rows.Clear();
        foreach (var log in history.OrderByDescending(l => l.Timestamp))
        {
            var idx = gridLogs.Rows.Add();
            var row = gridLogs.Rows[idx];

            row.Cells[colTime.Index].Value = log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
            row.Cells[colLogStatus.Index].Value = log.IsOnline ? "ONLINE" : "OFFLINE";
            row.Cells[colLogStatus.Index].Style.ForeColor = log.IsOnline ? Color.ForestGreen : Color.Firebrick;
            row.Cells[colLogStatus.Index].Style.Font = new Font(gridLogs.Font, FontStyle.Bold);

            row.Cells[colLogListeners.Index].Value = log.Listeners.ToString("N0");
            row.Cells[colLogBitrate.Index].Value = log.BitRateKbps > 0 ? $"{log.BitRateKbps} kbps" : "-";
            row.Cells[colLogData.Index].Value = $"{log.MegaBytesTransferred:F2}";
            row.Cells[colLogLatency.Index].Value = log.LatencyMs > 0 ? $"{log.LatencyMs} ms" : "-";
            row.Cells[colLogTitle.Index].Value = !string.IsNullOrWhiteSpace(log.SongTitle) ? log.SongTitle : log.StatusMessage;
        }
    }

    private void BtnRefresh_Click(object? sender, EventArgs e)
    {
        LoadData();
    }

    private void BtnExportCsv_Click(object? sender, EventArgs e)
    {
        var history = _historyService.Get24HourHistory(_channel.Id, _channel.Name, _currentStatus);
        if (history.Count == 0)
        {
            MessageBox.Show("No log entries to export.", "Empty Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "CSV File (*.csv)|*.csv|All Files (*.*)|*.*",
            FileName = $"stream_logs_{_channel.MountPath.TrimStart('/').Replace('/', '_')}_{DateTime.Now:yyyyMMdd_HHmm}.csv"
        };

        if (sfd.ShowDialog(this) == DialogResult.OK)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("Timestamp,Status,ConcurrentListeners,BitRateKbps,DataTransferMB,LatencyMs,SongTitle");
                foreach (var item in history)
                {
                    var title = item.SongTitle.Replace("\"", "\"\"");
                    sb.AppendLine($"\"{item.Timestamp:yyyy-MM-dd HH:mm:ss}\",\"{ (item.IsOnline ? "ONLINE" : "OFFLINE") }\",{item.Listeners},{item.BitRateKbps},{item.MegaBytesTransferred:F4},{item.LatencyMs},\"{title}\"");
                }

                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show($"Successfully exported {history.Count} log records to:\n{sfd.FileName}", "Export Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to export CSV: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
