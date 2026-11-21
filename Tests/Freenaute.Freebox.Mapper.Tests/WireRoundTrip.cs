namespace Freenaute.Freebox.Mapper.Tests;

/// <summary>Shared byte-identical wire round-trip assertion for source-generated contracts.</summary>
internal static class WireRoundTrip
{
	public static void AssertByteIdentical<T>(string wire, JsonTypeInfo<T> typeInfo)
	{
		var value = wire.FromWire(typeInfo);
		Assert.NotNull(value);
		Assert.Equal(wire, value!.ToWire(typeInfo));
	}

	public static void AssertByteIdentical<T>(FreeboxMapper mapper, string wire)
	{
		var value = mapper.FromWire<T>(wire);
		Assert.NotNull(value);
		Assert.Equal(wire, mapper.ToWire(value!));
	}
}
