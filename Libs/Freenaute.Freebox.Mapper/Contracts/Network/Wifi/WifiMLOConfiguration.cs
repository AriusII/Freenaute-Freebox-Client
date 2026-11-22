namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiMLOConfiguration.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiMloConfiguration
{
	/// <summary>
	///     List of phys participating in the MLD for the BSS An empty array means MLO is disabled An array with only the
	///     BSS’s AP index in it means SLO (single link mode) The allowed combinations are retrieved by the mlo/allowed_comb
	///     api.
	/// </summary>
	[JsonPropertyName("partners")]
	public long[]? Partners { get; init; }
}
