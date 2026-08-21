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

    // Controls for Step 2 (Multi-channel & Security)
    private TextBox txtServerHost = null!;
    private NumericUpDown numServerPort = null!;
    private TextBox txtMasterAdminPassword = null!;
    private TextBox txtDefaultStreamPassword = null!;
    private DataGridView gridMounts = null!;
    private Button btnAddMount = null!;
    private Button btnRemoveMount = null!;

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
    }

    private void InstallerForm_Load(object? sender, EventArgs e)
    {
        ShowStep(1);
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
            Text = "Welcome & Destination Folder",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            Location = new Point(20, 15),
            AutoSize = true
        };

        var lblDesc = new Label
        {
            Text = "This wizard will install Icecast & SHOUTcast Live Stream Monitor on your computer.\n\nPlease select the folder where the program will be installed:",
            Location = new Point(20, 50),
            Size = new Size(440, 45)
        };

        var defaultPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Programs",
            "IcecastStreamMonitor");

        if (txtInstallPath == null)
        {
            txtInstallPath = new TextBox
            {
                Text = defaultPath,
                Location = new Point(20, 110),
                Size = new Size(340, 23)
            };
        }

        btnBrowse = new Button
        {
            Text = "Browse...",
            Location = new Point(368, 109),
            Size = new Size(80, 25)
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

        var lblInfo = new Label
        {
            Text = "Encrypted Security: All channel passwords and Master Admin keys will be encrypted at rest in configuration JSON.\nRequired space: ~15 MB",
            ForeColor = SystemColors.GrayText,
            Location = new Point(20, 155),
            Size = new Size(440, 45)
        };

        pnlContainer.Controls.AddRange(new Control[] { lblHeader, lblDesc, txtInstallPath, btnBrowse, lblInfo });
    }

    private void BuildStep2()
    {
        var lblHeader = new Label
        {
            Text = "Stream Server & Multi-Channel Setup",
            Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
            Location = new Point(15, 8),
            AutoSize = true
        };

        // Server Host & Port
        var lblHost = new Label { Text = "Host/IP:", Location = new Point(15, 38), AutoSize = true };
        if (txtServerHost == null)
            txtServerHost = new TextBox { Text = string.Empty, PlaceholderText = "e.g. 192.168.1.100 or stream.myradio.com", Location = new Point(70, 35), Size = new Size(130, 23) };

        var lblPort = new Label { Text = "Port:", Location = new Point(210, 38), AutoSize = true };
        if (numServerPort == null)
            numServerPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 8000, Location = new Point(245, 35), Size = new Size(65, 23) };

        // Admin Password
        var lblAdminPass = new Label { Text = "Admin Key:", Location = new Point(320, 38), AutoSize = true };
        if (txtMasterAdminPassword == null)
            txtMasterAdminPassword = new TextBox { Text = string.Empty, PlaceholderText = "Panel Password", PasswordChar = '•', Location = new Point(390, 35), Size = new Size(75, 23) };

        // Default Stream Password
        var lblStreamPass = new Label { Text = "Stream Pass:", Location = new Point(15, 68), AutoSize = true };
        if (txtDefaultStreamPassword == null)
            txtDefaultStreamPassword = new TextBox { Text = string.Empty, PlaceholderText = "Server Password", PasswordChar = '•', Location = new Point(95, 65), Size = new Size(105, 23) };

        var lblMountsTitle = new Label
        {
            Text = "Mountpoints / Channels (Add at least 1):",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Location = new Point(15, 96),
            AutoSize = true
        };

        btnAddMount = new Button { Text = "+ Add Mount", Location = new Point(290, 93), Size = new Size(85, 24) };
        btnRemoveMount = new Button { Text = "✕ Remove", Location = new Point(380, 93), Size = new Size(85, 24) };

        if (gridMounts == null)
        {
            gridMounts = new DataGridView
            {
                Location = new Point(15, 122),
                Size = new Size(450, 190),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            gridMounts.Columns.Add("colName", "Channel Label");
            gridMounts.Columns.Add("colMount", "Mount Path");
            gridMounts.Columns.Add("colPass", "Custom Password (Optional)");

            gridMounts.Columns[0].Width = 160;
            gridMounts.Columns[1].Width = 110;
            gridMounts.Columns[2].Width = 175;

            // Start completely empty as requested - user must explicitly add mounts!
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

        var lblTip = new Label
        {
            Text = "🔒 Passwords will be DPAPI encrypted & admin keys securely hashed in config.",
            Font = new Font("Segoe UI", 8F, FontStyle.Italic),
            ForeColor = Color.DarkSlateGray,
            Location = new Point(15, 318),
            Size = new Size(450, 20)
        };

        pnlContainer.Controls.AddRange(new Control[]
        {
            lblHeader,
            lblHost, txtServerHost, lblPort, numServerPort,
            lblAdminPass, txtMasterAdminPassword,
            lblStreamPass, txtDefaultStreamPassword,
            lblMountsTitle, btnAddMount, btnRemoveMount,
            gridMounts,
            lblTip
        });
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
            Size = new Size(440, 25)
        };

        if (chkDesktopShortcut == null)
            chkDesktopShortcut = new CheckBox { Text = "Create a Desktop Shortcut", Checked = true, Location = new Point(25, 90), AutoSize = true };

        if (chkStartMenuShortcut == null)
            chkStartMenuShortcut = new CheckBox { Text = "Create a Start Menu Shortcut", Checked = true, Location = new Point(25, 125), AutoSize = true };

        if (chkAutoStart == null)
            chkAutoStart = new CheckBox { Text = "Start Stream Monitor automatically on Windows boot (minimized to tray)", Checked = false, Location = new Point(25, 160), AutoSize = true };

        if (chkLaunchAfter == null)
            chkLaunchAfter = new CheckBox { Text = "Launch Icecast Stream Monitor immediately after setup", Checked = true, Location = new Point(25, 195), AutoSize = true };

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
            Size = new Size(440, 25)
        };

        progressBar = new ProgressBar
        {
            Location = new Point(20, 85),
            Size = new Size(440, 23),
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

        lblInstallStatus.Text = "Deploying application binary files...";
        progressBar.Value = 25;
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

        progressBar.Value = 50;
        lblInstallStatus.Text = "Encrypting passwords and writing initial multi-channel configuration...";
        await Task.Delay(200);

        // Write preconfigured multi-channel settings with encrypted passwords
        WriteInitialSettings(targetDir);

        progressBar.Value = 75;
        lblInstallStatus.Text = "Creating shortcuts and registry entries...";
        await Task.Delay(200);

        var targetExe = Path.Combine(targetDir, "IcecastStreamMonitor.exe");
        var remoteExe = Path.Combine(targetDir, "IcecastStreamMonitor.RemoteControl.exe");

        // Desktop Shortcuts
        if (chkDesktopShortcut.Checked)
        {
            CreateShortcut(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Icecast Stream Monitor.lnk"),
                targetExe,
                targetDir,
                "Icecast & SHOUTcast Live Stream Monitor");

            if (File.Exists(remoteExe))
            {
                CreateShortcut(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Icecast Stream Monitor Remote.lnk"),
                    remoteExe,
                    targetDir,
                    "Icecast Stream Monitor - Remote Client");
            }
        }

        // Start Menu Shortcuts
        if (chkStartMenuShortcut.Checked)
        {
            var startMenuProg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "Icecast Stream Monitor");
            Directory.CreateDirectory(startMenuProg);
            CreateShortcut(
                Path.Combine(startMenuProg, "Icecast Stream Monitor.lnk"),
                targetExe,
                targetDir,
                "Icecast & SHOUTcast Live Stream Monitor");

            if (File.Exists(remoteExe))
            {
                CreateShortcut(
                    Path.Combine(startMenuProg, "Icecast Stream Monitor Remote.lnk"),
                    remoteExe,
                    targetDir,
                    "Icecast Stream Monitor - Remote Client");
            }
        }

        // Windows Startup
        if (chkAutoStart.Checked)
        {
            try
            {
                using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                key?.SetValue("IcecastStreamMonitor", $"\"{targetExe}\"");
            }
            catch { }
        }

        progressBar.Value = 100;
        lblInstallStatus.Text = "✓ Installation Completed Successfully!";
        lblInstallStatus.ForeColor = Color.ForestGreen;

        btnNext.Text = "Finish";
        btnNext.Enabled = true;
        btnCancel.Enabled = false;
    }

    private void WriteInitialSettings(string targetDir)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appData, "IcecastStreamMonitor");
        Directory.CreateDirectory(appFolder);
        var configPath = Path.Combine(appFolder, "config.json");

        var serverHost = string.IsNullOrWhiteSpace(txtServerHost.Text) ? "127.0.0.1" : txtServerHost.Text.Trim();
        var adminPass = string.IsNullOrWhiteSpace(txtMasterAdminPassword.Text) ? "admin" : txtMasterAdminPassword.Text.Trim();
        var defaultStreamPass = string.IsNullOrWhiteSpace(txtDefaultStreamPassword.Text) ? "hackme" : txtDefaultStreamPassword.Text.Trim();

        var channelList = new List<object>();
        foreach (DataGridViewRow row in gridMounts.Rows)
        {
            var chName = row.Cells[0].Value?.ToString() ?? "Live Stream";
            var mount = row.Cells[1].Value?.ToString() ?? "/live";
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

        var settings = new
        {
            DefaultPassword = SecurityHelper.EncryptSecret(defaultStreamPass),
            MasterAdminPassword = SecurityHelper.EncryptSecret(adminPass),
            MasterAdminPasswordHash = SecurityHelper.HashPassword(adminPass),
            PollingIntervalSeconds = 5,
            RequestTimeoutSeconds = 4,
            AutoRefreshEnabled = true,
            MinimizeToTray = true,
            CloseToTray = false,
            NotifyOnStreamOffline = true,
            Channels = channelList,
            WindowWidth = 1040,
            WindowHeight = 630,
            WindowLeft = -1,
            WindowTop = -1,
            IsMaximized = false
        };

        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
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
            ShowStep(2);
        }
        else if (_currentStep == 2)
        {
            if (string.IsNullOrWhiteSpace(txtServerHost.Text))
            {
                MessageBox.Show("Please enter a server host or IP address (e.g. 127.0.0.1 or stream.myradio.com).", "Host Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServerHost.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtMasterAdminPassword.Text))
            {
                MessageBox.Show("Please define a Master Admin Password for securing the monitor dashboard.", "Admin Key Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMasterAdminPassword.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDefaultStreamPassword.Text))
            {
                MessageBox.Show("Please enter the server stream authentication password.", "Stream Password Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDefaultStreamPassword.Focus();
                return;
            }
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
                    MessageBox.Show("Mount path cannot be empty for any configured channel (e.g. /live).", "Invalid Mount Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
