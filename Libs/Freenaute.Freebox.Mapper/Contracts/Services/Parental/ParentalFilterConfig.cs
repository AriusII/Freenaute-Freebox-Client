namespace Freenaute.Freebox.Mapper.Contracts.Services.Parental;

/// <summary>Read projection of the documented ParentalFilterConfig wire object.</summary>
/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#ParentalFilterConfig" />
public sealed record ParentalFilterConfig
{
	[JsonPropertyName("default_filter_mode")]
	public ParentalFilterState? DefaultFilterMode { get; init; }

	public override string ToString() => nameof(ParentalFilterConfig);
}
