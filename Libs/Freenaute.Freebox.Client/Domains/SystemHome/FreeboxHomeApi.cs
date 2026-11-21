namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxHomeApi
{
    private readonly IFreeboxTransport transport;

    internal FreeboxHomeApi(IFreeboxTransport transport)
    {
        this.transport = transport;
        Adapters = new FreeboxHomeAdaptersApi(transport);
        Nodes = new FreeboxHomeNodesApi(transport);
        Tiles = new FreeboxHomeTilesApi(transport);
    }

    public FreeboxHomeAdaptersApi Adapters { get; }
    public FreeboxHomeNodesApi Nodes { get; }
    public FreeboxHomeTilesApi Tiles { get; }

    public FreeboxHomeAdapterResource Adapter(long id)
    {
        return new FreeboxHomeAdapterResource(transport, id);
    }

    public FreeboxHomeNodeResource Node(long id)
    {
        return new FreeboxHomeNodeResource(transport, id);
    }
}
