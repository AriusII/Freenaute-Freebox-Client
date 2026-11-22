namespace Freenaute.Freebox.Mapper.Tests;

public sealed class FreeplugContractTests
{
	[Fact]
	public void FreeplugNetwork_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical<FreeplugNetwork>(
			new FreeboxMapper(),
			"""{"id":"net-1","members":[{"id":"plug-1","local":true,"net_role":"cco","model":"fbox","eth_port_status":"up","eth_full_duplex":true,"has_network":true,"eth_speed":1000,"inactive":12,"net_id":"net-1","rx_rate":100,"tx_rate":80}]}""");

	[Fact]
	public void Freeplug_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical(
			"""{"id":"plug-1","local":false,"net_role":"sta","rx_rate":-1,"tx_rate":-1}""",
			NetworkJsonSerializerContext.Default.Freeplug);
}
