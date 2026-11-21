namespace Freenaute.Freebox.Client;

internal sealed class FreeboxDiscoveryApi(FreeboxHttpTransport transport) : IFreeboxDiscoveryApi
{
    public Task<ApiVersionResponse> GetApiVersionAsync(CancellationToken cancellationToken = default)
    {
        return transport.GetApiVersionAsync(cancellationToken);
    }
}
