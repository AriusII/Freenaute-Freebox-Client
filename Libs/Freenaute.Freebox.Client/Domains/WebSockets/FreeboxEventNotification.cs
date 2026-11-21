namespace Freenaute.Freebox.Client.Domains.WebSockets;

/// <summary>Notification envelope with typed access to established VM/LAN payloads and an extensible JSON result.</summary>
public sealed class FreeboxEventNotification
{
    private readonly FreeboxWebSocketMessage _message;

    internal FreeboxEventNotification(FreeboxWebSocketMessage message)
    {
        _message = message;
    }

    public string? Source => _message.Source;
    public string? Event => _message.Event;
    public JsonElement? Result => _message.Result;

    public VmStateChange? GetVmStateChange()
    {
        return Is("vm", "state_changed")
            ? DeserializeResult(FilesJsonSerializerContext.Default.VmStateChange)
            : null;
    }

    public VmDiskTask? GetVmDiskTask()
    {
        return Is("vm", "disk_task_done")
            ? DeserializeResult(FilesJsonSerializerContext.Default.VmDiskTask)
            : null;
    }

    public LanHost? GetLanHost()
    {
        return Source + "_" + Event is "lan_host_l3addr_reachable" or "lan_host_l3addr_unreachable"
            ? DeserializeResult(NetworkJsonSerializerContext.Default.LanHost)
            : null;
    }

    public T? DeserializeResult<T>(JsonTypeInfo<T> typeInfo) where T : class
    {
        ArgumentNullException.ThrowIfNull(typeInfo);
        return Result?.Deserialize(typeInfo);
    }

    private bool Is(string source, string eventName)
    {
        return Source == source && Event == eventName;
    }
}
