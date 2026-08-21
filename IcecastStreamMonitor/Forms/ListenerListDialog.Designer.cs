namespace IcecastStreamMonitor.Forms;

partial class ListenerListDialog
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
        this.gridListeners = new System.Windows.Forms.DataGridView();
        this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colIp = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colUserAgent = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.pnlBottom = new System.Windows.Forms.Panel();
        this.chkAutoRefresh = new System.Windows.Forms.CheckBox();
        this.btnRefresh = new System.Windows.Forms.Button();
        this.btnCopySelected = new System.Windows.Forms.Button();
        this.btnCopyAll = new System.Windows.Forms.Button();
        this.btnClose = new System.Windows.Forms.Button();
        this.lblStatus = new System.Windows.Forms.Label();
        this.pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridListeners)).BeginInit();
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
        this.pnlHeader.Size = new System.Drawing.Size(684, 60);
        this.pnlHeader.TabIndex = 0;
        // 
        // lblSubtitle
        // 
        this.lblSubtitle.AutoSize = true;
        this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblSubtitle.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblSubtitle.Location = new System.Drawing.Point(16, 34);
        this.lblSubtitle.Name = "lblSubtitle";
        this.lblSubtitle.Size = new System.Drawing.Size(184, 15);
        this.lblSubtitle.TabIndex = 1;
        this.lblSubtitle.Text = "Server: 127.0.0.1:8000 | Mount: /live";
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.Location = new System.Drawing.Point(16, 11);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(248, 20);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "Connected Listeners (Client IP List)";
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
            this.colId,
            this.colIp,
            this.colLocation,
            this.colDuration,
            this.colUserAgent});
        this.gridListeners.Dock = System.Windows.Forms.DockStyle.Fill;
        this.gridListeners.Location = new System.Drawing.Point(0, 60);
        this.gridListeners.MultiSelect = true;
        this.gridListeners.Name = "gridListeners";
        this.gridListeners.ReadOnly = true;
        this.gridListeners.RowHeadersVisible = false;
        this.gridListeners.RowTemplate.Height = 25;
        this.gridListeners.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.gridListeners.Size = new System.Drawing.Size(684, 330);
        this.gridListeners.TabIndex = 1;
        // 
        // colId
        // 
        this.colId.HeaderText = "ID";
        this.colId.Name = "colId";
        this.colId.ReadOnly = true;
        this.colId.Width = 60;
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
        this.colLocation.Width = 130;
        // 
        // colDuration
        // 
        this.colDuration.HeaderText = "Connected Time";
        this.colDuration.Name = "colDuration";
        this.colDuration.ReadOnly = true;
        this.colDuration.Width = 110;
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
        this.pnlBottom.Controls.Add(this.lblStatus);
        this.pnlBottom.Controls.Add(this.chkAutoRefresh);
        this.pnlBottom.Controls.Add(this.btnRefresh);
        this.pnlBottom.Controls.Add(this.btnCopySelected);
        this.pnlBottom.Controls.Add(this.btnCopyAll);
        this.pnlBottom.Controls.Add(this.btnClose);
        this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnlBottom.Location = new System.Drawing.Point(0, 390);
        this.pnlBottom.Name = "pnlBottom";
        this.pnlBottom.Size = new System.Drawing.Size(684, 50);
        this.pnlBottom.TabIndex = 2;
        // 
        // chkAutoRefresh
        // 
        this.chkAutoRefresh.AutoSize = true;
        this.chkAutoRefresh.Checked = true;
        this.chkAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkAutoRefresh.Location = new System.Drawing.Point(16, 17);
        this.chkAutoRefresh.Name = "chkAutoRefresh";
        this.chkAutoRefresh.Size = new System.Drawing.Size(130, 19);
        this.chkAutoRefresh.TabIndex = 0;
        this.chkAutoRefresh.Text = "Auto-Refresh (every 5s)";
        this.chkAutoRefresh.UseVisualStyleBackColor = true;
        this.chkAutoRefresh.CheckedChanged += new System.EventHandler(this.ChkAutoRefresh_CheckedChanged);
        // 
        // btnRefresh
        // 
        this.btnRefresh.Location = new System.Drawing.Point(155, 12);
        this.btnRefresh.Name = "btnRefresh";
        this.btnRefresh.Size = new System.Drawing.Size(95, 28);
        this.btnRefresh.TabIndex = 1;
        this.btnRefresh.Text = "⟳ Refresh Now";
        this.btnRefresh.UseVisualStyleBackColor = true;
        this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
        // 
        // btnCopySelected
        // 
        this.btnCopySelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCopySelected.Location = new System.Drawing.Point(375, 12);
        this.btnCopySelected.Name = "btnCopySelected";
        this.btnCopySelected.Size = new System.Drawing.Size(105, 28);
        this.btnCopySelected.TabIndex = 2;
        this.btnCopySelected.Text = "📋 Copy IP";
        this.btnCopySelected.UseVisualStyleBackColor = true;
        this.btnCopySelected.Click += new System.EventHandler(this.BtnCopySelected_Click);
        // 
        // btnCopyAll
        // 
        this.btnCopyAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCopyAll.Location = new System.Drawing.Point(486, 12);
        this.btnCopyAll.Name = "btnCopyAll";
        this.btnCopyAll.Size = new System.Drawing.Size(105, 28);
        this.btnCopyAll.TabIndex = 3;
        this.btnCopyAll.Text = "📋 Copy All IPs";
        this.btnCopyAll.UseVisualStyleBackColor = true;
        this.btnCopyAll.Click += new System.EventHandler(this.BtnCopyAll_Click);
        // 
        // btnClose
        // 
        this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnClose.Location = new System.Drawing.Point(597, 12);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(75, 28);
        this.btnClose.TabIndex = 4;
        this.btnClose.Text = "Close";
        this.btnClose.UseVisualStyleBackColor = true;
        // 
        // lblStatus
        // 
        this.lblStatus.AutoSize = true;
        this.lblStatus.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblStatus.Location = new System.Drawing.Point(258, 18);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(59, 15);
        this.lblStatus.TabIndex = 5;
        this.lblStatus.Text = "Loading...";
        // 
        // ListenerListDialog
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(684, 440);
        this.Controls.Add(this.gridListeners);
        this.Controls.Add(this.pnlBottom);
        this.Controls.Add(this.pnlHeader);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.MinimumSize = new System.Drawing.Size(600, 350);
        this.Name = "ListenerListDialog";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Connected Listeners";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ListenerListDialog_FormClosing);
        this.Load += new System.EventHandler(this.ListenerListDialog_Load);
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridListeners)).EndInit();
        this.pnlBottom.ResumeLayout(false);
        this.pnlBottom.PerformLayout();
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblSubtitle;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.DataGridView gridListeners;
    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.CheckBox chkAutoRefresh;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnCopySelected;
    private System.Windows.Forms.Button btnCopyAll;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIp;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDuration;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUserAgent;
}
