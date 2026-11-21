namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#XdslStatus.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class XdslStatus
{
	/// <summary>
	///     Status Description down unsynchronized training synchronizing step 1/4 started synchronizing step 2/4
	///     chan_analysis synchronizing step 3/4 msg_exchange synchronizing step 4/4 showtime Ready disabled Disabled
	/// </summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>
	///     Protocol Description t1413 T1.413 adsl1_a ADSL adsl2_a ADSL2 adsl2plus_a ADSL2+ readsl2 ReachDSL adsl2_m ADSL2
	///     annex M adsl2plus_m ADSL2+ annex M unknown Unknown
	/// </summary>
	[JsonPropertyName("protocol")]
	public string? Protocol { get; init; }

	/// <summary>Protocol Description adsl ADSL vdsl VDSL</summary>
	[JsonPropertyName("modulation")]
	public string? Modulation { get; init; }

	/// <summary>uptime in seconds</summary>
	[JsonPropertyName("uptime")]
	public long? Uptime { get; init; }
}
