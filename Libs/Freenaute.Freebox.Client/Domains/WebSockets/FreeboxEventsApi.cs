namespace Freenaute.Freebox.Client.Domains.WebSockets;

public sealed class FreeboxEventsApi
{
    private readonly IFreeboxWebSocketTransport _transport;

    public FreeboxEventsApi(IFreeboxWebSocketTransport transport)
    {
        ArgumentNullException.ThrowIfNull(transport);
        _transport = transport;
    }

    public FreeboxEventSubscriptionCommand Subscribe(params FreeboxServerEvent[] events)
    {
        return new FreeboxEventSubscriptionCommand(_transport, events);
    }
}
