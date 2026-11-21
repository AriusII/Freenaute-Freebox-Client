namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-diag.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiDiagnosticsResult
{
	[JsonPropertyName("aps")] public WifiDiagItem[]? Aps { get; init; }

	[JsonPropertyName("bsss")] public WifiDiagItem[]? Bsss { get; init; }
}
