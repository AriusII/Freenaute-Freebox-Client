namespace Freenaute.Freebox.Client.Tests;

public sealed class DiscoveryTests
{
	[Fact]
	public async Task MdnsDiscoveryIsPreferredAndKeepsTheHttpFallbackUnused()
	{
		var resolver = new FakeResolver(_ => ValueTask.FromResult<FreeboxDiscoveryResult?>(ValidResult()));
		using var handler = FixtureHandler.Constant("{}", HttpStatusCode.InternalServerError);
		using var http = new HttpClient(handler);
		using var client = new FreeboxClient(http,
			new FreeboxClientOptions
			{
				ServerAddress = new Uri("https://fallback.example/"),
				MdnsTimeout = TimeSpan.FromSeconds(1)
			}, resolver);

		var result = await client.Discovery.GetApiVersionAsync();

		Assert.Equal("mdns-uid", result.Uid);
		Assert.Empty(handler.Requests);
		Assert.Equal(1, resolver.CallCount);
	}

	[Fact]
	public async Task MdnsTimeoutFallsBackToTheExactConfiguredHttpOrigin()
	{
		var resolver = new FakeResolver(async cancellationToken =>
		{
			await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
			return null;
		});
		using var handler = FixtureHandler.Constant(HttpClientTests.DiscoveryJson);
		using var http = new HttpClient(handler);
		using var client = new FreeboxClient(http,
			new FreeboxClientOptions
			{
				ServerAddress = new Uri("https://fallback.example:9443/"),
				MdnsTimeout = TimeSpan.FromMilliseconds(20)
			}, resolver);

		var result = await client.Discovery.GetApiVersionAsync();

		Assert.Equal("fixture", result.Uid);
		var request = Assert.Single(handler.Requests);
		Assert.Equal("https://fallback.example:9443/api_version", request.Address.AbsoluteUri);
	}

	[Fact]
	public async Task CallerCancellationDoesNotBecomeAnHttpFallback()
	{
		var resolver = new FakeResolver(async cancellationToken =>
		{
			await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
			return null;
		});
		using var handler = FixtureHandler.Constant(HttpClientTests.DiscoveryJson);
		using var http = new HttpClient(handler);
		using var client = new FreeboxClient(http, new FreeboxClientOptions(), resolver);
		using var cancellation = new CancellationTokenSource(20);

		await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
			await client.Discovery.GetApiVersionAsync(cancellation.Token));

		Assert.Empty(handler.Requests);
	}

	[Fact]
	public async Task UntrustedOrIncompleteMdnsMetadataFallsBackToHttp()
	{
		FreeboxDiscoveryResult[] invalid =
		[
			ValidResult() with { Origin = new Uri("http://mdns.example:8443/") },
			ValidResult() with { Origin = new Uri("https://other.example:8443/") },
			ValidResult() with { ApiVersion = ValidResult().ApiVersion with { Uid = null } },
			ValidResult() with { ApiVersion = ValidResult().ApiVersion with { HttpsAvailable = false } },
			ValidResult() with { ApiVersion = ValidResult().ApiVersion with { HttpsPort = null } },
			ValidResult() with { ApiVersion = ValidResult().ApiVersion with { ApiDomain = null } }
		];

		foreach (var candidate in invalid)
		{
			var resolver = new FakeResolver(_ => ValueTask.FromResult<FreeboxDiscoveryResult?>(candidate));
			using var handler = FixtureHandler.Constant(HttpClientTests.DiscoveryJson);
			using var http = new HttpClient(handler);
			using var client = new FreeboxClient(http,
				new FreeboxClientOptions { ServerAddress = new Uri("https://fallback.example/") }, resolver);

			var result = await client.Discovery.GetApiVersionAsync();

			Assert.Equal("fixture", result.Uid);
			Assert.Equal("https://fallback.example/api_version",
				Assert.Single(handler.Requests).Address.AbsoluteUri);
		}
	}

	[Fact]
	public async Task DependencyInjectionUsesTheMdnsUidForTheCredentialKeyAndOrigin()
	{
		var resolver = new FakeResolver(_ => ValueTask.FromResult<FreeboxDiscoveryResult?>(ValidResult()));
		var store = new InMemoryFreeboxCredentialStore();
		await store.PutAsync(new FreeboxCredentialId("default", "org.fixture", "mdns-uid"),
			HttpClientTests.ApprovedCredentials());
		var services = new ServiceCollection();
		services.AddSingleton<IFreeboxDiscoveryResolver>(resolver);
		services.AddSingleton<IFreeboxCredentialStore>(store);
		using var handler = FixtureHandler.Authenticated((_, _) => Task.FromResult(FixtureHandler.Json(
			"""{"success":true,"result":{"enabled":true}}""")), provideDiscovery: false);
		services.AddFreeboxClient(options =>
		{
			options.ServerAddress = new Uri("https://fallback.example/");
			options.UseApplication("org.fixture");
		}).ConfigurePrimaryHttpMessageHandler(() => handler);
		await using var provider = services.BuildServiceProvider();

		var configuration = await provider.GetRequiredService<IFreeboxClient>().AirMedia.GetConfigurationAsync();

		Assert.True(configuration.Enabled);
		Assert.DoesNotContain(handler.Requests, request => request.Address.AbsolutePath is "/api_version");
		Assert.All(handler.Requests, request =>
			Assert.Equal("mdns.example:8443", request.Address.Authority));
	}

	[Fact]
	public void DnsSdStateMergesSplitPacketsAndRejectsAmbiguousUids()
	{
		var state = new MdnsDiscoveryState();
		state.Add([Pointer("Freebox._fbx-api._tcp.local.")]);
		Assert.Contains(state.GetMissingQuestions(), question => question.Type is MdnsRecordType.Service);
		state.Add([Service("Freebox._fbx-api._tcp.local."), Text("Freebox._fbx-api._tcp.local.", "mdns-uid")]);
		state.Add([Address("freebox.local.")]);

		var result = state.TryCreateResult();

		Assert.NotNull(result);
		Assert.Equal("mdns-uid", result.ApiVersion.Uid);

		var conflicting = new MdnsDiscoveryState();
		string other = "Other._fbx-api._tcp.local.";
		conflicting.Add([Pointer("Freebox._fbx-api._tcp.local."), Pointer(other)]);
		conflicting.Add([
			Service("Freebox._fbx-api._tcp.local."), Text("Freebox._fbx-api._tcp.local.", "uid-a"),
			Address("freebox.local."), Service(other, "other.local."), Text(other, "uid-b"),
			Address("other.local.")
		]);
		Assert.Null(conflicting.TryCreateResult());
	}

	[Fact]
	public void DnsWireParsesACompletePtrResponse()
	{
		using var stream = new MemoryStream();
		WriteNetworkUInt16(stream, 0);
		WriteNetworkUInt16(stream, 0x8400);
		WriteNetworkUInt16(stream, 0);
		WriteNetworkUInt16(stream, 1);
		WriteNetworkUInt16(stream, 0);
		WriteNetworkUInt16(stream, 0);
		WriteDnsName(stream, MdnsWire.ServiceName);
		WriteNetworkUInt16(stream, (ushort)MdnsRecordType.Pointer);
		WriteNetworkUInt16(stream, 1);
		WriteNetworkUInt32(stream, 120);
		using var target = new MemoryStream();
		WriteDnsName(target, "Freebox._fbx-api._tcp.local.");
		WriteNetworkUInt16(stream, checked((ushort)target.Length));
		stream.Write(target.ToArray());

		Assert.True(MdnsWire.TryParseRecords(stream.ToArray(), out var records));
		var record = Assert.Single(records);
		Assert.Equal(MdnsRecordType.Pointer, record.Type);
		Assert.Equal("Freebox._fbx-api._tcp.local.", record.Target);
	}

	[Fact]
	public void DnsSdStateRejectsMalformedAndTruncatedText()
	{
		var malformed = new MdnsDiscoveryState();
		malformed.Add([
			Pointer("Freebox._fbx-api._tcp.local."), Service("Freebox._fbx-api._tcp.local."),
			Address("freebox.local."), new MdnsRecord("Freebox._fbx-api._tcp.local.", MdnsRecordType.Text,
				120, Target: null, Port: null, [3, (byte)'a'], Address: null)
		]);

		Assert.Null(malformed.TryCreateResult());
		Assert.False(MdnsWire.TryParseRecords([0, 0, 0, 0], out _));
	}

	private static void WriteDnsName(Stream stream, string name)
	{
		foreach (string label in name.TrimEnd('.').Split('.'))
		{
			byte[] bytes = Encoding.ASCII.GetBytes(label);
			stream.WriteByte(checked((byte)bytes.Length));
			stream.Write(bytes);
		}

		stream.WriteByte(0);
	}

	private static void WriteNetworkUInt16(Stream stream, ushort value)
	{
		stream.WriteByte((byte)(value >> 8));
		stream.WriteByte((byte)value);
	}

	private static void WriteNetworkUInt32(Stream stream, uint value)
	{
		stream.WriteByte((byte)(value >> 24));
		stream.WriteByte((byte)(value >> 16));
		stream.WriteByte((byte)(value >> 8));
		stream.WriteByte((byte)value);
	}

	private static FreeboxDiscoveryResult ValidResult() => new(
		new Uri("https://mdns.example:8443/"),
		new ApiVersionResponse("mdns-uid", "Server", "fbxgw9-r1/full", "Ultra", "16.0", "mdns.example",
			"/api/", HttpsAvailable: true, 8443));

	private static MdnsRecord Pointer(string target) => new(MdnsWire.ServiceName, MdnsRecordType.Pointer, 120,
		target, Port: null, Text: null, Address: null);

	private static MdnsRecord Service(string owner, string target = "freebox.local.") => new(owner,
		MdnsRecordType.Service, 120, target, 8443, Text: null, Address: null);

	private static MdnsRecord Text(string owner, string uid)
	{
		string[] entries =
		[
			$"uid={uid}", "api_version=16.0", "api_base_url=/api/", "api_domain=mdns.example",
			"https_available=true", "https_port=8443", "device_name=Server", "box_model=fbxgw9-r1/full",
			"box_model_name=Ultra"
		];
		using var stream = new MemoryStream();
		foreach (string entry in entries)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(entry);
			stream.WriteByte(checked((byte)bytes.Length));
			stream.Write(bytes);
		}

		return new MdnsRecord(owner, MdnsRecordType.Text, 120, Target: null, Port: null, stream.ToArray(), Address: null);
	}

	private static MdnsRecord Address(string target) => new(target, MdnsRecordType.Address, 120, Target: null, Port: null, Text: null,
		IPAddress.Parse("192.0.2.1"));

	private sealed class FakeResolver(Func<CancellationToken, ValueTask<FreeboxDiscoveryResult?>> discover) :
		IFreeboxDiscoveryResolver
	{
		internal int CallCount { get; private set; }

		public async ValueTask<FreeboxDiscoveryResult?> DiscoverAsync(CancellationToken cancellationToken = default)
		{
			CallCount++;
			return await discover(cancellationToken);
		}
	}
}
