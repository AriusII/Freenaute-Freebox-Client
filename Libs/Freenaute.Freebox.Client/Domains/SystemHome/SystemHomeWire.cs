namespace Freenaute.Freebox.Client.Domains.SystemHome;

internal static class SystemHomeWire
{
    internal static SystemHomeJsonSerializerContext Json => SystemHomeJsonSerializerContext.Default;

    internal static string Id(long value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
}
