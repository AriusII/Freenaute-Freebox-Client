namespace Freenaute.Freebox.AotSmoke;

internal sealed class ScriptedWebSocket(params string[] fragments) : WebSocket
{
	private readonly Queue<byte[]> _fragments = new(fragments.Select(Encoding.UTF8.GetBytes));
	private bool _aborted;
	private bool _disposed;

	public override WebSocketCloseStatus? CloseStatus => null;
	public override string? CloseStatusDescription => null;
	public override WebSocketState State => _disposed || _aborted ? WebSocketState.Closed : WebSocketState.Open;
	public override string? SubProtocol => null;

	public override void Abort() => _aborted = true;

	public override Task CloseAsync(WebSocketCloseStatus closeStatus, string? statusDescription,
		CancellationToken cancellationToken)
	{
		_disposed = true;
		return Task.CompletedTask;
	}

	public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string? statusDescription,
		CancellationToken cancellationToken)
	{
		_disposed = true;
		return Task.CompletedTask;
	}

	public override void Dispose() => _disposed = true;

	public override async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer,
		CancellationToken cancellationToken) =>
		await ReceiveCoreAsync(buffer, cancellationToken);

	public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage,
		CancellationToken cancellationToken) =>
		Task.CompletedTask;

	public override async ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(Memory<byte> buffer,
		CancellationToken cancellationToken = default)
	{
		byte[] fragment = _fragments.Dequeue();
		fragment.CopyTo(buffer);
		await Task.CompletedTask;
		return new ValueWebSocketReceiveResult(fragment.Length, WebSocketMessageType.Text, _fragments.Count is 0);
	}

	private Task<WebSocketReceiveResult> ReceiveCoreAsync(ArraySegment<byte> buffer,
		CancellationToken cancellationToken)
	{
		byte[] fragment = _fragments.Dequeue();
		fragment.CopyTo(buffer.AsSpan());
		return Task.FromResult(new WebSocketReceiveResult(fragment.Length, WebSocketMessageType.Text,
			_fragments.Count is 0));
	}
}
