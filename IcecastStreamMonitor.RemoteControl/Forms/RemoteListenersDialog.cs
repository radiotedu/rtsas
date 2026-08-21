using System.Reflection;
using System.Text;
using IcecastStreamMonitor.RemoteControl.Models;
using IcecastStreamMonitor.RemoteControl.Services;

namespace IcecastStreamMonitor.RemoteControl.Forms;

public partial class RemoteListenersDialog : Form
{
    private readonly RemoteStreamItem _channel;
    private readonly RemoteApiClient _apiClient;
    private readonly System.Windows.Forms.Timer _refreshTimer = new();
    private List<RemoteListenerInfo> _currentListeners = new();

    public RemoteListenersDialog(RemoteStreamItem channel, RemoteApiClient apiClient)
    {
        InitializeComponent();
        _channel = channel;
        _apiClient = apiClient;

        EnableDoubleBuffering(gridListeners);

        Text = $"Connected Listeners - {_channel.Name} ({_channel.MountPath})";
        lblTitle.Text = $"👥 Connected Listeners: {_channel.Name}";
        lblStreamInfo.Text = $"Server: {_channel.Host}:{_channel.Port} | Mount: {_channel.MountPath} | Loading...";

        _refreshTimer.Interval = 5000;
        _refreshTimer.Tick += async (s, e) => await LoadListenersAsync();
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

    private async void RemoteListenersDialog_Load(object? sender, EventArgs e)
    {
        await LoadListenersAsync();
        if (chkAutoRefresh.Checked)
        {
            _refreshTimer.Start();
        }
    }

    private async Task LoadListenersAsync()
    {
        btnRefresh.Enabled = false;
        var (allowed, message, listeners) = await _apiClient.GetListenersAsync(_channel.Id);
        btnRefresh.Enabled = true;

        if (!allowed)
        {
            _refreshTimer.Stop();
            MessageBox.Show(message, "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
            return;
        }

        _currentListeners = listeners;
        lblStreamInfo.Text = $"Server: {_channel.Host}:{_channel.Port} | Mount: {_channel.MountPath} | Connected Listeners: {_currentListeners.Count} | Last updated: {DateTime.Now:HH:mm:ss}";

        gridListeners.Rows.Clear();
        foreach (var listener in _currentListeners)
        {
            var idx = gridListeners.Rows.Add();
            var row = gridListeners.Rows[idx];

            row.Cells[colIp.Index].Value = listener.IpAddress;
            row.Cells[colLocation.Index].Value = listener.Location;
            row.Cells[colTime.Index].Value = listener.FormattedConnectedTime;
            row.Cells[colUserAgent.Index].Value = listener.UserAgent;
        }
    }

    private async void BtnRefresh_Click(object? sender, EventArgs e)
    {
        await LoadListenersAsync();
    }

    private void ChkAutoRefresh_CheckedChanged(object? sender, EventArgs e)
    {
        if (chkAutoRefresh.Checked)
        {
            _refreshTimer.Start();
        }
        else
        {
            _refreshTimer.Stop();
        }
    }

    private void BtnCopyIp_Click(object? sender, EventArgs e)
    {
        if (gridListeners.CurrentRow?.Cells[colIp.Index].Value is string ip && !string.IsNullOrWhiteSpace(ip))
        {
            Clipboard.SetText(ip);
            MessageBox.Show($"Copied IP address to clipboard:\n{ip}", "IP Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnCopyAll_Click(object? sender, EventArgs e)
    {
        if (_currentListeners.Count == 0)
        {
            MessageBox.Show("No connected listeners to copy.", "Empty List", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"--- Connected Listeners for {_channel.Name} ({_channel.MountPath}) at {DateTime.Now:yyyy-MM-dd HH:mm:ss} ---");
        foreach (var l in _currentListeners)
        {
            sb.AppendLine($"IP: {l.IpAddress,-18} | Time: {l.FormattedConnectedTime,-10} | Player: {l.UserAgent}");
        }

        Clipboard.SetText(sb.ToString());
        MessageBox.Show($"Copied {_currentListeners.Count} listener records to clipboard!", "Copied All", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void RemoteListenersDialog_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _refreshTimer.Stop();
    }
}
