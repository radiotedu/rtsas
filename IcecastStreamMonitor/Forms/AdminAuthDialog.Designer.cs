namespace IcecastStreamMonitor.Forms;

partial class AdminAuthDialog
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
        this.lblPrompt = new System.Windows.Forms.Label();
        this.txtAdminPassword = new System.Windows.Forms.TextBox();
        this.chkShowPassword = new System.Windows.Forms.CheckBox();
        this.lblError = new System.Windows.Forms.Label();
        this.pnlBottom = new System.Windows.Forms.Panel();
        this.btnUnlock = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.pnlHeader.SuspendLayout();
        this.pnlBottom.SuspendLayout();
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
        this.pnlHeader.Size = new System.Drawing.Size(434, 52);
        this.pnlHeader.TabIndex = 0;
        // 
        // lblSubtitle
        // 
        this.lblSubtitle.AutoSize = true;
        this.lblSubtitle.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblSubtitle.Location = new System.Drawing.Point(16, 30);
        this.lblSubtitle.Name = "lblSubtitle";
        this.lblSubtitle.Size = new System.Drawing.Size(262, 15);
        this.lblSubtitle.TabIndex = 1;
        this.lblSubtitle.Text = "Authentication required for password modifications";
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.Location = new System.Drawing.Point(16, 9);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(193, 19);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "🔒 Admin Security Verification";
        // 
        // lblPrompt
        // 
        this.lblPrompt.Location = new System.Drawing.Point(20, 68);
        this.lblPrompt.Name = "lblPrompt";
        this.lblPrompt.Size = new System.Drawing.Size(394, 35);
        this.lblPrompt.TabIndex = 1;
        this.lblPrompt.Text = "Enter the Master Admin Password to view or edit passwords for this channel:";
        // 
        // txtAdminPassword
        // 
        this.txtAdminPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.txtAdminPassword.Location = new System.Drawing.Point(20, 108);
        this.txtAdminPassword.Name = "txtAdminPassword";
        this.txtAdminPassword.Size = new System.Drawing.Size(394, 25);
        this.txtAdminPassword.TabIndex = 2;
        this.txtAdminPassword.UseSystemPasswordChar = true;
        this.txtAdminPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtAdminPassword_KeyDown);
        // 
        // chkShowPassword
        // 
        this.chkShowPassword.AutoSize = true;
        this.chkShowPassword.Location = new System.Drawing.Point(20, 140);
        this.chkShowPassword.Name = "chkShowPassword";
        this.chkShowPassword.Size = new System.Drawing.Size(108, 19);
        this.chkShowPassword.TabIndex = 3;
        this.chkShowPassword.Text = "Show password";
        this.chkShowPassword.UseVisualStyleBackColor = true;
        this.chkShowPassword.CheckedChanged += new System.EventHandler(this.ChkShowPassword_CheckedChanged);
        // 
        // lblError
        // 
        this.lblError.AutoSize = true;
        this.lblError.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblError.ForeColor = System.Drawing.Color.Firebrick;
        this.lblError.Location = new System.Drawing.Point(20, 168);
        this.lblError.Name = "lblError";
        this.lblError.Size = new System.Drawing.Size(0, 15);
        this.lblError.TabIndex = 4;
        // 
        // pnlBottom
        // 
        this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
        this.pnlBottom.Controls.Add(this.btnUnlock);
        this.pnlBottom.Controls.Add(this.btnCancel);
        this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnlBottom.Location = new System.Drawing.Point(0, 195);
        this.pnlBottom.Name = "pnlBottom";
        this.pnlBottom.Size = new System.Drawing.Size(434, 45);
        this.pnlBottom.TabIndex = 5;
        // 
        // btnUnlock
        // 
        this.btnUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnUnlock.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnUnlock.Location = new System.Drawing.Point(220, 8);
        this.btnUnlock.Name = "btnUnlock";
        this.btnUnlock.Size = new System.Drawing.Size(125, 28);
        this.btnUnlock.TabIndex = 0;
        this.btnUnlock.Text = "🔓 Unlock";
        this.btnUnlock.UseVisualStyleBackColor = true;
        this.btnUnlock.Click += new System.EventHandler(this.BtnUnlock_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point(351, 8);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 28);
        this.btnCancel.TabIndex = 1;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        // 
        // AdminAuthDialog
        // 
        this.AcceptButton = this.btnUnlock;
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(434, 240);
        this.Controls.Add(this.pnlBottom);
        this.Controls.Add(this.lblError);
        this.Controls.Add(this.chkShowPassword);
        this.Controls.Add(this.txtAdminPassword);
        this.Controls.Add(this.lblPrompt);
        this.Controls.Add(this.pnlHeader);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "AdminAuthDialog";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Admin Authentication Required";
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        this.pnlBottom.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblSubtitle;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblPrompt;
    private System.Windows.Forms.TextBox txtAdminPassword;
    private System.Windows.Forms.CheckBox chkShowPassword;
    private System.Windows.Forms.Label lblError;
    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.Button btnUnlock;
    private System.Windows.Forms.Button btnCancel;
}
