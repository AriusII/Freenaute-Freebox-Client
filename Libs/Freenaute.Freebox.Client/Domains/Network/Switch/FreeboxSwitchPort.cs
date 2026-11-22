using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Switch;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxSwitchPort
{
	private readonly string _id;
	private readonly IFreeboxTransport _transport;

	public FreeboxSwitchPort(IFreeboxTransport transport, long id)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
		_id = FreeboxApiPath.EncodeSegment(NetworkPath.Number(id));
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Get a port configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-switch-port-id</remarks>
	public async Task<SwitchPortConfig> GetConfigurationAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Switch.Port(_id), Json.SwitchPortConfig, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Update a port configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-switch-port-id</remarks>
	public Task<SwitchPortConfig> UpdateConfigurationAsync(SwitchPortConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Switch.Port(_id), request, Json.SwitchPortConfigurationPatch,
			Json.SwitchPortConfig, cancellationToken);
	}

	/// <summary>Get one switch port statistics snapshot.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-switch-port-id-stats
	/// </remarks>
	public async Task<SwitchPortStats> GetStatisticsAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Switch.PortStats(_id), Json.SwitchPortStats,
			cancellationToken).ConfigureAwait(false);
	}
}
