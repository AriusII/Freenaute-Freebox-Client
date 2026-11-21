namespace Freenaute.Freebox.Client;

public interface IFreeboxClient
{
	IFreeboxDiscoveryApi Discovery { get; }
	IFreeboxAuthenticationApi Authentication { get; }
	IFreeboxAirMediaApi AirMedia { get; }
	IFreeboxCamerasApi Cameras { get; }
	IFreeboxNotificationsApi Notifications { get; }
	IFreeboxNetworkApi Network { get; }
	IFreeboxParentalApi Parental { get; }
	IFreeboxServicesApi Services { get; }
	IFreeboxFilesApi Files { get; }
	IFreeboxSystemHomeApi SystemHome { get; }
	IFreeboxWebSocketsApi WebSockets { get; }
	IFreeboxTransport Transport { get; }
	IFreeboxBinaryTransport BinaryTransport { get; }
	IFreeboxWebSocketTransport WebSocketTransport { get; }
}
