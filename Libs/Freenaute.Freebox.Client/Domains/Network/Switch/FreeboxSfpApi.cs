using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Switch;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxSfpApi
{
	private readonly IFreeboxTransport _transport;

	public FreeboxSfpApi(IFreeboxTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Get SFP status.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v11-sfp-status</remarks>
	public async Task<SfpStatus> GetStatusAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Switch.SfpStatus, Json.SfpStatus, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Update SFP config.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v11-sfp-config</remarks>
	public Task<SfpConfig> UpdateConfigurationAsync(SfpConfigurationPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Switch.SfpConfig, request, Json.SfpConfigurationPatch, Json.SfpConfig,
			cancellationToken);
	}

	/// <summary>Get SFP config.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get-sfp-config</remarks>
	public async Task<SfpConfig> GetConfigurationAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Switch.SfpConfigSlash, Json.SfpConfig, cancellationToken)
			.ConfigureAwait(false);
}
