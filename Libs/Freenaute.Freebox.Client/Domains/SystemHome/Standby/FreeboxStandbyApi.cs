namespace Freenaute.Freebox.Client.Domains.SystemHome.Standby;

public sealed class FreeboxStandbyApi
{
	private readonly IFreeboxTransport transport;

	internal FreeboxStandbyApi(IFreeboxTransport transport)
	{
		this.transport = transport;
	}

	public Task<StandbyStatus> GetStatusAsync(CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, "standby/status", SystemHomeWire.Json.StandbyStatus,
			cancellationToken);
	}

	public Task<StandbyConfig> GetConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, "standby/config/", SystemHomeWire.Json.StandbyConfig,
			cancellationToken);
	}

	public FreeboxSystemHomeCommand<StandbyConfigUpdate, StandbyConfig> Configure()
	{
		return new FreeboxSystemHomeCommand<StandbyConfigUpdate, StandbyConfig>(transport, HttpMethod.Put,
			"standby/config", new StandbyConfigUpdate(), SystemHomeWire.Json.StandbyConfigUpdate,
			SystemHomeWire.Json.StandbyConfig);
	}
}
