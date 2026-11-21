namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#DhcpOption.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class DhcpOptionWrite
{
	/// <summary>
	///     The valid option identifiers and types are: Identifier Type Description time_offset s32 Time offset
	///     time_server ip_list Time server log_server ip_list Log server cookie_server ip_list Cookie server lpr_server
	///     ip_list LPR server impress_server ip_list Impress server resource_location_server ip_list Resource location server
	///     hostname string Hostname merit_dump_file string Merit dump file domain_name string Domain name swap_server ip_list
	///     Swap server root_path string Root path extensions_path string Extensions path ip_fwd bool IP forwarding
	///     ip_fwd_non_local bool Non-local IP source routing ip_max_reassembly_size u16 Maximum IP reassembly size ip_ttl u8
	///     Default IP TTL ip_pmtu_timeout u32 IP Path MTU timeout mtu u16 Interface MTU local_subnets bool All subnets are
	///     local mask_discovery bool Perform mask discovery mask_supplier bool Mask supplier perform_rd bool Perform router
	///     discovery rs_address ip Router solicitation address trailer_encapsulation bool Trailer encapsulation
	///     arp_cache_timeout u32 ARP cache timeout eth_encapsulation bool Ethernet encapsulation tcp_ttl u8 Default TCP TTL
	///     tcp_keepalive_interval u32 TCP keepalive interval tcp_keepalive_garbage bool TCP keepalive garbage nis_domain
	///     string NIS domain nis_server ip_list NIS server ntp_server ip_list NTP server vendor_specific hexstring Vendor
	///     specific information nis_plus_domain string NIS+ domain nis_plus_server ip_list NIS+ server tftp_server_name string
	///     TFTP server name bootfile_name string Bootfile name mobile_ip_agent ip_list Mobile IP home agent smtp_server
	///     ip_list SMTP server pop3_server ip_list POP3 server nntp_server ip_list NNTP server www_server ip_list Default WWW
	///     server finger_server ip_list Default Finger server irc_server ip_list Default IRC server streettalk_server ip_list
	///     StreetTalk server stda_server ip_list StreetTalk directory assistance server slp_directory_agent ip_list SLP
	///     directory agent slp_service_scope hexstring SLP service scope nds_servers ip_list NDS servers nds_tree_name string
	///     NDS tree name nds_context string NDS context ldap_servers ip_list LDAP servers timezone_posix string Timezone POSIX
	///     timezone_database string Timezone database name_service hexstring Name service domain_search hexstring Domain
	///     search classless_static_route hexstring Classless static route capwap_ac ip_list CAPWAP access controller
	///     tftp_server_address ip_list TFTP server address
	/// </summary>
	[JsonPropertyName("id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Id { get; init; }

	/// <summary>
	///     The value sent by the DHCP server when this option is requested by the client. The formats depend on the
	///     option type: ip: A single IPv4 address (as described in RFC 791) ip_list: A comma-separated list of IPv4 addresses
	///     string: A string of ASCII characters hexstring: A string of ASCII hexadecimal characters [0-9a-fA-F] representing a
	///     binary value (example: C0A801FE) bool: one of [ ‘true’, ‘false’, ‘1’, ‘0’ ] s8, s16, s32: An n-bit signed integer
	///     value u8, u16, u32: An n-bit unsigned integer value
	/// </summary>
	[JsonPropertyName("val")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Val { get; init; }
}
