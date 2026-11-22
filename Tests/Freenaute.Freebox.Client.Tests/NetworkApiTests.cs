namespace Freenaute.Freebox.Client.Tests;

public sealed class NetworkApiTests
{
	[Fact]
	public async Task DhcpPatchPreservesExplicitFalseAndStringValuedOptions()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"enabled":false}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var network = new FreeboxNetworkApi(client.Transport);

		var result = await network.Dhcp.UpdateConfigurationAsync(new DhcpConfigurationPatch
		{
			Enabled = false,
			Options = new DhcpOptionWrite[] { new() { Id = "tcp_ttl", Val = "0" } }
		});

		var request = handler.Requests.Last();
		Assert.Equal(HttpMethod.Put, request.Method);
		Assert.Equal("/api/v16/dhcp/config/", request.Address.AbsolutePath);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.False(body.RootElement.GetProperty("enabled").GetBoolean());
		Assert.Equal("0", body.RootElement.GetProperty("options")[0].GetProperty("val").GetString());
		Assert.Equal(2, body.RootElement.EnumerateObject().Count());
		Assert.False(result.Enabled);
	}

	[Fact]
	public async Task AccessPointPatchKeepsNestedFalseAndZeroWithoutOtherConfigurationFields()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"id":0,"config":{"channel_width":"20"},"capabilities":{"2d4g":{"n":true,"future":false}}}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var network = new FreeboxNetworkApi(client.Transport);

		var result = await network.Wifi.AccessPoint(0).UpdateAsync(new WifiAccessPointPatch
		{
			Config = new WifiApConfigurationPatch
			{
				PrimaryChannel = 0,
				Ht = new WifiApHtConfigurationPatch { AcEnabled = false }
			}
		});

		var request = handler.Requests.Last();
		Assert.Equal("/api/v16/wifi/ap/0", request.Address.AbsolutePath);
		using var body = JsonDocument.Parse(request.Body!);
		var configuration = body.RootElement.GetProperty("config");
		Assert.Equal(0, configuration.GetProperty("primary_channel").GetInt64());
		Assert.False(configuration.GetProperty("ht").GetProperty("ac_enabled").GetBoolean());
		Assert.Equal(2, configuration.EnumerateObject().Count());
		Assert.Single(configuration.GetProperty("ht").EnumerateObject());
		Assert.Equal("20", result.Config!.ChannelWidth!.Value.String);
		Assert.False(result.Capabilities!.Band2D4G!.Value.Flags["future"]);
	}

	[Fact]
	public async Task LanSelectorsRetainIndependentEncodedInterfaceAndHostIdentifiers()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"id":"fixture","l2ident":{"id":"00:11:22:33:44:55","type":"mac_address"}}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var network = new FreeboxNetworkApi(client.Transport);
		var lan = network.Lan.Interface("réseau &?");
		var first = lan.Host("host#1");
		var second = lan.Host("host%2");

		Assert.Empty(handler.Requests);
		var host = await first.GetAsync();
		await second.GetAsync();
		var requests = handler.Requests.Where(r => r.Address.AbsolutePath.Contains("/lan/", StringComparison.Ordinal))
			.ToArray();
		Assert.Equal("https://fixture.example/api/v16/lan/browser/r%C3%A9seau%20%26%3F/host%231/",
			requests[0].Address.AbsoluteUri);
		Assert.Equal("https://fixture.example/api/v16/lan/browser/r%C3%A9seau%20%26%3F/host%252/",
			requests[1].Address.AbsoluteUri);
		Assert.Equal(ObjectOrArrayKind.Object, host.L2Ident!.Value.Kind);
		Assert.Equal("mac_address", host.L2Ident.Value.Object.Type);
	}

	[Theory]
	[InlineData("{\"id\":\"02:00:00:00:00:08\",\"phy_id\":0}", StringOrIntegerKind.String, StringOrIntegerKind.Integer)]
	[InlineData("{\"id\":1,\"phy_id\":\"0\"}", StringOrIntegerKind.Integer, StringOrIntegerKind.String)]
	public async Task BssIdentifiersPreserveBothDocumentedKinds(string resultJson, StringOrIntegerKind idKind,
		StringOrIntegerKind phyKind)
	{
		using var handler = FixtureHandler.AuthenticatedConstant("{\"success\":true,\"result\":" + resultJson + "}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var network = new FreeboxNetworkApi(client.Transport);

		var bss = await network.Wifi.Bss("02:00:00:00:00:08").GetAsync();

		Assert.Equal(idKind, bss.Id!.Value.Kind);
		Assert.Equal(phyKind, bss.PhyId!.Value.Kind);
	}

	[Theory]
	[InlineData("{\"state\":\"future_state\"}", ObjectOrArrayKind.Object)]
	[InlineData("[{\"state\":\"future_state\"}]", ObjectOrArrayKind.Array)]
	public async Task GlobalWifiStatePreservesObjectOrArrayAndUnknownTokens(string json, ObjectOrArrayKind kind)
	{
		using var handler = FixtureHandler.AuthenticatedConstant("{\"success\":true,\"result\":" + json + "}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var state = await new FreeboxNetworkApi(client.Transport).Wifi.GetStateAsync();

		Assert.Equal(kind, state.Kind);
		Assert.Equal("future_state", state.GetSingle().State);
		using var roundtrip = JsonDocument.Parse(JsonSerializer.Serialize(state,
			NetworkJsonSerializerContext.Default.ObjectOrArrayWifiGlobalState));
		Assert.Equal(kind is ObjectOrArrayKind.Object ? JsonValueKind.Object : JsonValueKind.Array,
			roundtrip.RootElement.ValueKind);
	}

	[Fact]
	public async Task GuestCreateSendsDirectParametersAndRetainsStringMaxUseCount()
	{
		using var handler =
			FixtureHandler.AuthenticatedConstant(
				"""{"success":true,"result":{"id":11,"params":{"max_use_count":100}}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var result = await new FreeboxNetworkApi(client.Transport).Wifi.GuestAccess.CreateKeyAsync(
			new CreateWifiCustomKeyRequest
			{
				Description = "Guest",
				Key = "fixture-secret",
				MaxUseCount = StringOrInteger.FromString("100"),
				Duration = 0,
				AccessType = "net_only"
			});

		var request = handler.Requests.Last();
		Assert.Equal(HttpMethod.Post, request.Method);
		Assert.Equal("/api/v16/wifi/custom_key/", request.Address.AbsolutePath);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.False(body.RootElement.TryGetProperty("params", out _));
		Assert.Equal("100", body.RootElement.GetProperty("max_use_count").GetString());
		Assert.Equal(0, body.RootElement.GetProperty("duration").GetInt64());
		Assert.Equal(100, result.Params!.MaxUseCount!.Value.Integer);
	}

	[Fact]
	public async Task JsonNullAndDocumentedGuestLimitFailBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var network = new FreeboxNetworkApi(client.Transport);

		await Assert.ThrowsAsync<ArgumentException>(async () => await network.Wifi.UpdateConfigurationAsync(
			new WifiGlobalConfigurationPatch { Enabled = Optional<bool>.Null }));
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await network.Wifi.GuestAccess.CreateKeyAsync(
			new CreateWifiCustomKeyRequest { MaxUseCount = StringOrInteger.FromInteger(128) }));
		Assert.Empty(handler.Requests);
	}

	[Fact]
	public async Task RouteReplacementUsesArrayBodyAndPreservesDocumentedTrailingSlash()
	{
		using var handler =
			FixtureHandler.AuthenticatedConstant(
				"""{"success":true,"result":[{"prefix":"192.168.42.0/24","enabled":false}]}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var network = new FreeboxNetworkApi(client.Transport);

		await network.Lan.ReplaceRoutesAsync([new LanRouteWrite { Prefix = "192.168.42.0/24", Enabled = false }]);

		var request = handler.Requests.Last();
		Assert.Equal("/api/v16/lan/routes/", request.Address.AbsolutePath);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.Equal(JsonValueKind.Array, body.RootElement.ValueKind);
		Assert.False(body.RootElement[0].GetProperty("enabled").GetBoolean());
	}

	[Fact]
	public async Task VpnCoversOverviewPoolUserAndConnectionOperations()
	{
		using var handler = FixtureHandler.Authenticated((request, _) =>
		{
			string json = request.Address.AbsolutePath switch
			{
				"/api/v16/vpn/connection/pptp%202" => """{"success":true}""",
				"/api/v16/vpn/connection/" =>
					"""{"success":true,"result":[{"id":"pptp-2","authenticated":true,"rx_bytes":94,"tx_bytes":95}]}""",
				"/api/v16/vpn/ip_pool/" =>
					"""{"success":true,"result":{"ip_start":"192.168.27.65","ip_end":"192.168.27.95","reservations":[{"login":"alice","ip":"192.168.27.69"}]}}""",
				"/api/v16/vpn/user/alice" =>
					request.Method == HttpMethod.Delete
						? """{"success":true}"""
						: """{"success":true,"result":{"login":"alice","password_set":true,"ip_reservation":"192.168.27.69"}}""",
				"/api/v16/vpn/user/" =>
					request.Method == HttpMethod.Post
						? """{"success":true,"result":{"login":"alice","password_set":true}}"""
						: """{"success":true,"result":[{"login":"alice","password_set":true}]}""",
				_ => """{"success":true}"""
			};
			return Task.FromResult(FixtureHandler.Json(json));
		});
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var vpn = new FreeboxNetworkApi(client.Transport).Vpn;

		var pool = await vpn.GetIpPoolAsync();
		var connections = await vpn.GetConnectionsAsync();
		await vpn.DisconnectConnectionAsync("pptp 2");
		var users = await vpn.GetUsersAsync();
		var created = await vpn.CreateUserAsync(new VPNUser
		{
			Login = "alice",
			Password = "secret123",
			IpReservation = "192.168.27.69"
		});
		var user = await vpn.GetUserAsync("alice");
		var updated = await vpn.UpdateUserAsync("alice", new VPNUser { Password = "newsecret123" });
		await vpn.DeleteUserAsync("alice");

		Assert.Equal("192.168.27.65", pool.IpStart);
		Assert.Equal("192.168.27.69", Assert.Single(pool.Reservations!).Ip);
		Assert.Equal("pptp-2", Assert.Single(connections).Id);
		Assert.Equal("alice", Assert.Single(users).Login);
		Assert.Equal("alice", created.Login);
		Assert.Equal("alice", user.Login);
		Assert.Equal("alice", updated.Login);

		var requests = handler.Requests.Where(request =>
			request.Address.AbsolutePath.StartsWith("/api/v16/vpn/", StringComparison.Ordinal)).ToArray();
		Assert.Equal("/api/v16/vpn/ip_pool/", requests[0].Address.AbsolutePath);
		Assert.Equal("/api/v16/vpn/connection/", requests[1].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Delete, requests[2].Method);
		Assert.Equal("/api/v16/vpn/connection/pptp%202", requests[2].Address.AbsolutePath);
		Assert.Equal("/api/v16/vpn/user/", requests[3].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Post, requests[4].Method);
		Assert.Equal(HttpMethod.Get, requests[5].Method);
		Assert.Equal("/api/v16/vpn/user/alice", requests[5].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[6].Method);
		Assert.Equal("/api/v16/vpn/user/alice", requests[6].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Delete, requests[7].Method);
	}

	[Fact]
	public async Task VpnUserAndOpenVpnUpdatesAcceptRawJsonObjects()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"login":"alice","enabled":true}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var vpn = new FreeboxNetworkApi(client.Transport).Vpn;

		await vpn.CreateUserAsync(JsonDocument.Parse("""{"login":"alice","password":"secret123"}""").RootElement.Clone());
		await vpn.UpdateUserAsync("alice",
			JsonDocument.Parse("""{"password":"newsecret123"}""").RootElement.Clone());
		await vpn.UpdateOpenVpnRoutedConfigurationAsync(
			JsonDocument.Parse("""{"enabled":true,"port":1194}""").RootElement.Clone());

		var requests = handler.Requests.Where(request =>
			request.Address.AbsolutePath.StartsWith("/api/v16/vpn/", StringComparison.Ordinal)).ToArray();
		Assert.Equal(3, requests.Length);
		Assert.Equal(HttpMethod.Post, requests[0].Method);
		Assert.Equal("/api/v16/vpn/user/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/vpn/user/alice", requests[1].Address.AbsolutePath);
		Assert.Equal("/api/v16/vpn/openvpn_routed/config/", requests[2].Address.AbsolutePath);
	}

	[Fact]
	public async Task VpnIdentifierParametersFailValidationBeforeHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var vpn = new FreeboxNetworkApi(client.Transport).Vpn;

		await Assert.ThrowsAsync<ArgumentException>(async () => await vpn.DisconnectConnectionAsync("../pptp"));
		await Assert.ThrowsAsync<ArgumentException>(async () => await vpn.DownloadConfigurationAsync("../", "alice"));
		await Assert.ThrowsAsync<ArgumentException>(async () => await vpn.GetUserAsync(" "));
		await Assert.ThrowsAsync<ArgumentException>(async () =>
			await vpn.UpdateUserAsync(".", new VPNUser()));
		await Assert.ThrowsAsync<ArgumentException>(async () =>
			await vpn.GetConfigurationAsync("openvpn/bridge"));

		Assert.Empty(handler.Requests);
	}

	[Fact]
	public async Task VpnRoutesCoverConnectionsDownloadAndTypedOpenVpnUpdate()
	{
		using var handler = FixtureHandler.Authenticated((request, _) =>
		{
			string json = request.Address.AbsolutePath switch
			{
				"/api/v16/vpn/connection/" =>
					"""{"success":true,"result":[{"id":"connection-1","authenticated":true}]}""",
				"/api/v16/vpn/download_config/openvpn%20routed/alice%40example" =>
					"""{"success":true,"result":{"format":"ovpn","content":"fixture"}}""",
				_ => """{"success":true,"result":{"id":"openvpn_routed","enabled":false,"port":1194}}"""
			};
			return Task.FromResult(FixtureHandler.Json(json));
		});
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var vpn = new FreeboxNetworkApi(client.Transport).Vpn;

		var connections = await vpn.GetConnectionsAsync();
		var download = await vpn.DownloadConfigurationAsync("openvpn routed", "alice@example");
		var updated = await vpn.UpdateOpenVpnRoutedConfigurationAsync(new VPNServerConfig
		{
			Id = "openvpn_routed",
			Enabled = false,
			Port = 1194
		});

		var requests = handler.Requests.Where(request =>
			request.Address.AbsolutePath.StartsWith("/api/v16/vpn/", StringComparison.Ordinal)).ToArray();
		Assert.Single(connections);
		Assert.Equal("connection-1", connections[0].Id);
		Assert.True(connections[0].Authenticated);
		Assert.Equal("ovpn", download.GetProperty("format").GetString());
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/vpn/connection/", requests[0].Address.AbsolutePath);
		Assert.Equal(
			"/api/v16/vpn/download_config/openvpn%20routed/alice%40example",
			requests[1].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[2].Method);
		Assert.Equal("/api/v16/vpn/openvpn_routed/config/", requests[2].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[2].Body!);
		Assert.Equal("openvpn_routed", body.RootElement.GetProperty("id").GetString());
		Assert.False(body.RootElement.GetProperty("enabled").GetBoolean());
		Assert.Equal(1194, body.RootElement.GetProperty("port").GetInt64());
		Assert.Equal("openvpn_routed", updated.Id);
		Assert.False(updated.Enabled);
	}

	[Fact]
	public async Task VpnRouteParametersFailValidationBeforeHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var vpn = new FreeboxNetworkApi(client.Transport).Vpn;

		await Assert.ThrowsAsync<ArgumentException>(async () =>
			await vpn.DownloadConfigurationAsync("../", "alice"));
		await Assert.ThrowsAsync<ArgumentException>(async () =>
			await vpn.GetUserAsync(" "));

		Assert.Empty(handler.Requests);
	}

	[Fact]
	public async Task NetworkOperationPassesCancellationToTransport()
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
		var pending = new FreeboxNetworkApi(client.Transport).Wifi.AccessPoint(0)
			.ScanNeighborsAsync(cancellation.Token);
		await started.Task.WaitAsync(TimeSpan.FromSeconds(5));
		await cancellation.CancelAsync();

		await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await pending);
	}

	[Fact]
	public async Task StorageAndVpnRoutesAreWiredInNetworkAggregate()
	{
		using var handler = FixtureHandler.Authenticated((request, _) =>
		{
			string json = request.Address.AbsolutePath switch
			{
				"/api/v16/storage/disk/" =>
					"""{"success":true,"result":[{"id":1,"name":"fixture"}]}""",
				"/api/v16/vpn/" =>
					"""{"success":true,"result":[{"name":"openvpn","type":"openvpn","state":"started"}]}""",
				"/api/v16/vpn/user/" =>
					"""{"success":true,"result":[{"login":"alice"}]}""",
				"/api/v16/vpn_client/status" =>
					"""{"success":true,"result":{"enabled":true,"state":"up"}}""",
				_ => """{"success":true,"result":{"state":"ok"}}"""
			};
			return Task.FromResult(FixtureHandler.Json(json));
		});
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var network = new FreeboxNetworkApi(client.Transport);

		await network.Storage.GetConfigurationAsync();
		await network.Storage.GetDisksAsync();
		await network.Storage.SetDiskStateAsync(1, new SetStorageDiskStateRequest { State = "enabled" });
		await network.Vpn.GetAsync();
		await network.Vpn.GetUsersAsync();
		await network.VpnClient.GetStatusAsync();

		var requests = handler.Requests.Where(request =>
			request.Address.AbsolutePath is not "/api_version" &&
			!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
			!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal)).ToArray();
		Assert.Equal("/api/v16/storage/config/", requests[0].Address.AbsolutePath);
		Assert.Equal("/api/v16/storage/disk/", requests[1].Address.AbsolutePath);
		Assert.Equal("/api/v16/storage/disk/1", requests[2].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[2].Method);
		Assert.Equal("/api/v16/vpn/", requests[3].Address.AbsolutePath);
		Assert.Equal("/api/v16/vpn/user/", requests[4].Address.AbsolutePath);
		Assert.Equal("/api/v16/vpn_client/status", requests[5].Address.AbsolutePath);
	}

	[Theory]
	[InlineData("true")]
	[InlineData("1.5")]
	[InlineData("{}")]
	public void NatPortUnionRejectsUnestablishedJsonKinds(string value)
	{
		Assert.Throws<JsonException>(() => JsonSerializer.Deserialize(
			"{\"wan_port_start\":" + value + "}", NetworkJsonSerializerContext.Default.PortForwardingConfig));
	}

	[Fact]
	public void SourceGeneratedContractsIgnoreDeprecatedBssFlagsAndRetainCurrentReplacement()
	{
		var bss = JsonSerializer.Deserialize(
			"""{"use_shared_params":false,"status":{"is_main_bss":true},"config":{"use_default_config":true,"hide_ssid":false}}""",
			NetworkJsonSerializerContext.Default.WifiBss)!;
		using var serialized =
			JsonDocument.Parse(JsonSerializer.Serialize(bss, NetworkJsonSerializerContext.Default.WifiBss));

		Assert.False(bss.UseSharedParams);
		Assert.False(bss.Config!.HideSsid!.Value.Boolean);
		Assert.False(serialized.RootElement.GetProperty("status").TryGetProperty("is_main_bss", out _));
		Assert.False(serialized.RootElement.GetProperty("config").TryGetProperty("use_default_config", out _));
	}

	[Theory]
	[InlineData("[1,2]")]
	[InlineData("{\"n\":1}")]
	public void WifiCapabilitiesRejectShapesBeyondDocumentedIntegerOrBooleanMap(string value)
	{
		Assert.Throws<JsonException>(() => JsonSerializer.Deserialize(
			"{\"2d4g\":" + value + "}", NetworkJsonSerializerContext.Default.WifiApCapabilities));
	}

	[Fact]
	public void UnstableWifiHeAndHtFieldsAreIgnoredAndAcRemainsAvailable()
	{
		var config = JsonSerializer.Deserialize(
			"{\"he\":{\"enabled\":true},\"ht\":{\"ac_enabled\":true,\"ht_enabled\":true}}",
			NetworkJsonSerializerContext.Default.WifiApConfig)!;
		using var readJson = JsonDocument.Parse(JsonSerializer.Serialize(config,
			NetworkJsonSerializerContext.Default.WifiApConfig));
		Assert.True(config.Ht!.AcEnabled);
		Assert.False(readJson.RootElement.TryGetProperty("he", out _));
		Assert.False(readJson.RootElement.GetProperty("ht").TryGetProperty("ht_enabled", out _));
		Assert.Null(typeof(WifiApHtConfig).GetProperty("HtEnabled"));
		Assert.DoesNotContain(typeof(WifiApHtConfig).GetProperties(),
			property => Attribute.IsDefined(property, typeof(ObsoleteAttribute)));

		using var patchJson = JsonDocument.Parse(JsonSerializer.Serialize(
			new WifiApConfigurationPatch { Ht = new WifiApHtConfigurationPatch { AcEnabled = false } },
			NetworkJsonSerializerContext.Default.WifiApConfigurationPatch));
		Assert.False(patchJson.RootElement.TryGetProperty("he", out _));
		Assert.False(patchJson.RootElement.GetProperty("ht").TryGetProperty("ht_enabled", out _));
		Assert.False(patchJson.RootElement.GetProperty("ht").GetProperty("ac_enabled").GetBoolean());
	}
}
