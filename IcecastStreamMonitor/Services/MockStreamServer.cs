using System.Net;
using System.Text;

namespace IcecastStreamMonitor.Services;

public class MockStreamServer : IDisposable
{
    private HttpListener? _listener;
    private CancellationTokenSource? _cts;
    private Task? _serverTask;
    private readonly int _port;
    private readonly Random _random = new();

    public int Port => _port;
    public bool IsRunning => _listener?.IsListening ?? false;

    public MockStreamServer(int port = 8000)
    {
        _port = port;
    }

    public void Start()
    {
        if (IsRunning) return;

        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://127.0.0.1:{_port}/");
        _listener.Prefixes.Add($"http://localhost:{_port}/");

        try
        {
            _listener.Start();
            _cts = new CancellationTokenSource();
            _serverTask = Task.Run(() => ListenLoop(_cts.Token));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Mock server failed to start on port {_port}: {ex.Message}");
            _listener.Close();
            _listener = null;
            throw;
        }
    }

    private async Task ListenLoop(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested && _listener != null && _listener.IsListening)
        {
            try
            {
                var context = await _listener.GetContextAsync();
                _ = Task.Run(() => HandleRequest(context));
            }
            catch (HttpListenerException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Mock server error: {ex.Message}");
            }
        }
    }

    private void HandleRequest(HttpListenerContext context)
    {
        try
        {
            var req = context.Request;
            var resp = context.Response;
            var path = req.Url?.AbsolutePath.ToLowerInvariant() ?? "/";
            var query = req.Url?.Query.ToLowerInvariant() ?? "";

            // 1. Icecast status-json.xsl
            if (path == "/status-json.xsl")
            {
                int liveListeners = 140 + _random.Next(-5, 8);
                int mobileListeners = 85 + _random.Next(-3, 4);
                int studioListeners = 12 + _random.Next(-1, 2);

                var json = $$"""
                {
                  "icestats": {
                    "admin": "admin@radiocenter.net",
                    "host": "127.0.0.1",
                    "location": "Earth",
                    "server_id": "Icecast 2.4.4",
                    "server_start": "Sun, 01 Jan 2026 00:00:00 +0000",
                    "source": [
                      {
                        "audio_info": "channels=2;samplerate=44100;bitrate=320",
                        "bitrate": 320,
                        "genre": "Rock / Alternative",
                        "listener_peak": 250,
                        "listeners": {{liveListeners}},
                        "listenurl": "http://127.0.0.1:{{_port}}/live",
                        "server_description": "Main High-Quality Studio Live Broadcast",
                        "server_name": "Antigravity FM - Live Main",
                        "server_type": "audio/mpeg",
                        "title": "Queen - Bohemian Rhapsody"
                      },
                      {
                        "audio_info": "channels=2;samplerate=44100;bitrate=64",
                        "bitrate": 64,
                        "genre": "Pop / Hits",
                        "listener_peak": 120,
                        "listeners": {{mobileListeners}},
                        "listenurl": "http://127.0.0.1:{{_port}}/mobile.aac",
                        "server_description": "Low Bandwidth Mobile Stream",
                        "server_name": "Antigravity FM - Mobile",
                        "server_type": "audio/aacp",
                        "title": "Daft Punk - Get Lucky"
                      },
                      {
                        "audio_info": "channels=2;samplerate=96000;bitrate=1411",
                        "bitrate": 1411,
                        "genre": "Audiophile FLAC",
                        "listener_peak": 30,
                        "listeners": {{studioListeners}},
                        "listenurl": "http://127.0.0.1:{{_port}}/studio.flac",
                        "server_description": "Master Lossless Feed",
                        "server_name": "Antigravity FM - Studio Master",
                        "server_type": "audio/flac",
                        "title": "Dire Straits - Sultans of Swing"
                      }
                    ]
                  }
                }
                """;

                var bytes = Encoding.UTF8.GetBytes(json);
                resp.ContentType = "application/json; charset=utf-8";
                resp.ContentLength64 = bytes.Length;
                resp.OutputStream.Write(bytes, 0, bytes.Length);
                resp.OutputStream.Close();
                return;
            }

            // 1.5. Icecast admin listclients endpoint
            if (path == "/admin/listclients" || path == "/admin/listclients.xsl")
            {
                var mountQuery = req.QueryString["mount"] ?? "/live";
                var xml = $$"""
                <icestats>
                  <source mount="{{mountQuery}}">
                    <Listeners>5</Listeners>
                    <listener>
                      <IP>192.168.1.105</IP>
                      <UserAgent>VLC/3.0.18 LibVLC/3.0.18</UserAgent>
                      <Connected>2450</Connected>
                      <ID>1001</ID>
                    </listener>
                    <listener>
                      <IP>88.241.15.22</IP>
                      <UserAgent>Mozilla/5.0 (Windows NT 10.0; Win64; x64) Chrome/122.0.0.0</UserAgent>
                      <Connected>1340</Connected>
                      <ID>1002</ID>
                    </listener>
                    <listener>
                      <IP>178.233.19.84</IP>
                      <UserAgent>WinampMPEG/5.66</UserAgent>
                      <Connected>890</Connected>
                      <ID>1003</ID>
                    </listener>
                    <listener>
                      <IP>212.156.40.11</IP>
                      <UserAgent>Foobar2000/1.6.11</UserAgent>
                      <Connected>420</Connected>
                      <ID>1004</ID>
                    </listener>
                    <listener>
                      <IP>95.70.182.33</IP>
                      <UserAgent>AIMP/5.11</UserAgent>
                      <Connected>150</Connected>
                      <ID>1005</ID>
                    </listener>
                  </source>
                </icestats>
                """;

                var bytes = Encoding.UTF8.GetBytes(xml);
                resp.ContentType = "application/xml; charset=utf-8";
                resp.ContentLength64 = bytes.Length;
                resp.OutputStream.Write(bytes, 0, bytes.Length);
                resp.OutputStream.Close();
                return;
            }

            // 2. Icecast admin stats XML
            if (path == "/admin/stats.xml" || path == "/admin/stats")
            {
                var xml = $$"""
                <icestats>
                  <source mount="/live">
                    <listeners>{{140 + _random.Next(1, 10)}}</listeners>
                    <listener_peak>250</listener_peak>
                    <max_listeners>500</max_listeners>
                    <bitrate>320</bitrate>
                    <server_type>audio/mpeg</server_type>
                    <audio_info>channels=2;samplerate=44100;bitrate=320</audio_info>
                    <title>Pink Floyd - Time</title>
                    <server_name>Antigravity FM - Live Main</server_name>
                  </source>
                  <source mount="/mobile.aac">
                    <listeners>{{80 + _random.Next(1, 10)}}</listeners>
                    <listener_peak>120</listener_peak>
                    <max_listeners>300</max_listeners>
                    <bitrate>64</bitrate>
                    <server_type>audio/aacp</server_type>
                    <audio_info>channels=2;samplerate=44100;bitrate=64</audio_info>
                    <title>Pink Floyd - Time</title>
                    <server_name>Antigravity FM - Mobile</server_name>
                  </source>
                </icestats>
                """;

                var bytes = Encoding.UTF8.GetBytes(xml);
                resp.ContentType = "application/xml; charset=utf-8";
                resp.ContentLength64 = bytes.Length;
                resp.OutputStream.Write(bytes, 0, bytes.Length);
                resp.OutputStream.Close();
                return;
            }

            // 3. SHOUTcast v2 stats
            if (path == "/stats" && query.Contains("json=1"))
            {
                var shoutJson = $$"""
                {
                  "currentlisteners": {{64 + _random.Next(-2, 3)}},
                  "peaklisteners": 110,
                  "maxlisteners": 250,
                  "uniquelisteners": 60,
                  "bitrate": 192,
                  "samprate": 44100,
                  "streamstatus": 1,
                  "songtitle": "The Police - Every Breath You Take",
                  "servertitle": "Antigravity SHOUTcast Stream",
                  "content": "audio/mpeg"
                }
                """;

                var bytes = Encoding.UTF8.GetBytes(shoutJson);
                resp.ContentType = "application/json; charset=utf-8";
                resp.ContentLength64 = bytes.Length;
                resp.OutputStream.Write(bytes, 0, bytes.Length);
                resp.OutputStream.Close();
                return;
            }

            // 4. SHOUTcast v1 /7.html
            if (path == "/7.html")
            {
                var html = $"<html><body>{55 + _random.Next(-2, 3)},1,95,200,52,128,David Bowie - Heroes</body></html>";
                var bytes = Encoding.UTF8.GetBytes(html);
                resp.ContentType = "text/html; charset=utf-8";
                resp.ContentLength64 = bytes.Length;
                resp.OutputStream.Write(bytes, 0, bytes.Length);
                resp.OutputStream.Close();
                return;
            }

            // 5. Direct Stream Mounts
            if (path == "/live")
            {
                resp.Headers.Add("icy-br", "320");
                resp.Headers.Add("icy-name", "Antigravity FM - Live Main");
                resp.Headers.Add("icy-genre", "Rock");
                resp.ContentType = "audio/mpeg";
                resp.StatusCode = 200;
                var dummyChunk = new byte[512];
                resp.OutputStream.Write(dummyChunk, 0, dummyChunk.Length);
                resp.OutputStream.Close();
                return;
            }

            if (path == "/mobile.aac")
            {
                resp.Headers.Add("icy-br", "64");
                resp.Headers.Add("icy-name", "Antigravity FM - Mobile");
                resp.Headers.Add("icy-genre", "Pop");
                resp.ContentType = "audio/aacp";
                resp.StatusCode = 200;
                var dummyChunk = new byte[512];
                resp.OutputStream.Write(dummyChunk, 0, dummyChunk.Length);
                resp.OutputStream.Close();
                return;
            }

            // Unknown mount
            resp.StatusCode = 404;
            var notFound = Encoding.UTF8.GetBytes("Mountpoint Not Found");
            resp.OutputStream.Write(notFound, 0, notFound.Length);
            resp.OutputStream.Close();
        }
        catch
        {
            // Ignore socket disconnects during testing
        }
    }

    public void Stop()
    {
        try
        {
            _cts?.Cancel();
            _listener?.Stop();
            _listener?.Close();
            _listener = null;
        }
        catch { }
    }

    public void Dispose()
    {
        Stop();
        _cts?.Dispose();
    }
}
