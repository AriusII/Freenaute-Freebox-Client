namespace Freenaute.Freebox.Client.Tests;

public sealed class LanApiTests
{
	[Fact]
	public async Task ConfigurationRoundTripUsesLanConfigRouteAndKeepsExplicitValues()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"ip":"192.168.1.254","name":"fixture-box"}}"""
				: """{"success":true,"result":{"ip":"192.168.0.254","mode":"router"}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var lan = new FreeboxLanApi(client.Transport);

		var config = await lan.GetConfigurationAsync();
		var updated = await lan.UpdateConfigurationAsync(new LanConfigurationPatch
		{
			Ip = "192.168.1.254", Name = "fixture-box"
		});

		Assert.Equal("router", config.Mode);
		Assert.Equal("192.168.1.254", updated.Ip);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/lan/config/", requests[0].Address.AbsolutePath);
		Assert.Equal("fixture-session", requests[0].SessionToken);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/lan/config/", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.Equal("192.168.1.254", body.RootElement.GetProperty("ip").GetString());
		Assert.Equal(2, body.RootElement.EnumerateObject().Count());
	}

	[Fact]
	public async Task RoutesInterfacesAndHostTypesUseDocumentedRoutes()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Address.AbsolutePath.EndsWith("/routes", StringComparison.Ordinal)
				? """{"success":true,"result":[{"prefix":"10.0.0.0/8","gateway":"192.168.0.1","enabled":false}]}"""
				: request.Address.AbsolutePath.EndsWith("/interfaces/", StringComparison.Ordinal)
					? """{"success":true,"result":[{"name":"main","host_count":3}]}"""
					: """{"success":true,"result":[{"icon":"workstation","type":"workstation","name":"Workstation"}]}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var lan = new FreeboxLanApi(client.Transport);

		var routes = await lan.GetRoutesAsync();
		var interfaces = await lan.GetInterfacesAsync();
		var hostTypes = await lan.GetHostTypesAsync();

		Assert.False(Assert.Single(routes).Enabled);
		Assert.Equal(3, Assert.Single(interfaces).HostCount);
		Assert.Equal("workstation", Assert.Single(hostTypes).Type);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal("/api/v16/lan/routes", requests[0].Address.AbsolutePath);
		Assert.Equal("/api/v16/lan/browser/interfaces/", requests[1].Address.AbsolutePath);
		Assert.Equal("/api/v16/lan/browser/types/", requests[2].Address.AbsolutePath);
		Assert.All(requests, request => Assert.Equal(HttpMethod.Get, request.Method));
	}

	[Fact]
	public async Task InterfaceHostsWakeAndHostUpdateUseEncodedSegments()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Post
				? """{"success":true}"""
				: request.Address.AbsolutePath.EndsWith("/host-1/", StringComparison.Ordinal)
					? """{"success":true,"result":{"id":"host-1","persistent":true}}"""
					: """{"success":true,"result":[{"id":"host-1"}]}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var lan = new FreeboxLanApi(client.Transport);
		var lanInterface = lan.Interface("main");

		var hosts = await lanInterface.GetHostsAsync();
		await lanInterface.WakeAsync(new WakeOnLanRequest { Mac = "00:11:22:33:44:55" });
		var host = await lanInterface.Host("host-1").UpdateAsync(new LanHostPatch { Persistent = true });

		Assert.Equal("host-1", Assert.Single(hosts).Id);
		Assert.True(host.Persistent);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(3, requests.Length);
		Assert.Equal("/api/v16/lan/browser/main/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Post, requests[1].Method);
		Assert.Equal("/api/v16/lan/wol/main/", requests[1].Address.AbsolutePath);
		using var wakeBody = JsonDocument.Parse(requests[1].Body!);
		Assert.Equal("00:11:22:33:44:55", wakeBody.RootElement.GetProperty("mac").GetString());
		Assert.Equal(HttpMethod.Put, requests[2].Method);
		Assert.Equal("/api/v16/lan/browser/main/host-1/", requests[2].Address.AbsolutePath);
		using var hostBody = JsonDocument.Parse(requests[2].Body!);
		Assert.True(hostBody.RootElement.GetProperty("persistent").GetBoolean());
	}

	[Fact]
	public async Task ValidationFailsBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var lan = new FreeboxLanApi(client.Transport);

		await Assert.ThrowsAsync<ArgumentException>(() => lan.UpdateConfigurationAsync(
			new LanConfigurationPatch { Ip = Optional<string>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(() => lan.Interface("main")
			.WakeAsync(new WakeOnLanRequest { Mac = Optional<string>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(() => lan.Interface("main").Host("host-1")
			.UpdateAsync(new LanHostPatch { HostType = Optional<string>.Null }));
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
			new FreeboxLanApi(client.Transport).GetConfigurationAsync());

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

		var pending = new FreeboxLanApi(client.Transport).GetInterfacesAsync(cancellation.Token);
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
