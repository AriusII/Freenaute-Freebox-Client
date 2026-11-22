namespace Freenaute.Freebox.Mapper.Contracts.WebSockets.Uploads;

/// <summary>Explicit destructive cancellation. Unlike closing the connection, this deletes the partial file.</summary>
public sealed class FileUploadCancelRequest
{
	[JsonPropertyName("request_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public long? RequestId { get; init; }

	[JsonPropertyName(MapperConstants.JsonProperties.Action)]
	public string Action => MapperConstants.WebSocketActions.UploadCancel;
}
