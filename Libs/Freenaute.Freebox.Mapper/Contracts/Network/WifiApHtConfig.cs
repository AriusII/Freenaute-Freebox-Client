namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiApHtConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiApHtConfig
{
	/// <summary>enable 802.11ac</summary>
	[JsonPropertyName("ac_enabled")]
	public bool? AcEnabled { get; init; }
}
