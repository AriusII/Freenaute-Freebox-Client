namespace Freenaute.Freebox.Mapper.Tests;

public sealed class MapperAdapterTests
{
	[Fact]
	public void DefaultOptions_AreFrozenAndResolveEveryRetainedContext()
	{
		var mapper = new FreeboxMapper();
		JsonTypeInfo[] resolved =
		[
			mapper.GetTypeInfo<ApiVersionResponse>(),
			mapper.GetTypeInfo<StringOrInteger>(),
			mapper.GetTypeInfo<Download>(),
			mapper.GetTypeInfo<LanHost>(),
			mapper.GetTypeInfo<CallEntry>(),
			mapper.GetTypeInfo<NotificationTarget>(),
			mapper.GetTypeInfo<SystemModelInfo>(),
			mapper.GetTypeInfo<RegisterEventsRequest>()
		];

		Assert.True(mapper.Options.IsReadOnly);
		Assert.NotNull(mapper.Options.TypeInfoResolver);
		Assert.All(resolved, Assert.NotNull);
	}

	[Fact]
	public void GetTypeInfo_ReturnsCachedGeneratedMetadata()
	{
		var mapper = new FreeboxMapper();

		var first = mapper.GetTypeInfo<ApiVersionResponse>();
		var second = mapper.GetTypeInfo<ApiVersionResponse>();

		Assert.Same(first, second);
	}

	[Fact]
	public void GetTypeInfo_UnregisteredType_ThrowsClearError()
	{
		var mapper = new FreeboxMapper();

		var exception = Assert.Throws<NotSupportedException>(() => mapper.GetTypeInfo<Uri>());

		Assert.Contains("not registered", exception.Message, StringComparison.OrdinalIgnoreCase);
		Assert.Contains(typeof(Uri).FullName!, exception.Message, StringComparison.Ordinal);
	}

	[Fact]
	public void RegisteredType_RoundTripsThroughStringUtf8AndElementEntryPoints()
	{
		var mapper = new FreeboxMapper();
		var original = new ApiVersionResponse(
			"uid", "device", "fbx", "Freebox", FreeboxApiVersion.Current,
			"example.com", "/api", HttpsAvailable: true, 443);

		string json = mapper.ToWire(original);
		byte[] utf8 = mapper.ToUtf8Bytes(original);
		using var document = JsonDocument.Parse(json);

		var fromString = mapper.FromWire<ApiVersionResponse>(json);
		var fromUtf8 = mapper.FromWire<ApiVersionResponse>(utf8);
		var fromElement = mapper.FromWire<ApiVersionResponse>(document.RootElement);

		Assert.Equal(original, fromString);
		Assert.Equal(original, fromUtf8);
		Assert.Equal(original, fromElement);
	}

	[Fact]
	public void CurrentApiVersion_IsVersionFour()
	{
		Assert.Equal(4, FreeboxApiVersion.Major);
		Assert.Equal("v4", FreeboxApiVersion.Segment);
		Assert.Equal("4.0", FreeboxApiVersion.Current);
	}
}
