namespace IcecastStreamMonitor.RemoteControl.Forms;

partial class RemoteMainForm
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
        this.components = new System.ComponentModel.Container();
        this.toolStrip = new System.Windows.Forms.ToolStrip();
        this.btnToolRefresh = new System.Windows.Forms.ToolStripButton();
        this.btnToolAutoRefresh = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.btnToolAnalytics = new System.Windows.Forms.ToolStripButton();
        this.btnToolListeners = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.btnToolAddChannel = new System.Windows.Forms.ToolStripButton();
        this.btnToolDeleteChannel = new System.Windows.Forms.ToolStripButton();
        this.lblUserBadge = new System.Windows.Forms.ToolStripLabel();
        this.statusStrip = new System.Windows.Forms.StatusStrip();
        this.statusLabelServer = new System.Windows.Forms.ToolStripStatusLabel();
        this.statusLabelSpring = new System.Windows.Forms.ToolStripStatusLabel();
        this.statusLabelSummary = new System.Windows.Forms.ToolStripStatusLabel();
        this.gridStreams = new System.Windows.Forms.DataGridView();
        this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colMount = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colListeners = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colPeak = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colBitrate = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colFormat = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colLatency = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.menuItemAnalytics = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemListeners = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        this.menuItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStrip.SuspendLayout();
        this.statusStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridStreams)).BeginInit();
        this.contextMenu.SuspendLayout();
        this.SuspendLayout();
        // 
        // toolStrip
        // 
        this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
        this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnToolRefresh,
            this.btnToolAutoRefresh,
            this.toolStripSeparator1,
            this.btnToolAnalytics,
            this.btnToolListeners,
            this.toolStripSeparator2,
            this.btnToolAddChannel,
            this.btnToolDeleteChannel,
            this.lblUserBadge});
        this.toolStrip.Location = new System.Drawing.Point(0, 0);
        this.toolStrip.Name = "toolStrip";
        this.toolStrip.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
        this.toolStrip.Size = new System.Drawing.Size(984, 35);
        this.toolStrip.TabIndex = 0;
        this.toolStrip.Text = "toolStrip";
        // 
        // btnToolRefresh
        // 
        this.btnToolRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnToolRefresh.Name = "btnToolRefresh";
        this.btnToolRefresh.Size = new System.Drawing.Size(71, 24);
        this.btnToolRefresh.Text = "⟳ Refresh";
        this.btnToolRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
        // 
        // btnToolAutoRefresh
        // 
        this.btnToolAutoRefresh.Checked = true;
        this.btnToolAutoRefresh.CheckOnClick = true;
        this.btnToolAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
        this.btnToolAutoRefresh.Name = "btnToolAutoRefresh";
        this.btnToolAutoRefresh.Size = new System.Drawing.Size(126, 24);
        this.btnToolAutoRefresh.Text = "⏱ Auto-Refresh (5s)";
        this.btnToolAutoRefresh.Click += new System.EventHandler(this.BtnAutoRefresh_Click);
        // 
        // toolStripSeparator1
        // 
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
        // 
        // btnToolAnalytics
        // 
        this.btnToolAnalytics.Name = "btnToolAnalytics";
        this.btnToolAnalytics.Size = new System.Drawing.Size(130, 24);
        this.btnToolAnalytics.Text = "📊 24h Logs && Charts";
        this.btnToolAnalytics.Click += new System.EventHandler(this.BtnAnalytics_Click);
        // 
        // btnToolListeners
        // 
        this.btnToolListeners.Name = "btnToolListeners";
        this.btnToolListeners.Size = new System.Drawing.Size(139, 24);
        this.btnToolListeners.Text = "👥 Connected Listener IPs";
        this.btnToolListeners.Click += new System.EventHandler(this.BtnListeners_Click);
        // 
        // toolStripSeparator2
        // 
        this.toolStripSeparator2.Name = "toolStripSeparator2";
        this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
        // 
        // btnToolAddChannel
        // 
        this.btnToolAddChannel.Name = "btnToolAddChannel";
        this.btnToolAddChannel.Size = new System.Drawing.Size(97, 24);
        this.btnToolAddChannel.Text = "+ Add Channel";
        this.btnToolAddChannel.Visible = false;
        this.btnToolAddChannel.Click += new System.EventHandler(this.BtnAddChannel_Click);
        // 
        // btnToolDeleteChannel
        // 
        this.btnToolDeleteChannel.Name = "btnToolDeleteChannel";
        this.btnToolDeleteChannel.Size = new System.Drawing.Size(73, 24);
        this.btnToolDeleteChannel.Text = "✕ Delete";
        this.btnToolDeleteChannel.Visible = false;
        this.btnToolDeleteChannel.Click += new System.EventHandler(this.BtnDeleteChannel_Click);
        // 
        // lblUserBadge
        // 
        this.lblUserBadge.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
        this.lblUserBadge.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblUserBadge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
        this.lblUserBadge.Name = "lblUserBadge";
        this.lblUserBadge.Size = new System.Drawing.Size(175, 24);
        this.lblUserBadge.Text = "👤 admin (Role: SuperAdmin)";
        // 
        // statusStrip
        // 
        this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabelServer,
            this.statusLabelSpring,
            this.statusLabelSummary});
        this.statusStrip.Location = new System.Drawing.Point(0, 519);
        this.statusStrip.Name = "statusStrip";
        this.statusStrip.Size = new System.Drawing.Size(984, 22);
        this.statusStrip.TabIndex = 1;
        this.statusStrip.Text = "statusStrip";
        // 
        // statusLabelServer
        // 
        this.statusLabelServer.Name = "statusLabelServer";
        this.statusLabelServer.Size = new System.Drawing.Size(201, 17);
        this.statusLabelServer.Text = "Connected to http://127.0.0.1:8080";
        // 
        // statusLabelSpring
        // 
        this.statusLabelSpring.Name = "statusLabelSpring";
        this.statusLabelSpring.Size = new System.Drawing.Size(653, 17);
        this.statusLabelSpring.Spring = true;
        // 
        // statusLabelSummary
        // 
        this.statusLabelSummary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.statusLabelSummary.Name = "statusLabelSummary";
        this.statusLabelSummary.Size = new System.Drawing.Size(115, 17);
        this.statusLabelSummary.Text = "0 Channels Online";
        // 
        // gridStreams
        // 
        this.gridStreams.AllowUserToAddRows = false;
        this.gridStreams.AllowUserToDeleteRows = false;
        this.gridStreams.AllowUserToResizeRows = false;
        this.gridStreams.BackgroundColor = System.Drawing.SystemColors.Window;
        this.gridStreams.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.gridStreams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.gridStreams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStatus,
            this.colName,
            this.colMount,
            this.colListeners,
            this.colPeak,
            this.colBitrate,
            this.colFormat,
            this.colLatency,
            this.colTitle});
        this.gridStreams.ContextMenuStrip = this.contextMenu;
        this.gridStreams.Dock = System.Windows.Forms.DockStyle.Fill;
        this.gridStreams.Location = new System.Drawing.Point(0, 35);
        this.gridStreams.MultiSelect = false;
        this.gridStreams.Name = "gridStreams";
        this.gridStreams.ReadOnly = true;
        this.gridStreams.RowHeadersVisible = false;
        this.gridStreams.RowTemplate.Height = 28;
        this.gridStreams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.gridStreams.Size = new System.Drawing.Size(984, 484);
        this.gridStreams.TabIndex = 2;
        this.gridStreams.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridStreams_CellDoubleClick);
        // 
        // colStatus
        // 
        this.colStatus.HeaderText = "Status";
        this.colStatus.Name = "colStatus";
        this.colStatus.ReadOnly = true;
        this.colStatus.Width = 85;
        // 
        // colName
        // 
        this.colName.HeaderText = "Channel Label";
        this.colName.Name = "colName";
        this.colName.ReadOnly = true;
        this.colName.Width = 180;
        // 
        // colMount
        // 
        this.colMount.HeaderText = "Mount Path";
        this.colMount.Name = "colMount";
        this.colMount.ReadOnly = true;
        this.colMount.Width = 110;
        // 
        // colListeners
        // 
        this.colListeners.HeaderText = "Listeners";
        this.colListeners.Name = "colListeners";
        this.colListeners.ReadOnly = true;
        this.colListeners.Width = 85;
        // 
        // colPeak
        // 
        this.colPeak.HeaderText = "Peak";
        this.colPeak.Name = "colPeak";
        this.colPeak.ReadOnly = true;
        this.colPeak.Width = 70;
        // 
        // colBitrate
        // 
        this.colBitrate.HeaderText = "Bit Rate";
        this.colBitrate.Name = "colBitrate";
        this.colBitrate.ReadOnly = true;
        this.colBitrate.Width = 85;
        // 
        // colFormat
        // 
        this.colFormat.HeaderText = "Format";
        this.colFormat.Name = "colFormat";
        this.colFormat.ReadOnly = true;
        this.colFormat.Width = 85;
        // 
        // colLatency
        // 
        this.colLatency.HeaderText = "Latency";
        this.colLatency.Name = "colLatency";
        this.colLatency.ReadOnly = true;
        this.colLatency.Width = 75;
        // 
        // colTitle
        // 
        this.colTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.colTitle.HeaderText = "Now Playing / Stream Title";
        this.colTitle.Name = "colTitle";
        this.colTitle.ReadOnly = true;
        // 
        // contextMenu
        // 
        this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAnalytics,
            this.menuItemListeners,
            this.toolStripSeparator3,
            this.menuItemRefresh,
            this.menuItemDelete});
        this.contextMenu.Name = "contextMenu";
        this.contextMenu.Size = new System.Drawing.Size(232, 98);
        // 
        // menuItemAnalytics
        // 
        this.menuItemAnalytics.Name = "menuItemAnalytics";
        this.menuItemAnalytics.Size = new System.Drawing.Size(231, 22);
        this.menuItemAnalytics.Text = "📊 View 24h Logs && Charts...";
        this.menuItemAnalytics.Click += new System.EventHandler(this.BtnAnalytics_Click);
        // 
        // menuItemListeners
        // 
        this.menuItemListeners.Name = "menuItemListeners";
        this.menuItemListeners.Size = new System.Drawing.Size(231, 22);
        this.menuItemListeners.Text = "👥 Show Connected Client IPs...";
        this.menuItemListeners.Click += new System.EventHandler(this.BtnListeners_Click);
        // 
        // toolStripSeparator3
        // 
        this.toolStripSeparator3.Name = "toolStripSeparator3";
        this.toolStripSeparator3.Size = new System.Drawing.Size(228, 6);
        // 
        // menuItemRefresh
        // 
        this.menuItemRefresh.Name = "menuItemRefresh";
        this.menuItemRefresh.Size = new System.Drawing.Size(231, 22);
        this.menuItemRefresh.Text = "⟳ Refresh Stream";
        this.menuItemRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
        // 
        // menuItemDelete
        // 
        this.menuItemDelete.Name = "menuItemDelete";
        this.menuItemDelete.Size = new System.Drawing.Size(231, 22);
        this.menuItemDelete.Text = "✕ Delete Channel";
        this.menuItemDelete.Visible = false;
        this.menuItemDelete.Click += new System.EventHandler(this.BtnDeleteChannel_Click);
        // 
        // RemoteMainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(984, 541);
        this.Controls.Add(this.gridStreams);
        this.Controls.Add(this.statusStrip);
        this.Controls.Add(this.toolStrip);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.MinimumSize = new System.Drawing.Size(750, 400);
        this.Name = "RemoteMainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Icecast Stream Monitor - Remote Client (Stateless)";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteMainForm_FormClosing);
        this.Load += new System.EventHandler(this.RemoteMainForm_Load);
        this.toolStrip.ResumeLayout(false);
        this.toolStrip.PerformLayout();
        this.statusStrip.ResumeLayout(false);
        this.statusStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridStreams)).EndInit();
        this.contextMenu.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStrip;
    private System.Windows.Forms.ToolStripButton btnToolRefresh;
    private System.Windows.Forms.ToolStripButton btnToolAutoRefresh;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton btnToolAnalytics;
    private System.Windows.Forms.ToolStripButton btnToolListeners;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton btnToolAddChannel;
    private System.Windows.Forms.ToolStripButton btnToolDeleteChannel;
    private System.Windows.Forms.ToolStripLabel lblUserBadge;
    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel statusLabelServer;
    private System.Windows.Forms.ToolStripStatusLabel statusLabelSpring;
    private System.Windows.Forms.ToolStripStatusLabel statusLabelSummary;
    private System.Windows.Forms.DataGridView gridStreams;
    private System.Windows.Forms.ContextMenuStrip contextMenu;
    private System.Windows.Forms.ToolStripMenuItem menuItemAnalytics;
    private System.Windows.Forms.ToolStripMenuItem menuItemListeners;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem menuItemRefresh;
    private System.Windows.Forms.ToolStripMenuItem menuItemDelete;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colMount;
    private System.Windows.Forms.DataGridViewTextBoxColumn colListeners;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPeak;
    private System.Windows.Forms.DataGridViewTextBoxColumn colBitrate;
    private System.Windows.Forms.DataGridViewTextBoxColumn colFormat;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLatency;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
}
