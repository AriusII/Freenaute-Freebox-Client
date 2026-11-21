namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxProfilesApi
{
	private readonly IFreeboxTransport transport;

	internal FreeboxProfilesApi(IFreeboxTransport transport)
	{
		this.transport = transport;
	}

	public Task<ImmutableArray<Profile>> ListAsync(CancellationToken cancellationToken = default) =>
		transport.SendAsync(HttpMethod.Get, "profile", SystemHomeWire.Json.Profiles, cancellationToken);

	public FreeboxProfileResource For(long id) => new(transport, id);

	public FreeboxSystemHomeCommand<ProfileCreateRequest, ProfileCreated> Create(string name)
	{
		return new FreeboxSystemHomeCommand<ProfileCreateRequest, ProfileCreated>(transport, HttpMethod.Post,
			"profile/", new ProfileCreateRequest(name), SystemHomeWire.Json.ProfileCreateRequest,
			SystemHomeWire.Json.ProfileCreated);
	}

	public Task<DefaultModeMigrationStatus> GetMigrationStatusAsync(CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, "network_control/migrate",
			SystemHomeWire.Json.DefaultModeMigrationStatus, cancellationToken);
	}

	public Task<DefaultModeMigrationStatus> MigrateDefaultModeAsync(CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Post, "network_control/migrate",
			SystemHomeWire.Json.DefaultModeMigrationStatus, cancellationToken);
	}
}
