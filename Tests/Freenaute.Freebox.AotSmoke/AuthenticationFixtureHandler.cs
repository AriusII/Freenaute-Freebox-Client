namespace Freenaute.Freebox.AotSmoke;

internal sealed class AuthenticationFixtureHandler : HttpMessageHandler
{
	public string? LoginPath { get; private set; }
	public string? ProtectedAuthorization { get; private set; }
	public string? LoginBody { get; private set; }

	public bool ObservedSessionBodyDoesNotContain(string secret) =>
		LoginBody is not null && !LoginBody.Contains(secret, StringComparison.Ordinal);

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		string path = request.RequestUri!.AbsolutePath;
		if (path.EndsWith("/login/", StringComparison.Ordinal))
		{
			return Json("""{"success":true,"result":{"logged_in":false,"challenge":"fixture-challenge"}}""");
		}

		if (path.EndsWith("/login/session/", StringComparison.Ordinal))
		{
			LoginPath = path;
			LoginBody = await request.Content!.ReadAsStringAsync(cancellationToken);
			return Json(
				"""{"success":true,"result":{"session_token":"fixture-session","permissions":{"settings":true}}}""");
		}

		ProtectedAuthorization = request.Headers.TryGetValues("X-Fbx-App-Auth", out var values)
			? values.Single()
			: null;
		return Json("""{"success":true,"result":{"enabled":true}}""");
	}

	private static HttpResponseMessage Json(string json)
	{
		return new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(json, Encoding.UTF8, "application/json")
		};
	}
}
