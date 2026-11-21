# Freenaute.Freebox.Client

[![NuGet](https://img.shields.io/nuget/v/Freenaute.Freebox.Client)](https://www.nuget.org/packages/Freenaute.Freebox.Client)
[![.NET](https://img.shields.io/badge/.NET-11-512BD4)](https://dotnet.microsoft.com/)
[![Platforms](https://img.shields.io/badge/platforms-win--x64%20%7C%20linux--x64-blue)]()

DI-friendly, **NativeAOT-compatible** .NET 11 client for the FreeboxOS API. Pure managed single `lib/net11.0` package —
works as-is on **Windows x64 and Linux x64**.

## Registration (DI)

```csharp
services.AddFreeboxClient(options => options
    .UseApplication("org.example.my-app")
    .WithApplicationVersion("1.0.0")
    .UseCredentialStore(mySecureStore)          // optional; in-memory store by default (dev/tests only)
    .ConfigureHttpClient(...)                    // optional IHttpClientBuilder customization
);

// Named/isolated client:
services.AddFreeboxClient("secondary", options => options.UseApplication("org.example.other"));
var other = provider.GetRequiredKeyedService<IFreeboxClient>("secondary");
```

Registers: `IFreeboxClient`, all domain facades (`Discovery`, `Connection`, `Lan`, `Dhcp`, `Nat`, `Igd`, `Switch`,
`Freeplugs`, `Wifi`, `Files`, `Downloads`, `Services`, `SystemHome`, `WebSockets`, `AirMedia`, `Parental`, …),
`IFreeboxAuthorizationCoordinator`, `IFreeboxCredentialStore`, `IFreeboxDiscoveryResolver`, and the three transports.
Options are validated on start (`FreeboxClientOptionsValidator`, source-generated).

## Architecture

- Root public surface only: `FreeboxClient`, `IFreeboxClient`, `FreeboxClientOptions`, `FreeboxServiceCollectionExtensions`,
  the 3 public exceptions. Everything else is `internal`.
- `Http/Transport/` — split by concern, no `partial`:
  - `FreeboxHttpTransportCore` — shared session/discovery/envelope/error-mapping plumbing
  - `FreeboxHttpTransport` (`IFreeboxTransport`) — JSON requests
  - `Binary/FreeboxBinaryTransport` (`IFreeboxBinaryTransport`) — binary downloads (`GET /dl/{path}`)
  - `WebSockets/FreeboxWebSocketTransport` (`IFreeboxWebSocketTransport`) + `FreeboxWebSocketConnection` — events & WS upload
- `Authentication/`, `Credentials/` — pairing flow, HMAC-SHA1 session, secure-store abstraction.
- `Discovery/` (+`Mdns/`) — DNS-SD `_fbx-api._tcp` with TXT validation, HTTP fallback.
- `Http/Tls/` — pins/validates the Freebox Root CA and Freebox ECC Root CA chain.
- `Domains/<Domain>/` — one `IFreebox*Api` facade + implementation per API domain; `namespace` = folder path.

## Behavior guarantees

- Auth header `X-Fbx-App-Auth` on every authenticated call; automatic session renewal; 403 error codes mapped to
  `FreeboxAuthenticationException`/`FreeboxAuthorizationException`, other API errors to `FreeboxApiException`.
- Resilience via `Microsoft.Extensions.Http.Resilience` (opt-in; transient GETs retried, mutations never replayed).
- All JSON goes through the Mapper's source-generated contexts (`JsonSerializerIsReflectionEnabledByDefault=false` in tests/smoke).
- Stable, documented endpoints only — no unstable or deprecated APIs, no `[Obsolete]` members.

## Security

Pairing requires physical approval on the Freebox LCD. Store the `app_token` only in an `IFreeboxCredentialStore` backed by
OS-protected/encrypted storage; never in config, env vars, logs or source. Session tokens/challenges/HMAC values are memory-only.
