using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Wifi;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxWifiBss
{
	private readonly string _id;
	private readonly IFreeboxTransport _transport;

	public FreeboxWifiBss(IFreeboxTransport transport, string id)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
		_id = FreeboxApiPath.EncodeSegment(id);
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Get a particular BSS.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-bss-id</remarks>
	public async Task<WifiBss> GetAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.Bss(_id), Json.WifiBss, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Update an BSS.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v9-wifi-bss-id</remarks>
	public Task<WifiBss> UpdateAsync(WifiBssPatch request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Wifi.Bss(_id), request, Json.WifiBssPatch, Json.WifiBss,
			cancellationToken);
	}

	/// <summary>Config reset value of a BSS.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-bss-id-default</remarks>
	public async Task<WifiBssConfig> GetDefaultsAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.BssDefault(_id), Json.WifiBssConfig, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Per AP/BSS diagnostic.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-ap-id-diag &amp; -api-v9-wifi-bss-id-diag</remarks>
	public Task<WifiDiagItem[]> GetDiagnosticsAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.BssDiagnostics(_id), Json.WifiDiagItemArray, cancellationToken);

	/// <summary>Per AP/BSS diagnostic.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v9-wifi-ap-id-diag &amp; -api-v9-wifi-bss-id-diag</remarks>
	public async Task FixDiagnosticsAsync(string[] request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		await _transport.SendAsync(HttpMethod.Post, NetworkConstants.Routes.Wifi.BssDiagnostics(_id), request, Json.StringArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Available partner.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v14-wifi-bss-id-mlo-allowed_comb</remarks>
	public Task<long[][]> GetAllowedMloPartnersAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.BssMloAllowedCombinations(_id), Json.Int64ArrayArray,
			cancellationToken);
	}

	/// <summary>Getting the MLO config.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v14-wifi-bss-id-mlo-config</remarks>
	public async Task<WifiMloConfiguration> GetMloConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Wifi.BssMloConfiguration(_id), Json.WifiMloConfiguration,
			cancellationToken).ConfigureAwait(false);
	}
}
