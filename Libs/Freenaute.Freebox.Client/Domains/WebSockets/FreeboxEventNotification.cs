namespace Freenaute.Freebox.Client.Domains.WebSockets;

/// <summary>Notification envelope with typed LAN payload access and an extensible JSON result.</summary>
public sealed class FreeboxEventNotification
{
	private static readonly IFreeboxMapper Mapper = FreeboxMapper.Default;
	private readonly FreeboxWebSocketMessage _message;

	internal FreeboxEventNotification(FreeboxWebSocketMessage message)
	{
		_message = message;
	}

	public string? Source => _message.Source;
	public string? Event => _message.Event;
	public JsonElement? Result => _message.Result;

	public LanHost? GetLanHost()
	{
		return Source + "_" + Event is "lan_host_l3addr_reachable" or "lan_host_l3addr_unreachable"
			? DeserializeResult(NetworkJsonSerializerContext.Default.LanHost)
			: null;
	}

	public T? DeserializeResult<T>(JsonTypeInfo<T> typeInfo) where T : class
	{
		ArgumentNullException.ThrowIfNull(typeInfo);
		return Result is { } result ? Mapper.FromWire(result, typeInfo) : null;
	}
}
