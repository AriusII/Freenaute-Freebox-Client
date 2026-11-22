namespace Freenaute.Freebox.Client.Tests;

public sealed class DdnsApiTests
{
	[Fact]
	public async Task StatusAndConfigurationUseProviderScopedRoutes()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Address.AbsolutePath.EndsWith("/status/", StringComparison.Ordinal)
				? """{"success":true,"result":{"status":"ok","next_refresh":42}}"""
				: """{"success":true,"result":{"enabled":true,"hostname":"fixture.fbxos.fr","user":"fixture-user"}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var ddns = new FreeboxConnectionApi(client.Transport).Ddns("dyndns");

		var status = await ddns.GetStatusAsync();
		var config = await ddns.GetConfigurationAsync();

		Assert.Equal("ok", status.Status);
		Assert.Equal(42, status.NextRefresh);
		Assert.True(config.Enabled);
		Assert.Equal("fixture.fbxos.fr", config.Hostname);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/connection/ddns/dyndns/status/", requests[0].Address.AbsolutePath);
		Assert.Equal("fixture-session", requests[0].SessionToken);
		Assert.Equal(HttpMethod.Get, requests[1].Method);
		Assert.Equal("/api/v16/connection/ddns/dyndns/", requests[1].Address.AbsolutePath);
	}

	[Fact]
	public async Task UpdateConfigurationPutsPatchOnProviderRouteAndOmitsUnsetFields()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"enabled":false,"hostname":"updated.fbxos.fr"}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var ddns = new FreeboxConnectionApi(client.Transport).Ddns("ovh");

		var config = await ddns.UpdateConfigurationAsync(new DdnsConfigurationPatch
		{
			Enabled = false,
			Hostname = "updated.fbxos.fr",
			Password = "write-only-secret"
		});

		Assert.False(config.Enabled);
		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal(HttpMethod.Put, request.Method);
		Assert.Equal("/api/v16/connection/ddns/ovh/", request.Address.AbsolutePath);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.False(body.RootElement.GetProperty("enabled").GetBoolean());
		Assert.Equal("updated.fbxos.fr", body.RootElement.GetProperty("hostname").GetString());
		Assert.Equal("write-only-secret", body.RootElement.GetProperty("password").GetString());
		Assert.False(body.RootElement.TryGetProperty("user", out _));
	}

	[Fact]
	public async Task NullPatchFieldIsRejectedBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var ddns = new FreeboxConnectionApi(client.Transport).Ddns("noip");

		await Assert.ThrowsAsync<ArgumentException>(async () => await ddns.UpdateConfigurationAsync(
			new DdnsConfigurationPatch { Hostname = Optional<string>.Null }));

		Assert.Empty(ResourceRequests(handler));
	}

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData("provider/name")]
	[InlineData("..")]
	public void InvalidProviderIsRejectedBeforeRequests(string provider)
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		Assert.ThrowsAny<ArgumentException>(() =>
			new FreeboxConnectionApi(client.Transport).Ddns(provider));
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
			await new FreeboxConnectionApi(client.Transport).Ddns("dyndns").GetStatusAsync());

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

		var pending = new FreeboxConnectionApi(client.Transport).Ddns("dyndns")
			.GetConfigurationAsync(cancellation.Token);
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
