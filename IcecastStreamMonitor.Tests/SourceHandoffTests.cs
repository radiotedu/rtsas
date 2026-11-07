using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using IcecastStreamMonitor.Services.Streaming;
using Xunit;

namespace IcecastStreamMonitor.Tests;

public sealed class SourceHandoffTests
{
    private const string Mount="/next-source-fixture";

    [Fact]
    public async Task ListenerSurvivesABriefSourceDisconnectAndReceivesTheReplacement()
    {
        await using var lab=new Loopback();
        var first=await lab.OpenAsync(Source());Assert.Equal(200,await Header(first));
        using var listener=await lab.OpenAsync(Listener());Assert.Equal(200,await Header(listener));
        var a=Enumerable.Range(0,128).Select(x=>(byte)x).ToArray();var b=a.Reverse().ToArray();
        await first.GetStream().WriteAsync(a);Assert.Equal(a,await Exact(listener,a.Length));
        first.Dispose();await lab.WaitFor(()=>lab.Mount is{IsOnline:false});
        Assert.Equal(1,lab.Mount!.ConcurrentListeners);
        using var replacement=await lab.OpenAsync(Source());Assert.Equal(200,await Header(replacement));
        await replacement.GetStream().WriteAsync(b);Assert.Equal(b,await Exact(listener,b.Length));
        Assert.True(lab.Mount!.IsOnline);Assert.Equal(1,lab.Mount.ConcurrentListeners);
    }

    [Fact]
    public async Task SupersededSourceCannotCloseOrWriteIntoTheReplacement()
    {
        await using var lab=new Loopback();
        using var first=await lab.OpenAsync(Source());Assert.Equal(200,await Header(first));
        using var listener=await lab.OpenAsync(Listener());Assert.Equal(200,await Header(listener));
        var a=Enumerable.Range(0,128).Select(x=>(byte)x).ToArray();var b=a.Reverse().ToArray();
        await first.GetStream().WriteAsync(a);Assert.Equal(a,await Exact(listener,a.Length));
        using var replacement=await lab.OpenAsync(Source());Assert.Equal(200,await Header(replacement));
        await replacement.GetStream().WriteAsync(b);Assert.Equal(b,await Exact(listener,b.Length));
        // Give the old socket's finally block a chance to run after replacement.
        await Task.Delay(300);
        Assert.True(lab.Mount!.IsOnline);Assert.Equal(1,lab.Mount.ConcurrentListeners);
        await replacement.GetStream().WriteAsync(a);Assert.Equal(a,await Exact(listener,a.Length));
    }

    [Fact]
    public async Task ADisconnectedSourceEventuallyReleasesItsListeners()
    {
        await using var lab=new Loopback();
        var first=await lab.OpenAsync(Source());Assert.Equal(200,await Header(first));
        using var listener=await lab.OpenAsync(Listener());Assert.Equal(200,await Header(listener));
        first.Dispose();await lab.WaitFor(()=>lab.Mount is{IsOnline:false});Assert.Equal(1,lab.Mount!.ConcurrentListeners);
        await lab.WaitFor(()=>lab.Mount!.ConcurrentListeners==0,TimeSpan.FromSeconds(22));
        Assert.True(await Closed(listener));
    }

    [Fact]
    public async Task ExplicitMountRemovalDoesNotWaitForReconnectGrace()
    {
        await using var lab=new Loopback();
        var source=await lab.OpenAsync(Source());Assert.Equal(200,await Header(source));
        using var listener=await lab.OpenAsync(Listener());Assert.Equal(200,await Header(listener));
        source.Dispose();await lab.WaitFor(()=>lab.Mount is{IsOnline:false});
        Assert.True(lab.Server.RemoveMount(Mount));Assert.True(await Closed(listener));
    }

    [Fact]
    public async Task ACodecChangeClosesTheOldListenerInsteadOfMixingFormats()
    {
        await using var lab=new Loopback();
        using var source=await lab.OpenAsync(Source());Assert.Equal(200,await Header(source));
        using var listener=await lab.OpenAsync(Listener());Assert.Equal(200,await Header(listener));
        using var replacement=await lab.OpenAsync(Source("application/ogg"));Assert.Equal(200,await Header(replacement));
        Assert.True(await Closed(listener));
    }

    private static string Source(string contentType="audio/aac")=>$"SOURCE {Mount} HTTP/1.0\r\nContent-Type: {contentType}\r\nice-password: loopback-fixture-only\r\n\r\n";
    private static string Listener()=>$"GET {Mount} HTTP/1.0\r\nUser-Agent: SourceHandoffFixture/1.0\r\n\r\n";
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
    private static async Task<bool> Closed(TcpClient c)
    {
        using var stop=new CancellationTokenSource(TimeSpan.FromSeconds(10));var one=new byte[1];
        try{return await c.GetStream().ReadAsync(one,stop.Token)==0;}
        catch(IOException){return true;}catch(SocketException){return true;}
    }
    private sealed class Loopback:IAsyncDisposable
    {
        private readonly TcpListener listener=new(IPAddress.Loopback,0);private readonly CancellationTokenSource stop=new();private readonly List<Task> owned=[];
        private readonly MethodInfo handle=typeof(AudioStreamServer).GetMethod("HandleClientConnectionAsync",BindingFlags.Instance|BindingFlags.NonPublic)
            ??throw new MissingMethodException("Expected RTSAS connection handler.");
        public AudioStreamServer Server{get;}=new(){DefaultSourcePassword="loopback-fixture-only"};
        public MountPointStream? Mount=>Server.GetMount(SourceHandoffTests.Mount);
        public Loopback()=>listener.Start();
        public async Task<TcpClient> OpenAsync(string request)
        {
            using var deadline=new CancellationTokenSource(TimeSpan.FromSeconds(10));var client=new TcpClient(AddressFamily.InterNetwork);
            await client.ConnectAsync(IPAddress.Loopback,((IPEndPoint)listener.LocalEndpoint).Port,deadline.Token);
            var accepted=await listener.AcceptTcpClientAsync(deadline.Token);owned.Add((Task)handle.Invoke(Server,[accepted,stop.Token])!);
            await client.GetStream().WriteAsync(Encoding.Latin1.GetBytes(request),deadline.Token);return client;
        }
        public async Task WaitFor(Func<bool> predicate,TimeSpan? timeout=null)
        {
            using var deadline=new CancellationTokenSource(timeout??TimeSpan.FromSeconds(10));
            while(!predicate())await Task.Delay(20,deadline.Token);
        }
        public async ValueTask DisposeAsync()
        {
            stop.Cancel();listener.Stop();Server.RemoveMount(SourceHandoffTests.Mount);
            await Task.WhenAll(owned).WaitAsync(TimeSpan.FromSeconds(10));stop.Dispose();
        }
    }
}
