namespace Freenaute.Freebox.Client.Authentication;

public sealed class FreeboxAuthorizationCoordinator(
	IFreeboxClient client,
	FreeboxClientOptions options,
	IFreeboxCredentialStore credentialStore) : IFreeboxAuthorizationCoordinator
{
	public async ValueTask<FreeboxApprovalRegistration> BeginPairingAsync(
		CancellationToken cancellationToken = default)
	{
		EnsureStoreAvailable();
		var credentialId = await GetCredentialIdAsync(cancellationToken).ConfigureAwait(false);
		var existing = await credentialStore.GetAsync(credentialId, cancellationToken).ConfigureAwait(false);
		if (existing is not null)
		{
			if (existing.Status is FreeboxCredentialStatus.Pending && existing.TrackId is { } pendingTrackId)
			{
				return new FreeboxApprovalRegistration(existing.Status, pendingTrackId);
			}

			if (existing.Status is FreeboxCredentialStatus.Approved)
			{
				return new FreeboxApprovalRegistration(existing.Status);
			}
		}

		string applicationId = Require(options.ApplicationId, nameof(options.ApplicationId));
		string applicationVersion = Require(options.ApplicationVersion, nameof(options.ApplicationVersion));
		var authorization = await client.Authentication.AuthorizeAsync(
			new TokenRequest(applicationId, options.ApplicationName, applicationVersion, options.DeviceName),
			cancellationToken).ConfigureAwait(false);
		var pending = new FreeboxAppCredentials(applicationId, authorization.AppToken,
			FreeboxCredentialStatus.Pending, authorization.TrackId);

		// Persist before polling so a process restart resumes the same physical approval.
		await credentialStore.PutAsync(credentialId, pending, cancellationToken).ConfigureAwait(false);
		return new FreeboxApprovalRegistration(FreeboxCredentialStatus.Pending, authorization.TrackId);
	}

	public async ValueTask<FreeboxAppCredentials> WaitForApprovalAsync(TimeSpan? pollInterval = null,
		CancellationToken cancellationToken = default)
	{
		EnsureStoreAvailable();
		var credentialId = await GetCredentialIdAsync(cancellationToken).ConfigureAwait(false);
		var credentials = await credentialStore.GetAsync(credentialId, cancellationToken).ConfigureAwait(false)
		                  ?? throw new FreeboxAuthenticationException("Begin pairing before waiting for approval.");
		if (credentials.Status is FreeboxCredentialStatus.Approved)
		{
			return credentials;
		}

		if (credentials.Status is not FreeboxCredentialStatus.Pending || credentials.TrackId is not { } trackId)
		{
			throw new FreeboxAuthenticationException("No resumable pending Freebox approval is available.");
		}

		var tracked = await client.Authentication.WaitForAuthorizationAsync(trackId, pollInterval, cancellationToken)
			.ConfigureAwait(false);
		if (tracked.Status is not AuthorizationTrackEnum.Granted)
		{
			await credentialStore.PutAsync(credentialId,
					credentials.WithStatus(FreeboxCredentialStatus.Invalid, trackId), cancellationToken)
				.ConfigureAwait(false);
			throw new FreeboxAuthorizationException(tracked.Status);
		}

		var approved = credentials.WithStatus(FreeboxCredentialStatus.Approved, trackId);
		// The authorization response challenge is deliberately not copied into the store.
		await credentialStore.PutAsync(credentialId, approved, cancellationToken).ConfigureAwait(false);
		return approved;
	}

	public async ValueTask ForgetAsync(CancellationToken cancellationToken = default)
	{
		EnsureStoreAvailable();
		var credentialId = await GetCredentialIdAsync(cancellationToken).ConfigureAwait(false);
		await credentialStore.DeleteAsync(credentialId, cancellationToken).ConfigureAwait(false);
	}

	private async Task<FreeboxCredentialId> GetCredentialIdAsync(CancellationToken cancellationToken)
	{
		string applicationId = Require(options.ApplicationId, nameof(options.ApplicationId));
		var version = await client.Discovery.GetApiVersionAsync(cancellationToken).ConfigureAwait(false);
		return new FreeboxCredentialId(options.CredentialProfile, applicationId, version.Uid);
	}

	private void EnsureStoreAvailable()
	{
		if (!credentialStore.IsAvailable)
		{
			throw new InvalidOperationException($"The '{credentialStore.Kind}' credential store is unavailable.");
		}
	}

	private static string Require(string? value, string name)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value, name);
		return value;
	}
}
