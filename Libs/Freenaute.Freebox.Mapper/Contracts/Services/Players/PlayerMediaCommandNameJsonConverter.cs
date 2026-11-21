namespace Freenaute.Freebox.Mapper.Contracts.Services.Players;

public sealed class PlayerMediaCommandNameJsonConverter : JsonConverter<PlayerMediaCommandName>
{
	public override PlayerMediaCommandName Read(ref Utf8JsonReader reader, Type typeToConvert,
		JsonSerializerOptions options)
	{
		return reader.TokenType is JsonTokenType.String
			? new PlayerMediaCommandName(reader.GetString()!)
			: throw new JsonException("Expected a string for PlayerMediaCommandName.");
	}

	public override void Write(Utf8JsonWriter writer, PlayerMediaCommandName value, JsonSerializerOptions options)
	{
		if (value.Value is null)
		{
			throw new JsonException("An uninitialized wire value cannot be serialized.");
		}

		writer.WriteStringValue(value.Value);
	}
}
