namespace Freenaute.Freebox.Client.Domains.WebSockets.Events;

/// <summary>A single receive stream. Ending enumeration closes the socket; there is no implicit reconnection or polling.</summary>
public sealed class FreeboxEventSubscription : IAsyncDisposable
{
	private static readonly IFreeboxMapper Mapper = FreeboxMapper.Default;
	private readonly FreeboxWebSocketConnection _connection;
	private readonly CancellationTokenSource _stop = new();
	private int _disposed;
	private int _reading;

	internal FreeboxEventSubscription(FreeboxWebSocketConnection connection)
	{
		_connection = connection;
	}

	public async ValueTask DisposeAsync()
	{
		if (Interlocked.Exchange(ref _disposed, 1) is not 0)
		{
			return;
		}

		await _stop.CancelAsync().ConfigureAwait(false);
		await _connection.DisposeAsync().ConfigureAwait(false);
	}

	/// <summary>Registration replies, if received, are checked; notifications may arrive without a separate register reply.</summary>
	public async IAsyncEnumerable<FreeboxEventNotification> ReadAllAsync(
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) is not 0, this);
		if (Interlocked.CompareExchange(ref _reading, 1, 0) is not 0)
		{
			throw new InvalidOperationException("An event subscription permits only one reader.");
		}

		using var stop = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _stop.Token);
		try
		{
			while (true)
			{
				JsonElement raw = default;
				bool end = false;
				try
				{
					raw = await _connection.ReceiveJsonAsync(stop.Token).ConfigureAwait(false);
				}
				catch (EndOfStreamException)
				{
					end = true;
				}

				if (end)
				{
					yield break;
				}

				var message = Mapper.FromWire(raw, WebSocketJsonSerializerContext.Default.FreeboxWebSocketMessage)
				              ?? throw new JsonException("An event WebSocket message must be an object.");
				if (message.Success is false)
				{
					throw new FreeboxWebSocketApiException(message.Action, message.RequestId, message.ErrorCode,
						message.Message);
				}

				if (message.Success is not true)
				{
					throw new JsonException("An event WebSocket message must declare success.");
				}

				if (string.Equals(message.Action, "register", StringComparison.Ordinal))
				{
					if (message.RequestId is not 1)
					{
						throw new JsonException("A register acknowledgement has an unexpected request_id.");
					}

					continue;
				}

				if (!string.Equals(message.Action, "notification", StringComparison.Ordinal))
				{
					throw new JsonException("Unexpected event WebSocket action.");
				}

				yield return new FreeboxEventNotification(message);
			}
		}
		finally
		{
			Interlocked.Exchange(ref _reading, 0);
			await DisposeAsync().ConfigureAwait(false);
		}
	}
}
