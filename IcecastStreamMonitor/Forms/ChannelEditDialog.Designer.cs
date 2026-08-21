namespace IcecastStreamMonitor.Forms;

partial class ChannelEditDialog
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
        this.lblTitle = new System.Windows.Forms.Label();
        this.lblName = new System.Windows.Forms.Label();
        this.txtName = new System.Windows.Forms.TextBox();
        this.lblHost = new System.Windows.Forms.Label();
        this.txtHost = new System.Windows.Forms.TextBox();
        this.lblPort = new System.Windows.Forms.Label();
        this.numPort = new System.Windows.Forms.NumericUpDown();
        this.lblMount = new System.Windows.Forms.Label();
        this.txtMount = new System.Windows.Forms.TextBox();
        this.lblServerType = new System.Windows.Forms.Label();
        this.cboServerType = new System.Windows.Forms.ComboBox();
        this.grpAuth = new System.Windows.Forms.GroupBox();
        this.rbDefaultPassword = new System.Windows.Forms.RadioButton();
        this.rbCustomPassword = new System.Windows.Forms.RadioButton();
        this.lblCustomPassword = new System.Windows.Forms.Label();
        this.txtCustomPassword = new System.Windows.Forms.TextBox();
        this.chkShowPassword = new System.Windows.Forms.CheckBox();
        this.chkEnabled = new System.Windows.Forms.CheckBox();
        this.btnTestConnection = new System.Windows.Forms.Button();
        this.lblTestResult = new System.Windows.Forms.Label();
        this.btnSave = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
        this.grpAuth.SuspendLayout();
        this.SuspendLayout();
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.Location = new System.Drawing.Point(18, 16);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(200, 20);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "Channel / Mountpoint Settings";
        // 
        // lblName
        // 
        this.lblName.AutoSize = true;
        this.lblName.Location = new System.Drawing.Point(18, 52);
        this.lblName.Name = "lblName";
        this.lblName.Size = new System.Drawing.Size(89, 15);
        this.lblName.TabIndex = 1;
        this.lblName.Text = "Channel Label:";
        // 
        // txtName
        // 
        this.txtName.Location = new System.Drawing.Point(125, 49);
        this.txtName.Name = "txtName";
        this.txtName.Size = new System.Drawing.Size(325, 23);
        this.txtName.TabIndex = 2;
        // 
        // lblHost
        // 
        this.lblHost.AutoSize = true;
        this.lblHost.Location = new System.Drawing.Point(18, 86);
        this.lblHost.Name = "lblHost";
        this.lblHost.Size = new System.Drawing.Size(88, 15);
        this.lblHost.TabIndex = 3;
        this.lblHost.Text = "Server Host/IP:";
        // 
        // txtHost
        // 
        this.txtHost.Location = new System.Drawing.Point(125, 83);
        this.txtHost.Name = "txtHost";
        this.txtHost.Size = new System.Drawing.Size(200, 23);
        this.txtHost.TabIndex = 4;
        this.txtHost.Text = "127.0.0.1";
        // 
        // lblPort
        // 
        this.lblPort.AutoSize = true;
        this.lblPort.Location = new System.Drawing.Point(335, 86);
        this.lblPort.Name = "lblPort";
        this.lblPort.Size = new System.Drawing.Size(31, 15);
        this.lblPort.TabIndex = 5;
        this.lblPort.Text = "Port:";
        // 
        // numPort
        // 
        this.numPort.Location = new System.Drawing.Point(372, 83);
        this.numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        this.numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numPort.Name = "numPort";
        this.numPort.Size = new System.Drawing.Size(78, 23);
        this.numPort.TabIndex = 6;
        this.numPort.Value = new decimal(new int[] { 8000, 0, 0, 0 });
        // 
        // lblMount
        // 
        this.lblMount.AutoSize = true;
        this.lblMount.Location = new System.Drawing.Point(18, 120);
        this.lblMount.Name = "lblMount";
        this.lblMount.Size = new System.Drawing.Size(73, 15);
        this.lblMount.TabIndex = 7;
        this.lblMount.Text = "Mount Path:";
        // 
        // txtMount
        // 
        this.txtMount.Location = new System.Drawing.Point(125, 117);
        this.txtMount.Name = "txtMount";
        this.txtMount.Size = new System.Drawing.Size(325, 23);
        this.txtMount.TabIndex = 8;
        this.txtMount.Text = "/live";
        // 
        // lblServerType
        // 
        this.lblServerType.AutoSize = true;
        this.lblServerType.Location = new System.Drawing.Point(18, 154);
        this.lblServerType.Name = "lblServerType";
        this.lblServerType.Size = new System.Drawing.Size(70, 15);
        this.lblServerType.TabIndex = 9;
        this.lblServerType.Text = "Server Type:";
        // 
        // cboServerType
        // 
        this.cboServerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cboServerType.FormattingEnabled = true;
        this.cboServerType.Location = new System.Drawing.Point(125, 151);
        this.cboServerType.Name = "cboServerType";
        this.cboServerType.Size = new System.Drawing.Size(325, 23);
        this.cboServerType.TabIndex = 10;
        // 
        // grpAuth
        // 
        this.grpAuth.Controls.Add(this.chkShowPassword);
        this.grpAuth.Controls.Add(this.txtCustomPassword);
        this.grpAuth.Controls.Add(this.lblCustomPassword);
        this.grpAuth.Controls.Add(this.rbCustomPassword);
        this.grpAuth.Controls.Add(this.rbDefaultPassword);
        this.grpAuth.Location = new System.Drawing.Point(21, 186);
        this.grpAuth.Name = "grpAuth";
        this.grpAuth.Size = new System.Drawing.Size(429, 114);
        this.grpAuth.TabIndex = 11;
        this.grpAuth.TabStop = false;
        this.grpAuth.Text = "Authentication (Admin / Stats Password)";
        // 
        // rbDefaultPassword
        // 
        this.rbDefaultPassword.AutoSize = true;
        this.rbDefaultPassword.Checked = true;
        this.rbDefaultPassword.Location = new System.Drawing.Point(16, 26);
        this.rbDefaultPassword.Name = "rbDefaultPassword";
        this.rbDefaultPassword.Size = new System.Drawing.Size(161, 19);
        this.rbDefaultPassword.TabIndex = 0;
        this.rbDefaultPassword.TabStop = true;
        this.rbDefaultPassword.Text = "Use Global Default Password";
        this.rbDefaultPassword.UseVisualStyleBackColor = true;
        this.rbDefaultPassword.CheckedChanged += new System.EventHandler(this.AuthRadio_CheckedChanged);
        // 
        // rbCustomPassword
        // 
        this.rbCustomPassword.AutoSize = true;
        this.rbCustomPassword.Location = new System.Drawing.Point(16, 52);
        this.rbCustomPassword.Name = "rbCustomPassword";
        this.rbCustomPassword.Size = new System.Drawing.Size(176, 19);
        this.rbCustomPassword.TabIndex = 1;
        this.rbCustomPassword.Text = "Specify Custom Password:";
        this.rbCustomPassword.UseVisualStyleBackColor = true;
        this.rbCustomPassword.CheckedChanged += new System.EventHandler(this.AuthRadio_CheckedChanged);
        // 
        // lblCustomPassword
        // 
        this.lblCustomPassword.AutoSize = true;
        this.lblCustomPassword.Location = new System.Drawing.Point(34, 78);
        this.lblCustomPassword.Name = "lblCustomPassword";
        this.lblCustomPassword.Size = new System.Drawing.Size(60, 15);
        this.lblCustomPassword.TabIndex = 2;
        this.lblCustomPassword.Text = "Password:";
        // 
        // txtCustomPassword
        // 
        this.txtCustomPassword.Enabled = false;
        this.txtCustomPassword.Location = new System.Drawing.Point(104, 75);
        this.txtCustomPassword.Name = "txtCustomPassword";
        this.txtCustomPassword.PasswordChar = '•';
        this.txtCustomPassword.Size = new System.Drawing.Size(240, 23);
        this.txtCustomPassword.TabIndex = 3;
        // 
        // chkShowPassword
        // 
        this.chkShowPassword.AutoSize = true;
        this.chkShowPassword.Enabled = false;
        this.chkShowPassword.Location = new System.Drawing.Point(354, 77);
        this.chkShowPassword.Name = "chkShowPassword";
        this.chkShowPassword.Size = new System.Drawing.Size(55, 19);
        this.chkShowPassword.TabIndex = 4;
        this.chkShowPassword.Text = "Show";
        this.chkShowPassword.UseVisualStyleBackColor = true;
        this.chkShowPassword.CheckedChanged += new System.EventHandler(this.ChkShowPassword_CheckedChanged);
        // 
        // chkEnabled
        // 
        this.chkEnabled.AutoSize = true;
        this.chkEnabled.Checked = true;
        this.chkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkEnabled.Location = new System.Drawing.Point(21, 310);
        this.chkEnabled.Name = "chkEnabled";
        this.chkEnabled.Size = new System.Drawing.Size(171, 19);
        this.chkEnabled.TabIndex = 12;
        this.chkEnabled.Text = "Enable monitoring for this feed";
        this.chkEnabled.UseVisualStyleBackColor = true;
        // 
        // btnTestConnection
        // 
        this.btnTestConnection.Location = new System.Drawing.Point(21, 340);
        this.btnTestConnection.Name = "btnTestConnection";
        this.btnTestConnection.Size = new System.Drawing.Size(120, 28);
        this.btnTestConnection.TabIndex = 13;
        this.btnTestConnection.Text = "Test Connection";
        this.btnTestConnection.UseVisualStyleBackColor = true;
        this.btnTestConnection.Click += new System.EventHandler(this.BtnTestConnection_Click);
        // 
        // lblTestResult
        // 
        this.lblTestResult.AutoEllipsis = true;
        this.lblTestResult.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblTestResult.Location = new System.Drawing.Point(150, 344);
        this.lblTestResult.Name = "lblTestResult";
        this.lblTestResult.Size = new System.Drawing.Size(300, 23);
        this.lblTestResult.TabIndex = 14;
        this.lblTestResult.Text = "Status: Not tested yet";
        // 
        // btnSave
        // 
        this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnSave.Location = new System.Drawing.Point(264, 385);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(90, 30);
        this.btnSave.TabIndex = 15;
        this.btnSave.Text = "Save";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point(360, 385);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(90, 30);
        this.btnCancel.TabIndex = 16;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        // 
        // ChannelEditDialog
        // 
        this.AcceptButton = this.btnSave;
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(468, 430);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.lblTestResult);
        this.Controls.Add(this.btnTestConnection);
        this.Controls.Add(this.chkEnabled);
        this.Controls.Add(this.grpAuth);
        this.Controls.Add(this.cboServerType);
        this.Controls.Add(this.lblServerType);
        this.Controls.Add(this.txtMount);
        this.Controls.Add(this.lblMount);
        this.Controls.Add(this.numPort);
        this.Controls.Add(this.lblPort);
        this.Controls.Add(this.txtHost);
        this.Controls.Add(this.lblHost);
        this.Controls.Add(this.txtName);
        this.Controls.Add(this.lblName);
        this.Controls.Add(this.lblTitle);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ChannelEditDialog";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Mountpoint Configuration";
        ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
        this.grpAuth.ResumeLayout(false);
        this.grpAuth.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.Label lblHost;
    private System.Windows.Forms.TextBox txtHost;
    private System.Windows.Forms.Label lblPort;
    private System.Windows.Forms.NumericUpDown numPort;
    private System.Windows.Forms.Label lblMount;
    private System.Windows.Forms.TextBox txtMount;
    private System.Windows.Forms.Label lblServerType;
    private System.Windows.Forms.ComboBox cboServerType;
    private System.Windows.Forms.GroupBox grpAuth;
    private System.Windows.Forms.RadioButton rbDefaultPassword;
    private System.Windows.Forms.RadioButton rbCustomPassword;
    private System.Windows.Forms.Label lblCustomPassword;
    private System.Windows.Forms.TextBox txtCustomPassword;
    private System.Windows.Forms.CheckBox chkShowPassword;
    private System.Windows.Forms.CheckBox chkEnabled;
    private System.Windows.Forms.Button btnTestConnection;
    private System.Windows.Forms.Label lblTestResult;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
}
