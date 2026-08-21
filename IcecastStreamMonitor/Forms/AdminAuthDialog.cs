using IcecastStreamMonitor.Services;

namespace IcecastStreamMonitor.Forms;

public partial class AdminAuthDialog : Form
{
    private readonly string _expectedHash;
    private readonly string _expectedPlain;

    public AdminAuthDialog(string masterAdminPassword, string masterAdminPasswordHash = "", string targetItemName = "")
    {
        InitializeComponent();
        _expectedPlain = masterAdminPassword;
        _expectedHash = masterAdminPasswordHash;

        if (!string.IsNullOrWhiteSpace(targetItemName))
        {
            lblPrompt.Text = $"Enter the Master Admin Password to view or edit security credentials for '{targetItemName}':";
        }
    }

    private void ChkShowPassword_CheckedChanged(object? sender, EventArgs e)
    {
        txtAdminPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
    }

    private void TxtAdminPassword_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            BtnUnlock_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
    }

    private void BtnUnlock_Click(object? sender, EventArgs e)
    {
        var entered = txtAdminPassword.Text;

        bool isValid = false;
        if (!string.IsNullOrEmpty(_expectedHash))
        {
            isValid = SecurityHelper.VerifyPassword(entered, _expectedHash);
        }
        else
        {
            isValid = string.Equals(entered, _expectedPlain, StringComparison.Ordinal);
        }

        if (isValid)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            lblError.Text = "Incorrect admin password! Please try again.";
            txtAdminPassword.SelectAll();
            txtAdminPassword.Focus();
        }
    }
}
