namespace Freenaute.Freebox.Mapper.Contracts.Files;

public sealed class AddedDownloadsJsonConverter : JsonConverter<AddedDownloads>
{
	public override AddedDownloads Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType is not JsonTokenType.StartObject)
		{
			throw new JsonException("Expected a download-add result object.");
		}

		List<long>? ids = null;
		bool multiple = false;
		while (reader.Read() && reader.TokenType is not JsonTokenType.EndObject)
		{
			if (reader.TokenType is not JsonTokenType.PropertyName)
			{
				throw new JsonException();
			}

			bool isId = reader.ValueTextEquals("id"u8);
			if (!reader.Read())
			{
				throw new JsonException();
			}

			if (!isId)
			{
				reader.Skip();
				continue;
			}

			if (ids is not null)
			{
				throw new JsonException("Duplicate download-add identifier.");
			}

			ids = [];
			switch (reader.TokenType)
			{
				case JsonTokenType.Number:
					ids.Add(ReadId(ref reader));
					break;
				case JsonTokenType.StartArray:
				{
					multiple = true;
					while (reader.Read() && reader.TokenType is not JsonTokenType.EndArray)
					{
						if (reader.TokenType is not JsonTokenType.Number)
						{
							throw new JsonException("Expected an integer download identifier.");
						}

						ids.Add(ReadId(ref reader));
					}

					if (reader.TokenType is not JsonTokenType.EndArray)
					{
						throw new JsonException();
					}

					break;
				}
				case JsonTokenType.None:
				case JsonTokenType.StartObject:
				case JsonTokenType.EndObject:
				case JsonTokenType.EndArray:
				case JsonTokenType.PropertyName:
				case JsonTokenType.Comment:
				case JsonTokenType.String:
				case JsonTokenType.True:
				case JsonTokenType.False:
				case JsonTokenType.Null:
					break;
				default:
					throw new JsonException("Expected one integer or an array of download identifiers.");
			}
		}

		if (reader.TokenType is not JsonTokenType.EndObject || ids is null)
		{
			throw new JsonException("Missing download-add identifier.");
		}

		return new AddedDownloads(ids.AsReadOnly(), multiple);
	}

	private static long ReadId(ref Utf8JsonReader reader)
	{
		return reader.TryGetInt64(out long id)
			? id
			: throw new JsonException("Expected a signed 64-bit integer download identifier.");
	}

	public override void Write(Utf8JsonWriter writer, AddedDownloads value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("id");
		if (value.WasMultiple)
		{
			writer.WriteStartArray();
			foreach (long id in value.Ids)
			{
				writer.WriteNumberValue(id);
			}

			writer.WriteEndArray();
		}
		else
		{
			if (value.Ids.Count is not 1)
			{
				throw new JsonException("A scalar result must contain exactly one identifier.");
			}

			writer.WriteNumberValue(value.Ids[0]);
		}

		writer.WriteEndObject();
	}
}
