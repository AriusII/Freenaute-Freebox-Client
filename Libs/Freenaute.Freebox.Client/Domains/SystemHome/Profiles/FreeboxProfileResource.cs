namespace Freenaute.Freebox.Client.Domains.SystemHome.Profiles;

public sealed class FreeboxProfileResource
{
	private readonly string _path;
	private readonly IFreeboxTransport _transport;

	internal FreeboxProfileResource(IFreeboxTransport transport, long id)
	{
		_transport = transport;
		_path = "profile/" + SystemHomeWire.Id(id);
		NetworkControl = new FreeboxNetworkControlResource(transport, id);
	}

	public FreeboxNetworkControlResource NetworkControl { get; }

	public async Task<Profile> GetAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, _path, SystemHomeWire.Json.Profile, cancellationToken)
			.ConfigureAwait(false);

	public async Task DeleteAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Delete, _path, cancellationToken).ConfigureAwait(false);
}
