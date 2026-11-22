namespace Freenaute.Freebox.Mapper.Contracts.Network.Vpn;

/// <summary>PPTP server settings. This API is unstable and may change.</summary>
public sealed class VPNPPTPConfig
{
	[JsonPropertyName("mppe")] public string? Mppe { get; init; }
	[JsonPropertyName("allowed_auth")] public Dictionary<string, bool>? AllowedAuthentication { get; init; }
}
