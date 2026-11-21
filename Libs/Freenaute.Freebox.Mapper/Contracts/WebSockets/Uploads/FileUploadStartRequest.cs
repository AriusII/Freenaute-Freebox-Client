namespace Freenaute.Freebox.Mapper.Contracts.WebSockets.Uploads;

/// <summary>FileUploadStartAction. Size is optional and supplied by the caller, including during resume.</summary>
public sealed class FileUploadStartRequest
{
	[JsonPropertyName("request_id")] public long RequestId { get; init; }

	[JsonPropertyName("action")] public static string Action => "upload_start";

	[JsonPropertyName("size")] public long? Size { get; init; }

	[JsonPropertyName("dirname")] public EncodedFreeboxPath Directory { get; init; }

	[JsonPropertyName("filename")] public string Filename { get; init; } = "";

	[JsonPropertyName("force")] public string? Force { get; init; }
}
