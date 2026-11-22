namespace Freenaute.Freebox.Client.Tests;

public sealed class PvrApiTests
{
	[Fact]
	public async Task ConfigurationGetAndUpdateUseTypedPvrConfigRoute()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Get
				? """{"success":true,"result":{"margin_before":10,"margin_after":5}}"""
				: """{"success":true}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var pvr = new FreeboxServicesApi(client.Transport).Pvr;

		var config = await pvr.GetConfigurationAsync();
		await pvr.UpdateConfigurationAsync(new PvrConfig { MarginBefore = 12, MarginAfter = 7 });

		Assert.Equal(10, config.MarginBefore);
		Assert.Equal(5, config.MarginAfter);
		var requests = PvrRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/pvr/config/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/pvr/config/", requests[1].Address.AbsolutePath);
		Assert.Equal("""{"margin_before":12,"margin_after":7}""", requests[1].Body);
	}

	[Fact]
	public async Task FinishedCollectionAndItemRoutesSupportGetUpdateDelete()
	{
		const string itemJson = """
		                        {"id":"4","media":"Disk","path":"Records","filename":"fixture.ts","byte_size":123,
		                         "has_record_gen":false,"record_gen_id":0,"altered":false,"state":"finished",
		                         "error":"none","channel_uuid":"uuid","channel_name":"M6","channel_quality":"hd",
		                         "channel_type":"dvb","name":"Fixture","subname":"","broadcast_type":"tv",
		                         "start":1,"end":2,"secure":false}
		                        """;
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Get && request.Address.AbsolutePath.EndsWith("/pvr/finished/",
				StringComparison.Ordinal)
				? $"{{\"success\":true,\"result\":[{itemJson}]}}"
				: request.Method == HttpMethod.Delete
					? """{"success":true}"""
					: $"{{\"success\":true,\"result\":{itemJson}}}")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var pvr = new FreeboxServicesApi(client.Transport).Pvr;

		var list = await pvr.GetFinishedAsync();
		var item = await pvr.GetFinishedAsync(4);
		var updated = await pvr.UpdateFinishedAsync(4, new Frecord { Name = "Renamed" });
		await pvr.DeleteFinishedAsync(4);

		Assert.Single(list);
		Assert.Equal("4", item.Id);
		Assert.Equal("Fixture", item.Name);
		Assert.Equal("Fixture", updated.Name);
		var requests = PvrRequests(handler).ToArray();
		Assert.Equal(4, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/pvr/finished/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[1].Method);
		Assert.Equal("/api/v16/pvr/finished/4", requests[1].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[2].Method);
		Assert.Equal("""{"name":"Renamed"}""", requests[2].Body);
		Assert.Equal(HttpMethod.Delete, requests[3].Method);
		Assert.Equal("/api/v16/pvr/finished/4", requests[3].Address.AbsolutePath);
	}

	[Fact]
	public async Task ProgrammedCollectionAndItemRoutesSupportGetCreateUpdateDelete()
	{
		const string itemJson = """
		                        {"id":"9","media":"Disk","path":"Records","has_record_gen":false,"record_gen_id":0,
		                         "conflict":false,"overlap_list":[],"enabled":true,"altered":false,
		                         "state":"waiting_start_time","error":"none","channel_uuid":"uuid",
		                         "channel_name":"M6","channel_quality":"hd","channel_type":"dvb",
		                         "name":"Fixture","subname":"","broadcast_type":"tv","start":1,"end":2,
		                         "legacy_uri":"","force_channel_name":""}
		                        """;
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Get && request.Address.AbsolutePath.EndsWith("/pvr/programmed/",
				StringComparison.Ordinal)
				? $"{{\"success\":true,\"result\":[{itemJson}]}}"
				: request.Method == HttpMethod.Delete
					? """{"success":true}"""
					: $"{{\"success\":true,\"result\":{itemJson}}}")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var pvr = new FreeboxServicesApi(client.Transport).Pvr;

		var list = await pvr.GetProgrammedAsync();
		var created = await pvr.CreateProgrammedAsync(new Precord { Name = "Create" });
		var item = await pvr.GetProgrammedAsync(9);
		var updated = await pvr.UpdateProgrammedAsync(9, new Precord { Name = "Update" });
		await pvr.DeleteProgrammedAsync(9);

		Assert.Single(list);
		Assert.Equal("9", created.Id);
		Assert.Equal("9", item.Id);
		Assert.Equal("9", updated.Id);
		var requests = PvrRequests(handler).ToArray();
		Assert.Equal(5, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/pvr/programmed/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Post, requests[1].Method);
		Assert.Equal("""{"name":"Create"}""", requests[1].Body);
		Assert.Equal(HttpMethod.Get, requests[2].Method);
		Assert.Equal("/api/v16/pvr/programmed/9", requests[2].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[3].Method);
		Assert.Equal("""{"name":"Update"}""", requests[3].Body);
		Assert.Equal(HttpMethod.Delete, requests[4].Method);
		Assert.Equal("/api/v16/pvr/programmed/9", requests[4].Address.AbsolutePath);
	}

	[Fact]
	public async Task MediaAndQuotaRoutesUseOpenApiPayloads()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Address.AbsolutePath.EndsWith("/pvr/media/", StringComparison.Ordinal)
				? """{"success":true,"result":[{"media":"Disk","free_bytes":10,"total_bytes":100,"record_time":{"iptv":{"hd":99}}}]}"""
				: request.Method == HttpMethod.Put
					? """{"success":true,"result":{"quota_exceeded":false,"needed_tresh":80,"cur_tresh":60}}"""
					: """{"success":true,"result":{"quota_exceeded":true,"needed_tresh":80,"cur_tresh":40}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var pvr = new FreeboxServicesApi(client.Transport).Pvr;

		var media = await pvr.GetMediaAsync();
		var quota = await pvr.GetQuotaAsync();
		var updated = await pvr.UpdateQuotaAsync();

		Assert.Single(media);
		Assert.Equal("Disk", media[0].Media);
		Assert.True(media[0].RecordTime!.TryGetValue("iptv", out var iptvRecordTime));
		Assert.Equal(99, iptvRecordTime.GetProperty("hd").GetInt32());
		Assert.True(quota.QuotaExceeded);
		Assert.False(updated.QuotaExceeded);
		var requests = PvrRequests(handler).ToArray();
		Assert.Equal(3, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/pvr/media/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[1].Method);
		Assert.Equal("/api/v16/pvr/quota/", requests[1].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[2].Method);
		Assert.Equal("/api/v16/pvr/quota/", requests[2].Address.AbsolutePath);
		Assert.Equal("{}", requests[2].Body);
	}

	[Fact]
	public async Task InvalidIdentifiersAndNullPayloadsFailBeforeHttp()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var pvr = new FreeboxServicesApi(client.Transport).Pvr;

		await Assert.ThrowsAsync<ArgumentNullException>(async () => await pvr.UpdateConfigurationAsync(null!));
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await pvr.UpdateFinishedAsync(0, null!));
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await pvr.CreateProgrammedAsync(null!));
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await pvr.UpdateProgrammedAsync(0, null!));
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await pvr.GetFinishedAsync(-1));
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await pvr.UpdateFinishedAsync(-1, new Frecord()));
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await pvr.DeleteProgrammedAsync(-1));
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await pvr.GetProgrammedAsync(-1));
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await pvr.UpdateProgrammedAsync(-1, new Precord()));
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await pvr.DeleteFinishedAsync(-1));
		Assert.DoesNotContain(PvrRequests(handler), request => request.Address.AbsolutePath.Contains("/pvr/",
			StringComparison.Ordinal));
	}

	private static IEnumerable<CapturedRequest> PvrRequests(FixtureHandler handler)
	{
		return handler.Requests.Where(request =>
			request.Address.AbsolutePath is not "/api_version" &&
			!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
			!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal) &&
			request.Address.AbsolutePath.Contains("/pvr/", StringComparison.Ordinal));
	}
}
