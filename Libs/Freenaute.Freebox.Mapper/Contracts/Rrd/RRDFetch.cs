namespace Freenaute.Freebox.Mapper.Contracts.Rrd;

/// <summary>RRD statistics query. This API is unstable and may change.</summary>
public sealed class RRDFetch
{
	[JsonPropertyName("db")] public required string Db { get; init; }
	[JsonPropertyName("date_start")] public long? DateStart { get; init; }
	[JsonPropertyName("date_end")] public long? DateEnd { get; init; }
	[JsonPropertyName("precision")] public long? Precision { get; init; }
	[JsonPropertyName("fields")] public string[]? Fields { get; init; }
}
