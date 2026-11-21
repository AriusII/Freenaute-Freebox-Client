namespace Freenaute.Freebox.Mapper.Contracts.WebSockets;

public sealed class FileUploadFinalizeRequest
{
	[JsonPropertyName("request_id")] public long RequestId { get; init; }

	[JsonPropertyName("action")] public string Action => "upload_finalize";
}
