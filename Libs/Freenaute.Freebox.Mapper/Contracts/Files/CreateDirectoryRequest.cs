namespace Freenaute.Freebox.Mapper.Contracts.Files;

/// <summary>
///     Write contract from http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-mkdir-. Unset patch fields are
///     omitted; null clearing is not promised.
/// </summary>
public sealed record CreateDirectoryRequest
{
    [JsonPropertyName("parent")] public required EncodedFreeboxPath Parent { get; init; }

    [JsonPropertyName("dirname")] public required string Dirname { get; init; }
}
