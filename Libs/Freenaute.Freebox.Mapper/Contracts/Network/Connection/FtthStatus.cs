namespace Freenaute.Freebox.Mapper.Contracts.Network.Connection;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#FtthStatus.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class FtthStatus
{
	[JsonPropertyName("sfp_present")] public bool? SfpPresent { get; init; }

	[JsonPropertyName("sfp_alim_ok")] public bool? SfpAlimOk { get; init; }

	[JsonPropertyName("sfp_has_power_report")]
	public bool? SfpHasPowerReport { get; init; }

	[JsonPropertyName("sfp_has_signal")] public bool? SfpHasSignal { get; init; }

	[JsonPropertyName("link")] public bool? Link { get; init; }

	[JsonPropertyName("sfp_serial")] public string? SfpSerial { get; init; }

	[JsonPropertyName("sfp_model")] public string? SfpModel { get; init; }

	[JsonPropertyName("sfp_vendor")] public string? SfpVendor { get; init; }

	/// <summary>scaled by 100 (in dBm)</summary>
	[JsonPropertyName("sfp_pwr_tx")]
	public long? SfpPwrTx { get; init; }

	/// <summary>scaled by 100 (in dBm)</summary>
	[JsonPropertyName("sfp_pwr_rx")]
	public long? SfpPwrRx { get; init; }
}
