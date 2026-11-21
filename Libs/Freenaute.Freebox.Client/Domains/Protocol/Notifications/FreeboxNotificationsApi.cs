namespace Freenaute.Freebox.Client.Domains.Protocol.Notifications;

internal sealed class FreeboxNotificationsApi(IFreeboxTransport transport) : IFreeboxNotificationsApi
{
	public Task<NotificationTarget[]> ListTargetsAsync(CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, "notif/targets",
			ProtocolJsonSerializerContext.Default.NotificationTargetArray,
			cancellationToken);
	}

	public Task CreateTargetAsync(NotificationTargetWrite request, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Post, "notif/targets/", Snapshot(request),
			ProtocolJsonSerializerContext.Default.NotificationTargetWrite, cancellationToken);
	}

	public FreeboxNotificationTarget Target(string id) => new(transport, id);

	internal static NotificationTargetWrite Snapshot(NotificationTargetWrite request)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentException.ThrowIfNullOrWhiteSpace(request.Name);
		ArgumentException.ThrowIfNullOrWhiteSpace(request.Type);
		ArgumentException.ThrowIfNullOrWhiteSpace(request.Token);
		ArgumentException.ThrowIfNullOrWhiteSpace(request.MessageType);
		ArgumentNullException.ThrowIfNull(request.Subscriptions);
		if (!Uri.TryCreate(request.ApiUrl, UriKind.Absolute, out var uri) ||
		    uri.Scheme is not ("http" or "https") || !string.IsNullOrEmpty(uri.UserInfo))
		{
			throw new ArgumentException(
				"The notification API URL must be an absolute HTTP(S) address without credentials.", nameof(request));
		}

		return request with { Subscriptions = [.. request.Subscriptions] };
	}
}
