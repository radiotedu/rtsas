using IcecastStreamMonitor.Models;
using IcecastStreamMonitor.Services;

namespace IcecastStreamMonitor.Forms;

public partial class GlobalSettingsDialog : Form
{
    private readonly AppSettings _settings;

    public AppSettings Settings => _settings;

    public GlobalSettingsDialog(AppSettings settings)
    {
        InitializeComponent();
        _settings = settings;

        try
        {
            var appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (appIcon != null) Icon = appIcon;
        }
        catch { }

        txtAdminPassword.Text = _settings.MasterAdminPassword;
        txtDefaultPassword.Text = _settings.DefaultPassword;
        txtPublicDomain.Text = _settings.PublicDomain;
        numPollingInterval.Value = Math.Clamp(_settings.PollingIntervalSeconds, 1, 300);
        numTimeout.Value = Math.Clamp(_settings.RequestTimeoutSeconds, 1, 30);
        chkMinimizeToTray.Checked = _settings.MinimizeToTray;
        chkCloseToTray.Checked = _settings.CloseToTray;
        chkNotifyOffline.Checked = _settings.NotifyOnStreamOffline;
        chkDarkMode.Checked = _settings.DarkModeEnabled;

        ThemeService.ApplyTheme(this, _settings.DarkModeEnabled);
    }

    private void ChkShowAdminPassword_CheckedChanged(object? sender, EventArgs e)
    {
        txtAdminPassword.PasswordChar = chkShowAdminPassword.Checked ? '\0' : '•';
    }

    private void ChkShowPassword_CheckedChanged(object? sender, EventArgs e)
    {
        txtDefaultPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '•';
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        var newAdminPass = txtAdminPassword.Text;
        if (!string.IsNullOrWhiteSpace(newAdminPass))
        {
            _settings.MasterAdminPassword = newAdminPass;
            _settings.MasterAdminPasswordHash = SecurityHelper.HashPassword(newAdminPass);
        }

        _settings.DefaultPassword = txtDefaultPassword.Text;
        _settings.PublicDomain = string.IsNullOrWhiteSpace(txtPublicDomain.Text) ? "stream.radiotedu.com" : txtPublicDomain.Text.Trim();
        _settings.PollingIntervalSeconds = (int)numPollingInterval.Value;
        _settings.RequestTimeoutSeconds = (int)numTimeout.Value;
        _settings.MinimizeToTray = chkMinimizeToTray.Checked;
        _settings.CloseToTray = chkCloseToTray.Checked;
        _settings.NotifyOnStreamOffline = chkNotifyOffline.Checked;
        _settings.DarkModeEnabled = chkDarkMode.Checked;
    }
}
