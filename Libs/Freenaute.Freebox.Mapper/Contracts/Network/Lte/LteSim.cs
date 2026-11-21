namespace Freenaute.Freebox.Mapper.Contracts.Network.Lte;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LteSim.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LteSim
{
	[JsonPropertyName("present")] public bool? Present { get; init; }

	[JsonPropertyName("pin_locked")] public bool? PinLocked { get; init; }

	[JsonPropertyName("puk_remaining")] public long? PukRemaining { get; init; }

	[JsonPropertyName("iccid")] public string? Iccid { get; init; }

	[JsonPropertyName("puk_locked")] public bool? PukLocked { get; init; }

	[JsonPropertyName("pin_remaining")] public long? PinRemaining { get; init; }
}
