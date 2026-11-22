namespace Freenaute.Freebox.Mapper.Tests;

public sealed class VpnClientContractTests
{
	[Fact]
	public void VpnClientConfig_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical(
			"""{"id":"vpn0","description":"Fixture VPN","type":"pptp","active":true,"conf_pptp":{"remote_host":"vpn.example.org","username":"alice","password":"","mppe":"require","allowed_auth":{"mschapv2":true}}}""",
			NetworkJsonSerializerContext.Default.VPNClientConfig);

	[Fact]
	public void VpnClientStatus_AllowsObjectRoutesShape()
	{
		var status = JsonSerializer.Deserialize(
			"""{"enabled":true,"active_vpn":"vpn0","active_vpn_description":"Fixture VPN","type":"pptp","state":"up","last_up":2,"last_try":1,"next_try":0,"last_error":"none","stats":{"rate_up":0,"rate_down":1,"bytes_up":2,"bytes_down":3},"ipv4":{"config_valid":true,"ip_mask":{"ip":"192.168.27.65","mask":"255.255.255.255"},"domain":"","gateway":"212.27.38.253","dns":["212.27.38.253"],"provider":"ppp","routes":{},"dhcp":{"state":"down"}}}""",
			NetworkJsonSerializerContext.Default.VPNClientStatus);

		Assert.NotNull(status);
		Assert.Equal(JsonValueKind.Object, status.Ipv4!.Routes.ValueKind);
		Assert.Equal(3, status.Stats!.BytesDown);
		Assert.Equal("none", status.LastError);
		Assert.Equal("Fixture VPN", status.ActiveVpnDescription);
		Assert.Equal("212.27.38.253", status.Ipv4.Gateway);
		Assert.Equal("212.27.38.253", Assert.Single(status.Ipv4.Dns!));
	}

	[Fact]
	public void VpnClientStatus_AllowsArrayRoutesShape()
	{
		var status = JsonSerializer.Deserialize(
			"""{"enabled":false,"active_vpn":"","active_vpn_description":"","type":"openvpn","state":"down","last_up":0,"last_try":4,"next_try":30,"last_error":"connect_failed","stats":{"rate_up":1,"rate_down":2,"bytes_up":3,"bytes_down":4},"ipv4":{"config_valid":false,"ip_mask":{},"domain":"","gateway":"","dns":[],"provider":"none","routes":[{"destination":"10.0.0.0","gateway":"192.168.1.1"}],"dhcp":{}}}""",
			NetworkJsonSerializerContext.Default.VPNClientStatus);

		Assert.NotNull(status);
		Assert.False(status.Enabled);
		Assert.Equal("openvpn", status.Type);
		Assert.Equal("connect_failed", status.LastError);
		Assert.Equal(JsonValueKind.Array, status.Ipv4!.Routes.ValueKind);
		Assert.Equal(1, status.Ipv4.Routes.GetArrayLength());
		Assert.Equal(1, status.Stats!.RateUp);
		Assert.Empty(status.Ipv4.Dns!);
	}
}
