namespace Freenaute.Freebox.Mapper.Contracts.Primitives;

/// <summary>The presence of a JSON field, independent of its scalar value.</summary>
public enum OptionalValueKind : byte
{
    Unset,
    Null,
    Value
}
