using System.Text;

namespace IcecastStreamMonitor.Services.Streaming;

/// <summary>
/// Handles ICY metadata frame generation and interval chunking for MP3/AAC/OGG streaming clients.
/// </summary>
public static class IcyMetadataInjector
{
    public const int DefaultMetaInterval = 8192; // 8 KB chunks

    /// <summary>
    /// Encodes a song title into standard ICY metadata frame bytes: StreamTitle='...';
    /// The frame length byte is (length / 16), padded with null bytes.
    /// </summary>
    public static byte[] CreateMetadataFrame(string streamTitle, string? streamUrl = null)
    {
        // Apostrophes inside a title are valid. Neutralize only the ICY
        // field terminator so text cannot inject another metadata field.
        var cleanTitle = (streamTitle ?? "Live Broadcast Feed").Replace("';", "' ");
        var metaString = $"StreamTitle='{cleanTitle}';";
        if (!string.IsNullOrEmpty(streamUrl))
        {
            metaString += $"StreamUrl='{streamUrl}';";
        }

        var metaBytes = Encoding.UTF8.GetBytes(metaString);
        int length = metaBytes.Length;

        // ICY metadata length must be a multiple of 16 bytes
        int padding = 16 - (length % 16);
        if (padding == 16) padding = 0;
        int totalBodyLength = length + padding;

        int numBlocks = totalBodyLength / 16;
        if (numBlocks > 255) numBlocks = 255; // 1 byte max

        var frame = new byte[1 + (numBlocks * 16)];
        frame[0] = (byte)numBlocks;

        int copyLen = Math.Min(length, numBlocks * 16);
        Array.Copy(metaBytes, 0, frame, 1, copyLen);

        // Remaining bytes are null-padded
        for (int i = 1 + copyLen; i < frame.Length; i++)
        {
            frame[i] = 0;
        }

        return frame;
    }

    /// <summary>
    /// Creates an empty 1-byte metadata frame (0x00) indicating no title change.
    /// </summary>
    public static byte[] CreateEmptyMetadataFrame()
    {
        return new byte[] { 0 };
    }
}
