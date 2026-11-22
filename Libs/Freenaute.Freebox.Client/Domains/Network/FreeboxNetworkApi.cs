namespace Freenaute.Freebox.Client.Domains.Network;

/// <summary>Freebox Server network domains. Every facade shares the supplied transport and its box-scoped session.</summary>
internal sealed class FreeboxNetworkApi : IFreeboxNetworkApi
{
	public FreeboxNetworkApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		Connection = new FreeboxConnectionApi(transport);
		Dhcp = new FreeboxDhcpApi(transport);
		DhcpV6 = new FreeboxDhcpV6Api(transport);
		Lan = new FreeboxLanApi(transport);
		Nat = new FreeboxNatApi(transport);
		Igd = new FreeboxIgdApi(transport);
		Freeplug = new FreeboxFreeplugApi(transport);
		Storage = new FreeboxStorageApi(transport);
		Sfp = new FreeboxSfpApi(transport);
		Switch = new FreeboxSwitchApi(transport);
		Vpn = new FreeboxVpnApi(transport);
		VpnClient = new FreeboxVpnClientApi(transport);
		Wifi = new FreeboxWifiApi(transport);
	}

	public FreeboxConnectionApi Connection { get; }
	public FreeboxDhcpApi Dhcp { get; }
	public FreeboxDhcpV6Api DhcpV6 { get; }
	public FreeboxLanApi Lan { get; }
	public FreeboxNatApi Nat { get; }
	public FreeboxIgdApi Igd { get; }
	public FreeboxFreeplugApi Freeplug { get; }
	public FreeboxStorageApi Storage { get; }
	public FreeboxSfpApi Sfp { get; }
	public FreeboxSwitchApi Switch { get; }
	public FreeboxVpnApi Vpn { get; }
	public FreeboxVpnClientApi VpnClient { get; }
	public FreeboxWifiApi Wifi { get; }
}
