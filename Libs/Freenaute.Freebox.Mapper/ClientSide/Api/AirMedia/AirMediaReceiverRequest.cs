namespace Freenaute.Freebox.Mapper.ClientSide.Api.AirMedia;

public sealed record AirMediaReceiverRequest(
	[property: JsonPropertyName(MapperConstants.JsonProperties.Action)]
	AirMediaAction Action,
	[property: JsonPropertyName("media_type")]
	AirMediaMediaType MediaType,
	[property: JsonPropertyName("media")]
	[property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	string? Media = null,
	[property: JsonPropertyName("password")]
	[property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	string? Password = null,
	[property: JsonPropertyName("position")]
	[property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	int? Position = null
);
