namespace Freenaute.Freebox.Mapper.Contracts.Services.Parental;

public sealed class ParentalFilterStateJsonConverter : JsonConverter<ParentalFilterState>
{
	public override ParentalFilterState Read(ref Utf8JsonReader reader, Type typeToConvert,
		JsonSerializerOptions options)
	{
		return reader.TokenType is JsonTokenType.String
			? new ParentalFilterState(reader.GetString()!)
			: throw new JsonException("Expected a string for ParentalFilterState.");
	}

	public override void Write(Utf8JsonWriter writer, ParentalFilterState value, JsonSerializerOptions options)
	{
		if (value.Value is null)
		{
			throw new JsonException(MapperConstants.ExceptionMessages.JsonConverters.UninitializedWireValueCannotBeSerialized);
		}

		writer.WriteStringValue(value.Value);
	}
}
