namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented CallAccount wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#get--api-v10-call-account" />
public sealed record CallAccount
{
    [JsonPropertyName("phone_number")] public string? PhoneNumber { get; init; }

    public override string ToString()
    {
        return nameof(CallAccount);
    }
}
