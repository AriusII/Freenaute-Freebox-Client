namespace Freenaute.Freebox.Mapper.Contracts.Network.Xdsl;

/// <summary>xDSL status and statistics. This API is unstable and may change.</summary>
public sealed class XdslInfos
{
	[JsonPropertyName("status")] public XdslStatus? Status { get; init; }
	[JsonPropertyName("down")] public XdslStats? Down { get; init; }
	[JsonPropertyName("up")] public XdslStats? Up { get; init; }
}
