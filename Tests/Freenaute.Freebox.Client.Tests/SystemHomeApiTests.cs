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
		bool[] values = [.. Enumerable.Repeat(element: false, 7 * 48)];
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

	[Fact]
	public async Task SystemRoutesGetRebootAndShutdown()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Get
				? """{"success":true,"result":{"firmware_version":"4.9","box_flavor":"full","temp_cpub":55,"temp_cpum":54,"temp_sw":45,"fan_rpm":1200}}"""
				: """{"success":true}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var system = new FreeboxSystemHomeApi(client.Transport).System;

		var config = await system.GetAsync();
		await system.RebootAsync();
		await system.ShutdownAsync();

		Assert.Equal("4.9", config.FirmwareVersion);
		Assert.Equal("full", config.BoxFlavor);
		Assert.Equal(55, config.TempCpub);
		Assert.Equal(54, config.TempCpum);
		Assert.Equal(45, config.TempSw);
		Assert.Equal(1200, config.FanRpm);
		var requests = handler.Requests.Where(request =>
			request.Address.AbsolutePath is not "/api_version" &&
			!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
			!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal)).ToArray();
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/system/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Post, requests[1].Method);
		Assert.Equal("/api/v16/system/reboot/", requests[1].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Post, requests[2].Method);
		Assert.Equal("/api/v16/system/shutdown/", requests[2].Address.AbsolutePath);
	}

	[Fact]
	public async Task UpdateStatusUsesUpdateRoute()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"state":"up_to_date","upgrade_state":{"state":"commit","old_version":"4.8","new_version":"4.9","percent":100,"error_string":""}}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var status = await new FreeboxSystemHomeApi(client.Transport).Updates.GetAsync();

		Assert.Equal("up_to_date", status.State!.Value.Value);
		Assert.Equal("commit", status.UpgradeState!.State!.Value.Value);
		var request = Assert.Single(handler.Requests, request =>
			request.Address.AbsolutePath is not "/api_version" &&
			!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
			!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal));
		Assert.Equal(HttpMethod.Get, request.Method);
		Assert.Equal("/api/v16/update/", request.Address.AbsolutePath);
	}

	[Fact]
	public async Task NetworkControlRulesSupportGetCreateUpdateAndDelete()
	{
		using var handler = FixtureHandler.Authenticated((request, _) =>
		{
			string result = request.Address.AbsolutePath.EndsWith("/rules", StringComparison.Ordinal)
							&& request.Method == HttpMethod.Get
				? """{"success":true,"result":[]}"""
				: request.Method == HttpMethod.Put
					? """{"success":true,"result":{"id":5,"profile_id":3,"name":"fixture","mode":"allowed","start_time":0,"end_time":3600,"weekdays":[true,false,true,false,true,false,false],"enabled":false}}"""
					: request.Method == HttpMethod.Delete
						? """{"success":true}"""
						: """{"success":true,"result":{"id":5,"profile_id":3,"name":"fixture","mode":"allowed","start_time":0,"end_time":3600,"weekdays":[true,false,true,false,true,false,false],"enabled":true}}""";
			return Task.FromResult(FixtureHandler.Json(result));
		});
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var rules = new FreeboxSystemHomeApi(client.Transport).Profiles.For(3).NetworkControl.Rules;

		var listed = await rules.ListAsync();
		var rule = await rules.GetAsync(5);
		var created = await rules.Create().UseFields(
			new NetworkControlRuleUpdate().WithName("fixture").WithMode(NetworkControlMode.Allowed)
				.WithStartTime(0).WithEndTime(3600).WithEnabled(true)).SendAsync();
		var updated = await rules.Update(5).UseFields(
			new NetworkControlRuleUpdate().WithEnabled(false)).SendAsync();
		await rules.DeleteAsync(5);

		Assert.Equal(5, rule.Id);
		Assert.Equal("allowed", created.Mode!.Value.Value);
		Assert.False(updated.Enabled);
		Assert.Empty(listed);
		var requests = handler.Requests.Where(request =>
			request.Address.AbsolutePath is not "/api_version" &&
			!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
			!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal)).ToArray();
		Assert.Equal("/api/v16/network_control/3/rules", requests[0].Address.AbsolutePath);
		Assert.Equal("/api/v16/network_control/3/rules/5", requests[1].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Post, requests[2].Method);
		Assert.Equal("/api/v16/network_control/3/rules", requests[2].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[3].Method);
		Assert.Equal("/api/v16/network_control/3/rules/5", requests[3].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Delete, requests[4].Method);
		using var createBody = JsonDocument.Parse(requests[2].Body!);
		Assert.Equal("fixture", createBody.RootElement.GetProperty("name").GetString());
		Assert.Equal("allowed", createBody.RootElement.GetProperty("mode").GetString());
		using var updateBody = JsonDocument.Parse(requests[3].Body!);
		Assert.False(updateBody.RootElement.GetProperty("enabled").GetBoolean());
	}
}
