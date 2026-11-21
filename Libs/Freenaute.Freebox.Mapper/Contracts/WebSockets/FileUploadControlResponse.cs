namespace Freenaute.Freebox.Mapper.Contracts.WebSockets;

/// <summary>Upload-specific envelope. file_size is documented on a destination-conflict start response.</summary>
public sealed class FileUploadControlResponse
{
    [JsonPropertyName("request_id")] public long? RequestId { get; init; }

    [JsonPropertyName("action")] public string? Action { get; init; }

    [JsonPropertyName("success")] public bool? Success { get; init; }

    [JsonPropertyName("result")] public FileUploadChunkResult? Result { get; init; }

    [JsonPropertyName("file_size")] public long? FileSize { get; init; }

    [JsonPropertyName("error_code")] public string? ErrorCode { get; init; }

    [JsonPropertyName("msg")] public string? Message { get; init; }
}
