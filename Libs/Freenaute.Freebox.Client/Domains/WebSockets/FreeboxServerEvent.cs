namespace Freenaute.Freebox.Client.Domains.WebSockets;

/// <summary>The four event names documented by RegisterAction.events.</summary>
public enum FreeboxServerEvent
{
    VmStateChanged,
    VmDiskTaskDone,
    LanHostL3AddressReachable,
    LanHostL3AddressUnreachable
}
