using Freenaute.Freebox.Client.Domains.SystemHome.Support;

namespace Freenaute.Freebox.Client.Domains.SystemHome.Standby;

public sealed class FreeboxStandbyApi
{
	private readonly IFreeboxTransport _transport;

	internal FreeboxStandbyApi(IFreeboxTransport transport)
	{
		_transport = transport;
	}

	public async Task<StandbyStatus> GetStatusAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, SystemHomeConstants.Routes.StandbyStatus, SystemHomeWire.Json.StandbyStatus,
			cancellationToken).ConfigureAwait(false);
	}

	public async Task<StandbyConfig> GetConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, SystemHomeConstants.Routes.StandbyConfiguration, SystemHomeWire.Json.StandbyConfig,
			cancellationToken).ConfigureAwait(false);
	}

	public FreeboxSystemHomeCommand<StandbyConfigUpdate, StandbyConfig> Configure()
	{
		return new FreeboxSystemHomeCommand<StandbyConfigUpdate, StandbyConfig>(_transport, HttpMethod.Put,
			"standby/config", new StandbyConfigUpdate(), SystemHomeWire.Json.StandbyConfigUpdate,
			SystemHomeWire.Json.StandbyConfig);
	}
}
