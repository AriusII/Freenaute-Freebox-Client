namespace Freenaute.Freebox.Mapper.Contracts.Services.Parental;

/// <summary>Exact wire values documented for the parental filter_state; unknown incoming strings are preserved.</summary>
/// <seealso href="https://dev.freebox.fr/sdk/os/parental/" />
[JsonConverter(typeof(ParentalFilterStateJsonConverter))]
public readonly record struct ParentalFilterState(string Value)
{
	public static ParentalFilterState Allowed => new(MapperConstants.ServiceWireTokens.Parental.Allowed);
	public static ParentalFilterState Denied => new(MapperConstants.ServiceWireTokens.Parental.Denied);
	public static ParentalFilterState Webonly => new(MapperConstants.ServiceWireTokens.Parental.Webonly);

	public override string ToString() => Value ?? string.Empty;
}
