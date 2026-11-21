namespace Freenaute.Freebox.Mapper.Contracts.Services.Parental;

/// <summary>Read projection of the documented ParentalFilterPlanning wire object.</summary>
/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#ParentalFilterPlanning" />
public sealed record ParentalFilterPlanning
{
	[JsonPropertyName("resolution")] public long? Resolution { get; init; }

	[JsonPropertyName("cdayranges")] public string[]? Cdayranges { get; init; }

	[JsonPropertyName("mapping")] public ParentalFilterState[]? Mapping { get; init; }

	public override string ToString() => nameof(ParentalFilterPlanning);
}
