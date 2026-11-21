namespace Freenaute.Freebox.Client;

public interface IFreeboxClient
{
    IFreeboxDiscoveryApi Discovery { get; }
    IFreeboxAuthenticationApi Authentication { get; }
    IFreeboxAirMediaApi AirMedia { get; }
    IFreeboxCamerasApi Cameras { get; }
    IFreeboxNotificationsApi Notifications { get; }
    IFreeboxNetworkApi Network { get; }
    IFreeboxServicesApi Services { get; }
    IFreeboxFilesApi Files { get; }
    IFreeboxSystemHomeApi SystemHome { get; }
    IFreeboxWebSocketsApi WebSockets { get; }

    /// <summary>The typed transport for additional contracts with generated JSON metadata.</summary>
    IFreeboxTransport Transport { get; }

    IFreeboxBinaryTransport BinaryTransport { get; }
    IFreeboxWebSocketTransport WebSocketTransport { get; }
}
