namespace IcecastStreamMonitor.Forms;

partial class UpdateCheckDialog
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
        this.pnlStatus = new System.Windows.Forms.Panel();
        this.lblCurrentVersion = new System.Windows.Forms.Label();
        this.lblLatestVersion = new System.Windows.Forms.Label();
        this.lblStatusBadge = new System.Windows.Forms.Label();
        this.lblReleaseNotesTitle = new System.Windows.Forms.Label();
        this.txtReleaseNotes = new System.Windows.Forms.TextBox();
        this.pnlBottom = new System.Windows.Forms.Panel();
        this.btnCheckNow = new System.Windows.Forms.Button();
        this.btnDownloadUpdate = new System.Windows.Forms.Button();
        this.btnClose = new System.Windows.Forms.Button();
        this.pnlHeader.SuspendLayout();
        this.pnlStatus.SuspendLayout();
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
        this.pnlHeader.Size = new System.Drawing.Size(534, 55);
        this.pnlHeader.TabIndex = 0;
        // 
        // lblSubtitle
        // 
        this.lblSubtitle.AutoSize = true;
        this.lblSubtitle.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblSubtitle.Location = new System.Drawing.Point(16, 32);
        this.lblSubtitle.Name = "lblSubtitle";
        this.lblSubtitle.Size = new System.Drawing.Size(252, 15);
        this.lblSubtitle.TabIndex = 1;
        this.lblSubtitle.Text = "GitHub Releases & Automated Version Checker";
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.Location = new System.Drawing.Point(16, 9);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(189, 20);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "Check for Latest Updates";
        // 
        // pnlStatus
        // 
        this.pnlStatus.BackColor = System.Drawing.Color.White;
        this.pnlStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pnlStatus.Controls.Add(this.lblStatusBadge);
        this.pnlStatus.Controls.Add(this.lblLatestVersion);
        this.pnlStatus.Controls.Add(this.lblCurrentVersion);
        this.pnlStatus.Location = new System.Drawing.Point(16, 68);
        this.pnlStatus.Name = "pnlStatus";
        this.pnlStatus.Padding = new System.Windows.Forms.Padding(12);
        this.pnlStatus.Size = new System.Drawing.Size(502, 72);
        this.pnlStatus.TabIndex = 1;
        // 
        // lblCurrentVersion
        // 
        this.lblCurrentVersion.AutoSize = true;
        this.lblCurrentVersion.Location = new System.Drawing.Point(12, 12);
        this.lblCurrentVersion.Name = "lblCurrentVersion";
        this.lblCurrentVersion.Size = new System.Drawing.Size(142, 15);
        this.lblCurrentVersion.TabIndex = 0;
        this.lblCurrentVersion.Text = "Installed Version: v1.0.0";
        // 
        // lblLatestVersion
        // 
        this.lblLatestVersion.AutoSize = true;
        this.lblLatestVersion.Location = new System.Drawing.Point(12, 38);
        this.lblLatestVersion.Name = "lblLatestVersion";
        this.lblLatestVersion.Size = new System.Drawing.Size(130, 15);
        this.lblLatestVersion.TabIndex = 1;
        this.lblLatestVersion.Text = "Latest Version: Checking...";
        // 
        // lblStatusBadge
        // 
        this.lblStatusBadge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.lblStatusBadge.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblStatusBadge.ForeColor = System.Drawing.Color.DimGray;
        this.lblStatusBadge.Location = new System.Drawing.Point(235, 12);
        this.lblStatusBadge.Name = "lblStatusBadge";
        this.lblStatusBadge.Size = new System.Drawing.Size(252, 41);
        this.lblStatusBadge.TabIndex = 2;
        this.lblStatusBadge.Text = "Checking for updates...";
        this.lblStatusBadge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // lblReleaseNotesTitle
        // 
        this.lblReleaseNotesTitle.AutoSize = true;
        this.lblReleaseNotesTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblReleaseNotesTitle.Location = new System.Drawing.Point(16, 152);
        this.lblReleaseNotesTitle.Name = "lblReleaseNotesTitle";
        this.lblReleaseNotesTitle.Size = new System.Drawing.Size(149, 15);
        this.lblReleaseNotesTitle.TabIndex = 2;
        this.lblReleaseNotesTitle.Text = "Release Notes & Changelog:";
        // 
        // txtReleaseNotes
        // 
        this.txtReleaseNotes.BackColor = System.Drawing.SystemColors.Window;
        this.txtReleaseNotes.Location = new System.Drawing.Point(16, 172);
        this.txtReleaseNotes.Multiline = true;
        this.txtReleaseNotes.Name = "txtReleaseNotes";
        this.txtReleaseNotes.ReadOnly = true;
        this.txtReleaseNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtReleaseNotes.Size = new System.Drawing.Size(502, 140);
        this.txtReleaseNotes.TabIndex = 3;
        // 
        // pnlBottom
        // 
        this.pnlBottom.Controls.Add(this.btnCheckNow);
        this.pnlBottom.Controls.Add(this.btnDownloadUpdate);
        this.pnlBottom.Controls.Add(this.btnClose);
        this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnlBottom.Location = new System.Drawing.Point(0, 325);
        this.pnlBottom.Name = "pnlBottom";
        this.pnlBottom.Size = new System.Drawing.Size(534, 45);
        this.pnlBottom.TabIndex = 4;
        // 
        // btnCheckNow
        // 
        this.btnCheckNow.Location = new System.Drawing.Point(16, 8);
        this.btnCheckNow.Name = "btnCheckNow";
        this.btnCheckNow.Size = new System.Drawing.Size(100, 28);
        this.btnCheckNow.TabIndex = 0;
        this.btnCheckNow.Text = "⟳ Check Again";
        this.btnCheckNow.UseVisualStyleBackColor = true;
        this.btnCheckNow.Click += new System.EventHandler(this.BtnCheckNow_Click);
        // 
        // btnDownloadUpdate
        // 
        this.btnDownloadUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnDownloadUpdate.Enabled = false;
        this.btnDownloadUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnDownloadUpdate.Location = new System.Drawing.Point(265, 8);
        this.btnDownloadUpdate.Name = "btnDownloadUpdate";
        this.btnDownloadUpdate.Size = new System.Drawing.Size(175, 28);
        this.btnDownloadUpdate.TabIndex = 1;
        this.btnDownloadUpdate.Text = "🚀 Download & Update Now";
        this.btnDownloadUpdate.UseVisualStyleBackColor = true;
        this.btnDownloadUpdate.Click += new System.EventHandler(this.BtnDownloadUpdate_Click);
        // 
        // btnClose
        // 
        this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnClose.Location = new System.Drawing.Point(446, 8);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(75, 28);
        this.btnClose.TabIndex = 2;
        this.btnClose.Text = "Close";
        this.btnClose.UseVisualStyleBackColor = true;
        // 
        // UpdateCheckDialog
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(534, 370);
        this.Controls.Add(this.pnlBottom);
        this.Controls.Add(this.txtReleaseNotes);
        this.Controls.Add(this.lblReleaseNotesTitle);
        this.Controls.Add(this.pnlStatus);
        this.Controls.Add(this.pnlHeader);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "UpdateCheckDialog";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Software Updates";
        this.Load += new System.EventHandler(this.UpdateCheckDialog_Load);
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        this.pnlStatus.ResumeLayout(false);
        this.pnlStatus.PerformLayout();
        this.pnlBottom.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblSubtitle;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Panel pnlStatus;
    private System.Windows.Forms.Label lblCurrentVersion;
    private System.Windows.Forms.Label lblLatestVersion;
    private System.Windows.Forms.Label lblStatusBadge;
    private System.Windows.Forms.Label lblReleaseNotesTitle;
    private System.Windows.Forms.TextBox txtReleaseNotes;
    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.Button btnCheckNow;
    private System.Windows.Forms.Button btnDownloadUpdate;
    private System.Windows.Forms.Button btnClose;
}
