namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxHomeAdapterResource
{
    private readonly string path;
    private readonly IFreeboxTransport transport;

    internal FreeboxHomeAdapterResource(IFreeboxTransport transport, long id)
    {
        this.transport = transport;
        path = "home/adapters/" + SystemHomeWire.Id(id);
        Pairing = new FreeboxHomePairingResource(transport, id);
    }

    public FreeboxHomePairingResource Pairing { get; }

    public Task<HomeAdapter> GetAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, path, SystemHomeWire.Json.HomeAdapter, cancellationToken);
    }

    public FreeboxSystemHomeCommand<HomeAdapterStatusRequest> SetStatus(HomeAdapterStatus status)
    {
        return new FreeboxSystemHomeCommand<HomeAdapterStatusRequest>(transport, HttpMethod.Put, path,
            new HomeAdapterStatusRequest(status), SystemHomeWire.Json.HomeAdapterStatusRequest);
    }

    public FreeboxSystemHomeCommand<HomeAdapterStatusRequest> Activate()
    {
        return SetStatus(HomeAdapterStatus.Active);
    }

    public FreeboxSystemHomeCommand<HomeAdapterStatusRequest> Disable()
    {
        return SetStatus(HomeAdapterStatus.Disabled);
    }
}
