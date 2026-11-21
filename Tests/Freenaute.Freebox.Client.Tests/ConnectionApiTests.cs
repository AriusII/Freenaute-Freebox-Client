namespace Freenaute.Freebox.Client.Tests;

public sealed class ConnectionApiTests
{
	[Fact]
	public async Task GetStatusUsesGetOnConnectionRootWithSessionToken()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"state":"up","type":"ftth","media":"ftth"}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var connection = new FreeboxConnectionApi(client.Transport);

		var status = await connection.GetStatusAsync();

		Assert.Equal("up", status.State);
		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal(HttpMethod.Get, request.Method);
		Assert.Equal("/api/v16/connection/", request.Address.AbsolutePath);
		Assert.Equal("fixture-session", request.SessionToken);
	}

	[Fact]
	public async Task ConfigurationRoundTripUsesConfigRouteAndPatchKeepsExplicitFalse()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"ping":false,"sip_alg":"direct_media"}}"""
				: """{"success":true,"result":{"ping":true}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var connection = new FreeboxConnectionApi(client.Transport);

		await connection.GetConfigurationAsync();
		var updated = await connection.UpdateConfigurationAsync(new ConnectionConfigurationPatch
		{
			Ping = false, RemoteAccessPort = 0, SipAlg = "direct_media"
		});

		Assert.False(updated.Ping);
		Assert.Equal("direct_media", updated.SipAlg);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/connection/config/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/connection/config/", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.False(body.RootElement.GetProperty("ping").GetBoolean());
		Assert.Equal(0, body.RootElement.GetProperty("remote_access_port").GetInt64());
		Assert.Equal("direct_media", body.RootElement.GetProperty("sip_alg").GetString());
		Assert.Equal(3, body.RootElement.EnumerateObject().Count());
	}

	[Fact]
	public async Task Ipv6ConfigurationRequiresExactlyEightDelegationsAndEncodesThem()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"ipv6_enabled":true,"ipv6_firewall":false}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var connection = new FreeboxConnectionApi(client.Transport);

		var config = await connection.GetIpv6ConfigurationAsync();
		var updated = await connection.UpdateIpv6ConfigurationAsync(new ConnectionIpv6ConfigurationPatch
		{
			Ipv6Enabled = true,
			Delegations = Enumerable.Repeat(new ConnectionIpv6DelegationPatch { Prefix = "2001:db8::/64" }, 8)
				.ToArray()
		});

		Assert.True(config.Ipv6Enabled);
		Assert.False(updated.Ipv6Firewall);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/connection/ipv6/config/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/connection/ipv6/config/", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.Equal(8, body.RootElement.GetProperty("delegations").GetArrayLength());
	}

	[Fact]
	public async Task AggregationUsesSlashlessRouteAndReturnsResult()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true}"""
				: """{"success":true,"result":{"enabled":true}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var connection = new FreeboxConnectionApi(client.Transport);

		var result = await connection.GetAggregationAsync();
		await connection.UpdateAggregationAsync(new LteAggregationUpdate { Enabled = false });

		Assert.True(result.Enabled);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal("/api/v16/connection/aggregation", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/connection/aggregation", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.False(body.RootElement.GetProperty("enabled").GetBoolean());
	}

	[Fact]
	public async Task ValidationFailsBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var connection = new FreeboxConnectionApi(client.Transport);

		await Assert.ThrowsAsync<ArgumentException>(() => connection.UpdateConfigurationAsync(
			new ConnectionConfigurationPatch { Ping = Optional<bool>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(() => connection.UpdateIpv6ConfigurationAsync(
			new ConnectionIpv6ConfigurationPatch
			{
				Delegations = Enumerable.Repeat(new ConnectionIpv6DelegationPatch(), 7).ToArray()
			}));
		await Assert.ThrowsAsync<ArgumentException>(() => connection.UpdateAggregationAsync(
			new LteAggregationUpdate { Enabled = Optional<bool>.Null }));
		Assert.Empty(ResourceRequests(handler));
	}

	[Fact]
	public async Task ApiErrorIsMappedToFreeboxApiException()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":false,"error_code":"invalid_request","msg":"Fixture failure","uid":"fixture-error"}""",
			HttpStatusCode.BadRequest);
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var error = await Assert.ThrowsAsync<FreeboxApiException>(() =>
			new FreeboxConnectionApi(client.Transport).GetStatusAsync());

		Assert.Equal(HttpStatusCode.BadRequest, error.StatusCode);
		Assert.Equal("invalid_request", error.ErrorCode);
		Assert.Equal("Fixture failure", error.ApiMessage);
		Assert.Equal("fixture-error", error.ErrorUid);
		Assert.Single(ResourceRequests(handler));
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

		var pending = new FreeboxConnectionApi(client.Transport).GetStatusAsync(cancellation.Token);
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
