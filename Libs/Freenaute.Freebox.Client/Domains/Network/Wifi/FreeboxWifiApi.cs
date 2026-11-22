using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Wifi;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxWifiApi
{
	private readonly IFreeboxTransport _transport;

	public FreeboxWifiApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
		Wps = new FreeboxWifiWpsApi(transport);
		GuestAccess = new FreeboxWifiGuestAccessApi(transport);
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;

	public FreeboxWifiWpsApi Wps { get; }
	public FreeboxWifiGuestAccessApi GuestAccess { get; }

	public FreeboxWifiAccessPoint AccessPoint(long id) => new(_transport, id);

	public FreeboxWifiBss Bss(string id) => new(_transport, id);

	public FreeboxWifiBss Bss(long id) => Bss(NetworkPath.Number(id));

	public FreeboxWifiMacFilter MacFilter(string id) => new(_transport, id);

	/// <summary>Get the current Wi-Fi global configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-config-</remarks>
	public async Task<WifiGlobalConfig> GetConfigurationAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.Configuration, Json.WifiGlobalConfig, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Update the Wi-Fi global configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v9-wifi-config-</remarks>
	public Task<WifiGlobalConfig> UpdateConfigurationAsync(WifiGlobalConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Wifi.Configuration, request, Json.WifiGlobalConfigurationPatch,
			Json.WifiGlobalConfig, cancellationToken);
	}

	/// <summary>Get the current Wi-Fi steering configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v16-wifi-steering-config-</remarks>
	public async Task<WifiSteeringConfig> GetSteeringAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.SteeringConfiguration, Json.WifiSteeringConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update the Wi-Fi steering configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v16-wifi-steering-config-</remarks>
	public Task<WifiSteeringConfig> UpdateSteeringAsync(WifiSteeringConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Wifi.SteeringConfiguration, request,
			Json.WifiSteeringConfigurationPatch, Json.WifiSteeringConfig, cancellationToken);
	}

	/// <summary>Get the global wifi state.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v10-wifi-state-</remarks>
	public async Task<ObjectOrArray<WifiGlobalState>> GetStateAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.State, Json.ObjectOrArrayWifiGlobalState,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get the ap list.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-ap-</remarks>
	public Task<WifiAp[]> GetAccessPointsAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.AccessPoints, Json.WifiApArray, cancellationToken);

	/// <summary>Get the bss list.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-bss-</remarks>
	public async Task<WifiBss[]> GetBssListAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.Bsses, Json.WifiBssArray, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Get Wi-Fi Planning.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-planning-</remarks>
	public Task<WifiPlanning> GetPlanningAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.Planning, Json.WifiPlanning, cancellationToken);

	/// <summary>Update Wi-Fi Planning.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v9-wifi-planning-</remarks>
	public async Task<WifiPlanning> UpdatePlanningAsync(WifiPlanningPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return await _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Wifi.Planning, request, Json.WifiPlanningPatch,
			Json.WifiPlanning, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get the MAC filter list.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-mac_filter-</remarks>
	public Task<WifiMacFilter[]> GetMacFiltersAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.MacFilters, Json.WifiMacFilterArray, cancellationToken);

	/// <summary>Create a new MAC filter.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v9-wifi-mac_filter-</remarks>
	public async Task<WifiMacFilter> CreateMacFilterAsync(CreateWifiMacFilterRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return await _transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Wifi.MacFilters, request, Json.CreateWifiMacFilterRequest,
			Json.WifiMacFilter, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Global reset.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v9-wifi-config-reset-</remarks>
	public Task ResetAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Wifi.ConfigurationReset, cancellationToken);

	/// <summary>Config reset value (bulk).</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-default</remarks>
	public async Task<WifiDefaultsResult> GetDefaultsAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.Defaults, Json.WifiDefaultsResult, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Global diagnostic.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-diag</remarks>
	public Task<WifiDiagnosticsResult> GetDiagnosticsAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.Diagnostics, Json.WifiDiagnosticsResult, cancellationToken);

	/// <summary>Global diagnostic.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v9-wifi-diag</remarks>
	public async Task FixDiagnosticsAsync(WifiDiagnosticFix request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		await _transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Wifi.Diagnostics, request, Json.WifiDiagnosticFix, cancellationToken)
			.ConfigureAwait(false);
	}

	/// <summary>Get temporary disable state.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v13-wifi-temp_disable</remarks>
	public Task<TemporaryWifiDisable> GetTemporaryDisableAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.TemporaryDisable, Json.TemporaryWifiDisable, cancellationToken);

	/// <summary>Get temporary disable state.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v13-wifi-temp_disable</remarks>
	public async Task TemporarilyDisableAsync(TemporaryWifiDisableRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		await _transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Wifi.TemporaryDisable, request, Json.TemporaryWifiDisableRequest,
			cancellationToken).ConfigureAwait(false);
	}
}
