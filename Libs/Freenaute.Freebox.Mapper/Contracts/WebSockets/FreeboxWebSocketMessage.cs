namespace Freenaute.Freebox.Mapper.Contracts.WebSockets;

/// <summary>Common response/notification envelope. Result is explicitly extensible and may be omitted.</summary>
public sealed class FreeboxWebSocketMessage
{
	[JsonPropertyName("request_id")] public long? RequestId { get; init; }

	[JsonPropertyName(MapperConstants.JsonProperties.Action)]
	public string? Action { get; init; }

	[JsonPropertyName("success")] public bool? Success { get; init; }

	[JsonPropertyName("source")] public string? Source { get; init; }

	[JsonPropertyName("event")] public string? Event { get; init; }

	[JsonPropertyName("result")] public JsonElement? Result { get; init; }

	[JsonPropertyName("error_code")] public string? ErrorCode { get; init; }

	[JsonPropertyName("msg")] public string? Message { get; init; }
}
