namespace Freenaute.Freebox.Client.Tests;

public sealed class UPnpAvApiTests
{
	[Fact]
	public async Task ConfigurationRoundTripUsesUpnpAvConfigRouteAndKeepsExplicitFalse()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"enabled":false}}"""
				: """{"success":true,"result":{"enabled":true}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var upnpAv = new FreeboxUPnpAvApi(client.Transport);

		var config = await upnpAv.GetConfigurationAsync();
		var updated = await upnpAv.UpdateConfigurationAsync(new UPnpAvConfigPatch { Enabled = false });

		Assert.True(config.Enabled);
		Assert.False(updated.Enabled);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/upnpav/config/", requests[0].Address.AbsolutePath);
		Assert.Equal("fixture-session", requests[0].SessionToken);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/upnpav/config/", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.False(body.RootElement.GetProperty("enabled").GetBoolean());
		Assert.Single(body.RootElement.EnumerateObject());
	}

	[Fact]
	public async Task FluentConfigureCommandSendsPatchedRequest()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"enabled":true}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var result = await new FreeboxUPnpAvApi(client.Transport).Configure()
			.With(patch => patch with { Enabled = true }).SendAsync();

		Assert.True(result.Enabled);
		Assert.Equal("/api/v16/upnpav/config/", Assert.Single(ResourceRequests(handler)).Address.AbsolutePath);
	}

	[Fact]
	public async Task NullPatchFieldIsRejectedBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		await Assert.ThrowsAsync<ArgumentException>(async () => await new FreeboxUPnpAvApi(client.Transport)
			.UpdateConfigurationAsync(new UPnpAvConfigPatch { Enabled = Optional<bool>.Null }));

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
			await new FreeboxUPnpAvApi(client.Transport).GetConfigurationAsync());

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

		var pending = new FreeboxUPnpAvApi(client.Transport).GetConfigurationAsync(cancellation.Token);
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
