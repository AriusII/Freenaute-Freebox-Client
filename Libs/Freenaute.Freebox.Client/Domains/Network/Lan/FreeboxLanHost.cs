namespace Freenaute.Freebox.Client.Domains.Network.Lan;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxLanHost
{
	private readonly string _hostid;
	private readonly string _interface;
	private readonly IFreeboxTransport _transport;

	public FreeboxLanHost(IFreeboxTransport transport, string @interface, string hostid)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
		_interface = FreeboxApiPath.EncodeSegment(@interface);
		_hostid = FreeboxApiPath.EncodeSegment(hostid);
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Getting an host information.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v16-lan-browser-interface-hostid-</remarks>
	public Task<LanHost> GetAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, $"lan/browser/{_interface}/{_hostid}/", Json.LanHost,
			cancellationToken);
	}

	/// <summary>Updating an host information.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v16-lan-browser-interface-hostid-</remarks>
	public Task<LanHost> UpdateAsync(LanHostPatch request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, $"lan/browser/{_interface}/{_hostid}/", request, Json.LanHostPatch,
			Json.LanHost, cancellationToken);
	}
}
