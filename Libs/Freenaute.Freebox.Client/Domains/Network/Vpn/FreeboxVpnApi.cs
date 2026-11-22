using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Vpn;

/// <summary>VPN API routes.</summary>
public sealed class FreeboxVpnApi(IFreeboxTransport transport)
{
	private static JsonTypeInfo<JsonElement> JsonElementType => ServicesJsonSerializerContext.Default.JsonElement;

	/// <summary>Get VPN overview.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn-
	/// </remarks>
	public async Task<VPNServer[]> GetAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Vpn.Servers, NetworkJsonSerializerContext.Default.VPNServerArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Delete one active VPN connection.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-vpn-connection-id
	/// </remarks>
	public async Task DisconnectConnectionAsync(long id, CancellationToken cancellationToken = default) =>
		await transport.SendAsync(HttpMethod.Delete, NetworkConstants.Routes.Vpn.Connection(FilesPath.Id(id)), cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Delete one active VPN connection.</summary>
	public async Task DisconnectConnectionAsync(string id, CancellationToken cancellationToken = default)
	{
		await transport.SendAsync(HttpMethod.Delete,
			$"vpn/connection/{FreeboxApiPath.EncodeSegment(id)}", cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get VPN IP pool.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn-ip_pool-
	/// </remarks>
	public async Task<VpnIpPool> GetIpPoolAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Vpn.IpPool, NetworkJsonSerializerContext.Default.VpnIpPool,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update routed OpenVPN server configuration.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#put--api-v8-vpn-openvpn_routed-config-
	/// </remarks>
	public async Task<VPNServerConfig> UpdateOpenVpnRoutedConfigurationAsync(VPNServerConfig request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return await transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Vpn.OpenVpnRoutedConfiguration, request,
			NetworkJsonSerializerContext.Default.VPNServerConfig, NetworkJsonSerializerContext.Default.VPNServerConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update routed OpenVPN server configuration using a raw JSON object.</summary>
	public async Task<JsonElement> UpdateOpenVpnRoutedConfigurationAsync(JsonElement request,
		CancellationToken cancellationToken = default)
	{
		RequireObject(request, nameof(request));
		return await transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Vpn.OpenVpnRoutedConfiguration, request, JsonElementType,
			JsonElementType, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>List active VPN connections.</summary>
	public async Task<VPNConnection[]> GetConnectionsAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Vpn.Connections,
			NetworkJsonSerializerContext.Default.VPNConnectionArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Download a VPN server configuration for a user.</summary>
	public async Task<JsonElement> DownloadConfigurationAsync(string serverName, string login,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get,
			$"vpn/download_config/{FreeboxApiPath.EncodeSegment(serverName)}/{FreeboxApiPath.EncodeSegment(login)}",
			JsonElementType,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Download a VPN server configuration for a user.</summary>
	public async Task<JsonElement> DownloadConfigAsync(string serverName, string login,
		CancellationToken cancellationToken = default) =>
		await DownloadConfigurationAsync(serverName, login, cancellationToken).ConfigureAwait(false);

	/// <summary>Download a VPN server configuration for a user.</summary>
	public async Task<JsonElement> GetDownloadConfigAsync(string serverName, string login,
		CancellationToken cancellationToken = default) =>
		await DownloadConfigurationAsync(serverName, login, cancellationToken).ConfigureAwait(false);

	/// <summary>List VPN users.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn-user-
	/// </remarks>
	public async Task<VPNUser[]> GetUsersAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Vpn.Users, NetworkJsonSerializerContext.Default.VPNUserArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Create a VPN user.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#post--api-v8-vpn-user-
	/// </remarks>
	public async Task<VPNUser> CreateUserAsync(VPNUser request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return await transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Vpn.Users, request,
			NetworkJsonSerializerContext.Default.VPNUser,
			NetworkJsonSerializerContext.Default.VPNUser,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Create a VPN user using a raw JSON object.</summary>
	public async Task<JsonElement> CreateUserAsync(JsonElement request,
		CancellationToken cancellationToken = default)
	{
		RequireObject(request, nameof(request));
		return await transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Vpn.Users, request, JsonElementType, JsonElementType,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get one VPN user.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn-user-login
	/// </remarks>
	public async Task<VPNUser> GetUserAsync(string login, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Vpn.User(FreeboxApiPath.EncodeSegment(login)),
			NetworkJsonSerializerContext.Default.VPNUser,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update one VPN user.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#put--api-v8-vpn-user-login
	/// </remarks>
	public async Task<VPNUser> UpdateUserAsync(string login, VPNUser request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return await transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Vpn.User(FreeboxApiPath.EncodeSegment(login)), request,
			NetworkJsonSerializerContext.Default.VPNUser, NetworkJsonSerializerContext.Default.VPNUser,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update one VPN user using a raw JSON object.</summary>
	public async Task<JsonElement> UpdateUserAsync(string login, JsonElement request,
		CancellationToken cancellationToken = default)
	{
		RequireObject(request, nameof(request));
		return await transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Vpn.User(FreeboxApiPath.EncodeSegment(login)), request,
			JsonElementType, JsonElementType, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Delete one VPN user.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-vpn-user-login
	/// </remarks>
	public async Task DeleteUserAsync(string login, CancellationToken cancellationToken = default)
	{
		await transport.SendAsync(HttpMethod.Delete, NetworkConstants.Routes.Vpn.User(FreeboxApiPath.EncodeSegment(login)),
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get VPN server configuration by VPN identifier.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn-vpn_id-config-
	/// </remarks>
	public async Task<VPNServerConfig> GetConfigurationAsync(string vpnId,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Vpn.ServerConfiguration(FreeboxApiPath.EncodeSegment(vpnId)),
			NetworkJsonSerializerContext.Default.VPNServerConfig,
			cancellationToken).ConfigureAwait(false);
	}

	private static void RequireObject(JsonElement request, string parameterName)
	{
		if (request.ValueKind is not JsonValueKind.Object)
		{
			throw new ArgumentException("Request payload must be a JSON object.", parameterName);
		}
	}
}
