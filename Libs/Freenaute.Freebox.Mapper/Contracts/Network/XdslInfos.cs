namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#XdslInfos.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class XdslInfos
{
    [JsonPropertyName("status")] public XdslStatus? Status { get; init; }

    [JsonPropertyName("down")] public XdslStats? Down { get; init; }

    [JsonPropertyName("up")] public XdslStats? Up { get; init; }
}
