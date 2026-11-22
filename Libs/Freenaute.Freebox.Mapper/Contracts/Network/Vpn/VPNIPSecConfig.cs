namespace Freenaute.Freebox.Mapper.Contracts.Network.Vpn;

/// <summary>IPSec server settings. This API is unstable and may change.</summary>
public sealed class VPNIPSecConfig
{
	[JsonPropertyName("ike_version")] public long? IkeVersion { get; init; }
	[JsonPropertyName("auth_modes")] public VPNIPSecAuthMode[]? AuthenticationModes { get; init; }
}
