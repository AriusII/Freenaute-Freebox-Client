namespace Freenaute.Freebox.Client;

/// <summary>Identifies one application credential for one Freebox and caller-defined profile.</summary>
public sealed class FreeboxCredentialId : IEquatable<FreeboxCredentialId>
{
	public FreeboxCredentialId(string profile, string applicationId, string? freeboxUid = null)
	{
		Profile = ValidateComponent(profile, nameof(profile));
		ApplicationId = ValidateComponent(applicationId, nameof(applicationId));
		FreeboxUid = freeboxUid is null ? null : ValidateComponent(freeboxUid, nameof(freeboxUid));
	}

	public string Profile { get; }
	public string ApplicationId { get; }
	public string? FreeboxUid { get; }

	internal string StorageKey => FreeboxUid is null
		? string.Create(CultureInfo.InvariantCulture, $"{Profile}/{ApplicationId}")
		: string.Create(CultureInfo.InvariantCulture, $"{Profile}/{ApplicationId}/{FreeboxUid}");

	public bool Equals(FreeboxCredentialId? other)
	{
		return other is not null &&
		       StringComparer.Ordinal.Equals(Profile, other.Profile) &&
		       StringComparer.Ordinal.Equals(ApplicationId, other.ApplicationId) &&
		       StringComparer.Ordinal.Equals(FreeboxUid, other.FreeboxUid);
	}

	public override bool Equals(object? obj) => Equals(obj as FreeboxCredentialId);

	public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(StorageKey);

	public override string ToString() => StorageKey;

	private static string ValidateComponent(string value, string parameterName)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
		if (value.Length > 128 || value.Any(character => character is '/' or '\\' or '\0' || char.IsControl(character)))
		{
			throw new ArgumentException("Credential identity components must be safe, bounded path/key segments.",
				parameterName);
		}

		return value;
	}
}
