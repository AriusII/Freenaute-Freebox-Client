namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxHomeNodesApi
{
    private readonly IFreeboxTransport transport;

    internal FreeboxHomeNodesApi(IFreeboxTransport transport)
    {
        this.transport = transport;
    }

    public Task<ImmutableArray<HomeNode>> ListAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "home/nodes", SystemHomeWire.Json.HomeNodes, cancellationToken);
    }
}
