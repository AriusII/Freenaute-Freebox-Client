namespace Freenaute.Freebox.Client.Domains.Protocol;

public interface IFreeboxNotificationsApi
{
    Task<NotificationTarget[]> ListTargetsAsync(CancellationToken cancellationToken = default);
    Task CreateTargetAsync(NotificationTargetWrite request, CancellationToken cancellationToken = default);
    FreeboxNotificationTarget Target(string id);
}
