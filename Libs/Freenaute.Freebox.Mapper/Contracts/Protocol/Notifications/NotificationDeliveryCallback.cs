namespace Freenaute.Freebox.Mapper.Contracts.Protocol.Notifications;

public sealed record NotificationDeliveryCallback
{
	[JsonPropertyName("box_id")] public string? BoxId { get; init; }
	[JsonPropertyName("devices")] public string[]? Devices { get; init; }
	[JsonPropertyName("title")] public string? Title { get; init; }
	[JsonPropertyName("body")] public string? Body { get; init; }
	[JsonPropertyName("payload")] public JsonElement Payload { get; init; }
}
