using Freenaute.Freebox.Client.Domains.SystemHome.Support;

namespace Freenaute.Freebox.Client.Domains.SystemHome.Updates;

public sealed class FreeboxUpdatesApi
{
	private readonly IFreeboxTransport _transport;

	internal FreeboxUpdatesApi(IFreeboxTransport transport)
	{
		_transport = transport;
	}

	public async Task<UpdateStatus> GetAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, SystemHomeConstants.Routes.Updates, SystemHomeWire.Json.UpdateStatus, cancellationToken)
			.ConfigureAwait(false);
}
