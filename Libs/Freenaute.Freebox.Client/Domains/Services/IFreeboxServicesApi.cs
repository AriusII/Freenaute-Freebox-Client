namespace Freenaute.Freebox.Client.Domains.Services;

public interface IFreeboxServicesApi
{
    FreeboxCallApi Call { get; }
    FreeboxContactsApi Contacts { get; }
    FreeboxFtpApi Ftp { get; }
    FreeboxTftpApi Tftp { get; }
    FreeboxNetworkShareApi NetworkShare { get; }
    FreeboxUPnpAvApi UPnpAv { get; }
    FreeboxVpnApi Vpn { get; }
    FreeboxVpnClientApi VpnClient { get; }
    FreeboxPlayerApi Player { get; }
    FreeboxPvrApi Pvr { get; }
}
