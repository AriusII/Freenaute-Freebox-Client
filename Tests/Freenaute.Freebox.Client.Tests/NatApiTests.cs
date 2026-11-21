namespace Freenaute.Freebox.Client.Tests;

public sealed class NatApiTests
{
	[Fact]
	public async Task DmzRoundTripUsesFwDmzRouteAndKeepsExplicitFalse()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"ip":"192.168.0.10","enabled":false}}"""
				: """{"success":true,"result":{"ip":"","enabled":true}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var nat = new FreeboxNatApi(client.Transport);

		var dmz = await nat.GetDmzAsync();
		var updated = await nat.UpdateDmzAsync(new DmzConfigurationPatch { Ip = "192.168.0.10", Enabled = false });

		Assert.True(dmz.Enabled);
		Assert.False(updated.Enabled);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/fw/dmz/", requests[0].Address.AbsolutePath);
		Assert.Equal("fixture-session", requests[0].SessionToken);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/fw/dmz/", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.Equal("192.168.0.10", body.RootElement.GetProperty("ip").GetString());
		Assert.False(body.RootElement.GetProperty("enabled").GetBoolean());
	}

	[Fact]
	public async Task ForwardingCrudUsesRedirRoutesWithNumericId()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Post
				? """{"success":true,"result":{"id":7,"enabled":true,"ip_proto":"tcp","wan_port_start":8080,"lan_ip":"192.168.0.20","lan_port":80}}"""
				: request.Method == HttpMethod.Put
					? """{"success":true,"result":{"id":7,"enabled":false}}"""
					: request.Method == HttpMethod.Delete
						? """{"success":true}"""
						: request.Address.AbsolutePath.EndsWith("/redir/", StringComparison.Ordinal)
							? """{"success":true,"result":[{"id":7,"enabled":true}]}"""
							: """{"success":true,"result":{"id":7,"enabled":true,"comment":"fixture"}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var nat = new FreeboxNatApi(client.Transport);

		var forwardings = await nat.GetForwardingsAsync();
		var created = await nat.CreateForwardingAsync(new CreatePortForwardingRequest
		{
			Enabled = true,
			IpProto = "tcp",
			WanPortStart = StringOrInteger.FromInteger(8080),
			WanPortEnd = 8080,
			LanIp = "192.168.0.20",
			LanPort = 80
		});
		var fetched = await nat.Forwarding(7).GetAsync();
		var updated = await nat.Forwarding(7).UpdateAsync(new PortForwardingPatch { Enabled = false });
		await nat.Forwarding(7).DeleteAsync();

		Assert.Equal(7, Assert.Single(forwardings).Id);
		Assert.Equal(7, created.Id);
		Assert.Equal(8080, created.WanPortStart!.Value.Integer);
		Assert.Equal("fixture", fetched.Comment);
		Assert.False(updated.Enabled);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(5, requests.Length);
		Assert.Equal("/api/v16/fw/redir/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal(HttpMethod.Post, requests[1].Method);
		Assert.Equal("/api/v16/fw/redir/", requests[1].Address.AbsolutePath);
		using var createBody = JsonDocument.Parse(requests[1].Body!);
		Assert.Equal(8080, createBody.RootElement.GetProperty("wan_port_start").GetInt64());
		Assert.Equal("tcp", createBody.RootElement.GetProperty("ip_proto").GetString());
		Assert.Equal("/api/v16/fw/redir/7", requests[2].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[2].Method);
		Assert.Equal(HttpMethod.Put, requests[3].Method);
		Assert.Equal("/api/v16/fw/redir/7", requests[3].Address.AbsolutePath);
		using var updateBody = JsonDocument.Parse(requests[3].Body!);
		Assert.False(updateBody.RootElement.GetProperty("enabled").GetBoolean());
		Assert.Equal(HttpMethod.Delete, requests[4].Method);
		Assert.Equal("/api/v16/fw/redir/7", requests[4].Address.AbsolutePath);
	}

	[Fact]
	public async Task IncomingPortsUseFwIncomingRoutesWithStableId()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"id":"http","enabled":false,"in_port":0}}"""
				: request.Address.AbsolutePath.EndsWith("/incoming/", StringComparison.Ordinal)
					? """{"success":true,"result":[{"id":"http","enabled":true,"in_port":80}]}"""
					: """{"success":true,"result":{"id":"http","enabled":true,"active":true,"in_port":80}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var nat = new FreeboxNatApi(client.Transport);

		var incoming = await nat.GetIncomingPortsAsync();
		var fetched = await nat.IncomingPort("http").GetAsync();
		var updated = await nat.IncomingPort("http").UpdateAsync(new IncomingPortPatch { Enabled = false, InPort = 0 });

		Assert.Equal(80, Assert.Single(incoming).InPort);
		Assert.True(fetched.Active);
		Assert.False(updated.Enabled);
		Assert.Equal(0, updated.InPort);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(3, requests.Length);
		Assert.Equal("/api/v16/fw/incoming/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[1].Method);
		Assert.Equal("/api/v16/fw/incoming/http", requests[1].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[2].Method);
		Assert.Equal("/api/v16/fw/incoming/http", requests[2].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[2].Body!);
		Assert.False(body.RootElement.GetProperty("enabled").GetBoolean());
		Assert.Equal(0, body.RootElement.GetProperty("in_port").GetInt64());
	}

	[Fact]
	public async Task ValidationFailsBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var nat = new FreeboxNatApi(client.Transport);

		await Assert.ThrowsAsync<ArgumentException>(() => nat.UpdateDmzAsync(
			new DmzConfigurationPatch { Enabled = Optional<bool>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(() => nat.CreateForwardingAsync(
			new CreatePortForwardingRequest { LanIp = Optional<string>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(() => nat.Forwarding(7)
			.UpdateAsync(new PortForwardingPatch { Comment = Optional<string>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(() => nat.IncomingPort("http")
			.UpdateAsync(new IncomingPortPatch { InPort = Optional<long>.Null }));
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
			new FreeboxNatApi(client.Transport).GetDmzAsync());

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

		var pending = new FreeboxNatApi(client.Transport).GetForwardingsAsync(cancellation.Token);
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
