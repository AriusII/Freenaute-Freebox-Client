namespace Freenaute.Freebox.Mapper.Contracts.Files.Downloads;

/// <summary>Torrent piece-state string. This API is unstable and may change.</summary>
public sealed class DownloadPiecesResult
{
	[JsonPropertyName("result")] public string? Result { get; init; }
}
