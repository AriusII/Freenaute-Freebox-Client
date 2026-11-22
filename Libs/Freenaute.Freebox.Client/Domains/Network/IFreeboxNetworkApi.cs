namespace Freenaute.Freebox.Client.Domains.Network;

public interface IFreeboxNetworkApi
{
	FreeboxConnectionApi Connection { get; }
	FreeboxDhcpApi Dhcp { get; }
	FreeboxDhcpV6Api DhcpV6 { get; }
	FreeboxLanApi Lan { get; }
	FreeboxNatApi Nat { get; }
	FreeboxIgdApi Igd { get; }
	FreeboxFreeplugApi Freeplug { get; }
	FreeboxStorageApi Storage { get; }
	FreeboxSfpApi Sfp { get; }
	FreeboxSwitchApi Switch { get; }
	FreeboxVpnApi Vpn { get; }
	FreeboxVpnClientApi VpnClient { get; }
	FreeboxWifiApi Wifi { get; }
}
