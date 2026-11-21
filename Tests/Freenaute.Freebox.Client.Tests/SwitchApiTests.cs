namespace Freenaute.Freebox.Client.Tests;

public sealed class SwitchApiTests
{
	[Fact]
	public async Task SwitchStatusAndPortConfigurationUseDocumentedRoutes()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"id":2,"duplex":"full","speed":"1000"}}"""
				: request.Address.AbsolutePath.EndsWith("/status/", StringComparison.Ordinal)
					? """{"success":true,"result":[{"id":2,"link":"up","duplex":"full","speed":"1000"}]}"""
					: """{"success":true,"result":{"id":2,"duplex":"auto","speed":"auto"}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var network = new FreeboxNetworkApi(client.Transport);

		var status = await network.Switch.GetStatusAsync();
		var config = await network.Switch.Port(2).GetConfigurationAsync();
		var updated = await network.Switch.Port(2)
			.UpdateConfigurationAsync(new SwitchPortConfigurationPatch { Duplex = "full", Speed = "1000" });

		Assert.Equal("up", Assert.Single(status).Link);
		Assert.Equal("auto", config.Duplex);
		Assert.Equal("1000", updated.Speed);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(3, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/switch/status/", requests[0].Address.AbsolutePath);
		Assert.Equal("fixture-session", requests[0].SessionToken);
		Assert.Equal("/api/v16/switch/port/2", requests[1].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[2].Method);
		Assert.Equal("/api/v16/switch/port/2", requests[2].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[2].Body!);
		Assert.Equal("full", body.RootElement.GetProperty("duplex").GetString());
		Assert.Equal("1000", body.RootElement.GetProperty("speed").GetString());
	}

	[Fact]
	public async Task SfpUsesSlashlessStatusAndConfigRoutes()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"sfp_type_forced":true,"sfp_type_forced_value":"ethernet"}}"""
				: request.Address.AbsolutePath.EndsWith("/status", StringComparison.Ordinal)
					? """{"success":true,"result":{"present":true,"link":true,"type":"ethernet"}}"""
					: """{"success":true,"result":{"sfp_type_forced":false,"available_sfp_types":["ethernet","pon"]}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var sfp = new FreeboxSfpApi(client.Transport);

		var status = await sfp.GetStatusAsync();
		var config = await sfp.GetConfigurationAsync();
		var updated = await sfp.UpdateConfigurationAsync(new SfpConfigurationPatch
		{
			SfpTypeForced = true, SfpTypeForcedValue = "ethernet"
		});

		Assert.True(status.Present);
		Assert.Equal("ethernet", status.Type);
		Assert.False(config.SfpTypeForced);
		Assert.True(updated.SfpTypeForced);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(3, requests.Length);
		Assert.Equal("/api/v16/sfp/status", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/sfp/config/", requests[1].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[2].Method);
		Assert.Equal("/api/v16/sfp/config", requests[2].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[2].Body!);
		Assert.True(body.RootElement.GetProperty("sfp_type_forced").GetBoolean());
		Assert.Equal("ethernet", body.RootElement.GetProperty("sfp_type_forced_value").GetString());
	}

	[Fact]
	public async Task ValidationFailsBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var network = new FreeboxNetworkApi(client.Transport);

		await Assert.ThrowsAsync<ArgumentException>(() => network.Switch.Port(2)
			.UpdateConfigurationAsync(new SwitchPortConfigurationPatch { Duplex = Optional<string>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(() => network.Sfp.UpdateConfigurationAsync(
			new SfpConfigurationPatch { SfpTypeForcedValue = Optional<string>.Null }));
		Assert.Empty(ResourceRequests(handler));
	}

	[Fact]
	public async Task ApiErrorIsMappedToFreeboxApiException()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":false,"error_code":"invalid_request","msg":"Fixture failure"}""", HttpStatusCode.BadRequest);
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var error = await Assert.ThrowsAsync<FreeboxApiException>(() =>
			new FreeboxNetworkApi(client.Transport).Switch.GetStatusAsync());

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

		var pending = new FreeboxSfpApi(client.Transport).GetStatusAsync(cancellation.Token);
		await started.Task.WaitAsync(TimeSpan.FromSeconds(5));
		cancellation.Cancel();

		await Assert.ThrowsAnyAsync<OperationCanceledException>(() => pending);
	}

	private static IEnumerable<CapturedRequest> ResourceRequests(FixtureHandler handler)
	{
		return handler.Requests.Where(request =>
			request.Address.AbsolutePath is not "/api_version" &&
			!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
			!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal));
	}
}
