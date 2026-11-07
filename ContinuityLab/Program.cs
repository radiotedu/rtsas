using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.Json;
using IcecastStreamMonitor.Services.Streaming;

// Isolated synthetic lab only: literal loopback, ephemeral port, no production mounts.
var listener=new TcpListener(IPAddress.Loopback,0);
using var stop=new CancellationTokenSource();
var server=new AudioStreamServer{DefaultSourcePassword="loopback-fixture-only"};
var handler=typeof(AudioStreamServer).GetMethod("HandleClientConnectionAsync",BindingFlags.Instance|BindingFlags.NonPublic)
    ??throw new MissingMethodException("Expected RTSAS handler");
var owned=new List<Task>();listener.Start();
var accepting=Task.Run(async()=>
{
    try
    {
        while(!stop.IsCancellationRequested)
        {
            var client=await listener.AcceptTcpClientAsync(stop.Token);
            lock(owned)owned.Add((Task)handler.Invoke(server,[client,stop.Token])!);
        }
    }
    catch(OperationCanceledException) when(stop.IsCancellationRequested){}
    catch(SocketException) when(stop.IsCancellationRequested){}
});
Console.WriteLine(JsonSerializer.Serialize(new{port=((IPEndPoint)listener.LocalEndpoint).Port}));Console.Out.Flush();
await Console.In.ReadLineAsync();stop.Cancel();listener.Stop();await accepting;
foreach(var mount in server.GetAllMounts().Select(x=>x.MountPath).ToArray())server.RemoveMount(mount);
Task[] tasks;lock(owned)tasks=owned.ToArray();await Task.WhenAll(tasks).WaitAsync(TimeSpan.FromSeconds(10));
