namespace Freenaute.Freebox.Mapper.Contracts.WebSockets;

/// <summary>RegisterAction and common WebSocketRequest. Outgoing events are validated by the event builder.</summary>
public sealed class RegisterEventsRequest
{
	[JsonPropertyName(MapperConstants.JsonProperties.Action)]
	public string Action => MapperConstants.WebSocketActions.Register;

	[JsonPropertyName("request_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public long? RequestId { get; init; }

	[JsonPropertyName("events")] public string[] Events { get; init; } = [];
}
