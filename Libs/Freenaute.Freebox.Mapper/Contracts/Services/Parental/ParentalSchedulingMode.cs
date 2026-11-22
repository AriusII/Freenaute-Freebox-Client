namespace Freenaute.Freebox.Mapper.Contracts.Services.Parental;

/// <summary>Exact wire values documented for ParentalFilter.scheduling_mode; unknown incoming strings are preserved.</summary>
/// <seealso href="https://dev.freebox.fr/sdk/os/parental/" />
[JsonConverter(typeof(ParentalSchedulingModeJsonConverter))]
public readonly record struct ParentalSchedulingMode(string Value)
{
	public static ParentalSchedulingMode Forced => new(MapperConstants.ServiceWireTokens.Parental.Forced);
	public static ParentalSchedulingMode Temporary => new(MapperConstants.ServiceWireTokens.Parental.Temporary);
	public static ParentalSchedulingMode Planning => new(MapperConstants.ServiceWireTokens.Parental.Planning);

	public override string ToString() => Value ?? string.Empty;
}
