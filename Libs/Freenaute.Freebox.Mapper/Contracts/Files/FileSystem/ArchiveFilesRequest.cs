namespace Freenaute.Freebox.Mapper.Contracts.Files.FileSystem;

/// <summary>
///     Write contract from http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-archive-. Unset patch fields
///     are omitted; null clearing is not promised.
/// </summary>
public sealed record ArchiveFilesRequest
{
	[JsonPropertyName("files")] public required EncodedFreeboxPath[] Files { get; init; }

	[JsonPropertyName("dst")] public required EncodedFreeboxPath Dst { get; init; }
}
