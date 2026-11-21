namespace Freenaute.Freebox.Client.Credentials;

public sealed class FreeboxApprovalRegistration
{
	public FreeboxApprovalRegistration(FreeboxCredentialStatus status, int? trackId = null)
	{
		if (trackId is < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(trackId));
		}

		if (status is FreeboxCredentialStatus.Pending && trackId is null)
		{
			throw new ArgumentException("Pending approval requires a track identifier.", nameof(trackId));
		}

		Status = status;
		TrackId = trackId;
	}

	public FreeboxCredentialStatus Status { get; }
	public int? TrackId { get; }
}
