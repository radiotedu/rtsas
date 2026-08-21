using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using IcecastStreamMonitor.Models;

namespace IcecastStreamMonitor.Services.Streaming;

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

    // 256 KB Burst Buffer for Instant Zero-Lag Playback (~3-6 seconds of audio)
    private readonly byte[] _burstBuffer = new byte[262144];
    private int _burstBufferWritePos = 0;
    private int _burstBufferCount = 0;
    private readonly object _burstLock = new();

    private bool _metadataChanged = true;
    private byte[] _currentMetadataFrame = IcyMetadataInjector.CreateMetadataFrame("RadioTEDU Live Stream");

    public MountPointStream(string mountPath)
    {
        MountPath = mountPath.StartsWith('/') ? mountPath : "/" + mountPath;
    }

    public int ConcurrentListeners => _clients.Count;

    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle)) return;
        StreamTitle = newTitle.Trim();
        _currentMetadataFrame = IcyMetadataInjector.CreateMetadataFrame(StreamTitle);
        _metadataChanged = true;
    }

    public void SetSourceConnected(string sourceIp, string userAgent, string contentType, int bitrate, string name, string genre)
    {
        IsOnline = true;
        SourceConnectedAt = DateTime.Now;
        SourceIpAddress = sourceIp;
        SourceUserAgent = userAgent;
        if (!string.IsNullOrEmpty(contentType)) ContentType = contentType;
        if (bitrate > 0) BitRateKbps = bitrate;
        if (!string.IsNullOrEmpty(name)) StreamName = name;
        if (!string.IsNullOrEmpty(genre)) StreamGenre = genre;

        _currentMetadataFrame = IcyMetadataInjector.CreateMetadataFrame(StreamTitle);
        _metadataChanged = true;

        lock (_burstLock)
        {
            _burstBufferWritePos = 0;
            _burstBufferCount = 0;
        }
    }

    public void SetSourceDisconnected()
    {
        IsOnline = false;
        SourceConnectedAt = null;

        // Disconnect listeners when source drops
        foreach (var client in _clients.Values)
        {
            try
            {
                client.Cts.Cancel();
            }
            catch { }
        }
        _clients.Clear();
    }

    public void WriteAudioChunk(byte[] buffer, int offset, int count)
    {
        if (count <= 0) return;

        TotalBytesTransferred += count;

        // Write to circular burst buffer
        lock (_burstLock)
        {
            for (int i = 0; i < count; i++)
            {
                _burstBuffer[_burstBufferWritePos] = buffer[offset + i];
                _burstBufferWritePos = (_burstBufferWritePos + 1) % _burstBuffer.Length;
                if (_burstBufferCount < _burstBuffer.Length)
                {
                    _burstBufferCount++;
                }
            }
        }

        // Broadcast to all connected clients
        var clients = _clients.Values.ToList();
        if (clients.Count > PeakListeners)
        {
            PeakListeners = clients.Count;
        }

        foreach (var client in clients)
        {
            if (client.Cts.IsCancellationRequested || client.NetworkStream == null)
            {
                RemoveClient(client.Id);
                continue;
            }

            try
            {
                if (client.SupportsIcyMetadata)
                {
                    SendChunkWithIcyMetadata(client, buffer, offset, count);
                }
                else
                {
                    client.NetworkStream.Write(buffer, offset, count);
                    client.BytesSent += count;
                }
            }
            catch
            {
                RemoveClient(client.Id);
            }
        }
    }

    private void SendChunkWithIcyMetadata(StreamingClient client, byte[] buffer, int offset, int count)
    {
        if (client.NetworkStream == null) return;

        int bytesRemaining = count;
        int currentOffset = offset;

        while (bytesRemaining > 0)
        {
            int toSend = Math.Min(bytesRemaining, client.BytesUntilNextMetadata);
            if (toSend > 0)
            {
                client.NetworkStream.Write(buffer, currentOffset, toSend);
                client.BytesSent += toSend;
                client.BytesUntilNextMetadata -= toSend;
                bytesRemaining -= toSend;
                currentOffset += toSend;
            }

            if (client.BytesUntilNextMetadata == 0)
            {
                // Send metadata frame
                var metaFrame = _metadataChanged ? _currentMetadataFrame : IcyMetadataInjector.CreateEmptyMetadataFrame();
                client.NetworkStream.Write(metaFrame, 0, metaFrame.Length);
                client.BytesSent += metaFrame.Length;
                client.BytesUntilNextMetadata = IcyMetadataInjector.DefaultMetaInterval;
            }
        }
    }

    public StreamingClient RegisterClient(string ipAddress, string userAgent, bool supportsIcy, NetworkStream stream)
    {
        var id = Interlocked.Increment(ref _clientCounter);
        var client = new StreamingClient
        {
            Id = id,
            IpAddress = string.IsNullOrEmpty(ipAddress) ? "127.0.0.1" : ipAddress,
            UserAgent = string.IsNullOrEmpty(userAgent) ? "Generic Browser / Player" : userAgent,
            ConnectedSince = DateTime.Now,
            SupportsIcyMetadata = supportsIcy,
            NetworkStream = stream,
            BytesUntilNextMetadata = IcyMetadataInjector.DefaultMetaInterval
        };

        _clients[id] = client;
        if (_clients.Count > PeakListeners)
        {
            PeakListeners = _clients.Count;
        }

        // Send initial burst buffer if available (Instant Playback Engine)
        lock (_burstLock)
        {
            if (_burstBufferCount > 0)
            {
                try
                {
                    int startPos = (_burstBufferWritePos - _burstBufferCount + _burstBuffer.Length) % _burstBuffer.Length;
                    var rawBurst = new byte[_burstBufferCount];
                    for (int i = 0; i < _burstBufferCount; i++)
                    {
                        rawBurst[i] = _burstBuffer[(startPos + i) % _burstBuffer.Length];
                    }

                    // Find frame sync header (0xFF, 0xEx/0xFx) in first 2048 bytes for clean decoder start
                    int syncOffset = 0;
                    if (rawBurst.Length > 512 && (ContentType.Contains("mpeg", StringComparison.OrdinalIgnoreCase) || ContentType.Contains("mp3", StringComparison.OrdinalIgnoreCase) || ContentType.Contains("aac", StringComparison.OrdinalIgnoreCase)))
                    {
                        for (int s = 0; s < Math.Min(rawBurst.Length - 4, 2048); s++)
                        {
                            if (rawBurst[s] == 0xFF && (rawBurst[s + 1] & 0xE0) == 0xE0)
                            {
                                syncOffset = s;
                                break;
                            }
                        }
                    }

                    int validLength = rawBurst.Length - syncOffset;
                    if (validLength > 0)
                    {
                        if (client.SupportsIcyMetadata)
                        {
                            SendChunkWithIcyMetadata(client, rawBurst, syncOffset, validLength);
                        }
                        else
                        {
                            stream.Write(rawBurst, syncOffset, validLength);
                            client.BytesSent += validLength;
                        }
                    }
                }
                catch
                {
                    RemoveClient(id);
                }
            }
        }

        return client;
    }

    public void RemoveClient(long id)
    {
        if (_clients.TryRemove(id, out var client))
        {
            try
            {
                client.Cts.Cancel();
                client.NetworkStream?.Close();
            }
            catch { }
        }
    }

    public bool KickClient(long id)
    {
        if (_clients.TryGetValue(id, out var client))
        {
            RemoveClient(id);
            return true;
        }
        return false;
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
