namespace Freenaute.Freebox.Client.Tests;

public sealed class DependencyInjectionIsolationTests
{
	[Fact]
	public async Task KeyedClientsUseIndependentHttpClientsStateFacadesAndCredentials()
	{
		using var homeHandler = ClientHandler("home-session");
		using var officeHandler = ClientHandler("office-session");
		var store = new InMemoryFreeboxCredentialStore();
		await store.PutAsync(new FreeboxCredentialId("home", "org.fixture.home", "fixture"),
			HttpClientTests.ApprovedCredentials("org.fixture.home"));
		await store.PutAsync(new FreeboxCredentialId("office", "org.fixture.office", "fixture"),
			HttpClientTests.ApprovedCredentials("org.fixture.office"));
		var services = new ServiceCollection();
		services.AddSingleton<IFreeboxCredentialStore>(store);
		services.AddFreeboxClient("home", options => Configure(options, "home"))
			.ConfigurePrimaryHttpMessageHandler(() => homeHandler);
		services.AddFreeboxClient("office", options => Configure(options, "office"))
			.ConfigurePrimaryHttpMessageHandler(() => officeHandler);
		using var provider = services.BuildServiceProvider();

		var home = provider.GetRequiredKeyedService<IFreeboxClient>("home");
		var office = provider.GetRequiredKeyedService<IFreeboxClient>("office");
		var homeNetwork = provider.GetRequiredKeyedService<IFreeboxNetworkApi>("home");
		var officeNetwork = provider.GetRequiredKeyedService<IFreeboxNetworkApi>("office");

		await home.AirMedia.GetConfigurationAsync();
		await office.AirMedia.GetConfigurationAsync();
		await home.AirMedia.GetConfigurationAsync();

		Assert.Same(home.Network, homeNetwork);
		Assert.Same(office.Network, officeNetwork);
		Assert.NotSame(home, office);
		Assert.NotSame(home.Authentication, office.Authentication);
		Assert.Equal(2, ResourceRequests(homeHandler).Count(request => request.SessionToken is "home-session"));
		Assert.Single(ResourceRequests(officeHandler), request => request.SessionToken is "office-session");
		Assert.All(homeHandler.Requests.Where(ResourceSessionRequest), request =>
			Assert.Equal("home-session", request.SessionToken));
		Assert.All(officeHandler.Requests.Where(ResourceSessionRequest), request =>
			Assert.Equal("office-session", request.SessionToken));
	}

	[Fact]
	public async Task ResilienceRetriesTransientGetButNotMutation()
	{
		int getAttempts = 0;
		int mutationAttempts = 0;
		using var handler = FixtureHandler.Authenticated((request, _) =>
		{
			if (request.Address.AbsolutePath.EndsWith("/airmedia/config/", StringComparison.Ordinal))
			{
				if (request.Method == HttpMethod.Get)
				{
					getAttempts++;
					return Task.FromResult(getAttempts is 1
						? FixtureHandler.Json("{}", HttpStatusCode.InternalServerError)
						: FixtureHandler.Json("""{"success":true,"result":{"enabled":true}}"""));
				}

				mutationAttempts++;
				return Task.FromResult(FixtureHandler.Json("{}", HttpStatusCode.InternalServerError));
			}

			return Task.FromResult(FixtureHandler.Json("""{"success":true,"result":{"enabled":true}}"""));
		});
		var store = new InMemoryFreeboxCredentialStore();
		await store.PutAsync(new FreeboxCredentialId("default", "org.fixture", "fixture"),
			HttpClientTests.ApprovedCredentials());
		var services = new ServiceCollection();
		services.AddSingleton<IFreeboxCredentialStore>(store);
		services.AddFreeboxClient(options =>
		{
			options.ApiVersion = 16;
			options.ServerAddress = new Uri("https://fixture.example/");
			options.ApplicationId = "org.fixture";
			options.EnableMdnsDiscovery = false;
			options.EnableResilience = true;
		}).ConfigurePrimaryHttpMessageHandler(() => handler);
		using var provider = services.BuildServiceProvider();
		var client = provider.GetRequiredService<IFreeboxClient>();

		Assert.True((await client.AirMedia.GetConfigurationAsync()).Enabled);
		await Assert.ThrowsAsync<FreeboxApiException>(() =>
			client.AirMedia.UpdateConfigurationAsync(new UpdateAirMediaConfigRequest(false, "fixture-password")));

		Assert.Equal(2, getAttempts);
		Assert.Equal(1, mutationAttempts);
	}

	private static void Configure(FreeboxClientOptions options, string profile)
	{
		options.ApiVersion = 16;
		options.ServerAddress = new Uri("https://fixture.example/");
		options.ApplicationId = $"org.fixture.{profile}";
		options.CredentialProfile = profile;
		options.EnableMdnsDiscovery = false;
	}

	private static FixtureHandler ClientHandler(string sessionToken)
	{
		return FixtureHandler.Authenticated((request, _) => Task.FromResult(
			FixtureHandler.Json("""{"success":true,"result":{"enabled":true}}""")), sessionToken);
	}

	private static IEnumerable<CapturedRequest> ResourceRequests(FixtureHandler handler) =>
		handler.Requests.Where(ResourceSessionRequest);

	private static bool ResourceSessionRequest(CapturedRequest request) =>
		request.Address.AbsolutePath.Contains("/airmedia/", StringComparison.Ordinal);
}
