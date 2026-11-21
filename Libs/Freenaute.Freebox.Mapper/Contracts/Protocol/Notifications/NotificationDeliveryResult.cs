namespace Freenaute.Freebox.Mapper.Contracts.Protocol.Notifications;

public sealed record NotificationDeliveryResult
{
	[JsonPropertyName("failureIds")] public string[]? FailureIds { get; init; }
	[JsonPropertyName("successIds")] public string[]? SuccessIds { get; init; }
}
