using IcecastStreamMonitor.Models;
using IcecastStreamMonitor.Services;
using Xunit;

namespace IcecastStreamMonitor.Tests;

public class ParserAndConfigTests
{
    [Theory]
    [InlineData("audio/mpeg", null, "MP3")]
    [InlineData("audio/mp3", null, "MP3")]
    [InlineData("audio/aac", null, "AAC")]
    [InlineData("audio/aacp", null, "AAC+")]
    [InlineData("audio/ogg", null, "OGG Vorbis")]
    [InlineData("application/ogg", null, "OGG Vorbis")]
    [InlineData("audio/opus", null, "Opus")]
    [InlineData("audio/flac", null, "FLAC")]
    [InlineData("audio/mpeg", "channels=2;samplerate=48000;bitrate=320", "MP3")]
    [InlineData("application/ogg", "channels=2;samplerate=48000;bitrate=160;opus=1", "Opus")]
    public void NormalizeAudioFormat_ReturnsExpectedFormat(string? mime, string? audioInfo, string expected)
    {
        var result = AudioFormatHelper.NormalizeAudioFormat(mime, audioInfo);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseAudioInfo_ExtractsBitrateSamplerateAndChannels()
    {
        var audioInfo = "channels=2;samplerate=44100;bitrate=320;quality=0.0";
        var (bitrate, samplerate, channels) = AudioFormatHelper.ParseAudioInfo(audioInfo);

        Assert.Equal(320, bitrate);
        Assert.Equal(44100, samplerate);
        Assert.Equal(2, channels);
    }

    [Theory]
    [InlineData("128", 128)]
    [InlineData("320 kbps", 320)]
    [InlineData("128000", 128)]
    [InlineData("192, 192", 192)]
    [InlineData("", 0)]
    public void ParseBitrateString_ExtractsBitrateCorrectly(string raw, int expected)
    {
        var result = AudioFormatHelper.ParseBitrateString(raw);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseIcecastJson_MultipleSources_ExtractsCorrectMount()
    {
        var channelId = Guid.NewGuid();
        var json = """
        {
          "icestats": {
            "source": [
              {
                "audio_info": "channels=2;samplerate=44100;bitrate=320",
                "bitrate": 320,
                "listeners": 87,
                "listener_peak": 150,
                "listenurl": "http://127.0.0.1:8000/rock.mp3",
                "server_type": "audio/mpeg",
                "title": "Led Zeppelin - Stairway to Heaven"
              },
              {
                "audio_info": "channels=2;samplerate=44100;bitrate=64",
                "bitrate": 64,
                "listeners": 34,
                "listener_peak": 50,
                "listenurl": "http://127.0.0.1:8000/jazz.aac",
                "server_type": "audio/aacp",
                "title": "Miles Davis - So What"
              }
            ]
          }
        }
        """;

        var status = StreamMonitorService.ParseIcecastJson(json, "/rock.mp3", channelId, 15);
        Assert.NotNull(status);
        Assert.True(status.IsOnline);
        Assert.Equal(87, status.ConcurrentListeners);
        Assert.Equal(150, status.PeakListeners);
        Assert.Equal(320, status.BitRateKbps);
        Assert.Equal("MP3", status.AudioFormat);
        Assert.Equal("Led Zeppelin - Stairway to Heaven", status.StreamTitle);

        var jazzStatus = StreamMonitorService.ParseIcecastJson(json, "/jazz.aac", channelId, 20);
        Assert.NotNull(jazzStatus);
        Assert.True(jazzStatus.IsOnline);
        Assert.Equal(34, jazzStatus.ConcurrentListeners);
        Assert.Equal("AAC+", jazzStatus.AudioFormat);
        Assert.Equal(64, jazzStatus.BitRateKbps);
    }

    [Fact]
    public void ParseIcecastAdminXml_ExtractsSourceDetails()
    {
        var channelId = Guid.NewGuid();
        var xml = """
        <icestats>
          <source mount="/live">
            <listeners>45</listeners>
            <listener_peak>99</listener_peak>
            <max_listeners>200</max_listeners>
            <bitrate>256</bitrate>
            <server_type>audio/mpeg</server_type>
            <audio_info>channels=2;samplerate=44100;bitrate=256</audio_info>
            <title>AC/DC - Back in Black</title>
          </source>
        </icestats>
        """;

        var status = StreamMonitorService.ParseIcecastAdminXml(xml, "/live", channelId, 10);
        Assert.NotNull(status);
        Assert.True(status.IsOnline);
        Assert.Equal(45, status.ConcurrentListeners);
        Assert.Equal(99, status.PeakListeners);
        Assert.Equal(256, status.BitRateKbps);
        Assert.Equal("MP3", status.AudioFormat);
        Assert.Equal("AC/DC - Back in Black", status.StreamTitle);
    }

    [Fact]
    public void ParseShoutcastV2Json_ExtractsMetrics()
    {
        var channelId = Guid.NewGuid();
        var json = """
        {
          "currentlisteners": 58,
          "peaklisteners": 120,
          "maxlisteners": 300,
          "bitrate": 192,
          "samprate": 44100,
          "streamstatus": 1,
          "songtitle": "Queen - Radio Ga Ga",
          "content": "audio/mpeg"
        }
        """;

        var status = StreamMonitorService.ParseShoutcastV2Json(json, channelId, 12);
        Assert.NotNull(status);
        Assert.True(status.IsOnline);
        Assert.Equal(58, status.ConcurrentListeners);
        Assert.Equal(120, status.PeakListeners);
        Assert.Equal(192, status.BitRateKbps);
        Assert.Equal("MP3", status.AudioFormat);
        Assert.Equal("Queen - Radio Ga Ga", status.StreamTitle);
    }

    [Fact]
    public void ParseShoutcastV1Html_ExtractsCsvData()
    {
        var channelId = Guid.NewGuid();
        var html = "<html><body>75,1,140,500,70,128,The Beatles - Let It Be</body></html>";

        var status = StreamMonitorService.ParseShoutcastV1Html(html, channelId, 8);
        Assert.NotNull(status);
        Assert.True(status.IsOnline);
        Assert.Equal(75, status.ConcurrentListeners);
        Assert.Equal(140, status.PeakListeners);
        Assert.Equal(128, status.BitRateKbps);
        Assert.Equal("The Beatles - Let It Be", status.StreamTitle);
    }

    [Fact]
    public void ConfigStorage_SaveAndLoad_MaintainsState()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"stream_monitor_test_{Guid.NewGuid():N}.json");
        try
        {
            var configService = new ConfigStorageService(tempFile);
            var initialSettings = new AppSettings
            {
                DefaultPassword = "super_secret_test_password",
                PollingIntervalSeconds = 12,
                RequestTimeoutSeconds = 8,
                Channels = new List<StreamChannel>
                {
                    new()
                    {
                        Name = "Test Rock Channel",
                        Host = "stream.rock.net",
                        Port = 8000,
                        MountPath = "/rock",
                        ServerType = ServerType.Icecast,
                        UseDefaultPassword = false,
                        CustomPassword = "custom_channel_pass"
                    }
                }
            };

            configService.SaveSettings(initialSettings);
            Assert.True(File.Exists(tempFile));

            var loaded = configService.LoadSettings();
            Assert.Equal("super_secret_test_password", loaded.DefaultPassword);
            Assert.Equal(12, loaded.PollingIntervalSeconds);
            Assert.Equal(8, loaded.RequestTimeoutSeconds);
            Assert.Single(loaded.Channels);

            var ch = loaded.Channels[0];
            Assert.Equal("Test Rock Channel", ch.Name);
            Assert.Equal("stream.rock.net", ch.Host);
            Assert.Equal(8000, ch.Port);
            Assert.Equal("/rock", ch.MountPath);
            Assert.False(ch.UseDefaultPassword);
            Assert.Equal("custom_channel_pass", ch.CustomPassword);
            Assert.Equal("http://stream.rock.net:8000/rock", ch.FormattedUrl);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void ParseIcecastListenersXml_ExtractsClientIpsAndUserAgents()
    {
        var xml = """
        <icestats>
          <source mount="/live">
            <Listeners>2</Listeners>
            <listener>
              <IP>192.168.1.55</IP>
              <UserAgent>VLC/3.0.18 LibVLC/3.0.18</UserAgent>
              <Connected>1250</Connected>
              <ID>101</ID>
            </listener>
            <listener>
              <IP>88.241.10.5</IP>
              <UserAgent>Mozilla/5.0 Chrome/120.0</UserAgent>
              <Connected>360</Connected>
              <ID>102</ID>
            </listener>
          </source>
        </icestats>
        """;

        var list = StreamMonitorService.ParseIcecastListenersXml(xml);
        Assert.Equal(2, list.Count);

        Assert.Equal("192.168.1.55", list[0].IpAddress);
        Assert.Equal("VLC/3.0.18 LibVLC/3.0.18", list[0].UserAgent);
        Assert.Equal("20:50", list[0].FormattedConnectedTime);
        Assert.Equal("Local / LAN", list[0].Location);

        Assert.Equal("88.241.10.5", list[1].IpAddress);
        Assert.Equal("Mozilla/5.0 Chrome/120.0", list[1].UserAgent);
        Assert.Equal("06:00", list[1].FormattedConnectedTime);
        Assert.Equal("Remote Client", list[1].Location);
    }

    [Fact]
    public void ParseShoutcastListenersXml_ExtractsClientIps()
    {
        var xml = """
        <SHOUTCASTSERVER>
          <LISTENERS>
            <LISTENER>
              <HOSTNAME>178.233.19.84</HOSTNAME>
              <USERAGENT>WinampMPEG/5.66</USERAGENT>
              <CONNECTTIME>640</CONNECTTIME>
              <UID>201</UID>
            </LISTENER>
          </LISTENERS>
        </SHOUTCASTSERVER>
        """;

        var list = StreamMonitorService.ParseShoutcastListenersXml(xml);
        Assert.Single(list);
        Assert.Equal("178.233.19.84", list[0].IpAddress);
        Assert.Equal("WinampMPEG/5.66", list[0].UserAgent);
        Assert.Equal("10:40", list[0].FormattedConnectedTime);
    }

    [Fact]
    public void StreamHistoryService_Computes24HourMetrics()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"history_test_{Guid.NewGuid():N}.json");
        try
        {
            var historyService = new StreamHistoryService(tempFile);
            var channelId = Guid.NewGuid();

            var status = new StreamStatus
            {
                IsOnline = true,
                ConcurrentListeners = 150,
                BitRateKbps = 320,
                LatencyMs = 25
            };

            historyService.RecordSnapshot(channelId, status, 5, "Rock Stream");

            var history = historyService.Get24HourHistory(channelId);
            Assert.NotEmpty(history);

            var metrics = historyService.GetMetricsSummary(channelId);
            Assert.True(metrics.PeakListeners >= 100);
            Assert.True(metrics.AverageListeners > 0);
            Assert.True(metrics.TotalMegaBytesTransferred > 0);
            Assert.True(metrics.UptimePercentage > 90);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public void AuditLogService_RecordsAndFiltersEvents()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"audit_test_{Guid.NewGuid():N}.json");
        try
        {
            var auditService = new AuditLogService(tempFile);
            var ch1 = new StreamChannel { Name = "Main Feed", MountPath = "/live", Port = 8000, Host = "127.0.0.1", UseDefaultPassword = true };
            var ch2 = new StreamChannel { Name = "Main Feed", MountPath = "/studio.mp3", Port = 8002, Host = "127.0.0.1", UseDefaultPassword = false, CustomPassword = "secret" };

            auditService.LogChannelAdded(ch1);
            auditService.LogChannelModified(ch1, ch2);
            auditService.LogStreamAlert("Main Feed", "/studio.mp3", "Stream timeout");

            var logs = auditService.GetEntries();
            Assert.True(logs.Count >= 3);

            var configChange = logs.FirstOrDefault(l => l.Action == "Configuration Modified");
            Assert.NotNull(configChange);
            Assert.Contains("/studio.mp3", configChange.Details);
            Assert.Contains("8002", configChange.Details);
            Assert.Contains("Custom Password", configChange.Details);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public void SecurityHelper_EncryptAndDecrypt_MaintainsClearText()
    {
        var rawPassword = "super_secure_radio_stream_password_123!@#";
        var encrypted = SecurityHelper.EncryptSecret(rawPassword);

        Assert.NotNull(encrypted);
        Assert.StartsWith("ENC:", encrypted);
        Assert.NotEqual(rawPassword, encrypted);

        var decrypted = SecurityHelper.DecryptSecret(encrypted);
        Assert.Equal(rawPassword, decrypted);
    }

    [Fact]
    public void SecurityHelper_HashPassword_VerifiesCorrectly()
    {
        var adminPassword = "MyMasterAdminPassword2026!";
        var hash = SecurityHelper.HashPassword(adminPassword);

        Assert.NotNull(hash);
        Assert.StartsWith("PBKDF2:", hash);

        Assert.True(SecurityHelper.VerifyPassword(adminPassword, hash));
        Assert.False(SecurityHelper.VerifyPassword("wrong_password", hash));
    }

    [Fact]
    public void ConfigStorageService_ManagesAndPersistsRemoteUsers()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"remote_user_test_{Guid.NewGuid():N}.json");
        try
        {
            var configService = new ConfigStorageService(tempFile);
            var settings = configService.LoadSettings();

            Assert.NotNull(settings.RemoteUsers);
            Assert.True(settings.RemoteUsers.Count >= 2);

            var admin = settings.RemoteUsers.FirstOrDefault(u => u.RoleName == "SuperAdmin");
            var viewer = settings.RemoteUsers.FirstOrDefault(u => u.RoleName == "Standard Viewer");

            Assert.NotNull(admin);
            Assert.True(admin.Permissions.CanManageChannels);
            Assert.True(admin.Permissions.CanViewListenerIps);

            Assert.NotNull(viewer);
            Assert.False(viewer.Permissions.CanManageChannels);
            Assert.False(viewer.Permissions.CanViewListenerIps); // Restricted

            // Add custom operator
            settings.RemoteUsers.Add(new RemoteUser
            {
                Username = "operator1",
                PlainPassword = "op_secret_password",
                RoleName = "Operator",
                Permissions = new RemotePermissions
                {
                    CanViewStatus = true,
                    CanViewLogs = true,
                    CanViewListenerIps = true,
                    CanRefreshStreams = true,
                    CanManageChannels = false
                }
            });

            configService.SaveSettings(settings);

            // Reload and verify
            var reloaded = configService.LoadSettings();
            var opUser = reloaded.RemoteUsers.FirstOrDefault(u => u.Username == "operator1");
            Assert.NotNull(opUser);
            Assert.Equal("op_secret_password", opUser.PlainPassword);
            Assert.True(SecurityHelper.VerifyPassword("op_secret_password", opUser.PasswordHash));
            Assert.True(opUser.Permissions.CanViewListenerIps);
            Assert.False(opUser.Permissions.CanManageChannels);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }
}
