namespace Freenaute.Freebox.Mapper.ServerSide.Authentication.Login;

public sealed record AuthorizeTrackResponse(
	[property: JsonPropertyName("status")] AuthorizationTrackEnum Status,
	[property: JsonPropertyName("challenge")]
	string Challenge
);
