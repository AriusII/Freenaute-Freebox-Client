using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.VpnClient;

/// <summary>VPN client API routes.</summary>
public sealed class FreeboxVpnClientApi(IFreeboxTransport transport)
{
	/// <summary>List VPN client configurations.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn_client-config-
	/// </remarks>
	public async Task<VPNClientConfig[]> GetConfigurationsAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.VpnClient.Configurations,
			NetworkJsonSerializerContext.Default.VPNClientConfigArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Create a VPN client configuration.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#post--api-v8-vpn_client-config-
	/// </remarks>
	public async Task<VPNClientConfig> CreateConfigurationAsync(VPNClientConfig request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return await transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.VpnClient.Configurations, request,
			NetworkJsonSerializerContext.Default.VPNClientConfig, NetworkJsonSerializerContext.Default.VPNClientConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get one VPN client configuration.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn_client-config-id
	/// </remarks>
	public async Task<VPNClientConfig> GetConfigurationAsync(string id, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.VpnClient.Configuration(FreeboxApiPath.EncodeSegment(id)),
			NetworkJsonSerializerContext.Default.VPNClientConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update one VPN client configuration.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#put--api-v8-vpn_client-config-id
	/// </remarks>
	public async Task<VPNClientConfig> UpdateConfigurationAsync(string id, VPNClientConfig request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return await transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.VpnClient.Configuration(FreeboxApiPath.EncodeSegment(id)),
			request,
			NetworkJsonSerializerContext.Default.VPNClientConfig,
			NetworkJsonSerializerContext.Default.VPNClientConfig, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Delete one VPN client configuration.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-vpn_client-config-id
	/// </remarks>
	public async Task DeleteConfigurationAsync(string id, CancellationToken cancellationToken = default)
	{
		await transport.SendAsync(HttpMethod.Delete, NetworkConstants.Routes.VpnClient.Configuration(FreeboxApiPath.EncodeSegment(id)),
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get VPN client logs.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn_client-log
	/// </remarks>
	public async Task<string> GetLogAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.VpnClient.Log, NetworkJsonSerializerContext.Default.String,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get VPN client status.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn_client-status
	/// </remarks>
	public async Task<VPNClientStatus> GetStatusAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.VpnClient.Status,
			NetworkJsonSerializerContext.Default.VPNClientStatus,
			cancellationToken).ConfigureAwait(false);
	}
}
