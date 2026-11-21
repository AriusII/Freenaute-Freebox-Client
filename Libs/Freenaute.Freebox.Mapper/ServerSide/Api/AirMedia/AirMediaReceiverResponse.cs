namespace Freenaute.Freebox.Mapper.ServerSide.Api.AirMedia;

public sealed record AirMediaReceiverResponse(
	[property: JsonPropertyName("name")] string Name,
	[property: JsonPropertyName("password_protected")]
	bool PasswordProtected,
	[property: JsonPropertyName("capabilities")]
	AirMediaCapabilities Capabilities
);
