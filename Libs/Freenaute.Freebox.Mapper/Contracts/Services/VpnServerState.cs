namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for VPNServer.state; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNServer.state" />
[JsonConverter(typeof(VpnServerStateJsonConverter))]
public readonly record struct VpnServerState(string Value)
{
    public static VpnServerState Stopped => new("stopped");
    public static VpnServerState Starting => new("starting");
    public static VpnServerState Started => new("started");
    public static VpnServerState Stopping => new("stopping");
    public static VpnServerState Error => new("error");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
