namespace Freenaute.Freebox.Mapper.Contracts.Network.Vpn;

/// <summary>OpenVPN server settings. This API is unstable and may change.</summary>
public sealed class VPNOpenVpnConfig
{
	[JsonPropertyName("cipher")] public string? Cipher { get; init; }
	[JsonPropertyName("disable_fragment")] public bool? DisableFragment { get; init; }
	[JsonPropertyName("use_tcp")] public bool? UseTcp { get; init; }
}
