using System.Text.RegularExpressions;

namespace IcecastStreamMonitor.Services;

public static class AudioFormatHelper
{
    public static string NormalizeAudioFormat(string? mimeType, string? audioInfo = null)
    {
        if (!string.IsNullOrWhiteSpace(audioInfo))
        {
            if (audioInfo.Contains("opus", StringComparison.OrdinalIgnoreCase))
                return "Opus";
            if (audioInfo.Contains("vorbis", StringComparison.OrdinalIgnoreCase))
                return "OGG Vorbis";
            if (audioInfo.Contains("flac", StringComparison.OrdinalIgnoreCase))
                return "FLAC";
            if (audioInfo.Contains("aacp", StringComparison.OrdinalIgnoreCase) || audioInfo.Contains("aac+", StringComparison.OrdinalIgnoreCase))
                return "AAC+";
            if (audioInfo.Contains("aac", StringComparison.OrdinalIgnoreCase))
                return "AAC";
            if (audioInfo.Contains("mp3", StringComparison.OrdinalIgnoreCase) || audioInfo.Contains("mpeg", StringComparison.OrdinalIgnoreCase))
                return "MP3";
        }

        if (string.IsNullOrWhiteSpace(mimeType))
            return "Unknown";

        var mime = mimeType.Trim().ToLowerInvariant();
        if (mime.Contains(';'))
            mime = mime.Split(';')[0].Trim();

        return mime switch
        {
            "audio/mpeg" or "audio/mp3" or "audio/x-mpeg" or "audio/x-mp3" => "MP3",
            "audio/aac" or "audio/x-aac" or "audio/mp4" or "audio/m4a" => "AAC",
            "audio/aacp" or "audio/x-aacp" or "audio/aac+" => "AAC+",
            "audio/ogg" or "application/ogg" or "application/x-ogg" => "OGG Vorbis",
            "audio/opus" => "Opus",
            "audio/flac" or "audio/x-flac" => "FLAC",
            "audio/wav" or "audio/x-wav" => "WAV",
            "audio/x-ms-wma" => "WMA",
            _ => mime.StartsWith("audio/") ? mime["audio/".Length..].ToUpperInvariant() : mime
        };
    }

    public static (int bitrate, int samplerate, int channels) ParseAudioInfo(string? audioInfo)
    {
        int bitrate = 0;
        int samplerate = 0;
        int channels = 0;

        if (string.IsNullOrWhiteSpace(audioInfo))
            return (bitrate, samplerate, channels);

        // Format examples: "channels=2;samplerate=44100;bitrate=128" or "bitrate=320;channels=2"
        var parts = audioInfo.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var part in parts)
        {
            var kv = part.Split('=', 2);
            if (kv.Length != 2) continue;

            var key = kv[0].Trim().ToLowerInvariant();
            var val = kv[1].Trim();

            if (key == "bitrate" && int.TryParse(val, out var br))
            {
                bitrate = br;
            }
            else if (key == "samplerate" && int.TryParse(val, out var sr))
            {
                samplerate = sr;
            }
            else if (key == "channels" && int.TryParse(val, out var ch))
            {
                channels = ch;
            }
        }

        return (bitrate, samplerate, channels);
    }

    public static int ParseBitrateString(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return 0;
        // Could be "128", "128000", "128 kbps", "128, 128"
        var match = Regex.Match(raw, @"\d+");
        if (match.Success && int.TryParse(match.Value, out var num))
        {
            // If in bps (e.g. 128000), convert to kbps
            if (num > 1000)
                num /= 1000;
            return num;
        }
        return 0;
    }
}
