namespace Freenaute.Freebox.Mapper.Contracts.Network.Lte;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LteRadioBand.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LteRadioBand
{
	[JsonPropertyName("enabled")] public bool? Enabled { get; init; }

	[JsonPropertyName("bandwidth")] public long? Bandwidth { get; init; }

	[JsonPropertyName("rsrq")] public long? Rsrq { get; init; }

	[JsonPropertyName("rsrp")] public long? Rsrp { get; init; }

	[JsonPropertyName("rssi")] public long? Rssi { get; init; }

	[JsonPropertyName("band")] public long? Band { get; init; }

	[JsonPropertyName("pci")] public long? Pci { get; init; }
}
