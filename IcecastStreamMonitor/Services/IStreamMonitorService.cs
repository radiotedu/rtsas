using IcecastStreamMonitor.Models;

namespace IcecastStreamMonitor.Services;

public interface IStreamMonitorService
{
    Task<StreamStatus> CheckStreamAsync(StreamChannel channel, string defaultPassword, TimeSpan timeout, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, StreamStatus>> CheckAllStreamsAsync(IEnumerable<StreamChannel> channels, string defaultPassword, TimeSpan timeout, CancellationToken cancellationToken = default);
    Task<List<ListenerInfo>> GetListenersAsync(StreamChannel channel, string defaultPassword, TimeSpan timeout, CancellationToken cancellationToken = default);
}
