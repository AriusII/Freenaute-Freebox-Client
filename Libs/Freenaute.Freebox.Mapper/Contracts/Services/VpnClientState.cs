namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for VPNClientStatus.state; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNClientStatus.state" />
[JsonConverter(typeof(VpnClientStateJsonConverter))]
public readonly record struct VpnClientState(string Value)
{
    public static VpnClientState WaitingWan => new("waiting_wan");
    public static VpnClientState GoingUp => new("going_up");
    public static VpnClientState Up => new("up");
    public static VpnClientState GoingDown => new("going_down");
    public static VpnClientState Down => new("down");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
