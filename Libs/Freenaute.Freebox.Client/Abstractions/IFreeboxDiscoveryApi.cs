namespace Freenaute.Freebox.Client.Abstractions;

public interface IFreeboxDiscoveryApi
{
	Task<ApiVersionResponse> GetApiVersionAsync(CancellationToken cancellationToken = default);
}
