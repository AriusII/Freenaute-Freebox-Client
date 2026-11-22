namespace Freenaute.Freebox.Mapper.Contracts.Network.Vpn;

/// <summary>VPN IP pool and reservations. This API is unstable and may change.</summary>
public sealed class VpnIpPool
{
	[JsonPropertyName("ip_start")] public string? IpStart { get; init; }
	[JsonPropertyName("ip_end")] public string? IpEnd { get; init; }
	[JsonPropertyName("reservations")] public VpnIpReservation[]? Reservations { get; init; }
}
