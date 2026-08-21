using IcecastStreamMonitor.RemoteControl.Services;

namespace IcecastStreamMonitor.RemoteControl.Forms;

public partial class RemoteLoginForm : Form
{
    private readonly RemoteApiClient _apiClient;

    public RemoteLoginForm()
    {
        InitializeComponent();
        _apiClient = new RemoteApiClient();
    }

    private void ChkShowPassword_CheckedChanged(object? sender, EventArgs e)
    {
        txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
    }

    private async void BtnLogin_Click(object? sender, EventArgs e)
    {
        var host = txtServerHost.Text.Trim();
        var port = (int)numPort.Value;
        var uname = txtUsername.Text.Trim();
        var pass = txtPassword.Text;

        if (string.IsNullOrWhiteSpace(host))
        {
            lblStatus.Text = "Please enter server IP or Hostname.";
            return;
        }

        if (string.IsNullOrWhiteSpace(uname))
        {
            lblStatus.Text = "Please enter username.";
            return;
        }

        btnLogin.Enabled = false;
        lblStatus.ForeColor = Color.DarkSlateGray;
        lblStatus.Text = "Connecting to server...";

        var (success, error, session) = await _apiClient.LoginAsync(host, port, uname, pass);
        btnLogin.Enabled = true;

        if (!success || session == null)
        {
            lblStatus.ForeColor = Color.Firebrick;
            lblStatus.Text = error;
            return;
        }

        // Login success - Open Dashboard
        Hide();
        using var dash = new RemoteMainForm(_apiClient, session);
        dash.ShowDialog();
        Close();
    }

    private void BtnExit_Click(object? sender, EventArgs e)
    {
        Application.Exit();
    }
}
