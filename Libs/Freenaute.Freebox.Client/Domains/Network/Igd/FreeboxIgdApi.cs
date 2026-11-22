using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Igd;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxIgdApi
{
	private readonly IFreeboxTransport _transport;

	public FreeboxIgdApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;

	public FreeboxIgdRedirection Redirection(string id) => new(_transport, id);

	/// <summary>Get the current UPnP IGD configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-upnpigd-config-</remarks>
	public async Task<UPnPIGDConfig> GetConfigurationAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Igd.Config, Json.UPnPIGDConfig, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Update the UPnP IGD configuration.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-upnpigd-config-</remarks>
	public Task<UPnPIGDConfig> UpdateConfigurationAsync(IgdConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Igd.Config, request, Json.IgdConfigurationPatch,
			Json.UPnPIGDConfig, cancellationToken);
	}

	/// <summary>Get the list of current redirection.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-upnpigd-redir-</remarks>
	public async Task<UPnPRedir[]> GetRedirectionsAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Igd.Redirections, Json.UPnPRedirArray, cancellationToken)
			.ConfigureAwait(false);
}
