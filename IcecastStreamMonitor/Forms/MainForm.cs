using System.Diagnostics;
using System.Reflection;
using System.Text;
using IcecastStreamMonitor.Models;
using IcecastStreamMonitor.Services;

namespace IcecastStreamMonitor.Forms;

public partial class MainForm : Form
{
    private readonly ConfigStorageService _configService;
    private readonly IStreamMonitorService _monitorService;
    private readonly StreamHistoryService _historyService;
    private readonly AuditLogService _auditLogService;
    private readonly UpdateService _updateService;
    private readonly RemoteApiService _remoteApiService;
    private readonly Services.Streaming.AudioStreamServer _audioStreamServer;

    private AppSettings _settings;
    private readonly Dictionary<Guid, StreamStatus> _statusCache = new();
    private readonly System.Windows.Forms.Timer _pollTimer = new();
    private MockStreamServer? _mockServer;
    private bool _isScanning = false;
    private bool _forceExit = false;
    private CancellationTokenSource? _scanCts;

    public MainForm()
    {
        InitializeComponent();

        try
        {
            var appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (appIcon != null)
            {
                Icon = appIcon;
                notifyIcon.Icon = appIcon;
            }
        }
        catch { }

        _configService = new ConfigStorageService();
        _monitorService = new StreamMonitorService();
        _historyService = new StreamHistoryService();
        _auditLogService = new AuditLogService();
        _updateService = new UpdateService();
        _audioStreamServer = new Services.Streaming.AudioStreamServer();

        _settings = _configService.LoadSettings();

        if (_monitorService is StreamMonitorService sms)
        {
            sms.LocalServer = _audioStreamServer;
        }

        _audioStreamServer.OnSourceConnected += (mount, ip) =>
        {
            _auditLogService.Log(AuditCategory.StreamAlert, mount, "DJ Source Connected", $"Live broadcast feed started from {ip} on mount {mount}");
        };

        _audioStreamServer.OnSourceDisconnected += mount =>
        {
            _auditLogService.Log(AuditCategory.StreamAlert, mount, "DJ Source Disconnected", $"Live broadcast feed disconnected on mount {mount}");
        };

        _remoteApiService = new RemoteApiService(
            _settings,
            () => _statusCache,
            async () => await RefreshStreamsAsync(isAuto: false),
            _historyService,
            _monitorService,
            _auditLogService,
            () => _configService.SaveSettings(_settings));

        EnableDoubleBuffering(gridStreams);
        EnableDoubleBuffering(gridAuditLogs);

        _pollTimer.Tick += async (s, e) => await RefreshStreamsAsync(isAuto: true);

        _auditLogService.OnLogAdded += entry =>
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => AddAuditRow(entry, prepend: true)));
            }
            else
            {
                AddAuditRow(entry, prepend: true);
            }
        };
    }

    private static void EnableDoubleBuffering(DataGridView dgv)
    {
        typeof(DataGridView).InvokeMember(
            "DoubleBuffered",
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
            null,
            dgv,
            new object[] { true });
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        ApplyWindowSettings();
        SyncUiSettings();
        SetupNotifyIcon();
        SetupAuditFilter();
        PopulateGrid();
        PopulateAuditLogs();

        SyncAudioServerMounts();
        if (_settings.StreamingServerEnabled)
        {
            try
            {
                _audioStreamServer.Start(_settings.StreamingServerPort);
            }
            catch (Exception ex)
            {
                _auditLogService.Log(AuditCategory.System, "", "Server Error", $"Could not start streaming server on port {_settings.StreamingServerPort}: {ex.Message}");
            }
        }

        if (_settings.AutoRefreshEnabled)
        {
            StartAutoRefresh();
        }

        if (_settings.RemoteServerEnabled)
        {
            _remoteApiService.Start();
        }

        await RefreshStreamsAsync(isAuto: false);
    }

    private void SyncAudioServerMounts()
    {
        _audioStreamServer.DefaultSourcePassword = _settings.DefaultSourcePassword;
        _audioStreamServer.SetTrustedProxies(_settings.TrustedProxies);

        var existingMounts = _settings.Channels.Select(c => c.MountPath).ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var ch in _settings.Channels)
        {
            _audioStreamServer.EnsureMount(ch.MountPath, ch.UseDefaultPassword ? _settings.DefaultSourcePassword : ch.CustomPassword, ch.Enabled);
        }

        foreach (var m in _audioStreamServer.GetAllMounts().ToList())
        {
            if (!existingMounts.Contains(m.MountPath))
            {
                _audioStreamServer.RemoveMount(m.MountPath);
            }
        }
    }

    private void SetupAuditFilter()
    {
        cboAuditFilter.Items.Clear();
        cboAuditFilter.Items.Add("Filter: All Events");
        cboAuditFilter.Items.Add("Filter: Config Changes");
        cboAuditFilter.Items.Add("Filter: Auth Changes");
        cboAuditFilter.Items.Add("Filter: Channel Mgmt");
        cboAuditFilter.Items.Add("Filter: Stream Alerts");
        cboAuditFilter.SelectedIndex = 0;
    }

    private void SetupNotifyIcon()
    {
        try
        {
            notifyIcon.Icon = this.Icon ?? SystemIcons.Application;
            notifyIcon.Text = "Icecast Stream Monitor";
            notifyIcon.Visible = true;
        }
        catch { }
    }

    private void ApplyWindowSettings()
    {
        if (_settings.WindowWidth > 200 && _settings.WindowHeight > 200)
        {
            Width = _settings.WindowWidth;
            Height = _settings.WindowHeight;
        }

        if (_settings.WindowLeft >= 0 && _settings.WindowTop >= 0)
        {
            StartPosition = FormStartPosition.Manual;
            Location = new Point(_settings.WindowLeft, _settings.WindowTop);
        }

        if (_settings.IsMaximized)
        {
            WindowState = FormWindowState.Maximized;
        }
    }

    private void SyncUiSettings()
    {
        menuItemAutoRefresh.Checked = _settings.AutoRefreshEnabled;
        btnToolAutoRefresh.Checked = _settings.AutoRefreshEnabled;
        btnToolAutoRefresh.Text = _settings.AutoRefreshEnabled ? "▶ Auto-Monitor: ON" : "⏸ Auto-Monitor: PAUSED";
        trayItemAutoRefresh.Text = _settings.AutoRefreshEnabled ? "Pause Auto-Monitor" : "Resume Auto-Monitor";
        statusLabelNextRefresh.Text = _settings.AutoRefreshEnabled
            ? $"Refresh: Active ({_settings.PollingIntervalSeconds}s)"
            : "Refresh: Paused";
    }

    private void PopulateGrid()
    {
        gridStreams.Rows.Clear();

        foreach (var ch in _settings.Channels)
        {
            var rowIndex = gridStreams.Rows.Add();
            var row = gridStreams.Rows[rowIndex];
            row.Tag = ch;

            UpdateRowDisplay(row, ch, _statusCache.GetValueOrDefault(ch.Id));
        }

        UpdateSummaryFooter();
    }

    private void UpdateRowDisplay(DataGridViewRow row, StreamChannel ch, StreamStatus? status)
    {
        row.Cells[colEnabled.Index].Value = ch.Enabled;
        row.Cells[colName.Index].Value = string.IsNullOrWhiteSpace(ch.Name) ? ch.MountPath : ch.Name;
        row.Cells[colMount.Index].Value = ch.MountPath;
        row.Cells[colServer.Index].Value = $"{ch.Host}:{ch.Port}";
        row.Cells[colType.Index].Value = ch.ServerType.ToString();
        row.Cells[colAuth.Index].Value = ch.UseDefaultPassword ? "Default" : "Custom";

        if (!ch.Enabled)
        {
            row.Cells[colStatus.Index].Value = "Disabled";
            row.Cells[colListeners.Index].Value = "-";
            row.Cells[colPeak.Index].Value = "-";
            row.Cells[colFormat.Index].Value = "-";
            row.Cells[colBitrate.Index].Value = "-";
            row.Cells[colLatency.Index].Value = "-";
            row.Cells[colLastChecked.Index].Value = "-";
            row.Cells[colDetails.Index].Value = "Monitoring is disabled for this feed";

            row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
            return;
        }

        row.DefaultCellStyle.ForeColor = SystemColors.ControlText;

        if (status == null)
        {
            row.Cells[colStatus.Index].Value = "Pending...";
            row.Cells[colListeners.Index].Value = "...";
            row.Cells[colPeak.Index].Value = "...";
            row.Cells[colFormat.Index].Value = "...";
            row.Cells[colBitrate.Index].Value = "...";
            row.Cells[colLatency.Index].Value = "...";
            row.Cells[colLastChecked.Index].Value = "...";
            row.Cells[colDetails.Index].Value = "Waiting for check";
            return;
        }

        if (status.IsOnline)
        {
            row.Cells[colStatus.Index].Value = "● ONLINE";
            row.Cells[colStatus.Index].Style.ForeColor = Color.ForestGreen;
            row.Cells[colStatus.Index].Style.Font = new Font(gridStreams.Font, FontStyle.Bold);

            row.Cells[colListeners.Index].Value = status.ConcurrentListeners.ToString("N0");
            row.Cells[colListeners.Index].Style.Font = new Font(gridStreams.Font, FontStyle.Bold);

            row.Cells[colPeak.Index].Value = status.PeakListeners > 0 ? status.PeakListeners.ToString("N0") : "-";
            row.Cells[colFormat.Index].Value = status.AudioFormat;
            row.Cells[colBitrate.Index].Value = status.BitRateKbps > 0 ? $"{status.BitRateKbps} kbps" : "-";
            row.Cells[colLatency.Index].Value = $"{status.LatencyMs} ms";
            row.Cells[colLastChecked.Index].Value = status.LastChecked != DateTime.MinValue ? status.LastChecked.ToString("HH:mm:ss") : "-";
            row.Cells[colDetails.Index].Value = !string.IsNullOrWhiteSpace(status.StreamTitle)
                ? status.StreamTitle
                : status.ServerDescription;
        }
        else
        {
            row.Cells[colStatus.Index].Value = "○ OFFLINE";
            row.Cells[colStatus.Index].Style.ForeColor = Color.Firebrick;
            row.Cells[colStatus.Index].Style.Font = new Font(gridStreams.Font, FontStyle.Bold);

            row.Cells[colListeners.Index].Value = "0";
            row.Cells[colPeak.Index].Value = "-";
            row.Cells[colFormat.Index].Value = "-";
            row.Cells[colBitrate.Index].Value = "-";
            row.Cells[colLatency.Index].Value = status.LatencyMs > 0 ? $"{status.LatencyMs} ms" : "-";
            row.Cells[colLastChecked.Index].Value = status.LastChecked != DateTime.MinValue ? status.LastChecked.ToString("HH:mm:ss") : "-";
            row.Cells[colDetails.Index].Value = status.StatusMessage;
        }
    }

    private void PopulateAuditLogs()
    {
        gridAuditLogs.Rows.Clear();
        var entries = _auditLogService.GetEntries();
        var filter = cboAuditFilter.SelectedIndex;

        foreach (var e in entries)
        {
            if (MatchesFilter(e, filter))
            {
                AddAuditRow(e, prepend: false);
            }
        }
    }

    private static bool MatchesFilter(AuditLogEntry entry, int filterIdx)
    {
        return filterIdx switch
        {
            1 => entry.Category == AuditCategory.ConfigChange,
            2 => entry.Category == AuditCategory.AuthChange,
            3 => entry.Category == AuditCategory.ChannelManagement,
            4 => entry.Category == AuditCategory.StreamAlert,
            _ => true
        };
    }

    private void AddAuditRow(AuditLogEntry entry, bool prepend)
    {
        var filter = cboAuditFilter.SelectedIndex;
        if (!MatchesFilter(entry, filter)) return;

        int rowIdx = prepend ? 0 : gridAuditLogs.Rows.Count;
        if (prepend)
        {
            gridAuditLogs.Rows.Insert(0, 1);
        }
        else
        {
            gridAuditLogs.Rows.Add();
        }

        var row = gridAuditLogs.Rows[rowIdx];
        row.Cells[colAuditTime.Index].Value = entry.FormattedTimestamp;
        row.Cells[colAuditType.Index].Value = entry.CategoryDisplayName;
        row.Cells[colAuditChannel.Index].Value = entry.ChannelName;
        row.Cells[colAuditAction.Index].Value = entry.Action;
        row.Cells[colAuditDetails.Index].Value = entry.Details;

        // Styling based on category
        row.Cells[colAuditType.Index].Style.Font = new Font(gridAuditLogs.Font, FontStyle.Bold);
        row.Cells[colAuditType.Index].Style.ForeColor = entry.Category switch
        {
            AuditCategory.StreamAlert => Color.Firebrick,
            AuditCategory.ConfigChange => Color.DarkBlue,
            AuditCategory.AuthChange => Color.DarkMagenta,
            AuditCategory.ChannelManagement => Color.DarkGreen,
            _ => Color.DimGray
        };
    }

    private async Task RefreshStreamsAsync(bool isAuto)
    {
        if (_isScanning) return;
        _isScanning = true;

        _scanCts?.Cancel();
        _scanCts = new CancellationTokenSource();

        statusLabelMain.Text = "Scanning streams...";
        var sw = Stopwatch.StartNew();

        try
        {
            var timeout = TimeSpan.FromSeconds(_settings.RequestTimeoutSeconds);
            var results = await _monitorService.CheckAllStreamsAsync(
                _settings.Channels,
                _settings.DefaultPassword,
                timeout,
                _scanCts.Token);

            foreach (var (id, status) in results)
            {
                var ch = _settings.Channels.FirstOrDefault(c => c.Id == id);
                var name = ch?.Name ?? "Stream";

                // Check if stream changed from online to offline
                if (_statusCache.TryGetValue(id, out var prevStatus))
                {
                    if (prevStatus.IsOnline && !status.IsOnline)
                    {
                        _auditLogService.LogStreamAlert(name, ch?.MountPath ?? "", $"Went OFFLINE ({status.StatusMessage})");

                        if (_settings.NotifyOnStreamOffline)
                        {
                            notifyIcon.ShowBalloonTip(3000, "Stream Offline Alert!", $"'{name}' ({ch?.MountPath}) is now OFFLINE: {status.StatusMessage}", ToolTipIcon.Warning);
                        }
                    }
                    else if (!prevStatus.IsOnline && status.IsOnline)
                    {
                        _auditLogService.Log(AuditCategory.StreamAlert, name, "Stream Restored", $"Back ONLINE ({status.AudioFormat} @ {status.BitRateKbps}k, {status.ConcurrentListeners} listeners)");
                    }
                }

                _statusCache[id] = status;

                // Record historical point for 24h graphs
                _historyService.RecordSnapshot(id, status, _settings.PollingIntervalSeconds, name);
            }

            foreach (DataGridViewRow row in gridStreams.Rows)
            {
                if (row.Tag is StreamChannel ch)
                {
                    UpdateRowDisplay(row, ch, _statusCache.GetValueOrDefault(ch.Id));
                }
            }

            sw.Stop();
            statusLabelMain.Text = $"Last scan completed in {sw.ElapsedMilliseconds}ms at {DateTime.Now:HH:mm:ss}";
            UpdateSummaryFooter();
        }
        catch (OperationCanceledException)
        {
            statusLabelMain.Text = "Scan cancelled";
        }
        catch (Exception ex)
        {
            statusLabelMain.Text = $"Scan error: {ex.Message}";
        }
        finally
        {
            _isScanning = false;
        }
    }

    private void UpdateSummaryFooter()
    {
        var totalChannels = _settings.Channels.Count;
        var activeChannels = _settings.Channels.Count(c => c.Enabled);
        var onlineChannels = _settings.Channels.Count(c => c.Enabled && _statusCache.TryGetValue(c.Id, out var st) && st.IsOnline);
        var totalListeners = _settings.Channels
            .Where(c => c.Enabled && _statusCache.ContainsKey(c.Id))
            .Sum(c => _statusCache[c.Id].ConcurrentListeners);

        statusLabelTotalStreams.Text = $"Channels: {activeChannels}/{totalChannels}";
        statusLabelActiveStreams.Text = $"Online: {onlineChannels}";
        statusLabelTotalListeners.Text = $"Total Listeners: {totalListeners:N0}";

        var trayText = $"Stream Monitor | {onlineChannels}/{activeChannels} Online | {totalListeners:N0} Listeners";
        if (trayText.Length > 63) trayText = trayText[..63];
        notifyIcon.Text = trayText;
    }

    private void StartAutoRefresh()
    {
        _pollTimer.Interval = Math.Max(1000, _settings.PollingIntervalSeconds * 1000);
        _pollTimer.Start();
    }

    private void StopAutoRefresh()
    {
        _pollTimer.Stop();
    }

    private void ToggleAutoRefresh()
    {
        _settings.AutoRefreshEnabled = !_settings.AutoRefreshEnabled;
        SyncUiSettings();

        if (_settings.AutoRefreshEnabled)
        {
            StartAutoRefresh();
        }
        else
        {
            StopAutoRefresh();
        }

        _configService.SaveSettings(_settings);
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        using var authDlg = new AdminAuthDialog(_settings.MasterAdminPassword, _settings.MasterAdminPasswordHash, "Add New Channel");
        if (authDlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        var newChannel = new StreamChannel
        {
            Host = "127.0.0.1",
            Port = 8000,
            MountPath = "/live",
            Name = "New Stream Mountpoint",
            ServerType = ServerType.Auto,
            UseDefaultPassword = true,
            Enabled = true
        };

        using var dlg = new ChannelEditDialog(newChannel, _settings.DefaultPassword, _monitorService);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _settings.Channels.Add(dlg.Channel);
            _configService.SaveSettings(_settings);
            _auditLogService.LogChannelAdded(dlg.Channel);

            SyncAudioServerMounts();
            PopulateGrid();
            _ = RefreshStreamsAsync(isAuto: false);
        }
    }

    private void BtnImportTinyIce_Click(object? sender, EventArgs e)
    {
        using var authDlg = new AdminAuthDialog(_settings.MasterAdminPassword, _settings.MasterAdminPasswordHash, "Import TinyIce Configuration");
        if (authDlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        using var ofd = new OpenFileDialog
        {
            Title = "Select TinyIce config.json to Import Mounts",
            Filter = "JSON Configuration (*.json)|*.json|All Files (*.*)|*.*",
            FileName = "config.json"
        };

        if (ofd.ShowDialog(this) == DialogResult.OK)
        {
            try
            {
                var json = File.ReadAllText(ofd.FileName);
                var result = TinyIceConfigImporter.ImportFromTinyIceJson(json);

                if (result.Channels.Count == 0)
                {
                    MessageBox.Show("No mounts found in the selected configuration file.", "Import Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var res = MessageBox.Show(
                    $"Found {result.Channels.Count} channels in TinyIce configuration (Port: {result.Port}).\n\n" +
                    "Would you like to import and append these channels to your current channel list?",
                    "Confirm TinyIce Import",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (res == DialogResult.Yes)
                {
                    int addedCount = 0;
                    var existingPaths = _settings.Channels.Select(c => c.MountPath.ToLowerInvariant()).ToHashSet();

                    foreach (var ch in result.Channels)
                    {
                        if (!existingPaths.Contains(ch.MountPath.ToLowerInvariant()))
                        {
                            _settings.Channels.Add(ch);
                            existingPaths.Add(ch.MountPath.ToLowerInvariant());
                            addedCount++;
                        }
                    }

                    if (result.TrustedProxies.Count > 0)
                    {
                        foreach (var p in result.TrustedProxies)
                        {
                            if (!_settings.TrustedProxies.Contains(p, StringComparer.OrdinalIgnoreCase))
                            {
                                _settings.TrustedProxies.Add(p);
                            }
                        }
                    }

                    _configService.SaveSettings(_settings);
                    _auditLogService.Log(AuditCategory.ChannelManagement, "TinyIce Import", "Bulk Channels Imported",
                        $"Imported {addedCount} new channels from TinyIce config file '{Path.GetFileName(ofd.FileName)}'");

                    SyncAudioServerMounts();
                    PopulateGrid();
                    _ = RefreshStreamsAsync(isAuto: false);

                    MessageBox.Show(
                        $"Successfully imported and registered {addedCount} channels into RTSAS!",
                        "Import Complete",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to parse TinyIce config: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnEdit_Click(object? sender, EventArgs e)
    {
        if (gridStreams.CurrentRow?.Tag is not StreamChannel selectedChannel)
        {
            MessageBox.Show("Please select a channel to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Per-channel admin authentication challenge
        using var authDlg = new AdminAuthDialog(_settings.MasterAdminPassword, _settings.MasterAdminPasswordHash, selectedChannel.Name);
        if (authDlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        var oldClone = selectedChannel.Clone();

        using var dlg = new ChannelEditDialog(selectedChannel, _settings.DefaultPassword, _monitorService);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            var updated = dlg.Channel;
            selectedChannel.Name = updated.Name;
            selectedChannel.Host = updated.Host;
            selectedChannel.Port = updated.Port;
            selectedChannel.MountPath = updated.MountPath;
            selectedChannel.ServerType = updated.ServerType;
            selectedChannel.UseDefaultPassword = updated.UseDefaultPassword;
            selectedChannel.CustomPassword = updated.CustomPassword;
            selectedChannel.Enabled = updated.Enabled;

            _configService.SaveSettings(_settings);
            _auditLogService.LogChannelModified(oldClone, selectedChannel);

            SyncAudioServerMounts();
            UpdateRowDisplay(gridStreams.CurrentRow, selectedChannel, _statusCache.GetValueOrDefault(selectedChannel.Id));
            UpdateSummaryFooter();
            _ = RefreshStreamsAsync(isAuto: false);
        }
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (gridStreams.CurrentRow?.Tag is not StreamChannel selectedChannel)
        {
            MessageBox.Show("Please select a channel to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Admin authentication challenge before deletion
        using var authDlg = new AdminAuthDialog(_settings.MasterAdminPassword, _settings.MasterAdminPasswordHash, $"Delete '{selectedChannel.Name}'");
        if (authDlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        var res = MessageBox.Show(
            $"Are you sure you want to delete channel '{selectedChannel.Name}' ({selectedChannel.MountPath})?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (res == DialogResult.Yes)
        {
            _settings.Channels.Remove(selectedChannel);
            _statusCache.Remove(selectedChannel.Id);
            _configService.SaveSettings(_settings);
            _auditLogService.LogChannelDeleted(selectedChannel);

            SyncAudioServerMounts();
            PopulateGrid();
        }
    }

    private void BtnDuplicate_Click(object? sender, EventArgs e)
    {
        if (gridStreams.CurrentRow?.Tag is not StreamChannel selectedChannel) return;

        var clone = selectedChannel.Clone();
        clone.Name = $"{selectedChannel.Name} (Copy)";
        _settings.Channels.Add(clone);
        _configService.SaveSettings(_settings);
        _auditLogService.LogChannelAdded(clone);

        SyncAudioServerMounts();
        PopulateGrid();
        _ = RefreshStreamsAsync(isAuto: false);
    }

    private void BtnToggleActive_Click(object? sender, EventArgs e)
    {
        if (gridStreams.CurrentRow?.Tag is not StreamChannel selectedChannel) return;

        selectedChannel.Enabled = !selectedChannel.Enabled;
        _configService.SaveSettings(_settings);
        _auditLogService.LogChannelToggled(selectedChannel, selectedChannel.Enabled);

        SyncAudioServerMounts();
        UpdateRowDisplay(gridStreams.CurrentRow, selectedChannel, _statusCache.GetValueOrDefault(selectedChannel.Id));
        UpdateSummaryFooter();
        _ = RefreshStreamsAsync(isAuto: false);
    }

    private void BtnAnalytics_Click(object? sender, EventArgs e)
    {
        if (gridStreams.CurrentRow?.Tag is not StreamChannel selectedChannel)
        {
            MessageBox.Show("Please select a stream channel first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var status = _statusCache.GetValueOrDefault(selectedChannel.Id);
        using var dlg = new StreamAnalyticsDialog(selectedChannel, _historyService, status);
        dlg.ShowDialog(this);
    }

    private void BtnShowListeners_Click(object? sender, EventArgs e)
    {
        if (gridStreams.CurrentRow?.Tag is not StreamChannel selectedChannel)
        {
            MessageBox.Show("Please select a stream channel first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new ListenerListDialog(selectedChannel, _settings.DefaultPassword, _monitorService);
        dlg.ShowDialog(this);
    }

    private async void BtnRefresh_Click(object? sender, EventArgs e)
    {
        await RefreshStreamsAsync(isAuto: false);
    }

    private void BtnAutoRefresh_Click(object? sender, EventArgs e)
    {
        ToggleAutoRefresh();
    }

    private void BtnSettings_Click(object? sender, EventArgs e)
    {
        using var authDlg = new AdminAuthDialog(_settings.MasterAdminPassword, _settings.MasterAdminPasswordHash, "Global Settings");
        if (authDlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        var oldSettings = new AppSettings
        {
            DefaultPassword = _settings.DefaultPassword,
            MasterAdminPassword = _settings.MasterAdminPassword,
            PollingIntervalSeconds = _settings.PollingIntervalSeconds,
            RequestTimeoutSeconds = _settings.RequestTimeoutSeconds
        };

        using var dlg = new GlobalSettingsDialog(_settings);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _configService.SaveSettings(_settings);
            _auditLogService.LogGlobalSettingsChanged(oldSettings, _settings);

            SyncUiSettings();

            if (_settings.AutoRefreshEnabled)
            {
                StartAutoRefresh();
            }

            _ = RefreshStreamsAsync(isAuto: false);
        }
    }

    private void BtnRemoteUsers_Click(object? sender, EventArgs e)
    {
        using var authDlg = new AdminAuthDialog(_settings.MasterAdminPassword, _settings.MasterAdminPasswordHash, "Remote Users Management");
        if (authDlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        using var dlg = new RemoteUsersDialog(_settings);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _configService.SaveSettings(_settings);
            _remoteApiService.Stop();
            if (_settings.RemoteServerEnabled)
            {
                _remoteApiService.Start();
            }
        }
    }

    private void BtnToggleConsole_Click(object? sender, EventArgs e)
    {
        splitContainerMain.Panel2Collapsed = !splitContainerMain.Panel2Collapsed;
        menuItemToggleConsole.Checked = !splitContainerMain.Panel2Collapsed;
        btnToolConsole.Checked = !splitContainerMain.Panel2Collapsed;
    }

    private void CboAuditFilter_SelectedIndexChanged(object? sender, EventArgs e)
    {
        PopulateAuditLogs();
    }

    private void BtnCopyConsole_Click(object? sender, EventArgs e)
    {
        var entries = _auditLogService.GetEntries();
        var filter = cboAuditFilter.SelectedIndex;
        var sb = new StringBuilder();

        foreach (var entry in entries)
        {
            if (MatchesFilter(entry, filter))
            {
                sb.AppendLine(entry.ToString());
            }
        }

        var text = sb.ToString();
        if (!string.IsNullOrEmpty(text))
        {
            Clipboard.SetText(text);
            MessageBox.Show("Audit change console logs copied to clipboard!", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnClearConsole_Click(object? sender, EventArgs e)
    {
        var res = MessageBox.Show("Are you sure you want to clear the audit change console log history?", "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (res == DialogResult.Yes)
        {
            _auditLogService.ClearLogs();
            PopulateAuditLogs();
        }
    }

    private void BtnMockServer_Click(object? sender, EventArgs e)
    {
        if (_mockServer != null && _mockServer.IsRunning)
        {
            _mockServer.Stop();
            _mockServer.Dispose();
            _mockServer = null;

            btnToolMockServer.Text = "⚡ Start Local Demo Feed";
            btnToolMockServer.BackColor = SystemColors.Control;
            statusLabelMain.Text = "Local Demo Server stopped";
            _auditLogService.Log(AuditCategory.System, "Mock Server", "Demo Stopped", "Local simulated stream server stopped.");
        }
        else
        {
            try
            {
                _mockServer = new MockStreamServer(8000);
                _mockServer.Start();

                btnToolMockServer.Text = "⚡ Stop Demo Feed (:8000)";
                btnToolMockServer.BackColor = Color.LightGreen;
                statusLabelMain.Text = "Local Demo Server running on http://127.0.0.1:8000";
                _auditLogService.Log(AuditCategory.System, "Mock Server", "Demo Started", "Local simulated Icecast/SHOUTcast stream server started on port 8000.");

                _ = RefreshStreamsAsync(isAuto: false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start local mock server: {ex.Message}", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void GridStreams_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == colEnabled.Index)
        {
            var row = gridStreams.Rows[e.RowIndex];
            if (row.Tag is StreamChannel ch)
            {
                gridStreams.CommitEdit(DataGridViewDataErrorContexts.Commit);
                ch.Enabled = (bool)(row.Cells[colEnabled.Index].Value ?? false);
                _configService.SaveSettings(_settings);
                _auditLogService.LogChannelToggled(ch, ch.Enabled);

                UpdateRowDisplay(row, ch, _statusCache.GetValueOrDefault(ch.Id));
                UpdateSummaryFooter();
                _ = RefreshStreamsAsync(isAuto: false);
            }
        }
    }

    private void GridStreams_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
        {
            gridStreams.ClearSelection();
            gridStreams.Rows[e.RowIndex].Selected = true;
            gridStreams.CurrentCell = gridStreams.Rows[e.RowIndex].Cells[Math.Max(0, e.ColumnIndex)];
        }
    }

    private void GridStreams_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            if (e.ColumnIndex == colListeners.Index || e.ColumnIndex == colPeak.Index)
            {
                BtnShowListeners_Click(sender, EventArgs.Empty);
            }
            else
            {
                BtnAnalytics_Click(sender, EventArgs.Empty);
            }
        }
    }

    private void GridStreams_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.G)
        {
            BtnAnalytics_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.L)
        {
            BtnShowListeners_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Enter)
        {
            BtnEdit_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Space)
        {
            BtnToggleActive_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Delete)
        {
            BtnDelete_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.F5)
        {
            BtnRefresh_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
    }

    private void RestoreFromTray()
    {
        Show();
        WindowState = FormWindowState.Normal;
        ShowInTaskbar = true;
        BringToFront();
        Activate();
    }

    private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
    {
        RestoreFromTray();
    }

    private void TrayItemRestore_Click(object? sender, EventArgs e)
    {
        RestoreFromTray();
    }

    private void MainForm_Resize(object? sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Minimized && _settings.MinimizeToTray)
        {
            Hide();
            ShowInTaskbar = false;
            notifyIcon.ShowBalloonTip(1500, "Stream Monitor", "Running in background (System Tray). Double-click icon to open.", ToolTipIcon.Info);
        }
    }

    private void BtnCheckUpdates_Click(object? sender, EventArgs e)
    {
        using var dlg = new UpdateCheckDialog(_updateService);
        dlg.ShowDialog(this);
    }

    private void MenuItemAbout_Click(object? sender, EventArgs e)
    {
        MessageBox.Show(
            $"Icecast & SHOUTcast Live Stream Monitor\n" +
            $"Version {_updateService.CurrentVersion}\n\n" +
            "A lightweight, high-performance Windows Forms application designed to monitor live audio streams, " +
            "concurrent listeners, audio formats, bitrates, 24h charts, and configuration change audit logs.",
            "About Stream Monitor",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void MenuItemExit_Click(object? sender, EventArgs e)
    {
        _forceExit = true;
        Close();
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing && _settings.CloseToTray && !_forceExit)
        {
            e.Cancel = true;
            Hide();
            ShowInTaskbar = false;
            notifyIcon.ShowBalloonTip(1500, "Stream Monitor", "Continuing to monitor in background. Double-click tray icon to restore.", ToolTipIcon.Info);
            return;
        }

        _pollTimer.Stop();
        _scanCts?.Cancel();
        _audioStreamServer.Stop();
        _mockServer?.Dispose();
        _remoteApiService.Dispose();
        _historyService.SaveHistory();

        notifyIcon.Visible = false;
        notifyIcon.Dispose();

        if (WindowState == FormWindowState.Normal)
        {
            _settings.WindowWidth = Width;
            _settings.WindowHeight = Height;
            _settings.WindowLeft = Left;
            _settings.WindowTop = Top;
            _settings.IsMaximized = false;
        }
        else if (WindowState == FormWindowState.Maximized)
        {
            _settings.IsMaximized = true;
        }

        _configService.SaveSettings(_settings);
    }
}
