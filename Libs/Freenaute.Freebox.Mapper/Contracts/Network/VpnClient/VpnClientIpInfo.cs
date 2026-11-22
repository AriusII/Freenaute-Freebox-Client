namespace Freenaute.Freebox.Mapper.Contracts.Network.VpnClient;

/// <summary>VPN client IP information. This API is unstable and may change.</summary>
public sealed class VpnClientIpInfo
{
	[JsonPropertyName("config_valid")] public bool? ConfigValid { get; init; }
	[JsonPropertyName("ip_mask")] public JsonElement IpMask { get; init; }
	[JsonPropertyName("domain")] public string? Domain { get; init; }
	[JsonPropertyName("gateway")] public string? Gateway { get; init; }
	[JsonPropertyName("dns")] public string[]? Dns { get; init; }
	[JsonPropertyName("provider")] public string? Provider { get; init; }
	[JsonPropertyName("routes")] public JsonElement Routes { get; init; }
	[JsonPropertyName("dhcp")] public JsonElement Dhcp { get; init; }
}
