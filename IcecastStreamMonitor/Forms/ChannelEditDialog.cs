using IcecastStreamMonitor.Models;
using IcecastStreamMonitor.Services;

namespace IcecastStreamMonitor.Forms;

public partial class ChannelEditDialog : Form
{
    private readonly StreamChannel _channel;
    private readonly string _defaultPassword;
    private readonly IStreamMonitorService _monitorService;

    public StreamChannel Channel => _channel;

    public ChannelEditDialog(StreamChannel channel, string defaultPassword, IStreamMonitorService monitorService)
    {
        InitializeComponent();
        _channel = channel.Clone();
        _defaultPassword = defaultPassword;
        _monitorService = monitorService;

        PopulateServerTypes();
        LoadChannelData();
    }

    private void PopulateServerTypes()
    {
        cboServerType.Items.Clear();
        cboServerType.Items.Add(new ServerTypeItem(ServerType.Auto, "Auto-Detect (Icecast / SHOUTcast / ICY)"));
        cboServerType.Items.Add(new ServerTypeItem(ServerType.Icecast, "Icecast 2 (JSON / Admin XML / ICY)"));
        cboServerType.Items.Add(new ServerTypeItem(ServerType.ShoutcastV2, "SHOUTcast v2 (JSON / Stats)"));
        cboServerType.Items.Add(new ServerTypeItem(ServerType.ShoutcastV1, "SHOUTcast v1 (7.html / CSV)"));
        cboServerType.Items.Add(new ServerTypeItem(ServerType.DirectStream, "Direct ICY Stream Header Probe"));
    }

    private void LoadChannelData()
    {
        txtName.Text = _channel.Name;
        txtHost.Text = _channel.Host;
        numPort.Value = Math.Clamp(_channel.Port, 1, 65535);
        txtMount.Text = _channel.MountPath;
        chkEnabled.Checked = _channel.Enabled;

        // Server type
        for (int i = 0; i < cboServerType.Items.Count; i++)
        {
            if (cboServerType.Items[i] is ServerTypeItem item && item.Value == _channel.ServerType)
            {
                cboServerType.SelectedIndex = i;
                break;
            }
        }
        if (cboServerType.SelectedIndex < 0)
            cboServerType.SelectedIndex = 0;

        // Password mode
        if (_channel.UseDefaultPassword)
        {
            rbDefaultPassword.Checked = true;
            txtCustomPassword.Text = string.Empty;
            txtCustomPassword.Enabled = false;
            chkShowPassword.Enabled = false;
        }
        else
        {
            rbCustomPassword.Checked = true;
            txtCustomPassword.Text = _channel.CustomPassword;
            txtCustomPassword.Enabled = true;
            chkShowPassword.Enabled = true;
        }
    }

    private void AuthRadio_CheckedChanged(object? sender, EventArgs e)
    {
        txtCustomPassword.Enabled = rbCustomPassword.Checked;
        chkShowPassword.Enabled = rbCustomPassword.Checked;
    }

    private void ChkShowPassword_CheckedChanged(object? sender, EventArgs e)
    {
        txtCustomPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '•';
    }

    private async void BtnTestConnection_Click(object? sender, EventArgs e)
    {
        btnTestConnection.Enabled = false;
        lblTestResult.Text = "Testing connection...";
        lblTestResult.ForeColor = Color.DarkGoldenrod;

        var selectedType = (cboServerType.SelectedItem as ServerTypeItem)?.Value ?? ServerType.Auto;
        var tempChannel = new StreamChannel
        {
            Host = txtHost.Text.Trim(),
            Port = (int)numPort.Value,
            MountPath = txtMount.Text.Trim(),
            ServerType = selectedType,
            UseDefaultPassword = rbDefaultPassword.Checked,
            CustomPassword = txtCustomPassword.Text
        };

        try
        {
            var status = await _monitorService.CheckStreamAsync(
                tempChannel,
                _defaultPassword,
                TimeSpan.FromSeconds(5));

            if (status.IsOnline)
            {
                var br = status.BitRateKbps > 0 ? $"{status.BitRateKbps}k" : "N/A";
                lblTestResult.Text = $"✓ Online! {status.AudioFormat} @ {br} | {status.ConcurrentListeners} listeners ({status.LatencyMs}ms)";
                lblTestResult.ForeColor = Color.DarkGreen;
            }
            else
            {
                lblTestResult.Text = $"✗ Offline / Error: {status.StatusMessage}";
                lblTestResult.ForeColor = Color.Firebrick;
            }
        }
        catch (Exception ex)
        {
            lblTestResult.Text = $"✗ Error: {ex.Message}";
            lblTestResult.ForeColor = Color.Firebrick;
        }
        finally
        {
            btnTestConnection.Enabled = true;
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtHost.Text))
        {
            MessageBox.Show("Server host or IP cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtHost.Focus();
            DialogResult = DialogResult.None;
            return;
        }

        if (string.IsNullOrWhiteSpace(txtMount.Text))
        {
            MessageBox.Show("Mount path cannot be empty (e.g. /live or 1).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtMount.Focus();
            DialogResult = DialogResult.None;
            return;
        }

        _channel.Name = string.IsNullOrWhiteSpace(txtName.Text) ? txtMount.Text.Trim() : txtName.Text.Trim();
        _channel.Host = txtHost.Text.Trim();
        _channel.Port = (int)numPort.Value;
        _channel.MountPath = txtMount.Text.Trim();
        _channel.ServerType = (cboServerType.SelectedItem as ServerTypeItem)?.Value ?? ServerType.Auto;
        _channel.UseDefaultPassword = rbDefaultPassword.Checked;
        _channel.CustomPassword = rbCustomPassword.Checked ? txtCustomPassword.Text : string.Empty;
        _channel.Enabled = chkEnabled.Checked;
    }

    private record ServerTypeItem(ServerType Value, string DisplayName)
    {
        public override string ToString() => DisplayName;
    }
}
