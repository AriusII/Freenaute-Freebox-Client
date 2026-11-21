namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiCustomKey.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiCustomKey
{
    /// <summary>custom key id</summary>
    [JsonPropertyName("id")]
    public long? Id { get; init; }

    /// <summary>time remaining before the access (seconds) if 0 then it does not expire</summary>
    [JsonPropertyName("remaining")]
    public long? Remaining { get; init; }

    /// <summary>custom key parameters</summary>
    [JsonPropertyName("params")]
    public WifiCustomKeyParams? Params { get; init; }

    /// <summary>list of hosts that used the custom key</summary>
    [JsonPropertyName("users")]
    public WifiCustomKeyHost[]? Users { get; init; }
}
