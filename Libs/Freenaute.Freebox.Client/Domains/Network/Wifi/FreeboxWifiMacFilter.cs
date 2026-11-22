using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Wifi;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxWifiMacFilter
{
	private readonly string _filterId;
	private readonly IFreeboxTransport _transport;

	public FreeboxWifiMacFilter(IFreeboxTransport transport, string filterId)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
		_filterId = FreeboxApiPath.EncodeSegment(filterId);
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Getting a particular MAC filter.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-mac_filter-filter_id</remarks>
	public async Task<WifiMacFilter> GetAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.MacFilter(_filterId), Json.WifiMacFilter,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Updating a MAC filter.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v9-wifi-mac_filter-filter_id</remarks>
	public Task<WifiMacFilter> UpdateAsync(WifiMacFilterPatch request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Wifi.MacFilter(_filterId), request, Json.WifiMacFilterPatch,
			Json.WifiMacFilter, cancellationToken);
	}

	/// <summary>Delete a MAC filter.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#delete--api-v9-wifi-mac_filter-filter_id</remarks>
	public async Task DeleteAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Delete, NetworkConstants.Routes.Wifi.MacFilter(_filterId), cancellationToken)
			.ConfigureAwait(false);
}
