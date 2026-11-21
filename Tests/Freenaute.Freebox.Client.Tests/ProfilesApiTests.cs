namespace Freenaute.Freebox.Client.Tests;

public sealed class ProfilesApiTests
{
	[Fact]
	public async Task ListUsesSlashlessProfileRoute()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":[{"id":3,"name":"fixture-profile","icon":"declared"}]}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var profiles = new FreeboxSystemHomeApi(client.Transport).Profiles;

		var list = await profiles.ListAsync();

		var profile = Assert.Single(list);
		Assert.Equal(3, profile.Id);
		Assert.Equal("fixture-profile", profile.Name);
		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal(HttpMethod.Get, request.Method);
		Assert.Equal("/api/v16/profile", request.Address.AbsolutePath);
		Assert.Equal("fixture-session", request.SessionToken);
	}

	[Fact]
	public async Task ProfileResourceGetsAndDeletesById()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Delete
				? """{"success":true}"""
				: """{"success":true,"result":{"id":3,"name":"fixture-profile"}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var profiles = new FreeboxSystemHomeApi(client.Transport).Profiles;

		var profile = await profiles.For(3).GetAsync();
		await profiles.For(3).DeleteAsync();

		Assert.Equal(3, profile.Id);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/profile/3", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Delete, requests[1].Method);
		Assert.Equal("/api/v16/profile/3", requests[1].Address.AbsolutePath);
	}

	[Fact]
	public async Task CreatePostsNameAndReturnsCreatedId()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"id":9}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var created = await new FreeboxSystemHomeApi(client.Transport).Profiles.Create("new-profile").SendAsync();

		Assert.Equal(9, created.Id);
		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal(HttpMethod.Post, request.Method);
		Assert.Equal("/api/v16/profile/", request.Address.AbsolutePath);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.Equal("new-profile", body.RootElement.GetProperty("name").GetString());
		Assert.False(body.RootElement.TryGetProperty("url", out _));
	}

	[Fact]
	public async Task DefaultModeMigrationUsesNetworkControlMigrateRoute()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Post
				? """{"success":true,"result":{"default_mode_migrated":true}}"""
				: """{"success":true,"result":{"default_mode_migrated":false}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var profiles = new FreeboxSystemHomeApi(client.Transport).Profiles;

		var status = await profiles.GetMigrationStatusAsync();
		var migrated = await profiles.MigrateDefaultModeAsync();

		Assert.False(status.DefaultModeMigrated);
		Assert.True(migrated.DefaultModeMigrated);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/network_control/migrate", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Post, requests[1].Method);
		Assert.Equal("/api/v16/network_control/migrate", requests[1].Address.AbsolutePath);
	}

	[Fact]
	public async Task NullProfileNameIsRejectedBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var command = new FreeboxSystemHomeApi(client.Transport).Profiles.Create(null!);

		await Assert.ThrowsAsync<ArgumentNullException>(() => command.SendAsync());

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
			new FreeboxSystemHomeApi(client.Transport).Profiles.ListAsync());

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

		var pending = new FreeboxSystemHomeApi(client.Transport).Profiles.ListAsync(cancellation.Token);
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
