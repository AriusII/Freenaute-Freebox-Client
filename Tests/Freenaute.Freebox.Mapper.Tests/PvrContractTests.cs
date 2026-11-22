namespace Freenaute.Freebox.Mapper.Tests;

public sealed class PvrContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void PvrConfig_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"margin_before":10,"margin_after":5}""",
			ServicesJsonSerializerContext.Default.PvrConfig);

	[Fact]
	public void Precord_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":"190","media":"Disk","path":"Records","has_record_gen":true,"record_gen_id":10,"conflict":true,"overlap_list":[195],"enabled":true,"altered":false,"state":"waiting_start_time","error":"none","channel_uuid":"uuid-webtv-201","channel_name":"France 2","channel_quality":"hd","channel_type":"dvb","name":"Fixture","subname":"Episode","broadcast_type":"tv","start":1403755628,"end":1403755697,"legacy_uri":"","force_channel_name":""}""",
			ServicesJsonSerializerContext.Default.Precord);

	[Fact]
	public void Frecord_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":"236","media":"Disk","path":"Records","filename":"Fixture.ts","byte_size":4433869440,"has_record_gen":false,"record_gen_id":0,"altered":true,"state":"finished","error":"none","channel_uuid":"uuid-webtv-202","channel_name":"France 3","channel_quality":"auto","channel_type":"","name":"Fixture","subname":"Episode","broadcast_type":"tv","start":1428688800,"end":1428689400,"secure":false}""",
			ServicesJsonSerializerContext.Default.Frecord);

	[Fact]
	public void PvrMedia_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"media":"Disk","free_bytes":39700000000,"total_bytes":244950000000,"record_time":{"dvb":{"sd":48461,"hd":35245,"3d":35245},"iptv":{"ld":155078,"sd":110770,"hd":51012,"3d":51012}}}""",
			ServicesJsonSerializerContext.Default.PvrMedia);

	[Fact]
	public void PvrQuota_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"quota_exceeded":true,"needed_tresh":80,"cur_tresh":40}""",
			ServicesJsonSerializerContext.Default.PvrQuota);

	[Fact]
	public void PvrQuotaRequest_RoundTripsAsEmptyObject() =>
		AssertRoundTrip(
			"""{}""",
			ServicesJsonSerializerContext.Default.PvrQuotaRequest);
}
