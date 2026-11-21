namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#get--api-v8-switch-status-.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class SwitchMacEntry
{
	[JsonPropertyName("mac")] public string? Mac { get; init; }

	[JsonPropertyName("name")] public string? Name { get; init; }

	[JsonPropertyName("hostname")] public string? Hostname { get; init; }
}
