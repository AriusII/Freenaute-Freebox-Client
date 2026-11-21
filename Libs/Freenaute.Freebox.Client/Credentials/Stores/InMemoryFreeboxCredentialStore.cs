namespace Freenaute.Freebox.Client.Credentials.Stores;

/// <summary>Thread-safe development/test store kept only in process memory.</summary>
public sealed class InMemoryFreeboxCredentialStore : IFreeboxCredentialStore
{
	private readonly ConcurrentDictionary<string, FreeboxAppCredentials> _credentials = new(StringComparer.Ordinal);

	public bool IsAvailable => true;
	public string Kind => "in-memory";

	public ValueTask<FreeboxAppCredentials?> GetAsync(FreeboxCredentialId id,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(id);
		cancellationToken.ThrowIfCancellationRequested();
		return ValueTask.FromResult(_credentials.TryGetValue(id.StorageKey, out var credentials)
			? Clone(credentials)
			: null);
	}

	public ValueTask PutAsync(FreeboxCredentialId id, FreeboxAppCredentials credentials,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(id);
		ArgumentNullException.ThrowIfNull(credentials);
		cancellationToken.ThrowIfCancellationRequested();
		_credentials[id.StorageKey] = Clone(credentials);
		return ValueTask.CompletedTask;
	}

	public ValueTask DeleteAsync(FreeboxCredentialId id, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(id);
		cancellationToken.ThrowIfCancellationRequested();
		_credentials.TryRemove(id.StorageKey, out _);
		return ValueTask.CompletedTask;
	}

	private static FreeboxAppCredentials Clone(FreeboxAppCredentials credentials)
	{
		return new FreeboxAppCredentials(credentials.ApplicationId, credentials.GetAppToken(), credentials.Status,
			credentials.TrackId, credentials.CreatedAt) { UpdatedAt = credentials.UpdatedAt };
	}
}
