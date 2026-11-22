namespace Freenaute.Freebox.Mapper.Tests;

public sealed class DhcpContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void DhcpConfig_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"enabled":true,"sticky_assign":true,"gateway":"192.168.1.254","netmask":"255.255.255.0","ip_range_start":"192.168.1.10","ip_range_end":"192.168.1.200","always_broadcast":false,"ignore_out_of_range_hint":true,"boot_server":"","boot_file":"","dns":["192.168.1.254"],"options":[{"id":"6","val":"192.168.1.254"}]}""",
			NetworkJsonSerializerContext.Default.DhcpConfig);

	[Fact]
	public void DhcpStaticLease_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":"aa:bb:cc:dd:ee:ff","mac":"aa:bb:cc:dd:ee:ff","comment":"tv","hostname":"tv","ip":"192.168.1.50","options":[{"id":"6","val":"8.8.8.8"}]}""",
			NetworkJsonSerializerContext.Default.DhcpStaticLease);

	[Fact]
	public void DhcpDynamicLease_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"mac":"aa:bb:cc:dd:ee:ff","hostname":"tv","ip":"192.168.1.60","lease_remaining":3600,"assign_time":1,"refresh_time":2,"is_static":false,"options":[{"id":"6","val":"1.1.1.1"}]}""",
			NetworkJsonSerializerContext.Default.DhcpDynamicLease);

	[Fact]
	public void DhcpOption_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":"6","val":"1.1.1.1"}""",
			NetworkJsonSerializerContext.Default.DhcpOption);

	[Fact]
	public void DhcpOptionWrite_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":"6","val":"1.1.1.1"}""",
			NetworkJsonSerializerContext.Default.DhcpOptionWrite);

	[Fact]
	public void CreateDhcpStaticLeaseRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"mac":"aa:bb:cc:dd:ee:ff","comment":"tv","ip":"192.168.1.50","options":[{"id":"6","val":"8.8.8.8"}]}""",
			NetworkJsonSerializerContext.Default.CreateDhcpStaticLeaseRequest);

	[Fact]
	public void DhcpStaticLeasePatch_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"comment":"tv2","ip":"192.168.1.51"}""",
			NetworkJsonSerializerContext.Default.DhcpStaticLeasePatch);

	[Fact]
	public void DhcpConfigurationPatch_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"enabled":false,"ip_range_start":"192.168.1.20"}""",
			NetworkJsonSerializerContext.Default.DhcpConfigurationPatch);

	[Fact]
	public void DHCPv6Config_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"enabled":true,"use_custom_dns":false,"dns":["2a01::1"]}""",
			NetworkJsonSerializerContext.Default.DHCPv6Config);

	[Fact]
	public void DhcpV6ConfigurationPatch_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"enabled":false,"use_custom_dns":true,"dns":["2001:db8::1","2001:db8::2"]}""",
			NetworkJsonSerializerContext.Default.DhcpV6ConfigurationPatch);
}
