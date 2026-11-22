namespace Freenaute.Freebox.Mapper.Contracts.Rrd;

/// <summary>RRD statistics result. This API is unstable and may change.</summary>
public sealed class RRDStats
{
	[JsonPropertyName("date_start")] public long? DateStart { get; init; }
	[JsonPropertyName("date_end")] public long? DateEnd { get; init; }
	[JsonPropertyName("data")] public JsonElement[]? Data { get; init; }
}
