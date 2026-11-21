namespace Freenaute.Freebox.Client.Tests;

internal sealed record CapturedRequest(
	HttpMethod Method,
	Uri Address,
	string? SessionToken,
	string? Body,
	string? ContentType);
