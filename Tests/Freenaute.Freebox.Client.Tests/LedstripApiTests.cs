namespace Freenaute.Freebox.Client.Tests;

public sealed class LedstripApiTests
{
	[Fact]
	public async Task StatusAndPlanningUseSlashlessStatusAndPlanningRoutes()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Address.AbsolutePath.EndsWith("/status", StringComparison.Ordinal)
				? """{"success":true,"result":{"use_planning":true,"next_change":42}}"""
				: """{"success":true,"result":{"use_planning":false,"resolution":2,"mapping":[true,false]}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var ledstrip = new FreeboxSystemHomeApi(client.Transport).Ledstrip;

		var status = await ledstrip.GetStatusAsync();
		var planning = await ledstrip.GetPlanningAsync();

		Assert.True(status.UsePlanning);
		Assert.Equal(42, status.NextChange);
		Assert.False(planning.UsePlanning);
		Assert.Equal(2, planning.Resolution);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/ledstrip/status", requests[0].Address.AbsolutePath);
		Assert.Equal("fixture-session", requests[0].SessionToken);
		Assert.Equal(HttpMethod.Get, requests[1].Method);
		Assert.Equal("/api/v16/ledstrip/planning/", requests[1].Address.AbsolutePath);
	}

	[Fact]
	public async Task ConfigurePlanningPutsUpdateOnSlashlessPlanningRouteAndKeepsExplicitFalse()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"use_planning":false,"resolution":2,"mapping":[false,true]}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var ledstrip = new FreeboxSystemHomeApi(client.Transport).Ledstrip;

		var planning = await ledstrip.ConfigurePlanning()
			.UseFields(new LedstripPlanningUpdate().WithUsePlanning(false).WithMapping([true, false]))
			.SendAsync();

		Assert.False(planning.UsePlanning);
		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal(HttpMethod.Put, request.Method);
		Assert.Equal("/api/v16/ledstrip/planning", request.Address.AbsolutePath);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.False(body.RootElement.GetProperty("use_planning").GetBoolean());
		Assert.True(body.RootElement.GetProperty("mapping")[0].GetBoolean());
		Assert.False(body.RootElement.TryGetProperty("resolution", out _));
	}

	[Fact]
	public async Task NullPatchFieldIsRejectedBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var command = new FreeboxSystemHomeApi(client.Transport).Ledstrip.ConfigurePlanning()
			.UseFields(new LedstripPlanningUpdate { UsePlanning = Optional<bool>.Null });

		await Assert.ThrowsAsync<ArgumentException>(async () => await command.SendAsync());

		Assert.Empty(ResourceRequests(handler));
	}

	[Fact]
	public async Task ApiErrorIsMappedToFreeboxApiException()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":false,"error_code":"invalid_request","msg":"Fixture failure"}""", HttpStatusCode.BadRequest);
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var error = await Assert.ThrowsAsync<FreeboxApiException>(async () =>
			await new FreeboxSystemHomeApi(client.Transport).Ledstrip.GetStatusAsync());

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

		var pending = new FreeboxSystemHomeApi(client.Transport).Ledstrip.GetPlanningAsync(cancellation.Token);
		await started.Task.WaitAsync(TimeSpan.FromSeconds(5));
		await cancellation.CancelAsync();

		await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await pending);
	}

	private static IEnumerable<CapturedRequest> ResourceRequests(FixtureHandler handler)
	{
		return handler.Requests.Where(request =>
			request.Address.AbsolutePath is not "/api_version" &&
			!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
			!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal));
	}
}
