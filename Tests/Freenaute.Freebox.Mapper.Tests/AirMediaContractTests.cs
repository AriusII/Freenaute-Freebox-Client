namespace Freenaute.Freebox.Mapper.Tests;

public sealed class AirMediaContractTests
{
	private readonly FreeboxMapper _mapper = new();

	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void AirMediaReceiverRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"action":"start","media_type":"photo","media":"https://example.com/y.jpg","position":500}""",
			FreeboxJsonSerializerContext.Default.AirMediaReceiverRequest);

	[Fact]
	public void UpdateAirMediaConfigRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"enabled":true,"password":"secret"}""",
			FreeboxJsonSerializerContext.Default.UpdateAirMediaConfigRequest);

	[Fact]
	public void AirMediaReceiverResponse_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"name":"Freebox Player","password_protected":false,"capabilities":{"photo":true,"audio":true,"video":true,"screen":false}}""",
			FreeboxJsonSerializerContext.Default.AirMediaReceiverResponse);

	[Fact]
	public void AirMediaConfigResponse_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"enabled":true}""",
			FreeboxJsonSerializerContext.Default.AirMediaConfigResponse);

	[Fact]
	public void AirMediaErrorResponse_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"success":false,"error_code":"unknown_target"}""",
			FreeboxJsonSerializerContext.Default.AirMediaErrorResponse);

	[Fact]
	public void SuccessfulResponse_OfAirMediaReceiverResponse_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical<SuccessfulResponse<AirMediaReceiverResponse>>(_mapper,
			"""{"success":true,"result":{"name":"Freebox Player","password_protected":true,"capabilities":{"photo":true,"audio":false,"video":true,"screen":true}}}""");

	[Fact]
	public void SuccessfulResponse_OfAirMediaReceiverList_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical<SuccessfulResponse<List<AirMediaReceiverResponse>>>(_mapper,
			"""{"success":true,"result":[{"name":"Player 1","password_protected":false,"capabilities":{"photo":true,"audio":true,"video":false,"screen":false}},{"name":"Player 2","password_protected":false,"capabilities":{"photo":false,"audio":false,"video":true,"screen":true}}]}""");
}
