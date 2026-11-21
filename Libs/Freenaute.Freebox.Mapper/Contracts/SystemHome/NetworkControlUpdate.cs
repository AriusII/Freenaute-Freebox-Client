namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed record NetworkControlUpdate : ISystemHomeRequest
{
	[JsonPropertyName("override_mode")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<NetworkControlMode>))]
	public Optional<NetworkControlMode> OverrideMode { get; init; }

	[JsonPropertyName("override_until")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalInt64JsonConverter))]
	public Optional<long> OverrideUntil { get; init; }

	[JsonPropertyName("override")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalBooleanJsonConverter))]
	public Optional<bool> Override { get; init; }

	[JsonPropertyName("macs")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<ImmutableArray<string>>))]
	public Optional<ImmutableArray<string>> Macs { get; init; }

	[JsonPropertyName("cdayranges")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<ImmutableArray<string>>))]
	public Optional<ImmutableArray<string>> CustomDayRanges { get; init; }

	public void Validate()
	{
		SystemHomeRequestValidation.NoNull(OverrideMode, nameof(OverrideMode));
		if (OverrideMode.HasValue && OverrideMode.Value.Value is null)
		{
			throw new ArgumentException("Undefined token.", nameof(OverrideMode));
		}

		SystemHomeRequestValidation.NoNull(OverrideUntil, nameof(OverrideUntil));
		SystemHomeRequestValidation.NoNull(Override, nameof(Override));
		SystemHomeRequestValidation.Array(Macs, nameof(Macs));
		SystemHomeRequestValidation.Array(CustomDayRanges, nameof(CustomDayRanges));
	}

	public NetworkControlUpdate WithOverrideMode(NetworkControlMode value) =>
		this with { OverrideMode = Optional<NetworkControlMode>.FromValue(value) };

	public NetworkControlUpdate WithOverrideUntil(long value) =>
		this with { OverrideUntil = Optional<long>.FromValue(value) };

	public NetworkControlUpdate WithOverride(bool value) => this with { Override = Optional<bool>.FromValue(value) };

	public NetworkControlUpdate WithMacs(IEnumerable<string> values)
	{
		ArgumentNullException.ThrowIfNull(values);
		return this with { Macs = Optional<ImmutableArray<string>>.FromValue(values.ToImmutableArray()) };
	}

	public NetworkControlUpdate WithCustomDayRanges(IEnumerable<string> values)
	{
		ArgumentNullException.ThrowIfNull(values);
		return this with { CustomDayRanges = Optional<ImmutableArray<string>>.FromValue(values.ToImmutableArray()) };
	}
}
