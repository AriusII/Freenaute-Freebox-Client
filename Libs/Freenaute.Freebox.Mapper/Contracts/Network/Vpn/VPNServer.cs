namespace Freenaute.Freebox.Mapper.Contracts.Network.Vpn;

/// <summary>VPN server status. This API is unstable and may change.</summary>
public sealed class VPNServer
{
	[JsonPropertyName("name")] public string? Name { get; init; }
	[JsonPropertyName("type")] public string? Type { get; init; }
	[JsonPropertyName("state")] public string? State { get; init; }
	[JsonPropertyName("connection_count")] public long? ConnectionCount { get; init; }

	[JsonPropertyName("auth_connection_count")]
	public long? AuthConnectionCount { get; init; }
}
