using System.Diagnostics;
using System.Reflection;
using System.Text;
using IcecastStreamMonitor.Models;
using IcecastStreamMonitor.Services;

namespace IcecastStreamMonitor.Forms;

public partial class ListenerListDialog : Form
{
    private readonly StreamChannel _channel;
    private readonly string _defaultPassword;
    private readonly IStreamMonitorService _monitorService;
    private readonly System.Windows.Forms.Timer _timer = new();
    private bool _isLoading = false;
    private CancellationTokenSource? _cts;

    public ListenerListDialog(StreamChannel channel, string defaultPassword, IStreamMonitorService monitorService)
    {
        InitializeComponent();
        _channel = channel;
        _defaultPassword = defaultPassword;
        _monitorService = monitorService;

        EnableDoubleBuffering(gridListeners);

        Text = $"Listeners - {_channel.Name} ({_channel.MountPath})";
        lblTitle.Text = $"Connected Listeners: {_channel.Name}";
        lblSubtitle.Text = $"Server: {_channel.Host}:{_channel.Port} | Mount: {_channel.MountPath} | Auth: {(_channel.UseDefaultPassword ? "Default" : "Custom")}";

        _timer.Interval = 5000;
        _timer.Tick += async (s, e) => await LoadListenersAsync();
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

    private async void ListenerListDialog_Load(object? sender, EventArgs e)
    {
        if (chkAutoRefresh.Checked)
        {
            _timer.Start();
        }
        await LoadListenersAsync();
    }

    private async Task LoadListenersAsync()
    {
        if (_isLoading) return;
        _isLoading = true;

        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        lblStatus.Text = "Fetching client IPs from server admin API...";
        lblStatus.ForeColor = SystemColors.GrayText;
        btnRefresh.Enabled = false;

        var sw = Stopwatch.StartNew();

        try
        {
            var listeners = await _monitorService.GetListenersAsync(
                _channel,
                _defaultPassword,
                TimeSpan.FromSeconds(5),
                _cts.Token);

            sw.Stop();

            gridListeners.Rows.Clear();
            foreach (var l in listeners)
            {
                var idx = gridListeners.Rows.Add();
                var row = gridListeners.Rows[idx];
                row.Cells[colId.Index].Value = l.Id;
                row.Cells[colIp.Index].Value = l.IpAddress;
                row.Cells[colIp.Index].Style.Font = new Font(gridListeners.Font, FontStyle.Bold);
                row.Cells[colLocation.Index].Value = l.Location;
                row.Cells[colDuration.Index].Value = l.FormattedConnectedTime;
                row.Cells[colUserAgent.Index].Value = l.CleanUserAgent;
            }

            if (listeners.Count > 0)
            {
                lblStatus.Text = $"Found {listeners.Count} active listener(s) ({sw.ElapsedMilliseconds}ms at {DateTime.Now:HH:mm:ss})";
                lblStatus.ForeColor = Color.ForestGreen;
            }
            else
            {
                lblStatus.Text = "No client IPs returned (Server may require admin password or have 0 listeners).";
                lblStatus.ForeColor = Color.DarkGoldenrod;
            }
        }
        catch (OperationCanceledException)
        {
            lblStatus.Text = "Cancelled";
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"Error: {ex.Message}";
            lblStatus.ForeColor = Color.Firebrick;
        }
        finally
        {
            btnRefresh.Enabled = true;
            _isLoading = false;
        }
    }

    private async void BtnRefresh_Click(object? sender, EventArgs e)
    {
        await LoadListenersAsync();
    }

    private void ChkAutoRefresh_CheckedChanged(object? sender, EventArgs e)
    {
        if (chkAutoRefresh.Checked)
            _timer.Start();
        else
            _timer.Stop();
    }

    private void BtnCopySelected_Click(object? sender, EventArgs e)
    {
        if (gridListeners.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a listener row to copy its IP.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var sb = new StringBuilder();
        foreach (DataGridViewRow row in gridListeners.SelectedRows)
        {
            var ip = row.Cells[colIp.Index].Value?.ToString();
            if (!string.IsNullOrEmpty(ip))
                sb.AppendLine(ip);
        }

        var text = sb.ToString().Trim();
        if (!string.IsNullOrEmpty(text))
        {
            Clipboard.SetText(text);
            lblStatus.Text = $"Copied IP ({text}) to clipboard!";
            lblStatus.ForeColor = Color.ForestGreen;
        }
    }

    private void BtnCopyAll_Click(object? sender, EventArgs e)
    {
        if (gridListeners.Rows.Count == 0)
        {
            MessageBox.Show("No listener IPs available to copy.", "Empty List", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"# Listeners for {_channel.Name} ({_channel.MountPath}) at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine($"# ID\tIP Address\tConnected\tLocation\tPlayer/User-Agent");

        foreach (DataGridViewRow row in gridListeners.Rows)
        {
            var id = row.Cells[colId.Index].Value;
            var ip = row.Cells[colIp.Index].Value;
            var duration = row.Cells[colDuration.Index].Value;
            var loc = row.Cells[colLocation.Index].Value;
            var ua = row.Cells[colUserAgent.Index].Value;
            sb.AppendLine($"{id}\t{ip}\t{duration}\t{loc}\t{ua}");
        }

        var text = sb.ToString();
        Clipboard.SetText(text);
        lblStatus.Text = $"Copied all {gridListeners.Rows.Count} listener(s) to clipboard!";
        lblStatus.ForeColor = Color.ForestGreen;
    }

    private void ListenerListDialog_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _timer.Stop();
        _cts?.Cancel();
    }
}
