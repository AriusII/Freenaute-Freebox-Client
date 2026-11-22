# Freenaute.Freebox.Mapper

[![.NET](https://img.shields.io/badge/.NET-11-512BD4)](https://dotnet.microsoft.com/)

Typed **wire contracts** for the FreeboxOS API (`https://dev.freebox.fr/sdk/os/`), with **source-generated**
`System.Text.Json`
serializer contexts. Zero reflection at runtime → fully **trimming- and NativeAOT-compatible** (`IsAotCompatible`/
`IsTrimmable`).

> **Packaging**: this project is **not published as its own NuGet package** (`IsPackable=false`). Its assembly is
> embedded
> in `lib/net11.0/` of the single published package **`Freenaute.Freebox.Client`**.

## What's inside

- `Contracts/` — DTOs mapped 1:1 to the official API JSON, organized by domain:
    - `Network/` — connection, LAN, DHCP, DDNS, NAT/port-forwarding, IGD, switch, freeplugs, Wi-Fi, …
    - `Files/` — downloads, feeds, download config, file system, shares, upload tracking
    - `Services/` — calls, contacts, players, server configs, voicemail, **parental**
    - `SystemHome/` — system, LCD, ledstrip, standby, profiles, network controls, language, updates
    - `Protocol/`, `WebSockets/` — notifications, WebSocket requests/responses, upload actions
    - `Primitives/` — wire-token types (`EncodedFreeboxPath`, `StringOrInteger`, `ObjectOrArray<T>`, `Optional<T>`…) +
      their source-gen converters
- `ClientSide/` / `ServerSide/` — login/authentication and AirMedia wire DTOs
- `Common/` — `APIResponse` envelopes (`SuccessfulResponse<T>` / `ErrorResponse<T>`), shared types
- `Serialization/Contexts/` — 8 `JsonSerializerContext` classes (Freebox, Shared, Files, Network, Services, Protocol,
  SystemHome, WebSocket)
- `Adapter/` — `FreeboxMapper`: entry point to serialize/deserialize with the combined source-gen resolver

## Conventions

- `namespace` = folder path; every contract type is registered in its domain context via `[JsonSerializable]`.
- `sealed record` / `record struct` wire tokens; `[JsonPropertyName("snake_case")]` exactly as documented.
- Shared protocol literals are centralized in `MapperConstants` (`JsonProperties`, `WebSocketActions`,
  `ServiceWireTokens`,
  `SystemHomeWireTokens`, `BoxModels`) and reused from contracts/converters to keep wire values consistent.
- Enums carry an explicit minimal underlying type (`: byte`); string enums use `[JsonStringEnumMemberName]`-style
  converters.
- **Write-only secrets** (`password`, `key`) are never serialized on output (`JsonIgnoreCondition.WhenWriting` on read
  contracts).
- Paths on the file APIs are base64 (`EncodedFreeboxPath`) and reused verbatim.
- Contracts cover stable + unstable + deprecated + internal Freebox endpoints.
- Unstable contracts are flagged in XML documentation comments.
- Deprecated routes are intended to be surfaced by `.Client` APIs with `[Obsolete]` warnings.

## Usage

```csharp
using Freenaute.Freebox.Mapper.Adapter;

var mapper = new FreeboxMapper();
LanHost host = mapper.FromWire<LanHost>(jsonElement);   // or via a context: NetworkJsonSerializerContext.Default.LanHost
string json = mapper.ToWire(host);
```

You get these contracts automatically via the `Freenaute.Freebox.Client` package; reference this project directly only
if you want the raw contracts without the HTTP client.
