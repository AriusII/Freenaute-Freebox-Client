namespace Freenaute.Freebox.Mapper.Contracts.Network.Connection;

/// <summary>FTTH link status. This API is unstable and may change.</summary>
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
	[JsonPropertyName("sfp_pwr_tx")] public long? SfpPowerTx { get; init; }
	[JsonPropertyName("sfp_pwr_rx")] public long? SfpPowerRx { get; init; }
}
