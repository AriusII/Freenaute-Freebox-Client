namespace Freenaute.Freebox.AotSmoke;

internal static class Smoke
{
	private const string ApplicationId = "org.example.aot-smoke";
	private const string FixtureAppToken = "not-a-real-app-token";

	public static async Task<int> RunAsync()
	{
		try
		{
			CheckReflection();
			CheckDependencyInjection();
			CheckGeneratedOptionsValidation();
			await CheckCredentialStoreAsync();
			CheckMapperRoundTrips();
			CheckGeneratedDomainMetadata();
			await CheckAuthenticationFixtureAsync();
			await CheckScriptedDiscoveryAsync();
			await CheckScriptedWebSocketAsync();
			Console.WriteLine("AOT smoke passed: 9 check groups, no network or real Freebox used.");
			return 0;
		}
		catch (Exception error)
		{
			Console.Error.WriteLine($"AOT smoke failed: {error.Message}");
			return 1;
		}
	}

	private static void CheckReflection() =>
		Check("reflection-json-disabled", !JsonSerializer.IsReflectionEnabledByDefault);

	private static void CheckDependencyInjection()
	{
		var services = new ServiceCollection();
		services.AddFreeboxClient(options => options
			.UseApplication(ApplicationId)
			.WithApplicationVersion("1.0.0"));
		services.AddFreeboxClient("secondary", options => options
			.UseApplication(ApplicationId)
			.WithApplicationVersion("1.0.0"));

		using var provider = services.BuildServiceProvider();
		var primary = provider.GetRequiredService<IFreeboxClient>();
		var secondary = provider.GetRequiredKeyedService<IFreeboxClient>("secondary");
		Check("di-default-client", primary.Discovery is not null && primary.Transport is not null);
		Check("di-keyed-facades", !ReferenceEquals(primary, secondary) &&
								  ReferenceEquals(secondary.Discovery,
									  provider.GetRequiredKeyedService<IFreeboxDiscoveryApi>("secondary")));
	}

	private static void CheckGeneratedOptionsValidation()
	{
		var validator = new FreeboxClientOptionsValidator();
		var result = validator.Validate(name: null, new FreeboxClientOptions { ApiVersion = 0 });
		Check("generated-options-validator", result.Failed);
	}

	private static async Task CheckCredentialStoreAsync()
	{
		var store = new InMemoryFreeboxCredentialStore();
		var id = new FreeboxCredentialId("aot", ApplicationId, "fixture-uid");
		await store.PutAsync(id, new FreeboxAppCredentials(ApplicationId, FixtureAppToken,
			FreeboxCredentialStatus.Pending, 7));
		await store.PutAsync(id, new FreeboxAppCredentials(ApplicationId, FixtureAppToken,
			FreeboxCredentialStatus.Approved, 7));

		var loaded = await store.GetAsync(id);
		Check("credential-store-reload", loaded is
		{
			Status: FreeboxCredentialStatus.Approved,
			TrackId: 7
		} && loaded.GetAppToken() == FixtureAppToken);
		Check("credential-redaction", !loaded!.ToString().Contains(FixtureAppToken, StringComparison.Ordinal));
	}

	private static void CheckMapperRoundTrips()
	{
		var mapper = new FreeboxMapper();
		var version = new ApiVersionResponse("fixture-uid", "fixture-device", "fixture-model", "Fixture Freebox",
			FreeboxApiVersion.Current, "fixture.example", "/api", HttpsAvailable: true, 443);
		string versionJson = mapper.ToWire(version, FreeboxJsonSerializerContext.Default.ApiVersionResponse);
		var decoded = mapper.FromWire(versionJson,
			FreeboxJsonSerializerContext.Default.ApiVersionResponse);
		Check("mapper-api-version-roundtrip",
			decoded?.Uid is "fixture-uid" && decoded.ApiVersion == FreeboxApiVersion.Current);

		var union = StringOrInteger.FromInteger(42);
		string unionJson = mapper.ToWire(union, SharedJsonSerializerContext.Default.StringOrInteger);
		Check("mapper-nested-union-roundtrip",
			mapper.FromWire(unionJson, SharedJsonSerializerContext.Default.StringOrInteger) == union);
		Check("mapper-frozen-options", mapper.Options.IsReadOnly);
	}

	private static void CheckGeneratedDomainMetadata()
	{
		Check("generated-domain-metadata",
			FreeboxJsonSerializerContext.Default.ApiVersionResponse is not null &&
			SharedJsonSerializerContext.Default.StringOrInteger is not null &&
			NetworkJsonSerializerContext.Default.ConnectionConfiguration is not null &&
			FilesJsonSerializerContext.Default.FileEntry is not null &&
			ServicesJsonSerializerContext.Default.ContactEntry is not null &&
			SystemHomeJsonSerializerContext.Default.SystemConfig is not null &&
			ProtocolJsonSerializerContext.Default.Camera is not null &&
			WebSocketJsonSerializerContext.Default.RegisterEventsRequest is not null);
	}

	private static async Task CheckAuthenticationFixtureAsync()
	{
		using var handler = new AuthenticationFixtureHandler();
		using var http = new HttpClient(handler) { BaseAddress = new Uri("https://fixture.example/") };
		using var client = new FreeboxClient(http,
			new FreeboxClientOptions
			{
				ServerAddress = new Uri("https://fixture.example/"),
				ApplicationId = ApplicationId,
				ApplicationVersion = "1.0.0",
				ApiVersion = 4
			}, new FreeboxAppCredentials(ApplicationId, FixtureAppToken, FreeboxCredentialStatus.Approved));

		var configuration = await client.AirMedia.GetConfigurationAsync();
		Check("authenticated-fixture", configuration.Enabled && handler.ProtectedAuthorization is "fixture-session");
		Check("session-path-and-memory",
			handler.LoginPath is "/api/v4/login/session/" &&
			handler.ObservedSessionBodyDoesNotContain(FixtureAppToken));
	}

	private static async Task CheckScriptedDiscoveryAsync()
	{
		ApiVersionResponse metadata = new("fixture-mdns-uid", "Server", "fbx", "Fixture Freebox",
			FreeboxApiVersion.Current, "mdns.example", "/api/", HttpsAvailable: true, 443);
		var mdnsResolver = new ScriptedDiscoveryResolver(new FreeboxDiscoveryResult(
			new Uri("https://mdns.example/"), metadata));
		using var mdnsHandler = DiscoveryFixtureHandler.FailingIfCalled();
		using var mdnsHttp = new HttpClient(mdnsHandler);
		using var mdnsClient = new FreeboxClient(mdnsHttp, new FreeboxClientOptions(), mdnsResolver);

		var mdnsResult = await mdnsClient.Discovery.GetApiVersionAsync();
		Check("mdns-priority", ReferenceEquals(metadata, mdnsResult) && mdnsResolver.Calls is 1);

		using var fallbackHandler = new DiscoveryFixtureHandler();
		using var fallbackHttp = new HttpClient(fallbackHandler);
		using var fallbackClient = new FreeboxClient(fallbackHttp,
			new FreeboxClientOptions { ServerAddress = new Uri("https://fixture.example/") },
			new ScriptedDiscoveryResolver(result: null));
		var fallback = await fallbackClient.Discovery.GetApiVersionAsync();
		Check("http-fallback-selection", fallback.Uid is "fixture-http-uid" && fallbackHandler.RequestCount is 1);
	}

	private static async Task CheckScriptedWebSocketAsync()
	{
		await using var connection = new FreeboxWebSocketConnection(new ScriptedWebSocket(
			"{\"ordinal\":",
			"1}\n{\"ordinal\":2}\n{\"ordinal\":3}"));

		var first = await connection.ReceiveJsonAsync();
		var second = await connection.ReceiveJsonAsync();
		var third = await connection.ReceiveJsonAsync();
		Check("websocket-fragments-and-order",
			first.GetProperty("ordinal").GetInt32() is 1 && second.GetProperty("ordinal").GetInt32() is 2 &&
			third.GetProperty("ordinal").GetInt32() is 3);
	}

	private static void Check(string name, bool condition)
	{
		if (!condition)
		{
			throw new InvalidOperationException(name);
		}
	}
}
