namespace Freenaute.Freebox.Client.Domains.WebSockets;

/// <summary>Immutable event selection; registration is sent only by OpenAsync.</summary>
public sealed class FreeboxEventSubscriptionCommand
{
    private readonly string[] _events;
    private readonly IFreeboxWebSocketTransport _transport;

    public FreeboxEventSubscriptionCommand(IFreeboxWebSocketTransport transport, params FreeboxServerEvent[] events)
    {
        ArgumentNullException.ThrowIfNull(transport);
        ArgumentNullException.ThrowIfNull(events);
        _transport = transport;
        _events = events.Select(EventName).Distinct(StringComparer.Ordinal).ToArray();
    }

    public async Task<FreeboxEventSubscription> OpenAsync(CancellationToken cancellationToken = default)
    {
        var connection = await _transport.ConnectAsync("ws/event", cancellationToken).ConfigureAwait(false);
        try
        {
            await connection.SendJsonAsync(
                new RegisterEventsRequest { RequestId = 1, Events = (string[])_events.Clone() },
                WebSocketJsonSerializerContext.Default.RegisterEventsRequest, cancellationToken).ConfigureAwait(false);
            return new FreeboxEventSubscription(connection);
        }
        catch
        {
            await connection.DisposeAsync().ConfigureAwait(false);
            throw;
        }
    }

    private static string EventName(FreeboxServerEvent value)
    {
        return value switch
        {
            FreeboxServerEvent.VmStateChanged => "vm_state_changed",
            FreeboxServerEvent.VmDiskTaskDone => "vm_disk_task_done",
            FreeboxServerEvent.LanHostL3AddressReachable => "lan_host_l3addr_reachable",
            FreeboxServerEvent.LanHostL3AddressUnreachable => "lan_host_l3addr_unreachable",
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }
}
