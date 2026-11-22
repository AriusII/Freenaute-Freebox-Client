namespace Freenaute.Freebox.Mapper.Contracts.Services.Calls;

/// <summary>Exact wire values documented for CallEntry.type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#CallEntry.type" />
[JsonConverter(typeof(CallTypeJsonConverter))]
public readonly record struct CallType(string Value)
{
	public static CallType Missed => new(MapperConstants.ServiceWireTokens.Calls.Missed);
	public static CallType Accepted => new(MapperConstants.ServiceWireTokens.Calls.Accepted);
	public static CallType Outgoing => new(MapperConstants.ServiceWireTokens.Calls.Outgoing);

	public override string ToString() => Value ?? string.Empty;
}
