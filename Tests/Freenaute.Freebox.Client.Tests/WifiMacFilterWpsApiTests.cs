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
			Mac = "00:11:22:33:44:55",
			Type = "blacklist",
			Comment = "fixture"
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
	public async Task WifiDomainRoutesAndMloLifecycleUseExpectedPaths()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(
			request.Address.AbsolutePath.EndsWith("/state/", StringComparison.Ordinal)
				? FixtureHandler.Json("""{"success":true,"result":{"state":"enabled"}}""")
				: request.Address.AbsolutePath.EndsWith("/stations/", StringComparison.Ordinal)
					? FixtureHandler.Json(
						"""{"success":true,"result":[{"id":"aa:bb:cc:dd:ee:ff","mac":"aa:bb:cc:dd:ee:ff"}]}""")
					: request.Address.AbsolutePath.Contains("/stations/", StringComparison.Ordinal)
						? FixtureHandler.Json(
							"""{"success":true,"result":{"id":"aa:bb:cc:dd:ee:ff","mac":"aa:bb:cc:dd:ee:ff"}}""")
						: request.Address.AbsolutePath.Contains("/allowed_channel_comb", StringComparison.Ordinal)
							? FixtureHandler.Json(
								"""{"success":true,"result":[{"band":"5g","channel_width":"80mhz","need_dfs":false,"primary":36,"secondary":40}]}""")
							: request.Address.AbsolutePath.Contains("/channel_survey_history/",
								StringComparison.Ordinal)
								? FixtureHandler.Json("""{"success":true,"result":[]}""")
								: request.Address.AbsolutePath.EndsWith("/diag", StringComparison.Ordinal)
									? FixtureHandler.Json("""{"success":true,"result":[]}""")
									: request.Address.AbsolutePath.Contains("/mlo/allowed_comb",
										StringComparison.Ordinal)
										? FixtureHandler.Json("""{"success":true,"result":[[1,2],[1]]}""")
										: request.Address.AbsolutePath.EndsWith("/mlo/config", StringComparison.Ordinal)
											? FixtureHandler.Json("""{"success":true,"result":{"partners":[1,2]}}""")
											: FixtureHandler.Json("""{"success":true,"result":{}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var wifi = new FreeboxWifiApi(client.Transport);

		await wifi.GetSteeringAsync();
		await wifi.UpdateSteeringAsync(new WifiSteeringConfigurationPatch { SteeringLevel = 2 });
		await wifi.GetStateAsync();
		await wifi.GetPlanningAsync();
		await wifi.UpdatePlanningAsync(new WifiPlanningPatch
		{
			UsePlanning = true,
			Mapping = Optional<string[]>.FromValue(["on", "off"])
		});
		await wifi.GetTemporaryDisableAsync();
		await wifi.TemporarilyDisableAsync(new TemporaryWifiDisableRequest { Duration = 300, Keep = "5g" });
		await wifi.AccessPoint(1).GetStationsAsync();
		await wifi.AccessPoint(1).Station("aa:bb:cc:dd:ee:ff").GetAsync();
		await wifi.AccessPoint(1).GetAllowedChannelsAsync();
		await wifi.AccessPoint(1).GetSurveyHistoryAsync(1700000000);
		await wifi.AccessPoint(1).ScanNeighborsAsync();
		await wifi.AccessPoint(1).GetDefaultsAsync();
		await wifi.AccessPoint(1).GetDiagnosticsAsync();
		await wifi.Bss("aa:bb:cc:dd:ee:ff").GetAllowedMloPartnersAsync();
		await wifi.Bss("aa:bb:cc:dd:ee:ff").GetMloConfigurationAsync();

		var requests = ResourceRequests(handler).ToArray();
		Assert.Contains(requests, request =>
			request.Address.AbsolutePath == "/api/v16/wifi/steering/config/");
		Assert.Contains(requests, request =>
			request.Address.AbsolutePath == "/api/v16/wifi/planning/" &&
			request.Method == HttpMethod.Put);
		Assert.Contains(requests, request =>
			request.Address.AbsolutePath.StartsWith("/api/v16/wifi/temp_disable", StringComparison.Ordinal));
		Assert.Contains(requests, request =>
			request.Address.AbsolutePath.Contains("/api/v16/wifi/ap/1/stations/", StringComparison.Ordinal) &&
			request.Method == HttpMethod.Get);
		Assert.Contains(requests, request =>
			request.Address.AbsolutePath.Contains("/api/v16/wifi/ap/1/channel_survey_history/",
				StringComparison.Ordinal));
		Assert.Contains(requests, request =>
			request.Address.AbsolutePath.Contains("/api/v16/wifi/bss/", StringComparison.Ordinal) &&
			request.Address.AbsolutePath.EndsWith("/mlo/allowed_comb", StringComparison.Ordinal));
		Assert.Contains(requests, request =>
			request.Address.AbsolutePath.Contains("/api/v16/wifi/bss/", StringComparison.Ordinal) &&
			request.Address.AbsolutePath.EndsWith("/mlo/config", StringComparison.Ordinal));
	}

	[Fact]
	public async Task ValidationFailsBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var wifi = new FreeboxWifiApi(client.Transport);

		await Assert.ThrowsAsync<ArgumentException>(async () => await wifi.CreateMacFilterAsync(
			new CreateWifiMacFilterRequest { Mac = Optional<string>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(async () => await wifi.MacFilter("filter-1")
			.UpdateAsync(new WifiMacFilterPatch { Type = Optional<string>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(async () => await wifi.Wps.UpdateConfigurationAsync(
			new WifiWpsConfigurationPatch { Enabled = Optional<bool>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(async () => await wifi.Wps.StartAsync(
			new WifiWpsStartRequest { Bssid = Optional<string>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(async () => await wifi.Wps.StopAsync(
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

		var error = await Assert.ThrowsAsync<FreeboxApiException>(async () =>
			await new FreeboxWifiApi(client.Transport).Wps.GetSessionsAsync());

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
