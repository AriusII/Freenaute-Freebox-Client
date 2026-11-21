namespace Freenaute.Freebox.Mapper.Contracts.Protocol;

/// <summary>A callback contract implemented by a consumer push server, never an outbound Freebox operation.</summary>
public sealed record NotificationRegistrationCallback
{
    [JsonPropertyName("box_id")] public string? BoxId { get; init; }
    [JsonPropertyName("device_type")] public string? DeviceType { get; init; }
    [JsonPropertyName("token")] public string? Token { get; init; }
    [JsonPropertyName("device_name")] public string? DeviceName { get; init; }
    [JsonPropertyName("device_id")] public string? DeviceId { get; init; }
}
