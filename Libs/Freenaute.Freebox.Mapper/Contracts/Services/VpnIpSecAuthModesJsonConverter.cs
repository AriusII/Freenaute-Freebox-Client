namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class VpnIpSecAuthModesJsonConverter : JsonConverter<VpnIpSecAuthModes>
{
    public override VpnIpSecAuthModes Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.StartObject => VpnIpSecAuthModes.FromMap(
                JsonSerializer.Deserialize(ref reader, ServicesJsonSerializerContext.Default.VpnAuthModeMap)!),
            JsonTokenType.StartArray => VpnIpSecAuthModes.FromArray(JsonSerializer.Deserialize(ref reader,
                ServicesJsonSerializerContext.Default.VpnIpSecAuthModeArray)!),
            _ => throw new JsonException("Expected a named authentication-mode map or an array.")
        };
    }

    public override void Write(Utf8JsonWriter writer, VpnIpSecAuthModes value, JsonSerializerOptions options)
    {
        switch (value.Kind)
        {
            case VpnIpSecAuthModesKind.Map:
                JsonSerializer.Serialize(writer, value.WireMap,
                    ServicesJsonSerializerContext.Default.VpnAuthModeMap); break;
            case VpnIpSecAuthModesKind.Array:
                JsonSerializer.Serialize(writer, value.WireItems,
                    ServicesJsonSerializerContext.Default.VpnIpSecAuthModeArray); break;
            default: throw new JsonException("Uninitialized authentication modes cannot be serialized.");
        }
    }
}
