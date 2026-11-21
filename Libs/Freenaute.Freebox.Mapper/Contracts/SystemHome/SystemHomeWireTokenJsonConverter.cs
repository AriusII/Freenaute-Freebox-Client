namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed class SystemHomeWireTokenJsonConverter<TToken> : JsonConverter<TToken>
    where TToken : struct, ISystemHomeWireToken<TToken>
{
    public override TToken Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? TToken.FromWire(reader.GetString()!)
            : throw new JsonException("A system/home token must be a string.");
    }

    public override void Write(Utf8JsonWriter writer, TToken value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An undefined token cannot be written.");
        }

        writer.WriteStringValue(value.Value);
    }
}
