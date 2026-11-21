namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed record LedstripPlanningUpdate : ISystemHomeRequest
{
    [JsonPropertyName("use_planning")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalBooleanJsonConverter))]
    public Optional<bool> UsePlanning { get; init; }

    [JsonPropertyName("planning_mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<LedstripPlanningMode>))]
    public Optional<LedstripPlanningMode> PlanningMode { get; init; }

    [JsonPropertyName("mapping")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<ImmutableArray<bool>>))]
    public Optional<ImmutableArray<bool>> Mapping { get; init; }

    public void Validate()
    {
        SystemHomeRequestValidation.NoNull(UsePlanning, nameof(UsePlanning));
        SystemHomeRequestValidation.NoNull(PlanningMode, nameof(PlanningMode));
        if (PlanningMode.HasValue && PlanningMode.Value.Value is null)
        {
            throw new ArgumentException("Undefined token.", nameof(PlanningMode));
        }

        SystemHomeRequestValidation.Array(Mapping, nameof(Mapping));
    }

    public LedstripPlanningUpdate WithUsePlanning(bool value)
    {
        return this with { UsePlanning = Optional<bool>.FromValue(value) };
    }

    public LedstripPlanningUpdate WithPlanningMode(LedstripPlanningMode value)
    {
        return this with { PlanningMode = Optional<LedstripPlanningMode>.FromValue(value) };
    }

    public LedstripPlanningUpdate WithMapping(IEnumerable<bool> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        return this with { Mapping = Optional<ImmutableArray<bool>>.FromValue(values.ToImmutableArray()) };
    }
}
