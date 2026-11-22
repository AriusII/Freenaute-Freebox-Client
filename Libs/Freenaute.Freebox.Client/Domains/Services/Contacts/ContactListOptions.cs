namespace Freenaute.Freebox.Client.Domains.Services.Contacts;

public sealed record ContactListOptions
{
	/// <summary>Offset for contact enumeration.</summary>
	public long? Start { get; init; }

	/// <summary>Maximum items to return (-1 means no limit, per Freebox docs).</summary>
	public long? Limit { get; init; }

	/// <summary>Filters contacts by group identifier.</summary>
	public long? GroupId { get; init; }
}
