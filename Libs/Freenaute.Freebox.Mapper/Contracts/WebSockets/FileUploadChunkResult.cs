namespace Freenaute.Freebox.Mapper.Contracts.WebSockets;

/// <summary>FileUploadChunkResponse; the final/cancel response has complete=true.</summary>
public sealed class FileUploadChunkResult
{
    [JsonPropertyName("total_len")] public long? TotalLength { get; init; }

    [JsonPropertyName("complete")] public bool? Complete { get; init; }

    [JsonPropertyName("cancelled")] public bool? Cancelled { get; init; }
}
