namespace Freenaute.Freebox.Mapper.Tests;

public sealed class DownloadsContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void Download_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"type":"http","name":"ubuntu.iso","status":"downloading","size":4000000000,"queue_pos":2,"io_priority":"normal","tx_bytes":1024,"rx_bytes":2048000,"tx_rate":128,"rx_rate":204800,"tx_pct":1,"rx_pct":42,"created_ts":1700000000,"eta":120,"download_dir":"RGlzcXVlIGR1cg==","stop_ratio":200,"info_hash":"abcdef","piece_length":262144}""",
			FilesJsonSerializerContext.Default.Download);

	[Fact]
	public void DownloadStats_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"nb_tasks":3,"nb_tasks_stopped":1,"nb_tasks_queued":1,"nb_tasks_downloading":1,"nb_tasks_active":2,"nb_rss":4,"nb_rss_items_unread":7,"rx_rate":204800,"tx_rate":128,"throttling_mode":"normal","throttling_is_scheduled":false,"throttling_rate":{"tx_rate":0,"rx_rate":1024},"nzb_config_status":{"status":"ok","error":""},"conn_ready":true,"nb_peer":5,"blocklist_entries":10,"blocklist_hits":2,"dht_stats":{"enabled":true,"node_count":100,"enabled_ipv6":false,"node_count_ipv6":5}}""",
			FilesJsonSerializerContext.Default.DownloadStats);

	[Fact]
	public void DownloadConfiguration_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"max_downloading_tasks":5,"download_dir":"RGlzcXVlIGR1cg==","watch_dir":"RGlzcXVlIGR1ci9WaWTDqW9z","use_watch_dir":true,"throttling":{"normal":{"tx_rate":0,"rx_rate":0},"slow":{"tx_rate":0,"rx_rate":1024},"schedule":["on","off"],"mode":"normal"},"news":{"server":"news.free.fr","port":119,"ssl":false,"user":"u","nthreads":4,"auto_repair":true,"lazy_par2":false,"auto_extract":true,"erase_tmp":false},"bt":{"max_peers":100,"stop_ratio":100,"crypto_support":"plain","enable_dht":true,"enable_pex":false,"announce_timeout":300,"main_port":6881,"dht_port":6881},"feed":{"fetch_interval":3600,"max_items":100},"blocklist":{},"dns1":"1.1.1.1","dns2":"8.8.8.8"}""",
			FilesJsonSerializerContext.Default.DownloadConfiguration);

	[Fact]
	public void DownloadFile_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":"0","task_id":7,"filepath":"RGlzcXVlIGR1ci9maWxl","name":"file.bin","mimetype":"application/octet-stream","size":1024,"rx":512,"status":"done","error":"","priority":"normal","preview_url":"https://example.fbxos.fr/preview/0"}""",
			FilesJsonSerializerContext.Default.DownloadFile);

	[Fact]
	public void DownloadThrottlingStatus_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"is_scheduled":false,"throttling":"normal"}""",
			FilesJsonSerializerContext.Default.DownloadThrottlingStatus);

	[Fact]
	public void UpdateDownloadRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"status":"stopped","queue_pos":3,"io_priority":"low"}""",
			FilesJsonSerializerContext.Default.UpdateDownloadRequest);

	[Fact]
	public void SetDownloadThrottlingRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"throttling":"schedule"}""",
			FilesJsonSerializerContext.Default.SetDownloadThrottlingRequest);

	[Fact]
	public void UpdateDownloadConfiguration_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"max_downloading_tasks":5,"use_watch_dir":true,"dns1":"1.1.1.1"}""",
			FilesJsonSerializerContext.Default.UpdateDownloadConfiguration);
}
