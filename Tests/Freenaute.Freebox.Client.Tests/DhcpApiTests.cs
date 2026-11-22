namespace Freenaute.Freebox.Client.Tests;

public sealed class DhcpApiTests
{
	[Fact]
	public async Task ConfigurationAndLeaseRoutesCoverDhcpDomain()
	{
		using var handler = FixtureHandler.Authenticated((request, _) =>
		{
			string json = request.Method == HttpMethod.Get
				? request.Address.AbsolutePath switch
				{
					"/api/v16/dhcp/config/" =>
						"""{"success":true,"result":{"enabled":true,"sticky_assign":false,"ip_range_start":"192.168.1.10","ip_range_end":"192.168.1.200"}}""",
					"/api/v16/dhcp/static_lease/" =>
						"""{"success":true,"result":[{"id":"fixture","mac":"aa:bb:cc:dd:ee:ff","ip":"192.168.1.50"}]}""",
					"/api/v16/dhcp/dynamic_lease/" =>
						"""{"success":true,"result":[{"mac":"aa:bb:cc:dd:ee:ff","ip":"192.168.1.60","lease_remaining":3600,"is_static":false}]}""",
					"/api/v16/dhcp/static_lease/aa%3Abb%3Acc%3Add%3Aee%3Aff" =>
						"""{"success":true,"result":{"id":"aa:bb:cc:dd:ee:ff","mac":"aa:bb:cc:dd:ee:ff","ip":"192.168.1.50"}}""",
					_ => """{"success":true,"result":{}}"""
				}
				: request.Method == HttpMethod.Post
					? """{"success":true,"result":{"id":"fixture","mac":"aa:bb:cc:dd:ee:ff","ip":"192.168.1.50"}}"""
					: request.Address.AbsolutePath.EndsWith("/static_lease/aa%3Abb%3Acc%3Add%3Aee%3Aff",
						StringComparison.Ordinal)
						? """{"success":true,"result":{"id":"aa:bb:cc:dd:ee:ff","mac":"aa:bb:cc:dd:ee:ff","ip":"192.168.1.50"}}"""
						: """{"success":true,"result":{"enabled":false}}""";

			return Task.FromResult(FixtureHandler.Json(json));
		});
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var dhcp = new FreeboxDhcpApi(client.Transport);

		var config = await dhcp.GetConfigurationAsync();
		var updated = await dhcp.UpdateConfigurationAsync(new DhcpConfigurationPatch
		{
			Enabled = false,
			AlwaysBroadcast = false,
			Options = Optional<DhcpOptionWrite[]>.FromValue(
				[new DhcpOptionWrite { Id = "tcp_ttl", Val = "0" }])
		});
		var staticLeases = await dhcp.GetStaticLeasesAsync();
		var created = await dhcp.CreateStaticLeaseAsync(new CreateDhcpStaticLeaseRequest
		{
			Mac = "aa:bb:cc:dd:ee:ff",
			Comment = "fixture",
			Ip = "192.168.1.50"
		});
		var dynamicLeases = await dhcp.GetDynamicLeasesAsync();
		var lease = dhcp.StaticLease("aa:bb:cc:dd:ee:ff");
		await lease.GetAsync();
		await lease.UpdateAsync(new DhcpStaticLeasePatch { Comment = "updated", Ip = "192.168.1.51" });
		await lease.DeleteAsync();

		Assert.True(config.Enabled);
		Assert.False(updated.Enabled);
		Assert.Single(staticLeases);
		Assert.Equal("fixture", created.Id);
		Assert.Single(dynamicLeases);
		Assert.False(dynamicLeases[0].IsStatic);

		var requests = handler.Requests.Where(request =>
			request.Address.AbsolutePath is not "/api_version" &&
			!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
			!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal)).ToArray();

		Assert.Equal("/api/v16/dhcp/config/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/dhcp/config/", requests[1].Address.AbsolutePath);
		using var patch = JsonDocument.Parse(requests[1].Body!);
		Assert.False(patch.RootElement.GetProperty("enabled").GetBoolean());
		Assert.False(patch.RootElement.GetProperty("always_broadcast").GetBoolean());
		Assert.Equal("0", patch.RootElement.GetProperty("options")[0].GetProperty("val").GetString());

		Assert.Equal(HttpMethod.Post, requests[3].Method);
		Assert.Equal("/api/v16/dhcp/static_lease/", requests[3].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[4].Method);
		Assert.Equal("/api/v16/dhcp/dynamic_lease/", requests[4].Address.AbsolutePath);
		Assert.Equal("/api/v16/dhcp/static_lease/aa%3Abb%3Acc%3Add%3Aee%3Aff",
			requests[5].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[6].Method);
		Assert.Equal(HttpMethod.Delete, requests[7].Method);
	}

	[Fact]
	public async Task NullPatchValuesAndBlankStaticLeaseIdFailBeforeHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var dhcp = new FreeboxDhcpApi(client.Transport);

		await Assert.ThrowsAnyAsync<ArgumentException>(async () =>
			await dhcp.UpdateConfigurationAsync(new DhcpConfigurationPatch
			{
				Dns = Optional<string[]>.FromValue(["192.168.1.254", null!])
			}));
		await Assert.ThrowsAsync<ArgumentException>(async () =>
			await dhcp.CreateStaticLeaseAsync(new CreateDhcpStaticLeaseRequest
			{
				Options = Optional<DhcpOptionWrite[]>.FromValue(
					[new DhcpOptionWrite { Id = Optional<string>.Null }])
			}));
		Assert.Throws<ArgumentException>(() => dhcp.StaticLease(" "));

		Assert.Empty(handler.Requests);
	}
}
