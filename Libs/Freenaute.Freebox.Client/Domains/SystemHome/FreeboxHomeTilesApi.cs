namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxHomeTilesApi
{
    private readonly IFreeboxTransport transport;

    internal FreeboxHomeTilesApi(IFreeboxTransport transport)
    {
        this.transport = transport;
    }

    public Task<ImmutableArray<HomeTile>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "home/tileset/all", SystemHomeWire.Json.HomeTiles,
            cancellationToken);
    }

    public Task<ImmutableArray<HomeTile>> ForNodeAsync(long nodeId, CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "home/tileset/" + SystemHomeWire.Id(nodeId),
            SystemHomeWire.Json.HomeTiles, cancellationToken);
    }
}
