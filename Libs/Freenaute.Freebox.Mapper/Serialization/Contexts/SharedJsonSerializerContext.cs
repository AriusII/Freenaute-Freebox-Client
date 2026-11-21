namespace Freenaute.Freebox.Mapper.Serialization.Contexts;

/// <summary>Generated metadata for primitive contracts; domains register their explicitly closed model converters.</summary>
[JsonSourceGenerationOptions(Converters =
[
	typeof(OptionalBooleanJsonConverter), typeof(OptionalInt32JsonConverter), typeof(OptionalInt64JsonConverter),
	typeof(OptionalStringJsonConverter), typeof(OptionalDecimalJsonConverter)
])]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(decimal))]
[JsonSerializable(typeof(Optional<bool>), TypeInfoPropertyName = "OptionalBoolean")]
[JsonSerializable(typeof(Optional<int>), TypeInfoPropertyName = "OptionalInt32")]
[JsonSerializable(typeof(Optional<long>), TypeInfoPropertyName = "OptionalInt64")]
[JsonSerializable(typeof(Optional<string>), TypeInfoPropertyName = "OptionalString")]
[JsonSerializable(typeof(Optional<decimal>), TypeInfoPropertyName = "OptionalDecimal")]
[JsonSerializable(typeof(StringOrInteger))]
[JsonSerializable(typeof(EncodedFreeboxPath))]
public sealed partial class SharedJsonSerializerContext : JsonSerializerContext;
