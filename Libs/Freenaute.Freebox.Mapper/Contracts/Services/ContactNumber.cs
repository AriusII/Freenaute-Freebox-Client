namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented ContactNumber wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#ContactNumber" />
public sealed record ContactNumber
{
    [JsonPropertyName("id")] public long? Id { get; init; }

    [JsonPropertyName("contact_id")] public long? ContactId { get; init; }

    [JsonPropertyName("type")] public ContactNumberType? Type { get; init; }

    [JsonPropertyName("number")] public string? Number { get; init; }

    [JsonPropertyName("is_default")] public bool? IsDefault { get; init; }

    [JsonPropertyName("is_own")] public bool? IsOwn { get; init; }

    public override string ToString()
    {
        return nameof(ContactNumber);
    }
}
