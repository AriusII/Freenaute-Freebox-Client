namespace Freenaute.Freebox.Mapper.Tests;

public sealed class LanContractTests
{
	private readonly FreeboxMapper _mapper = new();

	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void LanConfig_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"ip":"192.168.1.254","name":"Freebox","name_dns":"freebox","name_mdns":"Freebox","name_netbios":"FREEBOX","type":"router","mode":"router"}""",
			NetworkJsonSerializerContext.Default.LanConfig);

	[Fact]
	public void LanHost_RoundTripsByteIdentical_WithSingleL2Ident() =>
		AssertRoundTrip(
			"""{"id":"aa:bb:cc:dd:ee:ff","primary_name":"phone","domain_name":"phone.home","host_type":"smartphone","primary_name_manual":false,"l2ident":{"id":"aa:bb:cc:dd:ee:ff","type":"mac"},"vendor_name":"Apple","persistent":true,"reachable":true,"last_time_reachable":1700000000,"active":true,"last_activity":1700000001,"first_activity":1600000000,"names":[{"name":"phone","source":"mdns"}],"l3connectivities":[{"addr":"192.168.1.10","af":"ipv4","active":true,"reachable":true,"last_activity":1700000001,"last_time_reachable":1700000000,"model":""}],"network_control":{"profile_id":1,"name":"default","current_mode":"allowed"},"interface":"ethernet"}""",
			NetworkJsonSerializerContext.Default.LanHost);

	[Fact]
	public void LanHost_L2Ident_PreservesArrayShape() =>
		WireRoundTrip.AssertByteIdentical<LanHost>(_mapper,
			"""{"id":"x","l2ident":[{"id":"aa:bb:cc:dd:ee:ff","type":"mac"},{"id":"phone","type":"hostname"}]}""");

	[Fact]
	public void LanBrowserInterface_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"name":"ethernet","host_count":3}""",
			NetworkJsonSerializerContext.Default.LanBrowserInterface);

	[Fact]
	public void LanHostPatch_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"primary_name":"phone","host_type":"smartphone","persistent":true}""",
			NetworkJsonSerializerContext.Default.LanHostPatch);

	[Fact]
	public void LanConfigurationPatch_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"name":"Maison","name_dns":"maison"}""",
			NetworkJsonSerializerContext.Default.LanConfigurationPatch);

	[Fact]
	public void LanRouteWrite_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"prefix":"192.168.2.0/24","gateway":"192.168.1.1","enabled":true,"description":"route"}""",
			NetworkJsonSerializerContext.Default.LanRouteWrite);

	[Fact]
	public void WakeOnLanRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"mac":"aa:bb:cc:dd:ee:ff"}""",
			NetworkJsonSerializerContext.Default.WakeOnLanRequest);
}
