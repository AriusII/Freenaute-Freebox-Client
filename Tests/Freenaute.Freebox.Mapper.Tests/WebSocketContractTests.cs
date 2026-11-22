namespace Freenaute.Freebox.Mapper.Tests;

public sealed class WebSocketContractTests
{
	private readonly FreeboxMapper _mapper = new();

	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void RegisterEventsRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"action":"register","request_id":1,"events":["downloads","wifi"]}""",
			WebSocketJsonSerializerContext.Default.RegisterEventsRequest);

	[Fact]
	public void RegisterEventsRequest_OmitsOptionalRequestId() =>
		AssertRoundTrip(
			"""{"action":"register","events":["downloads"]}""",
			WebSocketJsonSerializerContext.Default.RegisterEventsRequest);

	[Fact]
	public void FreeboxWebSocketMessage_Notification_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"action":"notification","success":true,"source":"wifi","event":"state_changed","result":{"state":"enabled"}}""",
			WebSocketJsonSerializerContext.Default.FreeboxWebSocketMessage);

	[Fact]
	public void FreeboxWebSocketMessage_Response_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"request_id":9,"action":"register","success":true,"result":{"registered":["downloads"]},"error_code":"","msg":""}""",
			WebSocketJsonSerializerContext.Default.FreeboxWebSocketMessage);

	[Fact]
	public void FileUploadStartRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"request_id":7,"action":"upload_start","size":1024,"dirname":"RGlzcXVlIGR1cg==","filename":"a.bin","force":"overwrite"}""",
			WebSocketJsonSerializerContext.Default.FileUploadStartRequest);

	[Fact]
	public void FileUploadStartRequest_OmitsOptionalRequestId() =>
		AssertRoundTrip(
			"""{"action":"upload_start","size":1024,"dirname":"RGlzcXVlIGR1cg==","filename":"a.bin","force":"overwrite"}""",
			WebSocketJsonSerializerContext.Default.FileUploadStartRequest);

	[Fact]
	public void FileUploadFinalizeRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"request_id":7,"action":"upload_finalize"}""",
			WebSocketJsonSerializerContext.Default.FileUploadFinalizeRequest);

	[Fact]
	public void FileUploadFinalizeRequest_OmitsOptionalRequestId() =>
		AssertRoundTrip(
			"""{"action":"upload_finalize"}""",
			WebSocketJsonSerializerContext.Default.FileUploadFinalizeRequest);

	[Fact]
	public void FileUploadCancelRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"request_id":7,"action":"upload_cancel"}""",
			WebSocketJsonSerializerContext.Default.FileUploadCancelRequest);

	[Fact]
	public void FileUploadCancelRequest_OmitsOptionalRequestId() =>
		AssertRoundTrip(
			"""{"action":"upload_cancel"}""",
			WebSocketJsonSerializerContext.Default.FileUploadCancelRequest);

	[Fact]
	public void FileUploadChunkResult_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"total_len":1024,"complete":true,"cancelled":false}""",
			WebSocketJsonSerializerContext.Default.FileUploadChunkResult);

	[Fact]
	public void FileUploadControlResponse_Conflict_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"request_id":7,"action":"upload_start","success":false,"result":{"total_len":512,"complete":false,"cancelled":false},"file_size":1024,"error_code":"conflict","msg":"file exists"}""",
			WebSocketJsonSerializerContext.Default.FileUploadControlResponse);

	[Fact]
	public void WebSocketRequest_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical<WebSocketRequest>(_mapper,
			"""{"request_id":1,"action":"register"}""");

	[Fact]
	public void WebSocketRequest_SupportsLargeRequestId() =>
		WireRoundTrip.AssertByteIdentical<WebSocketRequest>(_mapper,
			"""{"request_id":5000000000,"action":"register"}""");

	[Fact]
	public void WebSocketResponse_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical<WebSocketResponse>(_mapper,
			"""{"request_id":1,"action":"register","success":true,"result":{"ok":true}}""");

	[Fact]
	public void WebSocketNotification_RoundTripsByteIdentical() =>
		WireRoundTrip.AssertByteIdentical<WebSocketNotification>(_mapper,
			"""{"action":"notification","success":true,"source":"dl","event":"task_done","result":{"id":1}}""");
}
