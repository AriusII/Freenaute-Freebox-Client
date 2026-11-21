namespace Freenaute.Freebox.Client.Domains.Network;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxIncomingPort
{
	private readonly string _port_id;
	private readonly IFreeboxTransport _transport;

	public FreeboxIncomingPort(IFreeboxTransport transport, string port_id)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
		_port_id = FreeboxApiPath.EncodeSegment(port_id);
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Getting a specific incoming port.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-fw-incoming-port_id</remarks>
	public Task<IncomingPortConfig> GetAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, $"fw/incoming/{_port_id}", Json.IncomingPortConfig,
			cancellationToken);
	}

	/// <summary>Updating an incoming port.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-fw-incoming-port_id</remarks>
	public Task<IncomingPortConfig> UpdateAsync(IncomingPortPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, $"fw/incoming/{_port_id}", request, Json.IncomingPortPatch,
			Json.IncomingPortConfig, cancellationToken);
	}
}
