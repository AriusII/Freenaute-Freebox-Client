using Freenaute.Freebox.Client.Domains.SystemHome.Support;

namespace Freenaute.Freebox.Client.Domains.SystemHome.Ledstrip;

public sealed class FreeboxLedstripApi
{
	private readonly IFreeboxTransport _transport;

	internal FreeboxLedstripApi(IFreeboxTransport transport)
	{
		_transport = transport;
	}

	public async Task<LedstripStatus> GetStatusAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, SystemHomeConstants.Routes.LedstripStatus, SystemHomeWire.Json.LedstripStatus,
			cancellationToken).ConfigureAwait(false);
	}

	public async Task<LedstripPlanning> GetPlanningAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, SystemHomeConstants.Routes.LedstripPlanning, SystemHomeWire.Json.LedstripPlanning,
			cancellationToken).ConfigureAwait(false);
	}

	public FreeboxSystemHomeCommand<LedstripPlanningUpdate, LedstripPlanning> ConfigurePlanning()
	{
		return new FreeboxSystemHomeCommand<LedstripPlanningUpdate, LedstripPlanning>(_transport, HttpMethod.Put,
			"ledstrip/planning", new LedstripPlanningUpdate(), SystemHomeWire.Json.LedstripPlanningUpdate,
			SystemHomeWire.Json.LedstripPlanning);
	}
}
