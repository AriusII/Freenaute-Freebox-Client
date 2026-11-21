namespace Freenaute.Freebox.Mapper.Contracts.Network.Xdsl;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#SfpStatus.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class SfpStatus
{
	/// <summary>Indicates whether an SFP module present in the port</summary>
	[JsonPropertyName("present")]
	public bool? Present { get; init; }

	/// <summary>Indicates whether the SFP module has a valid EEPROM</summary>
	[JsonPropertyName("eeprom_valid")]
	public bool? EepromValid { get; init; }

	/// <summary>Indicates whether the SFP module is supported</summary>
	[JsonPropertyName("supported")]
	public bool? Supported { get; init; }

	/// <summary>SFP type read from EEPROM</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>SFP port is powered</summary>
	[JsonPropertyName("power_good")]
	public bool? PowerGood { get; init; }

	/// <summary>link status</summary>
	[JsonPropertyName("link")]
	public bool? Link { get; init; }

	/// <summary>vendor name</summary>
	[JsonPropertyName("vendor_name")]
	public string? VendorName { get; init; }

	/// <summary>part number</summary>
	[JsonPropertyName("part_number")]
	public string? PartNumber { get; init; }

	/// <summary>hardware revision</summary>
	[JsonPropertyName("hardware_rev")]
	public string? HardwareRev { get; init; }

	/// <summary>serial number</summary>
	[JsonPropertyName("serial_number")]
	public string? SerialNumber { get; init; }
}
