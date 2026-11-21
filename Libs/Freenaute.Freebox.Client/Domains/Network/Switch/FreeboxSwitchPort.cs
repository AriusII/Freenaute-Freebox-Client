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
	public Task<SwitchPortConfig> GetConfigurationAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Get, $"switch/port/{_id}", Json.SwitchPortConfig, cancellationToken);

	/// <summary>Update a port configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-switch-port-id</remarks>
	public Task<SwitchPortConfig> UpdateConfigurationAsync(SwitchPortConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, $"switch/port/{_id}", request, Json.SwitchPortConfigurationPatch,
			Json.SwitchPortConfig, cancellationToken);
	}
}
