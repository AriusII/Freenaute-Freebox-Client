# Freenaute.Freebox.AotSmoke

NativeAOT smoke harness for the two libraries — **not a test project** (`IsTestProject=false`, `IsPackable=false`).

## What it does

A console app (`Program.cs` → `Smoke.RunAsync()`) that exercises, with **zero network and no real Freebox**:
reflection-disabled JSON round-trips via the generated contexts, DI registration (default + keyed),
the generated options validator, the in-memory credential store, mapper round-trips, domain metadata,
and a scripted auth/discovery/WebSocket session.

9 check groups; prints `AOT smoke passed: 9 check groups` on success.

## Publishing (the real AOT gate)

```bash
dotnet publish Tests/Freenaute.Freebox.AotSmoke -c Release -r linux-x64 -o .artifacts/aot/linux-x64
dotnet publish Tests/Freenaute.Freebox.AotSmoke -c Release -r win-x64   -o .artifacts/aot/win-x64
```

`PublishAot=true`, `RuntimeIdentifiers=linux-x64;win-x64`, `JsonSerializerIsReflectionEnabledByDefault=false`.
CI (`_aot.yml`) asserts the output is a single native binary (ELF on Linux / PE `MZ` on Windows), with **no** `.deps.json`,
`.runtimeconfig.json` or managed `Freenaute.Freebox.*.dll`, runs it without `DOTNET_ROOT`, and records a SHA-256.

> Native linking needs the MSVC/Windows SDK toolchain — run on CI runners (ubuntu-24.04 / windows-2025), not on a Rider-only box.
