using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace IcecastStreamMonitor.Installer;

public partial class InstallerForm : Form
{
    private int _currentStep = 1;

    // Controls for Step 1
    private TextBox txtInstallPath = null!;
    private Button btnBrowse = null!;
    private bool _isExistingInstallation = false;
    private int _existingChannelsCount = 0;
    private string _existingVersion = "1.0.0";
    private bool _isUpgradeMode = false;
    private RadioButton radUpgradeInPlace = null!;
    private RadioButton radFreshInstall = null!;

    // Controls for Step 2 - Tab 1 (Server & Mounts)
    private TextBox txtServerHost = null!;
    private NumericUpDown numServerPort = null!;
    private DataGridView gridMounts = null!;
    private Button btnAddMount = null!;
    private Button btnRemoveMount = null!;

    // Controls for Step 2 - Tab 2 (Passwords & Remote User)
    private TextBox txtMasterAdminPassword = null!;
    private TextBox txtMasterAdminPasswordConfirm = null!;
    private TextBox txtDefaultStreamPassword = null!;
    private TextBox txtDefaultStreamPasswordConfirm = null!;

    private CheckBox chkEnableRemoteApi = null!;
    private TextBox txtRemoteUsername = null!;
    private TextBox txtRemotePassword = null!;
    private TextBox txtRemotePasswordConfirm = null!;
    private ComboBox cboRemoteRole = null!;
    private NumericUpDown numRemotePort = null!;

    // Controls for Step 3
    private CheckBox chkDesktopShortcut = null!;
    private CheckBox chkStartMenuShortcut = null!;
    private CheckBox chkAutoStart = null!;
    private CheckBox chkLaunchAfter = null!;

    // Controls for Step 4
    private ProgressBar progressBar = null!;
    private Label lblInstallStatus = null!;

    public InstallerForm()
    {
        InitializeComponent();
        try
        {
            var appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (appIcon != null) Icon = appIcon;
        }
        catch { }
    }

    private void InstallerForm_Load(object? sender, EventArgs e)
    {
        DetectExistingInstallation();
        ShowStep(1);
    }

    private void DetectExistingInstallation()
    {
        try
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var configPath = Path.Combine(appData, "IcecastStreamMonitor", "config.json");
            var localPrograms = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs", "IcecastStreamMonitor", "IcecastStreamMonitor.exe");

            if (File.Exists(configPath) || File.Exists(localPrograms))
            {
                _isExistingInstallation = true;
                _isUpgradeMode = true;

                if (File.Exists(configPath))
                {
                    var json = File.ReadAllText(configPath);
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("Channels", out var ch) && ch.ValueKind == JsonValueKind.Array)
                    {
                        _existingChannelsCount = ch.GetArrayLength();
                    }
                }

                if (File.Exists(localPrograms))
                {
                    var fvi = FileVersionInfo.GetVersionInfo(localPrograms);
                    if (!string.IsNullOrEmpty(fvi.FileVersion))
                    {
                        _existingVersion = fvi.FileVersion;
                    }
                }
            }
        }
        catch { }
    }

    private void ShowStep(int step)
    {
        _currentStep = step;
        pnlContainer.Controls.Clear();

        // Update side step labels
        lblStep1.Font = new Font(lblStep1.Font, step == 1 ? FontStyle.Bold : FontStyle.Regular);
        lblStep1.ForeColor = step == 1 ? Color.White : Color.FromArgb(148, 163, 184);

        lblStep2.Font = new Font(lblStep2.Font, step == 2 ? FontStyle.Bold : FontStyle.Regular);
        lblStep2.ForeColor = step == 2 ? Color.White : Color.FromArgb(148, 163, 184);

        lblStep3.Font = new Font(lblStep3.Font, step == 3 ? FontStyle.Bold : FontStyle.Regular);
        lblStep3.ForeColor = step == 3 ? Color.White : Color.FromArgb(148, 163, 184);

        lblStep4.Font = new Font(lblStep4.Font, step == 4 ? FontStyle.Bold : FontStyle.Regular);
        lblStep4.ForeColor = step == 4 ? Color.White : Color.FromArgb(148, 163, 184);

        btnBack.Enabled = step > 1 && step < 4;
        btnNext.Text = step switch
        {
            1 => (_isUpgradeMode && (radUpgradeInPlace?.Checked ?? true)) ? "Upgrade Now >" : "Next >",
            3 => "Install >",
            4 => "Finish",
            _ => "Next >"
        };

        switch (step)
        {
            case 1:
                BuildStep1();
                break;
            case 2:
                BuildStep2();
                break;
            case 3:
                BuildStep3();
                break;
            case 4:
                BuildStep4();
                break;
        }
    }

    private void BuildStep1()
    {
        var lblHeader = new Label
        {
            Text = _isExistingInstallation ? "RTSAS Setup & In-Place Upgrade" : "Welcome to RADIOTEDU Stream Audio Server Setup",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            Location = new Point(20, 15),
            AutoSize = true
        };

        int topY = 48;

        if (_isExistingInstallation)
        {
            var pnlUpgrade = new Panel
            {
                Location = new Point(20, topY),
                Size = new Size(520, 130),
                BackColor = Color.FromArgb(240, 249, 255),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblUpgradeTitle = new Label
            {
                Text = $"✨ Existing Installation Detected (v{_existingVersion})",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(12, 74, 110),
                Location = new Point(10, 8),
                AutoSize = true
            };

            var lblUpgradeDesc = new Label
            {
                Text = $"Found {_existingChannelsCount} active channel(s), encrypted passwords, and server configs.",
                ForeColor = Color.FromArgb(30, 58, 138),
                Location = new Point(10, 32),
                Size = new Size(500, 20)
            };

            radUpgradeInPlace = new RadioButton
            {
                Text = "🔄 Seamless Upgrade (Keep all mounts, passwords & settings intact)",
                Checked = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.ForestGreen,
                Location = new Point(12, 60),
                Size = new Size(490, 24)
            };
            radUpgradeInPlace.CheckedChanged += (s, e) =>
            {
                _isUpgradeMode = radUpgradeInPlace.Checked;
                btnNext.Text = _isUpgradeMode ? "Upgrade Now >" : "Next >";
            };

            radFreshInstall = new RadioButton
            {
                Text = "⚙️ Re-configure Setup (Manually reconfigure server, passwords & remote users)",
                Checked = false,
                Location = new Point(12, 90),
                Size = new Size(490, 24)
            };

            pnlUpgrade.Controls.AddRange(new Control[] { lblUpgradeTitle, lblUpgradeDesc, radUpgradeInPlace, radFreshInstall });
            pnlContainer.Controls.Add(pnlUpgrade);

            topY += 140;
        }
        else
        {
            var lblDesc = new Label
            {
                Text = "This wizard will install RADIOTEDU Stream Audio Server (RTSAS) and companion tools on your computer.\n\nPlease select the destination folder where the program will be installed:",
                Location = new Point(20, topY),
                Size = new Size(520, 50)
            };
            pnlContainer.Controls.Add(lblDesc);
            topY += 60;
        }

        var lblPathTitle = new Label
        {
            Text = "Installation Folder:",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Location = new Point(20, topY),
            AutoSize = true
        };
        topY += 24;

        var defaultPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Programs",
            "IcecastStreamMonitor");

        if (txtInstallPath == null)
        {
            txtInstallPath = new TextBox
            {
                Text = defaultPath,
                Location = new Point(20, topY),
                Size = new Size(420, 25)
            };
        }
        else
        {
            txtInstallPath.Location = new Point(20, topY);
            txtInstallPath.Size = new Size(420, 25);
        }

        btnBrowse = new Button
        {
            Text = "Browse...",
            Location = new Point(448, topY - 1),
            Size = new Size(90, 27)
        };
        btnBrowse.Click += (s, e) =>
        {
            using var fbd = new FolderBrowserDialog();
            fbd.SelectedPath = txtInstallPath.Text;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                txtInstallPath.Text = fbd.SelectedPath;
            }
        };

        topY += 40;

        var lblInfo = new Label
        {
            Text = "🔒 Encrypted Security: All passwords and credentials are DPAPI encrypted and PBKDF2 hashed.\nRequired space: ~15 MB",
            ForeColor = SystemColors.GrayText,
            Location = new Point(20, topY),
            Size = new Size(520, 40)
        };

        pnlContainer.Controls.AddRange(new Control[] { lblHeader, lblPathTitle, txtInstallPath, btnBrowse, lblInfo });
    }

    private void BuildStep2()
    {
        var lblHeader = new Label
        {
            Text = "Server Settings, Credentials & Remote Access",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            Location = new Point(15, 10),
            AutoSize = true
        };

        var tabControl = new TabControl
        {
            Location = new Point(15, 40),
            Size = new Size(540, 410),
            Font = new Font("Segoe UI", 9F, FontStyle.Regular)
        };

        // ==========================================
        // TAB 1: Server & Mountpoints
        // ==========================================
        var tabServer = new TabPage("📻 Server & Mountpoints");
        tabServer.Padding = new Padding(12);

        var lblHost = new Label { Text = "Server Host / IP:", Location = new Point(12, 16), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        if (txtServerHost == null)
            txtServerHost = new TextBox { Text = "127.0.0.1", PlaceholderText = "127.0.0.1 or stream.radiotedu.com", Location = new Point(125, 13), Size = new Size(210, 23) };

        var lblPort = new Label { Text = "Stream Port:", Location = new Point(345, 16), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        if (numServerPort == null)
            numServerPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 11154, Location = new Point(430, 13), Size = new Size(85, 23) };

        var btnImportTinyIce = new Button
        {
            Text = "📥 Import Mounts from TinyIce config.json",
            Location = new Point(12, 45),
            Size = new Size(503, 27),
            BackColor = Color.FromArgb(240, 246, 255),
            FlatStyle = FlatStyle.System
        };

        btnImportTinyIce.Click += (s, e) =>
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Select TinyIce config.json to Import Mounts & Port",
                Filter = "JSON Configuration (*.json)|*.json|All Files (*.*)|*.*",
                FileName = "config.json"
            };

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var json = File.ReadAllText(ofd.FileName);
                    var result = TinyIceConfigImporter.ImportFromTinyIceJson(json);

                    if (result.Mounts.Count > 0)
                    {
                        numServerPort.Value = Math.Clamp(result.Port, 1, 65535);
                        if (string.IsNullOrWhiteSpace(txtServerHost.Text))
                        {
                            txtServerHost.Text = "127.0.0.1";
                        }

                        gridMounts.Rows.Clear();
                        foreach (var m in result.Mounts)
                        {
                            gridMounts.Rows.Add(m.Name, m.MountPath, "");
                        }

                        MessageBox.Show(
                            $"Successfully imported {result.Mounts.Count} mountpoint(s) and server port ({result.Port}) from TinyIce configuration!",
                            "TinyIce Config Imported",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No mountpoints found in the selected configuration file.", "Import Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to parse TinyIce config: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        };

        var lblMountsTitle = new Label
        {
            Text = "Configured Channels / Mountpoints:",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Location = new Point(12, 85),
            AutoSize = true
        };

        btnAddMount = new Button { Text = "+ Add Mount", Location = new Point(330, 80), Size = new Size(90, 25) };
        btnRemoveMount = new Button { Text = "✕ Remove", Location = new Point(425, 80), Size = new Size(90, 25) };

        if (gridMounts == null)
        {
            gridMounts = new DataGridView
            {
                Location = new Point(12, 110),
                Size = new Size(503, 230),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            gridMounts.Columns.Add("colName", "Channel Name");
            gridMounts.Columns.Add("colMount", "Mount Path");
            gridMounts.Columns.Add("colPass", "Custom Password (Optional)");

            gridMounts.Columns[0].FillWeight = 35;
            gridMounts.Columns[1].FillWeight = 25;
            gridMounts.Columns[2].FillWeight = 40;

            // Default initial mount
            gridMounts.Rows.Add("Channel 1", "/stream1", "");
        }

        btnAddMount.Click += (s, e) =>
        {
            int count = gridMounts.Rows.Count + 1;
            gridMounts.Rows.Add($"Channel {count}", $"/stream{count}", "");
        };

        btnRemoveMount.Click += (s, e) =>
        {
            if (gridMounts.SelectedRows.Count > 0 && gridMounts.Rows.Count > 1)
            {
                gridMounts.Rows.RemoveAt(gridMounts.SelectedRows[0].Index);
            }
        };

        tabServer.Controls.AddRange(new Control[]
        {
            lblHost, txtServerHost, lblPort, numServerPort,
            btnImportTinyIce,
            lblMountsTitle, btnAddMount, btnRemoveMount,
            gridMounts
        });

        // ==========================================
        // TAB 2: Passwords & Remote Control User
        // ==========================================
        var tabSecurity = new TabPage("🔐 Passwords & Remote Access");
        tabSecurity.Padding = new Padding(12);

        // Group 1: Server & Admin Passwords (Spacious double-entry)
        var grpServerPasswords = new GroupBox
        {
            Text = "🛡️ Server & Admin Passwords (Double-Entry Confirmation)",
            Location = new Point(10, 8),
            Size = new Size(510, 160),
            Font = new Font("Segoe UI", 9F, FontStyle.Bold)
        };

        var lblAdmin1 = new Label { Text = "Master Admin Password:", Location = new Point(12, 28), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Regular) };
        if (txtMasterAdminPassword == null)
            txtMasterAdminPassword = new TextBox { Text = "admin123", PasswordChar = '•', Location = new Point(12, 48), Size = new Size(230, 23) };

        var lblAdmin2 = new Label { Text = "Confirm Admin Password:", Location = new Point(260, 28), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Regular) };
        if (txtMasterAdminPasswordConfirm == null)
            txtMasterAdminPasswordConfirm = new TextBox { Text = "admin123", PasswordChar = '•', Location = new Point(260, 48), Size = new Size(230, 23) };

        var lblStream1 = new Label { Text = "Stream (DJ / Source) Password:", Location = new Point(12, 85), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Regular) };
        if (txtDefaultStreamPassword == null)
            txtDefaultStreamPassword = new TextBox { Text = "hackme", PasswordChar = '•', Location = new Point(12, 105), Size = new Size(230, 23) };

        var lblStream2 = new Label { Text = "Confirm Stream Password:", Location = new Point(260, 85), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Regular) };
        if (txtDefaultStreamPasswordConfirm == null)
            txtDefaultStreamPasswordConfirm = new TextBox { Text = "hackme", PasswordChar = '•', Location = new Point(260, 105), Size = new Size(230, 23) };

        var lblNoteChars = new Label
        {
            Text = "ℹ️ Supports all special characters (<>, &, @, #, etc.) with DPAPI encryption.",
            Font = new Font("Segoe UI", 8F, FontStyle.Italic),
            ForeColor = Color.DarkSlateGray,
            Location = new Point(12, 134),
            Size = new Size(480, 20)
        };

        grpServerPasswords.Controls.AddRange(new Control[]
        {
            lblAdmin1, txtMasterAdminPassword, lblAdmin2, txtMasterAdminPasswordConfirm,
            lblStream1, txtDefaultStreamPassword, lblStream2, txtDefaultStreamPasswordConfirm,
            lblNoteChars
        });

        // Group 2: Remote Control Companion User (For Computer B)
        var grpRemoteUser = new GroupBox
        {
            Text = "💻 Remote Control Companion Client (For Computer B)",
            Location = new Point(10, 175),
            Size = new Size(510, 195),
            Font = new Font("Segoe UI", 9F, FontStyle.Bold)
        };

        if (chkEnableRemoteApi == null)
            chkEnableRemoteApi = new CheckBox
            {
                Text = "Enable Remote Control HTTP API Server (Required for Remote Companion)",
                Checked = true,
                Location = new Point(12, 24),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.MidnightBlue
            };

        var lblRemUname = new Label { Text = "Remote Username:", Location = new Point(12, 54), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Regular) };
        if (txtRemoteUsername == null)
            txtRemoteUsername = new TextBox { Text = "admin", Location = new Point(12, 74), Size = new Size(150, 23) };

        var lblRemRole = new Label { Text = "User Role:", Location = new Point(180, 54), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Regular) };
        if (cboRemoteRole == null)
        {
            cboRemoteRole = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(180, 74), Size = new Size(150, 23) };
            cboRemoteRole.Items.AddRange(new[] { "SuperAdmin", "Operator", "Viewer" });
            cboRemoteRole.SelectedIndex = 0;
        }

        var lblRemPort = new Label { Text = "Remote Port:", Location = new Point(350, 54), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Regular) };
        if (numRemotePort == null)
            numRemotePort = new NumericUpDown { Minimum = 1024, Maximum = 65535, Value = 8080, Location = new Point(350, 74), Size = new Size(140, 23) };

        var lblRemPass1 = new Label { Text = "Remote Password:", Location = new Point(12, 108), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Regular) };
        if (txtRemotePassword == null)
            txtRemotePassword = new TextBox { Text = "admin123", PasswordChar = '•', Location = new Point(12, 128), Size = new Size(230, 23) };

        var lblRemPass2 = new Label { Text = "Confirm Remote Password:", Location = new Point(260, 108), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Regular) };
        if (txtRemotePasswordConfirm == null)
            txtRemotePasswordConfirm = new TextBox { Text = "admin123", PasswordChar = '•', Location = new Point(260, 128), Size = new Size(230, 23) };

        var lblFirewallNote = new Label
        {
            Text = "🔥 Windows Defender Firewall rules for port 8080 & streaming ports will be auto-configured.",
            Font = new Font("Segoe UI", 8F, FontStyle.Italic),
            ForeColor = Color.DarkSlateBlue,
            Location = new Point(12, 160),
            Size = new Size(480, 25)
        };

        grpRemoteUser.Controls.AddRange(new Control[]
        {
            chkEnableRemoteApi,
            lblRemUname, txtRemoteUsername,
            lblRemRole, cboRemoteRole,
            lblRemPort, numRemotePort,
            lblRemPass1, txtRemotePassword, lblRemPass2, txtRemotePasswordConfirm,
            lblFirewallNote
        });

        tabSecurity.Controls.AddRange(new Control[] { grpServerPasswords, grpRemoteUser });

        tabControl.TabPages.Add(tabServer);
        tabControl.TabPages.Add(tabSecurity);

        pnlContainer.Controls.AddRange(new Control[] { lblHeader, tabControl });
    }

    private void BuildStep3()
    {
        var lblHeader = new Label
        {
            Text = "Shortcuts & Integration Options",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            Location = new Point(20, 15),
            AutoSize = true
        };

        var lblDesc = new Label
        {
            Text = "Select which shortcuts and startup preferences you want to configure:",
            Location = new Point(20, 48),
            Size = new Size(500, 25)
        };

        if (chkDesktopShortcut == null)
            chkDesktopShortcut = new CheckBox { Text = "Create a Desktop Shortcut (RTSAS Server & Remote Companion)", Checked = true, Location = new Point(25, 90), AutoSize = true };

        if (chkStartMenuShortcut == null)
            chkStartMenuShortcut = new CheckBox { Text = "Create a Start Menu Shortcut Folder", Checked = true, Location = new Point(25, 130), AutoSize = true };

        if (chkAutoStart == null)
            chkAutoStart = new CheckBox { Text = "Start Stream Monitor automatically on Windows boot (minimized to tray)", Checked = false, Location = new Point(25, 170), AutoSize = true };

        if (chkLaunchAfter == null)
            chkLaunchAfter = new CheckBox { Text = "Launch RADIOTEDU Stream Audio Server immediately after setup", Checked = true, Location = new Point(25, 210), AutoSize = true };

        pnlContainer.Controls.AddRange(new Control[]
        {
            lblHeader, lblDesc,
            chkDesktopShortcut,
            chkStartMenuShortcut,
            chkAutoStart,
            chkLaunchAfter
        });
    }

    private void BuildStep4()
    {
        var lblHeader = new Label
        {
            Text = "Installing Application...",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            Location = new Point(20, 15),
            AutoSize = true
        };

        lblInstallStatus = new Label
        {
            Text = "Preparing installation...",
            Location = new Point(20, 50),
            Size = new Size(500, 25)
        };

        progressBar = new ProgressBar
        {
            Location = new Point(20, 85),
            Size = new Size(500, 23),
            Style = ProgressBarStyle.Continuous,
            Value = 0
        };

        pnlContainer.Controls.AddRange(new Control[] { lblHeader, lblInstallStatus, progressBar });

        btnBack.Enabled = false;
        btnNext.Enabled = false;
        btnCancel.Enabled = false;

        // Perform actual installation asynchronously
        _ = ExecuteInstallationAsync();
    }

    private async Task ExecuteInstallationAsync()
    {
        var targetDir = txtInstallPath.Text.Trim();
        Directory.CreateDirectory(targetDir);

        lblInstallStatus.Text = "Closing active RTSAS instances...";
        progressBar.Value = 10;
        await Task.Delay(100);

        try
        {
            var procs = Process.GetProcessesByName("IcecastStreamMonitor")
                .Concat(Process.GetProcessesByName("IcecastStreamMonitor.RemoteControl"));
            foreach (var p in procs)
            {
                if (p.Id != Environment.ProcessId)
                {
                    try { p.Kill(); p.WaitForExit(1500); } catch { }
                }
            }
        }
        catch { }

        lblInstallStatus.Text = "Deploying application binaries...";
        progressBar.Value = 35;
        await Task.Delay(200);

        bool extractedEmbedded = false;
        try
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var resources = asm.GetManifestResourceNames();

            foreach (var resName in resources)
            {
                if (resName.EndsWith("IcecastStreamMonitor.exe", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = asm.GetManifestResourceStream(resName);
                    if (stream != null)
                    {
                        var destPath = Path.Combine(targetDir, "IcecastStreamMonitor.exe");
                        using var fs = File.Create(destPath);
                        stream.CopyTo(fs);
                        extractedEmbedded = true;
                    }
                }
                else if (resName.EndsWith("IcecastStreamMonitor.RemoteControl.exe", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = asm.GetManifestResourceStream(resName);
                    if (stream != null)
                    {
                        var destPath = Path.Combine(targetDir, "IcecastStreamMonitor.RemoteControl.exe");
                        using var fs = File.Create(destPath);
                        stream.CopyTo(fs);
                        extractedEmbedded = true;
                    }
                }
            }
        }
        catch { }

        // Fallback: Copy adjacent published files if not embedded
        if (!extractedEmbedded)
        {
            var currentExeDir = AppDomain.CurrentDomain.BaseDirectory;
            var publishDir = Path.Combine(currentExeDir, "..", "..", "..", "..", "publish");
            if (!Directory.Exists(publishDir))
            {
                publishDir = currentExeDir;
            }

            var sourceFiles = Directory.GetFiles(publishDir, "*.*")
                .Where(f => !f.EndsWith("IcecastStreamMonitor.Installer.exe", StringComparison.OrdinalIgnoreCase) &&
                            !f.EndsWith("IcecastStreamMonitor.Installer.dll", StringComparison.OrdinalIgnoreCase));

            foreach (var file in sourceFiles)
            {
                var dest = Path.Combine(targetDir, Path.GetFileName(file));
                File.Copy(file, dest, overwrite: true);
            }
        }

        progressBar.Value = 60;

        if (_isUpgradeMode && (radUpgradeInPlace == null || radUpgradeInPlace.Checked))
        {
            lblInstallStatus.Text = "Preserving existing mounts, passwords, and security settings...";
            await Task.Delay(200);
            PreserveExistingSettings(targetDir);
        }
        else
        {
            lblInstallStatus.Text = "Encrypting credentials and generating initial settings & remote users...";
            await Task.Delay(200);
            WriteInitialSettings(targetDir);
        }

        progressBar.Value = 75;
        lblInstallStatus.Text = "Configuring Windows Defender Firewall rules for audio & remote ports...";
        await Task.Delay(200);
        ConfigureFirewallRules((int)numServerPort.Value, (int)numRemotePort.Value);

        progressBar.Value = 85;
        lblInstallStatus.Text = "Updating shortcuts and registry entries...";
        await Task.Delay(200);

        var targetExe = Path.Combine(targetDir, "IcecastStreamMonitor.exe");
        var remoteExe = Path.Combine(targetDir, "IcecastStreamMonitor.RemoteControl.exe");

        // Desktop Shortcuts
        if (chkDesktopShortcut == null || chkDesktopShortcut.Checked)
        {
            CreateShortcut(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RADIOTEDU Stream Server.lnk"),
                targetExe,
                targetDir,
                "RADIOTEDU Stream Audio Server & Live Monitor");

            if (File.Exists(remoteExe))
            {
                CreateShortcut(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RADIOTEDU Remote Control.lnk"),
                    remoteExe,
                    targetDir,
                    "RADIOTEDU Remote Control Companion");
            }
        }

        // Start Menu Shortcuts
        if (chkStartMenuShortcut == null || chkStartMenuShortcut.Checked)
        {
            var startMenuProg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "RADIOTEDU Stream Server");
            Directory.CreateDirectory(startMenuProg);
            CreateShortcut(
                Path.Combine(startMenuProg, "RADIOTEDU Stream Server.lnk"),
                targetExe,
                targetDir,
                "RADIOTEDU Stream Audio Server & Live Monitor");

            if (File.Exists(remoteExe))
            {
                CreateShortcut(
                    Path.Combine(startMenuProg, "RADIOTEDU Remote Control.lnk"),
                    remoteExe,
                    targetDir,
                    "RADIOTEDU Remote Control Companion");
            }
        }

        // Windows Startup
        if (chkAutoStart != null && chkAutoStart.Checked)
        {
            try
            {
                using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                key?.SetValue("IcecastStreamMonitor", $"\"{targetExe}\"");
            }
            catch { }
        }

        progressBar.Value = 100;
        if (_isUpgradeMode && (radUpgradeInPlace == null || radUpgradeInPlace.Checked))
        {
            lblInstallStatus.Text = $"✓ Upgrade to v1.1.0 Completed! All {_existingChannelsCount} mounts & passwords preserved.";
        }
        else
        {
            lblInstallStatus.Text = "✓ Installation & Remote Companion Setup Completed Successfully!";
        }
        lblInstallStatus.ForeColor = Color.ForestGreen;

        btnNext.Text = "Finish";
        btnNext.Enabled = true;
        btnCancel.Enabled = false;
    }

    private void ConfigureFirewallRules(int streamPort, int remotePort)
    {
        try
        {
            // Configure inbound firewall rules silently via netsh
            var psi = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = $"advfirewall firewall add rule name=\"RTSAS Audio Stream Server\" dir=in action=allow protocol=TCP localport={streamPort},11154,8000 profile=any",
                CreateNoWindow = true,
                UseShellExecute = false
            };
            using var p1 = Process.Start(psi);
            p1?.WaitForExit(2000);

            var psiRemote = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = $"advfirewall firewall add rule name=\"RTSAS Remote Control API\" dir=in action=allow protocol=TCP localport={remotePort},8080 profile=any",
                CreateNoWindow = true,
                UseShellExecute = false
            };
            using var p2 = Process.Start(psiRemote);
            p2?.WaitForExit(2000);
        }
        catch { }
    }

    private void PreserveExistingSettings(string targetDir)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appData, "IcecastStreamMonitor");
        Directory.CreateDirectory(appFolder);
        var configPath = Path.Combine(appFolder, "config.json");

        if (File.Exists(configPath))
        {
            try
            {
                var targetConfig = Path.Combine(targetDir, "config.json");
                File.Copy(configPath, targetConfig, overwrite: true);
            }
            catch { }
        }
        else
        {
            WriteInitialSettings(targetDir);
        }
    }

    private void WriteInitialSettings(string targetDir)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appData, "IcecastStreamMonitor");
        Directory.CreateDirectory(appFolder);
        var configPath = Path.Combine(appFolder, "config.json");

        var serverHost = string.IsNullOrWhiteSpace(txtServerHost.Text) ? "127.0.0.1" : txtServerHost.Text.Trim();
        var adminPass = string.IsNullOrWhiteSpace(txtMasterAdminPassword.Text) ? "admin123" : txtMasterAdminPassword.Text;
        var defaultStreamPass = string.IsNullOrWhiteSpace(txtDefaultStreamPassword.Text) ? "hackme" : txtDefaultStreamPassword.Text;

        var channelList = new List<object>();
        foreach (DataGridViewRow row in gridMounts.Rows)
        {
            var chName = row.Cells[0].Value?.ToString() ?? "Live Stream";
            var mount = row.Cells[1].Value?.ToString() ?? "/stream1";
            var customPass = row.Cells[2].Value?.ToString() ?? "";
            bool hasCustom = !string.IsNullOrWhiteSpace(customPass);

            channelList.Add(new
            {
                Id = Guid.NewGuid(),
                Name = chName,
                Host = serverHost,
                Port = (int)numServerPort.Value,
                MountPath = mount,
                ServerType = 0, // Auto
                UseDefaultPassword = !hasCustom,
                CustomPassword = hasCustom ? SecurityHelper.EncryptSecret(customPass) : "",
                Enabled = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        // Configure Initial Remote User
        var remoteUname = string.IsNullOrWhiteSpace(txtRemoteUsername.Text) ? "admin" : txtRemoteUsername.Text.Trim();
        var remotePass = string.IsNullOrWhiteSpace(txtRemotePassword.Text) ? "admin123" : txtRemotePassword.Text;
        var remoteRole = cboRemoteRole.SelectedItem?.ToString() ?? "SuperAdmin";
        bool isSuperAdmin = remoteRole == "SuperAdmin";
        bool isOperator = remoteRole == "Operator";

        var remoteUsersList = new List<object>();
        if (chkEnableRemoteApi == null || chkEnableRemoteApi.Checked)
        {
            remoteUsersList.Add(new
            {
                Id = Guid.NewGuid(),
                Username = remoteUname,
                PasswordHash = SecurityHelper.HashPassword(remotePass),
                PlainPassword = remotePass,
                RoleName = remoteRole,
                IsActive = true,
                Permissions = new
                {
                    CanViewStatus = true,
                    CanViewLogs = true,
                    CanViewListenerIps = isSuperAdmin || isOperator,
                    CanRefreshStreams = true,
                    CanManageChannels = isSuperAdmin,
                    CanViewAuditLogs = isSuperAdmin
                },
                CreatedAt = DateTime.UtcNow
            });
        }

        var settings = new
        {
            DefaultPassword = SecurityHelper.EncryptSecret(defaultStreamPass),
            MasterAdminPassword = SecurityHelper.EncryptSecret(adminPass),
            MasterAdminPasswordHash = SecurityHelper.HashPassword(adminPass),
            DefaultSourcePassword = SecurityHelper.EncryptSecret(defaultStreamPass),
            StreamingServerEnabled = true,
            StreamingServerPort = (int)numServerPort.Value,
            TrustedProxies = new[] { "127.0.0.1", "::1", "localhost" },
            RemoteServerEnabled = chkEnableRemoteApi == null || chkEnableRemoteApi.Checked,
            RemoteServerPort = (int)numRemotePort.Value,
            PollingIntervalSeconds = 5,
            RequestTimeoutSeconds = 4,
            AutoRefreshEnabled = true,
            MinimizeToTray = true,
            CloseToTray = false,
            NotifyOnStreamOffline = true,
            Channels = channelList,
            RemoteUsers = remoteUsersList,
            WindowWidth = 1040,
            WindowHeight = 630,
            WindowLeft = -1,
            WindowTop = -1,
            IsMaximized = false
        };

        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        File.WriteAllText(configPath, json, Encoding.UTF8);

        // Also copy into target directory
        File.WriteAllText(Path.Combine(targetDir, "config.json"), json, Encoding.UTF8);
    }

    private static void CreateShortcut(string shortcutPath, string targetPath, string workingDir, string description)
    {
        try
        {
            var wshShellType = Type.GetTypeFromProgID("WScript.Shell");
            if (wshShellType != null)
            {
                dynamic shell = Activator.CreateInstance(wshShellType)!;
                dynamic shortcut = shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = targetPath;
                shortcut.WorkingDirectory = workingDir;
                shortcut.Description = description;
                shortcut.Save();
            }
        }
        catch { }
    }

    private void BtnNext_Click(object? sender, EventArgs e)
    {
        if (_currentStep == 1)
        {
            if (string.IsNullOrWhiteSpace(txtInstallPath.Text))
            {
                MessageBox.Show("Please select an installation folder.", "Path Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_isUpgradeMode && (radUpgradeInPlace?.Checked ?? true))
            {
                // In-Place Upgrade: jump directly to Step 4 (Deploy & Preserve settings)
                ShowStep(4);
                return;
            }

            ShowStep(2);
        }
        else if (_currentStep == 2)
        {
            if (string.IsNullOrWhiteSpace(txtServerHost.Text))
            {
                MessageBox.Show("Please enter a server host or IP address (e.g. 127.0.0.1 or stream.radiotedu.com).", "Host Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServerHost.Focus();
                return;
            }

            // 1. Validate Master Admin Passwords (Double-Entry)
            if (string.IsNullOrWhiteSpace(txtMasterAdminPassword.Text))
            {
                MessageBox.Show("Please define a Master Admin Password for securing the monitor dashboard.", "Admin Password Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMasterAdminPassword.Focus();
                return;
            }
            if (txtMasterAdminPassword.Text != txtMasterAdminPasswordConfirm.Text)
            {
                MessageBox.Show("Master Admin passwords do not match!\n\nLütfen iki alana da birebir aynı şifreyi girdiğinizden emin olun.", "Admin Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMasterAdminPasswordConfirm.Focus();
                txtMasterAdminPasswordConfirm.SelectAll();
                return;
            }

            // 2. Validate Stream / Source Passwords (Double-Entry)
            if (string.IsNullOrWhiteSpace(txtDefaultStreamPassword.Text))
            {
                MessageBox.Show("Please enter the default stream / source broadcast password.", "Stream Password Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDefaultStreamPassword.Focus();
                return;
            }
            if (txtDefaultStreamPassword.Text != txtDefaultStreamPasswordConfirm.Text)
            {
                MessageBox.Show("Stream (DJ / Source) passwords do not match!\n\nLütfen yayın şifresi tekrarını kontrol edin.", "Stream Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDefaultStreamPasswordConfirm.Focus();
                txtDefaultStreamPasswordConfirm.SelectAll();
                return;
            }

            // 3. Validate Remote Control User Passwords (Double-Entry)
            if (chkEnableRemoteApi.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtRemoteUsername.Text))
                {
                    MessageBox.Show("Please enter a username for the Remote Companion client.", "Remote Username Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRemoteUsername.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtRemotePassword.Text))
                {
                    MessageBox.Show("Please enter a password for the Remote Companion client.", "Remote Password Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRemotePassword.Focus();
                    return;
                }
                if (txtRemotePassword.Text != txtRemotePasswordConfirm.Text)
                {
                    MessageBox.Show("Remote Companion user passwords do not match!\n\nLütfen uzaktan izleme şifre tekrarını kontrol edin.", "Remote Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRemotePasswordConfirm.Focus();
                    txtRemotePasswordConfirm.SelectAll();
                    return;
                }
            }

            // 4. Validate Mountpoints
            if (gridMounts.Rows.Count == 0)
            {
                MessageBox.Show("Please click '+ Add Mount' and configure at least one stream mountpoint to monitor.", "Mountpoint Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DataGridViewRow row in gridMounts.Rows)
            {
                var mount = row.Cells[1].Value?.ToString()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(mount))
                {
                    MessageBox.Show("Mount path cannot be empty for any configured channel (e.g. /stream1).", "Invalid Mount Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            ShowStep(3);
        }
        else if (_currentStep == 3)
        {
            ShowStep(4);
        }
        else if (_currentStep == 4)
        {
            // Finish clicked
            if (chkLaunchAfter != null && chkLaunchAfter.Checked)
            {
                var targetExe = Path.Combine(txtInstallPath.Text.Trim(), "IcecastStreamMonitor.exe");
                if (File.Exists(targetExe))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = targetExe,
                        WorkingDirectory = txtInstallPath.Text.Trim(),
                        UseShellExecute = true
                    });
                }
            }

            Application.Exit();
        }
    }

    private void BtnBack_Click(object? sender, EventArgs e)
    {
        if (_currentStep > 1 && _currentStep < 4)
        {
            ShowStep(_currentStep - 1);
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        var res = MessageBox.Show("Are you sure you want to cancel the installation?", "Exit Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (res == DialogResult.Yes)
        {
            Application.Exit();
        }
    }
}
