namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiAllowedComb.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiAllowedComb
{
	/// <summary>the band for which the combination can be used band Description 2d4g 2.4 GHz 5g 5 GHz 60g 60 GHz</summary>
	[JsonPropertyName("band")]
	public string? Band { get; init; }

	/// <summary>the channel_width for which the combination can be used</summary>
	[JsonPropertyName("channel_width")]
	public string? ChannelWidth { get; init; }

	/// <summary>does this combination requires DFS. You should only allow this combination if ap has allowed dfs.</summary>
	[JsonPropertyName("need_dfs")]
	public bool? NeedDfs { get; init; }

	/// <summary>time required in dfs state before being able to start the AP.</summary>
	[JsonPropertyName("dfs_cac_time")]
	public long? DfsCacTime { get; init; }

	/// <summary>
	///     is this using a PSC channel as primary. Some phones/PCs can only see 6GHz APs when their primary channel is a
	///     Preferred Scanning Channel (PSC).
	/// </summary>
	[JsonPropertyName("psc")]
	public bool? Psc { get; init; }

	/// <summary>primary channel</summary>
	[JsonPropertyName("primary")]
	public long? Primary { get; init; }

	/// <summary>secondary channel (zero means that secondary channel will not be used)</summary>
	[JsonPropertyName("secondary")]
	public long? Secondary { get; init; }
}
