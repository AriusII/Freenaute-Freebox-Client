namespace Freenaute.Freebox.Mapper.Contracts.WebSockets.Uploads;

/// <summary>Explicit destructive cancellation. Unlike closing the connection, this deletes the partial file.</summary>
public sealed class FileUploadCancelRequest
{
	[JsonPropertyName("request_id")] public long RequestId { get; init; }

	[JsonPropertyName("action")] public string Action => "upload_cancel";
}
