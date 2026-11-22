namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Lcd;

public sealed record LcdConfigPatch : ISystemHomeRequest
{
	[JsonPropertyName("brightness")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalInt32JsonConverter))]
	public Optional<int> Brightness { get; init; }

	[JsonPropertyName("orientation_forced")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalBooleanJsonConverter))]
	public Optional<bool> OrientationForced { get; init; }

	[JsonPropertyName("orientation")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalInt64JsonConverter))]
	public Optional<long> Orientation { get; init; }

	[JsonPropertyName("hide_wifi_key")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalBooleanJsonConverter))]
	public Optional<bool> HideWifiKey { get; init; }

	[JsonPropertyName("led_strip_enabled")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalBooleanJsonConverter))]
	public Optional<bool> LedStripEnabled { get; init; }

	[JsonPropertyName("led_strip_brightness")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalInt32JsonConverter))]
	public Optional<int> LedStripBrightness { get; init; }

	[JsonPropertyName("led_strip_animation")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<LcdLedAnimation>))]
	public Optional<LcdLedAnimation> LedStripAnimation { get; init; }

	[JsonPropertyName("hide_status_led")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalBooleanJsonConverter))]
	public Optional<bool> HideStatusLed { get; init; }

	[JsonPropertyName("screensaver")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<LcdScreensaver>))]
	public Optional<LcdScreensaver> Screensaver { get; init; }

	public void Validate()
	{
		SystemHomeRequestValidation.NoNull(Brightness, nameof(Brightness));
		if (Brightness.HasValue && Brightness.Value is < 0 or > 100)
		{
			throw new ArgumentOutOfRangeException(nameof(Brightness));
		}

		SystemHomeRequestValidation.NoNull(OrientationForced, nameof(OrientationForced));
		SystemHomeRequestValidation.NoNull(Orientation, nameof(Orientation));
		SystemHomeRequestValidation.NoNull(HideWifiKey, nameof(HideWifiKey));
		SystemHomeRequestValidation.NoNull(LedStripEnabled, nameof(LedStripEnabled));
		SystemHomeRequestValidation.NoNull(LedStripBrightness, nameof(LedStripBrightness));
		if (LedStripBrightness.HasValue && LedStripBrightness.Value is < 0 or > 100)
		{
			throw new ArgumentOutOfRangeException(nameof(LedStripBrightness));
		}

		SystemHomeRequestValidation.NoNull(LedStripAnimation, nameof(LedStripAnimation));
		if (LedStripAnimation.HasValue && LedStripAnimation.Value.Value is null)
		{
			throw new ArgumentException(MapperConstants.ExceptionMessages.Validation.UndefinedToken, nameof(LedStripAnimation));
		}

		SystemHomeRequestValidation.NoNull(HideStatusLed, nameof(HideStatusLed));
		SystemHomeRequestValidation.NoNull(Screensaver, nameof(Screensaver));
		if (Screensaver.HasValue && Screensaver.Value.Value is null)
		{
			throw new ArgumentException(MapperConstants.ExceptionMessages.Validation.UndefinedToken, nameof(Screensaver));
		}
	}

	public LcdConfigPatch WithBrightness(int value) => this with { Brightness = Optional<int>.FromValue(value) };

	public LcdConfigPatch WithOrientationForced(bool value) =>
		this with { OrientationForced = Optional<bool>.FromValue(value) };

	public LcdConfigPatch WithOrientation(long value) => this with { Orientation = Optional<long>.FromValue(value) };

	public LcdConfigPatch WithHideWifiKey(bool value) => this with { HideWifiKey = Optional<bool>.FromValue(value) };

	public LcdConfigPatch WithLedStripEnabled(bool value) =>
		this with { LedStripEnabled = Optional<bool>.FromValue(value) };

	public LcdConfigPatch WithLedStripBrightness(int value) =>
		this with { LedStripBrightness = Optional<int>.FromValue(value) };

	public LcdConfigPatch WithLedStripAnimation(LcdLedAnimation value) =>
		this with { LedStripAnimation = Optional<LcdLedAnimation>.FromValue(value) };

	public LcdConfigPatch WithHideStatusLed(bool value) =>
		this with { HideStatusLed = Optional<bool>.FromValue(value) };

	public LcdConfigPatch WithScreensaver(LcdScreensaver value) =>
		this with { Screensaver = Optional<LcdScreensaver>.FromValue(value) };
}
