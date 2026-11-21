namespace Freenaute.Freebox.Client;

public interface IFreeboxAirMediaApi
{
    Task<AirMediaConfigResponse> GetConfigurationAsync(CancellationToken cancellationToken = default);

    Task<AirMediaConfigResponse> UpdateConfigurationAsync(UpdateAirMediaConfigRequest request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AirMediaReceiverResponse>> GetReceiversAsync(CancellationToken cancellationToken = default);

    Task SendToReceiverAsync(string receiverName, AirMediaReceiverRequest request,
        CancellationToken cancellationToken = default);

    FreeboxAirMediaReceiver Receiver(string receiverName);
}
