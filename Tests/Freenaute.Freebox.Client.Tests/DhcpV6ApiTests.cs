namespace Freenaute.Freebox.Client.Tests;

public sealed class DhcpV6ApiTests
{
	[Fact]
	public async Task GetAndUpdateUseDhcpV6ConfigRouteAndKeepExplicitFalse()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"enabled":false,"use_custom_dns":true}}"""
				: """{"success":true,"result":{"enabled":true,"dns":["2001:db8::1"]}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var dhcpV6 = new FreeboxDhcpV6Api(client.Transport);

		var config = await dhcpV6.GetConfigurationAsync();
		var updated = await dhcpV6.UpdateConfigurationAsync(new DhcpV6ConfigurationPatch
		{
			Enabled = false,
			UseCustomDns = true,
			Dns = Optional<string[]>.Set(["2001:db8::1"])
		});

		Assert.True(config.Enabled);
		Assert.Equal("2001:db8::1", Assert.Single(config.Dns!));
		Assert.False(updated.Enabled);
		Assert.True(updated.UseCustomDns);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/dhcpv6/config/", requests[0].Address.AbsolutePath);
		Assert.Equal("fixture-session", requests[0].SessionToken);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/dhcpv6/config/", requests[1].Address.AbsolutePath);
		using var body = JsonDocument.Parse(requests[1].Body!);
		Assert.False(body.RootElement.GetProperty("enabled").GetBoolean());
		Assert.True(body.RootElement.GetProperty("use_custom_dns").GetBoolean());
		Assert.Equal("2001:db8::1", body.RootElement.GetProperty("dns")[0].GetString());
		Assert.Equal(3, body.RootElement.EnumerateObject().Count());
	}

	[Fact]
	public async Task NullPatchFieldIsRejectedBeforeAnyHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		await Assert.ThrowsAnyAsync<ArgumentException>(async () => await new FreeboxDhcpV6Api(client.Transport)
			.UpdateConfigurationAsync(new DhcpV6ConfigurationPatch { Enabled = Optional<bool>.Null }));
		await Assert.ThrowsAnyAsync<ArgumentException>(async () => await new FreeboxDhcpV6Api(client.Transport)
			.UpdateConfigurationAsync(new DhcpV6ConfigurationPatch
			{
				Dns = Optional<string[]>.Set([(string)null!])
			}));

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
			await new FreeboxDhcpV6Api(client.Transport).GetConfigurationAsync());

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

		var pending = new FreeboxDhcpV6Api(client.Transport).GetConfigurationAsync(cancellation.Token);
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
