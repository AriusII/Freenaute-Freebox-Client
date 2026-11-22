namespace Freenaute.Freebox.Mapper.Common.Types;

/// <summary>
///     Represents the model of a Freebox.
/// </summary>
/// <remarks>
///     The model of a Freebox is used to determine the capabilities of the device.
/// </remarks>
/// <seealso href="https://dev.freebox.fr/sdk/os/#get__system__device">
///     Freebox API documentation
/// </seealso>
[JsonConverter(typeof(JsonStringEnumConverter<BoxModels>))]
public enum BoxModels : byte
{
	[JsonStringEnumMemberName(MapperConstants.BoxModels.FreeboxServerV6R1)]
	FreeboxServerV6R1,

	[JsonStringEnumMemberName(MapperConstants.BoxModels.FreeboxServerV6R2)]
	FreeboxServerV6R2,

	[JsonStringEnumMemberName(MapperConstants.BoxModels.FreeboxMiniR1)]
	FreeboxMiniR1,

	[JsonStringEnumMemberName(MapperConstants.BoxModels.FreeboxMiniR2)]
	FreeboxMiniR2,

	[JsonStringEnumMemberName(MapperConstants.BoxModels.FreeboxOneR1)]
	FreeboxOneR1,

	[JsonStringEnumMemberName(MapperConstants.BoxModels.FreeboxOneR2)]
	FreeboxOneR2,

	[JsonStringEnumMemberName(MapperConstants.BoxModels.FreeboxV7R1)]
	FreeboxV7R1,

	[JsonStringEnumMemberName(MapperConstants.BoxModels.FreeboxV8R1)]
	FreeboxV8R1,

	[JsonStringEnumMemberName(MapperConstants.BoxModels.FreeboxV9R1)]
	FreeboxV9R1
}
