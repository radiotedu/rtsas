namespace IcecastStreamMonitor.RemoteControl.Forms;

partial class RemoteListenersDialog
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
        this.lblStreamInfo = new System.Windows.Forms.Label();
        this.lblTitle = new System.Windows.Forms.Label();
        this.gridListeners = new System.Windows.Forms.DataGridView();
        this.colIp = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colUserAgent = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.pnlBottom = new System.Windows.Forms.Panel();
        this.chkAutoRefresh = new System.Windows.Forms.CheckBox();
        this.btnRefresh = new System.Windows.Forms.Button();
        this.btnCopyIp = new System.Windows.Forms.Button();
        this.btnCopyAll = new System.Windows.Forms.Button();
        this.btnClose = new System.Windows.Forms.Button();
        this.pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridListeners)).BeginInit();
        this.pnlBottom.SuspendLayout();
        this.SuspendLayout();
        // 
        // pnlHeader
        // 
        this.pnlHeader.BackColor = System.Drawing.SystemColors.ControlLightLight;
        this.pnlHeader.Controls.Add(this.lblStreamInfo);
        this.pnlHeader.Controls.Add(this.lblTitle);
        this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlHeader.Location = new System.Drawing.Point(0, 0);
        this.pnlHeader.Name = "pnlHeader";
        this.pnlHeader.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
        this.pnlHeader.Size = new System.Drawing.Size(684, 55);
        this.pnlHeader.TabIndex = 0;
        // 
        // lblStreamInfo
        // 
        this.lblStreamInfo.AutoSize = true;
        this.lblStreamInfo.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblStreamInfo.Location = new System.Drawing.Point(16, 32);
        this.lblStreamInfo.Name = "lblStreamInfo";
        this.lblStreamInfo.Size = new System.Drawing.Size(185, 15);
        this.lblStreamInfo.TabIndex = 1;
        this.lblStreamInfo.Text = "Stream: /live | Connected Listeners: 0";
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.Location = new System.Drawing.Point(16, 9);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(248, 20);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "👥 Connected Listener Client IPs";
        // 
        // gridListeners
        // 
        this.gridListeners.AllowUserToAddRows = false;
        this.gridListeners.AllowUserToDeleteRows = false;
        this.gridListeners.AllowUserToResizeRows = false;
        this.gridListeners.BackgroundColor = System.Drawing.SystemColors.Window;
        this.gridListeners.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.gridListeners.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.gridListeners.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIp,
            this.colLocation,
            this.colTime,
            this.colUserAgent});
        this.gridListeners.Dock = System.Windows.Forms.DockStyle.Fill;
        this.gridListeners.Location = new System.Drawing.Point(0, 55);
        this.gridListeners.MultiSelect = true;
        this.gridListeners.Name = "gridListeners";
        this.gridListeners.ReadOnly = true;
        this.gridListeners.RowHeadersVisible = false;
        this.gridListeners.RowTemplate.Height = 25;
        this.gridListeners.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.gridListeners.Size = new System.Drawing.Size(684, 301);
        this.gridListeners.TabIndex = 1;
        // 
        // colIp
        // 
        this.colIp.HeaderText = "IP Address";
        this.colIp.Name = "colIp";
        this.colIp.ReadOnly = true;
        this.colIp.Width = 140;
        // 
        // colLocation
        // 
        this.colLocation.HeaderText = "Network / Location";
        this.colLocation.Name = "colLocation";
        this.colLocation.ReadOnly = true;
        this.colLocation.Width = 140;
        // 
        // colTime
        // 
        this.colTime.HeaderText = "Connected Time";
        this.colTime.Name = "colTime";
        this.colTime.ReadOnly = true;
        this.colTime.Width = 120;
        // 
        // colUserAgent
        // 
        this.colUserAgent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.colUserAgent.HeaderText = "Player / User Agent";
        this.colUserAgent.Name = "colUserAgent";
        this.colUserAgent.ReadOnly = true;
        // 
        // pnlBottom
        // 
        this.pnlBottom.Controls.Add(this.chkAutoRefresh);
        this.pnlBottom.Controls.Add(this.btnRefresh);
        this.pnlBottom.Controls.Add(this.btnCopyIp);
        this.pnlBottom.Controls.Add(this.btnCopyAll);
        this.pnlBottom.Controls.Add(this.btnClose);
        this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnlBottom.Location = new System.Drawing.Point(0, 356);
        this.pnlBottom.Name = "pnlBottom";
        this.pnlBottom.Size = new System.Drawing.Size(684, 45);
        this.pnlBottom.TabIndex = 2;
        // 
        // chkAutoRefresh
        // 
        this.chkAutoRefresh.AutoSize = true;
        this.chkAutoRefresh.Checked = true;
        this.chkAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkAutoRefresh.Location = new System.Drawing.Point(125, 14);
        this.chkAutoRefresh.Name = "chkAutoRefresh";
        this.chkAutoRefresh.Size = new System.Drawing.Size(142, 19);
        this.chkAutoRefresh.TabIndex = 1;
        this.chkAutoRefresh.Text = "Auto-Refresh every 5s";
        this.chkAutoRefresh.UseVisualStyleBackColor = true;
        this.chkAutoRefresh.CheckedChanged += new System.EventHandler(this.ChkAutoRefresh_CheckedChanged);
        // 
        // btnRefresh
        // 
        this.btnRefresh.Location = new System.Drawing.Point(16, 8);
        this.btnRefresh.Name = "btnRefresh";
        this.btnRefresh.Size = new System.Drawing.Size(95, 28);
        this.btnRefresh.TabIndex = 0;
        this.btnRefresh.Text = "⟳ Refresh";
        this.btnRefresh.UseVisualStyleBackColor = true;
        this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
        // 
        // btnCopyIp
        // 
        this.btnCopyIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCopyIp.Location = new System.Drawing.Point(348, 8);
        this.btnCopyIp.Name = "btnCopyIp";
        this.btnCopyIp.Size = new System.Drawing.Size(115, 28);
        this.btnCopyIp.TabIndex = 2;
        this.btnCopyIp.Text = "📋 Copy Selected";
        this.btnCopyIp.UseVisualStyleBackColor = true;
        this.btnCopyIp.Click += new System.EventHandler(this.BtnCopyIp_Click);
        // 
        // btnCopyAll
        // 
        this.btnCopyAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCopyAll.Location = new System.Drawing.Point(469, 8);
        this.btnCopyAll.Name = "btnCopyAll";
        this.btnCopyAll.Size = new System.Drawing.Size(115, 28);
        this.btnCopyAll.TabIndex = 3;
        this.btnCopyAll.Text = "📋 Copy All";
        this.btnCopyAll.UseVisualStyleBackColor = true;
        this.btnCopyAll.Click += new System.EventHandler(this.BtnCopyAll_Click);
        // 
        // btnClose
        // 
        this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnClose.Location = new System.Drawing.Point(590, 8);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(78, 28);
        this.btnClose.TabIndex = 4;
        this.btnClose.Text = "Close";
        this.btnClose.UseVisualStyleBackColor = true;
        // 
        // RemoteListenersDialog
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(684, 401);
        this.Controls.Add(this.gridListeners);
        this.Controls.Add(this.pnlBottom);
        this.Controls.Add(this.pnlHeader);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.MinimumSize = new System.Drawing.Size(550, 350);
        this.Name = "RemoteListenersDialog";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Connected Listener IPs";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteListenersDialog_FormClosing);
        this.Load += new System.EventHandler(this.RemoteListenersDialog_Load);
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridListeners)).EndInit();
        this.pnlBottom.ResumeLayout(false);
        this.pnlBottom.PerformLayout();
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblStreamInfo;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.DataGridView gridListeners;
    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.CheckBox chkAutoRefresh;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnCopyIp;
    private System.Windows.Forms.Button btnCopyAll;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIp;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUserAgent;
}
