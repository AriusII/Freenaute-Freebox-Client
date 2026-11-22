using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Nat;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxNatApi
{
	private readonly IFreeboxTransport _transport;

	public FreeboxNatApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;

	public FreeboxPortForwarding Forwarding(long id) => new(_transport, id);

	public FreeboxIncomingPort IncomingPort(string id) => new(_transport, id);

	/// <summary>Get the current Dmz configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-fw-dmz-</remarks>
	public async Task<DmzConfig> GetDmzAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Nat.Dmz, Json.DmzConfig, cancellationToken).ConfigureAwait(false);

	/// <summary>Update the current Dmz configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-fw-dmz-</remarks>
	public Task<DmzConfig> UpdateDmzAsync(DmzConfigurationPatch request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Nat.Dmz, request, Json.DmzConfigurationPatch, Json.DmzConfig,
			cancellationToken);
	}

	/// <summary>Getting the list of port forwarding.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-fw-redir-</remarks>
	public async Task<PortForwardingConfig[]> GetForwardingsAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Nat.Redirections, Json.PortForwardingConfigArray, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Add a port forwarding.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v8-fw-redir-</remarks>
	public Task<PortForwardingConfig> CreateForwardingAsync(CreatePortForwardingRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Nat.Redirections, request, Json.CreatePortForwardingRequest,
			Json.PortForwardingConfig, cancellationToken);
	}

	/// <summary>Getting the list of incoming ports.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-fw-incoming-</remarks>
	public async Task<IncomingPortConfig[]> GetIncomingPortsAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Nat.IncomingPorts, Json.IncomingPortConfigArray, cancellationToken)
			.ConfigureAwait(false);
}
