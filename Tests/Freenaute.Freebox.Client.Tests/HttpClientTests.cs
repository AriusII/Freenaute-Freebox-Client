namespace Freenaute.Freebox.Client.Tests;

public sealed class HttpClientTests
{
	internal const string DiscoveryJson = """
	                                      {"uid":"fixture","device_name":"Server","box_model":"fbxgw9-r1/full","box_model_name":"Ultra",
	                                       "api_version":"16.0","api_domain":"advertised.example","api_base_url":"/custom/api/",
	                                       "https_available":true,"https_port":8443}
	                                      """;

	[Fact]
	public async Task DependencyInjectionUsesConfiguredHttpClientHandler()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"enabled":true}}""");
		var services = new ServiceCollection();
		var credentials = ApprovedCredentials();
		var store = new InMemoryFreeboxCredentialStore();
		store.PutAsync(new FreeboxCredentialId("default", "org.fixture", "fixture"), credentials).AsTask().GetAwaiter()
			.GetResult();
		services.AddSingleton<IFreeboxCredentialStore>(store);
		services.AddFreeboxClient(options =>
		{
			options.ServerAddress = new Uri("https://fixture.example:8443/");
			options.ApiVersion = 16;
			options.EnableMdnsDiscovery = false;
			options.UseApplication("org.fixture");
		}).ConfigurePrimaryHttpMessageHandler(() => handler);
		await using var provider = services.BuildServiceProvider();

		var client = provider.GetRequiredService<IFreeboxClient>();
		var configuration = await client.AirMedia.GetConfigurationAsync();

		Assert.True(configuration.Enabled);
		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal(HttpMethod.Get, request.Method);
		Assert.Equal("https://fixture.example:8443/api/v16/airmedia/config/", request.Address.AbsoluteUri);
		Assert.Equal("fixture-session", request.SessionToken);
	}

	[Fact]
	public async Task DiscoveryUsesUnversionedEndpointAndAdvertisedVersionOnConfiguredOrigin()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Address.AbsolutePath is "/api_version"
				? DiscoveryJson
				: """{"success":true,"result":{"enabled":true}}""")));
		using var http = new HttpClient(handler);
		var client = new FreeboxClient(http,
			new FreeboxClientOptions
			{
				ServerAddress = new Uri("https://fixture.example:8080/"),
				EnableMdnsDiscovery = false,
				ApplicationId = "org.fixture"
			}, ApprovedCredentials());

		var configuration = await client.AirMedia.GetConfigurationAsync();

		Assert.True(configuration.Enabled);
		Assert.Collection(handler.Requests.Where(request =>
				!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
				!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal)),
			request => Assert.Equal("https://fixture.example:8080/api_version", request.Address.AbsoluteUri),
			request => Assert.Equal("https://fixture.example:8080/custom/api/v16/airmedia/config/",
				request.Address.AbsoluteUri));
		Assert.Null(handler.Requests.Single(request => request.Address.AbsolutePath is "/api_version").SessionToken);
		Assert.Equal("fixture-session", handler.Requests.Single(request =>
			request.Address.AbsolutePath.Contains("airmedia", StringComparison.Ordinal)).SessionToken);
	}

	[Fact]
	public async Task DiscoveryRouteUsesOneUnauthenticatedGetAndCachesTheResult()
	{
		using var handler = FixtureHandler.Constant(DiscoveryJson);
		using var http = new HttpClient(handler);
		using var client = new FreeboxClient(http,
			new FreeboxClientOptions
			{
				ServerAddress = new Uri("https://fixture.example:8443/"),
				EnableMdnsDiscovery = false
			});

		var first = await client.Discovery.GetApiVersionAsync();
		var second = await client.Discovery.GetApiVersionAsync();

		Assert.Equal("fixture", first.Uid);
		Assert.Same(first, second);
		var request = Assert.Single(handler.Requests);
		Assert.Equal(HttpMethod.Get, request.Method);
		Assert.Equal("https://fixture.example:8443/api_version", request.Address.AbsoluteUri);
		Assert.Null(request.SessionToken);
	}

	[Fact]
	public async Task TypedTransportSupportsConsumerGeneratedMetadata()
	{
		using var handler = FixtureHandler.Constant("""{"success":true,"result":{"value":"accepted"}}""");
		using var http = new HttpClient(handler);
		var client = CreateClient(http);

		var result = await client.Transport.SendAsync(HttpMethod.Put, "fixture/",
			new FixtureCommand("é / ?"), TestJsonContext.Default.FixtureCommand,
			TestJsonContext.Default.FixtureResult, requiresAuthentication: false);

		Assert.Equal("accepted", result.Value);
		var request = Assert.Single(handler.Requests);
		Assert.Equal(HttpMethod.Put, request.Method);
		Assert.Equal("https://fixture.example/api/v16/fixture/", request.Address.AbsoluteUri);
		Assert.Equal("application/json", request.ContentType);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.Equal("é / ?", body.RootElement.GetProperty("value").GetString());
	}

	[Fact]
	public async Task AirMediaReceiverNameIsOneEscapedPathSegmentAndWireRequestUsesStrings()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true}""");
		using var http = new HttpClient(handler);
		var client = CreateClient(http);

		await client.AirMedia.SendToReceiverAsync("Séjour & TV?#%", new AirMediaReceiverRequest(
			AirMediaAction.Start, AirMediaMediaType.Video, "https://media.example/video.mp4"));

		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal(HttpMethod.Post, request.Method);
		Assert.Equal("https://fixture.example/api/v16/airmedia/receivers/S%C3%A9jour%20%26%20TV%3F%23%25/",
			request.Address.AbsoluteUri);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.Equal("start", body.RootElement.GetProperty("action").GetString());
		Assert.Equal("video", body.RootElement.GetProperty("media_type").GetString());
		Assert.False(body.RootElement.TryGetProperty("password", out _));
	}

	[Fact]
	public async Task AirMediaLegacyReceiverRouteMatchesApiModelTypoPath()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true}""");
		using var http = new HttpClient(handler);
		var client = CreateClient(http);

		await client.AirMedia.SendToLegacyReceiverRouteAsync("Séjour & TV?#%", new AirMediaReceiverRequest(
			AirMediaAction.Start, AirMediaMediaType.Photo, "L0Rpc3F1ZQ==", "1111"));

		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal(HttpMethod.Post, request.Method);
		Assert.Equal("/api/v16/airmedia/receviers/S%C3%A9jour%20%26%20TV%3F%23%25/", request.Address.PathAndQuery);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.Equal("start", body.RootElement.GetProperty("action").GetString());
		Assert.Equal("photo", body.RootElement.GetProperty("media_type").GetString());
		Assert.Equal("1111", body.RootElement.GetProperty("password").GetString());
	}

	[Fact]
	public async Task AirMediaUpdateAndReceiversUseExpectedMethodsAndReadNestedCapabilities()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			request.Method == HttpMethod.Put
				? """{"success":true,"result":{"enabled":false}}"""
				: """
				  {"success":true,"result":[{"name":"Living room","password_protected":true,
				   "capabilities":{"photo":true,"audio":false,"video":true,"screen":false}}]}
				  """)));
		using var http = new HttpClient(handler);
		var client = CreateClient(http);

		var updated =
			await client.AirMedia.UpdateConfigurationAsync(new UpdateAirMediaConfigRequest(Enabled: false, "fixture-password"));
		var receivers = await client.AirMedia.GetReceiversAsync();

		Assert.False(updated.Enabled);
		var receiver = Assert.Single(receivers);
		Assert.Equal("Living room", receiver.Name);
		Assert.True(receiver.PasswordProtected);
		Assert.True(receiver.Capabilities.CanDisplayPhotos);
		Assert.True(receiver.Capabilities.CanPlayVideo);
		Assert.False(receiver.Capabilities.CanPlayAudio);
		Assert.Collection(ResourceRequests(handler),
			request =>
			{
				Assert.Equal(HttpMethod.Put, request.Method);
				Assert.Equal("/api/v16/airmedia/config/", request.Address.AbsolutePath);
				using var body = JsonDocument.Parse(request.Body!);
				Assert.False(body.RootElement.GetProperty("enabled").GetBoolean());
				Assert.Equal("fixture-password", body.RootElement.GetProperty("password").GetString());
			},
			request =>
			{
				Assert.Equal(HttpMethod.Get, request.Method);
				Assert.Equal("/api/v16/airmedia/receivers/", request.Address.AbsolutePath);
			});
	}

	[Theory]
	[InlineData(HttpStatusCode.OK)]
	[InlineData(HttpStatusCode.Forbidden)]
	public async Task ApiFailureRetainsStructuredDiagnosticsAcrossHttpStatuses(HttpStatusCode status)
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":false,"error_code":"insufficient_rights","msg":"Access denied","uid":"fixture-error","result":{"permission":"settings"}}""",
			status);
		using var http = new HttpClient(handler);
		var client = CreateClient(http);

		var error = await Assert.ThrowsAsync<FreeboxApiException>(async () =>
			await client.AirMedia.GetConfigurationAsync());

		Assert.Equal(status, error.StatusCode);
		Assert.Equal("insufficient_rights", error.ErrorCode);
		Assert.Equal("Access denied", error.ApiMessage);
		Assert.Equal("fixture-error", error.ErrorUid);
		Assert.Equal("settings", error.Details?.GetProperty("permission").GetString());
		Assert.Single(ResourceRequests(handler));
	}

	[Fact]
	public async Task HttpFailureWithoutJsonDoesNotExposeResponseBody()
	{
		using var handler =
			FixtureHandler.AuthenticatedConstant("private fixture diagnostic", HttpStatusCode.BadGateway);
		using var http = new HttpClient(handler);
		var client = CreateClient(http);

		var error = await Assert.ThrowsAsync<FreeboxApiException>(async () =>
			await client.AirMedia.GetConfigurationAsync());

		Assert.Equal(HttpStatusCode.BadGateway, error.StatusCode);
		Assert.DoesNotContain("private fixture diagnostic", error.Message, StringComparison.OrdinalIgnoreCase);
	}

	[Fact]
	public async Task CancellationReachesHttpHandlerWithoutRetry()
	{
		var entered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
		using var handler = FixtureHandler.Authenticated(async (_, cancellationToken) =>
		{
			entered.SetResult();
			await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
			return FixtureHandler.Json("{}");
		});
		using var http = new HttpClient(handler);
		var client = CreateClient(http);
		using var cancellation = new CancellationTokenSource();

		var call = client.AirMedia.GetConfigurationAsync(cancellation.Token);
		await entered.Task.WaitAsync(TimeSpan.FromSeconds(10));
		await cancellation.CancelAsync();

		await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await call);
		Assert.Single(ResourceRequests(handler));
	}

	[Fact]
	public async Task FluentReceiverCommandsRemainIndependentWhenReused()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true}""");
		using var http = new HttpClient(handler);
		var client = CreateClient(http);
		var original = client.AirMedia.Receiver("TV").PlayVideo("https://media.example/video.mp4");
		var customized = original.At(30).WithPassword("fixture-password");

		await customized.SendAsync();
		await original.SendAsync();

		Assert.Collection(ResourceRequests(handler),
			request =>
			{
				using var body = JsonDocument.Parse(request.Body!);
				Assert.Equal(30, body.RootElement.GetProperty("position").GetInt32());
				Assert.Equal("fixture-password", body.RootElement.GetProperty("password").GetString());
			},
			request =>
			{
				using var body = JsonDocument.Parse(request.Body!);
				Assert.False(body.RootElement.TryGetProperty("position", out _));
				Assert.False(body.RootElement.TryGetProperty("password", out _));
			});
	}

	[Theory]
	[InlineData("https://other.example/api/")]
	[InlineData("//other.example/api/")]
	[InlineData("../login/")]
	[InlineData("system/%2e%2e/login/")]
	[InlineData("system/%2fsecret/")]
	[InlineData("system/%5csecret/")]
	[InlineData("system/#fragment")]
	public async Task TransportRejectsPathsThatEscapeVersionedApiRoot(string path)
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		var client = CreateClient(http);

		await Assert.ThrowsAsync<ArgumentException>(async () => await client.Transport.SendAsync(HttpMethod.Get,
			path, TestJsonContext.Default.FixtureResult, requiresAuthentication: false));

		Assert.Empty(handler.Requests);
	}

	private static IEnumerable<CapturedRequest> ResourceRequests(FixtureHandler handler)
	{
		return handler.Requests.Where(request =>
			request.Address.AbsolutePath is not "/api_version" &&
			!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
			!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal));
	}

	internal static FreeboxClient CreateClient(HttpClient http, bool authenticate = true)
	{
		var options = new FreeboxClientOptions
		{
			ServerAddress = new Uri("https://fixture.example/"),
			ApiVersion = 16,
			AuthenticateAutomatically = authenticate,
			ApplicationId = authenticate ? "org.fixture" : null
		};
		return authenticate
			? new FreeboxClient(http, options, ApprovedCredentials())
			: new FreeboxClient(http, options);
	}

	internal static FreeboxAppCredentials ApprovedCredentials(string applicationId = "org.fixture") =>
		new(applicationId, "fixture-app-token", FreeboxCredentialStatus.Approved);
}
