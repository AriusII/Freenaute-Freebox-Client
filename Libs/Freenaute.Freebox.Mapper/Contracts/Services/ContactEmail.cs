namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented ContactEmail wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#ContactEmail" />
public sealed record ContactEmail
{
    [JsonPropertyName("id")] public long? Id { get; init; }

    [JsonPropertyName("contact_id")] public long? ContactId { get; init; }

    [JsonPropertyName("type")] public ContactEmailType? Type { get; init; }

    [JsonPropertyName("email")] public string? Email { get; init; }

    public override string ToString()
    {
        return nameof(ContactEmail);
    }
}
