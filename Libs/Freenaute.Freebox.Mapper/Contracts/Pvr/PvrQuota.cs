namespace Freenaute.Freebox.Mapper.Contracts.Pvr;

/// <summary>PVR quota status. This API is unstable and may change.</summary>
public sealed class PvrQuota
{
	[JsonPropertyName("quota_exceeded")] public bool? QuotaExceeded { get; init; }
	[JsonPropertyName("needed_tresh")] public long? NeededThreshold { get; init; }
	[JsonPropertyName("cur_tresh")] public long? CurrentThreshold { get; init; }
}
