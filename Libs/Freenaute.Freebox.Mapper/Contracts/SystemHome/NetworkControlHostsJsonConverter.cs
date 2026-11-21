namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed class NetworkControlHostsJsonConverter : JsonConverter<NetworkControlHosts>
{
    public override NetworkControlHosts Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Network-control hosts must be an array.");
        }

        var names = ImmutableArray.CreateBuilder<string>();
        var hosts = ImmutableArray.CreateBuilder<LanHost>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return hosts.Count > 0 ? NetworkControlHosts.FromHosts(hosts) : NetworkControlHosts.FromNames(names);
            }

            if (reader.TokenType == JsonTokenType.String && hosts.Count == 0)
            {
                names.Add(reader.GetString()!);
            }
            else if (reader.TokenType == JsonTokenType.StartObject && names.Count == 0)
            {
                hosts.Add(JsonSerializer.Deserialize(ref reader, NetworkJsonSerializerContext.Default.LanHost)
                          ?? throw new JsonException("LAN host cannot be null."));
            }
            else
            {
                throw new JsonException("Network-control hosts cannot mix names and host objects.");
            }
        }

        throw new JsonException("Unterminated hosts array.");
    }

    public override void Write(Utf8JsonWriter writer, NetworkControlHosts value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (string name in value.Names)
        {
            writer.WriteStringValue(name);
        }

        foreach (var host in value.Hosts)
        {
            JsonSerializer.Serialize(writer, host, NetworkJsonSerializerContext.Default.LanHost);
        }

        writer.WriteEndArray();
    }
}
