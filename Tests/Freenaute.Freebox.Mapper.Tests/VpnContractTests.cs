namespace Freenaute.Freebox.Mapper.Tests;

public sealed class VpnContractTests
{
	[Fact]
	public void VpnServerContractsRoundTripByteIdentically()
	{
		WireRoundTrip.AssertByteIdentical(
			"""{"name":"openvpn_routed","type":"openvpn","state":"started","connection_count":2,"auth_connection_count":1}""",
			NetworkJsonSerializerContext.Default.VPNServer);
		WireRoundTrip.AssertByteIdentical(
			"""{"login":"alice","password_set":true,"ip_reservation":"192.168.27.69"}""",
			NetworkJsonSerializerContext.Default.VPNUser);
		WireRoundTrip.AssertByteIdentical(
			"""{"login":"alice","ip":"192.168.27.69"}""",
			NetworkJsonSerializerContext.Default.VpnIpReservation);
	}

	[Fact]
	public void VpnServerConfigRoundTripsNestedConfigurations()
	{
		WireRoundTrip.AssertByteIdentical(
			"""{"id":"openvpn_routed","type":"openvpn","enabled":true,"enable_ipv4":true,"enable_ipv6":false,"port":1194,"min_port":1024,"max_port":65535,"port_ike":500,"port_nat":4500,"conf_pptp":{"mppe":"require","allowed_auth":{"pap":true}},"conf_openvpn":{"cipher":"aes256","disable_fragment":true,"use_tcp":false},"conf_ipsec":{"ike_version":2,"auth_modes":[{"id_source":"custom","id_custom":"fixture"}]},"ip_start":"192.168.27.65","ip_end":"192.168.27.95","ip6_start":"2001:db8::1","ip6_end":"2001:db8::ffff"}""",
			NetworkJsonSerializerContext.Default.VPNServerConfig);
	}

	[Fact]
	public void VpnConnectionAndIpPoolPreserveDocumentedShapes()
	{
		WireRoundTrip.AssertByteIdentical(
			"""{"id":"pptp-2","vpn":"pptp","user":"alice","authenticated":true,"auth_time":1392895603,"src_ip":"93.184.216.119","src_port":443,"local_ip":1921682765,"rx_bytes":94,"tx_bytes":95}""",
			NetworkJsonSerializerContext.Default.VPNConnection);
		WireRoundTrip.AssertByteIdentical(
			"""{"ip_start":"192.168.27.65","ip_end":"192.168.27.95","reservations":[{"login":"alice","ip":"192.168.27.69"}]}""",
			NetworkJsonSerializerContext.Default.VpnIpPool);
	}
}
