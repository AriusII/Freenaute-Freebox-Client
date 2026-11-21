namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxHomePairingResource
{
    private readonly string path;
    private readonly IFreeboxTransport transport;

    internal FreeboxHomePairingResource(IFreeboxTransport transport, long id)
    {
        this.transport = transport;
        path = "home/pairing/" + SystemHomeWire.Id(id);
    }

    public Task<HomePairingStep> GetStepAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, path, SystemHomeWire.Json.HomePairingStep, cancellationToken);
    }

    public FreeboxSystemHomeCommand<HomePairingStartRequest> Start(string? type = null)
    {
        return new FreeboxSystemHomeCommand<HomePairingStartRequest>(transport, HttpMethod.Post, path,
            new HomePairingStartRequest { Type = type },
            SystemHomeWire.Json.HomePairingStartRequest);
    }

    public FreeboxSystemHomeCommand<HomePairingNextRequest, HomePairingStep> Next(
        StringOrInteger session, StringOrInteger pageId, IEnumerable<HomeIoValue> fields)
    {
        ArgumentNullException.ThrowIfNull(fields);
        return new FreeboxSystemHomeCommand<HomePairingNextRequest, HomePairingStep>(transport, HttpMethod.Post, path,
            new HomePairingNextRequest { Session = session, PageId = pageId, Fields = fields.ToImmutableArray() },
            SystemHomeWire.Json.HomePairingNextRequest, SystemHomeWire.Json.HomePairingStep);
    }

    public FreeboxSystemHomeCommand<HomePairingStopRequest> Stop(long session)
    {
        return new FreeboxSystemHomeCommand<HomePairingStopRequest>(transport, HttpMethod.Post, path,
            new HomePairingStopRequest(session), SystemHomeWire.Json.HomePairingStopRequest);
    }
}
