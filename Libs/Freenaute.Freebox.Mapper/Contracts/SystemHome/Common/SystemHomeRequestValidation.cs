namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Common;

internal static class SystemHomeRequestValidation
{
	internal static void NoNull<T>(Optional<T> value, string name)
	{
		if (value.IsNull)
		{
			throw new ArgumentException("Explicit null is not documented for this field.", name);
		}
	}

	internal static void Array<T>(Optional<ImmutableArray<T>> value, string name)
	{
		NoNull(value, name);
		if (value.HasValue && value.Value.IsDefault)
		{
			throw new ArgumentException("An array must be initialized.", name);
		}
	}
}
