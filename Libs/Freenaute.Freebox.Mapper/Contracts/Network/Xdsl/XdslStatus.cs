namespace Freenaute.Freebox.Mapper.Contracts.Network.Xdsl;

/// <summary>xDSL link status. This API is unstable and may change.</summary>
public sealed class XdslStatus
{
	[JsonPropertyName("status")] public string? Status { get; init; }
	[JsonPropertyName("protocol")] public string? Protocol { get; init; }
	[JsonPropertyName("modulation")] public string? Modulation { get; init; }
	[JsonPropertyName("uptime")] public long? Uptime { get; init; }
}
