using Freenaute.Freebox.Client.Domains.Protocol.Support;

namespace Freenaute.Freebox.Client.Domains.Protocol.Notifications;

internal sealed class FreeboxNotificationsApi(IFreeboxTransport transport) : IFreeboxNotificationsApi
{
	public async Task<NotificationTarget[]> ListTargetsAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ProtocolConstants.Routes.NotificationTargets,
			ProtocolJsonSerializerContext.Default.NotificationTargetArray,
			cancellationToken).ConfigureAwait(false);
	}

	public async Task CreateTargetAsync(NotificationTargetWrite request, CancellationToken cancellationToken = default)
	{
		await transport.SendAsync(HttpMethod.Post, ProtocolConstants.Routes.NotificationTargetsCollection, Snapshot(request),
			ProtocolJsonSerializerContext.Default.NotificationTargetWrite, cancellationToken).ConfigureAwait(false);
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
