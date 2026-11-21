namespace Freenaute.Freebox.Client.Domains.Files.Api;

public sealed class FreeboxFilesApi : IFreeboxFilesApi
{
	public FreeboxFilesApi(IFreeboxTransport transport, IFreeboxBinaryTransport binaryTransport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		ArgumentNullException.ThrowIfNull(binaryTransport);
		Downloads = new FreeboxDownloadsApi(transport);
		FileSystem = new FreeboxFileSystemApi(transport, binaryTransport);
		Shares = new FreeboxSharesApi(transport);
		Uploads = new FreeboxFileUploadsApi(transport);
	}

	public FreeboxDownloadsApi Downloads { get; }
	public FreeboxFileSystemApi FileSystem { get; }
	public FreeboxSharesApi Shares { get; }
	public FreeboxFileUploadsApi Uploads { get; }
}
