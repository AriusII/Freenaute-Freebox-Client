namespace Freenaute.Freebox.Mapper.ServerSide.Models;

public sealed record SessionFailedModel(
	[property: JsonPropertyName("challenge")]
	string Challenge);
