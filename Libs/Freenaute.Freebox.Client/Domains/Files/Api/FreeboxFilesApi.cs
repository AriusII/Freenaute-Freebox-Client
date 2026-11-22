namespace Freenaute.Freebox.Client.Domains.Files.Api;

internal sealed class FreeboxFilesApi : IFreeboxFilesApi
{
	public FreeboxFilesApi(IFreeboxTransport transport, IFreeboxBinaryTransport binaryTransport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		ArgumentNullException.ThrowIfNull(binaryTransport);
		Downloads = new FreeboxDownloadsApi(transport);
		FileSystem = new FreeboxFileSystemApi(transport, binaryTransport);
		Shares = new FreeboxSharesApi(transport);
#pragma warning disable CS0618 // Deprecated by Freebox API documentation. Exposed for backward compatibility.
		Uploads = new FreeboxFileUploadsApi(transport);
#pragma warning restore CS0618
	}

	public FreeboxDownloadsApi Downloads { get; }
	public FreeboxFileSystemApi FileSystem { get; }
	public FreeboxSharesApi Shares { get; }
#pragma warning disable CS0618 // Deprecated by Freebox API documentation. Exposed for backward compatibility.
	[Obsolete("Deprecated by Freebox API documentation. Prefer WebSocket upload via WebSockets.Uploads (/ws/upload).")]
	public FreeboxFileUploadsApi Uploads { get; }
#pragma warning restore CS0618
}
