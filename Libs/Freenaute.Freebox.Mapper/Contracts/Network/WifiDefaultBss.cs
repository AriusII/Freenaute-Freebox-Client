namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-default.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiDefaultBss
{
	[JsonPropertyName("bssid")] public string? Bssid { get; init; }

	[JsonPropertyName("params")] public WifiBssConfig? Params { get; init; }
}
