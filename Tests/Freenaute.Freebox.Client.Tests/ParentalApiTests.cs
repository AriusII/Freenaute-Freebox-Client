namespace Freenaute.Freebox.Client.Tests;

public sealed class ParentalApiTests
{
	private const string FilterJson = """
	                                  {
	                                    "forced": true,
	                                    "id": 4,
	                                    "hosts": ["iPad-r0ro"],
	                                    "tmp_mode_expire": 0,
	                                    "ip": "0.0.0.0",
	                                    "forced_mode": "denied",
	                                    "scheduling_mode": "forced",
	                                    "macs": ["D8:A2:CA:FE:BA:DF"],
	                                    "current_mapping_idx": 0,
	                                    "next_change": 0,
	                                    "desc": "r0ro",
	                                    "tmp_mode": "allowed",
	                                    "filter_state": "denied"
	                                  }
	                                  """;

	[Fact]
	public async Task ConfigGetAndUpdateUseParentalConfigRoute()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"default_filter_mode":"denied"}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var config = await client.Parental.GetConfigAsync();
		Assert.Equal(ParentalFilterState.Denied, config.DefaultFilterMode);

		await client.Parental.Configure()
			.With(patch => patch with { DefaultFilterMode = ParentalFilterState.Webonly })
			.SendAsync();

		var requests = handler.Requests
			.Where(r => r.Address.AbsolutePath.Contains("/parental/", StringComparison.Ordinal)).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/parental/config/", requests[0].Address.AbsolutePath);
		Assert.Null(requests[0].Body);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/parental/config/", requests[1].Address.AbsolutePath);
		Assert.Equal("""{"default_filter_mode":"webonly"}""", requests[1].Body);
	}

	[Fact]
	public async Task FilterCollectionListsAndCreatesRules()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(
			FixtureHandler.Json(request.Method == HttpMethod.Get
				? $"{{\"success\":true,\"result\":[{FilterJson}]}}"
				: $"{{\"success\":true,\"result\":{FilterJson}}}")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var filters = await client.Parental.GetFiltersAsync();
		Assert.Single(filters);
		Assert.Equal(4, filters[0].Id);
		Assert.Equal(ParentalFilterState.Denied, filters[0].FilterState);
		Assert.Equal(ParentalSchedulingMode.Forced, filters[0].SchedulingMode);

		await client.Parental.CreateFilterAsync(new ParentalFilterWriteRequest
		{
			Macs = new[] { "00:07:cb:00:00:69", "00:07:cb:00:00:42" }
		});

		var requests = handler.Requests
			.Where(r => r.Address.AbsolutePath.Contains("/parental/filter", StringComparison.Ordinal)).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/parental/filter/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Post, requests[1].Method);
		Assert.Equal("/api/v16/parental/filter/", requests[1].Address.AbsolutePath);
		Assert.Equal("""{"macs":["00:07:cb:00:00:69","00:07:cb:00:00:42"]}""", requests[1].Body);
	}

	[Fact]
	public async Task FilterItemSupportsGetUpdateAndDelete()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			$"{{\"success\":true,\"result\":{FilterJson}}}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var filter = client.Parental.Filter(4);

		await filter.GetAsync();
		await filter.UpdateAsync(new ParentalFilterWriteRequest
		{
			Forced = true,
			ForcedMode = ParentalFilterState.Denied
		});
		await filter.DeleteAsync();

		var requests = handler.Requests
			.Where(r => !r.Address.AbsolutePath.Contains("/login/", StringComparison.Ordinal) &&
						r.Address.AbsolutePath.Contains("/parental/", StringComparison.Ordinal))
			.ToArray();
		Assert.Equal(3, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/parental/filter/4", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/parental/filter/4", requests[1].Address.AbsolutePath);
		Assert.Equal("""{"forced":true,"forced_mode":"denied"}""", requests[1].Body);
		Assert.Equal(HttpMethod.Delete, requests[2].Method);
		Assert.Equal("/api/v16/parental/filter/4", requests[2].Address.AbsolutePath);
	}

	[Fact]
	public async Task FilterPlanningSupportsGetAndUpdate()
	{
		const string planningJson = """
		                            {
		                              "cdayranges": [":fr_bank_holidays,:fr_school_holidays_c,"],
		                              "resolution": 48,
		                              "mapping": ["allowed", "webonly", "denied"]
		                            }
		                            """;
		using var handler = FixtureHandler.AuthenticatedConstant(
			$"{{\"success\":true,\"result\":{planningJson}}}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var planning = client.Parental.Filter(4).Planning;

		var current = await planning.GetAsync();
		Assert.Equal(48, current.Resolution);
		Assert.Equal([ParentalFilterState.Allowed, ParentalFilterState.Webonly, ParentalFilterState.Denied],
			current.Mapping!);

		await planning.UpdateAsync(new ParentalFilterPlanningPatch
		{
			Cdayranges = Array.Empty<string>(),
			Mapping = new[] { ParentalFilterState.Webonly, ParentalFilterState.Webonly }
		});

		var requests = handler.Requests
			.Where(r => r.Address.AbsolutePath.Contains("/planning", StringComparison.Ordinal)).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/parental/filter/4/planning", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/parental/filter/4/planning", requests[1].Address.AbsolutePath);
		Assert.Equal("""{"cdayranges":[],"mapping":["webonly","webonly"]}""", requests[1].Body);
	}

	[Fact]
	public void InvalidFilterIdAndExplicitNullFailBeforeHttp()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		Assert.Throws<ArgumentOutOfRangeException>(() => client.Parental.Filter(-1));
		Assert.Throws<ArgumentException>(() =>
		{
			_ = client.Parental.UpdateConfigAsync(new ParentalFilterConfigPatch
			{
				DefaultFilterMode = Optional<ParentalFilterState>.Null
			});
		});
		Assert.Throws<ArgumentException>(() =>
		{
			_ = client.Parental.CreateFilterAsync(new ParentalFilterWriteRequest { Macs = Optional<string[]>.Null });
		});
		Assert.Throws<ArgumentNullException>(() =>
		{
			_ = client.Parental.UpdateConfigAsync(null!);
		});
		Assert.DoesNotContain(handler.Requests, r => r.Address.AbsolutePath.Contains("/parental/",
			StringComparison.Ordinal));
	}

	[Fact]
	public async Task ParentalFacadeIsExposedThroughDependencyInjectionDefaultAndKeyed()
	{
		using var handler =
			FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"default_filter_mode":"allowed"}}""");
		var store = new InMemoryFreeboxCredentialStore();
		await store.PutAsync(new FreeboxCredentialId("default", "org.fixture.default", "fixture"),
			HttpClientTests.ApprovedCredentials("org.fixture.default"));
		await store.PutAsync(new FreeboxCredentialId("second", "org.fixture.second", "fixture"),
			HttpClientTests.ApprovedCredentials("org.fixture.second"));
		var services = new ServiceCollection();
		services.AddSingleton<IFreeboxCredentialStore>(store);
		services.AddFreeboxClient(options => Configure(options, "default"))
			.ConfigurePrimaryHttpMessageHandler(() => handler);
		services.AddFreeboxClient("second", options => Configure(options, "second"))
			.ConfigurePrimaryHttpMessageHandler(() => handler);
		await using var provider = services.BuildServiceProvider();

		var client = provider.GetRequiredService<IFreeboxClient>();
		Assert.Same(client.Parental, provider.GetRequiredService<IFreeboxParentalApi>());
		Assert.Same(provider.GetRequiredKeyedService<IFreeboxClient>("second").Parental,
			provider.GetRequiredKeyedService<IFreeboxParentalApi>("second"));

		var config = await provider.GetRequiredService<IFreeboxParentalApi>().GetConfigAsync();
		Assert.Equal(ParentalFilterState.Allowed, config.DefaultFilterMode);
	}

	private static void Configure(FreeboxClientOptions options, string profile)
	{
		options.ApiVersion = 16;
		options.ServerAddress = new Uri("https://fixture.example/");
		options.ApplicationId = $"org.fixture.{profile}";
		options.CredentialProfile = profile;
		options.EnableMdnsDiscovery = false;
	}
}
