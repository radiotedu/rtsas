using IcecastStreamMonitor.Models;
using IcecastStreamMonitor.Services;

namespace IcecastStreamMonitor.Forms;

public partial class RemoteUsersDialog : Form
{
    private readonly AppSettings _settings;
    private readonly List<RemoteUser> _users;
    private RemoteUser? _selectedUser;

    public RemoteUsersDialog(AppSettings settings)
    {
        InitializeComponent();
        _settings = settings;
        _users = _settings.RemoteUsers;

        cboRole.Items.Add("SuperAdmin");
        cboRole.Items.Add("Operator");
        cboRole.Items.Add("Standard Viewer");
        cboRole.Items.Add("Custom Role");
        cboRole.SelectedIndex = 2;

        chkEnableServer.Checked = _settings.RemoteServerEnabled;
        numPort.Value = Math.Clamp(_settings.RemoteServerPort, 1024, 65535);
    }

    private void RemoteUsersDialog_Load(object? sender, EventArgs e)
    {
        PopulateGrid();
    }

    private void PopulateGrid()
    {
        gridUsers.Rows.Clear();
        foreach (var u in _users)
        {
            var idx = gridUsers.Rows.Add();
            var row = gridUsers.Rows[idx];
            row.Tag = u;

            row.Cells[colUsername.Index].Value = u.Username;
            row.Cells[colRole.Index].Value = u.RoleName;
            row.Cells[colActive.Index].Value = u.IsActive;

            var perms = new List<string>();
            if (u.Permissions.CanViewStatus) perms.Add("Status");
            if (u.Permissions.CanViewLogs) perms.Add("Logs");
            if (u.Permissions.CanViewListenerIps) perms.Add("IPs");
            if (u.Permissions.CanRefreshStreams) perms.Add("Refresh");
            if (u.Permissions.CanManageChannels) perms.Add("Manage");
            if (u.Permissions.CanViewAuditLogs) perms.Add("Audit");

            row.Cells[colPermissions.Index].Value = string.Join(", ", perms);
        }

        if (gridUsers.Rows.Count > 0)
        {
            gridUsers.Rows[0].Selected = true;
            LoadUserToEdit(_users[0]);
        }
    }

    private void GridUsers_SelectionChanged(object? sender, EventArgs e)
    {
        if (gridUsers.CurrentRow?.Tag is RemoteUser u)
        {
            LoadUserToEdit(u);
        }
    }

    private void LoadUserToEdit(RemoteUser u)
    {
        _selectedUser = u;
        txtUsername.Text = u.Username;
        txtPassword.Text = u.PlainPassword;
        chkUserActive.Checked = u.IsActive;

        int roleIdx = cboRole.FindStringExact(u.RoleName);
        cboRole.SelectedIndex = roleIdx >= 0 ? roleIdx : 3;

        chkViewStatus.Checked = u.Permissions.CanViewStatus;
        chkViewLogs.Checked = u.Permissions.CanViewLogs;
        chkViewIPs.Checked = u.Permissions.CanViewListenerIps;
        chkRefreshStreams.Checked = u.Permissions.CanRefreshStreams;
        chkManageChannels.Checked = u.Permissions.CanManageChannels;
        chkViewAudit.Checked = u.Permissions.CanViewAuditLogs;
    }

    private void CboRole_SelectedIndexChanged(object? sender, EventArgs e)
    {
        var role = cboRole.SelectedItem?.ToString();
        if (role == "SuperAdmin")
        {
            chkViewStatus.Checked = true;
            chkViewLogs.Checked = true;
            chkViewIPs.Checked = true;
            chkRefreshStreams.Checked = true;
            chkManageChannels.Checked = true;
            chkViewAudit.Checked = true;
        }
        else if (role == "Operator")
        {
            chkViewStatus.Checked = true;
            chkViewLogs.Checked = true;
            chkViewIPs.Checked = true;
            chkRefreshStreams.Checked = true;
            chkManageChannels.Checked = false;
            chkViewAudit.Checked = false;
        }
        else if (role == "Standard Viewer")
        {
            chkViewStatus.Checked = true;
            chkViewLogs.Checked = true;
            chkViewIPs.Checked = false; // Restricted
            chkRefreshStreams.Checked = true;
            chkManageChannels.Checked = false; // Restricted
            chkViewAudit.Checked = false;
        }
    }

    private void BtnNewUser_Click(object? sender, EventArgs e)
    {
        _selectedUser = null;
        txtUsername.Text = $"user{_users.Count + 1}";
        txtPassword.Text = "pass123";
        cboRole.SelectedIndex = 2; // Viewer
        chkUserActive.Checked = true;

        txtUsername.Focus();
        txtUsername.SelectAll();
    }

    private void BtnSaveUser_Click(object? sender, EventArgs e)
    {
        var uname = txtUsername.Text.Trim();
        var pass = txtPassword.Text;

        if (string.IsNullOrWhiteSpace(uname))
        {
            MessageBox.Show("Username cannot be empty.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_selectedUser == null)
        {
            // Check for duplicate username
            if (_users.Any(u => string.Equals(u.Username, uname, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("A user with this username already exists.", "Duplicate User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newUser = new RemoteUser
            {
                Username = uname,
                PlainPassword = pass,
                PasswordHash = SecurityHelper.HashPassword(pass),
                RoleName = cboRole.SelectedItem?.ToString() ?? "Custom",
                IsActive = chkUserActive.Checked,
                Permissions = new RemotePermissions
                {
                    CanViewStatus = chkViewStatus.Checked,
                    CanViewLogs = chkViewLogs.Checked,
                    CanViewListenerIps = chkViewIPs.Checked,
                    CanRefreshStreams = chkRefreshStreams.Checked,
                    CanManageChannels = chkManageChannels.Checked,
                    CanViewAuditLogs = chkViewAudit.Checked
                }
            };
            _users.Add(newUser);
            _selectedUser = newUser;
        }
        else
        {
            _selectedUser.Username = uname;
            if (!string.IsNullOrEmpty(pass))
            {
                _selectedUser.PlainPassword = pass;
                _selectedUser.PasswordHash = SecurityHelper.HashPassword(pass);
            }
            _selectedUser.RoleName = cboRole.SelectedItem?.ToString() ?? "Custom";
            _selectedUser.IsActive = chkUserActive.Checked;
            _selectedUser.Permissions.CanViewStatus = chkViewStatus.Checked;
            _selectedUser.Permissions.CanViewLogs = chkViewLogs.Checked;
            _selectedUser.Permissions.CanViewListenerIps = chkViewIPs.Checked;
            _selectedUser.Permissions.CanRefreshStreams = chkRefreshStreams.Checked;
            _selectedUser.Permissions.CanManageChannels = chkManageChannels.Checked;
            _selectedUser.Permissions.CanViewAuditLogs = chkViewAudit.Checked;
        }

        PopulateGrid();
        MessageBox.Show($"User '{uname}' saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnDeleteUser_Click(object? sender, EventArgs e)
    {
        if (_selectedUser == null) return;

        if (_users.Count <= 1)
        {
            MessageBox.Show("You cannot delete the last remaining remote user.", "Deletion Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var res = MessageBox.Show($"Are you sure you want to delete remote user '{_selectedUser.Username}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (res == DialogResult.Yes)
        {
            _users.Remove(_selectedUser);
            _selectedUser = null;
            PopulateGrid();
        }
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        _settings.RemoteServerEnabled = chkEnableServer.Checked;
        _settings.RemoteServerPort = (int)numPort.Value;
    }
}
