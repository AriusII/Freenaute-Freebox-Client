namespace Freenaute.Freebox.Mapper.Contracts.Services.Parental;

/// <summary>
///     Write projection of ParentalFilterPlanning; unset fields are omitted and explicit null is rejected. The array
///     size of mapping is (number of cdayranges + 7) * resolution.
/// </summary>
/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#ParentalFilterPlanning" />
public sealed record ParentalFilterPlanningPatch
{
	[JsonPropertyName("resolution")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<long>))]
	public Optional<long> Resolution { get; init; }

	[JsonPropertyName("cdayranges")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string[]>))]
	public Optional<string[]> Cdayranges { get; init; }

	[JsonPropertyName("mapping")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<ParentalFilterState[]>))]
	public Optional<ParentalFilterState[]> Mapping { get; init; }

	public void Validate()
	{
		if (Resolution.IsNull)
		{
			throw new ArgumentException("Explicit null is not documented for ParentalFilterPlanning.Resolution.",
				nameof(Resolution));
		}

		if (Cdayranges.IsNull)
		{
			throw new ArgumentException("Explicit null is not documented for ParentalFilterPlanning.Cdayranges.",
				nameof(Cdayranges));
		}

		if (Mapping.IsNull)
		{
			throw new ArgumentException("Explicit null is not documented for ParentalFilterPlanning.Mapping.",
				nameof(Mapping));
		}
	}

	public override string ToString() => nameof(ParentalFilterPlanningPatch);
}
