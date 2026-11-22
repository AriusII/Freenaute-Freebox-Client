namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.NetworkControls;

public sealed record NetworkControlRuleUpdate : ISystemHomeRequest
{
	[JsonPropertyName("name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Name { get; init; }

	[JsonPropertyName("mode")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<NetworkControlMode>))]
	public Optional<NetworkControlMode> Mode { get; init; }

	[JsonPropertyName("start_time")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalInt64JsonConverter))]
	public Optional<long> StartTime { get; init; }

	[JsonPropertyName("end_time")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalInt64JsonConverter))]
	public Optional<long> EndTime { get; init; }

	[JsonPropertyName("weekdays")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<ImmutableArray<bool>>))]
	public Optional<ImmutableArray<bool>> Weekdays { get; init; }

	[JsonPropertyName("enabled")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalBooleanJsonConverter))]
	public Optional<bool> Enabled { get; init; }

	public void Validate()
	{
		SystemHomeRequestValidation.NoNull(Name, nameof(Name));
		SystemHomeRequestValidation.NoNull(Mode, nameof(Mode));
		if (Mode.HasValue && Mode.Value.Value is null)
		{
			throw new ArgumentException(MapperConstants.ExceptionMessages.Validation.UndefinedToken, nameof(Mode));
		}

		SystemHomeRequestValidation.NoNull(StartTime, nameof(StartTime));
		SystemHomeRequestValidation.NoNull(EndTime, nameof(EndTime));
		SystemHomeRequestValidation.Array(Weekdays, nameof(Weekdays));
		SystemHomeRequestValidation.NoNull(Enabled, nameof(Enabled));
	}

	public NetworkControlRuleUpdate WithName(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return this with { Name = Optional<string>.FromValue(value) };
	}

	public NetworkControlRuleUpdate WithMode(NetworkControlMode value) =>
		this with { Mode = Optional<NetworkControlMode>.FromValue(value) };

	public NetworkControlRuleUpdate WithStartTime(long value) =>
		this with { StartTime = Optional<long>.FromValue(value) };

	public NetworkControlRuleUpdate WithEndTime(long value) =>
		this with { EndTime = Optional<long>.FromValue(value) };

	public NetworkControlRuleUpdate WithWeekdays(IEnumerable<bool> values)
	{
		ArgumentNullException.ThrowIfNull(values);
		return this with { Weekdays = Optional<ImmutableArray<bool>>.FromValue([.. values]) };
	}

	public NetworkControlRuleUpdate WithEnabled(bool value) =>
		this with { Enabled = Optional<bool>.FromValue(value) };
}
