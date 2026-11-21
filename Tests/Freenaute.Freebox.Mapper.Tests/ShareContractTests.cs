namespace Freenaute.Freebox.Mapper.Tests;

public sealed class ShareContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void ShareLink_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"token":"abc123","path":"RGlzcXVlIGR1cg==","name":"a.bin","expire":1700000000,"fullurl":"https://example.fbxos.fr/api/v4/share/abc123/"}""",
			FilesJsonSerializerContext.Default.ShareLink);

	[Fact]
	public void CreateShareLinkRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"path":"/Disque dur/a.bin","expire":1700000000}""",
			FilesJsonSerializerContext.Default.CreateShareLinkRequest);
}
