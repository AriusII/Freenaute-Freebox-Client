namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#SystemModelInfo; absent metadata remains
///     nullable.
/// </summary>
public sealed record SystemModelInfo
{
	// Example-only metadata: retain raw counters/tokens without inventing units or enums.
	[JsonPropertyName("internal_hdd_size")]
	public long? InternalHddSize { get; init; }

	[JsonPropertyName("wifi_type")] public string? WifiType { get; init; }
	[JsonPropertyName("name")] public string? Name { get; init; }
	[JsonPropertyName("pretty_name")] public string? PrettyName { get; init; }
	[JsonPropertyName("has_expansions")] public bool? HasExpansions { get; init; }
	[JsonPropertyName("has_lan_sfp")] public bool? HasLanSfp { get; init; }
	[JsonPropertyName("has_dect")] public bool? HasDect { get; init; }

	[JsonPropertyName("has_home_automation")]
	public bool? HasHomeAutomation { get; init; }

	[JsonPropertyName("has_femtocell_exp")]
	public bool? HasFemtocellExp { get; init; }

	[JsonPropertyName("has_fixed_femtocell")]
	public bool? HasFixedFemtocell { get; init; }

	[JsonPropertyName("has_vm")] public bool? HasVm { get; init; }
	[JsonPropertyName("has_dsl")] public bool? HasDsl { get; init; }
	[JsonPropertyName("has_standby")] public bool? HasStandby { get; init; }
	[JsonPropertyName("has_eco_wifi")] public bool? HasEcoWifi { get; init; }
	[JsonPropertyName("has_wop")] public bool? HasWop { get; init; }
	[JsonPropertyName("has_led_strip")] public bool? HasLedStrip { get; init; }
	[JsonPropertyName("has_status_led")] public bool? HasStatusLed { get; init; }
	[JsonPropertyName("has_usb3_enable")] public bool? HasUsb3Enable { get; init; }

	[JsonPropertyName("has_lcd_screensaver")]
	public bool? HasLcdScreensaver { get; init; }
}
