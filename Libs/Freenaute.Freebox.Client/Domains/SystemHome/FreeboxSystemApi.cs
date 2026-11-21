namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxSystemApi
{
    private readonly IFreeboxTransport transport;

    internal FreeboxSystemApi(IFreeboxTransport transport)
    {
        this.transport = transport;
    }

    public Task<SystemConfig> GetAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "system/", SystemHomeWire.Json.SystemConfig, cancellationToken);
    }

    public Task RebootAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Post, "system/reboot/", cancellationToken);
    }

    public Task ShutdownAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Post, "system/shutdown/", cancellationToken);
    }
}
