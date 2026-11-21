namespace Freenaute.Freebox.Mapper.Tests;

public sealed class AuthContractTests
{
	private readonly FreeboxMapper _mapper = new();

	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void TokenRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"app_id":"fr.freebox.test","app_name":"Test","app_version":"1.0.0","device_name":"pc"}""",
			FreeboxJsonSerializerContext.Default.TokenRequest);

	[Fact]
	public void SessionStartRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"app_id":"fr.freebox.test","password":"0123456789abcdef0123456789abcdef01234567","app_version":"1.0.0"}""",
			FreeboxJsonSerializerContext.Default.SessionStartRequest);

	[Fact]
	public void SessionStartRequest_OmitsNullAppVersion() =>
		AssertRoundTrip(
			"""{"app_id":"fr.freebox.test","password":"0123456789abcdef0123456789abcdef01234567"}""",
			FreeboxJsonSerializerContext.Default.SessionStartRequest);

	[Fact]
	public void AuthorizeResponse_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"app_token":"tok123","track_id":12345}""",
			FreeboxJsonSerializerContext.Default.AuthorizeResponse);

	[Theory]
	[InlineData("unknown")]
	[InlineData("pending")]
	[InlineData("timeout")]
	[InlineData("granted")]
	[InlineData("denied")]
	public void AuthorizeTrackResponse_RoundTripsByteIdentical(string status) =>
		AssertRoundTrip(
			$$"""{"status":"{{status}}","challenge":"challenge123"}""",
			FreeboxJsonSerializerContext.Default.AuthorizeTrackResponse);

	[Fact]
	public void LoginResponse_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"logged_in":true,"challenge":"challenge123"}""",
			FreeboxJsonSerializerContext.Default.LoginResponse);

	[Fact]
	public void SessionResponse_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"session_token":"st123","challenge":"ch456","permissions":{"settings":true,"contacts":false,"calls":true,"explorer":true,"downloader":true,"profile":false,"camera":false}}""",
			FreeboxJsonSerializerContext.Default.SessionResponse);

	[Fact]
	public void SessionFailedModel_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"challenge":"ch789"}""",
			FreeboxJsonSerializerContext.Default.SessionFailedModel);

	[Fact]
	public void SuccessfulResponse_OfLoginResponse_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical<SuccessfulResponse<LoginResponse>>(_mapper,
			"""{"success":true,"result":{"logged_in":true,"challenge":"c1"}}""");

	[Fact]
	public void ErrorResponse_OfSessionFailedModel_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical<ErrorResponse<SessionFailedModel>>(_mapper,
			"""{"success":false,"error_code":"invalid_token","msg":"bad token","result":{"challenge":"c2"},"uid":"u1"}""");
}
