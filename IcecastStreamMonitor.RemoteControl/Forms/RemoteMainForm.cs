using System.Reflection;
using IcecastStreamMonitor.RemoteControl.Models;
using IcecastStreamMonitor.RemoteControl.Services;

namespace IcecastStreamMonitor.RemoteControl.Forms;

public partial class RemoteMainForm : Form
{
    private readonly RemoteApiClient _apiClient;
    private readonly RemoteUserSession _session;
    private readonly System.Windows.Forms.Timer _pollTimer = new();
    private List<RemoteStreamItem> _streams = new();
    private bool _isUpdating = false;

    public RemoteMainForm(RemoteApiClient apiClient, RemoteUserSession session)
    {
        InitializeComponent();
        _apiClient = apiClient;
        _session = session;

        EnableDoubleBuffering(gridStreams);

        lblUserBadge.Text = $"👤 {_session.Username} (Role: {_session.Role})";
        statusLabelServer.Text = $"Connected to {_apiClient.BaseUrl} | Role: {_session.Role}";

        // Configure capabilities based on granular RBAC permissions
        btnToolAnalytics.Enabled = _session.Permissions.CanViewLogs;
        menuItemAnalytics.Enabled = _session.Permissions.CanViewLogs;

        btnToolListeners.Enabled = _session.Permissions.CanViewListenerIps;
        menuItemListeners.Enabled = _session.Permissions.CanViewListenerIps;

        btnToolRefresh.Enabled = _session.Permissions.CanRefreshStreams;
        menuItemRefresh.Enabled = _session.Permissions.CanRefreshStreams;

        btnToolAddChannel.Visible = _session.Permissions.CanManageChannels;
        btnToolDeleteChannel.Visible = _session.Permissions.CanManageChannels;
        menuItemDelete.Visible = _session.Permissions.CanManageChannels;

        _pollTimer.Interval = 5000;
        _pollTimer.Tick += async (s, e) => await RefreshDataAsync(isAuto: true);
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

    private async void RemoteMainForm_Load(object? sender, EventArgs e)
    {
        await RefreshDataAsync(isAuto: false);
        if (btnToolAutoRefresh.Checked)
        {
            _pollTimer.Start();
        }
    }

    private async Task RefreshDataAsync(bool isAuto)
    {
        if (_isUpdating) return;
        _isUpdating = true;

        if (!isAuto) btnToolRefresh.Enabled = false;

        try
        {
            _streams = await _apiClient.GetStreamsAsync();
            UpdateGrid();
        }
        catch { }
        finally
        {
            _isUpdating = false;
            if (!isAuto) btnToolRefresh.Enabled = _session.Permissions.CanRefreshStreams;
        }
    }

    private void UpdateGrid()
    {
        var selectedId = GetSelectedChannel()?.Id;

        gridStreams.Rows.Clear();
        int onlineCount = 0;
        int totalListeners = 0;

        foreach (var stream in _streams)
        {
            var idx = gridStreams.Rows.Add();
            var row = gridStreams.Rows[idx];
            row.Tag = stream;

            if (stream.IsOnline)
            {
                onlineCount++;
                totalListeners += stream.ConcurrentListeners;
                row.Cells[colStatus.Index].Value = "ONLINE";
                row.Cells[colStatus.Index].Style.ForeColor = Color.DarkGreen;
                row.Cells[colStatus.Index].Style.Font = new Font(gridStreams.Font, FontStyle.Bold);
            }
            else
            {
                row.Cells[colStatus.Index].Value = "OFFLINE";
                row.Cells[colStatus.Index].Style.ForeColor = Color.Firebrick;
            }

            row.Cells[colName.Index].Value = stream.Name;
            row.Cells[colMount.Index].Value = stream.MountPath;
            row.Cells[colListeners.Index].Value = stream.IsOnline ? stream.ConcurrentListeners.ToString("N0") : "-";
            row.Cells[colPeak.Index].Value = stream.IsOnline ? stream.PeakListeners.ToString("N0") : "-";
            row.Cells[colBitrate.Index].Value = stream.IsOnline && stream.BitRateKbps > 0 ? $"{stream.BitRateKbps}k" : "-";
            row.Cells[colFormat.Index].Value = stream.IsOnline && !string.IsNullOrEmpty(stream.AudioFormat) ? stream.AudioFormat : "-";
            row.Cells[colLatency.Index].Value = stream.IsOnline && stream.LatencyMs > 0 ? $"{stream.LatencyMs}ms" : "-";
            row.Cells[colTitle.Index].Value = stream.IsOnline && !string.IsNullOrEmpty(stream.StreamTitle) ? stream.StreamTitle : "(No metadata)";

            if (selectedId.HasValue && stream.Id == selectedId.Value)
            {
                row.Selected = true;
            }
        }

        statusLabelSummary.Text = $"{onlineCount}/{_streams.Count} Channels Online | {totalListeners:N0} Total Listeners";
    }

    private RemoteStreamItem? GetSelectedChannel()
    {
        return gridStreams.CurrentRow?.Tag as RemoteStreamItem;
    }

    private async void BtnRefresh_Click(object? sender, EventArgs e)
    {
        await _apiClient.TriggerRefreshAsync();
        await RefreshDataAsync(isAuto: false);
    }

    private void BtnAutoRefresh_Click(object? sender, EventArgs e)
    {
        if (btnToolAutoRefresh.Checked)
        {
            _pollTimer.Start();
        }
        else
        {
            _pollTimer.Stop();
        }
    }

    private void BtnAnalytics_Click(object? sender, EventArgs e)
    {
        if (!_session.Permissions.CanViewLogs)
        {
            MessageBox.Show("Your user account does not have permission to view 24h logs & charts.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var channel = GetSelectedChannel();
        if (channel == null)
        {
            MessageBox.Show("Please select a stream channel first.", "Select Stream", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new RemoteAnalyticsDialog(channel, _apiClient);
        dlg.ShowDialog(this);
    }

    private void BtnListeners_Click(object? sender, EventArgs e)
    {
        if (!_session.Permissions.CanViewListenerIps)
        {
            MessageBox.Show("Access Denied: Your account role does not have permission to view connected client IPs.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var channel = GetSelectedChannel();
        if (channel == null)
        {
            MessageBox.Show("Please select a stream channel first.", "Select Stream", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new RemoteListenersDialog(channel, _apiClient);
        dlg.ShowDialog(this);
    }

    private async void BtnAddChannel_Click(object? sender, EventArgs e)
    {
        if (!_session.Permissions.CanManageChannels)
        {
            MessageBox.Show("SuperAdmin permissions required to add channels remotely.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Quick input dialog prompt
        var name = Microsoft.VisualBasic.Interaction.InputBox("Enter Channel Name / Label:", "Add Remote Channel", "New Live Feed");
        if (string.IsNullOrWhiteSpace(name)) return;

        var mount = Microsoft.VisualBasic.Interaction.InputBox("Enter Mount Path (e.g. /rock, /dj):", "Add Remote Channel", "/stream");
        if (string.IsNullOrWhiteSpace(mount)) return;

        var success = await _apiClient.AddChannelAsync(name, mount, "127.0.0.1", 8000, 0);
        if (success)
        {
            await RefreshDataAsync(isAuto: false);
            MessageBox.Show($"Channel '{name}' added successfully on server!", "Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private async void BtnDeleteChannel_Click(object? sender, EventArgs e)
    {
        if (!_session.Permissions.CanManageChannels)
        {
            MessageBox.Show("SuperAdmin permissions required to delete channels remotely.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var channel = GetSelectedChannel();
        if (channel == null) return;

        var res = MessageBox.Show($"Are you sure you want to delete '{channel.Name}' ({channel.MountPath}) on the host server?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (res == DialogResult.Yes)
        {
            var success = await _apiClient.DeleteChannelAsync(channel.Id);
            if (success)
            {
                await RefreshDataAsync(isAuto: false);
            }
        }
    }

    private void GridStreams_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            if (_session.Permissions.CanViewLogs)
            {
                BtnAnalytics_Click(sender, e);
            }
            else if (_session.Permissions.CanViewListenerIps)
            {
                BtnListeners_Click(sender, e);
            }
        }
    }

    private void RemoteMainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _pollTimer.Stop();
    }
}
