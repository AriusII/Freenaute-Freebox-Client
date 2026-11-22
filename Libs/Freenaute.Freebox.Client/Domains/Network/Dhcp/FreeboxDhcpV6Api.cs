using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Dhcp;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxDhcpV6Api
{
	private readonly IFreeboxTransport _transport;

	public FreeboxDhcpV6Api(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Get the current DHCPv6 configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-dhcpv6-config-</remarks>
	public async Task<DHCPv6Config> GetConfigurationAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Dhcp.V6Config, Json.DHCPv6Config, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Update the current DHCPv6 configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-dhcpv6-config-</remarks>
	public Task<DHCPv6Config> UpdateConfigurationAsync(DhcpV6ConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Dhcp.V6Config, request, Json.DhcpV6ConfigurationPatch,
			Json.DHCPv6Config, cancellationToken);
	}
}
