using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Dhcp;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxDhcpApi
{
	private readonly IFreeboxTransport _transport;

	public FreeboxDhcpApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;

	public FreeboxDhcpStaticLease StaticLease(string id) => new(_transport, id);

	/// <summary>Get the current DHCP configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v16-dhcp-config-</remarks>
	public async Task<DhcpConfig> GetConfigurationAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Dhcp.Config, Json.DhcpConfig, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Update the current DHCP configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v16-dhcp-config-</remarks>
	public Task<DhcpConfig> UpdateConfigurationAsync(DhcpConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Dhcp.Config, request, Json.DhcpConfigurationPatch,
			Json.DhcpConfig, cancellationToken);
	}

	/// <summary>Get the list of DHCP static leases.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v16-dhcp-static_lease-</remarks>
	public async Task<DhcpStaticLease[]> GetStaticLeasesAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Dhcp.StaticLeases, Json.DhcpStaticLeaseArray, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Add a DHCP static lease.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v16-dhcp-static_lease-</remarks>
	public Task<DhcpStaticLease> CreateStaticLeaseAsync(CreateDhcpStaticLeaseRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Dhcp.StaticLeases, request, Json.CreateDhcpStaticLeaseRequest,
			Json.DhcpStaticLease, cancellationToken);
	}

	/// <summary>Get the list of DHCP dynamic leases.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v16-dhcp-dynamic_lease-</remarks>
	public async Task<DhcpDynamicLease[]> GetDynamicLeasesAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Dhcp.DynamicLeases, Json.DhcpDynamicLeaseArray,
			cancellationToken).ConfigureAwait(false);
	}
}
