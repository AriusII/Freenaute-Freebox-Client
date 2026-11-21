# Freenaute.Freebox.Client.Tests

Unit/integration tests for `Freenaute.Freebox.Client` — **xUnit v3** on Microsoft.Testing.Platform (`dotnet test`),
self-executing `OutputType=Exe`, with `JsonSerializerIsReflectionEnabledByDefault=false` (source-gen JSON only, like AOT).

## Approach

- **No mocking framework** (AOT-friendly): hand-rolled `FixtureHandler : HttpMessageHandler` returning scripted
  `HttpResponseMessage`s and capturing every request (`CapturedRequest`: method, URI, `X-Fbx-App-Auth` header, body).
  Factories: `FixtureHandler.Constant`, `FixtureHandler.Authenticated` (auto-answers `/api_version`, `/login/`, `/login/session/`).
- WebSockets: `ScriptedSocket` / `SocketFixtureTransport` / `SocketFrame` in-memory scripted socket + an `UpgradeHandler` for handshakes.
- DI tests use keyed services (`AddFreeboxClient("name", …)`) and `ConfigurePrimaryHttpMessageHandler(() => handler)`.

## Coverage

Auth/pairing flow (HMAC session, renewal, logout, error codes), credentials coordinator, discovery (mDNS + HTTP fallback),
HTTP transport (path-escape rejection, error mapping, cancellation, timeouts, binary downloads, multipart),
TLS pinning, options validation, DI isolation (keyed clients), resilience, and one `*ApiTests` class per domain
(connection, ddns, dhcp, dhcpv6, nat, lan, igd, switch, freeplugs, wifi incl. mac_filter/wps, ftp, netshare, upnpav,
ledstrip, language, profiles, files, downloads, services, parental, protocol, airmedia, websockets…).

Run: `dotnet test Tests/Freenaute.Freebox.Client.Tests -c Release`
