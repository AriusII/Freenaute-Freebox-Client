namespace Freenaute.Freebox.Mapper.Contracts.Protocol;

/// <summary>
///     The fields attested by notification target creation and update examples. Values are snapshotted by the
///     command.
/// </summary>
public sealed record NotificationTargetWrite
{
    [JsonPropertyName("name")] public required string Name { get; init; }
    [JsonPropertyName("type")] public required string Type { get; init; }
    [JsonPropertyName("token")] public required string Token { get; init; }
    [JsonPropertyName("api_url")] public required string ApiUrl { get; init; }
    [JsonPropertyName("message_type")] public required string MessageType { get; init; }
    [JsonPropertyName("subscriptions")] public required string[] Subscriptions { get; init; }
}
