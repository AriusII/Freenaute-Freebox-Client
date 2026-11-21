namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiBssStatus.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiBssStatus
{
	/// <summary>
	///     state Description phy_stopped associated AP is stopped no_param bss is missing config bad_param bss has an
	///     invalid config disabled bss is disabled temp_disabled bss has been temporary disabled starting bss is starting
	///     active bss is active failed bss has failed to start
	/// </summary>
	[JsonPropertyName("state")]
	public string? State { get; init; }

	/// <summary>number of stations for this bss</summary>
	[JsonPropertyName("sta_count")]
	public long? StaCount { get; init; }

	/// <summary>number of authenticated stations for this bss</summary>
	[JsonPropertyName("authorized_sta_count")]
	public long? AuthorizedStaCount { get; init; }

	/// <summary>SSID to use with custom keys</summary>
	[JsonPropertyName("custom_key_ssid")]
	public string? CustomKeySsid { get; init; }

	/// <summary>
	///     The currently active MLO partners’s AP for this BSS. Can be empty if MLO is disabled. See the MLO chapter for
	///     more info
	/// </summary>
	[JsonPropertyName("partners")]
	public long[]? Partners { get; init; }
}
