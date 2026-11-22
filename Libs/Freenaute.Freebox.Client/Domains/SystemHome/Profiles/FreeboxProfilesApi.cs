using Freenaute.Freebox.Client.Domains.SystemHome.Support;

namespace Freenaute.Freebox.Client.Domains.SystemHome.Profiles;

public sealed class FreeboxProfilesApi
{
	private readonly IFreeboxTransport _transport;

	internal FreeboxProfilesApi(IFreeboxTransport transport)
	{
		_transport = transport;
	}

	public async Task<ImmutableArray<Profile>> ListAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, SystemHomeConstants.Routes.Profile, SystemHomeWire.Json.Profiles, cancellationToken)
			.ConfigureAwait(false);

	public FreeboxProfileResource For(long id) => new(_transport, id);

	public FreeboxSystemHomeCommand<ProfileCreateRequest, ProfileCreated> Create(string name)
	{
		return new FreeboxSystemHomeCommand<ProfileCreateRequest, ProfileCreated>(_transport, HttpMethod.Post,
			"profile/", new ProfileCreateRequest(name), SystemHomeWire.Json.ProfileCreateRequest,
			SystemHomeWire.Json.ProfileCreated);
	}

	public async Task<DefaultModeMigrationStatus> GetMigrationStatusAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, SystemHomeConstants.Routes.NetworkControlMigrate,
			SystemHomeWire.Json.DefaultModeMigrationStatus, cancellationToken).ConfigureAwait(false);
	}

	public async Task<DefaultModeMigrationStatus> MigrateDefaultModeAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Post, SystemHomeConstants.Routes.NetworkControlMigrate,
			SystemHomeWire.Json.DefaultModeMigrationStatus, cancellationToken).ConfigureAwait(false);
	}
}
