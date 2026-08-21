namespace IcecastStreamMonitor.Forms;

partial class GlobalSettingsDialog
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
        this.lblAdminPassword = new System.Windows.Forms.Label();
        this.txtAdminPassword = new System.Windows.Forms.TextBox();
        this.chkShowAdminPassword = new System.Windows.Forms.CheckBox();
        this.lblDefaultPassword = new System.Windows.Forms.Label();
        this.txtDefaultPassword = new System.Windows.Forms.TextBox();
        this.chkShowPassword = new System.Windows.Forms.CheckBox();
        this.lblPollingInterval = new System.Windows.Forms.Label();
        this.numPollingInterval = new System.Windows.Forms.NumericUpDown();
        this.lblSeconds = new System.Windows.Forms.Label();
        this.lblTimeout = new System.Windows.Forms.Label();
        this.numTimeout = new System.Windows.Forms.NumericUpDown();
        this.lblTimeoutSec = new System.Windows.Forms.Label();
        this.grpBackground = new System.Windows.Forms.GroupBox();
        this.chkNotifyOffline = new System.Windows.Forms.CheckBox();
        this.chkCloseToTray = new System.Windows.Forms.CheckBox();
        this.chkMinimizeToTray = new System.Windows.Forms.CheckBox();
        this.btnSave = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.lblDesc = new System.Windows.Forms.Label();
        ((System.ComponentModel.ISupportInitialize)(this.numPollingInterval)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
        this.grpBackground.SuspendLayout();
        this.SuspendLayout();
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.Location = new System.Drawing.Point(18, 16);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(182, 20);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "Global Monitor Settings";
        // 
        // lblAdminPassword
        // 
        this.lblAdminPassword.AutoSize = true;
        this.lblAdminPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblAdminPassword.Location = new System.Drawing.Point(18, 52);
        this.lblAdminPassword.Name = "lblAdminPassword";
        this.lblAdminPassword.Size = new System.Drawing.Size(142, 15);
        this.lblAdminPassword.TabIndex = 1;
        this.lblAdminPassword.Text = "Master Admin Password:";
        // 
        // txtAdminPassword
        // 
        this.txtAdminPassword.Location = new System.Drawing.Point(165, 49);
        this.txtAdminPassword.Name = "txtAdminPassword";
        this.txtAdminPassword.PasswordChar = '•';
        this.txtAdminPassword.Size = new System.Drawing.Size(180, 23);
        this.txtAdminPassword.TabIndex = 2;
        // 
        // chkShowAdminPassword
        // 
        this.chkShowAdminPassword.AutoSize = true;
        this.chkShowAdminPassword.Location = new System.Drawing.Point(355, 51);
        this.chkShowAdminPassword.Name = "chkShowAdminPassword";
        this.chkShowAdminPassword.Size = new System.Drawing.Size(55, 19);
        this.chkShowAdminPassword.TabIndex = 3;
        this.chkShowAdminPassword.Text = "Show";
        this.chkShowAdminPassword.UseVisualStyleBackColor = true;
        this.chkShowAdminPassword.CheckedChanged += new System.EventHandler(this.ChkShowAdminPassword_CheckedChanged);
        // 
        // lblDefaultPassword
        // 
        this.lblDefaultPassword.AutoSize = true;
        this.lblDefaultPassword.Location = new System.Drawing.Point(18, 85);
        this.lblDefaultPassword.Name = "lblDefaultPassword";
        this.lblDefaultPassword.Size = new System.Drawing.Size(133, 15);
        this.lblDefaultPassword.TabIndex = 4;
        this.lblDefaultPassword.Text = "Default Stream Password:";
        // 
        // txtDefaultPassword
        // 
        this.txtDefaultPassword.Location = new System.Drawing.Point(165, 82);
        this.txtDefaultPassword.Name = "txtDefaultPassword";
        this.txtDefaultPassword.PasswordChar = '•';
        this.txtDefaultPassword.Size = new System.Drawing.Size(180, 23);
        this.txtDefaultPassword.TabIndex = 5;
        // 
        // chkShowPassword
        // 
        this.chkShowPassword.AutoSize = true;
        this.chkShowPassword.Location = new System.Drawing.Point(355, 84);
        this.chkShowPassword.Name = "chkShowPassword";
        this.chkShowPassword.Size = new System.Drawing.Size(55, 19);
        this.chkShowPassword.TabIndex = 6;
        this.chkShowPassword.Text = "Show";
        this.chkShowPassword.UseVisualStyleBackColor = true;
        this.chkShowPassword.CheckedChanged += new System.EventHandler(this.ChkShowPassword_CheckedChanged);
        // 
        // lblPollingInterval
        // 
        this.lblPollingInterval.AutoSize = true;
        this.lblPollingInterval.Location = new System.Drawing.Point(18, 118);
        this.lblPollingInterval.Name = "lblPollingInterval";
        this.lblPollingInterval.Size = new System.Drawing.Size(126, 15);
        this.lblPollingInterval.TabIndex = 7;
        this.lblPollingInterval.Text = "Auto-Refresh Interval:";
        // 
        // numPollingInterval
        // 
        this.numPollingInterval.Location = new System.Drawing.Point(165, 116);
        this.numPollingInterval.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
        this.numPollingInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numPollingInterval.Name = "numPollingInterval";
        this.numPollingInterval.Size = new System.Drawing.Size(80, 23);
        this.numPollingInterval.TabIndex = 8;
        this.numPollingInterval.Value = new decimal(new int[] { 5, 0, 0, 0 });
        // 
        // lblSeconds
        // 
        this.lblSeconds.AutoSize = true;
        this.lblSeconds.Location = new System.Drawing.Point(255, 118);
        this.lblSeconds.Name = "lblSeconds";
        this.lblSeconds.Size = new System.Drawing.Size(50, 15);
        this.lblSeconds.TabIndex = 9;
        this.lblSeconds.Text = "seconds";
        // 
        // lblTimeout
        // 
        this.lblTimeout.AutoSize = true;
        this.lblTimeout.Location = new System.Drawing.Point(18, 151);
        this.lblTimeout.Name = "lblTimeout";
        this.lblTimeout.Size = new System.Drawing.Size(102, 15);
        this.lblTimeout.TabIndex = 10;
        this.lblTimeout.Text = "Network Timeout:";
        // 
        // numTimeout
        // 
        this.numTimeout.Location = new System.Drawing.Point(165, 149);
        this.numTimeout.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
        this.numTimeout.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numTimeout.Name = "numTimeout";
        this.numTimeout.Size = new System.Drawing.Size(80, 23);
        this.numTimeout.TabIndex = 11;
        this.numTimeout.Value = new decimal(new int[] { 4, 0, 0, 0 });
        // 
        // lblTimeoutSec
        // 
        this.lblTimeoutSec.AutoSize = true;
        this.lblTimeoutSec.Location = new System.Drawing.Point(255, 151);
        this.lblTimeoutSec.Name = "lblTimeoutSec";
        this.lblTimeoutSec.Size = new System.Drawing.Size(50, 15);
        this.lblTimeoutSec.TabIndex = 12;
        this.lblTimeoutSec.Text = "seconds";
        // 
        // grpBackground
        // 
        this.grpBackground.Controls.Add(this.chkNotifyOffline);
        this.grpBackground.Controls.Add(this.chkCloseToTray);
        this.grpBackground.Controls.Add(this.chkMinimizeToTray);
        this.grpBackground.Location = new System.Drawing.Point(21, 185);
        this.grpBackground.Name = "grpBackground";
        this.grpBackground.Size = new System.Drawing.Size(389, 110);
        this.grpBackground.TabIndex = 13;
        this.grpBackground.TabStop = false;
        this.grpBackground.Text = "Background Operation & Notifications";
        this.grpBackground.TabStop = false;
        this.grpBackground.Text = "Background Operation & Notifications";
        // 
        // chkNotifyOffline
        // 
        this.chkNotifyOffline.AutoSize = true;
        this.chkNotifyOffline.Checked = true;
        this.chkNotifyOffline.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkNotifyOffline.Location = new System.Drawing.Point(16, 76);
        this.chkNotifyOffline.Name = "chkNotifyOffline";
        this.chkNotifyOffline.Size = new System.Drawing.Size(315, 19);
        this.chkNotifyOffline.TabIndex = 2;
        this.chkNotifyOffline.Text = "Show balloon alert if an active stream drops OFFLINE";
        this.chkNotifyOffline.UseVisualStyleBackColor = true;
        // 
        // chkCloseToTray
        // 
        this.chkCloseToTray.AutoSize = true;
        this.chkCloseToTray.Location = new System.Drawing.Point(16, 50);
        this.chkCloseToTray.Name = "chkCloseToTray";
        this.chkCloseToTray.Size = new System.Drawing.Size(325, 19);
        this.chkCloseToTray.TabIndex = 1;
        this.chkCloseToTray.Text = "Keep running in background when window is closed (X)";
        this.chkCloseToTray.UseVisualStyleBackColor = true;
        // 
        // chkMinimizeToTray
        // 
        this.chkMinimizeToTray.AutoSize = true;
        this.chkMinimizeToTray.Checked = true;
        this.chkMinimizeToTray.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkMinimizeToTray.Location = new System.Drawing.Point(16, 24);
        this.chkMinimizeToTray.Name = "chkMinimizeToTray";
        this.chkMinimizeToTray.Size = new System.Drawing.Size(262, 19);
        this.chkMinimizeToTray.TabIndex = 0;
        this.chkMinimizeToTray.Text = "Minimize to System Tray (Notification Area)";
        this.chkMinimizeToTray.UseVisualStyleBackColor = true;
        // 
        // btnSave
        // 
        this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnSave.Location = new System.Drawing.Point(232, 320);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(85, 30);
        this.btnSave.TabIndex = 11;
        this.btnSave.Text = "Save";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point(325, 320);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(85, 30);
        this.btnCancel.TabIndex = 12;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        // 
        // lblDesc
        // 
        this.lblDesc.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblDesc.Location = new System.Drawing.Point(18, 278);
        this.lblDesc.Name = "lblDesc";
        this.lblDesc.Size = new System.Drawing.Size(392, 32);
        this.lblDesc.TabIndex = 13;
        this.lblDesc.Text = "Channels configured to 'Use Default Password' will authenticate with this passwor" +
    "d when querying server stats.";
        // 
        // GlobalSettingsDialog
        // 
        this.AcceptButton = this.btnSave;
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(426, 362);
        this.Controls.Add(this.chkShowAdminPassword);
        this.Controls.Add(this.txtAdminPassword);
        this.Controls.Add(this.lblAdminPassword);
        this.Controls.Add(this.lblDesc);
        this.Controls.Add(this.grpBackground);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.lblTimeoutSec);
        this.Controls.Add(this.numTimeout);
        this.Controls.Add(this.lblTimeout);
        this.Controls.Add(this.lblSeconds);
        this.Controls.Add(this.numPollingInterval);
        this.Controls.Add(this.lblPollingInterval);
        this.Controls.Add(this.chkShowPassword);
        this.Controls.Add(this.txtDefaultPassword);
        this.Controls.Add(this.lblDefaultPassword);
        this.Controls.Add(this.lblTitle);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "GlobalSettingsDialog";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Monitor Settings";
        ((System.ComponentModel.ISupportInitialize)(this.numPollingInterval)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
        this.grpBackground.ResumeLayout(false);
        this.grpBackground.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblAdminPassword;
    private System.Windows.Forms.TextBox txtAdminPassword;
    private System.Windows.Forms.CheckBox chkShowAdminPassword;
    private System.Windows.Forms.Label lblDefaultPassword;
    private System.Windows.Forms.TextBox txtDefaultPassword;
    private System.Windows.Forms.CheckBox chkShowPassword;
    private System.Windows.Forms.Label lblPollingInterval;
    private System.Windows.Forms.NumericUpDown numPollingInterval;
    private System.Windows.Forms.Label lblSeconds;
    private System.Windows.Forms.Label lblTimeout;
    private System.Windows.Forms.NumericUpDown numTimeout;
    private System.Windows.Forms.Label lblTimeoutSec;
    private System.Windows.Forms.GroupBox grpBackground;
    private System.Windows.Forms.CheckBox chkNotifyOffline;
    private System.Windows.Forms.CheckBox chkCloseToTray;
    private System.Windows.Forms.CheckBox chkMinimizeToTray;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblDesc;
}
