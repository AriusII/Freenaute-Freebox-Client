namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxLedstripApi
{
    private readonly IFreeboxTransport transport;

    internal FreeboxLedstripApi(IFreeboxTransport transport)
    {
        this.transport = transport;
    }

    public Task<LedstripStatus> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "ledstrip/status", SystemHomeWire.Json.LedstripStatus,
            cancellationToken);
    }

    public Task<LedstripPlanning> GetPlanningAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "ledstrip/planning/", SystemHomeWire.Json.LedstripPlanning,
            cancellationToken);
    }

    public FreeboxSystemHomeCommand<LedstripPlanningUpdate, LedstripPlanning> ConfigurePlanning()
    {
        return new FreeboxSystemHomeCommand<LedstripPlanningUpdate, LedstripPlanning>(transport, HttpMethod.Put,
            "ledstrip/planning", new LedstripPlanningUpdate(), SystemHomeWire.Json.LedstripPlanningUpdate,
            SystemHomeWire.Json.LedstripPlanning);
    }
}
