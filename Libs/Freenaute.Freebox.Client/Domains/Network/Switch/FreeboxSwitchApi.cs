namespace Freenaute.Freebox.Client.Domains.Network.Switch;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxSwitchApi
{
	private readonly IFreeboxTransport _transport;

	public FreeboxSwitchApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;

	public FreeboxSwitchPort Port(long id) => new(_transport, id);

	/// <summary>Get the current switch status.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-switch-status-</remarks>
	public Task<SwitchPortStatus[]> GetStatusAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Get, "switch/status/", Json.SwitchPortStatusArray, cancellationToken);
}
