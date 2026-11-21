namespace Freenaute.Freebox.Client.Domains.SystemHome.Profiles;

public sealed class FreeboxProfileResource
{
	private readonly string path;
	private readonly IFreeboxTransport transport;

	internal FreeboxProfileResource(IFreeboxTransport transport, long id)
	{
		this.transport = transport;
		path = "profile/" + SystemHomeWire.Id(id);
		NetworkControl = new FreeboxNetworkControlResource(transport, id);
	}

	public FreeboxNetworkControlResource NetworkControl { get; }

	public Task<Profile> GetAsync(CancellationToken cancellationToken = default) =>
		transport.SendAsync(HttpMethod.Get, path, SystemHomeWire.Json.Profile, cancellationToken);

	public Task DeleteAsync(CancellationToken cancellationToken = default) =>
		transport.SendAsync(HttpMethod.Delete, path, cancellationToken);
}
