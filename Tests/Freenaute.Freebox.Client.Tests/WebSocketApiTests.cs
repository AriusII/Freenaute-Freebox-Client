namespace Freenaute.Freebox.Client.Tests;

public sealed class WebSocketApiTests
{
	private const string StartAck = """{"action":"upload_start","request_id":1,"success":true}""";

	[Fact]
	public async Task UploadPipelinesBinaryChunksWhileReceivingProgressAndWaitsForFinalAck()
	{
		using var socket = new ScriptedSocket();
		var seenProgress = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
		var progress = new InlineProgress<FreeboxUploadProgress>(value =>
		{
			if (value.TotalFileLength is 8 && !value.Complete)
			{
				seenProgress.TrySetResult();
			}
		});
		int binaryCount = 0;
		socket.Respond = async frame =>
		{
			if (frame.Type is WebSocketMessageType.Binary)
			{
				int count = Interlocked.Increment(ref binaryCount);
				if (count is 2)
				{
					socket.Text(
						"""{"action":"upload_data","request_id":1,"success":true,"result":{"total_len":8,"complete":false}}""");
				}

				if (count is 3)
				{
					await seenProgress.Task.WaitAsync(TimeSpan.FromSeconds(5));
				}

				return;
			}

			using var json = JsonDocument.Parse(frame.Bytes);
			string? action = json.RootElement.GetProperty("action").GetString();
			if (action is "upload_start")
			{
				socket.Text(StartAck);
			}
			else if (action is "upload_finalize")
			{
				socket.Text(
					"""{"action":"upload_finalize","request_id":2,"success":true,"result":{"total_len":9,"complete":true}}""");
			}
		};
		var transport = new SocketFixtureTransport(socket);
		var path = EncodedFreeboxPath.FromEncoded("L0Rpc3F1ZSBkdXIvMF91cGxvYWRfdGVzdA==");
		await using var source = new MemoryStream([1, 2, 3, 4, 5, 6, 7, 8, 9]);
		var result = await new FreeboxWebSocketsApi(transport).Uploads.To(path).File("épreuve.bin")
			.Overwrite().WithSize(9).WithChunkSize(4).SendAsync(source, progress: progress);

		Assert.Equal(9, result.TotalFileLength);
		Assert.Equal(9, result.BytesSent);
		Assert.Equal("ws/upload", Assert.Single(transport.Paths));
		Assert.Equal(3, binaryCount);
		Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
			socket.Sent.Where(x => x.Type is WebSocketMessageType.Binary).SelectMany(x => x.Bytes));
		var start = socket.Controls().First();
		Assert.Equal("upload_start", start.GetProperty("action").GetString());
		Assert.Equal(path.Value, start.GetProperty("dirname").GetString());
		Assert.Equal("épreuve.bin", start.GetProperty("filename").GetString());
		Assert.Equal("overwrite", start.GetProperty("force").GetString());
		Assert.Equal(9, start.GetProperty("size").GetInt64());
		Assert.DoesNotContain(socket.Controls(), x => x.GetProperty("action").GetString() is "upload_cancel");
		Assert.True(socket.WasDisposed);
	}

	[Fact]
	public async Task ResumeDoesNotInspectStreamLengthOrSeekAndOmitsUnknownSize()
	{
		using var socket = SuccessfulUploadSocket(3);
		using var source = new NonSeekableReadStream([7, 8, 9]);
		var command = new FreeboxWebSocketsApi(new SocketFixtureTransport(socket)).Uploads
			.To(EncodedFreeboxPath.FromUtf8Path("/Disque dur")).File("resume.bin");
		var resume = command.Resume();

		await resume.SendAsync(source);

		var start = socket.Controls().First();
		Assert.Equal("resume", start.GetProperty("force").GetString());
		Assert.False(start.TryGetProperty("size", out _));
		Assert.Equal(new byte[] { 7, 8, 9 },
			socket.Sent.Where(x => x.Type is WebSocketMessageType.Binary).SelectMany(x => x.Bytes));
		Assert.False(source.WasDisposed);
	}

	[Fact]
	public async Task CallerCancellationClosesAndKeepsPartialWithoutFinalizeOrDestructiveCancel()
	{
		using var socket = new ScriptedSocket();
		var writing = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
		socket.RespondWithCancellation = async (frame, cancellationToken) =>
		{
			if (frame.Type is WebSocketMessageType.Text)
			{
				socket.Text(StartAck);
			}
			else
			{
				writing.SetResult();
				await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
			}
		};
		using var cancellation = new CancellationTokenSource();
		using var source = new MemoryStream([1, 2, 3]);
		var pending = new FreeboxWebSocketsApi(new SocketFixtureTransport(socket)).Uploads
			.To(EncodedFreeboxPath.FromUtf8Path("/Disque dur")).File("partial.bin")
			.SendAsync(source, cancellation.Token);
		await writing.Task.WaitAsync(TimeSpan.FromSeconds(5));
		await cancellation.CancelAsync();

		await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await pending);

		Assert.True(socket.WasDisposed);
		Assert.Equal("upload_start", Assert.Single(socket.Controls()).GetProperty("action").GetString());
	}

	[Fact]
	public async Task ExplicitSessionCancelSendsDestructiveActionAndRequiresItsCorrelatedAck()
	{
		using var socket = new ScriptedSocket();
		socket.Respond = frame =>
		{
			if (frame.Type is WebSocketMessageType.Text)
			{
				using var json = JsonDocument.Parse(frame.Bytes);
				if (json.RootElement.GetProperty("action").GetString() is "upload_start")
				{
					socket.Text(StartAck);
				}
				else
				{
					socket.Text(
						"""{"action":"upload_cancel","request_id":3,"success":true,"result":{"complete":true,"cancelled":true}}""");
				}
			}

			return Task.CompletedTask;
		};
		await using var session = await new FreeboxWebSocketsApi(new SocketFixtureTransport(socket)).Uploads
			.To(EncodedFreeboxPath.FromUtf8Path("/Disque dur")).File("cancel.bin").OpenAsync();
		await session.SendChunkAsync(new byte[] { 1, 2 });
		await session.CancelAsync();

		Assert.Equal(FreeboxUploadState.Cancelled, session.State);
		Assert.Equal(2, session.BytesSent);
		Assert.Equal("upload_cancel", socket.Controls().Last().GetProperty("action").GetString());
		Assert.Equal(3, socket.Controls().Last().GetProperty("request_id").GetInt64());
	}

	[Fact]
	public async Task StartRejectionPreservesConflictCodeAndExistingLargeSizeWithoutLeakingMessage()
	{
		using var socket = new ScriptedSocket();
		socket.Respond = _ =>
		{
			socket.Text(
				"""{"action":"upload_start","request_id":1,"success":false,"file_size":5000000000,"error_code":"conflict","msg":"untrusted secret"}""");
			return Task.CompletedTask;
		};
		using var source = new MemoryStream([1]);
		var exception = await Assert.ThrowsAsync<FreeboxWebSocketApiException>(async () =>
			await new FreeboxWebSocketsApi(new SocketFixtureTransport(socket)).Uploads
				.To(EncodedFreeboxPath.FromUtf8Path("/Disque dur")).File("conflict.bin").SendAsync(source));

		Assert.Equal("conflict", exception.ErrorCode);
		Assert.Equal(5_000_000_000, exception.ExistingFileSize);
		Assert.Equal("untrusted secret", exception.ApiMessage);
		Assert.DoesNotContain("untrusted secret", exception.Message, StringComparison.OrdinalIgnoreCase);
		Assert.DoesNotContain(socket.Sent, x => x.Type is WebSocketMessageType.Binary);
		Assert.Single(socket.Controls());
		Assert.True(socket.WasDisposed);
	}

	[Theory]
	[InlineData(999, true)]
	[InlineData(2, false)]
	public async Task FinalizeCannotSucceedWithWrongCorrelationOrMissingCompletion(long requestId, bool complete)
	{
		using var socket = new ScriptedSocket();
		socket.Respond = frame =>
		{
			if (frame.Type is WebSocketMessageType.Text)
			{
				using var json = JsonDocument.Parse(frame.Bytes);
				if (json.RootElement.GetProperty("action").GetString() is "upload_start")
				{
					socket.Text(StartAck);
				}
				else
				{
					socket.Text(
						string.Create(CultureInfo.InvariantCulture,
							$"{{\"action\":\"upload_finalize\",\"request_id\":{requestId},\"success\":true,\"result\":{{\"total_len\":0,\"complete\":{complete.ToString().ToLowerInvariant()}}}}}"));
				}
			}

			return Task.CompletedTask;
		};
		using var source = new MemoryStream();
		await Assert.ThrowsAsync<JsonException>(async () => await new FreeboxWebSocketsApi(new SocketFixtureTransport(socket))
			.Uploads
			.To(EncodedFreeboxPath.FromUtf8Path("/Disque dur")).File("empty.bin").WithSize(0).SendAsync(source));
		Assert.True(socket.WasDisposed);
		Assert.Equal(0, socket.Controls().First().GetProperty("size").GetInt64());
	}

	[Fact]
	public async Task ServerDataErrorStopsStreamAndIsNotRetriedOrConvertedToUserCancellation()
	{
		using var socket = new ScriptedSocket();
		var errorRead = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
		socket.RespondWithCancellation = async (frame, cancellationToken) =>
		{
			if (frame.Type is WebSocketMessageType.Text)
			{
				socket.Text(StartAck);
			}
			else
			{
				socket.Text(
					"""{"action":"upload_data","request_id":1,"success":false,"error_code":"disk_full","msg":"disque plein"}""");
				errorRead.TrySetResult();
				await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
			}
		};
		using var source = new MemoryStream([1, 2, 3]);
		var exception = await Assert.ThrowsAsync<FreeboxWebSocketApiException>(async () =>
			await new FreeboxWebSocketsApi(new SocketFixtureTransport(socket)).Uploads
				.To(EncodedFreeboxPath.FromUtf8Path("/Disque dur")).File("disk-full.bin").SendAsync(source));

		Assert.Equal("disk_full", exception.ErrorCode);
		Assert.Single(socket.Sent, x => x.Type is WebSocketMessageType.Binary);
		Assert.Equal("upload_start", Assert.Single(socket.Controls()).GetProperty("action").GetString());
	}

	[Fact]
	public async Task EventsPreserveExactNamesAndReadFragmentedNdjsonWithTypedPayloads()
	{
		using var socket = new ScriptedSocket();
		socket.Respond = _ =>
		{
			const string messages =
				"{\"action\":\"register\",\"request_id\":1,\"success\":true}\n{\"action\":\"notification\",\"success\":true,\"source\":\"lan\",\"event\":\"host_l3addr_reachable\",\"result\":{\"id\":\"hôte\",\"l2ident\":{\"id\":\"mac\",\"type\":\"mac_address\"}}}\n";
			socket.FragmentedText(messages,
				Encoding.UTF8.GetBytes(messages[..messages.IndexOf('ô', StringComparison.OrdinalIgnoreCase)]).Length +
				1);
			socket.NormalClose();
			return Task.CompletedTask;
		};
		var transport = new SocketFixtureTransport(socket);
		await using var subscription = await new FreeboxWebSocketsApi(transport).Events.Subscribe(
			FreeboxServerEvent.LanHostL3AddressReachable,
			FreeboxServerEvent.LanHostL3AddressUnreachable).OpenAsync();
		var events = new List<FreeboxEventNotification>();
		await foreach (var item in subscription.ReadAllAsync())
		{
			events.Add(item);
		}

		Assert.Equal("ws/event", Assert.Single(transport.Paths));
		Assert.Equal(["lan_host_l3addr_reachable", "lan_host_l3addr_unreachable"],
			socket.Controls().Single().GetProperty("events").EnumerateArray().Select(x => x.GetString()));
		var notification = Assert.Single(events);
		Assert.Equal("hôte", notification.GetLanHost()!.Id);
		Assert.Equal(ObjectOrArrayKind.Object, notification.GetLanHost()!.L2Ident!.Value.Kind);
		Assert.True(socket.WasDisposed);
	}

	[Fact]
	public async Task EventReaderCancellationClosesSubscriptionWithoutBackgroundPolling()
	{
		using var socket = new ScriptedSocket();
		await using var subscription = await new FreeboxWebSocketsApi(new SocketFixtureTransport(socket)).Events
			.Subscribe(FreeboxServerEvent.LanHostL3AddressReachable).OpenAsync();
		using var cancellation = new CancellationTokenSource();
		await using var reader = subscription.ReadAllAsync(cancellation.Token).GetAsyncEnumerator();
		var pending = reader.MoveNextAsync().AsTask();
		await cancellation.CancelAsync();

		await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await pending);
		Assert.True(socket.WasDisposed);
		Assert.Single(socket.Controls());
	}

	private static ScriptedSocket SuccessfulUploadSocket(long totalLength)
	{
		var socket = new ScriptedSocket();
		socket.Respond = frame =>
		{
			if (frame.Type is WebSocketMessageType.Text)
			{
				using var json = JsonDocument.Parse(frame.Bytes);
				if (json.RootElement.GetProperty("action").GetString() is "upload_start")
				{
					socket.Text(StartAck);
				}
				else
				{
					socket.Text(
						string.Create(CultureInfo.InvariantCulture,
							$"{{\"action\":\"upload_finalize\",\"request_id\":2,\"success\":true,\"result\":{{\"total_len\":{totalLength},\"complete\":true}}}}"));
				}
			}

			return Task.CompletedTask;
		};
		return socket;
	}
}
