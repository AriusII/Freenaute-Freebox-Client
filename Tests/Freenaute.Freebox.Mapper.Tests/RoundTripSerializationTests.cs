namespace Freenaute.Freebox.Mapper.Tests;

public sealed class RoundTripSerializationTests
{
	[Fact]
	public void ApiVersionResponse_RoundTrips()
	{
		var original = new ApiVersionResponse(
			"uid",
			"device",
			"fbx",
			"Freebox",
			FreeboxApiVersion.Current,
			"example.com",
			"/api",
			true,
			443);

		string json = original.ToWire(FreeboxJsonSerializerContext.Default.ApiVersionResponse);
		var roundtrip = json.FromWire(FreeboxJsonSerializerContext.Default.ApiVersionResponse);

		Assert.NotNull(roundtrip);
		Assert.Equal(original.Uid, roundtrip!.Uid);
		Assert.Equal(original.ApiVersion, roundtrip.ApiVersion);
		Assert.Equal(original.ApiBaseUrl, roundtrip.ApiBaseUrl);
	}

	[Fact]
	public void StringOrInteger_RoundTrips()
	{
		var value = StringOrInteger.FromInteger(42);
		string json = value.ToWire(SharedJsonSerializerContext.Default.StringOrInteger);
		var roundtrip = json.FromWire(SharedJsonSerializerContext.Default.StringOrInteger);
		Assert.Equal(value, roundtrip);
	}

	[Fact]
	public void FreeboxMapper_UsesFrozenOptions()
	{
		var mapper = new FreeboxMapper();
		Assert.True(mapper.Options.IsReadOnly);
	}
}
