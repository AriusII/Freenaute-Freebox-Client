using Freenaute.Freebox.Client.Domains.SystemHome.Support;

namespace Freenaute.Freebox.Client.Domains.SystemHome.System;

public sealed class FreeboxSystemApi
{
	private readonly IFreeboxTransport _transport;

	internal FreeboxSystemApi(IFreeboxTransport transport)
	{
		_transport = transport;
	}

	public async Task<SystemConfig> GetAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, SystemHomeConstants.Routes.System, SystemHomeWire.Json.SystemConfig, cancellationToken)
			.ConfigureAwait(false);

	public async Task RebootAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Post, SystemHomeConstants.Routes.SystemReboot, cancellationToken).ConfigureAwait(false);

	public async Task ShutdownAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Post, SystemHomeConstants.Routes.SystemShutdown, cancellationToken).ConfigureAwait(false);
}
