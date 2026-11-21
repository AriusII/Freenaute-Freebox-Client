namespace Freenaute.Freebox.Mapper.Contracts.Services.Parental;

/// <summary>Exact wire values documented for ParentalFilter.scheduling_mode; unknown incoming strings are preserved.</summary>
/// <seealso href="https://dev.freebox.fr/sdk/os/parental/" />
[JsonConverter(typeof(ParentalSchedulingModeJsonConverter))]
public readonly record struct ParentalSchedulingMode(string Value)
{
	public static ParentalSchedulingMode Forced => new("forced");
	public static ParentalSchedulingMode Temporary => new("temporary");
	public static ParentalSchedulingMode Planning => new("planning");

	public override string ToString() => Value ?? string.Empty;
}
