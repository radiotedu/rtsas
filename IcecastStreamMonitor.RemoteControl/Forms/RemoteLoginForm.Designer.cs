namespace IcecastStreamMonitor.RemoteControl.Forms;

partial class RemoteLoginForm
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
        this.lblServerHost = new System.Windows.Forms.Label();
        this.txtServerHost = new System.Windows.Forms.TextBox();
        this.lblPort = new System.Windows.Forms.Label();
        this.numPort = new System.Windows.Forms.NumericUpDown();
        this.lblUsername = new System.Windows.Forms.Label();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.lblPassword = new System.Windows.Forms.Label();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.chkShowPassword = new System.Windows.Forms.CheckBox();
        this.lblStatus = new System.Windows.Forms.Label();
        this.btnLogin = new System.Windows.Forms.Button();
        this.btnExit = new System.Windows.Forms.Button();
        this.pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
        this.SuspendLayout();
        // 
        // pnlHeader
        // 
        this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
        this.pnlHeader.Controls.Add(this.lblSubtitle);
        this.pnlHeader.Controls.Add(this.lblTitle);
        this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlHeader.Location = new System.Drawing.Point(0, 0);
        this.pnlHeader.Name = "pnlHeader";
        this.pnlHeader.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
        this.pnlHeader.Size = new System.Drawing.Size(384, 60);
        this.pnlHeader.TabIndex = 0;
        // 
        // lblSubtitle
        // 
        this.lblSubtitle.AutoSize = true;
        this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
        this.lblSubtitle.Location = new System.Drawing.Point(16, 34);
        this.lblSubtitle.Name = "lblSubtitle";
        this.lblSubtitle.Size = new System.Drawing.Size(262, 15);
        this.lblSubtitle.TabIndex = 1;
        this.lblSubtitle.Text = "Zero-Storage Stateless Remote Companion Client";
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.ForeColor = System.Drawing.Color.White;
        this.lblTitle.Location = new System.Drawing.Point(16, 11);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(265, 20);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "📻 Stream Monitor - Remote Client";
        // 
        // lblServerHost
        // 
        this.lblServerHost.AutoSize = true;
        this.lblServerHost.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblServerHost.Location = new System.Drawing.Point(20, 80);
        this.lblServerHost.Name = "lblServerHost";
        this.lblServerHost.Size = new System.Drawing.Size(95, 15);
        this.lblServerHost.TabIndex = 1;
        this.lblServerHost.Text = "Server IP / Host:";
        // 
        // txtServerHost
        // 
        this.txtServerHost.Location = new System.Drawing.Point(20, 100);
        this.txtServerHost.Name = "txtServerHost";
        this.txtServerHost.Size = new System.Drawing.Size(250, 23);
        this.txtServerHost.TabIndex = 2;
        this.txtServerHost.Text = "127.0.0.1";
        // 
        // lblPort
        // 
        this.lblPort.AutoSize = true;
        this.lblPort.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblPort.Location = new System.Drawing.Point(280, 80);
        this.lblPort.Name = "lblPort";
        this.lblPort.Size = new System.Drawing.Size(34, 15);
        this.lblPort.TabIndex = 3;
        this.lblPort.Text = "Port:";
        // 
        // numPort
        // 
        this.numPort.Location = new System.Drawing.Point(280, 100);
        this.numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        this.numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numPort.Name = "numPort";
        this.numPort.Size = new System.Drawing.Size(84, 23);
        this.numPort.TabIndex = 4;
        this.numPort.Value = new decimal(new int[] { 8080, 0, 0, 0 });
        // 
        // lblUsername
        // 
        this.lblUsername.AutoSize = true;
        this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblUsername.Location = new System.Drawing.Point(20, 135);
        this.lblUsername.Name = "lblUsername";
        this.lblUsername.Size = new System.Drawing.Size(67, 15);
        this.lblUsername.TabIndex = 5;
        this.lblUsername.Text = "Username:";
        // 
        // txtUsername
        // 
        this.txtUsername.Location = new System.Drawing.Point(20, 155);
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(344, 23);
        this.txtUsername.TabIndex = 6;
        this.txtUsername.Text = "admin";
        // 
        // lblPassword
        // 
        this.lblPassword.AutoSize = true;
        this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblPassword.Location = new System.Drawing.Point(20, 190);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new System.Drawing.Size(62, 15);
        this.lblPassword.TabIndex = 7;
        this.lblPassword.Text = "Password:";
        // 
        // txtPassword
        // 
        this.txtPassword.Location = new System.Drawing.Point(20, 210);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.Size = new System.Drawing.Size(344, 23);
        this.txtPassword.TabIndex = 8;
        this.txtPassword.Text = "admin123";
        this.txtPassword.UseSystemPasswordChar = true;
        // 
        // chkShowPassword
        // 
        this.chkShowPassword.AutoSize = true;
        this.chkShowPassword.Location = new System.Drawing.Point(20, 240);
        this.chkShowPassword.Name = "chkShowPassword";
        this.chkShowPassword.Size = new System.Drawing.Size(108, 19);
        this.chkShowPassword.TabIndex = 9;
        this.chkShowPassword.Text = "Show password";
        this.chkShowPassword.UseVisualStyleBackColor = true;
        this.chkShowPassword.CheckedChanged += new System.EventHandler(this.ChkShowPassword_CheckedChanged);
        // 
        // lblStatus
        // 
        this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblStatus.ForeColor = System.Drawing.Color.Firebrick;
        this.lblStatus.Location = new System.Drawing.Point(20, 265);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(344, 35);
        this.lblStatus.TabIndex = 10;
        // 
        // btnLogin
        // 
        this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
        this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnLogin.ForeColor = System.Drawing.Color.White;
        this.btnLogin.Location = new System.Drawing.Point(20, 305);
        this.btnLogin.Name = "btnLogin";
        this.btnLogin.Size = new System.Drawing.Size(240, 35);
        this.btnLogin.TabIndex = 11;
        this.btnLogin.Text = "🚀 Connect && Login";
        this.btnLogin.UseVisualStyleBackColor = false;
        this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
        // 
        // btnExit
        // 
        this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnExit.Location = new System.Drawing.Point(269, 305);
        this.btnExit.Name = "btnExit";
        this.btnExit.Size = new System.Drawing.Size(95, 35);
        this.btnExit.TabIndex = 12;
        this.btnExit.Text = "Exit";
        this.btnExit.UseVisualStyleBackColor = true;
        this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
        // 
        // RemoteLoginForm
        // 
        this.AcceptButton = this.btnLogin;
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.White;
        this.CancelButton = this.btnExit;
        this.ClientSize = new System.Drawing.Size(384, 356);
        this.Controls.Add(this.btnExit);
        this.Controls.Add(this.btnLogin);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.chkShowPassword);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.lblPassword);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.lblUsername);
        this.Controls.Add(this.numPort);
        this.Controls.Add(this.lblPort);
        this.Controls.Add(this.txtServerHost);
        this.Controls.Add(this.lblServerHost);
        this.Controls.Add(this.pnlHeader);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "RemoteLoginForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "RADIOTEDU Stream Audio Server — Remote Login";
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblSubtitle;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblServerHost;
    private System.Windows.Forms.TextBox txtServerHost;
    private System.Windows.Forms.Label lblPort;
    private System.Windows.Forms.NumericUpDown numPort;
    private System.Windows.Forms.Label lblUsername;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.CheckBox chkShowPassword;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.Button btnLogin;
    private System.Windows.Forms.Button btnExit;
}
