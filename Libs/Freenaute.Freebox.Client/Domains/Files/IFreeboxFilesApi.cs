namespace Freenaute.Freebox.Client.Domains.Files;

/// <summary>Files, downloads, storage and VM HTTP resources using one shared authenticated transport.</summary>
public interface IFreeboxFilesApi
{
    FreeboxDownloadsApi Downloads { get; }
    FreeboxFileSystemApi FileSystem { get; }
    FreeboxSharesApi Shares { get; }
    FreeboxUploadsApi Uploads { get; }
    FreeboxStorageApi Storage { get; }
    FreeboxRaidApi Raid { get; }
    FreeboxVirtualMachinesApi VirtualMachines { get; }
    FreeboxStatisticsApi Statistics { get; }
}
