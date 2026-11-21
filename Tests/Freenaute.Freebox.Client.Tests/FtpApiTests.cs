namespace Freenaute.Freebox.Client.Tests;

public sealed class FtpApiTests
{
	[Fact]
	public async Task ConfigurationRoundTripUsesFtpConfigRouteAndKeepsExplicitFalseAndZero()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"enabled":true,"allow_anonymous":false,"port_ctrl":0}}"""
				: """{"success":true,"result":{"enabled":false,"allow_remote_access":true}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var ftp = new FreeboxFtpApi(client.Transport);

		var config = await ftp.GetConfigurationAsync();
		var updated = await ftp.UpdateConfigurationAsync(new FtpConfigPatch
		{
			Enabled = true, AllowAnonymous = false, PortCtrl = 0, Password = "write-only-secret"
		});

		Assert.False(config.Enabled);
		Assert.True(config.AllowRemoteAccess);
		Assert.False(updated.AllowAnonymous);
		Assert.Equal(0, updated.PortCtrl);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/ftp/config/", requests[0].Address.AbsolutePath);
		Assert.Equal("fixture-session", requests[0].SessionToken);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/ftp/config/", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.True(body.RootElement.GetProperty("enabled").GetBoolean());
		Assert.False(body.RootElement.GetProperty("allow_anonymous").GetBoolean());
		Assert.Equal(0, body.RootElement.GetProperty("port_ctrl").GetInt64());
		Assert.Equal("write-only-secret", body.RootElement.GetProperty("password").GetString());
		Assert.Equal(4, body.RootElement.EnumerateObject().Count());
	}

	[Fact]
	public async Task FluentConfigureCommandSendsPatchedRequest()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"enabled":false}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var result = await new FreeboxFtpApi(client.Transport).Configure()
			.With(patch => patch with { Enabled = false }).SendAsync();

		Assert.False(result.Enabled);
		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal(HttpMethod.Put, request.Method);
		Assert.Equal("/api/v16/ftp/config/", request.Address.AbsolutePath);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.False(body.RootElement.GetProperty("enabled").GetBoolean());
	}

	[Fact]
	public async Task NullPatchFieldIsRejectedBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		await Assert.ThrowsAsync<ArgumentException>(() => new FreeboxFtpApi(client.Transport)
			.UpdateConfigurationAsync(new FtpConfigPatch { Enabled = Optional<bool>.Null }));

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
			new FreeboxFtpApi(client.Transport).GetConfigurationAsync());

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

		var pending = new FreeboxFtpApi(client.Transport).GetConfigurationAsync(cancellation.Token);
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
