namespace Freenaute.Freebox.Client.Tests;

public sealed class SystemHomeApiTests
{
	[Fact]
	public async Task LcdPatchSendsFalseAndZeroWhileOmittingUnsetAndReadonlyFields()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"brightness":0}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var api = new FreeboxSystemHomeApi(client.Transport);
		await api.Lcd.Configure().UseFields(new LcdConfigPatch().WithBrightness(0).WithOrientationForced(false))
			.SendAsync();
		var request = handler.Requests.Last();
		Assert.Equal(HttpMethod.Put, request.Method);
		Assert.Equal("/api/v16/lcd/config/", request.Address.AbsolutePath);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.Equal(0, body.RootElement.GetProperty("brightness").GetInt32());
		Assert.False(body.RootElement.GetProperty("orientation_forced").GetBoolean());
		Assert.Equal(2, body.RootElement.EnumerateObject().Count());
		Assert.False(body.RootElement.TryGetProperty("available_led_strip_animations", out _));
		Assert.False(body.RootElement.TryGetProperty("hide_led", out _));
	}

	[Fact]
	public async Task UpdatingACommandLeavesItsOriginalFieldsUnchanged()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var original = new FreeboxSystemHomeApi(client.Transport).Lcd.WithBrightness(50);
		var modified = original.UseFields(original.Fields.WithHideWifiKey(false));
		Assert.Empty(handler.Requests);
		await modified.SendAsync();
		using var modifiedBody = JsonDocument.Parse(handler.Requests.Last().Body!);
		Assert.False(modifiedBody.RootElement.GetProperty("hide_wifi_key").GetBoolean());
		await original.SendAsync();
		using var originalBody = JsonDocument.Parse(handler.Requests.Last().Body!);
		Assert.False(originalBody.RootElement.TryGetProperty("hide_wifi_key", out _));
	}

	[Fact]
	public void PatchNullIsRejectedBeforeNetworkWhenNoClearSemanticsAreDocumented()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var command = new FreeboxSystemHomeApi(client.Transport).Lcd.Configure()
			.UseFields(new LcdConfigPatch { HideWifiKey = Optional<bool>.Null });
		Assert.Throws<ArgumentException>(() => { _ = command.SendAsync(); });
		Assert.Empty(handler.Requests);
	}

	[Fact]
	public async Task PlanningRequestSnapshotsMappingAndExcludesReadonlyResolution()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"resolution":48}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		bool[] values = Enumerable.Repeat(false, 7 * 48).ToArray();
		var fields = new StandbyConfigUpdate().WithUsePlanning(false).WithMapping(values);
		values[0] = true;
		await new FreeboxSystemHomeApi(client.Transport).Standby.Configure().UseFields(fields).SendAsync();
		var request = handler.Requests.Last();
		Assert.Equal("/api/v16/standby/config", request.Address.AbsolutePath);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.False(body.RootElement.GetProperty("use_planning").GetBoolean());
		Assert.False(body.RootElement.GetProperty("mapping")[0].GetBoolean());
		Assert.False(body.RootElement.TryGetProperty("resolution", out _));
	}

	[Fact]
	public void UnknownStandbyModeAndDistinctProfileKeysArePreserved()
	{
		var standby = JsonSerializer.Deserialize("""{"planning_mode":"suspend"}""",
			SystemHomeJsonSerializerContext.Default.StandbyConfig)!;
		Assert.Equal("suspend", standby.PlanningMode!.Value.Value);
		Assert.False(standby.PlanningMode.Value.IsKnown);
		var profile = JsonSerializer.Deserialize("""{"icon":"declared","url":"example"}""",
			SystemHomeJsonSerializerContext.Default.Profile)!;
		Assert.Equal("declared", profile.Icon);
		Assert.Equal("example", profile.Url);
		using var serialized =
			JsonDocument.Parse(JsonSerializer.Serialize(profile, SystemHomeJsonSerializerContext.Default.Profile));
		Assert.Equal("declared", serialized.RootElement.GetProperty("icon").GetString());
		Assert.Equal("example", serialized.RootElement.GetProperty("url").GetString());
	}

	[Theory]
	[InlineData("[\"3DS-Thibault\"]", NetworkControlHostsKind.Names)]
	[InlineData("[{\"id\":\"ether-host\"}]", NetworkControlHostsKind.Hosts)]
	[InlineData("[]", NetworkControlHostsKind.Empty)]
	public void NetworkControlHostsPreserveNamesOrHostObjects(string wire, NetworkControlHostsKind kind)
	{
		var hosts = JsonSerializer.Deserialize(wire, SystemHomeJsonSerializerContext.Default.NetworkControlHosts)!;
		Assert.Equal(kind, hosts.Kind);
		var restored = JsonSerializer.Deserialize(JsonSerializer.Serialize(hosts,
				SystemHomeJsonSerializerContext.Default.NetworkControlHosts),
			SystemHomeJsonSerializerContext.Default.NetworkControlHosts)!;
		Assert.Equal(kind, restored.Kind);
	}

	[Fact]
	public void HostUnionRejectsHeterogeneousArrays()
	{
		Assert.Throws<JsonException>(() => JsonSerializer.Deserialize("""["name",{"id":"host"}]""",
			SystemHomeJsonSerializerContext.Default.NetworkControlHosts));
	}

	[Fact]
	public void ModernSystemSensorsAndExpansionModulesRemainSeparate()
	{
		var system = JsonSerializer.Deserialize(
			"""{"firmware_version":"4.9","sensors":[{"id":"temp_sw","value":38}],"expansions":[{"slot":1,"present":true}],"model_info":{"name":"fbxgw9-r1/full","has_lan_sfp":true}}""",
			SystemHomeJsonSerializerContext.Default.SystemConfig)!;
		Assert.Equal(38, system.Sensors!.Value[0].Value);
		Assert.True(system.Expansions!.Value[0].Present);
		Assert.True(system.ModelInfo!.HasLanSfp);
	}
}
