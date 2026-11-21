namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for VPNClientStatus.last_error; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNClientStatus.last_error" />
[JsonConverter(typeof(VpnClientErrorJsonConverter))]
public readonly record struct VpnClientError(string Value)
{
    public static VpnClientError None => new("none");
    public static VpnClientError Internal => new("internal");
    public static VpnClientError AuthenticationFailed => new("authentication_failed");
    public static VpnClientError AuthFailed => new("auth_failed");
    public static VpnClientError ResolvFailed => new("resolv_failed");
    public static VpnClientError ConnectTimeout => new("connect_timeout");
    public static VpnClientError ConnectFailed => new("connect_failed");
    public static VpnClientError SetupControlFailed => new("setup_control_failed");
    public static VpnClientError SetupCallFailed => new("setup_call_failed");
    public static VpnClientError Protocol => new("protocol");
    public static VpnClientError RemoteTerminated => new("remote_terminated");
    public static VpnClientError RemoteDisconnect => new("remote_disconnect");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
