namespace Freenaute.Freebox.Mapper.Contracts.Services.Players;

/// <summary>Read projection of the documented Player wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#Player" />
public sealed record Player
{
	[JsonPropertyName("id")] public long? Id { get; init; }

	[JsonPropertyName("device_name")] public string? DeviceName { get; init; }

	[JsonPropertyName("uid")] public string? Uid { get; init; }

	[JsonPropertyName("reachable")] public bool? Reachable { get; init; }

	[JsonPropertyName("api_version")] public string? ApiVersion { get; init; }

	[JsonPropertyName("api_available")] public bool? ApiAvailable { get; init; }

	[JsonPropertyName("stb_type")] public string? StbType { get; init; }

	public override string ToString() => nameof(Player);
}
