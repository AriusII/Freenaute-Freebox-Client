namespace Freenaute.Freebox.Mapper.Contracts.Network.Vpn;

/// <summary>A reserved VPN IP address. This API is unstable and may change.</summary>
public sealed class VpnIpReservation
{
	[JsonPropertyName("login")] public string? Login { get; init; }
	[JsonPropertyName("ip")] public string? Ip { get; init; }
}
