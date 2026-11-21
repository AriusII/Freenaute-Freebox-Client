namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiBss.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiBss
{
	/// <summary>bss id</summary>
	[JsonPropertyName("id")]
	public StringOrInteger? Id { get; init; }

	/// <summary>associated AP id</summary>
	[JsonPropertyName("phy_id")]
	public StringOrInteger? PhyId { get; init; }

	/// <summary>bss status</summary>
	[JsonPropertyName("status")]
	public WifiBssStatus? Status { get; init; }

	/// <summary>
	///     if set to True the bss will use the shared parameters stored under shared_bss_params if not the bss will use a
	///     configuration specific to this bss stored under bss_params when you want to edit the bss config you should change
	///     the config values using values from bss_params or shared_bss_params as a source and update use_shared_params
	///     accordingly.
	/// </summary>
	[JsonPropertyName("use_shared_params")]
	public bool? UseSharedParams { get; init; }

	/// <summary>bss configuration (use this field for editing)</summary>
	[JsonPropertyName("config")]
	public WifiBssConfig? Config { get; init; }

	/// <summary>current configuration specific to this bss</summary>
	[JsonPropertyName("bss_params")]
	public WifiBssConfig? BssParams { get; init; }

	/// <summary>current configuration for shared bss config</summary>
	[JsonPropertyName("shared_bss_params")]
	public WifiBssConfig? SharedBssParams { get; init; }

	/// <summary>Whether or not this BSS can work with wep encryption or not</summary>
	[JsonPropertyName("disable_wep")]
	public bool? DisableWep { get; init; }
}
