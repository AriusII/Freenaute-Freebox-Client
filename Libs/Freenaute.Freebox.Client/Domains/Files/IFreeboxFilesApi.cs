namespace Freenaute.Freebox.Client.Domains.Files;

/// <summary>Files and download HTTP resources using one shared authenticated transport.</summary>
public interface IFreeboxFilesApi
{
	FreeboxDownloadsApi Downloads { get; }
	FreeboxFileSystemApi FileSystem { get; }
	FreeboxSharesApi Shares { get; }
	FreeboxUploadsApi Uploads { get; }
}
