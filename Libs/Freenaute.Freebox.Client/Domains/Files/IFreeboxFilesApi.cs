namespace Freenaute.Freebox.Client.Domains.Files;

/// <summary>Files and download HTTP resources using one shared authenticated transport.</summary>
public interface IFreeboxFilesApi
{
	FreeboxDownloadsApi Downloads { get; }
	FreeboxFileSystemApi FileSystem { get; }
	FreeboxSharesApi Shares { get; }

	[Obsolete("Deprecated by Freebox API documentation. Prefer WebSocket upload via WebSockets.Uploads (/ws/upload).")]
	FreeboxFileUploadsApi Uploads { get; }
}
