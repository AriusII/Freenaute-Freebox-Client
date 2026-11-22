namespace Freenaute.Freebox.Client.Domains.Files.Models;

/// <summary>An immutable download submission; I/O starts only with AddAsync.</summary>
public sealed class FreeboxDownloadSubmission
{
	private readonly FreeboxDownloadsApi _api;
	private readonly DownloadUrlRequest _request;

	internal FreeboxDownloadSubmission(FreeboxDownloadsApi api, DownloadUrlRequest request)
	{
		_api = api;
		_request = request;
	}

	public FreeboxDownloadSubmission ToDirectory(EncodedFreeboxPath directory) =>
		new(_api, _request with { DownloadDirectory = directory });

	public FreeboxDownloadSubmission WithFileName(string fileName) => new(_api, _request with { FileName = fileName });

	public FreeboxDownloadSubmission WithHash(string hash) => new(_api, _request with { Hash = hash });

	public FreeboxDownloadSubmission Recursively(bool recursive = true) =>
		new(_api, _request with { Recursive = recursive });

	public FreeboxDownloadSubmission WithCredentials(string username, string password) =>
		new(_api, _request with { Username = username, Password = password });

	public FreeboxDownloadSubmission WithArchivePassword(string password) =>
		new(_api, _request with { ArchivePassword = password });

	public FreeboxDownloadSubmission WithCookies(string cookies) => new(_api, _request with { Cookies = cookies });

	public async Task<AddedDownloads> AddAsync(CancellationToken cancellationToken = default) =>
		await _api.AddUrlAsync(_request, cancellationToken).ConfigureAwait(false);
}
