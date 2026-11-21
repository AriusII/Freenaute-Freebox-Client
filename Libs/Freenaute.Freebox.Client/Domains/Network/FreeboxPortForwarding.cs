namespace Freenaute.Freebox.Client.Domains.Network;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxPortForwarding
{
	private readonly string _redir_id;
	private readonly IFreeboxTransport _transport;

	public FreeboxPortForwarding(IFreeboxTransport transport, long redir_id)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
		_redir_id = FreeboxApiPath.EncodeSegment(NetworkPath.Number(redir_id));
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Getting a specific port forwarding.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-fw-redir-redir_id</remarks>
	public Task<PortForwardingConfig> GetAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, $"fw/redir/{_redir_id}", Json.PortForwardingConfig,
			cancellationToken);
	}

	/// <summary>Updating a port forwarding.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-fw-redir-redir_id</remarks>
	public Task<PortForwardingConfig> UpdateAsync(PortForwardingPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, $"fw/redir/{_redir_id}", request, Json.PortForwardingPatch,
			Json.PortForwardingConfig, cancellationToken);
	}

	/// <summary>Delete a port forwarding.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-fw-redir-redir_id</remarks>
	public Task DeleteAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Delete, $"fw/redir/{_redir_id}", cancellationToken);
}
