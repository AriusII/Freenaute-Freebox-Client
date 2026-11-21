namespace Freenaute.Freebox.Mapper.Contracts.Network.Lan;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#get--api-v8-lan-browser-interfaces-.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LanBrowserInterface
{
	[JsonPropertyName("name")] public string? Name { get; init; }

	[JsonPropertyName("host_count")] public long? HostCount { get; init; }
}
