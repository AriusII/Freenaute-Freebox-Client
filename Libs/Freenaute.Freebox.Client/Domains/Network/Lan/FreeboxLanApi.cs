using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Lan;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxLanApi
{
	private readonly IFreeboxTransport _transport;

	public FreeboxLanApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;

	public FreeboxLanInterface Interface(string name) => new(_transport, name);

	/// <summary>Get the current Lan configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-lan-config-</remarks>
	public async Task<LanConfig> GetConfigurationAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Lan.Config, Json.LanConfig, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Update the current Lan configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-lan-config-</remarks>
	public Task<LanConfig> UpdateConfigurationAsync(LanConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Lan.Config, request, Json.LanConfigurationPatch, Json.LanConfig,
			cancellationToken);
	}

	/// <summary>Get the current routing configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v16-lan-routes</remarks>
	public async Task<Route[]> GetRoutesAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Lan.Routes, Json.RouteArray, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Update the current routing configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v16-lan-routes-</remarks>
	public Task<Route[]> ReplaceRoutesAsync(LanRouteWrite[] request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Lan.RoutesCollection, request, Json.LanRouteWriteArray, Json.RouteArray,
			cancellationToken);
	}

	/// <summary>Getting the list of browsable LAN interfaces.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-lan-browser-interfaces-</remarks>
	public async Task<LanBrowserInterface[]> GetInterfacesAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Lan.BrowserInterfaces, Json.LanBrowserInterfaceArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Getting available lan host types.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-lan-browser-types-</remarks>
	public Task<LanHostTypeDescriptor[]> GetHostTypesAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Lan.BrowserTypes, Json.LanHostTypeDescriptorArray,
			cancellationToken);
	}
}
