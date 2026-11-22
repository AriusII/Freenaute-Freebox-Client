namespace Freenaute.Freebox.Mapper.ClientSide.WebSockets;

/// <summary>
///     Represents a WebSocket request.
/// </summary>
/// <param name="RequestId">
///     The optional request ID. If specified, it will be added in the corresponding reply to correlate
///     responses to the request.
/// </param>
/// <param name="Action">The action of the request.</param>
public sealed record WebSocketRequest(
	[property: JsonPropertyName("request_id")]
	[property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	long? RequestId,
	[property: JsonPropertyName(MapperConstants.JsonProperties.Action)]
	string Action);
