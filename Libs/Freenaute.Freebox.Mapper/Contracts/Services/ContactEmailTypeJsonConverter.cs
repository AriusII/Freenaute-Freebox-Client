namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class ContactEmailTypeJsonConverter : JsonConverter<ContactEmailType>
{
	public override ContactEmailType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return reader.TokenType is JsonTokenType.String
			? new ContactEmailType(reader.GetString()!)
			: throw new JsonException("Expected a string for ContactEmailType.");
	}

	public override void Write(Utf8JsonWriter writer, ContactEmailType value, JsonSerializerOptions options)
	{
		if (value.Value is null)
		{
			throw new JsonException("An uninitialized wire value cannot be serialized.");
		}

		writer.WriteStringValue(value.Value);
	}
}
