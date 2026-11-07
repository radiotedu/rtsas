using System.Buffers.Binary;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using IcecastStreamMonitor.Services.Streaming;
using Xunit;

namespace IcecastStreamMonitor.Tests;

public sealed class ListenerContinuityTests
{
    private const string Mount = "/next-continuity-fixture";
    private const string Agent = "ContinuityFixture/1.0";

    [Fact]
    public async Task IdenticalIpAndUserAgentDoNotEvictAnExistingListener()
    {
        await using var lab=new Loopback();
        using var source=await lab.OpenAsync(Source());Assert.Equal(200,await Header(source));
        using var first=await lab.OpenAsync(Listener(Agent));Assert.Equal(200,await Header(first));
        byte[] one=Enumerable.Range(0,128).Select(x=>(byte)x).ToArray(),two=one.Reverse().ToArray();
        await source.GetStream().WriteAsync(one);Assert.Equal(one,await Exact(first,one.Length));
        using var second=await lab.OpenAsync(Listener(Agent));Assert.Equal(200,await Header(second));
        Assert.Equal(one,await Exact(second,one.Length));
        await source.GetStream().WriteAsync(two);
        Assert.Equal(two,await Exact(first,two.Length));Assert.Equal(two,await Exact(second,two.Length));
        Assert.Equal(2,lab.Server.GetMount(Mount)!.ConcurrentListeners);
    }

    [Fact]
    public async Task UnreadListenerCannotStallTheSourceOrAnotherListener()
    {
        await using var lab=new Loopback();
        using var source=await lab.OpenAsync(Source());Assert.Equal(200,await Header(source));
        using var healthy=await lab.OpenAsync(Listener("HealthyFixture"));Assert.Equal(200,await Header(healthy));
        using var slow=await lab.OpenAsync(Listener("SlowFixture"),receiveBuffer:1024);Assert.Equal(200,await Header(slow));
        var packet=new byte[16*1024];var clock=Stopwatch.StartNew();
        for(var i=0;i<220;i++)
        {
            packet.AsSpan().Fill((byte)i);await source.GetStream().WriteAsync(packet);
            Assert.Equal(packet,await Exact(healthy,packet.Length));
        }
        Assert.True(clock.Elapsed<TimeSpan.FromSeconds(10),"An unread peer blocked the healthy listener.");
        using var deadline=new CancellationTokenSource(TimeSpan.FromSeconds(8));
        while(lab.Server.GetMount(Mount)!.GetConnectedListeners().Any(x=>x.UserAgent=="SlowFixture"))
            await Task.Delay(20,deadline.Token);
        Assert.Contains(lab.Server.GetMount(Mount)!.GetConnectedListeners(),x=>x.UserAgent=="HealthyFixture");
    }

    [Fact]
    public async Task OggLateListenerReceivesHeadersOnceBeforeCachedAudio()
    {
        await using var lab=new Loopback();
        using var source=await lab.OpenAsync(Source("application/ogg"));Assert.Equal(200,await Header(source));
        var prefix=Enumerable.Range(0,4).SelectMany(i=>OggPage(i)).ToArray();
        await source.GetStream().WriteAsync(prefix);await lab.WaitForBytes(prefix.Length);
        using var late=await lab.OpenAsync(Listener("OggFixture"));Assert.Equal(200,await Header(late));
        Assert.Equal(prefix,await Exact(late,prefix.Length));
        var next=OggPage(4);await source.GetStream().WriteAsync(next);
        Assert.Equal(next,await Exact(late,next.Length));
    }

    [Fact]
    public async Task OggLateListenerReceivesOneHeaderAndPageAlignedRingAfterWrap()
    {
        await using var lab=new Loopback();
        using var source=await lab.OpenAsync(Source("application/ogg"));Assert.Equal(200,await Header(source));
        var pages=Enumerable.Range(0,1100).SelectMany(i=>OggPage(i)).ToArray();
        await source.GetStream().WriteAsync(pages);await lab.WaitForBytes(pages.Length);
        const int ring=262144;Assert.True(pages.Length>ring);
        var ringStart=pages.Length-ring;var aligned=ringStart;
        while(aligned<pages.Length-4&&!pages.AsSpan(aligned,4).SequenceEqual("OggS"u8))aligned++;
        Assert.True(aligned-ringStart<4096);
        var expected=pages.AsSpan(0,OggPage(0).Length*3).ToArray().Concat(pages.AsSpan(aligned).ToArray()).ToArray();
        using var late=await lab.OpenAsync(Listener("OggWrapFixture"));Assert.Equal(200,await Header(late));
        Assert.Equal(expected,await Exact(late,expected.Length));
    }

    // Ogg framing fixture only: no codec or PCM-decoder compliance is asserted.
    private static byte[] OggPage(int seq)
    {
        const int body=220;var page=new byte[28+body];"OggS"u8.CopyTo(page);
        BinaryPrimitives.WriteInt64LittleEndian(page.AsSpan(6,8),seq<3?0:seq*960L);
        BinaryPrimitives.WriteInt32LittleEndian(page.AsSpan(14,4),12345);
        BinaryPrimitives.WriteInt32LittleEndian(page.AsSpan(18,4),seq);
        page[26]=1;page[27]=body;page.AsSpan(28).Fill((byte)(seq%251));return page;
    }

    private static string Source(string contentType="audio/aac")=>$"SOURCE {Mount} HTTP/1.0\r\nContent-Type: {contentType}\r\nice-password: loopback-fixture-only\r\n\r\n";
    private static string Listener(string agent)=>$"GET {Mount} HTTP/1.0\r\nUser-Agent: {agent}\r\n\r\n";
    private static async Task<int> Header(TcpClient c)
    {
        using var stop=new CancellationTokenSource(TimeSpan.FromSeconds(10));var bytes=new List<byte>();var one=new byte[1];
        while(bytes.Count<8192)
        {
            if(await c.GetStream().ReadAsync(one,stop.Token)==0)throw new EndOfStreamException();bytes.Add(one[0]);
            if(bytes.Count>=4&&bytes[^4]==13&&bytes[^3]==10&&bytes[^2]==13&&bytes[^1]==10)
                return int.Parse(Encoding.Latin1.GetString(bytes.ToArray()).Split(' ',3)[1]);
        }
        throw new InvalidDataException("Fixture HTTP header too large.");
    }
    private static async Task<byte[]> Exact(TcpClient c,int count)
    {
        using var stop=new CancellationTokenSource(TimeSpan.FromSeconds(10));var bytes=new byte[count];
        await c.GetStream().ReadExactlyAsync(bytes,stop.Token);return bytes;
    }
    private sealed class Loopback:IAsyncDisposable
    {
        private readonly TcpListener listener=new(IPAddress.Loopback,0);
        private readonly CancellationTokenSource stop=new();private readonly List<Task> owned=[];
        private readonly MethodInfo handle=typeof(AudioStreamServer).GetMethod("HandleClientConnectionAsync",BindingFlags.Instance|BindingFlags.NonPublic)
            ??throw new MissingMethodException("Expected RTSAS connection handler.");
        public AudioStreamServer Server{get;}=new(){DefaultSourcePassword="loopback-fixture-only"};
        public Loopback()=>listener.Start();
        public async Task<TcpClient> OpenAsync(string request,int? receiveBuffer=null)
        {
            using var deadline=new CancellationTokenSource(TimeSpan.FromSeconds(10));var client=new TcpClient(AddressFamily.InterNetwork);
            if(receiveBuffer is int n)client.ReceiveBufferSize=n;
            await client.ConnectAsync(IPAddress.Loopback,((IPEndPoint)listener.LocalEndpoint).Port,deadline.Token);
            var accepted=await listener.AcceptTcpClientAsync(deadline.Token);if(receiveBuffer.HasValue)accepted.SendBufferSize=4096;
            owned.Add((Task)handle.Invoke(Server,[accepted,stop.Token])!);
            await client.GetStream().WriteAsync(Encoding.Latin1.GetBytes(request),deadline.Token);return client;
        }
        public async Task WaitForBytes(int count)
        {
            using var deadline=new CancellationTokenSource(TimeSpan.FromSeconds(10));
            while((Server.GetMount(Mount)?.TotalBytesTransferred??0)<count)await Task.Delay(10,deadline.Token);
        }
        public async ValueTask DisposeAsync()
        {
            stop.Cancel();listener.Stop();Server.RemoveMount(Mount);
            await Task.WhenAll(owned).WaitAsync(TimeSpan.FromSeconds(10));stop.Dispose();
        }
    }
}
