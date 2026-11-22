namespace Freenaute.Freebox.Client.Tests;

public sealed class CredentialCoordinatorTests
{
	[Fact]
	public async Task InMemoryCredentialStore_PersistsPendingCloneAndRedactsSecret()
	{
		var store = new InMemoryFreeboxCredentialStore();
		var id = new FreeboxCredentialId("home", "org.fixture", "fixture");
		var credentials =
			new FreeboxAppCredentials("org.fixture", "fixture-secret", FreeboxCredentialStatus.Pending, 42);

		await store.PutAsync(id, credentials);
		var restored = await store.GetAsync(id);

		Assert.NotNull(restored);
		Assert.NotSame(credentials, restored);
		Assert.Equal(FreeboxCredentialStatus.Pending, restored.Status);
		Assert.Equal(42, restored.TrackId);
		Assert.Equal("fixture-secret", restored.GetAppToken());
		Assert.DoesNotContain("fixture-secret", credentials.ToString(), StringComparison.Ordinal);
	}

	[Fact]
	public async Task BeginPairingAsync_PersistsPendingAndResumesWithoutReauthorization()
	{
		using var handler = PairingHandler(AuthorizationTrackEnum.Pending);
		var services = CreatePairingServices(handler);
		await using var provider = services.BuildServiceProvider();
		var coordinator = provider.GetRequiredService<IFreeboxAuthorizationCoordinator>();

		var first = await coordinator.BeginPairingAsync();
		var second = await coordinator.BeginPairingAsync();

		Assert.Equal(FreeboxCredentialStatus.Pending, first.Status);
		Assert.Equal(first.TrackId, second.TrackId);
		Assert.Single(handler.Requests, request => request.Method == HttpMethod.Post &&
												   request.Address.AbsolutePath.EndsWith("/login/authorize/"));
		var stored = await provider.GetRequiredService<IFreeboxCredentialStore>()
			.GetAsync(new FreeboxCredentialId("home", "org.fixture", "fixture"));
		Assert.Equal(FreeboxCredentialStatus.Pending, stored!.Status);
	}

	[Fact]
	public async Task WaitForApprovalAsync_PromotesApprovedCredentialWithoutPersistingChallenge()
	{
		using var handler = PairingHandler(AuthorizationTrackEnum.Granted);
		var services = CreatePairingServices(handler);
		await using var provider = services.BuildServiceProvider();
		var coordinator = provider.GetRequiredService<IFreeboxAuthorizationCoordinator>();

		await coordinator.BeginPairingAsync();
		var approved = await coordinator.WaitForApprovalAsync(TimeSpan.FromMilliseconds(1));

		Assert.Equal(FreeboxCredentialStatus.Approved, approved.Status);
		Assert.Equal("fixture-app-token", approved.GetAppToken());
		Assert.DoesNotContain("fixture-challenge", approved.ToString(), StringComparison.Ordinal);
		var persisted = await provider.GetRequiredService<IFreeboxCredentialStore>()
			.GetAsync(new FreeboxCredentialId("home", "org.fixture", "fixture"));
		Assert.Equal(FreeboxCredentialStatus.Approved, persisted!.Status);
		Assert.DoesNotContain("fixture-challenge", persisted.ToString(), StringComparison.Ordinal);
	}

	[Fact]
	public async Task ApprovedCredentialsAreReusedAcrossClientsAndLogoutPreservesAppToken()
	{
		var store = new InMemoryFreeboxCredentialStore();
		var id = new FreeboxCredentialId("home", "org.fixture", "fixture");
		await store.PutAsync(id,
			new FreeboxAppCredentials("org.fixture", "fixture-app-token", FreeboxCredentialStatus.Approved));
		var services = CreatePairingServices(FixtureHandler.Authenticated((request, _) =>
			Task.FromResult(request.Address.AbsolutePath.EndsWith("/login/logout/", StringComparison.Ordinal)
				? FixtureHandler.Json("""{"success":true}""")
				: FixtureHandler.Json("""{"success":true,"result":{"enabled":true}}"""))), store);
		await using var provider = services.BuildServiceProvider();

		var client = provider.GetRequiredService<IFreeboxClient>();
		await client.AirMedia.GetConfigurationAsync();
		await client.Authentication.CloseSessionAsync();
		await client.AirMedia.GetConfigurationAsync();

		var persisted = await store.GetAsync(id);
		Assert.Equal(FreeboxCredentialStatus.Approved, persisted!.Status);
		Assert.Equal("fixture-app-token", persisted.GetAppToken());
	}

	[Fact]
	public async Task SessionOpeningDoesNotWriteSessionTokensBackToCredentialStore()
	{
		var store = new RecordingCredentialStore();
		var id = new FreeboxCredentialId("home", "org.fixture", "fixture");
		await store.PutAsync(id,
			new FreeboxAppCredentials("org.fixture", "fixture-app-token", FreeboxCredentialStatus.Approved));
		store.ResetWrites();
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"enabled":true}}""");
		var services = CreatePairingServices(handler, store);
		await using var provider = services.BuildServiceProvider();

		await provider.GetRequiredService<IFreeboxClient>().AirMedia.GetConfigurationAsync();

		Assert.Equal(0, store.PutCount);
		Assert.All(store.Reads, read => Assert.Equal(id, read));
	}

	private static ServiceCollection CreatePairingServices(FixtureHandler handler,
		IFreeboxCredentialStore? store = null)
	{
		var services = new ServiceCollection();
		services.AddSingleton(store ?? new InMemoryFreeboxCredentialStore());
		services.AddFreeboxClient(options =>
		{
			options.ApiVersion = 16;
			options.ServerAddress = new Uri("https://fixture.example/");
			options.ApplicationId = "org.fixture";
			options.ApplicationName = "Fixture";
			options.ApplicationVersion = "1.0";
			options.DeviceName = "Test";
			options.CredentialProfile = "home";
			options.EnableMdnsDiscovery = false;
		}).ConfigurePrimaryHttpMessageHandler(() => handler);
		return services;
	}

	private static FixtureHandler PairingHandler(AuthorizationTrackEnum status)
	{
		return FixtureHandler.Authenticated((request, _) => Task.FromResult(
			request.Address.AbsolutePath.EndsWith("/login/authorize/", StringComparison.Ordinal)
				? FixtureHandler.Json("""{"success":true,"result":{"app_token":"fixture-app-token","track_id":42}}""")
				: request.Address.AbsolutePath.EndsWith("/login/authorize/42/", StringComparison.Ordinal)
					? FixtureHandler.Json(
						$"{{\"success\":true,\"result\":{{\"status\":\"{TrackName(status)}\",\"challenge\":\"fixture-challenge\"}}}}")
					: FixtureHandler.Json("""{"success":true,"result":{"enabled":true}}""")));
	}

	private static string TrackName(AuthorizationTrackEnum status)
	{
		return status switch
		{
			AuthorizationTrackEnum.Pending => "pending",
			AuthorizationTrackEnum.Granted => "granted",
			AuthorizationTrackEnum.Denied => "denied",
			AuthorizationTrackEnum.Timeout => "timeout",
			_ => "unknown"
		};
	}

	private sealed class RecordingCredentialStore : IFreeboxCredentialStore
	{
		private readonly InMemoryFreeboxCredentialStore _inner = new();
		public int PutCount { get; private set; }
		public List<FreeboxCredentialId> Reads { get; } = [];

		public bool IsAvailable => true;
		public string Kind => "recording-memory";

		public async ValueTask<FreeboxAppCredentials?> GetAsync(FreeboxCredentialId id,
			CancellationToken cancellationToken = default)
		{
			Reads.Add(id);
			return await _inner.GetAsync(id, cancellationToken);
		}

		public async ValueTask PutAsync(FreeboxCredentialId id, FreeboxAppCredentials credentials,
			CancellationToken cancellationToken = default)
		{
			PutCount++;
			await _inner.PutAsync(id, credentials, cancellationToken);
		}

		public async ValueTask DeleteAsync(FreeboxCredentialId id, CancellationToken cancellationToken = default) =>
			await _inner.DeleteAsync(id, cancellationToken);

		public void ResetWrites() => PutCount = 0;
	}
}
