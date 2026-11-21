namespace Freenaute.Freebox.Client.Tests;

internal sealed class SocketFixtureTransport(ScriptedSocket socket) : IFreeboxWebSocketTransport
{
    public ConcurrentQueue<string> Paths { get; } = new();

    public Task<FreeboxWebSocketConnection> ConnectAsync(string relativePath,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Paths.Enqueue(relativePath);
        return Task.FromResult(new FreeboxWebSocketConnection(socket));
    }
}
