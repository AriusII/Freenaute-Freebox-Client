namespace Freenaute.Freebox.Client.Tests;

[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    Converters =
    [
        typeof(OptionalBooleanJsonConverter), typeof(OptionalInt32JsonConverter),
        typeof(OptionalInt64JsonConverter), typeof(OptionalStringJsonConverter), typeof(OptionalDecimalJsonConverter),
        typeof(ObjectOrArrayJsonConverter<PrimitiveItem>), typeof(ObjectOrArrayJsonConverter<int>)
    ])]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(decimal))]
[JsonSerializable(typeof(int[]))]
[JsonSerializable(typeof(PrimitivePatch))]
[JsonSerializable(typeof(PrimitiveNestedPatch))]
[JsonSerializable(typeof(PrimitiveItem))]
[JsonSerializable(typeof(ObjectOrArray<PrimitiveItem>), TypeInfoPropertyName = "ItemObjectOrArray")]
[JsonSerializable(typeof(ObjectOrArray<int>), TypeInfoPropertyName = "IntegerObjectOrArray")]
internal sealed partial class PrimitiveTestJsonContext : JsonSerializerContext;
