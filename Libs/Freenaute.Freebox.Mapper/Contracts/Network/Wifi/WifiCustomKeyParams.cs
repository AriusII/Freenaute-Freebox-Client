namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiCustomKeyParams.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiCustomKeyParams
{
	/// <summary>description of the custom key</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>Wi-Fi password for this custom access “ **** ” will be returned when insufficient permission</summary>
	[JsonPropertyName("key")]
	public string? Key { get; init; }

	/// <summary>
	///     Number of different hosts that can connect to this network (maximum 127) 0 has special meaning, it means
	///     unlimited number of users.
	/// </summary>
	[JsonPropertyName("max_use_count")]
	public StringOrInteger? MaxUseCount { get; init; }

	/// <summary>Number of seconds before the custom access is revoked</summary>
	[JsonPropertyName("duration")]
	public long? Duration { get; init; }

	/// <summary>
	///     access_type Description full stations will get full access to local network + internet net_only stations
	///     connected using this custom key will be isolated and won’t have access to local network devices
	/// </summary>
	[JsonPropertyName("access_type")]
	public string? AccessType { get; init; }
}
