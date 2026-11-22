namespace Freenaute.Freebox.Client.Domains.Network.Support;

internal static class NetworkConstants
{
	internal static class Routes
	{
		internal static class Connection
		{
			internal const string Base = "connection/";
			internal const string Config = "connection/config/";
			internal const string Ipv6Config = "connection/ipv6/config/";
			internal const string Aggregation = "connection/aggregation";
			internal const string Ftth = "connection/ftth/";
			internal const string Xdsl = "connection/xdsl/";
			internal const string DdnsProviders = "connection/ddns/";
			internal const string DdnsStatusSuffix = "/status/";

			internal static string DdnsProvider(string provider) => $"{DdnsProviders}{provider}/";
			internal static string DdnsProviderStatus(string provider) => $"{DdnsProviders}{provider}{DdnsStatusSuffix}";
		}

		internal static class Dhcp
		{
			internal const string Config = "dhcp/config/";
			internal const string StaticLeases = "dhcp/static_lease/";
			internal const string DynamicLeases = "dhcp/dynamic_lease/";
			internal const string V6Config = "dhcpv6/config/";

			internal static string StaticLease(string id) => $"{StaticLeases}{id}";
		}

		internal static class Igd
		{
			internal const string Config = "upnpigd/config/";
			internal const string Redirections = "upnpigd/redir/";

			internal static string Redirection(string id) => $"{Redirections}{id}";
		}

		internal static class Lan
		{
			internal const string Config = "lan/config/";
			internal const string Routes = "lan/routes";
			internal const string RoutesCollection = "lan/routes/";
			internal const string BrowserInterfaces = "lan/browser/interfaces/";
			internal const string BrowserTypes = "lan/browser/types/";
			internal const string Browser = "lan/browser/";
			internal const string WakeOnLan = "lan/wol/";

			internal static string InterfaceHosts(string networkInterface) => $"{Browser}{networkInterface}/";
			internal static string Host(string networkInterface, string hostId) => $"{Browser}{networkInterface}/{hostId}/";
			internal static string WakeOnLanInterface(string networkInterface) => $"{WakeOnLan}{networkInterface}/";
		}

		internal static class Nat
		{
			internal const string Dmz = "fw/dmz/";
			internal const string Redirections = "fw/redir/";
			internal const string IncomingPorts = "fw/incoming/";

			internal static string Redirection(string id) => $"{Redirections}{id}";
			internal static string IncomingPort(string id) => $"{IncomingPorts}{id}";
		}

		internal static class Freeplug
		{
			internal const string Base = "freeplug/";
			internal const string ResetSuffix = "/reset/";

			internal static string Item(string id) => $"{Base}{id}/";
			internal static string Reset(string id) => $"{Base}{id}{ResetSuffix}";
		}

		internal static class Storage
		{
			internal const string Config = "storage/config/";
			internal const string Disks = "storage/disk/";
			internal const string FormatSuffix = "/format/";
			internal const string Partitions = "storage/partition/";
			internal const string CheckSuffix = "/check/";

			internal static string Disk(string id) => $"{Disks}{id}";
			internal static string DiskFormat(string id) => $"{Disks}{id}{FormatSuffix}";
			internal static string Partition(string id) => $"{Partitions}{id}";
			internal static string PartitionCheck(string id) => $"{Partitions}{id}{CheckSuffix}";
		}

		internal static class Switch
		{
			internal const string Status = "switch/status/";
			internal const string PortPrefix = "switch/port/";
			internal const string StatsSuffix = "/stats";
			internal const string SfpStatus = "sfp/status";
			internal const string SfpConfig = "sfp/config";
			internal const string SfpConfigSlash = "sfp/config/";

			internal static string Port(string id) => $"{PortPrefix}{id}";
			internal static string PortStats(string id) => $"{PortPrefix}{id}{StatsSuffix}";
		}

		internal static class VpnClient
		{
			internal const string Configurations = "vpn_client/config/";
			internal const string Log = "vpn_client/log";
			internal const string Status = "vpn_client/status";

			internal static string Configuration(string id) => $"{Configurations}{id}";
		}

		internal static class Vpn
		{
			internal const string Servers = "vpn/";
			internal const string Connections = "vpn/connection/";
			internal const string IpPool = "vpn/ip_pool/";
			internal const string OpenVpnRoutedConfiguration = "vpn/openvpn_routed/config/";
			internal const string Users = "vpn/user/";
			internal const string ConfigurationSuffix = "/config/";

			internal static string Connection(string id) => $"{Connections}{id}";
			internal static string User(string login) => $"{Users}{login}";
			internal static string ServerConfiguration(string vpnId) => $"{Servers}{vpnId}{ConfigurationSuffix}";
		}

		internal static class Wifi
		{
			internal const string Configuration = "wifi/config/";
			internal const string ConfigurationReset = "wifi/config/reset/";
			internal const string SteeringConfiguration = "wifi/steering/config/";
			internal const string State = "wifi/state/";
			internal const string AccessPoints = "wifi/ap/";
			internal const string AccessPointAllowedChannelCombinationSuffix = "/allowed_channel_comb";
			internal const string AccessPointStationsSuffix = "/stations/";
			internal const string AccessPointRestartSuffix = "/restart";
			internal const string AccessPointNeighborsSuffix = "/neighbors/";
			internal const string AccessPointNeighborsScanSuffix = "/neighbors/scan";
			internal const string AccessPointChannelUsageSuffix = "/channel_usage/";
			internal const string AccessPointDefaultSuffix = "/default";
			internal const string AccessPointDiagnosticsSuffix = "/diag";
			internal const string Bsses = "wifi/bss/";
			internal const string BssDefaultSuffix = "/default";
			internal const string BssDiagnosticsSuffix = "/diag";
			internal const string BssMloAllowedCombinationsSuffix = "/mlo/allowed_comb";
			internal const string BssMloConfigurationSuffix = "/mlo/config";
			internal const string Planning = "wifi/planning/";
			internal const string MacFilters = "wifi/mac_filter/";
			internal const string Defaults = "wifi/default";
			internal const string Diagnostics = "wifi/diag";
			internal const string TemporaryDisable = "wifi/temp_disable";
			internal const string CustomKeysConfiguration = "wifi/custom_keys/config/";
			internal const string CustomKeys = "wifi/custom_key/";
			internal const string WpsConfiguration = "wifi/wps/config/";
			internal const string WpsSessions = "wifi/wps/sessions/";
			internal const string WpsStart = "wifi/wps/start/";
			internal const string WpsStop = "wifi/wps/stop/";

			internal static string AccessPoint(string id) => $"{AccessPoints}{id}";
			internal static string AccessPointAllowedChannelCombination(string id) => $"{AccessPoints}{id}{AccessPointAllowedChannelCombinationSuffix}";
			internal static string AccessPointStations(string id) => $"{AccessPoints}{id}{AccessPointStationsSuffix}";
			internal static string AccessPointStation(string id, string mac) => $"{AccessPoints}{id}{AccessPointStationsSuffix}{mac}";
			internal static string AccessPointRestart(string id) => $"{AccessPoints}{id}{AccessPointRestartSuffix}";
			internal static string AccessPointNeighbors(string id) => $"{AccessPoints}{id}{AccessPointNeighborsSuffix}";
			internal static string AccessPointNeighborsScan(string id) => $"{AccessPoints}{id}{AccessPointNeighborsScanSuffix}";
			internal static string AccessPointChannelUsage(string id) => $"{AccessPoints}{id}{AccessPointChannelUsageSuffix}";
			internal static string AccessPointDefault(string id) => $"{AccessPoints}{id}{AccessPointDefaultSuffix}";
			internal static string AccessPointDiagnostics(string id) => $"{AccessPoints}{id}{AccessPointDiagnosticsSuffix}";
			internal static string Bss(string id) => $"{Bsses}{id}";
			internal static string BssDefault(string id) => $"{Bsses}{id}{BssDefaultSuffix}";
			internal static string BssDiagnostics(string id) => $"{Bsses}{id}{BssDiagnosticsSuffix}";
			internal static string BssMloAllowedCombinations(string id) => $"{Bsses}{id}{BssMloAllowedCombinationsSuffix}";
			internal static string BssMloConfiguration(string id) => $"{Bsses}{id}{BssMloConfigurationSuffix}";
			internal static string MacFilter(string filterId) => $"{MacFilters}{filterId}";
			internal static string CustomKey(string keyId) => $"{CustomKeys}{keyId}";
		}
	}
}
