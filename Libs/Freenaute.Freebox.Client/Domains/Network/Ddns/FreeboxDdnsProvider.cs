using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Ddns;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxDdnsProvider
{
	private readonly string _provider;
	private readonly IFreeboxTransport _transport;

	public FreeboxDdnsProvider(IFreeboxTransport transport, string provider)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
		_provider = FreeboxApiPath.EncodeSegment(provider);
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Get the status of a DynDNS service.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v11-connection-ddns-provider-status-</remarks>
	public async Task<DdnsStatus> GetStatusAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Connection.DdnsProviderStatus(_provider), Json.DdnsStatus,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get the config of a DynDNS service.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v11-connection-ddns-provider-</remarks>
	public Task<DdnsConfig> GetConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Connection.DdnsProvider(_provider), Json.DdnsConfig,
			cancellationToken);
	}

	/// <summary>Set the config of a DynDNS service.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v11-connection-ddns-provider-</remarks>
	public async Task<DdnsConfig> UpdateConfigurationAsync(DdnsConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return await _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Connection.DdnsProvider(_provider), request,
			Json.DdnsConfigurationPatch, Json.DdnsConfig, cancellationToken).ConfigureAwait(false);
	}
}
