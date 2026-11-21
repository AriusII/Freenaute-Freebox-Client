namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiApStatus.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiApStatus
{
	/// <summary>
	///     state Description scanning Ap is probing wifi channels no_param Ap is not configured bad_param Ap has an
	///     invalid configuration disabled Ap is permanently disabled disabled_planning Ap is currently disabled according to
	///     planning disabled_power_saving Ap is currently disabled according to power save disabled_temp Ap is currently
	///     disabled temporarily no_active_bss Ap has no active BSS starting Ap is starting starting Ap is stopping acs Ap is
	///     selecting the best available channel ht_scan Ap is scanning for other access point dfs Ap is performing dynamic
	///     frequency selection active Ap is active failed Ap has failed to start
	/// </summary>
	[JsonPropertyName("state")]
	public string? State { get; init; }

	/// <summary>effective channel width (in MHz)</summary>
	[JsonPropertyName("channel_width")]
	public StringOrInteger? ChannelWidth { get; init; }

	/// <summary>effective primary channel</summary>
	[JsonPropertyName("primary_channel")]
	public long? PrimaryChannel { get; init; }

	/// <summary>effective secondary channel</summary>
	[JsonPropertyName("secondary_channel")]
	public long? SecondaryChannel { get; init; }

	/// <summary>time left in dfs state</summary>
	[JsonPropertyName("dfs_cac_remaining_time")]
	public long? DfsCacRemainingTime { get; init; }

	/// <summary>
	///     Indicates if DFS channels are unavailable regardless of how the WifiApConfig is configured for this phy. This
	///     is enabled when your freebox is in compatibility mode for other Freebox wifi products.
	/// </summary>
	[JsonPropertyName("dfs_disabled")]
	public bool? DfsDisabled { get; init; }

	/// <summary>Optional remaining time this access point is temporarily disabled.</summary>
	[JsonPropertyName("temp_disable_remaining_time")]
	public long? TempDisableRemainingTime { get; init; }
}
