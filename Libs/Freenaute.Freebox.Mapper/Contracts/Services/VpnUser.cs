namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNUser wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNUser" />
public sealed record VpnUser
{
    [JsonPropertyName("login")] public string? Login { get; init; }

    [JsonPropertyName("type")] public VpnUserType? Type { get; init; }

    [JsonPropertyName("password_set")] public bool? PasswordSet { get; init; }

    [JsonPropertyName("ip_reservation")] public string? IpReservation { get; init; }

    [JsonPropertyName("conf_wireguard")] public VpnUserWireGuardConfig? ConfWireGuard { get; init; }

    public override string ToString()
    {
        return nameof(VpnUser);
    }
}
