namespace Freenaute.Freebox.Mapper.Contracts.Network.Common;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#TemporaryWifiDisable.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class TemporaryWifiDisable
{
	/// <summary>remaining seconds the wifi is temporarily disabled. Set to 0 to stop the temporary wifi disabling period.</summary>
	[JsonPropertyName("remaining")]
	public long? Remaining { get; init; }
}
