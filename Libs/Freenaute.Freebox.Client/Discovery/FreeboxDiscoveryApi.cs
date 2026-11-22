namespace Freenaute.Freebox.Client.Discovery;

internal sealed class FreeboxDiscoveryApi(FreeboxHttpTransport transport) : IFreeboxDiscoveryApi
{
	public async Task<ApiVersionResponse> GetApiVersionAsync(CancellationToken cancellationToken = default) =>
		await transport.GetApiVersionAsync(cancellationToken).ConfigureAwait(false);
}
