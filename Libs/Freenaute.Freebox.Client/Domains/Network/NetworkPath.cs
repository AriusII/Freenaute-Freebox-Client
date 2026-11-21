namespace Freenaute.Freebox.Client.Domains.Network;

internal static class NetworkPath
{
    internal static string Number(long value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
}
