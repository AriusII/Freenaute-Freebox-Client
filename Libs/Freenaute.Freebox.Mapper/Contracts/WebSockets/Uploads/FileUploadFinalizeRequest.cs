namespace Freenaute.Freebox.Mapper.Contracts.WebSockets.Uploads;

public sealed class FileUploadFinalizeRequest
{
	[JsonPropertyName("request_id")] public long RequestId { get; init; }

	[JsonPropertyName("action")] public static string Action => "upload_finalize";
}
