namespace Freenaute.Freebox.Client.AirMedia;

internal sealed class FreeboxAirMediaApi(FreeboxHttpTransport transport) : IFreeboxAirMediaApi
{
	private static readonly FreeboxJsonSerializerContext Json = FreeboxJsonSerializerContext.Default;

	public async Task<AirMediaConfigResponse> GetConfigurationAsync(CancellationToken cancellationToken = default) =>
		await transport.SendAsync(HttpMethod.Get, ClientConstants.Routes.AirMedia.Configuration,
			Json.AirMediaConfigResponse, cancellationToken)
			.ConfigureAwait(false);

	public async Task<AirMediaConfigResponse> UpdateConfigurationAsync(UpdateAirMediaConfigRequest request,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Put, ClientConstants.Routes.AirMedia.Configuration, request,
			Json.UpdateAirMediaConfigRequest, Json.AirMediaConfigResponse, cancellationToken).ConfigureAwait(false);
	}

	public async Task<IReadOnlyList<AirMediaReceiverResponse>> GetReceiversAsync(
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ClientConstants.Routes.AirMedia.Receivers,
			Json.AirMediaReceiverResponseArray,
			cancellationToken).ConfigureAwait(false);
	}

	public async Task SendToReceiverAsync(string receiverName, AirMediaReceiverRequest request,
		CancellationToken cancellationToken = default)
	{
		await transport.SendAsync(HttpMethod.Post,
			$"{ClientConstants.Routes.AirMedia.Receivers}{FreeboxApiPath.EncodeSegment(receiverName)}/",
			request, Json.AirMediaReceiverRequest, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Legacy typo route retained in Freebox documentation.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v8-airmedia-receviers-receiver_name-</remarks>
	public async Task SendToLegacyReceiverRouteAsync(string receiverName, AirMediaReceiverRequest request,
		CancellationToken cancellationToken = default)
	{
		await transport.SendAsync(HttpMethod.Post,
			$"{ClientConstants.Routes.AirMedia.LegacyReceiversTypo}{FreeboxApiPath.EncodeSegment(receiverName)}/",
			request, Json.AirMediaReceiverRequest, cancellationToken).ConfigureAwait(false);
	}

	public FreeboxAirMediaReceiver Receiver(string receiverName) => new(this, receiverName);
}
