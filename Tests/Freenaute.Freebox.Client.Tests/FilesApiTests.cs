#pragma warning disable CS0618

namespace Freenaute.Freebox.Client.Tests;

public sealed class FilesApiTests
{
	[Fact]
	public async Task ListingPreservesBase64AndOpaqueCursorAndReturnsPagingObject()
	{
		// A valid Base64 value containing '/', '+' and padding is intentionally not Base64URL-normalized.
		var path = EncodedFreeboxPath.FromEncoded("L9+/w6k=");
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"entries":[{"name":"large","path":"L9+/w6k=","size":5000000000,"type":"future-kind"}],"cursor":"next/+&="}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var files = Create(client);
		var resource = files.FileSystem.At(path);
		Assert.Empty(handler.Requests);

		var listing = await resource.ListAsync(new FileListingOptions
		{
			OnlyFolders = false,
			CountSubFolders = true,
			ExifMode = "light",
			Limit = 100,
			Cursor = "opaque/+&=%"
		});

		Assert.Equal("next/+&=", listing.Cursor);
		var entry = Assert.Single(listing.Entries!);
		Assert.Equal(5_000_000_000, entry.Size);
		Assert.Equal("future-kind", entry.Type);
		Assert.Equal(path, entry.Path);
		var request = ResourceRequests(handler).Single();
		Assert.Equal(HttpMethod.Get, request.Method);
		Assert.Equal(
			"https://fixture.example/api/v16/fs/ls/L9%2B%2Fw6k%3D?onlyFolder=false&countSubFolder=true&exifMode=light&limit=100&cursor=opaque%2F%2B%26%3D%25",
			request.Address.AbsoluteUri);
	}
#pragma warning restore CS0618

	[Fact]
	public async Task FileCopyReturnsAsyncTaskWithoutPollingAndKeepsPathsAsEncodedStrings()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"id":43,"type":"cp","state":"running","progress":24,"src":["/plain/source"],"dst":"/plain/destination"}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var source = EncodedFreeboxPath.FromUtf8Path("/Disque dur/e\u0301té.bin");
		var destination = EncodedFreeboxPath.FromUtf8Path("/Disque dur/archive");

		var task = await Create(client).FileSystem.CopyAsync(new FileTransferRequest
		{
			Files = [source],
			Dst = destination,
			Mode = FileConflictMode.Both
		});

		Assert.Equal("running", task.State);
		Assert.Equal(24, task.Progress); // Preserve the documented raw counter; no inferred percentage normalization.
		Assert.Equal("/plain/destination", task.Dst);
		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal("/api/v16/fs/cp/", request.Address.AbsolutePath);
		using var body = JsonDocument.Parse(request.Body!);
		Assert.Equal(source.Value, body.RootElement.GetProperty("files")[0].GetString());
		Assert.Equal(destination.Value, body.RootElement.GetProperty("dst").GetString());
		Assert.Equal("both", body.RootElement.GetProperty("mode").GetString());
	}

	[Fact]
	public async Task UrlSubmissionUsesFormEncodingAndImmutableDeferredBuilder()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"id":5000000000}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var original = Create(client).Downloads.FromUrl("https://source.example/video?a=1&b=é+space");
		var customized = original.WithFileName("épisode.mp4").Recursively(recursive: false)
			.ToDirectory(EncodedFreeboxPath.FromUtf8Path("/Disque dur/videos"));
		Assert.Empty(handler.Requests);

		var result = await customized.AddAsync();
		await original.AddAsync();

		Assert.False(result.WasMultiple);
		Assert.Equal(5_000_000_000, Assert.Single(result.Ids));
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(2, requests.Length);
		Assert.All(requests, request =>
		{
			Assert.Equal(HttpMethod.Post, request.Method);
			Assert.Equal("/api/v16/downloads/add", request.Address.AbsolutePath);
			Assert.Equal("application/x-www-form-urlencoded", request.ContentType);
		});
		var first = DecodeForm(requests[0].Body!);
		Assert.Equal("https://source.example/video?a=1&b=é+space", first["download_url"]);
		Assert.Equal("épisode.mp4", first["filename"]);
		Assert.Equal("false", first["recursive"]);
		Assert.Equal(EncodedFreeboxPath.FromUtf8Path("/Disque dur/videos").Value, first["download_dir"]);
		Assert.Single(DecodeForm(requests[1].Body!));
	}

	[Fact]
	public async Task MultipleUrlsUseDocumentedNewlineDelimiterAndReadIntegerArrayResult()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"id":[42,43]}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		string[] urls = ["https://one.example/?q=a&b=2", "magnet:?xt=urn:btih:fixture"];

		var result = await Create(client).Downloads.FromUrls(urls).AddAsync();

		Assert.True(result.WasMultiple);
		Assert.Equal([42, 43], result.Ids);
		Assert.Equal(string.Join('\n', urls),
			DecodeForm(Assert.Single(ResourceRequests(handler)).Body!)["download_url_list"]);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task MultipartDownloadDescriptorHasExplicitStreamOwnership(bool leaveOpen)
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"id":42}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		using var stream = new MemoryStream([.. "torrent fixture bytes"u8]);

		await Create(client).Downloads.AddFileAsync(stream, "fixture.torrent",
			EncodedFreeboxPath.FromUtf8Path("/Disque dur/torrents"), "fixture-password", leaveOpen);

		Assert.Equal(leaveOpen, stream.CanRead);
		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal("multipart/form-data", request.ContentType);
		Assert.Contains("name=download_file", request.Body!, StringComparison.OrdinalIgnoreCase);
		Assert.Contains("filename=fixture.torrent", request.Body!, StringComparison.OrdinalIgnoreCase);
		Assert.Contains("torrent fixture bytes", request.Body!, StringComparison.OrdinalIgnoreCase);
		Assert.Contains("name=download_dir", request.Body!, StringComparison.OrdinalIgnoreCase);
		Assert.Contains("name=archive_password", request.Body!, StringComparison.OrdinalIgnoreCase);
	}

	[Fact]
	public async Task SubmissionRejectsAmbiguousOrUnsupportedOverridesBeforeAnyIo()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var downloads = Create(client).Downloads;

		await Assert.ThrowsAsync<ArgumentException>(async () => await downloads.AddUrlAsync(new DownloadUrlRequest
		{
			DownloadUrl = "https://one.example/",
			DownloadUrls = ["https://two.example/"]
		}));
		await Assert.ThrowsAsync<ArgumentException>(async () => await downloads
			.FromUrls("https://one.example/", "https://two.example/")
			.WithFileName("one-file").AddAsync());
		await Assert.ThrowsAsync<ArgumentException>(async () => await downloads.FromUrl("https://one.example/").Recursively()
			.WithHash("sha256:fixture").AddAsync());
		Assert.Empty(handler.Requests);
	}

	[Theory]
	[InlineData("{\"id\":42,\"status\":\"future\"}", true)]
	[InlineData("[{\"id\":42,\"status\":\"future\"}]", false)]
	public async Task DownloadCollectionPreservesBothAttestedResultShapes(string result, bool objectShape)
	{
		using var handler = FixtureHandler.AuthenticatedConstant("{\"success\":true,\"result\":" + result + "}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var downloads = await Create(client).Downloads.GetAllAsync();

		Assert.Equal(objectShape, downloads.IsObject);
		Assert.Equal("future", Assert.Single(downloads.Items).Status);
	}

	[Fact]
	public void ContradictoryDownloadFileKindsRemainDistinctWithGeneratedJson()
	{
		Assert.False(JsonSerializer.IsReflectionEnabledByDefault);
		var file = JsonSerializer.Deserialize(
			"""{"id":"opaque","task_id":"42","filepath":"Lw==","path":"deprecated"}""",
			FilesJsonSerializerContext.Default.DownloadFile)!;
		Assert.Equal("42", file.TaskId?.String);
		Assert.Equal(EncodedFreeboxPath.FromEncoded("Lw=="), file.Filepath);
		using var serialized =
			JsonDocument.Parse(JsonSerializer.Serialize(file, FilesJsonSerializerContext.Default.DownloadFile));
		Assert.False(serialized.RootElement.TryGetProperty("path", out _));
	}

	[Fact]
	public void DeprecatedUploadAndContractMembersCarryObsoleteAnnotations()
	{
#pragma warning disable CS0618
		var uploadType = typeof(FreeboxFileUploadsApi);
		Assert.NotNull(Attribute.GetCustomAttribute(uploadType, typeof(ObsoleteAttribute)));
#pragma warning restore CS0618

		Assert.NotNull(Attribute.GetCustomAttribute(
			typeof(IFreeboxFilesApi).GetProperty("Uploads")!,
			typeof(ObsoleteAttribute)));
		Assert.NotNull(Attribute.GetCustomAttribute(
			typeof(FreeboxFilesApi).GetProperty("Uploads")!,
			typeof(ObsoleteAttribute)));

		Assert.NotNull(Attribute.GetCustomAttribute(
			typeof(DownloadFile).GetProperty("Path")!,
			typeof(ObsoleteAttribute)));
		Assert.NotNull(Attribute.GetCustomAttribute(
			typeof(ParentalFilter).GetProperty("Ip")!,
			typeof(ObsoleteAttribute)));
	}

	[Fact]
	public async Task ConfigurationPatchKeepsFalseAndZeroWhileOmittingUnsetAndReadOnlyFields()
	{
		using var handler =
			FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"use_watch_dir":false}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		await Create(client).Downloads.UpdateConfigurationAsync(new UpdateDownloadConfiguration
		{
			UseWatchDir = false,
			MaxDownloadingTasks = 0,
			News = new UpdateDlNewsConfig
			{
				Password = "fixture-password",
				Ssl = false,
				Port = StringOrInteger.FromString("119")
			},
			Blocklist = new UpdateDlBlockListConfig
			{
				Sources = Optional<string[]>.FromValue(["http://list.example/cidr"])
			}
		});

		using var body = JsonDocument.Parse(Assert.Single(ResourceRequests(handler)).Body!);
		Assert.Equal(4, body.RootElement.EnumerateObject().Count());
		Assert.False(body.RootElement.GetProperty("use_watch_dir").GetBoolean());
		Assert.Equal(0, body.RootElement.GetProperty("max_downloading_tasks").GetInt64());
		Assert.Equal("119", body.RootElement.GetProperty("news").GetProperty("port").GetString());
		Assert.Equal("http://list.example/cidr",
			body.RootElement.GetProperty("blocklist").GetProperty("sources")[0].GetString());
		Assert.False(body.RootElement.TryGetProperty("download_dir", out _));
		await Assert.ThrowsAsync<ArgumentException>(async () => await Create(client).Downloads.UpdateConfigurationAsync(
			new UpdateDownloadConfiguration { Dns1 = Optional<string>.Null }));
	}

	[Fact]
	public async Task FeedMutationIdIsNotSilentlyCoalescedIntoReadId()
	{
		using var handler =
			FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"feed_id":42,"title":"Fixture"}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		var feed = await Create(client).Downloads.Feeds
			.CreateAsync(new CreateDownloadFeedRequest { Url = "https://feed.example/rss" });

		Assert.Equal(42, feed.FeedId);
		Assert.Null(feed.Id);
		var request = Assert.Single(ResourceRequests(handler));
		Assert.Equal(HttpMethod.Post, request.Method);
		Assert.Equal("/api/v16/downloads/feeds/", request.Address.AbsolutePath);
	}

	[Fact]
	public async Task DownloadConfigurationAndFeedRoutesUseDocumentedPaths()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(
			request.Address.AbsolutePath switch
			{
				"/api/v16/downloads/config/" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json(
						"""{"success":true,"result":{"max_downloading_tasks":3,"use_watch_dir":false,"blocklist":{"sources":["http://block.example/cidr"]}}}"""),
				"/api/v16/downloads/config/" when request.Method == HttpMethod.Put =>
					FixtureHandler.Json(
						"""{"success":true,"result":{"max_downloading_tasks":0,"use_watch_dir":false}}"""),
				"/api/v16/downloads/throttling" =>
					FixtureHandler.Json("""{"success":true,"result":{"is_scheduled":false,"throttling":"slow"}}"""),
				"/api/v16/downloads/feeds/" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json(
						"""{"success":true,"result":[{"id":1,"status":"ready","url":"https://feed.example/rss","auto_download":true}]}"""),
				"/api/v16/downloads/feeds/" when request.Method == HttpMethod.Post =>
					FixtureHandler.Json(
						"""{"success":true,"result":{"feed_id":42,"status":"ready","url":"https://feed.example/rss","auto_download":false}}"""),
				"/api/v16/downloads/feeds/1" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json(
						"""{"success":true,"result":{"id":1,"status":"ready","url":"https://feed.example/rss","auto_download":true}}"""),
				"/api/v16/downloads/feeds/1" when request.Method == HttpMethod.Put =>
					FixtureHandler.Json("""{"success":true,"result":{"feed_id":1,"auto_download":false}}"""),
				"/api/v16/downloads/feeds/1" when request.Method == HttpMethod.Delete =>
					FixtureHandler.Json("""{"success":true}"""),
				"/api/v16/downloads/feeds/1/fetch" =>
					FixtureHandler.Json("""{"success":true}"""),
				"/api/v16/downloads/feeds/fetch" =>
					FixtureHandler.Json("""{"success":true}"""),
				"/api/v16/downloads/feeds/1/items/" =>
					FixtureHandler.Json(
						"""{"success":true,"result":[{"id":10,"feed_id":1,"title":"Fixture","is_read":false,"is_downloaded":false}]}"""),
				"/api/v16/downloads/feeds/1/items/10" =>
					FixtureHandler.Json("""{"success":true}"""),
				"/api/v16/downloads/feeds/1/items/10/download" =>
					FixtureHandler.Json("""{"success":true}"""),
				"/api/v16/downloads/feeds/1/items/mark_all_as_read" =>
					FixtureHandler.Json("""{"success":true}"""),
				_ => FixtureHandler.Json("""{"success":true}""")
			}));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var downloads = Create(client).Downloads;

		var configuration = await downloads.GetConfigurationAsync();
		Assert.Equal(3, configuration.MaxDownloadingTasks);
		Assert.Equal("http://block.example/cidr", Assert.Single(configuration.Blocklist!.Sources!));

		var updated = await downloads.UpdateConfigurationAsync(new UpdateDownloadConfiguration
		{
			MaxDownloadingTasks = 0,
			UseWatchDir = false
		});
		Assert.Equal(0, updated.MaxDownloadingTasks);

		var throttling = await downloads.SetThrottlingAsync(new SetDownloadThrottlingRequest
		{
			Throttling = DownloadThrottlingMode.Slow
		});
		Assert.False(throttling.IsScheduled);
		Assert.Equal("slow", throttling.Throttling);

		var feeds = await downloads.Feeds.GetAllAsync();
		Assert.Equal("ready", Assert.Single(feeds).Status);
		var feed = await downloads.Feeds.GetAsync(1);
		Assert.Equal(1, feed.Id);
		var created = await downloads.Feeds.CreateAsync(new CreateDownloadFeedRequest
		{
			Url = "https://feed.example/rss"
		});
		Assert.Equal(42, created.FeedId);
		var mutation = await downloads.Feeds.UpdateAsync(1, new UpdateDownloadFeedRequest { AutoDownload = false });
		Assert.False(mutation.AutoDownload);
		await downloads.Feeds.DeleteAsync(1);
		await downloads.Feeds.FetchAsync(1);
		await downloads.Feeds.FetchAllAsync();
		var items = await downloads.Feeds.GetItemsAsync(1);
		Assert.False(Assert.Single(items).IsRead);
		await downloads.Feeds.MarkItemAsync(1, 10, new MarkDownloadFeedItemRequest { IsRead = false });
		await downloads.Feeds.DownloadItemAsync(1, 10);
		await downloads.Feeds.MarkAllAsReadAsync(1);

		var requests = ResourceRequests(handler).ToArray();
		Assert.Contains(requests, request =>
			request.Method == HttpMethod.Get &&
			request.Address.AbsolutePath == "/api/v16/downloads/config/");
		Assert.Contains(requests, request =>
			request.Method == HttpMethod.Put &&
			request.Address.AbsolutePath == "/api/v16/downloads/config/");
		Assert.Contains(requests, request =>
			request.Address.AbsolutePath == "/api/v16/downloads/throttling");
		Assert.Contains(requests, request =>
			request.Address.AbsolutePath == "/api/v16/downloads/feeds/1/items/10/download");
		Assert.Contains(requests, request =>
			request.Address.AbsolutePath == "/api/v16/downloads/feeds/1/items/mark_all_as_read");
	}

	[Fact]
	public async Task DownloadUnstableRoutesUseDocumentedPathsAndTypedPayloads()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(
			request.Address.AbsolutePath switch
			{
				"/api/v16/downloads/blacklist/" when request.Method == HttpMethod.Post =>
					FixtureHandler.Json(
						"""{"success":true,"result":{"host":"tracker.example","reason":"user","expire":120,"global":true}}"""),
				"/api/v16/downloads/8/blacklist/" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json(
						"""{"success":true,"result":[{"host":"tracker.example","reason":"timeout","expire":120,"global":true}]}"""),
				"/api/v16/downloads/8/blacklist/empty/" when request.Method == HttpMethod.Delete =>
					FixtureHandler.Json("""{"success":true}"""),
				"/api/v16/downloads/8/peers/" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json(
						"""{"success":true,"result":[{"host":"198.51.100.1","port":51413,"state":"ready","origin":"tracker","protocol":"tcp","client":"Fixture","country_code":"FR","tx":1,"rx":2,"tx_rate":3,"rx_rate":4,"progress":91,"requests":[5,6]}]}"""),
				"/api/v16/downloads/8/trackers/" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json(
						"""{"success":true,"result":[{"announce":"udp://tracker.example:80","is_backup":false,"status":"announced","interval":1800,"min_interval":900,"reannounce_in":300,"nseeders":10,"nleechers":2,"is_enabled":true}]}"""),
				"/api/v16/downloads/8/trackers/" when request.Method == HttpMethod.Post =>
					FixtureHandler.Json("""{"success":true}"""),
				"/api/v16/downloads/8/trackers/udp%3A%2F%2Ftracker.example%3A80/" when request.Method == HttpMethod.Put
					=>
					FixtureHandler.Json("""{"success":true}"""),
				"/api/v16/downloads/8/trackers/udp%3A%2F%2Ftracker.example%3A80/" when request.Method ==
					HttpMethod.Delete =>
					FixtureHandler.Json("""{"success":true}"""),
				"/api/v16/downloads/8/pieces" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json("""{"success":true,"result":"XX+.-/U"}"""),
				_ => FixtureHandler.Json("""{"success":true}""")
			}));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var downloads = Create(client).Downloads;

		var blacklistEntry = await downloads.AddBlacklistHostAsync(new AddDownloadBlacklistEntryRequest
		{
			Host = "tracker.example",
			Expire = 120
		});
		await downloads.RemoveBlacklistHostAsync("tracker.example");
		var blacklist = await downloads.GetBlacklistAsync(8);
		await downloads.ClearBlacklistAsync(8);
		var peers = await downloads.GetPeersAsync(8);
		var trackers = await downloads.GetTrackersAsync(8);
		await downloads.AddTrackerAsync(8, new AddDownloadTrackerRequest { Announce = "udp://tracker.example:80" });
		await downloads.UpdateTrackerAsync(8, "udp://tracker.example:80",
			new UpdateDownloadTrackerRequest { Announce = "udp://tracker.example:80", IsEnabled = false });
		await downloads.DeleteTrackerAsync(8, "udp://tracker.example:80");
		string pieces = await downloads.GetPiecesAsync(8);

		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal("tracker.example", blacklistEntry.Host);
		Assert.Equal(120, blacklistEntry.Expire);
		Assert.Equal("timeout", Assert.Single(blacklist).Reason);
		Assert.Equal(91, Assert.Single(peers).Progress);
		Assert.Equal("announced", Assert.Single(trackers).Status);
		Assert.Equal("XX+.-/U", pieces);
		Assert.Equal(10, requests.Length);
		Assert.Equal("/api/v16/downloads/blacklist/", requests[0].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/blacklist/tracker.example/", requests[1].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/8/blacklist/", requests[2].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/8/blacklist/empty/", requests[3].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/8/peers/", requests[4].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/8/trackers/", requests[5].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/8/trackers/", requests[6].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/8/trackers/udp%3A%2F%2Ftracker.example%3A80/",
			requests[7].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/8/trackers/udp%3A%2F%2Ftracker.example%3A80/",
			requests[8].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Delete, requests[8].Method);
		Assert.Equal("/api/v16/downloads/8/pieces", requests[9].Address.AbsolutePath);
		using var postBody = JsonDocument.Parse(requests[0].Body!);
		Assert.Equal("tracker.example", postBody.RootElement.GetProperty("host").GetString());
		Assert.Equal(120, postBody.RootElement.GetProperty("expire").GetInt64());
		using var addTrackerBody = JsonDocument.Parse(requests[6].Body!);
		Assert.Equal("udp://tracker.example:80", addTrackerBody.RootElement.GetProperty("announce").GetString());
		using var updateTrackerBody = JsonDocument.Parse(requests[7].Body!);
		Assert.Equal("udp://tracker.example:80", updateTrackerBody.RootElement.GetProperty("announce").GetString());
		Assert.False(updateTrackerBody.RootElement.GetProperty("is_enabled").GetBoolean());
	}

	[Fact]
	public async Task DownloadUnstableTrackerPatchRejectsNullClearingBeforeAnyIo()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		await Assert.ThrowsAsync<ArgumentException>(async () => await Create(client).Downloads.UpdateTrackerAsync(8,
			"udp://tracker.example:80", new UpdateDownloadTrackerRequest { IsEnabled = Optional<bool>.Null }));
		Assert.Empty(handler.Requests);
	}

	[Fact]
	public async Task StableDownloadRoutesUseDocumentedPathsAndTypedPayloads()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(
			request.Address.AbsolutePath switch
			{
				"/api/v16/downloads/8" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json(
						"""{"success":true,"result":{"id":8,"type":"bt","name":"fixture","status":"downloading","size":1024,"queue_pos":0,"io_priority":"normal","tx_bytes":10,"rx_bytes":20,"tx_rate":1,"rx_rate":2,"tx_pct":0,"rx_pct":195,"error":"none","created_ts":1700000000,"eta":30,"download_dir":"Rml4dHVyZXM=","stop_ratio":150,"archive_password":"","info_hash":"abcdef","piece_length":262144}}"""),
				"/api/v16/downloads/8" when request.Method == HttpMethod.Put =>
					FixtureHandler.Json("""{"success":true,"result":{"id":8,"status":"downloading"}}"""),
				"/api/v16/downloads/8" when request.Method == HttpMethod.Delete =>
					FixtureHandler.Json("""{"success":true}"""),
				"/api/v16/downloads/8/erase" when request.Method == HttpMethod.Delete =>
					FixtureHandler.Json("""{"success":true}"""),
				"/api/v16/downloads/8/log" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json("""{"success":true,"result":"fixture log"}"""),
				"/api/v16/downloads/stats" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json(
						"""{"success":true,"result":{"nb_tasks":3,"nb_tasks_downloading":1,"nb_tasks_active":2,"rx_rate":2048,"tx_rate":128,"throttling_mode":"normal","throttling_is_scheduled":false,"nb_peer":5,"blocklist_entries":10,"blocklist_hits":2}}"""),
				"/api/v16/downloads/8/files" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json(
						"""{"success":true,"result":[{"id":"opaque","task_id":8,"filepath":"Rml4dHVyZS5iaW4=","name":"fixture.bin","size":1024,"rx":512,"status":"done","priority":"normal"}]}"""),
				"/api/v16/downloads/8/files/opaque" when request.Method == HttpMethod.Put =>
					FixtureHandler.Json("""{"success":true}"""),
				"/api/v16/downloads/config/" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json(
						"""{"success":true,"result":{"max_downloading_tasks":5,"use_watch_dir":false}}"""),
				"/api/v16/downloads/config/" when request.Method == HttpMethod.Put =>
					FixtureHandler.Json(
						"""{"success":true,"result":{"max_downloading_tasks":7,"use_watch_dir":true}}"""),
				"/api/v16/downloads/throttling" when request.Method == HttpMethod.Put =>
					FixtureHandler.Json("""{"success":true,"result":{"is_scheduled":false,"throttling":"normal"}}"""),
				_ => FixtureHandler.Json("""{"success":true}""")
			}));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var downloads = Create(client).Downloads;

		var download = await downloads.GetAsync(8);
		var updated = await downloads.UpdateAsync(8,
			new UpdateDownloadRequest { Status = DownloadStatus.Downloading });
		await downloads.DeleteAsync(8);
		await downloads.EraseAsync(8);
		string log = await downloads.GetLogAsync(8);
		var stats = await downloads.GetStatisticsAsync();
		var files = await downloads.GetFilesAsync(8);
		await downloads.SetFilePriorityAsync(8, "opaque",
			new DownloadFilePriorityRequest { Priority = DownloadFilePriority.Normal });
		var configuration = await downloads.GetConfigurationAsync();
		var updatedConfiguration = await downloads.UpdateConfigurationAsync(
			new UpdateDownloadConfiguration { MaxDownloadingTasks = 7, UseWatchDir = true });
		var throttling = await downloads.SetThrottlingAsync(
			new SetDownloadThrottlingRequest { Throttling = DownloadThrottlingMode.Normal });

		Assert.Equal("fixture", download.Name);
		Assert.Equal("downloading", updated.Status);
		Assert.Equal("fixture log", log);
		Assert.Equal(3, stats.NbTasks);
		Assert.Equal("fixture.bin", Assert.Single(files).Name);
		Assert.Equal(7, updatedConfiguration.MaxDownloadingTasks);
		Assert.Equal("normal", throttling.Throttling);
		Assert.True(configuration.UseWatchDir is false);
		var requests = ResourceRequests(handler).ToArray();
		Assert.Equal(11, requests.Length);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/downloads/8", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[1].Method);
		Assert.Equal("/api/v16/downloads/8", requests[1].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Delete, requests[2].Method);
		Assert.Equal("/api/v16/downloads/8", requests[2].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/8/erase", requests[3].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/8/log", requests[4].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/stats", requests[5].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/8/files", requests[6].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/8/files/opaque", requests[7].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/config/", requests[8].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/config/", requests[9].Address.AbsolutePath);
		Assert.Equal("/api/v16/downloads/throttling", requests[10].Address.AbsolutePath);
		using var updateBody = JsonDocument.Parse(requests[1].Body!);
		Assert.Equal("downloading", updateBody.RootElement.GetProperty("status").GetString());
	}

	[Fact]
	public async Task DownloadNegativeIdsAndOpaqueSegmentsRejectBeforeAnyIo()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var downloads = Create(client).Downloads;

		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await downloads.GetAsync(-1));
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await downloads.GetTrackersAsync(-1));
		await Assert.ThrowsAsync<ArgumentException>(async () => await downloads.RemoveBlacklistHostAsync(" "));
		await Assert.ThrowsAsync<ArgumentException>(async () => await downloads.UpdateTrackerAsync(8, " ",
			new UpdateDownloadTrackerRequest { IsEnabled = false }));
		Assert.Empty(handler.Requests);
	}

	[Fact]
	public async Task BinaryFileUsesEscapedOpaquePathAndReturnsOwnedResponse()
	{
		var stream = new MemoryStream([.. "binary fixture"u8]);
		using var handler = FixtureHandler.Authenticated((_, _) => Task.FromResult(
			new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(stream) }));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);

		await using (var download =
					 await Create(client).FileSystem.At(EncodedFreeboxPath.FromEncoded("L9+/w6k=")).DownloadAsync())
		{
			using var buffer = new MemoryStream();
			await download.Content.CopyToAsync(buffer);
			Assert.Equal("binary fixture", Encoding.UTF8.GetString(buffer.ToArray()));
		}

		Assert.False(stream.CanRead);
		Assert.Equal("https://fixture.example/api/v16/dl/L9%2B%2Fw6k%3D",
			Assert.Single(ResourceRequests(handler)).Address.AbsoluteUri);
	}

	private static FreeboxFilesApi Create(FreeboxClient client) => new(client.Transport, client.BinaryTransport);

	private static IEnumerable<CapturedRequest> ResourceRequests(FixtureHandler handler)
	{
		return handler.Requests.Where(request =>
			!request.Address.AbsolutePath.Contains("/login/", StringComparison.Ordinal));
	}

	private static Dictionary<string, string> DecodeForm(string body)
	{
		return body.Split('&').Select(field => field.Split('=', 2))
			.ToDictionary(field => Uri.UnescapeDataString(field[0].Replace('+', ' ')),
				field => Uri.UnescapeDataString(field[1].Replace('+', ' ')));
	}
}
#pragma warning restore CS0618
