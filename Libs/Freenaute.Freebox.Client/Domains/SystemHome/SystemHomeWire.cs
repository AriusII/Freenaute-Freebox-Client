namespace Freenaute.Freebox.Client.Domains.SystemHome;

internal static class SystemHomeWire
{
	internal static SystemHomeJsonSerializerContext Json => SystemHomeJsonSerializerContext.Default;

	internal static string Id(long value) => value.ToString(CultureInfo.InvariantCulture);
}
