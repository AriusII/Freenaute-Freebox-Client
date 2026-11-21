namespace Freenaute.Freebox.Mapper.Contracts.Services.Parental;

/// <summary>Write projection of ParentalFilterConfig; unset fields are omitted and explicit null is rejected.</summary>
/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#ParentalFilterConfig" />
public sealed record ParentalFilterConfigPatch
{
	[JsonPropertyName("default_filter_mode")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<ParentalFilterState>))]
	public Optional<ParentalFilterState> DefaultFilterMode { get; init; }

	public void Validate()
	{
		if (DefaultFilterMode.IsNull)
		{
			throw new ArgumentException("Explicit null is not documented for ParentalFilterConfig.DefaultFilterMode.",
				nameof(DefaultFilterMode));
		}
	}

	public override string ToString() => nameof(ParentalFilterConfigPatch);
}
