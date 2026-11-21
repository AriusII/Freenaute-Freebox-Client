namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#SwitchPortStatus.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class SwitchPortStatus
{
	/// <summary>switch port id</summary>
	[JsonPropertyName("id")]
	public long? Id { get; init; }

	/// <summary>link Description up port is up down port is down</summary>
	[JsonPropertyName("link")]
	public string? Link { get; init; }

	/// <summary>duplex Description half force in half duplex mode full force in full duplex mode</summary>
	[JsonPropertyName("duplex")]
	public string? Duplex { get; init; }

	/// <summary>duplex Description 10 10Base-T 100 100Base-TX 1000 1000Base-T</summary>
	[JsonPropertyName("speed")]
	public string? Speed { get; init; }

	/// <summary>display form of speed and duplex mode</summary>
	[JsonPropertyName("mode")]
	public string? Mode { get; init; }

	/// <summary>list of { mac, name } of hosts connected to this port</summary>
	[JsonPropertyName("mac_list")]
	public SwitchMacEntry[]? MacList { get; init; }
}
