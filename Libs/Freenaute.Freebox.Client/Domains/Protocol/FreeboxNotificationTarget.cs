namespace Freenaute.Freebox.Client.Domains.Protocol;

public sealed class FreeboxNotificationTarget
{
    private readonly string _path;
    private readonly IFreeboxTransport _transport;

    internal FreeboxNotificationTarget(IFreeboxTransport transport, string id)
    {
        _transport = transport;
        _path = $"notif/targets/{FreeboxApiPath.EncodeSegment(id)}";
    }

    /// <summary>Preserves the documented object-or-array wire shape.</summary>
    public Task<ObjectOrArray<NotificationTarget>> GetAsync(CancellationToken cancellationToken = default)
    {
        return _transport.SendAsync(HttpMethod.Get, _path,
            ProtocolJsonSerializerContext.Default.ObjectOrArrayNotificationTarget, cancellationToken);
    }

    public Task UpdateAsync(NotificationTargetWrite request, CancellationToken cancellationToken = default)
    {
        return _transport.SendAsync(HttpMethod.Put, _path, FreeboxNotificationsApi.Snapshot(request),
            ProtocolJsonSerializerContext.Default.NotificationTargetWrite, cancellationToken);
    }

    public Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        return _transport.SendAsync(HttpMethod.Delete, _path, cancellationToken);
    }
}
