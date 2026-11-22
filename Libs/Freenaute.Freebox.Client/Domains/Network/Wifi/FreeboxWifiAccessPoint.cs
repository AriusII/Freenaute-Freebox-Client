using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Wifi;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxWifiAccessPoint
{
	private readonly string _id;
	private readonly long _idValue;
	private readonly IFreeboxTransport _transport;

	public FreeboxWifiAccessPoint(IFreeboxTransport transport, long id)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
		_id = FreeboxApiPath.EncodeSegment(NetworkPath.Number(id));
		_idValue = id;
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;

	public FreeboxWifiStation Station(string mac) => new(_transport, _idValue, mac);

	/// <summary>Get a particular AP.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-ap-id</remarks>
	public async Task<WifiAp> GetAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.AccessPoint(_id), Json.WifiAp, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Update an AP.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v9-wifi-ap-id</remarks>
	public Task<WifiAp> UpdateAsync(WifiAccessPointPatch request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Wifi.AccessPoint(_id), request, Json.WifiAccessPointPatch, Json.WifiAp,
			cancellationToken);
	}

	/// <summary>Wi-Fi AP allowed channels.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-ap-id-allowed_channel_comb</remarks>
	public async Task<WifiAllowedComb[]> GetAllowedChannelsAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.AccessPointAllowedChannelCombination(_id),
			Json.WifiAllowedCombArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get Wi-Fi Stations List.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-ap-id-stations-</remarks>
	public Task<WifiStation[]> GetStationsAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.AccessPointStations(_id), Json.WifiStationArray,
			cancellationToken);
	}

	/// <summary>Get survey data history.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-ap-id-channel_survey_history-timestamp</remarks>
	public async Task<WifiApChannelSurveyData[]> GetSurveyHistoryAsync(long timestamp,
		CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get,
			$"wifi/ap/{_id}/channel_survey_history/{NetworkPath.Number(timestamp)}", Json.WifiApChannelSurveyDataArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Restart an AP.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v9-wifi-ap-id-restart</remarks>
	public Task RestartAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Wifi.AccessPointRestart(_id), cancellationToken);

	/// <summary>List AP neighbors.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-ap-id-neighbors-</remarks>
	public async Task<WifiNeighbor[]> GetNeighborsAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.AccessPointNeighbors(_id), Json.WifiNeighborArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>List Wi-Fi channels usage.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-ap-id-channel_usage-</remarks>
	public Task<WifiChannelUsage[]> GetChannelUsageAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.AccessPointChannelUsage(_id), Json.WifiChannelUsageArray,
			cancellationToken);
	}

	/// <summary>Refresh radar informations.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v9-wifi-ap-id-neighbors-scan</remarks>
	public async Task ScanNeighborsAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Wifi.AccessPointNeighborsScan(_id), cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Config reset value of an AP.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-ap-id-default</remarks>
	public Task<WifiApConfig> GetDefaultsAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.AccessPointDefault(_id), Json.WifiApConfig, cancellationToken);

	/// <summary>Per AP/BSS diagnostic.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-ap-id-diag &amp; -api-v9-wifi-bss-id-diag</remarks>
	public async Task<WifiDiagItem[]> GetDiagnosticsAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.AccessPointDiagnostics(_id), Json.WifiDiagItemArray, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Per AP/BSS diagnostic.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v9-wifi-ap-id-diag &amp; -api-v9-wifi-bss-id-diag</remarks>
	public Task FixDiagnosticsAsync(string[] request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Wifi.AccessPointDiagnostics(_id), request, Json.StringArray,
			cancellationToken);
	}
}
