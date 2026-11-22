namespace Freenaute.Freebox.Client.Domains.Files.Models;

/// <summary>An immutable selector retaining the exact encoded path returned by Freebox.</summary>
public sealed class FreeboxFileResource
{
	private readonly FreeboxFileSystemApi _api;
	private readonly EncodedFreeboxPath _path;

	internal FreeboxFileResource(FreeboxFileSystemApi api, EncodedFreeboxPath path)
	{
		_ = path.Value;
		_api = api;
		_path = path;
	}

	public async Task<FileEntry> GetInformationAsync(CancellationToken cancellationToken = default) =>
		await _api.GetInformationAsync(_path, cancellationToken).ConfigureAwait(false);

	public async Task<FileListing> ListAsync(FileListingOptions? options = null,
		CancellationToken cancellationToken = default) =>
		await _api.ListAsync(_path, options, cancellationToken).ConfigureAwait(false);

	public async Task<FreeboxDownload> DownloadAsync(CancellationToken cancellationToken = default) =>
		await _api.DownloadAsync(_path, cancellationToken).ConfigureAwait(false);
}
