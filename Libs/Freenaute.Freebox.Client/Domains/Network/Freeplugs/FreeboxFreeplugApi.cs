using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Freeplugs;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxFreeplugApi
{
	private readonly IFreeboxTransport _transport;

	public FreeboxFreeplugApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;

	public FreeboxFreeplug Plug(string id) => new(_transport, id);

	/// <summary>Get the current Freeplugs networks.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-freeplug-</remarks>
	public async Task<FreeplugNetwork[]> GetNetworksAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Freeplug.Base, Json.FreeplugNetworkArray, cancellationToken)
			.ConfigureAwait(false);
}
