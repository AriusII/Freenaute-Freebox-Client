using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Wifi;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxWifiGuestKey
{
	private readonly string _keyId;
	private readonly IFreeboxTransport _transport;

	public FreeboxWifiGuestKey(IFreeboxTransport transport, long keyId)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
		_keyId = FreeboxApiPath.EncodeSegment(NetworkPath.Number(keyId));
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Getting a particular wifi custom key.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-custom_key-key_id</remarks>
	public async Task<WifiCustomKey> GetAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.CustomKey(_keyId), Json.WifiCustomKey,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Delete a wifi custom key.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#delete--api-v9-wifi-custom_key-key_id</remarks>
	public Task DeleteAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Delete, NetworkConstants.Routes.Wifi.CustomKey(_keyId), cancellationToken);
}
