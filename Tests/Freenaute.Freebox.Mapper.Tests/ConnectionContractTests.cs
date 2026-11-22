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

	[Fact]
	public void FtthStatus_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"sfp_present":true,"sfp_alim_ok":true,"sfp_has_power_report":true,"sfp_has_signal":false,"link":false,"sfp_serial":"DE104900000471","sfp_model":"SPBD-1250E4H2RDB","sfp_vendor":"DELTA","sfp_pwr_tx":-1172,"sfp_pwr_rx":-3698}""",
			NetworkJsonSerializerContext.Default.FtthStatus);

	[Fact]
	public void XdslInfos_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"status":{"status":"showtime","protocol":"adsl2plus_a","modulation":"adsl","uptime":5017},"down":{"maxrate":30636,"rate":28031,"snr":7,"attn":0,"snr_10":70,"attn_10":0,"fec":0,"crc":0,"hec":0,"es":43,"ses":43,"phyr":true,"ginp":false,"nitro":true,"rxmt":0,"rxmt_corr":0,"rxmt_uncorr":0,"rtx_tx":0,"rtx_c":0,"rtx_uc":0},"up":{"maxrate":1022,"rate":1022,"snr":15,"attn":23,"snr_10":150,"attn_10":230,"fec":0,"crc":0,"hec":0,"es":0,"ses":0,"phyr":false,"ginp":false,"nitro":true,"rxmt":0,"rxmt_corr":0,"rxmt_uncorr":0,"rtx_tx":0,"rtx_c":0,"rtx_uc":0}}""",
			NetworkJsonSerializerContext.Default.XdslInfos);
}
