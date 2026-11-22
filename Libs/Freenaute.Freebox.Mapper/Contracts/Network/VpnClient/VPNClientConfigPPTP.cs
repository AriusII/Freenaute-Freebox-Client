namespace Freenaute.Freebox.Mapper.Contracts.Network.VpnClient;

/// <summary>PPTP VPN client settings. This API is unstable and may change.</summary>
public sealed class VPNClientConfigPPTP
{
	[JsonPropertyName("remote_host")] public string? RemoteHost { get; init; }
	[JsonPropertyName("username")] public string? Username { get; init; }
	[JsonPropertyName("password")] public string? Password { get; init; }
	[JsonPropertyName("mppe")] public string? Mppe { get; init; }
	[JsonPropertyName("allowed_auth")] public Dictionary<string, bool>? AllowedAuthentication { get; init; }
}
