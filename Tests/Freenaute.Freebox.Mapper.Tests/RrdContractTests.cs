namespace Freenaute.Freebox.Mapper.Tests;

public sealed class RrdContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void RRDFetch_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"db":"net","date_start":1,"date_end":2,"precision":100,"fields":["rx","tx"]}""",
			NetworkJsonSerializerContext.Default.RRDFetch);

	[Fact]
	public void RRDStats_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"date_start":1,"date_end":2,"data":[{"time":123,"rx":42}]}""",
			NetworkJsonSerializerContext.Default.RRDStats);
}
