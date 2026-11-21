namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-wps-config-.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiWpsConfiguration
{
	[JsonPropertyName("enabled")] public bool? Enabled { get; init; }
}
