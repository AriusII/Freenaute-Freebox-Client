namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxHomeAdaptersApi
{
    private readonly IFreeboxTransport transport;

    internal FreeboxHomeAdaptersApi(IFreeboxTransport transport)
    {
        this.transport = transport;
    }

    public Task<ImmutableArray<HomeAdapter>> ListAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "home/adapters", SystemHomeWire.Json.HomeAdapters,
            cancellationToken);
    }
}
