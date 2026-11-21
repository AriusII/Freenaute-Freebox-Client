namespace Freenaute.Freebox.Client.Domains.Files;

/// <summary>An immutable download submission; I/O starts only with AddAsync.</summary>
public sealed class FreeboxDownloadSubmission
{
    private readonly FreeboxDownloadsApi api;
    private readonly DownloadUrlRequest request;

    internal FreeboxDownloadSubmission(FreeboxDownloadsApi api, DownloadUrlRequest request)
    {
        this.api = api;
        this.request = request;
    }

    public FreeboxDownloadSubmission ToDirectory(EncodedFreeboxPath directory)
    {
        return new FreeboxDownloadSubmission(api, request with { DownloadDirectory = directory });
    }

    public FreeboxDownloadSubmission WithFileName(string fileName)
    {
        return new FreeboxDownloadSubmission(api, request with { FileName = fileName });
    }

    public FreeboxDownloadSubmission WithHash(string hash)
    {
        return new FreeboxDownloadSubmission(api, request with { Hash = hash });
    }

    public FreeboxDownloadSubmission Recursively(bool recursive = true)
    {
        return new FreeboxDownloadSubmission(api, request with { Recursive = recursive });
    }

    public FreeboxDownloadSubmission WithCredentials(string username, string password)
    {
        return new FreeboxDownloadSubmission(api, request with { Username = username, Password = password });
    }

    public FreeboxDownloadSubmission WithArchivePassword(string password)
    {
        return new FreeboxDownloadSubmission(api, request with { ArchivePassword = password });
    }

    public FreeboxDownloadSubmission WithCookies(string cookies)
    {
        return new FreeboxDownloadSubmission(api, request with { Cookies = cookies });
    }

    public Task<AddedDownloads> AddAsync(CancellationToken cancellationToken = default)
    {
        return api.AddUrlAsync(request, cancellationToken);
    }
}
