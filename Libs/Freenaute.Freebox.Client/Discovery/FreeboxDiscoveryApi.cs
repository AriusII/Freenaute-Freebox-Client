namespace Freenaute.Freebox.Client.Discovery;

internal sealed class FreeboxDiscoveryApi(FreeboxHttpTransport transport) : IFreeboxDiscoveryApi
{
	public Task<ApiVersionResponse> GetApiVersionAsync(CancellationToken cancellationToken = default) =>
		transport.GetApiVersionAsync(cancellationToken);
}
