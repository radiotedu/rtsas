namespace IcecastStreamMonitor.Forms;

partial class MainForm
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
        this.menuStrip = new System.Windows.Forms.MenuStrip();
        this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemAdd = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemImportTinyIce = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.menuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
        this.menuChannels = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemAnalytics = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemListeners = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
        this.menuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemDuplicate = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemToggle = new System.Windows.Forms.ToolStripMenuItem();
        this.menuTools = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemAutoRefresh = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemToggleConsole = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemRemoteUsers = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemMockServer = new System.Windows.Forms.ToolStripMenuItem();
        this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemCheckUpdates = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStrip = new System.Windows.Forms.ToolStrip();
        this.btnToolAdd = new System.Windows.Forms.ToolStripButton();
        this.btnToolEdit = new System.Windows.Forms.ToolStripButton();
        this.btnToolDelete = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        this.btnToolAnalytics = new System.Windows.Forms.ToolStripButton();
        this.btnToolListeners = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
        this.btnToolRefresh = new System.Windows.Forms.ToolStripButton();
        this.btnToolAutoRefresh = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
        this.btnToolSettings = new System.Windows.Forms.ToolStripButton();
        this.btnToolConsole = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
        this.btnToolMockServer = new System.Windows.Forms.ToolStripButton();
        this.statusStrip = new System.Windows.Forms.StatusStrip();
        this.statusLabelMain = new System.Windows.Forms.ToolStripStatusLabel();
        this.statusLabelTotalStreams = new System.Windows.Forms.ToolStripStatusLabel();
        this.statusLabelActiveStreams = new System.Windows.Forms.ToolStripStatusLabel();
        this.statusLabelTotalListeners = new System.Windows.Forms.ToolStripStatusLabel();
        this.statusLabelNextRefresh = new System.Windows.Forms.ToolStripStatusLabel();
        this.splitContainerMain = new System.Windows.Forms.SplitContainer();
        this.gridStreams = new System.Windows.Forms.DataGridView();
        this.colEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
        this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colMount = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colServer = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colListeners = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colPeak = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colFormat = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colBitrate = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colAuth = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colLatency = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colLastChecked = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.gridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.contextItemAnalytics = new System.Windows.Forms.ToolStripMenuItem();
        this.contextItemListeners = new System.Windows.Forms.ToolStripMenuItem();
        this.contextSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.contextItemEdit = new System.Windows.Forms.ToolStripMenuItem();
        this.contextItemDelete = new System.Windows.Forms.ToolStripMenuItem();
        this.contextItemDuplicate = new System.Windows.Forms.ToolStripMenuItem();
        this.contextItemToggle = new System.Windows.Forms.ToolStripMenuItem();
        this.contextSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.contextItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
        this.pnlConsoleHeader = new System.Windows.Forms.Panel();
        this.lblConsoleTitle = new System.Windows.Forms.Label();
        this.cboAuditFilter = new System.Windows.Forms.ComboBox();
        this.btnCopyConsole = new System.Windows.Forms.Button();
        this.btnClearConsole = new System.Windows.Forms.Button();
        this.gridAuditLogs = new System.Windows.Forms.DataGridView();
        this.colAuditTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colAuditType = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colAuditChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colAuditAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colAuditDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
        this.trayContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.trayItemRestore = new System.Windows.Forms.ToolStripMenuItem();
        this.trayItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
        this.trayItemAutoRefresh = new System.Windows.Forms.ToolStripMenuItem();
        this.traySeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.trayItemExit = new System.Windows.Forms.ToolStripMenuItem();
        this.menuStrip.SuspendLayout();
        this.toolStrip.SuspendLayout();
        this.statusStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
        this.splitContainerMain.Panel1.SuspendLayout();
        this.splitContainerMain.Panel2.SuspendLayout();
        this.splitContainerMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridStreams)).BeginInit();
        this.gridContextMenu.SuspendLayout();
        this.pnlConsoleHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridAuditLogs)).BeginInit();
        this.trayContextMenu.SuspendLayout();
        this.SuspendLayout();
        // 
        // menuStrip
        // 
        this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuChannels,
            this.menuTools,
            this.menuHelp});
        this.menuStrip.Location = new System.Drawing.Point(0, 0);
        this.menuStrip.Name = "menuStrip";
        this.menuStrip.Size = new System.Drawing.Size(1040, 24);
        this.menuStrip.TabIndex = 0;
        this.menuStrip.Text = "menuStrip";
        // 
        // menuFile
        // 
        this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAdd,
            this.menuItemImportTinyIce,
            this.menuItemRefresh,
            this.toolStripSeparator1,
            this.menuItemSettings,
            this.toolStripSeparator2,
            this.menuItemExit});
        this.menuFile.Name = "menuFile";
        this.menuFile.Size = new System.Drawing.Size(37, 20);
        this.menuFile.Text = "&File";
        // 
        // menuItemAdd
        // 
        this.menuItemAdd.Name = "menuItemAdd";
        this.menuItemAdd.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
        this.menuItemAdd.Size = new System.Drawing.Size(265, 22);
        this.menuItemAdd.Text = "&Add Channel...";
        this.menuItemAdd.Click += new System.EventHandler(this.BtnAdd_Click);
        // 
        // menuItemImportTinyIce
        // 
        this.menuItemImportTinyIce.Name = "menuItemImportTinyIce";
        this.menuItemImportTinyIce.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
        this.menuItemImportTinyIce.Size = new System.Drawing.Size(265, 22);
        this.menuItemImportTinyIce.Text = "📥 &Import from TinyIce Config...";
        this.menuItemImportTinyIce.Click += new System.EventHandler(this.BtnImportTinyIce_Click);
        // 
        // menuItemRefresh
        // 
        this.menuItemRefresh.Name = "menuItemRefresh";
        this.menuItemRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
        this.menuItemRefresh.Size = new System.Drawing.Size(265, 22);
        this.menuItemRefresh.Text = "&Refresh All";
        this.menuItemRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
        // 
        // toolStripSeparator1
        // 
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(190, 22);
        // 
        // menuItemSettings
        // 
        this.menuItemSettings.Name = "menuItemSettings";
        this.menuItemSettings.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemcomma)));
        this.menuItemSettings.Size = new System.Drawing.Size(193, 22);
        this.menuItemSettings.Text = "&Settings...";
        this.menuItemSettings.Click += new System.EventHandler(this.BtnSettings_Click);
        // 
        // toolStripSeparator2
        // 
        this.toolStripSeparator2.Name = "toolStripSeparator2";
        this.toolStripSeparator2.Size = new System.Drawing.Size(190, 22);
        // 
        // menuItemExit
        // 
        this.menuItemExit.Name = "menuItemExit";
        this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
        this.menuItemExit.Size = new System.Drawing.Size(193, 22);
        this.menuItemExit.Text = "E&xit";
        this.menuItemExit.Click += new System.EventHandler(this.MenuItemExit_Click);
        // 
        // menuChannels
        // 
        this.menuChannels.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAnalytics,
            this.menuItemListeners,
            this.toolStripSeparator6,
            this.menuItemEdit,
            this.menuItemDelete,
            this.menuItemDuplicate,
            this.menuItemToggle});
        this.menuChannels.Name = "menuChannels";
        this.menuChannels.Size = new System.Drawing.Size(68, 20);
        this.menuChannels.Text = "&Channels";
        // 
        // menuItemAnalytics
        // 
        this.menuItemAnalytics.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.menuItemAnalytics.Name = "menuItemAnalytics";
        this.menuItemAnalytics.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
        this.menuItemAnalytics.Size = new System.Drawing.Size(252, 22);
        this.menuItemAnalytics.Text = "📊 Show 24h Logs && Charts...";
        this.menuItemAnalytics.Click += new System.EventHandler(this.BtnAnalytics_Click);
        // 
        // menuItemListeners
        // 
        this.menuItemListeners.Name = "menuItemListeners";
        this.menuItemListeners.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
        this.menuItemListeners.Size = new System.Drawing.Size(252, 22);
        this.menuItemListeners.Text = "👥 Show Listener IPs...";
        this.menuItemListeners.Click += new System.EventHandler(this.BtnShowListeners_Click);
        // 
        // toolStripSeparator6
        // 
        this.toolStripSeparator6.Name = "toolStripSeparator6";
        this.toolStripSeparator6.Size = new System.Drawing.Size(249, 22);
        // 
        // menuItemEdit
        // 
        this.menuItemEdit.Name = "menuItemEdit";
        this.menuItemEdit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
        this.menuItemEdit.Size = new System.Drawing.Size(252, 22);
        this.menuItemEdit.Text = "&Edit Channel...";
        this.menuItemEdit.Click += new System.EventHandler(this.BtnEdit_Click);
        // 
        // menuItemDelete
        // 
        this.menuItemDelete.Name = "menuItemDelete";
        this.menuItemDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
        this.menuItemDelete.Size = new System.Drawing.Size(252, 22);
        this.menuItemDelete.Text = "&Delete Channel";
        this.menuItemDelete.Click += new System.EventHandler(this.BtnDelete_Click);
        // 
        // menuItemDuplicate
        // 
        this.menuItemDuplicate.Name = "menuItemDuplicate";
        this.menuItemDuplicate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
        this.menuItemDuplicate.Size = new System.Drawing.Size(252, 22);
        this.menuItemDuplicate.Text = "D&uplicate";
        this.menuItemDuplicate.Click += new System.EventHandler(this.BtnDuplicate_Click);
        // 
        // menuItemToggle
        // 
        this.menuItemToggle.Name = "menuItemToggle";
        this.menuItemToggle.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
        this.menuItemToggle.Size = new System.Drawing.Size(252, 22);
        this.menuItemToggle.Text = "&Toggle Active";
        this.menuItemToggle.Click += new System.EventHandler(this.BtnToggleActive_Click);
        // 
        // menuTools
        // 
        this.menuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAutoRefresh,
            this.menuItemToggleConsole,
            this.menuItemRemoteUsers,
            this.menuItemMockServer});
        this.menuTools.Name = "menuTools";
        this.menuTools.Size = new System.Drawing.Size(46, 20);
        this.menuTools.Text = "&Tools";
        // 
        // menuItemAutoRefresh
        // 
        this.menuItemAutoRefresh.CheckOnClick = true;
        this.menuItemAutoRefresh.Name = "menuItemAutoRefresh";
        this.menuItemAutoRefresh.Size = new System.Drawing.Size(245, 22);
        this.menuItemAutoRefresh.Text = "Auto-Refresh Enabled";
        this.menuItemAutoRefresh.Click += new System.EventHandler(this.BtnAutoRefresh_Click);
        // 
        // menuItemToggleConsole
        // 
        this.menuItemToggleConsole.Checked = true;
        this.menuItemToggleConsole.CheckOnClick = true;
        this.menuItemToggleConsole.CheckState = System.Windows.Forms.CheckState.Checked;
        this.menuItemToggleConsole.Name = "menuItemToggleConsole";
        this.menuItemToggleConsole.Size = new System.Drawing.Size(245, 22);
        this.menuItemToggleConsole.Text = "Show Audit / Change Console";
        this.menuItemToggleConsole.Click += new System.EventHandler(this.BtnToggleConsole_Click);
        // 
        // menuItemRemoteUsers
        // 
        this.menuItemRemoteUsers.Name = "menuItemRemoteUsers";
        this.menuItemRemoteUsers.Size = new System.Drawing.Size(245, 22);
        this.menuItemRemoteUsers.Text = "👥 Remote Users && Access Control...";
        this.menuItemRemoteUsers.Click += new System.EventHandler(this.BtnRemoteUsers_Click);
        // 
        // menuItemMockServer
        // 
        this.menuItemMockServer.Name = "menuItemMockServer";
        this.menuItemMockServer.Size = new System.Drawing.Size(245, 22);
        this.menuItemMockServer.Text = "Toggle Local Demo Server";
        this.menuItemMockServer.Click += new System.EventHandler(this.BtnMockServer_Click);
        // 
        // menuHelp
        // 
        this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCheckUpdates,
            this.menuItemAbout});
        this.menuHelp.Name = "menuHelp";
        this.menuHelp.Size = new System.Drawing.Size(44, 20);
        this.menuHelp.Text = "&Help";
        // 
        // menuItemCheckUpdates
        // 
        this.menuItemCheckUpdates.Name = "menuItemCheckUpdates";
        this.menuItemCheckUpdates.Size = new System.Drawing.Size(190, 22);
        this.menuItemCheckUpdates.Text = "Check for &Updates...";
        this.menuItemCheckUpdates.Click += new System.EventHandler(this.BtnCheckUpdates_Click);
        // 
        // menuItemAbout
        // 
        this.menuItemAbout.Name = "menuItemAbout";
        this.menuItemAbout.Size = new System.Drawing.Size(190, 22);
        this.menuItemAbout.Text = "&About";
        this.menuItemAbout.Click += new System.EventHandler(this.MenuItemAbout_Click);
        // 
        // toolStrip
        // 
        this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
        this.toolStrip.ImageScalingSize = new System.Drawing.Size(16, 16);
        this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnToolAdd,
            this.btnToolEdit,
            this.btnToolDelete,
            this.toolStripSeparator3,
            this.btnToolAnalytics,
            this.btnToolListeners,
            this.toolStripSeparator7,
            this.btnToolRefresh,
            this.btnToolAutoRefresh,
            this.toolStripSeparator4,
            this.btnToolSettings,
            this.btnToolConsole,
            this.toolStripSeparator5,
            this.btnToolMockServer});
        this.toolStrip.Location = new System.Drawing.Point(0, 24);
        this.toolStrip.Name = "toolStrip";
        this.toolStrip.Size = new System.Drawing.Size(1040, 25);
        this.toolStrip.TabIndex = 1;
        this.toolStrip.Text = "toolStrip";
        // 
        // btnToolAdd
        // 
        this.btnToolAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnToolAdd.Name = "btnToolAdd";
        this.btnToolAdd.Size = new System.Drawing.Size(96, 22);
        this.btnToolAdd.Text = "+ Add Channel";
        this.btnToolAdd.Click += new System.EventHandler(this.BtnAdd_Click);
        // 
        // btnToolEdit
        // 
        this.btnToolEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnToolEdit.Name = "btnToolEdit";
        this.btnToolEdit.Size = new System.Drawing.Size(56, 22);
        this.btnToolEdit.Text = "✎ Edit";
        this.btnToolEdit.Click += new System.EventHandler(this.BtnEdit_Click);
        // 
        // btnToolDelete
        // 
        this.btnToolDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnToolDelete.Name = "btnToolDelete";
        this.btnToolDelete.Size = new System.Drawing.Size(68, 22);
        this.btnToolDelete.Text = "✕ Delete";
        this.btnToolDelete.Click += new System.EventHandler(this.BtnDelete_Click);
        // 
        // toolStripSeparator3
        // 
        this.toolStripSeparator3.Name = "toolStripSeparator3";
        this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
        // 
        // btnToolAnalytics
        // 
        this.btnToolAnalytics.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnToolAnalytics.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnToolAnalytics.Name = "btnToolAnalytics";
        this.btnToolAnalytics.Size = new System.Drawing.Size(130, 22);
        this.btnToolAnalytics.Text = "📊 24h Logs & Charts";
        this.btnToolAnalytics.Click += new System.EventHandler(this.BtnAnalytics_Click);
        // 
        // btnToolListeners
        // 
        this.btnToolListeners.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnToolListeners.Name = "btnToolListeners";
        this.btnToolListeners.Size = new System.Drawing.Size(102, 22);
        this.btnToolListeners.Text = "👥 Listener IPs";
        this.btnToolListeners.Click += new System.EventHandler(this.BtnShowListeners_Click);
        // 
        // toolStripSeparator7
        // 
        this.toolStripSeparator7.Name = "toolStripSeparator7";
        this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
        // 
        // btnToolRefresh
        // 
        this.btnToolRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnToolRefresh.Name = "btnToolRefresh";
        this.btnToolRefresh.Size = new System.Drawing.Size(95, 22);
        this.btnToolRefresh.Text = "⟳ Refresh (F5)";
        this.btnToolRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
        // 
        // btnToolAutoRefresh
        // 
        this.btnToolAutoRefresh.CheckOnClick = true;
        this.btnToolAutoRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnToolAutoRefresh.Name = "btnToolAutoRefresh";
        this.btnToolAutoRefresh.Size = new System.Drawing.Size(126, 22);
        this.btnToolAutoRefresh.Text = "▶ Auto-Monitor: ON";
        this.btnToolAutoRefresh.Click += new System.EventHandler(this.BtnAutoRefresh_Click);
        // 
        // toolStripSeparator4
        // 
        this.toolStripSeparator4.Name = "toolStripSeparator4";
        this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
        // 
        // btnToolSettings
        // 
        this.btnToolSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnToolSettings.Name = "btnToolSettings";
        this.btnToolSettings.Size = new System.Drawing.Size(77, 22);
        this.btnToolSettings.Text = "⚙ Settings";
        this.btnToolSettings.Click += new System.EventHandler(this.BtnSettings_Click);
        // 
        // btnToolConsole
        // 
        this.btnToolConsole.Checked = true;
        this.btnToolConsole.CheckOnClick = true;
        this.btnToolConsole.CheckState = System.Windows.Forms.CheckState.Checked;
        this.btnToolConsole.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnToolConsole.Name = "btnToolConsole";
        this.btnToolConsole.Size = new System.Drawing.Size(117, 22);
        this.btnToolConsole.Text = "📋 Change Console";
        this.btnToolConsole.Click += new System.EventHandler(this.BtnToggleConsole_Click);
        // 
        // toolStripSeparator5
        // 
        this.toolStripSeparator5.Name = "toolStripSeparator5";
        this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
        // 
        // btnToolMockServer
        // 
        this.btnToolMockServer.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnToolMockServer.Name = "btnToolMockServer";
        this.btnToolMockServer.Size = new System.Drawing.Size(146, 22);
        this.btnToolMockServer.Text = "⚡ Start Local Demo Feed";
        this.btnToolMockServer.Click += new System.EventHandler(this.BtnMockServer_Click);
        // 
        // statusStrip
        // 
        this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabelMain,
            this.statusLabelTotalStreams,
            this.statusLabelActiveStreams,
            this.statusLabelTotalListeners,
            this.statusLabelNextRefresh});
        this.statusStrip.Location = new System.Drawing.Point(0, 607);
        this.statusStrip.Name = "statusStrip";
        this.statusStrip.Size = new System.Drawing.Size(1040, 24);
        this.statusStrip.TabIndex = 2;
        this.statusStrip.Text = "statusStrip";
        // 
        // statusLabelMain
        // 
        this.statusLabelMain.Name = "statusLabelMain";
        this.statusLabelMain.Size = new System.Drawing.Size(334, 19);
        this.statusLabelMain.Spring = true;
        this.statusLabelMain.Text = "Ready";
        this.statusLabelMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // statusLabelTotalStreams
        // 
        this.statusLabelTotalStreams.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
        this.statusLabelTotalStreams.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
        this.statusLabelTotalStreams.Name = "statusLabelTotalStreams";
        this.statusLabelTotalStreams.Size = new System.Drawing.Size(89, 19);
        this.statusLabelTotalStreams.Text = "Channels: 0";
        // 
        // statusLabelActiveStreams
        // 
        this.statusLabelActiveStreams.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
        this.statusLabelActiveStreams.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
        this.statusLabelActiveStreams.Name = "statusLabelActiveStreams";
        this.statusLabelActiveStreams.Size = new System.Drawing.Size(73, 19);
        this.statusLabelActiveStreams.Text = "Online: 0";
        // 
        // statusLabelTotalListeners
        // 
        this.statusLabelTotalListeners.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
        this.statusLabelTotalListeners.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
        this.statusLabelTotalListeners.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.statusLabelTotalListeners.Name = "statusLabelTotalListeners";
        this.statusLabelTotalListeners.Size = new System.Drawing.Size(126, 19);
        this.statusLabelTotalListeners.Text = "Total Listeners: 0";
        // 
        // statusLabelNextRefresh
        // 
        this.statusLabelNextRefresh.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
        this.statusLabelNextRefresh.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
        this.statusLabelNextRefresh.Name = "statusLabelNextRefresh";
        this.statusLabelNextRefresh.Size = new System.Drawing.Size(121, 19);
        this.statusLabelNextRefresh.Text = "Refresh: Active (5s)";
        // 
        // splitContainerMain
        // 
        this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
        this.splitContainerMain.Location = new System.Drawing.Point(0, 49);
        this.splitContainerMain.Name = "splitContainerMain";
        this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
        // 
        // splitContainerMain.Panel1
        // 
        this.splitContainerMain.Panel1.Controls.Add(this.gridStreams);
        // 
        // splitContainerMain.Panel2
        // 
        this.splitContainerMain.Panel2.Controls.Add(this.gridAuditLogs);
        this.splitContainerMain.Panel2.Controls.Add(this.pnlConsoleHeader);
        this.splitContainerMain.Size = new System.Drawing.Size(1040, 558);
        this.splitContainerMain.SplitterDistance = 370;
        this.splitContainerMain.TabIndex = 3;
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
            this.colEnabled,
            this.colName,
            this.colMount,
            this.colServer,
            this.colType,
            this.colStatus,
            this.colListeners,
            this.colPeak,
            this.colFormat,
            this.colBitrate,
            this.colAuth,
            this.colLatency,
            this.colLastChecked,
            this.colDetails});
        this.gridStreams.ContextMenuStrip = this.gridContextMenu;
        this.gridStreams.Dock = System.Windows.Forms.DockStyle.Fill;
        this.gridStreams.Location = new System.Drawing.Point(0, 0);
        this.gridStreams.MultiSelect = false;
        this.gridStreams.Name = "gridStreams";
        this.gridStreams.RowHeadersVisible = false;
        this.gridStreams.RowTemplate.Height = 25;
        this.gridStreams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.gridStreams.Size = new System.Drawing.Size(1040, 370);
        this.gridStreams.TabIndex = 4;
        this.gridStreams.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridStreams_CellContentClick);
        this.gridStreams.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridStreams_CellDoubleClick);
        this.gridStreams.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridStreams_CellMouseDown);
        this.gridStreams.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridStreams_KeyDown);
        // 
        // colEnabled
        // 
        this.colEnabled.HeaderText = "Active";
        this.colEnabled.Name = "colEnabled";
        this.colEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        this.colEnabled.Width = 50;
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
        // colServer
        // 
        this.colServer.HeaderText = "Host : Port";
        this.colServer.Name = "colServer";
        this.colServer.ReadOnly = true;
        this.colServer.Width = 130;
        // 
        // colType
        // 
        this.colType.HeaderText = "Type";
        this.colType.Name = "colType";
        this.colType.ReadOnly = true;
        this.colType.Width = 85;
        // 
        // colStatus
        // 
        this.colStatus.HeaderText = "Status";
        this.colStatus.Name = "colStatus";
        this.colStatus.ReadOnly = true;
        this.colStatus.Width = 90;
        // 
        // colListeners
        // 
        this.colListeners.HeaderText = "Listeners";
        this.colListeners.Name = "colListeners";
        this.colListeners.ReadOnly = true;
        this.colListeners.Width = 80;
        // 
        // colPeak
        // 
        this.colPeak.HeaderText = "Peak";
        this.colPeak.Name = "colPeak";
        this.colPeak.ReadOnly = true;
        this.colPeak.Width = 60;
        // 
        // colFormat
        // 
        this.colFormat.HeaderText = "Audio Format";
        this.colFormat.Name = "colFormat";
        this.colFormat.ReadOnly = true;
        this.colFormat.Width = 100;
        // 
        // colBitrate
        // 
        this.colBitrate.HeaderText = "Bit Rate";
        this.colBitrate.Name = "colBitrate";
        this.colBitrate.ReadOnly = true;
        this.colBitrate.Width = 80;
        // 
        // colAuth
        // 
        this.colAuth.HeaderText = "Auth";
        this.colAuth.Name = "colAuth";
        this.colAuth.ReadOnly = true;
        this.colAuth.Width = 75;
        // 
        // colLatency
        // 
        this.colLatency.HeaderText = "Latency";
        this.colLatency.Name = "colLatency";
        this.colLatency.ReadOnly = true;
        this.colLatency.Width = 70;
        // 
        // colLastChecked
        // 
        this.colLastChecked.HeaderText = "Last Update";
        this.colLastChecked.Name = "colLastChecked";
        this.colLastChecked.ReadOnly = true;
        this.colLastChecked.Width = 95;
        // 
        // colDetails
        // 
        this.colDetails.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.colDetails.HeaderText = "Stream Title / Metadata";
        this.colDetails.Name = "colDetails";
        this.colDetails.ReadOnly = true;
        // 
        // gridContextMenu
        // 
        this.gridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextItemAnalytics,
            this.contextItemListeners,
            this.contextSeparator1,
            this.contextItemEdit,
            this.contextItemDelete,
            this.contextItemDuplicate,
            this.contextItemToggle,
            this.contextSeparator2,
            this.contextItemRefresh});
        this.gridContextMenu.Name = "gridContextMenu";
        this.gridContextMenu.Size = new System.Drawing.Size(236, 170);
        // 
        // contextItemAnalytics
        // 
        this.contextItemAnalytics.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.contextItemAnalytics.Name = "contextItemAnalytics";
        this.contextItemAnalytics.Size = new System.Drawing.Size(235, 22);
        this.contextItemAnalytics.Text = "📊 Show 24h Logs && Charts...";
        this.contextItemAnalytics.Click += new System.EventHandler(this.BtnAnalytics_Click);
        // 
        // contextItemListeners
        // 
        this.contextItemListeners.Name = "contextItemListeners";
        this.contextItemListeners.Size = new System.Drawing.Size(235, 22);
        this.contextItemListeners.Text = "👥 Show Connected Listener IPs...";
        this.contextItemListeners.Click += new System.EventHandler(this.BtnShowListeners_Click);
        // 
        // contextSeparator1
        // 
        this.contextSeparator1.Name = "contextSeparator1";
        this.contextSeparator1.Size = new System.Drawing.Size(232, 6);
        // 
        // contextItemEdit
        // 
        this.contextItemEdit.Name = "contextItemEdit";
        this.contextItemEdit.Size = new System.Drawing.Size(235, 22);
        this.contextItemEdit.Text = "✎ Edit Channel...";
        this.contextItemEdit.Click += new System.EventHandler(this.BtnEdit_Click);
        // 
        // contextItemDelete
        // 
        this.contextItemDelete.Name = "contextItemDelete";
        this.contextItemDelete.Size = new System.Drawing.Size(235, 22);
        this.contextItemDelete.Text = "✕ Delete Channel";
        this.contextItemDelete.Click += new System.EventHandler(this.BtnDelete_Click);
        // 
        // contextItemDuplicate
        // 
        this.contextItemDuplicate.Name = "contextItemDuplicate";
        this.contextItemDuplicate.Size = new System.Drawing.Size(235, 22);
        this.contextItemDuplicate.Text = "D&uplicate";
        this.contextItemDuplicate.Click += new System.EventHandler(this.BtnDuplicate_Click);
        // 
        // contextItemToggle
        // 
        this.contextItemToggle.Name = "contextItemToggle";
        this.contextItemToggle.Size = new System.Drawing.Size(235, 22);
        this.contextItemToggle.Text = "Toggle Active";
        this.contextItemToggle.Click += new System.EventHandler(this.BtnToggleActive_Click);
        // 
        // contextSeparator2
        // 
        this.contextSeparator2.Name = "contextSeparator2";
        this.contextSeparator2.Size = new System.Drawing.Size(232, 6);
        // 
        // contextItemRefresh
        // 
        this.contextItemRefresh.Name = "contextItemRefresh";
        this.contextItemRefresh.Size = new System.Drawing.Size(235, 22);
        this.contextItemRefresh.Text = "⟳ Refresh Stream";
        this.contextItemRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
        // 
        // pnlConsoleHeader
        // 
        this.pnlConsoleHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
        this.pnlConsoleHeader.Controls.Add(this.btnClearConsole);
        this.pnlConsoleHeader.Controls.Add(this.btnCopyConsole);
        this.pnlConsoleHeader.Controls.Add(this.cboAuditFilter);
        this.pnlConsoleHeader.Controls.Add(this.lblConsoleTitle);
        this.pnlConsoleHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlConsoleHeader.Location = new System.Drawing.Point(0, 0);
        this.pnlConsoleHeader.Name = "pnlConsoleHeader";
        this.pnlConsoleHeader.Size = new System.Drawing.Size(1040, 30);
        this.pnlConsoleHeader.TabIndex = 0;
        // 
        // lblConsoleTitle
        // 
        this.lblConsoleTitle.AutoSize = true;
        this.lblConsoleTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblConsoleTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(75)))));
        this.lblConsoleTitle.Location = new System.Drawing.Point(8, 7);
        this.lblConsoleTitle.Name = "lblConsoleTitle";
        this.lblConsoleTitle.Size = new System.Drawing.Size(296, 15);
        this.lblConsoleTitle.TabIndex = 0;
        this.lblConsoleTitle.Text = "📋 Channel Configuration & Event History (Audit Trail)";
        // 
        // cboAuditFilter
        // 
        this.cboAuditFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.cboAuditFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cboAuditFilter.FormattingEnabled = true;
        this.cboAuditFilter.Location = new System.Drawing.Point(670, 4);
        this.cboAuditFilter.Name = "cboAuditFilter";
        this.cboAuditFilter.Size = new System.Drawing.Size(160, 23);
        this.cboAuditFilter.TabIndex = 1;
        this.cboAuditFilter.SelectedIndexChanged += new System.EventHandler(this.CboAuditFilter_SelectedIndexChanged);
        // 
        // btnCopyConsole
        // 
        this.btnCopyConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCopyConsole.Location = new System.Drawing.Point(836, 4);
        this.btnCopyConsole.Name = "btnCopyConsole";
        this.btnCopyConsole.Size = new System.Drawing.Size(95, 23);
        this.btnCopyConsole.TabIndex = 2;
        this.btnCopyConsole.Text = "📋 Copy Log";
        this.btnCopyConsole.UseVisualStyleBackColor = true;
        this.btnCopyConsole.Click += new System.EventHandler(this.BtnCopyConsole_Click);
        // 
        // btnClearConsole
        // 
        this.btnClearConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClearConsole.Location = new System.Drawing.Point(937, 4);
        this.btnClearConsole.Name = "btnClearConsole";
        this.btnClearConsole.Size = new System.Drawing.Size(95, 23);
        this.btnClearConsole.TabIndex = 3;
        this.btnClearConsole.Text = "✕ Clear";
        this.btnClearConsole.UseVisualStyleBackColor = true;
        this.btnClearConsole.Click += new System.EventHandler(this.BtnClearConsole_Click);
        // 
        // gridAuditLogs
        // 
        this.gridAuditLogs.AllowUserToAddRows = false;
        this.gridAuditLogs.AllowUserToDeleteRows = false;
        this.gridAuditLogs.AllowUserToResizeRows = false;
        this.gridAuditLogs.BackgroundColor = System.Drawing.SystemColors.Window;
        this.gridAuditLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.gridAuditLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.gridAuditLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAuditTime,
            this.colAuditType,
            this.colAuditChannel,
            this.colAuditAction,
            this.colAuditDetails});
        this.gridAuditLogs.Dock = System.Windows.Forms.DockStyle.Fill;
        this.gridAuditLogs.Location = new System.Drawing.Point(0, 30);
        this.gridAuditLogs.MultiSelect = true;
        this.gridAuditLogs.Name = "gridAuditLogs";
        this.gridAuditLogs.ReadOnly = true;
        this.gridAuditLogs.RowHeadersVisible = false;
        this.gridAuditLogs.RowTemplate.Height = 23;
        this.gridAuditLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.gridAuditLogs.Size = new System.Drawing.Size(1040, 154);
        this.gridAuditLogs.TabIndex = 1;
        // 
        // colAuditTime
        // 
        this.colAuditTime.HeaderText = "Timestamp";
        this.colAuditTime.Name = "colAuditTime";
        this.colAuditTime.ReadOnly = true;
        this.colAuditTime.Width = 140;
        // 
        // colAuditType
        // 
        this.colAuditType.HeaderText = "Category";
        this.colAuditType.Name = "colAuditType";
        this.colAuditType.ReadOnly = true;
        this.colAuditType.Width = 90;
        // 
        // colAuditChannel
        // 
        this.colAuditChannel.HeaderText = "Channel";
        this.colAuditChannel.Name = "colAuditChannel";
        this.colAuditChannel.ReadOnly = true;
        this.colAuditChannel.Width = 160;
        // 
        // colAuditAction
        // 
        this.colAuditAction.HeaderText = "Action";
        this.colAuditAction.Name = "colAuditAction";
        this.colAuditAction.ReadOnly = true;
        this.colAuditAction.Width = 160;
        // 
        // colAuditDetails
        // 
        this.colAuditDetails.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.colAuditDetails.HeaderText = "Details & Modification Values";
        this.colAuditDetails.Name = "colAuditDetails";
        this.colAuditDetails.ReadOnly = true;
        // 
        // notifyIcon
        // 
        this.notifyIcon.ContextMenuStrip = this.trayContextMenu;
        this.notifyIcon.Text = "Icecast & SHOUTcast Stream Monitor";
        this.notifyIcon.Visible = true;
        this.notifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);
        // 
        // trayContextMenu
        // 
        this.trayContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayItemRestore,
            this.trayItemRefresh,
            this.trayItemAutoRefresh,
            this.traySeparator1,
            this.trayItemExit});
        this.trayContextMenu.Name = "trayContextMenu";
        this.trayContextMenu.Size = new System.Drawing.Size(192, 98);
        // 
        // trayItemRestore
        // 
        this.trayItemRestore.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.trayItemRestore.Name = "trayItemRestore";
        this.trayItemRestore.Size = new System.Drawing.Size(191, 22);
        this.trayItemRestore.Text = "Show Dashboard";
        this.trayItemRestore.Click += new System.EventHandler(this.TrayItemRestore_Click);
        // 
        // trayItemRefresh
        // 
        this.trayItemRefresh.Name = "trayItemRefresh";
        this.trayItemRefresh.Size = new System.Drawing.Size(191, 22);
        this.trayItemRefresh.Text = "Refresh All Now";
        this.trayItemRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
        // 
        // trayItemAutoRefresh
        // 
        this.trayItemAutoRefresh.Name = "trayItemAutoRefresh";
        this.trayItemAutoRefresh.Size = new System.Drawing.Size(191, 22);
        this.trayItemAutoRefresh.Text = "Pause Auto-Monitor";
        this.trayItemAutoRefresh.Click += new System.EventHandler(this.BtnAutoRefresh_Click);
        // 
        // traySeparator1
        // 
        this.traySeparator1.Name = "traySeparator1";
        this.traySeparator1.Size = new System.Drawing.Size(188, 6);
        // 
        // trayItemExit
        // 
        this.trayItemExit.Name = "trayItemExit";
        this.trayItemExit.Size = new System.Drawing.Size(191, 22);
        this.trayItemExit.Text = "Exit";
        this.trayItemExit.Click += new System.EventHandler(this.MenuItemExit_Click);
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1040, 631);
        this.Controls.Add(this.splitContainerMain);
        this.Controls.Add(this.statusStrip);
        this.Controls.Add(this.toolStrip);
        this.Controls.Add(this.menuStrip);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.MainMenuStrip = this.menuStrip;
        this.MinimumSize = new System.Drawing.Size(850, 480);
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Icecast & SHOUTcast Live Stream Monitor";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
        this.Load += new System.EventHandler(this.MainForm_Load);
        this.Resize += new System.EventHandler(this.MainForm_Resize);
        this.menuStrip.ResumeLayout(false);
        this.menuStrip.PerformLayout();
        this.toolStrip.ResumeLayout(false);
        this.toolStrip.PerformLayout();
        this.statusStrip.ResumeLayout(false);
        this.statusStrip.PerformLayout();
        this.splitContainerMain.Panel1.ResumeLayout(false);
        this.splitContainerMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
        this.splitContainerMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.gridStreams)).EndInit();
        this.gridContextMenu.ResumeLayout(false);
        this.pnlConsoleHeader.ResumeLayout(false);
        this.pnlConsoleHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridAuditLogs)).EndInit();
        this.trayContextMenu.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip;
    private System.Windows.Forms.ToolStripMenuItem menuFile;
    private System.Windows.Forms.ToolStripMenuItem menuItemAdd;
    private System.Windows.Forms.ToolStripMenuItem menuItemImportTinyIce;
    private System.Windows.Forms.ToolStripMenuItem menuItemRefresh;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem menuItemSettings;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem menuItemExit;
    private System.Windows.Forms.ToolStripMenuItem menuChannels;
    private System.Windows.Forms.ToolStripMenuItem menuItemAnalytics;
    private System.Windows.Forms.ToolStripMenuItem menuItemListeners;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ToolStripMenuItem menuItemEdit;
    private System.Windows.Forms.ToolStripMenuItem menuItemDelete;
    private System.Windows.Forms.ToolStripMenuItem menuItemDuplicate;
    private System.Windows.Forms.ToolStripMenuItem menuItemToggle;
    private System.Windows.Forms.ToolStripMenuItem menuTools;
    private System.Windows.Forms.ToolStripMenuItem menuItemAutoRefresh;
    private System.Windows.Forms.ToolStripMenuItem menuItemToggleConsole;
    private System.Windows.Forms.ToolStripMenuItem menuItemRemoteUsers;
    private System.Windows.Forms.ToolStripMenuItem menuItemMockServer;
    private System.Windows.Forms.ToolStripMenuItem menuHelp;
    private System.Windows.Forms.ToolStripMenuItem menuItemCheckUpdates;
    private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
    private System.Windows.Forms.ToolStrip toolStrip;
    private System.Windows.Forms.ToolStripButton btnToolAdd;
    private System.Windows.Forms.ToolStripButton btnToolEdit;
    private System.Windows.Forms.ToolStripButton btnToolDelete;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripButton btnToolAnalytics;
    private System.Windows.Forms.ToolStripButton btnToolListeners;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
    private System.Windows.Forms.ToolStripButton btnToolRefresh;
    private System.Windows.Forms.ToolStripButton btnToolAutoRefresh;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripButton btnToolSettings;
    private System.Windows.Forms.ToolStripButton btnToolConsole;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripButton btnToolMockServer;
    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel statusLabelMain;
    private System.Windows.Forms.ToolStripStatusLabel statusLabelTotalStreams;
    private System.Windows.Forms.ToolStripStatusLabel statusLabelActiveStreams;
    private System.Windows.Forms.ToolStripStatusLabel statusLabelTotalListeners;
    private System.Windows.Forms.ToolStripStatusLabel statusLabelNextRefresh;
    private System.Windows.Forms.SplitContainer splitContainerMain;
    private System.Windows.Forms.DataGridView gridStreams;
    private System.Windows.Forms.DataGridViewCheckBoxColumn colEnabled;
    private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colMount;
    private System.Windows.Forms.DataGridViewTextBoxColumn colServer;
    private System.Windows.Forms.DataGridViewTextBoxColumn colType;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    private System.Windows.Forms.DataGridViewTextBoxColumn colListeners;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPeak;
    private System.Windows.Forms.DataGridViewTextBoxColumn colFormat;
    private System.Windows.Forms.DataGridViewTextBoxColumn colBitrate;
    private System.Windows.Forms.DataGridViewTextBoxColumn colAuth;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLatency;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLastChecked;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDetails;
    private System.Windows.Forms.ContextMenuStrip gridContextMenu;
    private System.Windows.Forms.ToolStripMenuItem contextItemAnalytics;
    private System.Windows.Forms.ToolStripMenuItem contextItemListeners;
    private System.Windows.Forms.ToolStripSeparator contextSeparator1;
    private System.Windows.Forms.ToolStripMenuItem contextItemEdit;
    private System.Windows.Forms.ToolStripMenuItem contextItemDelete;
    private System.Windows.Forms.ToolStripMenuItem contextItemDuplicate;
    private System.Windows.Forms.ToolStripMenuItem contextItemToggle;
    private System.Windows.Forms.ToolStripSeparator contextSeparator2;
    private System.Windows.Forms.ToolStripMenuItem contextItemRefresh;
    private System.Windows.Forms.Panel pnlConsoleHeader;
    private System.Windows.Forms.Label lblConsoleTitle;
    private System.Windows.Forms.ComboBox cboAuditFilter;
    private System.Windows.Forms.Button btnCopyConsole;
    private System.Windows.Forms.Button btnClearConsole;
    private System.Windows.Forms.DataGridView gridAuditLogs;
    private System.Windows.Forms.DataGridViewTextBoxColumn colAuditTime;
    private System.Windows.Forms.DataGridViewTextBoxColumn colAuditType;
    private System.Windows.Forms.DataGridViewTextBoxColumn colAuditChannel;
    private System.Windows.Forms.DataGridViewTextBoxColumn colAuditAction;
    private System.Windows.Forms.DataGridViewTextBoxColumn colAuditDetails;
    private System.Windows.Forms.NotifyIcon notifyIcon;
    private System.Windows.Forms.ContextMenuStrip trayContextMenu;
    private System.Windows.Forms.ToolStripMenuItem trayItemRestore;
    private System.Windows.Forms.ToolStripMenuItem trayItemRefresh;
    private System.Windows.Forms.ToolStripMenuItem trayItemAutoRefresh;
    private System.Windows.Forms.ToolStripSeparator traySeparator1;
    private System.Windows.Forms.ToolStripMenuItem trayItemExit;
}
