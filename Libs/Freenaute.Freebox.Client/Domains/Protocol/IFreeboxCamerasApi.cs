namespace Freenaute.Freebox.Client.Domains.Protocol;

public interface IFreeboxCamerasApi
{
    Task<Camera[]> ListAsync(CancellationToken cancellationToken = default);
    FreeboxCamera Camera(string id);
}
