namespace Freenaute.Freebox.Client.Domains.Services;

public sealed class FreeboxPlayerApi(IFreeboxTransport transport)
{
    public Task<Player[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "player", ServicesJsonSerializerContext.Default.PlayerArray,
            cancellationToken);
    }

    public FreeboxPlayer Device(long id)
    {
        return new FreeboxPlayer(transport, ServicesPath.Id(id));
    }
}
