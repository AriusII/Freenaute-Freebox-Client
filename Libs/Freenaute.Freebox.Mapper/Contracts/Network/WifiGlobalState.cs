namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiGlobalState.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiGlobalState
{
	/// <summary>
	///     wifi global state state Description enabled Wifi is enabled disabled Wi-Fi is disabled disabled_planning Wi-Fi
	///     is disabled by planning
	/// </summary>
	[JsonPropertyName("state")]
	public string? State { get; init; }

	/// <summary>expected wifi cards</summary>
	[JsonPropertyName("expected_phys")]
	public ExpectedPhy[]? ExpectedPhys { get; init; }
}
