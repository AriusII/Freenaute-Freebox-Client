namespace Freenaute.Freebox.Client;

internal sealed class FreeboxClientState(FreeboxClientOptions options) : IDisposable
{
    internal Uri? ApiAddress;
    internal ApiVersionResponse? ApiVersion;
    internal SessionResponse? Session;
    internal FreeboxClientOptions Options { get; } = options;
    internal SemaphoreSlim DiscoveryGate { get; } = new(1, 1);
    internal SemaphoreSlim SessionGate { get; } = new(1, 1);

    public void Dispose()
    {
        DiscoveryGate.Dispose();
        SessionGate.Dispose();
    }
}
