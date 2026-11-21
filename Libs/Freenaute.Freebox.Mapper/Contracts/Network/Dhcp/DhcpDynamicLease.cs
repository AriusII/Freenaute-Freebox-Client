namespace Freenaute.Freebox.Mapper.Contracts.Network.Dhcp;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#DhcpDynamicLease.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class DhcpDynamicLease
{
	/// <summary>Host mac address</summary>
	[JsonPropertyName("mac")]
	public string? Mac { get; init; }

	/// <summary>hostname matching the mac address</summary>
	[JsonPropertyName("hostname")]
	public string? Hostname { get; init; }

	/// <summary>IPv4 assigned to the host</summary>
	[JsonPropertyName("ip")]
	public string? Ip { get; init; }

	/// <summary>time left before lease needs to be refreshed</summary>
	[JsonPropertyName("lease_remaining")]
	public long? LeaseRemaining { get; init; }

	/// <summary>timestamp of the lease first assignment</summary>
	[JsonPropertyName("assign_time")]
	public long? AssignTime { get; init; }

	/// <summary>timestamp of the last lease refresh</summary>
	[JsonPropertyName("refresh_time")]
	public long? RefreshTime { get; init; }

	/// <summary>is the lease static</summary>
	[JsonPropertyName("is_static")]
	public bool? IsStatic { get; init; }

	/// <summary>LAN host information from LAN browser (refer to LanHost documentation)</summary>
	[JsonPropertyName("host")]
	public LanHost? Host { get; init; }

	[JsonPropertyName("options")] public DhcpOption[]? Options { get; init; }
}
