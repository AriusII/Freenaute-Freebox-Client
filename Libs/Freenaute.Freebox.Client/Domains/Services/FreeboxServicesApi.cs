namespace Freenaute.Freebox.Client.Domains.Services;

/// <summary>
///     Freebox Server services using the discovered server API major. Player proxy paths retain their documented
///     internal v6 API.
/// </summary>
public sealed class FreeboxServicesApi : IFreeboxServicesApi
{
	public FreeboxServicesApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		var binary = transport as IFreeboxBinaryTransport;
		Call = new FreeboxCallApi(transport, binary);
		Contacts = new FreeboxContactsApi(transport);
		Ftp = new FreeboxFtpApi(transport);
		Tftp = new FreeboxTftpApi(transport);
		NetworkShare = new FreeboxNetworkShareApi(transport);
		UPnpAv = new FreeboxUPnpAvApi(transport);
		Player = new FreeboxPlayerApi(transport);
	}

	public FreeboxCallApi Call { get; }
	public FreeboxContactsApi Contacts { get; }
	public FreeboxFtpApi Ftp { get; }
	public FreeboxTftpApi Tftp { get; }
	public FreeboxNetworkShareApi NetworkShare { get; }
	public FreeboxUPnpAvApi UPnpAv { get; }
	public FreeboxPlayerApi Player { get; }
}
