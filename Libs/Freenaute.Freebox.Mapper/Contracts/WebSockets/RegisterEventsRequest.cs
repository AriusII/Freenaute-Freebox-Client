namespace Freenaute.Freebox.Mapper.Contracts.WebSockets;

/// <summary>RegisterAction and common WebSocketRequest. Outgoing events are validated by the event builder.</summary>
public sealed class RegisterEventsRequest
{
    [JsonPropertyName("action")] public string Action => "register";

    [JsonPropertyName("request_id")] public long RequestId { get; init; }

    [JsonPropertyName("events")] public string[] Events { get; init; } = [];
}
