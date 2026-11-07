using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using IcecastStreamMonitor.Models;

namespace IcecastStreamMonitor.Services.Streaming;

internal readonly record struct AudioPacket(byte[] Data, int Offset, int Length);
internal sealed record SourceLease(long Id, NetworkStream Stream);

public class StreamingClient
{
    public long Id { get; set; }
    public string IpAddress { get; set; } = "127.0.0.1";
    public string UserAgent { get; set; } = "Generic Audio Player";
    public DateTime ConnectedSince { get; set; } = DateTime.Now;
    public long BytesSent { get; set; }
    public bool SupportsIcyMetadata { get; set; }
    public int BytesUntilNextMetadata { get; set; } = IcyMetadataInjector.DefaultMetaInterval;
    public NetworkStream? NetworkStream { get; set; }
    public CancellationTokenSource Cts { get; set; } = new();
    // One slow connection must neither stall the source nor drop a partial
    // packet into the middle of that listener's encoded stream.
    internal Channel<AudioPacket> Output { get; } = Channel.CreateBounded<AudioPacket>(
        new BoundedChannelOptions(128) { SingleReader = true, FullMode = BoundedChannelFullMode.Wait });
}

public class MountPointStream
{
    public string MountPath { get; }
    public string Password { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public bool Enabled { get; set; } = true;

    // Stream state
    public bool IsOnline { get; set; }
    public string StreamTitle { get; set; } = "RadioTEDU Live Broadcast";
    public string StreamName { get; set; } = "RadioTEDU Station";
    public string StreamDescription { get; set; } = "High Quality Live Audio Stream";
    public string StreamGenre { get; set; } = "Various";
    public string ContentType { get; set; } = "audio/mpeg";
    public int BitRateKbps { get; set; } = 320;
    public int SampleRateHz { get; set; } = 44100;
    public int Channels { get; set; } = 2;
    public DateTime? SourceConnectedAt { get; set; }
    public string SourceUserAgent { get; set; } = "";
    public string SourceIpAddress { get; set; } = "";

    // Stats
    public int PeakListeners { get; private set; }
    public long TotalBytesTransferred { get; private set; }

    // Listeners
    private readonly ConcurrentDictionary<long, StreamingClient> _clients = new();
    private long _clientCounter = 0;
    private long _sourceCounter;
    private SourceLease? _currentSource;
    private CancellationTokenSource? _listenerGrace;
    private static readonly TimeSpan ReconnectGrace = TimeSpan.FromSeconds(15);

    // 256 KB Burst Buffer for Instant Zero-Lag Playback (~3-6 seconds of audio)
    private readonly byte[] _burstBuffer = new byte[262144];
    private int _burstBufferWritePos = 0;
    private int _burstBufferCount = 0;
    private long _sourceStartBytes;
    private readonly object _burstLock = new();

    // OGG Container Header Capture (Vorbis / Opus / Ogg FLAC)
    private byte[]? _oggHeaderPages;
    private readonly MemoryStream _oggCaptureStream = new();
    private bool _oggHeadersCaptured = false;

    private bool _metadataChanged = true;
    private byte[] _currentMetadataFrame = IcyMetadataInjector.CreateMetadataFrame("RadioTEDU Live Broadcast");

    public MountPointStream(string mountPath)
    {
        MountPath = mountPath.StartsWith('/') ? mountPath : "/" + mountPath;
        ContentType = DetectInitialContentType(MountPath);
    }

    public bool IsOggStream =>
        ContentType.Contains("ogg", StringComparison.OrdinalIgnoreCase) ||
        ContentType.Contains("opus", StringComparison.OrdinalIgnoreCase) ||
        ContentType.Contains("vorbis", StringComparison.OrdinalIgnoreCase) ||
        MountPath.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase) ||
        MountPath.EndsWith(".opus", StringComparison.OrdinalIgnoreCase);

    public int ConcurrentListeners => _clients.Count;

    private static string DetectInitialContentType(string path)
    {
        if (path.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase)) return "audio/ogg";
        if (path.EndsWith(".opus", StringComparison.OrdinalIgnoreCase)) return "audio/ogg; codecs=opus";
        if (path.EndsWith(".flac", StringComparison.OrdinalIgnoreCase)) return "audio/flac";
        if (path.EndsWith(".aac", StringComparison.OrdinalIgnoreCase)) return "audio/aac";
        return "audio/mpeg";
    }

    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle)) return;
        StreamTitle = newTitle.Trim();
        _currentMetadataFrame = IcyMetadataInjector.CreateMetadataFrame(StreamTitle);
        _metadataChanged = true;
    }

    // Only the newest authenticated source owns this mount. A short reconnect
    // can reuse existing listeners without letting an old source close them.
    internal SourceLease BeginSource(NetworkStream stream, string sourceIp, string userAgent,
        string contentType, int bitrate, string name, string genre, string description)
    {
        SourceLease lease; NetworkStream? previous; CancellationTokenSource? grace;
        StreamingClient[] incompatible = [];
        lock (_burstLock)
        {
            var nextType = !string.IsNullOrWhiteSpace(contentType)
                ? contentType : DetectInitialContentType(MountPath);
            if (_clients.Count != 0 && !string.Equals(ContentType, nextType, StringComparison.OrdinalIgnoreCase))
            {
                // A socket's advertised content type cannot change safely.
                incompatible = _clients.Values.ToArray();
                _clients.Clear();
            }
            previous = _currentSource?.Stream;
            grace = _listenerGrace; _listenerGrace = null;
            lease = new SourceLease(checked(++_sourceCounter), stream);
            _currentSource = lease;
            IsOnline = true;
            SourceConnectedAt = DateTime.Now;
            SourceIpAddress = sourceIp;
            SourceUserAgent = userAgent;
            ContentType = nextType;
            if (bitrate > 0) BitRateKbps = bitrate;
            if (!string.IsNullOrEmpty(name)) StreamName = name;
            if (!string.IsNullOrEmpty(genre)) StreamGenre = genre;
            StreamDescription = description;
            _currentMetadataFrame = IcyMetadataInjector.CreateMetadataFrame(StreamTitle);
            _metadataChanged = true;
            _burstBufferWritePos = 0;
            _burstBufferCount = 0;
            _sourceStartBytes = TotalBytesTransferred;
            _oggHeaderPages = null;
            _oggHeadersCaptured = false;
            _oggCaptureStream.SetLength(0);
        }
        CancelGrace(grace);
        if (previous is not null && !ReferenceEquals(previous, stream)) CloseSource(previous);
        foreach (var client in incompatible) CloseClient(client);
        return lease;
    }

    internal bool EndSource(SourceLease lease)
    {
        CancellationTokenSource? grace = null;
        lock (_burstLock)
        {
            if (!ReferenceEquals(_currentSource, lease)) return false;
            _currentSource = null;
            IsOnline = false;
            SourceConnectedAt = null;
            if (!_clients.IsEmpty) _listenerGrace = grace = new CancellationTokenSource();
        }
        if (grace is not null) _ = ExpireListenersAsync(grace);
        return true;
    }

    private async Task ExpireListenersAsync(CancellationTokenSource grace)
    {
        try
        {
            await Task.Delay(ReconnectGrace, grace.Token).ConfigureAwait(false);
            StreamingClient[] expired;
            lock (_burstLock)
            {
                if (!ReferenceEquals(_listenerGrace, grace) || _currentSource is not null) return;
                _listenerGrace = null;
                expired = _clients.Values.ToArray();
                _clients.Clear();
            }
            foreach (var client in expired) CloseClient(client);
        }
        catch (OperationCanceledException) when (grace.IsCancellationRequested) { }
        finally { grace.Dispose(); }
    }

    // Explicit server stop or mount removal remains immediate, not a grace wait.
    public void SetSourceDisconnected()
    {
        StreamingClient[] disconnected; NetworkStream? source; CancellationTokenSource? grace;
        lock (_burstLock)
        {
            source = _currentSource?.Stream; _currentSource = null;
            grace = _listenerGrace; _listenerGrace = null;
            IsOnline = false; SourceConnectedAt = null;
            disconnected = _clients.Values.ToArray(); _clients.Clear();
        }
        CancelGrace(grace);
        if (source is not null) CloseSource(source);
        foreach (var client in disconnected) CloseClient(client);
    }

    private static void CancelGrace(CancellationTokenSource? grace)
    {
        if (grace is null) return;
        try { grace.Cancel(); } catch (ObjectDisposedException) { }
    }
    private static void CloseSource(NetworkStream source)
    {
        try { source.Close(); }
        catch (Exception ex) when (ex is IOException or SocketException or ObjectDisposedException) { }
    }

    internal bool WriteAudioChunk(SourceLease lease, byte[] buffer, int offset, int count)
    {
        if (count <= 0) return true;
        List<long>? slow = null;
        lock (_burstLock)
        {
            if (!ReferenceEquals(_currentSource, lease)) return false;
            TotalBytesTransferred += count;
            if (IsOggStream && !_oggHeadersCaptured)
            {
                _oggCaptureStream.Write(buffer, offset, count);
                var captured = _oggCaptureStream.ToArray();
                var headerLength = TryParseOggHeaderLength(captured);
                if (headerLength > 0)
                {
                    _oggHeaderPages = captured.AsSpan(0, headerLength).ToArray();
                    _oggHeadersCaptured = true;
                }
                else if (_oggCaptureStream.Length > 65536)
                    _oggHeadersCaptured = true;
            }
            for (int i = 0; i < count; i++)
            {
                _burstBuffer[_burstBufferWritePos] = buffer[offset + i];
                _burstBufferWritePos = (_burstBufferWritePos + 1) % _burstBuffer.Length;
                if (_burstBufferCount < _burstBuffer.Length) _burstBufferCount++;
            }

            // The source reuses its read buffer. Copy once for all listeners.
            // Keep snapshot publication and enqueue atomic with registration.
            var listeners = _clients.Values;
            if (listeners.Count != 0)
            {
                var packet = new AudioPacket(buffer.AsSpan(offset, count).ToArray(), 0, count);
                if (listeners.Count > PeakListeners) PeakListeners = listeners.Count;
                foreach (var client in listeners)
                    if (!client.Output.Writer.TryWrite(packet)) (slow ??= new()).Add(client.Id);
            }
        }
        if (slow != null)
            foreach (var id in slow) KickClient(id);
        return true;
    }

    private static int TryParseOggHeaderLength(byte[] data)
    {
        int offset = 0;
        int pageCount = 0;

        while (offset + 27 <= data.Length)
        {
            // Verify 'OggS'
            if (data[offset] != 0x4F || data[offset + 1] != 0x67 || data[offset + 2] != 0x67 || data[offset + 3] != 0x53)
            {
                break;
            }

            int segmentCount = data[offset + 26];
            if (offset + 27 + segmentCount > data.Length)
            {
                break; // Incomplete page header
            }

            int bodyLength = 0;
            for (int i = 0; i < segmentCount; i++)
            {
                bodyLength += data[offset + 27 + i];
            }

            int totalPageSize = 27 + segmentCount + bodyLength;
            if (offset + totalPageSize > data.Length)
            {
                break; // Incomplete page body
            }

            long granule = BitConverter.ToInt64(data, offset + 6);
            int pageSeq = BitConverter.ToInt32(data, offset + 18);
            pageCount++;
            offset += totalPageSize;

            // In Vorbis/Opus streams, the header consists of the first 2 or 3 pages (with granule == 0)
            if (pageCount >= 3 || (pageCount >= 2 && granule > 0) || pageSeq >= 2)
            {
                return offset;
            }
        }

        return 0;
    }

    private async Task RunClientWriterAsync(StreamingClient client)
    {
        var stream = client.NetworkStream!;
        try
        {
            await foreach (var packet in client.Output.Reader.ReadAllAsync(client.Cts.Token))
            {
                using var deadline = CancellationTokenSource.CreateLinkedTokenSource(client.Cts.Token);
                deadline.CancelAfter(TimeSpan.FromSeconds(3));
                var data = packet.Data.AsMemory(packet.Offset, packet.Length);
                if (!client.SupportsIcyMetadata)
                {
                    await stream.WriteAsync(data, deadline.Token);
                    client.BytesSent += data.Length;
                    continue;
                }
                while (!data.IsEmpty)
                {
                    var count = Math.Min(data.Length, client.BytesUntilNextMetadata);
                    if (count > 0)
                    {
                        await stream.WriteAsync(data[..count], deadline.Token);
                        client.BytesSent += count;
                        client.BytesUntilNextMetadata -= count;
                        data = data[count..];
                    }
                    if (client.BytesUntilNextMetadata == 0)
                    {
                        var metadata = _metadataChanged ? _currentMetadataFrame : IcyMetadataInjector.CreateEmptyMetadataFrame();
                        await stream.WriteAsync(metadata, deadline.Token);
                        client.BytesSent += metadata.Length;
                        client.BytesUntilNextMetadata = IcyMetadataInjector.DefaultMetaInterval;
                    }
                }
            }
        }
        catch (Exception ex) when (ex is IOException or SocketException or OperationCanceledException or ObjectDisposedException)
        {
            // Only this listener's writer failed, timed out or was cancelled.
        }
        finally { KickClient(client.Id); }
    }

    public StreamingClient RegisterClient(string ipAddress, string userAgent, bool supportsIcy, NetworkStream stream)
    {
        var client = new StreamingClient
        {
            Id = Interlocked.Increment(ref _clientCounter),
            IpAddress = string.IsNullOrEmpty(ipAddress) ? "127.0.0.1" : ipAddress,
            UserAgent = string.IsNullOrEmpty(userAgent) ? "Generic Browser / Player" : userAgent,
            ConnectedSince = DateTime.Now,
            NetworkStream = stream,
        };
        lock (_burstLock)
        {
            if (!IsOnline)
            {
                // Source can leave between writing the HTTP header and joining.
                client.Cts.Cancel();
                return client;
            }
            client.SupportsIcyMetadata = !IsOggStream && supportsIcy;
            if (_burstBufferCount > 0)
            {
                int start = (_burstBufferWritePos - _burstBufferCount + _burstBuffer.Length) % _burstBuffer.Length;
                var burst = new byte[_burstBufferCount];
                for (int i = 0; i < burst.Length; i++)
                    burst[i] = _burstBuffer[(start + i) % _burstBuffer.Length];
                var skip = 0;
                if (IsOggStream && _oggHeaderPages is { Length: > 0 } oggHeader)
                {
                    var burstSourceOffset = TotalBytesTransferred - _sourceStartBytes - _burstBufferCount;
                    // Before the ring wraps it already includes the original
                    // Ogg prefix. Sending a second copy repeats packet numbers
                    // and may reset a listener decoder's timestamps.
                    if (burstSourceOffset > 0)
                    {
                        client.Output.Writer.TryWrite(new AudioPacket(oggHeader, 0, oggHeader.Length));
                        skip = (int)Math.Clamp(oggHeader.Length - burstSourceOffset, 0, burst.Length);
                    }
                }
                var offset = skip + BurstSyncOffset(burst.AsSpan(skip));
                if (offset < burst.Length)
                    client.Output.Writer.TryWrite(new AudioPacket(burst, offset, burst.Length - offset));
            }

            // An address and browser identifier do not identify a connection.
            // Publish only after queuing its cached prefix, under the same
            // lock used by the source, so no live packet can precede it.
            _clients[client.Id] = client;
            if (_clients.Count > PeakListeners) PeakListeners = _clients.Count;
        }
        _ = RunClientWriterAsync(client);
        return client;
    }

    private int BurstSyncOffset(ReadOnlySpan<byte> buffer)
    {
        if (IsOggStream)
        {
            for (var i = 0; i < Math.Min(buffer.Length - 4, 4096); i++)
                if (buffer[i] == 0x4f && buffer[i + 1] == 0x67 && buffer[i + 2] == 0x67 && buffer[i + 3] == 0x53)
                    return i;
        }
        else if (buffer.Length > 512 && (ContentType.Contains("mpeg", StringComparison.OrdinalIgnoreCase) ||
                 ContentType.Contains("mp3", StringComparison.OrdinalIgnoreCase) ||
                 ContentType.Contains("aac", StringComparison.OrdinalIgnoreCase)))
        {
            for (var i = 0; i < Math.Min(buffer.Length - 4, 2048); i++)
                if (buffer[i] == 0xff && (buffer[i + 1] & 0xe0) == 0xe0) return i;
        }
        return 0;
    }

    private static void CloseClient(StreamingClient client)
    {
        client.Output.Writer.TryComplete();
        try { client.Cts.Cancel(); } catch (ObjectDisposedException) { }
        try { client.NetworkStream?.Close(); } catch (IOException) { }
    }

    public bool KickClient(long id)
    {
        if (!_clients.TryRemove(id, out var client)) return false;
        CloseClient(client);
        return true;
    }

    public void RemoveClient(long id)
    {
        KickClient(id);
    }

    public List<ListenerInfo> GetConnectedListeners()
    {
        return _clients.Values.Select(c =>
        {
            var duration = DateTime.Now - c.ConnectedSince;
            return new ListenerInfo
            {
                Id = (int)(c.Id % int.MaxValue),
                IpAddress = c.IpAddress,
                UserAgent = c.UserAgent,
                ConnectedSeconds = (int)duration.TotalSeconds,
                BytesSent = c.BytesSent,
                Location = c.IpAddress == "127.0.0.1" || c.IpAddress == "::1" ? "Localhost / Server" : "LAN / External Web Player"
            };
        }).ToList();
    }
}
