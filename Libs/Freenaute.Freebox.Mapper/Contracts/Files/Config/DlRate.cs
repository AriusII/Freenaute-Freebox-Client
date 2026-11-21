namespace Freenaute.Freebox.Mapper.Contracts.Files.Config;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#DlRate. Unknown enum strings are preserved.</summary>
public sealed record DlRate
{
	/// <summary>http://mafreebox.freebox.fr/doc/index.html#DlRate.tx_rate</summary>
	[JsonPropertyName("tx_rate")]
	public long? TxRate { get; init; }

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#DlRate.rx_rate</summary>
	[JsonPropertyName("rx_rate")]
	public long? RxRate { get; init; }
}
