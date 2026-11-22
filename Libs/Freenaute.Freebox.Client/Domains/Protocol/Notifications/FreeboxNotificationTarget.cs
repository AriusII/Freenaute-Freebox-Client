namespace Freenaute.Freebox.Client.Domains.Protocol.Notifications;

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
	public async Task<ObjectOrArray<NotificationTarget>> GetAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, _path,
				ProtocolJsonSerializerContext.Default.ObjectOrArrayNotificationTarget, cancellationToken)
			.ConfigureAwait(false);
	}

	public async Task UpdateAsync(NotificationTargetWrite request, CancellationToken cancellationToken = default)
	{
		await _transport.SendAsync(HttpMethod.Put, _path, FreeboxNotificationsApi.Snapshot(request),
			ProtocolJsonSerializerContext.Default.NotificationTargetWrite, cancellationToken).ConfigureAwait(false);
	}

	public async Task DeleteAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Delete, _path, cancellationToken).ConfigureAwait(false);
}
