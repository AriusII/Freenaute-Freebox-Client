namespace Freenaute.Freebox.Mapper.Tests;

public sealed class WifiContractTests
{
	private readonly FreeboxMapper _mapper = new();

	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void WifiGlobalConfig_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"enabled":true,"mac_filter_state":"blacklist"}""",
			NetworkJsonSerializerContext.Default.WifiGlobalConfig);

	[Fact]
	public void WifiGlobalState_ObjectOrArray_PreservesBothShapes()
	{
		WireRoundTrip.AssertByteIdentical<ObjectOrArray<WifiGlobalState>>(_mapper,
			"""{"state":"enabled","expected_phys":[{"band":"2d4g","phy_id":0,"detected":true}]}""");
		WireRoundTrip.AssertByteIdentical<ObjectOrArray<WifiGlobalState>>(_mapper,
			"""[{"state":"disabled"},{"state":"enabled","expected_phys":[{"band":"5g","phy_id":1,"detected":false}]}]""");
	}

	[Fact]
	public void WifiAp_RoundTripsByteIdentical_WithMixedCapabilityShapes() =>
		AssertRoundTrip(
			"""{"id":1,"name":"AP principal","status":{"state":"active","channel_width":"80","primary_channel":36,"secondary_channel":0,"dfs_cac_remaining_time":0,"dfs_disabled":false,"temp_disable_remaining_time":0},"capabilities":{"2d4g":{"ax":true,"he":false},"5g":7},"config":{"band":"5g","channel_width":"80","primary_channel":36,"secondary_channel":0,"dfs_enabled":true,"ht":{"ac_enabled":true}}}""",
			NetworkJsonSerializerContext.Default.WifiAp);

	[Fact]
	public void WifiBss_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"phy_id":"phy0","status":{"state":"up","sta_count":2,"authorized_sta_count":2,"custom_key_ssid":"","partners":[]},"use_shared_params":true,"config":{"enabled":true,"ssid":"Freebox-ABCD","hide_ssid":false,"encryption":"wpa2_psk_ccmp","eapol_version":2,"wps_enabled":false,"wps_uuid":""},"disable_wep":false}""",
			NetworkJsonSerializerContext.Default.WifiBss);

	[Fact]
	public void WifiStation_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":"aa:bb:cc:dd:ee:ff","mac":"aa:bb:cc:dd:ee:ff","bssid":"00:11:22:33:44:55","hostname":"phone","state":"connected","inactive":0,"conn_duration":3600,"rx_bytes":1000,"tx_bytes":2000,"tx_rate":100,"rx_rate":200,"signal":-55,"flags":{"legacy":false,"ht":true,"vht":true,"he":true,"authorized":true}}""",
			NetworkJsonSerializerContext.Default.WifiStation);

	[Fact]
	public void WifiPlanning_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"use_planning":true,"resolution":96,"mapping":["on","off","on"]}""",
			NetworkJsonSerializerContext.Default.WifiPlanning);

	[Fact]
	public void WifiMacFilter_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":"1","mac":"aa:bb:cc:dd:ee:ff","comment":"blocked","type":"blacklist","hostname":"phone"}""",
			NetworkJsonSerializerContext.Default.WifiMacFilter);

	[Fact]
	public void WifiWpsSession_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"bss_uuid":"uuid","ssid":"Freebox-ABCD","active":true,"result":"success","start_date":1,"end_date":2,"mac":"aa:bb:cc:dd:ee:ff"}""",
			NetworkJsonSerializerContext.Default.WifiWpsSession);

	[Theory]
	[InlineData("\"5\"")]
	[InlineData("5")]
	public void WifiCustomKeyParams_MaxUseCount_PreservesStringOrInteger(string maxUseCount) =>
		AssertRoundTrip(
			$$"""{"description":"invites","max_use_count":{{maxUseCount}},"duration":3600,"access_type":"net_only"}""",
			NetworkJsonSerializerContext.Default.WifiCustomKeyParams);

	[Fact]
	public void WifiCustomKey_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"remaining":3,"params":{"description":"invites","max_use_count":"5","duration":3600,"access_type":"net_only"},"users":[{"hostname":"phone","host":{"id":"aa:bb:cc:dd:ee:ff"}}]}""",
			NetworkJsonSerializerContext.Default.WifiCustomKey);

	[Fact]
	public void WifiChannelUsage_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"channel":36,"band":"5g","noise_level":-90,"rx_busy_percent":12}""",
			NetworkJsonSerializerContext.Default.WifiChannelUsage);

	[Fact]
	public void WifiWpsConfiguration_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"enabled":true}""",
			NetworkJsonSerializerContext.Default.WifiWpsConfiguration);

	[Fact]
	public void WifiGlobalConfigurationPatch_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"enabled":false,"mac_filter_state":"disabled"}""",
			NetworkJsonSerializerContext.Default.WifiGlobalConfigurationPatch);

	[Fact]
	public void WifiUnstableContracts_RoundTripByteIdentical()
	{
		AssertRoundTrip(
			"""{"ac_enabled":true}""",
			NetworkJsonSerializerContext.Default.WifiApHtConfig);
		AssertRoundTrip(
			"""{"legacy":false,"ht":true,"vht":true,"he":true,"authorized":true}""",
			NetworkJsonSerializerContext.Default.WifiStationFlags);
		AssertRoundTrip(
			"""{"bitrate":360,"mcs":7,"vht_mcs":9,"width":"80","shortgi":true}""",
			NetworkJsonSerializerContext.Default.WifiStationStats);
		AssertRoundTrip(
			"""{"2d4g":{"ax":true,"he":false},"5g":7,"60g":{}}""",
			NetworkJsonSerializerContext.Default.WifiApCapabilities);
	}
}
