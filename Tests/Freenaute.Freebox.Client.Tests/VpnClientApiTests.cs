namespace Freenaute.Freebox.Client.Tests;

public sealed class VpnClientApiTests
{
	[Fact]
	public async Task ListAndCreateUseDocumentedConfigCollectionRoute()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Post
				? """{"success":true,"result":{"id":"vpn1","description":"Fixture VPN","type":"pptp","active":false}}"""
				: """{"success":true,"result":[{"id":"vpn0","description":"Fixture VPN","type":"pptp","active":true}]}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var vpnClient = new FreeboxNetworkApi(client.Transport).VpnClient;

		var list = await vpnClient.GetConfigurationsAsync();
		var created = await vpnClient.CreateConfigurationAsync(new VPNClientConfig
		{
			Description = "Fixture VPN",
			Type = "pptp",
			Active = false
		});

		Assert.Single(list);
		Assert.Equal("vpn0", list[0].Id);
		Assert.Equal("vpn1", created.Id);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/vpn_client/config/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Post, requests[1].Method);
		Assert.Equal("/api/v16/vpn_client/config/", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.Equal("Fixture VPN", body.RootElement.GetProperty("description").GetString());
		Assert.Equal("pptp", body.RootElement.GetProperty("type").GetString());
		Assert.False(body.RootElement.GetProperty("active").GetBoolean());
	}

	[Fact]
	public async Task ItemOperationsUseStringIdentifiersAndEncodedSegment()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"id":"vpn%201","description":"Fixture VPN","type":"pptp","active":false}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var vpnClient = new FreeboxNetworkApi(client.Transport).VpnClient;

		await vpnClient.GetConfigurationAsync("vpn 1");
		await vpnClient.UpdateConfigurationAsync("vpn 1", new VPNClientConfig { Active = false });
		await vpnClient.DeleteConfigurationAsync("vpn 1");

		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(3, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/vpn_client/config/vpn%201", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/vpn_client/config/vpn%201", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.False(body.RootElement.GetProperty("active").GetBoolean());
		Assert.Equal(HttpMethod.Delete, requests[2].Method);
		Assert.Equal("/api/v16/vpn_client/config/vpn%201", requests[2].Address.AbsolutePath);
	}

	[Fact]
	public async Task InvalidIdentifierIsRejectedBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var vpnClient = new FreeboxNetworkApi(client.Transport).VpnClient;

		await Assert.ThrowsAsync<ArgumentException>(async () => await vpnClient.GetConfigurationAsync("../vpn0"));
		await Assert.ThrowsAsync<ArgumentException>(async () =>
			await vpnClient.UpdateConfigurationAsync("vpn/0", new VPNClientConfig()));
		await Assert.ThrowsAsync<ArgumentException>(async () => await vpnClient.DeleteConfigurationAsync("."));
		Assert.Empty(ResourceRequests(handler));
	}

	[Fact]
	public async Task NullConfigurationPayloadsAreRejectedBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var vpnClient = new FreeboxNetworkApi(client.Transport).VpnClient;

		await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			await vpnClient.CreateConfigurationAsync(null!));
		await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			await vpnClient.UpdateConfigurationAsync("vpn0", null!));
		Assert.Empty(ResourceRequests(handler));
	}

	[Fact]
	public async Task GetLogReturnsPlainString()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":"2014-02-20 14:55:10 dbg: ppp"}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var vpnClient = new FreeboxNetworkApi(client.Transport).VpnClient;

		string log = await vpnClient.GetLogAsync();

		Assert.Equal("2014-02-20 14:55:10 dbg: ppp", log);
		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal("/api/v16/vpn_client/log", request.Address.AbsolutePath);
	}

	[Fact]
	public async Task StatusPreservesRoutesWhenApiReturnsObjectShape()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"enabled":true,"type":"pptp","last_error":"none","active_vpn":"vpn0","active_vpn_description":"Fixture VPN","last_try":1,"state":"up","next_try":0,"last_up":2,"stats":{"rate_up":0,"rate_down":1,"bytes_up":2,"bytes_down":3},"ipv4":{"routes":{},"config_valid":true,"ip_mask":{"ip":"192.168.27.65","mask":"255.255.255.255"},"provider":"ppp","dhcp":{"state":"down"},"dns":["212.27.38.253"],"domain":"","gateway":"212.27.38.253"}}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var vpnClient = new FreeboxNetworkApi(client.Transport).VpnClient;

		var status = await vpnClient.GetStatusAsync();

		Assert.True(status.Enabled);
		Assert.Equal(3, status.Stats!.BytesDown);
		Assert.Equal(JsonValueKind.Object, status.Ipv4!.Routes.ValueKind);
		using var serialized = JsonDocument.Parse(JsonSerializer.Serialize(status,
			NetworkJsonSerializerContext.Default.VPNClientStatus));
		Assert.Equal(JsonValueKind.Object,
			serialized.RootElement.GetProperty("ipv4").GetProperty("routes").ValueKind);
	}

	private static IEnumerable<CapturedRequest> ResourceRequests(FixtureHandler handler)
	{
		return handler.Requests.Where(request =>
			request.Address.AbsolutePath is not "/api_version" &&
			!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
			!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal));
	}
}
