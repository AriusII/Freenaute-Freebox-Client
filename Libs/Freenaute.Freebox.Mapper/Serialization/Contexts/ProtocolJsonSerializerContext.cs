namespace Freenaute.Freebox.Mapper.Serialization.Contexts;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	Converters = [typeof(NotificationTargetResultJsonConverter)])]
[JsonSerializable(typeof(Camera))]
[JsonSerializable(typeof(Camera[]))]
[JsonSerializable(typeof(NotificationTarget))]
[JsonSerializable(typeof(NotificationTarget[]))]
[JsonSerializable(typeof(ObjectOrArray<NotificationTarget>))]
[JsonSerializable(typeof(NotificationTargetWrite))]
[JsonSerializable(typeof(NotificationRegistrationCallback))]
[JsonSerializable(typeof(NotificationDeliveryCallback))]
[JsonSerializable(typeof(NotificationDeliveryResult))]
internal sealed partial class ProtocolJsonSerializerContext : JsonSerializerContext;
