namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Lcd;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#LcdConfig; absent metadata remains nullable.</summary>
public sealed record LcdConfig
{
	[JsonPropertyName("brightness")] public long? Brightness { get; init; }

	[JsonPropertyName("orientation_forced")]
	public bool? OrientationForced { get; init; }

	[JsonPropertyName("orientation")] public long? Orientation { get; init; }
	[JsonPropertyName("hide_wifi_key")] public bool? HideWifiKey { get; init; }

	[JsonPropertyName("led_strip_enabled")]
	public bool? LedStripEnabled { get; init; }

	[JsonPropertyName("led_strip_brightness")]
	public long? LedStripBrightness { get; init; }

	[JsonPropertyName("led_strip_animation")]
	public LcdLedAnimation? LedStripAnimation { get; init; }

	[JsonPropertyName("available_led_strip_animations")]
	public ImmutableArray<LcdLedAnimation>? AvailableLedStripAnimations { get; init; }

	[JsonPropertyName("hide_status_led")] public bool? HideStatusLed { get; init; }

	[JsonPropertyName("screensaver")] public LcdScreensaver? Screensaver { get; init; }

	// Example key retained separately; no automatic alias rewrite.
	[JsonPropertyName("hide_led")] public bool? HideLed { get; init; }
}
