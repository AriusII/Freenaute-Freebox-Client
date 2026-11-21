namespace Freenaute.Freebox.Client.Domains.Services.Players;

public sealed class FreeboxPlayerApi(IFreeboxTransport transport)
{
	public Task<Player[]> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, "player", ServicesJsonSerializerContext.Default.PlayerArray,
			cancellationToken);
	}

	public FreeboxPlayer Device(long id) => new(transport, ServicesPath.Id(id));
}
