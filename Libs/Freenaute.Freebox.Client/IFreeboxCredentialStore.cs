namespace Freenaute.Freebox.Client;

/// <summary>Stores reusable application credentials without storing transient Freebox sessions.</summary>
public interface IFreeboxCredentialStore
{
	/// <summary>Whether this store is currently available for reads and writes.</summary>
	bool IsAvailable { get; }

	/// <summary>A non-secret name describing the storage backend.</summary>
	string Kind { get; }

	ValueTask<FreeboxAppCredentials?> GetAsync(FreeboxCredentialId id, CancellationToken cancellationToken = default);

	ValueTask PutAsync(FreeboxCredentialId id, FreeboxAppCredentials credentials,
		CancellationToken cancellationToken = default);

	ValueTask DeleteAsync(FreeboxCredentialId id, CancellationToken cancellationToken = default);
}
