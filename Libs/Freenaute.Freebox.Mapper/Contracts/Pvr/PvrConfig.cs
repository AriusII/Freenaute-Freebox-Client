namespace Freenaute.Freebox.Mapper.Contracts.Pvr;

/// <summary>PVR configuration. This API is unstable and may change.</summary>
public sealed class PvrConfig
{
	[JsonPropertyName("margin_before")] public long? MarginBefore { get; init; }
	[JsonPropertyName("margin_after")] public long? MarginAfter { get; init; }
}
