namespace Freenaute.Freebox.Client.Credentials;

/// <summary>Reusable application pairing data. Transient session data is intentionally absent.</summary>
public sealed class FreeboxAppCredentials
{
	public FreeboxAppCredentials(string applicationId, string appToken, FreeboxCredentialStatus status,
		int? trackId = null, DateTimeOffset? createdAt = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(applicationId);
		ArgumentException.ThrowIfNullOrWhiteSpace(appToken);
		if (trackId is < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(trackId));
		}

		ApplicationId = applicationId;
		AppToken = appToken;
		Status = status;
		TrackId = trackId;
		CreatedAt = createdAt ?? DateTimeOffset.UtcNow;
		UpdatedAt = CreatedAt;
	}

	public string ApplicationId { get; }
	public FreeboxCredentialStatus Status { get; }
	public int? TrackId { get; private init; }
	public DateTimeOffset CreatedAt { get; }
	public DateTimeOffset UpdatedAt { get; internal init; }
	public static int FormatVersion => 1;

	internal string AppToken { get; }

	/// <summary>Reads the secret for a secure store implementation or authentication operation.</summary>
	public string GetAppToken() => AppToken;

	internal FreeboxAppCredentials WithStatus(FreeboxCredentialStatus status, int? trackId)
	{
		return new FreeboxAppCredentials(ApplicationId, AppToken, status, trackId, CreatedAt)
		{
			UpdatedAt = DateTimeOffset.UtcNow
		};
	}

	public override string ToString()
	{
		return
			$"FreeboxAppCredentials {{{{ ApplicationId = {ApplicationId}, Status = {Status}, AppToken = <redacted> }}}}";
	}
}
