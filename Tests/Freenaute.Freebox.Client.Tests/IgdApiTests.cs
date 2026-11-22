namespace Freenaute.Freebox.Client.Tests;

public sealed class IgdApiTests
{
	[Fact]
	public async Task ConfigurationRoundTripUsesUpnpIgdConfigRoute()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"enabled":true,"version":2}}"""
				: """{"success":true,"result":{"enabled":false,"version":1}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var igd = new FreeboxIgdApi(client.Transport);

		var config = await igd.GetConfigurationAsync();
		var updated = await igd.UpdateConfigurationAsync(new IgdConfigurationPatch { Enabled = true, Version = 2 });

		Assert.False(config.Enabled);
		Assert.True(updated.Enabled);
		Assert.Equal(2, updated.Version);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/upnpigd/config/", requests[0].Address.AbsolutePath);
		Assert.Equal("fixture-session", requests[0].SessionToken);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/upnpigd/config/", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.True(body.RootElement.GetProperty("enabled").GetBoolean());
		Assert.Equal(2, body.RootElement.GetProperty("version").GetInt64());
	}

	[Fact]
	public async Task RedirectionsListAndSingleDeleteUseExpectedRoutes()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Delete
				? """{"success":true}"""
				: """{"success":true,"result":[{"id":"redir-1","enabled":true,"proto":"tcp","ext_port":443}]}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var igd = new FreeboxIgdApi(client.Transport);

		var redirections = await igd.GetRedirectionsAsync();
		await igd.Redirection("redir-1").DeleteAsync();

		Assert.Equal("redir-1", Assert.Single(redirections).Id);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/upnpigd/redir/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Delete, requests[1].Method);
		Assert.Equal("/api/v16/upnpigd/redir/redir-1", requests[1].Address.AbsolutePath);
	}

	[Fact]
	public async Task NullPatchFieldIsRejectedBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		await Assert.ThrowsAsync<ArgumentException>(async () => await new FreeboxIgdApi(client.Transport)
			.UpdateConfigurationAsync(new IgdConfigurationPatch { Version = Optional<long>.Null }));

		Assert.Empty(ResourceRequests(handler));
	}

	[Fact]
	public async Task ApiErrorIsMappedToFreeboxApiException()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":false,"error_code":"invalid_request","msg":"Fixture failure"}""", HttpStatusCode.BadRequest);
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var error = await Assert.ThrowsAsync<FreeboxApiException>(async () =>
			await new FreeboxIgdApi(client.Transport).GetRedirectionsAsync());

		Assert.Equal(HttpStatusCode.BadRequest, error.StatusCode);
		Assert.Equal("invalid_request", error.ErrorCode);
		Assert.Equal("Fixture failure", error.ApiMessage);
	}

	[Fact]
	public async Task CancellationReachesHttpHandler()
	{
		var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
		using var handler = FixtureHandler.Authenticated(async (_, cancellationToken) =>
		{
			started.SetResult();
			await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
			throw new InvalidOperationException("The canceled fixture must not finish normally.");
		});
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		using var cancellation = new CancellationTokenSource();

		var pending = new FreeboxIgdApi(client.Transport).GetConfigurationAsync(cancellation.Token);
		await started.Task.WaitAsync(TimeSpan.FromSeconds(5));
		await cancellation.CancelAsync();

		await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await pending);
	}

	private static IEnumerable<CapturedRequest> ResourceRequests(FixtureHandler handler)
	{
		return handler.Requests.Where(request =>
			request.Address.AbsolutePath is not "/api_version" &&
			!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
			!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal));
	}
}
