namespace Freenaute.Freebox.Mapper.Contracts.Network.Vpn;

/// <summary>Active VPN connection. This API is unstable and may change.</summary>
public sealed class VPNConnection
{
	[JsonPropertyName("id")] public string? Id { get; init; }
	[JsonPropertyName("vpn")] public string? Vpn { get; init; }
	[JsonPropertyName("user")] public string? User { get; init; }
	[JsonPropertyName("authenticated")] public bool? Authenticated { get; init; }
	[JsonPropertyName("auth_time")] public long? AuthTime { get; init; }
	[JsonPropertyName("src_ip")] public string? SourceIp { get; init; }
	[JsonPropertyName("src_port")] public long? SourcePort { get; init; }
	[JsonPropertyName("local_ip")] public long? LocalIp { get; init; }
	[JsonPropertyName("rx_bytes")] public long? ReceiveBytes { get; init; }
	[JsonPropertyName("tx_bytes")] public long? TransmitBytes { get; init; }
}
