namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed record SetLanguageRequest([property: JsonPropertyName("lang")] string Language) : ISystemHomeRequest
{
    public void Validate()
    {
        ArgumentNullException.ThrowIfNull(Language);
        if (Language.Length != 3 || Language.Any(static c => !char.IsAsciiLetter(c)))
        {
            throw new ArgumentException("Use the ISO 639-3 language code advertised by the server.", nameof(Language));
        }
    }
}
