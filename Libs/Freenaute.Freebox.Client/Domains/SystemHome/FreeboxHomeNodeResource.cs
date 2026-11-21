namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxHomeNodeResource
{
    private readonly long id;
    private readonly string path;
    private readonly IFreeboxTransport transport;

    internal FreeboxHomeNodeResource(IFreeboxTransport transport, long id)
    {
        this.transport = transport;
        this.id = id;
        path = "home/nodes/" + SystemHomeWire.Id(id);
    }

    public Task<HomeNode> GetAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, path, SystemHomeWire.Json.HomeNode, cancellationToken);
    }

    public FreeboxSystemHomeCommand<RenameHomeNodeRequest> Rename(string label)
    {
        return new FreeboxSystemHomeCommand<RenameHomeNodeRequest>(transport, HttpMethod.Put, path,
            new RenameHomeNodeRequest(label), SystemHomeWire.Json.RenameHomeNodeRequest);
    }

    public Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Delete, path, cancellationToken);
    }

    public FreeboxHomeEndpointResource Endpoint(long endpointId)
    {
        return new FreeboxHomeEndpointResource(transport, id, endpointId);
    }
}
