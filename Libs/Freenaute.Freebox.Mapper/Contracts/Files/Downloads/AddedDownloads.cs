namespace Freenaute.Freebox.Mapper.Contracts.Files.Downloads;

/// <summary>downloads/add result: id is one integer or an array of integers; the original shape is preserved.</summary>
[JsonConverter(typeof(AddedDownloadsJsonConverter))]
public sealed record AddedDownloads(IReadOnlyList<long> Ids, bool WasMultiple);
