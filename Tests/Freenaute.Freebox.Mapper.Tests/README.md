# Freenaute.Freebox.Mapper.Tests

Wire-contract tests for `Freenaute.Freebox.Mapper` — **xUnit v3** (MTP), reflection-based JSON disabled.

## Approach

Round-trip tests per domain (`*ContractTests.cs`): deserialize a realistic wire JSON sample (from the official SDK docs)
→ re-serialize through the domain's source-generated `JsonSerializerContext` → assert **byte-identical** output.
This catches: wrong `[JsonPropertyName]`, lost enum wire values, unions losing their kind, read-only/write-only fields
being emitted, and polymorphic shapes (e.g. `ObjectOrArray<T>`, `StringOrInteger`, wifi capabilities bool-map vs bitmask,
`max_use_count` string-vs-int).

Also covers the custom converters (`ConverterRoundTripTests`) and the `FreeboxMapper` adapter
(options frozen, cached generated metadata, unregistered-type errors).

Run: `dotnet test Tests/Freenaute.Freebox.Mapper.Tests -c Release`
