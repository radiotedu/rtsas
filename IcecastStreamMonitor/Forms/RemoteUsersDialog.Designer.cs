namespace IcecastStreamMonitor.Forms;

partial class RemoteUsersDialog
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        this.pnlHeader = new System.Windows.Forms.Panel();
        this.lblSubtitle = new System.Windows.Forms.Label();
        this.lblTitle = new System.Windows.Forms.Label();
        this.gridUsers = new System.Windows.Forms.DataGridView();
        this.colUsername = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
        this.colPermissions = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.pnlUserEdit = new System.Windows.Forms.GroupBox();
        this.chkViewAudit = new System.Windows.Forms.CheckBox();
        this.chkManageChannels = new System.Windows.Forms.CheckBox();
        this.chkRefreshStreams = new System.Windows.Forms.CheckBox();
        this.chkViewIPs = new System.Windows.Forms.CheckBox();
        this.chkViewLogs = new System.Windows.Forms.CheckBox();
        this.chkViewStatus = new System.Windows.Forms.CheckBox();
        this.lblPermsTitle = new System.Windows.Forms.Label();
        this.chkUserActive = new System.Windows.Forms.CheckBox();
        this.cboRole = new System.Windows.Forms.ComboBox();
        this.lblRole = new System.Windows.Forms.Label();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.lblPassword = new System.Windows.Forms.Label();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.lblUsername = new System.Windows.Forms.Label();
        this.btnSaveUser = new System.Windows.Forms.Button();
        this.btnDeleteUser = new System.Windows.Forms.Button();
        this.btnNewUser = new System.Windows.Forms.Button();
        this.pnlBottom = new System.Windows.Forms.Panel();
        this.chkEnableServer = new System.Windows.Forms.CheckBox();
        this.numPort = new System.Windows.Forms.NumericUpDown();
        this.lblPort = new System.Windows.Forms.Label();
        this.btnClose = new System.Windows.Forms.Button();
        this.pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridUsers)).BeginInit();
        this.pnlUserEdit.SuspendLayout();
        this.pnlBottom.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
        this.SuspendLayout();
        // 
        // pnlHeader
        // 
        this.pnlHeader.BackColor = System.Drawing.SystemColors.ControlLightLight;
        this.pnlHeader.Controls.Add(this.lblSubtitle);
        this.pnlHeader.Controls.Add(this.lblTitle);
        this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlHeader.Location = new System.Drawing.Point(0, 0);
        this.pnlHeader.Name = "pnlHeader";
        this.pnlHeader.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
        this.pnlHeader.Size = new System.Drawing.Size(784, 55);
        this.pnlHeader.TabIndex = 0;
        // 
        // lblSubtitle
        // 
        this.lblSubtitle.AutoSize = true;
        this.lblSubtitle.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblSubtitle.Location = new System.Drawing.Point(16, 32);
        this.lblSubtitle.Name = "lblSubtitle";
        this.lblSubtitle.Size = new System.Drawing.Size(434, 15);
        this.lblSubtitle.TabIndex = 1;
        this.lblSubtitle.Text = "Configure Remote Control users, roles, and granular access permissions over LAN";
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.Location = new System.Drawing.Point(16, 9);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(315, 20);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "👥 Remote Control Access & User Management";
        // 
        // gridUsers
        // 
        this.gridUsers.AllowUserToAddRows = false;
        this.gridUsers.AllowUserToDeleteRows = false;
        this.gridUsers.AllowUserToResizeRows = false;
        this.gridUsers.BackgroundColor = System.Drawing.SystemColors.Window;
        this.gridUsers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        this.gridUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.gridUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUsername,
            this.colRole,
            this.colActive,
            this.colPermissions});
        this.gridUsers.Location = new System.Drawing.Point(16, 68);
        this.gridUsers.MultiSelect = false;
        this.gridUsers.Name = "gridUsers";
        this.gridUsers.ReadOnly = true;
        this.gridUsers.RowHeadersVisible = false;
        this.gridUsers.RowTemplate.Height = 25;
        this.gridUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.gridUsers.Size = new System.Drawing.Size(400, 310);
        this.gridUsers.TabIndex = 1;
        this.gridUsers.SelectionChanged += new System.EventHandler(this.GridUsers_SelectionChanged);
        // 
        // colUsername
        // 
        this.colUsername.HeaderText = "Username";
        this.colUsername.Name = "colUsername";
        this.colUsername.ReadOnly = true;
        this.colUsername.Width = 105;
        // 
        // colRole
        // 
        this.colRole.HeaderText = "Role";
        this.colRole.Name = "colRole";
        this.colRole.ReadOnly = true;
        this.colRole.Width = 95;
        // 
        // colActive
        // 
        this.colActive.HeaderText = "Active";
        this.colActive.Name = "colActive";
        this.colActive.ReadOnly = true;
        this.colActive.Width = 50;
        // 
        // colPermissions
        // 
        this.colPermissions.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.colPermissions.HeaderText = "Permissions Summary";
        this.colPermissions.Name = "colPermissions";
        this.colPermissions.ReadOnly = true;
        // 
        // pnlUserEdit
        // 
        this.pnlUserEdit.Controls.Add(this.chkViewAudit);
        this.pnlUserEdit.Controls.Add(this.chkManageChannels);
        this.pnlUserEdit.Controls.Add(this.chkRefreshStreams);
        this.pnlUserEdit.Controls.Add(this.chkViewIPs);
        this.pnlUserEdit.Controls.Add(this.chkViewLogs);
        this.pnlUserEdit.Controls.Add(this.chkViewStatus);
        this.pnlUserEdit.Controls.Add(this.lblPermsTitle);
        this.pnlUserEdit.Controls.Add(this.chkUserActive);
        this.pnlUserEdit.Controls.Add(this.cboRole);
        this.pnlUserEdit.Controls.Add(this.lblRole);
        this.pnlUserEdit.Controls.Add(this.txtPassword);
        this.pnlUserEdit.Controls.Add(this.lblPassword);
        this.pnlUserEdit.Controls.Add(this.txtUsername);
        this.pnlUserEdit.Controls.Add(this.lblUsername);
        this.pnlUserEdit.Controls.Add(this.btnSaveUser);
        this.pnlUserEdit.Location = new System.Drawing.Point(426, 68);
        this.pnlUserEdit.Name = "pnlUserEdit";
        this.pnlUserEdit.Size = new System.Drawing.Size(342, 310);
        this.pnlUserEdit.TabIndex = 2;
        this.pnlUserEdit.TabStop = false;
        this.pnlUserEdit.Text = "User Details & Granular RBAC Permissions";
        // 
        // chkViewAudit
        // 
        this.chkViewAudit.AutoSize = true;
        this.chkViewAudit.Location = new System.Drawing.Point(18, 245);
        this.chkViewAudit.Name = "chkViewAudit";
        this.chkViewAudit.Size = new System.Drawing.Size(188, 19);
        this.chkViewAudit.TabIndex = 13;
        this.chkViewAudit.Text = "Can View Audit & Change Logs";
        this.chkViewAudit.UseVisualStyleBackColor = true;
        // 
        // chkManageChannels
        // 
        this.chkManageChannels.AutoSize = true;
        this.chkManageChannels.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.chkManageChannels.ForeColor = System.Drawing.Color.DarkRed;
        this.chkManageChannels.Location = new System.Drawing.Point(18, 222);
        this.chkManageChannels.Name = "chkManageChannels";
        this.chkManageChannels.Size = new System.Drawing.Size(262, 19);
        this.chkManageChannels.TabIndex = 12;
        this.chkManageChannels.Text = "Can Add / Edit / Delete Channels (Admin)";
        this.chkManageChannels.UseVisualStyleBackColor = true;
        // 
        // chkRefreshStreams
        // 
        this.chkRefreshStreams.AutoSize = true;
        this.chkRefreshStreams.Location = new System.Drawing.Point(18, 200);
        this.chkRefreshStreams.Name = "chkRefreshStreams";
        this.chkRefreshStreams.Size = new System.Drawing.Size(168, 19);
        this.chkRefreshStreams.TabIndex = 11;
        this.chkRefreshStreams.Text = "Can Trigger Stream Refresh";
        this.chkRefreshStreams.UseVisualStyleBackColor = true;
        // 
        // chkViewIPs
        // 
        this.chkViewIPs.AutoSize = true;
        this.chkViewIPs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.chkViewIPs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
        this.chkViewIPs.Location = new System.Drawing.Point(18, 178);
        this.chkViewIPs.Name = "chkViewIPs";
        this.chkViewIPs.Size = new System.Drawing.Size(203, 19);
        this.chkViewIPs.TabIndex = 10;
        this.chkViewIPs.Text = "Can View Connected Client IPs";
        this.chkViewIPs.UseVisualStyleBackColor = true;
        // 
        // chkViewLogs
        // 
        this.chkViewLogs.AutoSize = true;
        this.chkViewLogs.Location = new System.Drawing.Point(18, 156);
        this.chkViewLogs.Name = "chkViewLogs";
        this.chkViewLogs.Size = new System.Drawing.Size(225, 19);
        this.chkViewLogs.TabIndex = 9;
        this.chkViewLogs.Text = "Can View 24h Charts && Data Transfer";
        this.chkViewLogs.UseVisualStyleBackColor = true;
        // 
        // chkViewStatus
        // 
        this.chkViewStatus.AutoSize = true;
        this.chkViewStatus.Checked = true;
        this.chkViewStatus.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkViewStatus.Location = new System.Drawing.Point(18, 134);
        this.chkViewStatus.Name = "chkViewStatus";
        this.chkViewStatus.Size = new System.Drawing.Size(211, 19);
        this.chkViewStatus.TabIndex = 8;
        this.chkViewStatus.Text = "Can View Live Stream Status Table";
        this.chkViewStatus.UseVisualStyleBackColor = true;
        // 
        // lblPermsTitle
        // 
        this.lblPermsTitle.AutoSize = true;
        this.lblPermsTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblPermsTitle.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblPermsTitle.Location = new System.Drawing.Point(15, 114);
        this.lblPermsTitle.Name = "lblPermsTitle";
        this.lblPermsTitle.Size = new System.Drawing.Size(155, 13);
        this.lblPermsTitle.TabIndex = 7;
        this.lblPermsTitle.Text = "ACCESS CONTROL CHECKS:";
        // 
        // chkUserActive
        // 
        this.chkUserActive.AutoSize = true;
        this.chkUserActive.Checked = true;
        this.chkUserActive.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkUserActive.Location = new System.Drawing.Point(235, 86);
        this.chkUserActive.Name = "chkUserActive";
        this.chkUserActive.Size = new System.Drawing.Size(89, 19);
        this.chkUserActive.TabIndex = 6;
        this.chkUserActive.Text = "User Active";
        this.chkUserActive.UseVisualStyleBackColor = true;
        // 
        // cboRole
        // 
        this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cboRole.FormattingEnabled = true;
        this.cboRole.Location = new System.Drawing.Point(82, 84);
        this.cboRole.Name = "cboRole";
        this.cboRole.Size = new System.Drawing.Size(145, 23);
        this.cboRole.TabIndex = 5;
        this.cboRole.SelectedIndexChanged += new System.EventHandler(this.CboRole_SelectedIndexChanged);
        // 
        // lblRole
        // 
        this.lblRole.AutoSize = true;
        this.lblRole.Location = new System.Drawing.Point(15, 87);
        this.lblRole.Name = "lblRole";
        this.lblRole.Size = new System.Drawing.Size(33, 15);
        this.lblRole.TabIndex = 4;
        this.lblRole.Text = "Role:";
        // 
        // txtPassword
        // 
        this.txtPassword.Location = new System.Drawing.Point(82, 54);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.Size = new System.Drawing.Size(240, 23);
        this.txtPassword.TabIndex = 3;
        this.txtPassword.UseSystemPasswordChar = true;
        // 
        // lblPassword
        // 
        this.lblPassword.AutoSize = true;
        this.lblPassword.Location = new System.Drawing.Point(15, 57);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new System.Drawing.Size(60, 15);
        this.lblPassword.TabIndex = 2;
        this.lblPassword.Text = "Password:";
        // 
        // txtUsername
        // 
        this.txtUsername.Location = new System.Drawing.Point(82, 24);
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(240, 23);
        this.txtUsername.TabIndex = 1;
        // 
        // lblUsername
        // 
        this.lblUsername.AutoSize = true;
        this.lblUsername.Location = new System.Drawing.Point(15, 27);
        this.lblUsername.Name = "lblUsername";
        this.lblUsername.Size = new System.Drawing.Size(63, 15);
        this.lblUsername.TabIndex = 0;
        this.lblUsername.Text = "Username:";
        // 
        // btnSaveUser
        // 
        this.btnSaveUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnSaveUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnSaveUser.Location = new System.Drawing.Point(232, 272);
        this.btnSaveUser.Name = "btnSaveUser";
        this.btnSaveUser.Size = new System.Drawing.Size(95, 26);
        this.btnSaveUser.TabIndex = 14;
        this.btnSaveUser.Text = "💾 Save User";
        this.btnSaveUser.UseVisualStyleBackColor = true;
        this.btnSaveUser.Click += new System.EventHandler(this.BtnSaveUser_Click);
        // 
        // btnDeleteUser
        // 
        this.btnDeleteUser.Location = new System.Drawing.Point(107, 386);
        this.btnDeleteUser.Name = "btnDeleteUser";
        this.btnDeleteUser.Size = new System.Drawing.Size(85, 26);
        this.btnDeleteUser.TabIndex = 4;
        this.btnDeleteUser.Text = "✕ Delete";
        this.btnDeleteUser.UseVisualStyleBackColor = true;
        this.btnDeleteUser.Click += new System.EventHandler(this.BtnDeleteUser_Click);
        // 
        // btnNewUser
        // 
        this.btnNewUser.Location = new System.Drawing.Point(16, 386);
        this.btnNewUser.Name = "btnNewUser";
        this.btnNewUser.Size = new System.Drawing.Size(85, 26);
        this.btnNewUser.TabIndex = 3;
        this.btnNewUser.Text = "+ Add User";
        this.btnNewUser.UseVisualStyleBackColor = true;
        this.btnNewUser.Click += new System.EventHandler(this.BtnNewUser_Click);
        // 
        // pnlBottom
        // 
        this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.pnlBottom.Controls.Add(this.chkEnableServer);
        this.pnlBottom.Controls.Add(this.numPort);
        this.pnlBottom.Controls.Add(this.lblPort);
        this.pnlBottom.Controls.Add(this.btnNewUser);
        this.pnlBottom.Controls.Add(this.btnDeleteUser);
        this.pnlBottom.Controls.Add(this.btnClose);
        this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnlBottom.Location = new System.Drawing.Point(0, 420);
        this.pnlBottom.Name = "pnlBottom";
        this.pnlBottom.Size = new System.Drawing.Size(784, 50);
        this.pnlBottom.TabIndex = 5;
        // 
        // chkEnableServer
        // 
        this.chkEnableServer.AutoSize = true;
        this.chkEnableServer.Checked = true;
        this.chkEnableServer.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkEnableServer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.chkEnableServer.Location = new System.Drawing.Point(210, 16);
        this.chkEnableServer.Name = "chkEnableServer";
        this.chkEnableServer.Size = new System.Drawing.Size(200, 19);
        this.chkEnableServer.TabIndex = 0;
        this.chkEnableServer.Text = "Enable Remote Control API Server";
        this.chkEnableServer.UseVisualStyleBackColor = true;
        // 
        // numPort
        // 
        this.numPort.Location = new System.Drawing.Point(465, 14);
        this.numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        this.numPort.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
        this.numPort.Name = "numPort";
        this.numPort.Size = new System.Drawing.Size(70, 23);
        this.numPort.TabIndex = 2;
        this.numPort.Value = new decimal(new int[] { 8080, 0, 0, 0 });
        // 
        // lblPort
        // 
        this.lblPort.AutoSize = true;
        this.lblPort.Location = new System.Drawing.Point(425, 17);
        this.lblPort.Name = "lblPort";
        this.lblPort.Size = new System.Drawing.Size(32, 15);
        this.lblPort.TabIndex = 1;
        this.lblPort.Text = "Port:";
        // 
        // btnClose
        // 
        this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnClose.Location = new System.Drawing.Point(693, 11);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(75, 28);
        this.btnClose.TabIndex = 5;
        this.btnClose.Text = "Done";
        this.btnClose.UseVisualStyleBackColor = true;
        this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
        // 
        // RemoteUsersDialog
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(784, 470);
        this.Controls.Add(this.pnlBottom);
        this.Controls.Add(this.pnlUserEdit);
        this.Controls.Add(this.gridUsers);
        this.Controls.Add(this.pnlHeader);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "RemoteUsersDialog";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Remote Control Users & RBAC Permissions";
        this.Load += new System.EventHandler(this.RemoteUsersDialog_Load);
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridUsers)).EndInit();
        this.pnlUserEdit.ResumeLayout(false);
        this.pnlUserEdit.PerformLayout();
        this.pnlBottom.ResumeLayout(false);
        this.pnlBottom.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblSubtitle;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.DataGridView gridUsers;
    private System.Windows.Forms.GroupBox pnlUserEdit;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Label lblUsername;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.ComboBox cboRole;
    private System.Windows.Forms.Label lblRole;
    private System.Windows.Forms.CheckBox chkUserActive;
    private System.Windows.Forms.Label lblPermsTitle;
    private System.Windows.Forms.CheckBox chkViewStatus;
    private System.Windows.Forms.CheckBox chkViewLogs;
    private System.Windows.Forms.CheckBox chkViewIPs;
    private System.Windows.Forms.CheckBox chkRefreshStreams;
    private System.Windows.Forms.CheckBox chkManageChannels;
    private System.Windows.Forms.CheckBox chkViewAudit;
    private System.Windows.Forms.Button btnSaveUser;
    private System.Windows.Forms.Button btnNewUser;
    private System.Windows.Forms.Button btnDeleteUser;
    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.CheckBox chkEnableServer;
    private System.Windows.Forms.NumericUpDown numPort;
    private System.Windows.Forms.Label lblPort;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUsername;
    private System.Windows.Forms.DataGridViewTextBoxColumn colRole;
    private System.Windows.Forms.DataGridViewCheckBoxColumn colActive;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPermissions;
}
