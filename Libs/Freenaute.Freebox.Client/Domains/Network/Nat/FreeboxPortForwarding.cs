using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Nat;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxPortForwarding
{
	private readonly string _redirId;
	private readonly IFreeboxTransport _transport;

	public FreeboxPortForwarding(IFreeboxTransport transport, long redirId)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
		_redirId = FreeboxApiPath.EncodeSegment(NetworkPath.Number(redirId));
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Getting a specific port forwarding.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-fw-redir-redir_id</remarks>
	public async Task<PortForwardingConfig> GetAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Nat.Redirection(_redirId), Json.PortForwardingConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Updating a port forwarding.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-fw-redir-redir_id</remarks>
	public Task<PortForwardingConfig> UpdateAsync(PortForwardingPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Nat.Redirection(_redirId), request, Json.PortForwardingPatch,
			Json.PortForwardingConfig, cancellationToken);
	}

	/// <summary>Delete a port forwarding.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-fw-redir-redir_id</remarks>
	public async Task DeleteAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Delete, NetworkConstants.Routes.Nat.Redirection(_redirId), cancellationToken).ConfigureAwait(false);
}
