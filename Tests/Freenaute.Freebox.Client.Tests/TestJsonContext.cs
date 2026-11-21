namespace Freenaute.Freebox.Client.Tests;

[JsonSerializable(typeof(FixtureCommand))]
[JsonSerializable(typeof(FixtureResult))]
internal partial class TestJsonContext : JsonSerializerContext;
