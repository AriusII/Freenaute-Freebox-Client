using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Wifi;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxWifiWpsApi
{
	private readonly IFreeboxTransport _transport;

	public FreeboxWifiWpsApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Enable/disable WPS on all Wi-Fi cards.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-wps-config-</remarks>
	public async Task<WifiWpsConfiguration> GetConfigurationAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.WpsConfiguration, Json.WifiWpsConfiguration, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Enable/disable WPS on all Wi-Fi cards.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v9-wifi-wps-config-</remarks>
	public Task<WifiWpsConfiguration> UpdateConfigurationAsync(WifiWpsConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Wifi.WpsConfiguration, request, Json.WifiWpsConfigurationPatch,
			Json.WifiWpsConfiguration, cancellationToken);
	}

	/// <summary>Start a Wps session on a bss.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v9-wifi-wps-start-</remarks>
	public async Task<long> StartAsync(WifiWpsStartRequest request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return await _transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Wifi.WpsStart, request, Json.WifiWpsStartRequest,
			Json.Int64,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>List the Wps session.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-wps-sessions-</remarks>
	public Task<WifiWpsSession[]> GetSessionsAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.WpsSessions, Json.WifiWpsSessionArray, cancellationToken);

	/// <summary>Clear all Wps Sessions.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#delete--api-v9-wifi-wps-sessions-</remarks>
	public async Task ClearSessionsAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Delete, NetworkConstants.Routes.Wifi.WpsSessions, cancellationToken).ConfigureAwait(false);

	/// <summary>Stop a Wps session.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#stop-a-wps-session</remarks>
	public Task StopAsync(WifiWpsStopRequest request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Wifi.WpsStop, request, Json.WifiWpsStopRequest,
			cancellationToken);
	}
}
