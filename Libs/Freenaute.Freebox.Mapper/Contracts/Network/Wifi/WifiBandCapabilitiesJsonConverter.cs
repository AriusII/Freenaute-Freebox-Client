namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

public sealed class WifiBandCapabilitiesJsonConverter : JsonConverter<WifiBandCapabilities>
{
	public override bool HandleNull => true;

	public override WifiBandCapabilities Read(ref Utf8JsonReader reader, Type typeToConvert,
		JsonSerializerOptions options)
	{
		if (reader.TokenType is JsonTokenType.Number && reader.TryGetInt64(out long integer))
		{
			return WifiBandCapabilities.FromInteger(integer);
		}

		if (reader.TokenType is not JsonTokenType.StartObject)
		{
			throw new JsonException("Expected an integer or a map of boolean WiFi capabilities.");
		}

		var flags = new Dictionary<string, bool>(StringComparer.Ordinal);
		while (reader.Read())
		{
			if (reader.TokenType is JsonTokenType.EndObject)
			{
				return WifiBandCapabilities.FromFlags(flags);
			}

			if (reader.TokenType is not JsonTokenType.PropertyName)
			{
				throw new JsonException("Expected a WiFi capability name.");
			}

			string name = reader.GetString()!;
			if (!reader.Read() || reader.TokenType is not (JsonTokenType.True or JsonTokenType.False))
			{
				throw new JsonException("WiFi capability map values must be booleans.");
			}

			flags[name] = reader.GetBoolean();
		}

		throw new JsonException("Incomplete WiFi capability map.");
	}

	public override void Write(Utf8JsonWriter writer, WifiBandCapabilities value, JsonSerializerOptions options)
	{
		if (value.Kind is WifiBandCapabilitiesKind.Integer)
		{
			writer.WriteNumberValue(value.Integer);
			return;
		}

		if (value.Kind is not WifiBandCapabilitiesKind.BooleanMap)
		{
			throw new JsonException("Uninitialized WiFi capabilities cannot be written.");
		}

		writer.WriteStartObject();
		foreach ((string key, bool enabled) in value.Flags)
		{
			writer.WriteBoolean(key, enabled);
		}

		writer.WriteEndObject();
	}
}
