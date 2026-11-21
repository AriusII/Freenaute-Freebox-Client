namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxHomeEndpointResource
{
    private readonly string path;
    private readonly IFreeboxTransport transport;

    internal FreeboxHomeEndpointResource(IFreeboxTransport transport, long nodeId, long endpointId)
    {
        this.transport = transport;
        path = "home/endpoints/" + SystemHomeWire.Id(nodeId) + "/" + SystemHomeWire.Id(endpointId);
    }

    public Task<HomeNodeEndpointValue> GetAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, path, SystemHomeWire.Json.HomeNodeEndpointValue, cancellationToken);
    }

    public FreeboxSystemHomeCommand<HomeEndpointValueRequest> Set(HomeIoValue value)
    {
        return new FreeboxSystemHomeCommand<HomeEndpointValueRequest>(transport, HttpMethod.Put, path,
            new HomeEndpointValueRequest(value), SystemHomeWire.Json.HomeEndpointValueRequest);
    }

    public FreeboxSystemHomeCommand<HomeEndpointValueRequest> Trigger()
    {
        return Set(HomeIoValue.Null);
    }

    public FreeboxSystemHomeCommand<HomeEndpointValueRequest> SetBoolean(bool value)
    {
        return Set(HomeIoValue.FromBoolean(value));
    }

    public FreeboxSystemHomeCommand<HomeEndpointValueRequest> SetInteger(long value)
    {
        return Set(HomeIoValue.FromInteger(value));
    }

    public FreeboxSystemHomeCommand<HomeEndpointValueRequest> SetFloat(double value)
    {
        return Set(HomeIoValue.FromFloat(value));
    }

    public FreeboxSystemHomeCommand<HomeEndpointValueRequest> SetString(string value)
    {
        return Set(HomeIoValue.FromString(value));
    }
}
