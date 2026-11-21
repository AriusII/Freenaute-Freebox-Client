# Dependency injection

## Registration

`AddFreeboxClient(Action<FreeboxClientOptions>)` registers the default client. The overload accepting `IConfiguration` binds configuration using generated configuration metadata. The action-plus-`validateOnStart` overload enables startup validation. Named clients use `AddFreeboxClient(string name, ...)` and are resolved with keyed dependency-injection APIs.

Options include the server origin, optional discovered API-major override, base path, timeout, application identity, display names, credential profile, automatic authentication, HTTP client name, and opt-in resilience. Keep one `HttpClientName` unique per client unless sharing it is deliberate.

The container exposes `IFreeboxClient`, `IFreeboxAuthorizationCoordinator`, each public sub-facade, `IFreeboxTransport`, `IFreeboxBinaryTransport`, and `IFreeboxWebSocketTransport`. Named registrations expose keyed equivalents. The default and keyed facades share the session/state owned by their client registration.

## Stores and lifecycle

`TryAddSingleton` provides the in-memory credential store only when the application has not supplied `IFreeboxCredentialStore`. Replace it during production registration with an available, thread-safe secure store. Stores must be compatible with the lifetime of the singleton client state; do not capture a scoped store from a singleton coordinator.

## Handlers and resilience

`AddFreeboxClient` returns `IHttpClientBuilder`, so consumers can configure the client, primary handler, and additional handlers with the standard Microsoft Extensions APIs. Keep certificate, origin, redirect, and secret-redaction behavior intact.

Resilience is opt-in. Mutations and operations with documented side effects are not replayed automatically; a caller must choose retry semantics after considering idempotency. Network reads may opt in only through the supported resilience configuration.

## Configuration and AOT

All binding uses generated configuration metadata and the generated options validator. Invalid origins, non-positive explicit API majors, invalid timeouts, and invalid credential identifiers fail as options validation errors rather than reflection-based binding failures at runtime. Keep token material out of ordinary configuration because configuration providers are not secure credential stores.
