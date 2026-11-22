namespace Freenaute.Freebox.Client.Domains.WebSockets.Events;

/// <summary>The currently supported LAN reachability event names documented by RegisterAction.events.</summary>
public enum FreeboxServerEvent : byte
{
	LanHostL3AddressReachable,
	LanHostL3AddressUnreachable
}
