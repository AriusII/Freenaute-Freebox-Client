namespace Freenaute.Freebox.Mapper.Tests;

public sealed class NatContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Theory]
	[InlineData("\"8080\"")]
	[InlineData("8080")]
	public void PortForwardingConfig_WanPortStart_PreservesStringOrInteger(string wanPortStart) =>
		AssertRoundTrip(
			$$"""{"id":1,"enabled":true,"ip_proto":"tcp","wan_port_start":{{wanPortStart}},"wan_port_end":8080,"lan_ip":"192.168.1.50","lan_port":80,"hostname":"tv","src_ip":"0.0.0.0","comment":"web"}""",
			NetworkJsonSerializerContext.Default.PortForwardingConfig);

	[Fact]
	public void CreatePortForwardingRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"enabled":true,"ip_proto":"udp","wan_port_start":53,"wan_port_end":53,"lan_ip":"192.168.1.50","lan_port":53,"src_ip":"0.0.0.0","comment":"dns"}""",
			NetworkJsonSerializerContext.Default.CreatePortForwardingRequest);

	[Fact]
	public void PortForwardingPatch_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"enabled":false,"comment":"off"}""",
			NetworkJsonSerializerContext.Default.PortForwardingPatch);
}
