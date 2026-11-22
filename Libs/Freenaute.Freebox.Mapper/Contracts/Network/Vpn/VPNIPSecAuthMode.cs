namespace Freenaute.Freebox.Mapper.Contracts.Network.Vpn;

/// <summary>IPSec authentication mode. This API is unstable and may change.</summary>
public sealed class VPNIPSecAuthMode
{
	[JsonPropertyName("id_source")] public string? IdSource { get; init; }
	[JsonPropertyName("id_custom")] public string? IdCustom { get; init; }
}
