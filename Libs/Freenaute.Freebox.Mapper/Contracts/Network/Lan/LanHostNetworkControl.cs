namespace Freenaute.Freebox.Mapper.Contracts.Network.Lan;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LanHostNetworkControl.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LanHostNetworkControl
{
	/// <summary>Id of profile this device is associated with.</summary>
	[JsonPropertyName("profile_id")]
	public long? ProfileId { get; init; }

	/// <summary>Name of profile this device is associated with.</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>Mode described in Network Control Object</summary>
	[JsonPropertyName("current_mode")]
	public string? CurrentMode { get; init; }
}
