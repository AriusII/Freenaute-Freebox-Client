using Freenaute.Freebox.Client.Domains.Services.Support;

namespace Freenaute.Freebox.Client.Domains.Services.Players;

public sealed class FreeboxPlayerApi(IFreeboxTransport transport)
{
	public async Task<Player[]> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Players.Base, ServicesJsonSerializerContext.Default.PlayerArray,
			cancellationToken).ConfigureAwait(false);
	}

	public FreeboxPlayer Device(long id) => new(transport, ServicesPath.Id(id));
}
