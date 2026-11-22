namespace Freenaute.Freebox.Mapper.Tests;

public sealed class IgdContractTests
{
	[Fact]
	public void UPnPIGDConfig_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical(
			"""{"enabled":true,"version":2}""",
			NetworkJsonSerializerContext.Default.UPnPIGDConfig);

	[Fact]
	public void IgdConfigurationPatch_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical(
			"""{"enabled":false,"version":1}""",
			NetworkJsonSerializerContext.Default.IgdConfigurationPatch);

	[Fact]
	public void UPnPRedir_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical(
			"""{"id":"redir-1","enabled":true,"ext_src_ip":"0.0.0.0","ext_port":443,"int_ip":"192.168.1.20","int_port":443,"proto":"tcp","desc":"https","remaining":3600}""",
			NetworkJsonSerializerContext.Default.UPnPRedir);
}
