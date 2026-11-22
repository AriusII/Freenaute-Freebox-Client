namespace Freenaute.Freebox.Mapper.Tests;

public sealed class SwitchContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void SwitchPortStats_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"rx_bad_bytes":0,"rx_broadcast_packets":45,"rx_bytes_rate":608,"rx_err_packets":0,"rx_fcs_packets":0,"rx_fragments_packets":0,"rx_good_bytes":20018805,"rx_good_packets":114296,"rx_jabber_packets":0,"rx_multicast_packets":1217,"rx_oversize_packets":0,"rx_packets_rate":4,"rx_pause":0,"rx_undersize_packets":0,"rx_unicast_packets":113034,"tx_broadcast_packets":25895,"tx_bytes":25316860,"tx_bytes_rate":736,"tx_collisions":0,"tx_deferred":0,"tx_excessive":0,"tx_fcs":0,"tx_late":0,"tx_multicast_packets":27962,"tx_multiple":0,"tx_packets":166266,"tx_packets_rate":6,"tx_pause":0,"tx_single":0,"tx_unicast_packets":112409}""",
			NetworkJsonSerializerContext.Default.SwitchPortStats);

	[Fact]
	public void SwitchPortStatus_RoundTripsByteIdenticalWithMacList() =>
		AssertRoundTrip(
			"""{"id":2,"link":"up","duplex":"full","speed":"1000","mode":"full-duplex","mac_list":[{"mac":"aa:bb:cc:dd:ee:ff","name":"nas","hostname":"nas.local"}]}""",
			NetworkJsonSerializerContext.Default.SwitchPortStatus);
}
