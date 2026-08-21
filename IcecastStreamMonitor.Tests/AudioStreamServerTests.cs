using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using IcecastStreamMonitor.Models;
using IcecastStreamMonitor.Services;
using IcecastStreamMonitor.Services.Streaming;
using Xunit;

namespace IcecastStreamMonitor.Tests;

public class AudioStreamServerTests
{
    [Fact]
    public void IcyMetadataInjector_CreatesCompliantFrame_Success()
    {
        var frame = IcyMetadataInjector.CreateMetadataFrame("RadioTEDU - Great Music", "https://stream.radiotedu.com");
        Assert.NotNull(frame);
        Assert.True(frame.Length > 1);

        int lengthByte = frame[0];
        Assert.Equal((frame.Length - 1) / 16, lengthByte);

        var metaBody = Encoding.UTF8.GetString(frame, 1, frame.Length - 1);
        Assert.Contains("StreamTitle='RadioTEDU - Great Music';", metaBody);
        Assert.Contains("StreamUrl='https://stream.radiotedu.com';", metaBody);
    }

    [Fact]
    public async Task AudioStreamServer_FullIngestBroadcastAndStatsLifecycle_Success()
    {
        int serverPort = 12154;
        var server = new AudioStreamServer();
        server.Start(serverPort);

        try
        {
            // 1. Verify initial status-json.xsl when no sources are streaming
            using var httpClient = new HttpClient();
            var jsonInitial = await httpClient.GetStringAsync($"http://127.0.0.1:{serverPort}/status-json.xsl");
            Assert.Contains("RTSAS", jsonInitial);

            // 2. Connect Simulated DJ Source (SOURCE /live HTTP/1.0)
            using var djClient = new TcpClient("127.0.0.1", serverPort);
            using var djStream = djClient.GetStream();

            var sourceHandshake =
                "SOURCE /live HTTP/1.0\r\n" +
                "ice-password: hackme\r\n" +
                "ice-name: RadioTEDU Live Studio\r\n" +
                "ice-genre: Electro Chill\r\n" +
                "ice-bitrate: 320\r\n" +
                "Content-Type: audio/mpeg\r\n" +
                "\r\n";

            var sourceBytes = Encoding.Latin1.GetBytes(sourceHandshake);
            await djStream.WriteAsync(sourceBytes, 0, sourceBytes.Length);

            var djReader = new StreamReader(djStream, Encoding.Latin1);
            var djResp = await djReader.ReadLineAsync();
            Assert.Equal("HTTP/1.0 200 OK", djResp);

            // Verify mount is online
            var mount = server.GetMount("/live");
            Assert.NotNull(mount);
            Assert.True(mount.IsOnline);
            Assert.Equal(320, mount.BitRateKbps);
            Assert.Equal("RadioTEDU Live Studio", mount.StreamName);

            // 3. DJ Streams audio bytes
            var testAudioChunk = new byte[4096];
            Array.Fill<byte>(testAudioChunk, 0xAA);
            await djStream.WriteAsync(testAudioChunk, 0, testAudioChunk.Length);
            await Task.Delay(100);

            // 4. Connect Simulated Listener (GET /live with ICY-MetaData)
            using var listenerClient = new TcpClient("127.0.0.1", serverPort);
            using var listenerStream = listenerClient.GetStream();

            var listenerReq =
                "GET /live HTTP/1.1\r\n" +
                "Host: 127.0.0.1\r\n" +
                "User-Agent: VLC/3.0.18 LibVLC\r\n" +
                "Icy-MetaData: 1\r\n" +
                "X-Forwarded-For: 203.0.113.88\r\n" +
                "\r\n";

            var listenerBytes = Encoding.Latin1.GetBytes(listenerReq);
            await listenerStream.WriteAsync(listenerBytes, 0, listenerBytes.Length);

            var listenerReader = new StreamReader(listenerStream, Encoding.Latin1);
            var listenerResp = await listenerReader.ReadLineAsync();
            Assert.Equal("HTTP/1.0 200 OK", listenerResp);

            await Task.Delay(100);

            // 5. Verify Listener is registered and real IP extracted
            var listeners = mount.GetConnectedListeners();
            Assert.Single(listeners);
            Assert.Equal("203.0.113.88", listeners[0].IpAddress);
            Assert.Contains("VLC", listeners[0].UserAgent);

            // 6. Test in-memory monitor bridge
            using var monitor = new StreamMonitorService { LocalServer = server };
            var ch = new StreamChannel
            {
                Host = "127.0.0.1",
                Port = serverPort,
                MountPath = "/live",
                ServerType = ServerType.Icecast,
                UseDefaultPassword = true
            };

            var status = await monitor.CheckStreamAsync(ch, "hackme", TimeSpan.FromSeconds(2));
            Assert.True(status.IsOnline);
            Assert.Equal(1, status.ConcurrentListeners);
            Assert.Equal(320, status.BitRateKbps);

            // 7. Update dynamic title from DJ
            await httpClient.GetStringAsync($"http://127.0.0.1:{serverPort}/admin/metadata?mount=/live&mode=updinfo&song=Daft%20Punk%20-%20One%20More%20Time");
            Assert.Equal("Daft Punk - One More Time", mount.StreamTitle);

            // 8. Disconnect DJ and verify mount drops to offline
            djClient.Close();
            await Task.Delay(100);
            Assert.False(mount.IsOnline);
        }
        finally
        {
            server.Stop();
        }
    }
}
