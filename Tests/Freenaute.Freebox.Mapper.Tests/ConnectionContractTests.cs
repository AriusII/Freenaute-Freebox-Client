namespace Freenaute.Freebox.Mapper.Tests;

public sealed class ConnectionContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void ConnectionStatus_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"state":"up","type":"ftth","media":"ftth","ipv4":"82.64.1.1","ipv6":"2a01::1","rate_up":1024,"rate_down":2048,"bandwidth_up":1000000,"bandwidth_down":10000000,"bytes_up":1000,"bytes_down":2000,"ipv4_port_range":[1024,2048]}""",
			NetworkJsonSerializerContext.Default.ConnectionStatus);

	[Fact]
	public void ConnectionConfiguration_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"ping":true,"is_secure_pass":true,"remote_access":true,"wol":false,"adblock":true,"adblock_not_set":false,"allow_token_request":false,"sip_alg":"none"}""",
			NetworkJsonSerializerContext.Default.ConnectionConfiguration);

	[Fact]
	public void ConnectionConfigurationPatch_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"remote_access":false,"wol":true}""",
			NetworkJsonSerializerContext.Default.ConnectionConfigurationPatch);

	[Fact]
	public void ConnectionIpv6Configuration_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"ipv6_enabled":true,"ipv6_firewall":false,"ipv6_prefix_firewall":false,"ipv6ll":"fe80::1","delegations":[{"prefix":"2a01::/64","next_hop":"fe80::1"}]}""",
			NetworkJsonSerializerContext.Default.ConnectionIpv6Configuration);
}
