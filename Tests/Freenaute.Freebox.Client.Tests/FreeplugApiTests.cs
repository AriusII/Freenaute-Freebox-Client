namespace Freenaute.Freebox.Client.Tests;

public sealed class FreeplugApiTests
{
	[Fact]
	public async Task GetNetworksReturnsArrayOfNetworksWithMembers()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":[{"id":"net-1","members":[{"id":"plug-1","local":true,"net_role":"cco"}]}]}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var freeplug = new FreeboxFreeplugApi(client.Transport);

		var networks = await freeplug.GetNetworksAsync();

		var network = Assert.Single(networks);
		Assert.Equal("net-1", network.Id);
		Assert.Equal("plug-1", Assert.Single(network.Members!).Id);
		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal(HttpMethod.Get, request.Method);
		Assert.Equal("/api/v16/freeplug/", request.Address.AbsolutePath);
		Assert.Equal("fixture-session", request.SessionToken);
	}

	[Fact]
	public async Task PlugResourceGetsAndResetsWithTrailingSlashRoutes()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Post
				? """{"success":true}"""
				: """{"success":true,"result":{"id":"plug-1","net_id":"net-1","rx_rate":1000}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var plug = new FreeboxFreeplugApi(client.Transport).Plug("plug-1");

		var info = await plug.GetAsync();
		await plug.ResetAsync();

		Assert.Equal("plug-1", info.Id);
		Assert.Equal("net-1", info.NetId);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/freeplug/plug-1/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Post, requests[1].Method);
		Assert.Equal("/api/v16/freeplug/plug-1/reset/", requests[1].Address.AbsolutePath);
	}

	[Fact]
	public async Task ApiErrorIsMappedToFreeboxApiException()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":false,"error_code":"invalid_request","msg":"Fixture failure"}""", HttpStatusCode.BadRequest);
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var error = await Assert.ThrowsAsync<FreeboxApiException>(() =>
			new FreeboxFreeplugApi(client.Transport).GetNetworksAsync());

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

		var pending = new FreeboxFreeplugApi(client.Transport).GetNetworksAsync(cancellation.Token);
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
