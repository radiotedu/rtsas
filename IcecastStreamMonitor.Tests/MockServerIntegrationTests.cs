using IcecastStreamMonitor.Models;
using IcecastStreamMonitor.Services;
using Xunit;

namespace IcecastStreamMonitor.Tests;

public class MockServerIntegrationTests
{
    [Fact]
    public async Task StreamMonitorService_QueriesMockServer_Successfully()
    {
        // Choose an ephemeral port
        int testPort = 8123;
        using var mockServer = new MockStreamServer(testPort);
        mockServer.Start();

        using var monitor = new StreamMonitorService();

        var channelIcecastLive = new StreamChannel
        {
            Host = "127.0.0.1",
            Port = testPort,
            MountPath = "/live",
            ServerType = ServerType.Icecast,
            UseDefaultPassword = true
        };

        var channelMobileAac = new StreamChannel
        {
            Host = "127.0.0.1",
            Port = testPort,
            MountPath = "/mobile.aac",
            ServerType = ServerType.Icecast,
            UseDefaultPassword = true
        };

        var channelShoutcastV2 = new StreamChannel
        {
            Host = "127.0.0.1",
            Port = testPort,
            MountPath = "1",
            ServerType = ServerType.ShoutcastV2,
            UseDefaultPassword = true
        };

        var channelShoutcastV1 = new StreamChannel
        {
            Host = "127.0.0.1",
            Port = testPort,
            MountPath = "/7.html",
            ServerType = ServerType.ShoutcastV1,
            UseDefaultPassword = true
        };

        // Check Icecast live channel
        var statusLive = await monitor.CheckStreamAsync(channelIcecastLive, "hackme", TimeSpan.FromSeconds(3));
        Assert.True(statusLive.IsOnline);
        Assert.True(statusLive.ConcurrentListeners >= 100);
        Assert.Equal("MP3", statusLive.AudioFormat);
        Assert.Equal(320, statusLive.BitRateKbps);

        // Check AAC mobile channel
        var statusMobile = await monitor.CheckStreamAsync(channelMobileAac, "hackme", TimeSpan.FromSeconds(3));
        Assert.True(statusMobile.IsOnline);
        Assert.True(statusMobile.ConcurrentListeners >= 50);
        Assert.Equal("AAC+", statusMobile.AudioFormat);
        Assert.Equal(64, statusMobile.BitRateKbps);

        // Check SHOUTcast v2
        var statusShoutV2 = await monitor.CheckStreamAsync(channelShoutcastV2, "hackme", TimeSpan.FromSeconds(3));
        Assert.True(statusShoutV2.IsOnline);
        Assert.True(statusShoutV2.ConcurrentListeners >= 50);
        Assert.Equal("MP3", statusShoutV2.AudioFormat);
        Assert.Equal(192, statusShoutV2.BitRateKbps);

        // Check SHOUTcast v1
        var statusShoutV1 = await monitor.CheckStreamAsync(channelShoutcastV1, "hackme", TimeSpan.FromSeconds(3));
        Assert.True(statusShoutV1.IsOnline);
        Assert.True(statusShoutV1.ConcurrentListeners >= 40);
        Assert.Equal("MP3", statusShoutV1.AudioFormat);
        Assert.Equal(128, statusShoutV1.BitRateKbps);

        // Check batch monitor
        var channels = new List<StreamChannel> { channelIcecastLive, channelMobileAac, channelShoutcastV2 };
        var batchResults = await monitor.CheckAllStreamsAsync(channels, "hackme", TimeSpan.FromSeconds(3));
        Assert.Equal(3, batchResults.Count);
        Assert.All(batchResults.Values, res => Assert.True(res.IsOnline));
    }
}
