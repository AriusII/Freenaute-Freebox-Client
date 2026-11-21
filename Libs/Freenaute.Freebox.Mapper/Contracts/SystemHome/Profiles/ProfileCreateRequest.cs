namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Profiles;

/// <summary>Creation fields shown by the official POST example; URL remains distinct from the read model's icon.</summary>
public sealed record ProfileCreateRequest([property: JsonPropertyName("name")] string Name) : ISystemHomeRequest
{
	[JsonPropertyName("url")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Url { get; init; }

	public void Validate() => ArgumentNullException.ThrowIfNull(Name);

	public ProfileCreateRequest WithIconUrl(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return this with { Url = value };
	}
}
