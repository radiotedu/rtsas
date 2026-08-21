using System.Collections.Concurrent;
using System.Text.Json;
using IcecastStreamMonitor.Models;

namespace IcecastStreamMonitor.Services;

public class StreamHistoryService
{
    private readonly ConcurrentDictionary<Guid, List<StreamSnapshotLog>> _history = new();
    private readonly string _storagePath;
    private readonly object _lock = new();

    public StreamHistoryService(string? customPath = null)
    {
        if (!string.IsNullOrEmpty(customPath))
        {
            _storagePath = customPath;
        }
        else
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(appData, "IcecastStreamMonitor");
            Directory.CreateDirectory(appFolder);
            _storagePath = Path.Combine(appFolder, "history_24h.json");
        }

        LoadHistory();
    }

    public void RecordSnapshot(Guid channelId, StreamStatus status, int intervalSeconds, string channelName = "")
    {
        lock (_lock)
        {
            if (!_history.TryGetValue(channelId, out var list))
            {
                list = new List<StreamSnapshotLog>();
                _history[channelId] = list;
            }

            // Estimate MB transferred in this slice: (bitrate kbps / 8) / 1024 * listeners * interval_seconds
            double mbSlice = 0;
            if (status.IsOnline && status.BitRateKbps > 0 && status.ConcurrentListeners > 0)
            {
                double bytesPerSec = (status.BitRateKbps * 1000.0 / 8.0) * status.ConcurrentListeners;
                mbSlice = (bytesPerSec * intervalSeconds) / (1024.0 * 1024.0);
            }

            var snapshot = new StreamSnapshotLog
            {
                Timestamp = DateTime.Now,
                ChannelId = channelId,
                IsOnline = status.IsOnline,
                Listeners = status.ConcurrentListeners,
                BitRateKbps = status.BitRateKbps,
                LatencyMs = status.LatencyMs,
                MegaBytesTransferred = mbSlice,
                SongTitle = status.StreamTitle,
                StatusMessage = status.StatusMessage
            };

            list.Add(snapshot);
            PruneOldLogs(list);
        }
    }

    public List<StreamSnapshotLog> Get24HourHistory(Guid channelId, string? fallbackName = null, StreamStatus? currentStatus = null)
    {
        lock (_lock)
        {
            if (_history.TryGetValue(channelId, out var list) && list.Count > 0)
            {
                return list.OrderBy(s => s.Timestamp).ToList();
            }

            return new List<StreamSnapshotLog>();
        }
    }

    public Stream24HourMetrics GetMetricsSummary(Guid channelId)
    {
        var logs = Get24HourHistory(channelId);
        if (logs.Count == 0)
        {
            return new Stream24HourMetrics
            {
                PeakListeners = 0,
                MinListeners = 0,
                AverageListeners = 0,
                TotalMegaBytesTransferred = 0,
                UptimePercentage = 100.0,
                AverageLatencyMs = 0,
                TotalSnapshots = 0
            };
        }

        var onlineLogs = logs.Where(l => l.IsOnline).ToList();
        int peak = logs.Count > 0 ? logs.Max(l => l.Listeners) : 0;
        int min = onlineLogs.Count > 0 ? onlineLogs.Min(l => l.Listeners) : 0;
        double avgListeners = onlineLogs.Count > 0 ? onlineLogs.Average(l => l.Listeners) : 0;
        double totalMb = logs.Sum(l => l.MegaBytesTransferred);
        double uptime = (double)onlineLogs.Count / logs.Count * 100.0;
        double avgLatency = onlineLogs.Count > 0 ? onlineLogs.Average(l => l.LatencyMs) : 0;

        return new Stream24HourMetrics
        {
            PeakListeners = peak,
            MinListeners = min,
            AverageListeners = Math.Round(avgListeners, 1),
            TotalMegaBytesTransferred = Math.Round(totalMb, 2),
            UptimePercentage = Math.Round(uptime, 1),
            AverageLatencyMs = Math.Round(avgLatency, 1),
            TotalSnapshots = logs.Count
        };
    }

    private static void PruneOldLogs(List<StreamSnapshotLog> list)
    {
        var cutoff = DateTime.Now.AddHours(-24);
        list.RemoveAll(l => l.Timestamp < cutoff);
    }

    public void SaveHistory()
    {
        try
        {
            lock (_lock)
            {
                var dict = new Dictionary<string, List<StreamSnapshotLog>>();
                foreach (var (k, v) in _history)
                {
                    dict[k.ToString()] = v;
                }
                var json = JsonSerializer.Serialize(dict, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_storagePath, json);
            }
        }
        catch { }
    }

    private void LoadHistory()
    {
        try
        {
            if (File.Exists(_storagePath))
            {
                var json = File.ReadAllText(_storagePath);
                var dict = JsonSerializer.Deserialize<Dictionary<string, List<StreamSnapshotLog>>>(json);
                if (dict != null)
                {
                    foreach (var (k, v) in dict)
                    {
                        if (Guid.TryParse(k, out var guid))
                        {
                            PruneOldLogs(v);
                            _history[guid] = v;
                        }
                    }
                }
            }
        }
        catch { }
    }
}
