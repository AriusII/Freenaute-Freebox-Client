using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Wifi;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxWifiGuestAccessApi
{
	private readonly IFreeboxTransport _transport;

	public FreeboxWifiGuestAccessApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;

	public FreeboxWifiGuestKey Key(long id) => new(_transport, id);

	/// <summary>Get or change the dedicated ap config.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v14-wifi-custom_keys-config-</remarks>
	public async Task<WifiCustomKeyConfig> GetConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.CustomKeysConfiguration, Json.WifiCustomKeyConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get or change the dedicated ap config.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v14-wifi-custom_keys-config-</remarks>
	public Task<WifiCustomKeyConfig> UpdateConfigurationAsync(WifiCustomKeyConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Wifi.CustomKeysConfiguration, request,
			Json.WifiCustomKeyConfigurationPatch, Json.WifiCustomKeyConfig, cancellationToken);
	}

	/// <summary>Get the list of wifi custom key.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-custom_key-</remarks>
	public async Task<WifiCustomKey[]> GetKeysAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.CustomKeys, Json.WifiCustomKeyArray, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Create a new wifi custom key.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v9-wifi-custom_key-</remarks>
	public Task<WifiCustomKey> CreateKeyAsync(CreateWifiCustomKeyRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Wifi.CustomKeys, request, Json.CreateWifiCustomKeyRequest,
			Json.WifiCustomKey, cancellationToken);
	}
}
