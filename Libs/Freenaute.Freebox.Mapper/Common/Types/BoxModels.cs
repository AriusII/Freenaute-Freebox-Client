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
	[JsonStringEnumMemberName("fbxgw-r1/full")]
	FreeboxServerV6R1,

	[JsonStringEnumMemberName("fbxgw-r2/full")]
	FreeboxServerV6R2,

	[JsonStringEnumMemberName("fbxgw-r1/mini")]
	FreeboxMiniR1,

	[JsonStringEnumMemberName("fbxgw-r2/mini")]
	FreeboxMiniR2,

	[JsonStringEnumMemberName("fbxgw-r1/one")]
	FreeboxOneR1,

	[JsonStringEnumMemberName("fbxgw-r2/one")]
	FreeboxOneR2,

	[JsonStringEnumMemberName("fbxgw7-r1/full")]
	FreeboxV7R1,

	[JsonStringEnumMemberName("fbxgw8-r1/full")]
	FreeboxV8R1,

	[JsonStringEnumMemberName("fbxgw9-r1/full")]
	FreeboxV9R1
}
