namespace Freenaute.Freebox.Mapper.Contracts.Network.Vpn;

/// <summary>VPN server user. This API is unstable and may change.</summary>
public sealed class VPNUser
{
	[JsonPropertyName("login")] public string? Login { get; init; }
	[JsonPropertyName("password")] public string? Password { get; init; }
	[JsonPropertyName("password_set")] public bool? PasswordSet { get; init; }
	[JsonPropertyName("ip_reservation")] public string? IpReservation { get; init; }
}
