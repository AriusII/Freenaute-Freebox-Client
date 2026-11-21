namespace Freenaute.Freebox.Client.Domains.Files;

public sealed class FreeboxFilesApi : IFreeboxFilesApi
{
    public FreeboxFilesApi(IFreeboxTransport transport, IFreeboxBinaryTransport binaryTransport)
    {
        ArgumentNullException.ThrowIfNull(transport);
        ArgumentNullException.ThrowIfNull(binaryTransport);
        Downloads = new FreeboxDownloadsApi(transport);
        FileSystem = new FreeboxFileSystemApi(transport, binaryTransport);
        Shares = new FreeboxSharesApi(transport);
        Uploads = new FreeboxUploadsApi(transport);
        Storage = new FreeboxStorageApi(transport);
        Raid = new FreeboxRaidApi(transport);
        VirtualMachines = new FreeboxVirtualMachinesApi(transport);
        Statistics = new FreeboxStatisticsApi(transport);
    }

    public FreeboxDownloadsApi Downloads { get; }
    public FreeboxFileSystemApi FileSystem { get; }
    public FreeboxSharesApi Shares { get; }
    public FreeboxUploadsApi Uploads { get; }
    public FreeboxStorageApi Storage { get; }
    public FreeboxRaidApi Raid { get; }
    public FreeboxVirtualMachinesApi VirtualMachines { get; }
    public FreeboxStatisticsApi Statistics { get; }
}
