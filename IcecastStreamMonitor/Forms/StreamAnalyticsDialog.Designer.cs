namespace IcecastStreamMonitor.Forms;

partial class StreamAnalyticsDialog
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
        this.pnlMetrics = new System.Windows.Forms.Panel();
        this.pnlMetric5 = new System.Windows.Forms.Panel();
        this.lblValLatency = new System.Windows.Forms.Label();
        this.lblTitleLatency = new System.Windows.Forms.Label();
        this.pnlMetric4 = new System.Windows.Forms.Panel();
        this.lblValUptime = new System.Windows.Forms.Label();
        this.lblTitleUptime = new System.Windows.Forms.Label();
        this.pnlMetric3 = new System.Windows.Forms.Panel();
        this.lblValData = new System.Windows.Forms.Label();
        this.lblTitleData = new System.Windows.Forms.Label();
        this.pnlMetric2 = new System.Windows.Forms.Panel();
        this.lblValAvg = new System.Windows.Forms.Label();
        this.lblTitleAvg = new System.Windows.Forms.Label();
        this.pnlMetric1 = new System.Windows.Forms.Panel();
        this.lblValPeak = new System.Windows.Forms.Label();
        this.lblTitlePeak = new System.Windows.Forms.Label();
        this.tabCharts = new System.Windows.Forms.TabControl();
        this.tabListeners = new System.Windows.Forms.TabPage();
        this.chartListeners = new IcecastStreamMonitor.Controls.HistoricalChartControl();
        this.tabData = new System.Windows.Forms.TabPage();
        this.chartData = new IcecastStreamMonitor.Controls.HistoricalChartControl();
        this.tabLatency = new System.Windows.Forms.TabPage();
        this.chartLatency = new IcecastStreamMonitor.Controls.HistoricalChartControl();
        this.tabTable = new System.Windows.Forms.TabPage();
        this.gridLogs = new System.Windows.Forms.DataGridView();
        this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colLogStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colLogListeners = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colLogBitrate = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colLogData = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colLogLatency = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colLogTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.pnlBottom = new System.Windows.Forms.Panel();
        this.btnRefresh = new System.Windows.Forms.Button();
        this.btnExportCsv = new System.Windows.Forms.Button();
        this.btnClose = new System.Windows.Forms.Button();
        this.pnlHeader.SuspendLayout();
        this.pnlMetrics.SuspendLayout();
        this.pnlMetric5.SuspendLayout();
        this.pnlMetric4.SuspendLayout();
        this.pnlMetric3.SuspendLayout();
        this.pnlMetric2.SuspendLayout();
        this.pnlMetric1.SuspendLayout();
        this.tabCharts.SuspendLayout();
        this.tabListeners.SuspendLayout();
        this.tabData.SuspendLayout();
        this.tabLatency.SuspendLayout();
        this.tabTable.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridLogs)).BeginInit();
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
        this.pnlHeader.Size = new System.Drawing.Size(884, 55);
        this.pnlHeader.TabIndex = 0;
        // 
        // lblStreamInfo
        // 
        this.lblStreamInfo.AutoSize = true;
        this.lblStreamInfo.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblStreamInfo.Location = new System.Drawing.Point(16, 32);
        this.lblStreamInfo.Name = "lblStreamInfo";
        this.lblStreamInfo.Size = new System.Drawing.Size(271, 15);
        this.lblStreamInfo.TabIndex = 1;
        this.lblStreamInfo.Text = "Server: 127.0.0.1:8000 | Mount: /live | MP3 320 kbps";
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.Location = new System.Drawing.Point(16, 9);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(288, 20);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "24-Hour Stream Analytics & Event Logs";
        // 
        // pnlMetrics
        // 
        this.pnlMetrics.Controls.Add(this.pnlMetric5);
        this.pnlMetrics.Controls.Add(this.pnlMetric4);
        this.pnlMetrics.Controls.Add(this.pnlMetric3);
        this.pnlMetrics.Controls.Add(this.pnlMetric2);
        this.pnlMetrics.Controls.Add(this.pnlMetric1);
        this.pnlMetrics.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlMetrics.Location = new System.Drawing.Point(0, 55);
        this.pnlMetrics.Name = "pnlMetrics";
        this.pnlMetrics.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
        this.pnlMetrics.Size = new System.Drawing.Size(884, 75);
        this.pnlMetrics.TabIndex = 1;
        // 
        // pnlMetric5
        // 
        this.pnlMetric5.BackColor = System.Drawing.Color.White;
        this.pnlMetric5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pnlMetric5.Controls.Add(this.lblValLatency);
        this.pnlMetric5.Controls.Add(this.lblTitleLatency);
        this.pnlMetric5.Location = new System.Drawing.Point(704, 8);
        this.pnlMetric5.Name = "pnlMetric5";
        this.pnlMetric5.Size = new System.Drawing.Size(165, 58);
        this.pnlMetric5.TabIndex = 4;
        // 
        // lblValLatency
        // 
        this.lblValLatency.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblValLatency.ForeColor = System.Drawing.Color.FromArgb(235, 115, 25);
        this.lblValLatency.Location = new System.Drawing.Point(6, 26);
        this.lblValLatency.Name = "lblValLatency";
        this.lblValLatency.Size = new System.Drawing.Size(150, 25);
        this.lblValLatency.TabIndex = 1;
        this.lblValLatency.Text = "0 ms";
        // 
        // lblTitleLatency
        // 
        this.lblTitleLatency.AutoSize = true;
        this.lblTitleLatency.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblTitleLatency.Location = new System.Drawing.Point(6, 6);
        this.lblTitleLatency.Name = "lblTitleLatency";
        this.lblTitleLatency.Size = new System.Drawing.Size(76, 15);
        this.lblTitleLatency.TabIndex = 0;
        this.lblTitleLatency.Text = "AVG LATENCY";
        // 
        // pnlMetric4
        // 
        this.pnlMetric4.BackColor = System.Drawing.Color.White;
        this.pnlMetric4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pnlMetric4.Controls.Add(this.lblValUptime);
        this.pnlMetric4.Controls.Add(this.lblTitleUptime);
        this.pnlMetric4.Location = new System.Drawing.Point(531, 8);
        this.pnlMetric4.Name = "pnlMetric4";
        this.pnlMetric4.Size = new System.Drawing.Size(165, 58);
        this.pnlMetric4.TabIndex = 3;
        // 
        // lblValUptime
        // 
        this.lblValUptime.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblValUptime.ForeColor = System.Drawing.Color.FromArgb(46, 170, 80);
        this.lblValUptime.Location = new System.Drawing.Point(6, 26);
        this.lblValUptime.Name = "lblValUptime";
        this.lblValUptime.Size = new System.Drawing.Size(150, 25);
        this.lblValUptime.TabIndex = 1;
        this.lblValUptime.Text = "100.0 %";
        // 
        // lblTitleUptime
        // 
        this.lblTitleUptime.AutoSize = true;
        this.lblTitleUptime.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblTitleUptime.Location = new System.Drawing.Point(6, 6);
        this.lblTitleUptime.Name = "lblTitleUptime";
        this.lblTitleUptime.Size = new System.Drawing.Size(74, 15);
        this.lblTitleUptime.TabIndex = 0;
        this.lblTitleUptime.Text = "24H UPTIME";
        // 
        // pnlMetric3
        // 
        this.pnlMetric3.BackColor = System.Drawing.Color.White;
        this.pnlMetric3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pnlMetric3.Controls.Add(this.lblValData);
        this.pnlMetric3.Controls.Add(this.lblTitleData);
        this.pnlMetric3.Location = new System.Drawing.Point(358, 8);
        this.pnlMetric3.Name = "pnlMetric3";
        this.pnlMetric3.Size = new System.Drawing.Size(165, 58);
        this.pnlMetric3.TabIndex = 2;
        // 
        // lblValData
        // 
        this.lblValData.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblValData.ForeColor = System.Drawing.Color.FromArgb(40, 50, 65);
        this.lblValData.Location = new System.Drawing.Point(6, 26);
        this.lblValData.Name = "lblValData";
        this.lblValData.Size = new System.Drawing.Size(150, 25);
        this.lblValData.TabIndex = 1;
        this.lblValData.Text = "0.0 MB";
        // 
        // lblTitleData
        // 
        this.lblTitleData.AutoSize = true;
        this.lblTitleData.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblTitleData.Location = new System.Drawing.Point(6, 6);
        this.lblTitleData.Name = "lblTitleData";
        this.lblTitleData.Size = new System.Drawing.Size(100, 15);
        this.lblTitleData.TabIndex = 0;
        this.lblTitleData.Text = "TOTAL TRANSFER";
        // 
        // pnlMetric2
        // 
        this.pnlMetric2.BackColor = System.Drawing.Color.White;
        this.pnlMetric2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pnlMetric2.Controls.Add(this.lblValAvg);
        this.pnlMetric2.Controls.Add(this.lblTitleAvg);
        this.pnlMetric2.Location = new System.Drawing.Point(185, 8);
        this.pnlMetric2.Name = "pnlMetric2";
        this.pnlMetric2.Size = new System.Drawing.Size(165, 58);
        this.pnlMetric2.TabIndex = 1;
        // 
        // lblValAvg
        // 
        this.lblValAvg.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblValAvg.ForeColor = System.Drawing.Color.FromArgb(0, 122, 255);
        this.lblValAvg.Location = new System.Drawing.Point(6, 26);
        this.lblValAvg.Name = "lblValAvg";
        this.lblValAvg.Size = new System.Drawing.Size(150, 25);
        this.lblValAvg.TabIndex = 1;
        this.lblValAvg.Text = "0";
        // 
        // lblTitleAvg
        // 
        this.lblTitleAvg.AutoSize = true;
        this.lblTitleAvg.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblTitleAvg.Location = new System.Drawing.Point(6, 6);
        this.lblTitleAvg.Name = "lblTitleAvg";
        this.lblTitleAvg.Size = new System.Drawing.Size(89, 15);
        this.lblTitleAvg.TabIndex = 0;
        this.lblTitleAvg.Text = "AVG LISTENERS";
        // 
        // pnlMetric1
        // 
        this.pnlMetric1.BackColor = System.Drawing.Color.White;
        this.pnlMetric1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pnlMetric1.Controls.Add(this.lblValPeak);
        this.pnlMetric1.Controls.Add(this.lblTitlePeak);
        this.pnlMetric1.Location = new System.Drawing.Point(12, 8);
        this.pnlMetric1.Name = "pnlMetric1";
        this.pnlMetric1.Size = new System.Drawing.Size(165, 58);
        this.pnlMetric1.TabIndex = 0;
        // 
        // lblValPeak
        // 
        this.lblValPeak.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblValPeak.ForeColor = System.Drawing.Color.FromArgb(0, 122, 255);
        this.lblValPeak.Location = new System.Drawing.Point(6, 26);
        this.lblValPeak.Name = "lblValPeak";
        this.lblValPeak.Size = new System.Drawing.Size(150, 25);
        this.lblValPeak.TabIndex = 1;
        this.lblValPeak.Text = "0";
        // 
        // lblTitlePeak
        // 
        this.lblTitlePeak.AutoSize = true;
        this.lblTitlePeak.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblTitlePeak.Location = new System.Drawing.Point(6, 6);
        this.lblTitlePeak.Name = "lblTitlePeak";
        this.lblTitlePeak.Size = new System.Drawing.Size(96, 15);
        this.lblTitlePeak.TabIndex = 0;
        this.lblTitlePeak.Text = "PEAK LISTENERS";
        // 
        // tabCharts
        // 
        this.tabCharts.Controls.Add(this.tabListeners);
        this.tabCharts.Controls.Add(this.tabData);
        this.tabCharts.Controls.Add(this.tabLatency);
        this.tabCharts.Controls.Add(this.tabTable);
        this.tabCharts.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabCharts.Location = new System.Drawing.Point(0, 130);
        this.tabCharts.Name = "tabCharts";
        this.tabCharts.SelectedIndex = 0;
        this.tabCharts.Size = new System.Drawing.Size(884, 381);
        this.tabCharts.TabIndex = 2;
        // 
        // tabListeners
        // 
        this.tabListeners.Controls.Add(this.chartListeners);
        this.tabListeners.Location = new System.Drawing.Point(4, 24);
        this.tabListeners.Name = "tabListeners";
        this.tabListeners.Padding = new System.Windows.Forms.Padding(3);
        this.tabListeners.Size = new System.Drawing.Size(876, 353);
        this.tabListeners.TabIndex = 0;
        this.tabListeners.Text = "📈 Concurrent Listeners (24h)";
        this.tabListeners.UseVisualStyleBackColor = true;
        // 
        // chartListeners
        // 
        this.chartListeners.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
        this.chartListeners.Dock = System.Windows.Forms.DockStyle.Fill;
        this.chartListeners.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.chartListeners.Location = new System.Drawing.Point(3, 3);
        this.chartListeners.MetricType = IcecastStreamMonitor.Controls.ChartMetricType.Listeners;
        this.chartListeners.Name = "chartListeners";
        this.chartListeners.Size = new System.Drawing.Size(870, 347);
        this.chartListeners.TabIndex = 0;
        this.chartListeners.Text = "chartListeners";
        // 
        // tabData
        // 
        this.tabData.Controls.Add(this.chartData);
        this.tabData.Location = new System.Drawing.Point(4, 24);
        this.tabData.Name = "tabData";
        this.tabData.Padding = new System.Windows.Forms.Padding(3);
        this.tabData.Size = new System.Drawing.Size(876, 353);
        this.tabData.TabIndex = 1;
        this.tabData.Text = "📊 Data Transfer & Bandwidth";
        this.tabData.UseVisualStyleBackColor = true;
        // 
        // chartData
        // 
        this.chartData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
        this.chartData.Dock = System.Windows.Forms.DockStyle.Fill;
        this.chartData.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.chartData.Location = new System.Drawing.Point(3, 3);
        this.chartData.MetricType = IcecastStreamMonitor.Controls.ChartMetricType.DataTransferMb;
        this.chartData.Name = "chartData";
        this.chartData.Size = new System.Drawing.Size(870, 347);
        this.chartData.TabIndex = 0;
        this.chartData.Text = "chartData";
        // 
        // tabLatency
        // 
        this.tabLatency.Controls.Add(this.chartLatency);
        this.tabLatency.Location = new System.Drawing.Point(4, 24);
        this.tabLatency.Name = "tabLatency";
        this.tabLatency.Padding = new System.Windows.Forms.Padding(3);
        this.tabLatency.Size = new System.Drawing.Size(876, 353);
        this.tabLatency.TabIndex = 2;
        this.tabLatency.Text = "⚡ Response Latency (Ping)";
        this.tabLatency.UseVisualStyleBackColor = true;
        // 
        // chartLatency
        // 
        this.chartLatency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
        this.chartLatency.Dock = System.Windows.Forms.DockStyle.Fill;
        this.chartLatency.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.chartLatency.Location = new System.Drawing.Point(3, 3);
        this.chartLatency.MetricType = IcecastStreamMonitor.Controls.ChartMetricType.LatencyMs;
        this.chartLatency.Name = "chartLatency";
        this.chartLatency.Size = new System.Drawing.Size(870, 347);
        this.chartLatency.TabIndex = 0;
        this.chartLatency.Text = "chartLatency";
        // 
        // tabTable
        // 
        this.tabTable.Controls.Add(this.gridLogs);
        this.tabTable.Location = new System.Drawing.Point(4, 24);
        this.tabTable.Name = "tabTable";
        this.tabTable.Padding = new System.Windows.Forms.Padding(3);
        this.tabTable.Size = new System.Drawing.Size(876, 353);
        this.tabTable.TabIndex = 3;
        this.tabTable.Text = "📝 24h Event Logs Table";
        this.tabTable.UseVisualStyleBackColor = true;
        // 
        // gridLogs
        // 
        this.gridLogs.AllowUserToAddRows = false;
        this.gridLogs.AllowUserToDeleteRows = false;
        this.gridLogs.AllowUserToResizeRows = false;
        this.gridLogs.BackgroundColor = System.Drawing.SystemColors.Window;
        this.gridLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.gridLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.gridLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTime,
            this.colLogStatus,
            this.colLogListeners,
            this.colLogBitrate,
            this.colLogData,
            this.colLogLatency,
            this.colLogTitle});
        this.gridLogs.Dock = System.Windows.Forms.DockStyle.Fill;
        this.gridLogs.Location = new System.Drawing.Point(3, 3);
        this.gridLogs.MultiSelect = false;
        this.gridLogs.Name = "gridLogs";
        this.gridLogs.ReadOnly = true;
        this.gridLogs.RowHeadersVisible = false;
        this.gridLogs.RowTemplate.Height = 25;
        this.gridLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.gridLogs.Size = new System.Drawing.Size(870, 347);
        this.gridLogs.TabIndex = 0;
        // 
        // colTime
        // 
        this.colTime.HeaderText = "Timestamp";
        this.colTime.Name = "colTime";
        this.colTime.ReadOnly = true;
        this.colTime.Width = 140;
        // 
        // colLogStatus
        // 
        this.colLogStatus.HeaderText = "Status";
        this.colLogStatus.Name = "colLogStatus";
        this.colLogStatus.ReadOnly = true;
        this.colLogStatus.Width = 85;
        // 
        // colLogListeners
        // 
        this.colLogListeners.HeaderText = "Listeners";
        this.colLogListeners.Name = "colLogListeners";
        this.colLogListeners.ReadOnly = true;
        this.colLogListeners.Width = 85;
        // 
        // colLogBitrate
        // 
        this.colLogBitrate.HeaderText = "Bit Rate";
        this.colLogBitrate.Name = "colLogBitrate";
        this.colLogBitrate.ReadOnly = true;
        this.colLogBitrate.Width = 85;
        // 
        // colLogData
        // 
        this.colLogData.HeaderText = "Data (MB)";
        this.colLogData.Name = "colLogData";
        this.colLogData.ReadOnly = true;
        this.colLogData.Width = 90;
        // 
        // colLogLatency
        // 
        this.colLogLatency.HeaderText = "Latency";
        this.colLogLatency.Name = "colLogLatency";
        this.colLogLatency.ReadOnly = true;
        this.colLogLatency.Width = 80;
        // 
        // colLogTitle
        // 
        this.colLogTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.colLogTitle.HeaderText = "Metadata / Song Title";
        this.colLogTitle.Name = "colLogTitle";
        this.colLogTitle.ReadOnly = true;
        // 
        // pnlBottom
        // 
        this.pnlBottom.Controls.Add(this.btnRefresh);
        this.pnlBottom.Controls.Add(this.btnExportCsv);
        this.pnlBottom.Controls.Add(this.btnClose);
        this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnlBottom.Location = new System.Drawing.Point(0, 511);
        this.pnlBottom.Name = "pnlBottom";
        this.pnlBottom.Size = new System.Drawing.Size(884, 50);
        this.pnlBottom.TabIndex = 3;
        // 
        // btnRefresh
        // 
        this.btnRefresh.Location = new System.Drawing.Point(16, 11);
        this.btnRefresh.Name = "btnRefresh";
        this.btnRefresh.Size = new System.Drawing.Size(100, 28);
        this.btnRefresh.TabIndex = 0;
        this.btnRefresh.Text = "⟳ Refresh";
        this.btnRefresh.UseVisualStyleBackColor = true;
        this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
        // 
        // btnExportCsv
        // 
        this.btnExportCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnExportCsv.Location = new System.Drawing.Point(670, 11);
        this.btnExportCsv.Name = "btnExportCsv";
        this.btnExportCsv.Size = new System.Drawing.Size(115, 28);
        this.btnExportCsv.TabIndex = 1;
        this.btnExportCsv.Text = "📋 Export CSV";
        this.btnExportCsv.UseVisualStyleBackColor = true;
        this.btnExportCsv.Click += new System.EventHandler(this.BtnExportCsv_Click);
        // 
        // btnClose
        // 
        this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnClose.Location = new System.Drawing.Point(793, 11);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(75, 28);
        this.btnClose.TabIndex = 2;
        this.btnClose.Text = "Close";
        this.btnClose.UseVisualStyleBackColor = true;
        // 
        // StreamAnalyticsDialog
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(884, 561);
        this.Controls.Add(this.tabCharts);
        this.Controls.Add(this.pnlBottom);
        this.Controls.Add(this.pnlMetrics);
        this.Controls.Add(this.pnlHeader);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.MinimumSize = new System.Drawing.Size(750, 480);
        this.Name = "StreamAnalyticsDialog";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "24-Hour Stream Analytics & Logs";
        this.Load += new System.EventHandler(this.StreamAnalyticsDialog_Load);
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        this.pnlMetrics.ResumeLayout(false);
        this.pnlMetric5.ResumeLayout(false);
        this.pnlMetric5.PerformLayout();
        this.pnlMetric4.ResumeLayout(false);
        this.pnlMetric4.PerformLayout();
        this.pnlMetric3.ResumeLayout(false);
        this.pnlMetric3.PerformLayout();
        this.pnlMetric2.ResumeLayout(false);
        this.pnlMetric2.PerformLayout();
        this.pnlMetric1.ResumeLayout(false);
        this.pnlMetric1.PerformLayout();
        this.tabCharts.ResumeLayout(false);
        this.tabListeners.ResumeLayout(false);
        this.tabData.ResumeLayout(false);
        this.tabLatency.ResumeLayout(false);
        this.tabTable.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.gridLogs)).EndInit();
        this.pnlBottom.ResumeLayout(false);
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblStreamInfo;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Panel pnlMetrics;
    private System.Windows.Forms.Panel pnlMetric1;
    private System.Windows.Forms.Label lblValPeak;
    private System.Windows.Forms.Label lblTitlePeak;
    private System.Windows.Forms.Panel pnlMetric2;
    private System.Windows.Forms.Label lblValAvg;
    private System.Windows.Forms.Label lblTitleAvg;
    private System.Windows.Forms.Panel pnlMetric3;
    private System.Windows.Forms.Label lblValData;
    private System.Windows.Forms.Label lblTitleData;
    private System.Windows.Forms.Panel pnlMetric4;
    private System.Windows.Forms.Label lblValUptime;
    private System.Windows.Forms.Label lblTitleUptime;
    private System.Windows.Forms.Panel pnlMetric5;
    private System.Windows.Forms.Label lblValLatency;
    private System.Windows.Forms.Label lblTitleLatency;
    private System.Windows.Forms.TabControl tabCharts;
    private System.Windows.Forms.TabPage tabListeners;
    private IcecastStreamMonitor.Controls.HistoricalChartControl chartListeners;
    private System.Windows.Forms.TabPage tabData;
    private IcecastStreamMonitor.Controls.HistoricalChartControl chartData;
    private System.Windows.Forms.TabPage tabLatency;
    private IcecastStreamMonitor.Controls.HistoricalChartControl chartLatency;
    private System.Windows.Forms.TabPage tabTable;
    private System.Windows.Forms.DataGridView gridLogs;
    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnExportCsv;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLogStatus;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLogListeners;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLogBitrate;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLogData;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLogLatency;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLogTitle;
}
