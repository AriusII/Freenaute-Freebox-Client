namespace Freenaute.Freebox.Client.Domains.Services;

public interface IFreeboxServicesApi
{
	FreeboxCallApi Call { get; }
	FreeboxContactsApi Contacts { get; }
	FreeboxFtpApi Ftp { get; }
	FreeboxTftpApi Tftp { get; }
	FreeboxNetworkShareApi NetworkShare { get; }
	FreeboxUPnpAvApi UPnpAv { get; }
	FreeboxPlayerApi Player { get; }
}
