namespace Freenaute.Freebox.Client.Domains.WebSockets.Events;

public sealed class FreeboxEventsApi
{
	private readonly IFreeboxWebSocketTransport _transport;

	public FreeboxEventsApi(IFreeboxWebSocketTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	public FreeboxEventSubscriptionCommand Subscribe(params FreeboxServerEvent[] events) => new(_transport, events);
}
