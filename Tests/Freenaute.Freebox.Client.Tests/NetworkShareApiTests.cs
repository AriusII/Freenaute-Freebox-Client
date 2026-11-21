namespace Freenaute.Freebox.Client.Tests;

public sealed class NetworkShareApiTests
{
	[Fact]
	public async Task SambaRoundTripUsesNetshareSambaRouteAndKeepsExplicitFalse()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"file_share_enabled":true,"smbv2_enabled":false,"workgroup":"fixture-wg"}}"""
				: """{"success":true,"result":{"file_share_enabled":false,"logon_enabled":true}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var share = new FreeboxNetworkShareApi(client.Transport);

		var config = await share.GetSambaConfigurationAsync();
		var updated = await share.UpdateSambaConfigurationAsync(new SambaConfigPatch
		{
			FileShareEnabled = true, Smbv2Enabled = false, Workgroup = "fixture-wg"
		});

		Assert.False(config.FileShareEnabled);
		Assert.True(updated.FileShareEnabled);
		Assert.False(updated.Smbv2Enabled);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/netshare/samba/", requests[0].Address.AbsolutePath);
		Assert.Equal("fixture-session", requests[0].SessionToken);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/netshare/samba/", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.True(body.RootElement.GetProperty("file_share_enabled").GetBoolean());
		Assert.False(body.RootElement.GetProperty("smbv2_enabled").GetBoolean());
		Assert.Equal("fixture-wg", body.RootElement.GetProperty("workgroup").GetString());
	}

	[Fact]
	public async Task AfpRoundTripUsesNetshareAfpRoute()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"enabled":true,"guest_allow":false,"server_type":"macbook"}}"""
				: """{"success":true,"result":{"enabled":false,"guest_allow":true}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var share = new FreeboxNetworkShareApi(client.Transport);

		var config = await share.GetAfpConfigurationAsync();
		var updated = await share.UpdateAfpConfigurationAsync(new AfpConfigPatch
		{
			Enabled = true, GuestAllow = false, ServerType = AfpServerType.Macbook
		});

		Assert.False(config.Enabled);
		Assert.True(updated.Enabled);
		Assert.False(updated.GuestAllow);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal("/api/v16/netshare/afp/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/netshare/afp/", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.True(body.RootElement.GetProperty("enabled").GetBoolean());
		Assert.False(body.RootElement.GetProperty("guest_allow").GetBoolean());
		Assert.Equal("macbook", body.RootElement.GetProperty("server_type").GetString());
	}

	[Fact]
	public async Task FluentConfigureCommandsSendPatchedRequests()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Address.AbsolutePath.EndsWith("/samba/", StringComparison.Ordinal)
				? """{"success":true,"result":{"print_share_enabled":false}}"""
				: """{"success":true,"result":{"enabled":false}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var share = new FreeboxNetworkShareApi(client.Transport);

		await share.ConfigureSamba().With(patch => patch with { PrintShareEnabled = false }).SendAsync();
		await share.ConfigureAfp().With(patch => patch with { Enabled = false }).SendAsync();

		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal("/api/v16/netshare/samba/", requests[0].Address.AbsolutePath);
		Assert.Equal("/api/v16/netshare/afp/", requests[1].Address.AbsolutePath);
	}

	[Fact]
	public async Task NullPatchFieldIsRejectedBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var share = new FreeboxNetworkShareApi(client.Transport);

		await Assert.ThrowsAsync<ArgumentException>(() => share.UpdateSambaConfigurationAsync(
			new SambaConfigPatch { Workgroup = Optional<string>.Null }));
		await Assert.ThrowsAsync<ArgumentException>(() => share.UpdateAfpConfigurationAsync(
			new AfpConfigPatch { LoginPassword = Optional<string>.Null }));

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
			new FreeboxNetworkShareApi(client.Transport).GetSambaConfigurationAsync());

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

		var pending = new FreeboxNetworkShareApi(client.Transport).GetAfpConfigurationAsync(cancellation.Token);
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
