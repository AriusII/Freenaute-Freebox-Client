namespace Freenaute.Freebox.Client.Tests;

public sealed class WifiMacFilterWpsApiTests
{
	[Fact]
	public async Task MacFilterCrudUsesFilterIdSegment()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Post
				? """{"success":true,"result":{"id":"filter-1","mac":"00:11:22:33:44:55","type":"blacklist"}}"""
				: request.Method == HttpMethod.Put
					? """{"success":true,"result":{"id":"filter-1","comment":"updated","type":"whitelist"}}"""
					: request.Method == HttpMethod.Delete
						? """{"success":true}"""
						: request.Address.AbsolutePath.EndsWith("/mac_filter/", StringComparison.Ordinal)
							? """{"success":true,"result":[{"id":"filter-1","mac":"00:11:22:33:44:55"}]}"""
							: """{"success":true,"result":{"id":"filter-1","mac":"00:11:22:33:44:55"}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var wifi = new FreeboxWifiApi(client.Transport);

		var filters = await wifi.GetMacFiltersAsync();
		var created = await wifi.CreateMacFilterAsync(new CreateWifiMacFilterRequest
		{
			Mac = "00:11:22:33:44:55", Type = "blacklist", Comment = "fixture"
		});
		var fetched = await wifi.MacFilter("filter-1").GetAsync();
		var updated = await wifi.MacFilter("filter-1")
			.UpdateAsync(new WifiMacFilterPatch { Comment = "updated", Type = "whitelist" });
		await wifi.MacFilter("filter-1").DeleteAsync();

		Assert.Equal("filter-1", Assert.Single(filters).Id);
		Assert.Equal("blacklist", created.Type);
		Assert.Equal("00:11:22:33:44:55", fetched.Mac);
		Assert.Equal("updated", updated.Comment);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(5, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/wifi/mac_filter/", requests[0].Address.AbsolutePath);
		Assert.Equal("fixture-session", requests[0].SessionToken);
		Assert.Equal(HttpMethod.Post, requests[1].Method);
		Assert.Equal("/api/v16/wifi/mac_filter/", requests[1].Address.AbsolutePath);
		using var createBody = JsonDocument.Parse(requests[1].Body!);
		Assert.Equal("00:11:22:33:44:55", createBody.RootElement.GetProperty("mac").GetString());
		Assert.Equal("/api/v16/wifi/mac_filter/filter-1", requests[2].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[2].Method);
		Assert.Equal(HttpMethod.Put, requests[3].Method);
		Assert.Equal("/api/v16/wifi/mac_filter/filter-1", requests[3].Address.AbsolutePath);
		using var updateBody = JsonDocument.Parse(requests[3].Body!);
		Assert.Equal("whitelist", updateBody.RootElement.GetProperty("type").GetString());
		Assert.Equal(HttpMethod.Delete, requests[4].Method);
		Assert.Equal("/api/v16/wifi/mac_filter/filter-1", requests[4].Address.AbsolutePath);
	}

	[Fact]
	public async Task WpsLifecycleUsesConfigStartSessionsAndStopRoutes()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Post &&
			request.Address.AbsolutePath.EndsWith("/start/", StringComparison.Ordinal)
				? """{"success":true,"result":42}"""
				: request.Method == HttpMethod.Post
					? """{"success":true}"""
					: request.Method == HttpMethod.Put
						? """{"success":true,"result":{"enabled":false}}"""
						: request.Method == HttpMethod.Delete
							? """{"success":true}"""
							: request.Address.AbsolutePath.EndsWith("/sessions/", StringComparison.Ordinal)
								? """{"success":true,"result":[{"id":42,"ssid":"fixture-ssid","active":true}]}"""
								: """{"success":true,"result":{"enabled":true}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var wps = new FreeboxWifiApi(client.Transport).Wps;

		var config = await wps.GetConfigurationAsync();
		var updated = await wps.UpdateConfigurationAsync(new WifiWpsConfigurationPatch { Enabled = false });
		long sessionId = await wps.StartAsync(new WifiWpsStartRequest { Bssid = "02:00:00:00:00:08" });
		var sessions = await wps.GetSessionsAsync();
		await wps.StopAsync(new WifiWpsStopRequest { SessionId = sessionId });
		await wps.ClearSessionsAsync();

		Assert.True(config.Enabled);
		Assert.False(updated.Enabled);
		Assert.Equal(42, sessionId);
		Assert.Equal("fixture-ssid", Assert.Single(sessions).Ssid);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(6, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/wifi/wps/config/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/wifi/wps/config/", requests[1].Address.AbsolutePath);
		using var configBody = JsonDocument.Parse(requests[1].Body!);
		Assert.False(configBody.RootElement.GetProperty("enabled").GetBoolean());
		Assert.Equal(HttpMethod.Post, requests[2].Method);
		Assert.Equal("/api/v16/wifi/wps/start/", requests[2].Address.AbsolutePath);
		using var startBody = JsonDocument.Parse(requests[2].Body!);
		Assert.Equal("02:00:00:00:00:08", startBody.RootElement.GetProperty("bssid").GetString());
		Assert.Equal(HttpMethod.Get, requests[3].Method);
		Assert.Equal("/api/v16/wifi/wps/sessions/", requests[3].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Post, requests[4].Method);
		Assert.Equal("/api/v16/wifi/wps/stop/", requests[4].Address.AbsolutePath);
		using var stopBody = JsonDocument.Parse(requests[4].Body!);
		Assert.Equal(42, stopBody.RootElement.GetProperty("session_id").GetInt64());
		Assert.Equal(HttpMethod.Delete, requests[5].Method);
		Assert.Equal("/api/v16/wifi/wps/sessions/", requests[5].Address.AbsolutePath);
	}

	[Fact]
	public async Task ValidationFailsBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var wifi = new FreeboxWifiApi(client.Transport);

		await Assert.ThrowsAsync<ArgumentException>(() => wifi.CreateMacFilterAsync(
			new CreateWifiMacFilterRequest { Mac = Optional<string>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(() => wifi.MacFilter("filter-1")
			.UpdateAsync(new WifiMacFilterPatch { Type = Optional<string>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(() => wifi.Wps.UpdateConfigurationAsync(
			new WifiWpsConfigurationPatch { Enabled = Optional<bool>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(() => wifi.Wps.StartAsync(
			new WifiWpsStartRequest { Bssid = Optional<string>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(() => wifi.Wps.StopAsync(
			new WifiWpsStopRequest { SessionId = Optional<long>.Null }));
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
			new FreeboxWifiApi(client.Transport).Wps.GetSessionsAsync());

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

		var pending = new FreeboxWifiApi(client.Transport).GetMacFiltersAsync(cancellation.Token);
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
