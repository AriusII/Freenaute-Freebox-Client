namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class ContactNumberTypeJsonConverter : JsonConverter<ContactNumberType>
{
    public override ContactNumberType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new ContactNumberType(reader.GetString()!)
            : throw new JsonException("Expected a string for ContactNumberType.");
    }

    public override void Write(Utf8JsonWriter writer, ContactNumberType value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
