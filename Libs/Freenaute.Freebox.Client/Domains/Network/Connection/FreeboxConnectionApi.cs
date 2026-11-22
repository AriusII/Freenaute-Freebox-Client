using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Connection;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxConnectionApi
{
	private readonly IFreeboxTransport _transport;

	public FreeboxConnectionApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;

	public FreeboxDdnsProvider Ddns(string provider) => new(_transport, provider);

	/// <summary>Get the current Connection status.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v11-connection-</remarks>
	public async Task<ConnectionStatus> GetStatusAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Connection.Base, Json.ConnectionStatus, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Get the current Connection configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v11-connection-config-</remarks>
	public Task<ConnectionConfiguration> GetConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Connection.Config, Json.ConnectionConfiguration,
			cancellationToken);
	}

	/// <summary>Update the Connection configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v11-connection-config-</remarks>
	public async Task<ConnectionConfiguration> UpdateConfigurationAsync(ConnectionConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return await _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Connection.Config, request,
			Json.ConnectionConfigurationPatch,
			Json.ConnectionConfiguration, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get the current IPv6 Connection configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v11-connection-ipv6-config-</remarks>
	public Task<ConnectionIpv6Configuration> GetIpv6ConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Connection.Ipv6Config, Json.ConnectionIpv6Configuration,
			cancellationToken);
	}

	/// <summary>Update the IPv6 Connection configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v11-connection-ipv6-config-</remarks>
	public async Task<ConnectionIpv6Configuration> UpdateIpv6ConfigurationAsync(
		ConnectionIpv6ConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return await _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Connection.Ipv6Config, request,
				Json.ConnectionIpv6ConfigurationPatch, Json.ConnectionIpv6Configuration, cancellationToken)
			.ConfigureAwait(false);
	}

	/// <summary>Get the current xDSL/LTE aggregation infos.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v11-connection-aggregation</remarks>
	public Task<LteAggregationResult> GetAggregationAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Connection.Aggregation, Json.LteAggregationResult,
			cancellationToken);
	}

	/// <summary>Update the xDSL/LTE aggregation configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v11-connection-aggregation</remarks>
	public async Task UpdateAggregationAsync(LteAggregationUpdate request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		await _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Connection.Aggregation, request, Json.LteAggregationUpdate,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get FTTH optical link details.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v11-connection-ftth-
	/// </remarks>
	public Task<FtthStatus> GetFtthAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Connection.Ftth, Json.FtthStatus,
			cancellationToken);
	}

	/// <summary>Get xDSL line details.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v11-connection-xdsl-
	/// </remarks>
	public async Task<XdslInfos> GetXdslAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Connection.Xdsl, Json.XdslInfos,
			cancellationToken).ConfigureAwait(false);
	}
}
