namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiGlobalConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiGlobalConfig
{
	/// <summary>is wifi enabled</summary>
	[JsonPropertyName("enabled")]
	public bool? Enabled { get; init; }

	/// <summary>
	///     mac_filter_state Description disabled mac filter is disabled whitelist mac filter is enabled, using a
	///     whitelist blacklist mac filter is enabled, using a blacklist
	/// </summary>
	[JsonPropertyName("mac_filter_state")]
	public string? MacFilterState { get; init; }
}
