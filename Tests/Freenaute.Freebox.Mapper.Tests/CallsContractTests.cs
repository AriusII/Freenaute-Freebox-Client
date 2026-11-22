namespace Freenaute.Freebox.Mapper.Tests;

public sealed class CallsContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void CallEntry_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"type":"outgoing","datetime":1700000000,"number":"0102030405","name":"Bob","duration":60,"new":false,"contact_id":3,"line_id":1}""",
			ServicesJsonSerializerContext.Default.CallEntry);

	[Theory]
	[InlineData("missed")]
	[InlineData("accepted")]
	[InlineData("future_kind")]
	public void CallEntry_Type_PreservesKnownAndUnknownWireStrings(string type) =>
		AssertRoundTrip(
			$$"""{"id":2,"type":"{{type}}"}""",
			ServicesJsonSerializerContext.Default.CallEntry);

	[Fact]
	public void CallAccount_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"phone_number":"0102030405"}""",
			ServicesJsonSerializerContext.Default.CallAccount);

	[Fact]
	public void CallEntryPatch_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"new":true}""",
			ServicesJsonSerializerContext.Default.CallEntryPatch);

	[Fact]
	public void CallEntryPatch_ExplicitNullIsRejectedAndUnsetIsOmitted()
	{
		Assert.Throws<ArgumentException>(() => new CallEntryPatch { New = Optional<bool>.Null }.Validate());
		Assert.Equal("{}", JsonSerializer.Serialize(new CallEntryPatch(),
			ServicesJsonSerializerContext.Default.CallEntryPatch));
		Assert.Equal("{\"new\":false}", JsonSerializer.Serialize(new CallEntryPatch { New = false },
			ServicesJsonSerializerContext.Default.CallEntryPatch));
	}
}
