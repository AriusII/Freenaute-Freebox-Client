namespace Freenaute.Freebox.Client;

public interface IFreeboxDiscoveryApi
{
	Task<ApiVersionResponse> GetApiVersionAsync(CancellationToken cancellationToken = default);
}
