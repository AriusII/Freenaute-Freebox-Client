namespace Freenaute.Freebox.Mapper.Contracts.WebSockets.Uploads;

public sealed class FileUploadFinalizeRequest
{
	[JsonPropertyName("request_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public long? RequestId { get; init; }

	[JsonPropertyName(MapperConstants.JsonProperties.Action)]
	public string Action => MapperConstants.WebSocketActions.UploadFinalize;
}
