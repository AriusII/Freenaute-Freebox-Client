namespace Freenaute.Freebox.Mapper.Contracts.Protocol;

public sealed record NotificationTarget
{
	[JsonPropertyName("id")] public string? Id { get; init; }
	[JsonPropertyName("last_use")] public long LastUse { get; init; }
	[JsonPropertyName("type")] public string? Type { get; init; }
	[JsonPropertyName("name")] public string? Name { get; init; }
	[JsonPropertyName("api_url")] public string? ApiUrl { get; init; }
	[JsonPropertyName("message_type")] public string? MessageType { get; init; }
	[JsonPropertyName("subscriptions")] public string[]? Subscriptions { get; init; }
}
