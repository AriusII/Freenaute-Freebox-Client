using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Nat;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxIncomingPort
{
	private readonly string _portId;
	private readonly IFreeboxTransport _transport;

	public FreeboxIncomingPort(IFreeboxTransport transport, string portId)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
		_portId = FreeboxApiPath.EncodeSegment(portId);
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Getting a specific incoming port.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-fw-incoming-port_id</remarks>
	public async Task<IncomingPortConfig> GetAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Nat.IncomingPort(_portId), Json.IncomingPortConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Updating an incoming port.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-fw-incoming-port_id</remarks>
	public Task<IncomingPortConfig> UpdateAsync(IncomingPortPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Nat.IncomingPort(_portId), request, Json.IncomingPortPatch,
			Json.IncomingPortConfig, cancellationToken);
	}
}
