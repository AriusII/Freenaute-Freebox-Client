namespace Freenaute.Freebox.Mapper.Adapter;

/// <summary>Explicit extension helpers for wire conversion (no reflection).</summary>
public static class FreeboxMapperExtensions
{
	public static string ToWire<T>(this T value, JsonTypeInfo<T> typeInfo) =>
		FreeboxMapper.Default.ToWire(value, typeInfo);

	public static byte[] ToUtf8Bytes<T>(this T value, JsonTypeInfo<T> typeInfo) =>
		FreeboxMapper.Default.ToUtf8Bytes(value, typeInfo);

	public static T? FromWire<T>(this string json, JsonTypeInfo<T> typeInfo) =>
		FreeboxMapper.Default.FromWire(json, typeInfo);

	public static T? FromWire<T>(this ReadOnlySpan<byte> json, JsonTypeInfo<T> typeInfo) =>
		FreeboxMapper.Default.FromWire(json, typeInfo);
}
