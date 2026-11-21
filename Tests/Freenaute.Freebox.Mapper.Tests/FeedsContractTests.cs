namespace Freenaute.Freebox.Mapper.Tests;

public sealed class FeedsContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void DownloadFeed_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"status":"ok","url":"https://example.com/rss","title":"Feed","desc":"d","image_url":"https://example.com/i.png","nb_read":5,"nb_unread":2,"auto_download":true,"fetch_ts":1700000000,"pub_ts":1700000001,"error":""}""",
			FilesJsonSerializerContext.Default.DownloadFeed);

	[Fact]
	public void DownloadFeedItem_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":10,"feed_id":1,"title":"t","desc":"d","author":"a","link":"https://example.com/1","is_read":false,"is_downloaded":true,"fetch_ts":1,"pub_ts":2,"enclosure_url":"https://example.com/e.mp3","enclosure_type":"audio/mpeg","enclosure_length":12345}""",
			FilesJsonSerializerContext.Default.DownloadFeedItem);

	[Fact]
	public void DownloadFeedMutation_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"feed_id":1,"id":2,"status":"ok","url":"https://example.com/rss","auto_download":false}""",
			FilesJsonSerializerContext.Default.DownloadFeedMutation);

	[Fact]
	public void CreateDownloadFeedRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"url":"https://example.com/rss"}""",
			FilesJsonSerializerContext.Default.CreateDownloadFeedRequest);

	[Fact]
	public void UpdateDownloadFeedRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"auto_download":true}""",
			FilesJsonSerializerContext.Default.UpdateDownloadFeedRequest);

	[Fact]
	public void MarkDownloadFeedItemRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"is_read":true}""",
			FilesJsonSerializerContext.Default.MarkDownloadFeedItemRequest);
}
