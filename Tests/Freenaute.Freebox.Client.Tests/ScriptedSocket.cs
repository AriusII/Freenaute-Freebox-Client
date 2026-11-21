namespace Freenaute.Freebox.Client.Tests;

internal sealed class ScriptedSocket : WebSocket
{
    private readonly Channel<SocketFrame> _incoming = Channel.CreateUnbounded<SocketFrame>();
    private WebSocketCloseStatus? _closeStatus;
    private SocketFrame? _pending;
    private int _pendingOffset;
    private int _state = (int)WebSocketState.Open;
    public ConcurrentQueue<SocketFrame> Sent { get; } = new();
    public Func<SocketFrame, Task>? Respond { get; set; }
    public Func<SocketFrame, CancellationToken, Task>? RespondWithCancellation { get; set; }
    public bool WasDisposed { get; private set; }
    public override WebSocketCloseStatus? CloseStatus => _closeStatus;
    public override string? CloseStatusDescription => null;
    public override string? SubProtocol => null;
    public override WebSocketState State => (WebSocketState)Volatile.Read(ref _state);

    public void Text(string json)
    {
        _incoming.Writer.TryWrite(new SocketFrame(Encoding.UTF8.GetBytes(json), WebSocketMessageType.Text, true));
    }

    public void FragmentedText(string json, int split)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        _incoming.Writer.TryWrite(new SocketFrame(bytes[..split], WebSocketMessageType.Text, false));
        _incoming.Writer.TryWrite(new SocketFrame(bytes[split..], WebSocketMessageType.Text, true));
    }

    public void NormalClose()
    {
        _incoming.Writer.TryWrite(new SocketFrame([], WebSocketMessageType.Close, true));
    }

    public IEnumerable<JsonElement> Controls()
    {
        return Sent.Where(x => x.Type == WebSocketMessageType.Text).Select(x =>
        {
            using var json = JsonDocument.Parse(x.Bytes);
            return json.RootElement.Clone();
        });
    }

    public override async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var frame = new SocketFrame(buffer.ToArray(), messageType, endOfMessage);
        Sent.Enqueue(frame);
        if (RespondWithCancellation is { } withCancellation)
        {
            await withCancellation(frame, cancellationToken);
        }
        else if (Respond is { } respond)
        {
            await respond(frame);
        }
    }

    public override async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer,
        CancellationToken cancellationToken)
    {
        _pending ??= await _incoming.Reader.ReadAsync(cancellationToken);
        var frame = _pending;
        if (frame.Type == WebSocketMessageType.Close)
        {
            _closeStatus = WebSocketCloseStatus.NormalClosure;
            Volatile.Write(ref _state, (int)WebSocketState.CloseReceived);
            _pending = null;
            return new WebSocketReceiveResult(0, WebSocketMessageType.Close, true, _closeStatus, null);
        }

        int count = Math.Min(buffer.Count, frame.Bytes.Length - _pendingOffset);
        frame.Bytes.AsMemory(_pendingOffset, count).CopyTo(buffer.AsMemory());
        _pendingOffset += count;
        bool end = _pendingOffset == frame.Bytes.Length;
        if (end)
        {
            _pending = null;
            _pendingOffset = 0;
        }

        return new WebSocketReceiveResult(count, frame.Type, end && frame.EndOfMessage);
    }

    public override Task CloseAsync(WebSocketCloseStatus closeStatus, string? statusDescription,
        CancellationToken cancellationToken)
    {
        _closeStatus = closeStatus;
        Volatile.Write(ref _state, (int)WebSocketState.Closed);
        return Task.CompletedTask;
    }

    public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string? statusDescription,
        CancellationToken cancellationToken)
    {
        _closeStatus = closeStatus;
        Volatile.Write(ref _state, (int)WebSocketState.CloseSent);
        return Task.CompletedTask;
    }

    public override void Abort()
    {
        Volatile.Write(ref _state, (int)WebSocketState.Aborted);
    }

    public override void Dispose()
    {
        WasDisposed = true;
        Volatile.Write(ref _state, (int)WebSocketState.Closed);
        _incoming.Writer.TryComplete();
    }
}
