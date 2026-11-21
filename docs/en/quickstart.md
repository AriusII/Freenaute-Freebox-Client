# Quick start

## Prerequisites

- .NET 11 SDK or a compatible consuming application on Windows or Linux x64.
- A Freebox Server reachable on the local network for initial pairing.
- Physical access to approve the application on the Freebox.

Install the client package (Mapper is installed as its dependency):

```bash
dotnet add package Freenaute.Freebox.Client
```

Register a default client:

```csharp
services.AddFreeboxClient(options => options
    .UseApplication("org.example.my-app")
    .WithApplicationVersion("1.0.0"));
```

Add a second identity with a named client by passing a client name as the first argument to `AddFreeboxClient`. Do not reuse the same credential profile unless the clients are meant to share one Freebox/application identity.

## Pair once

Resolve `IFreeboxAuthorizationCoordinator`, call `BeginPairingAsync`, and immediately instruct the user to approve the request physically on the Freebox. Then call `WaitForApprovalAsync` with an application cancellation token.

The built-in in-memory store is intended for development and is lost at process exit. Production applications must register an explicit secure implementation of `IFreeboxCredentialStore` before constructing the client. Never fall back to plaintext files, environment variables, or user secrets for a persistent `app_token`.

After approval, authenticated calls reuse the stored application token. The session token is generated in memory and is never persisted.

## Use the client

Resolve `IFreeboxClient` (or its keyed named client), then use a facade such as `Discovery`, `Network`, `Files`, or `WebSockets`. Pass cancellation tokens for network operations. `Authentication.CloseSessionAsync` logs out the in-memory session without deleting the reusable app token; coordinator `ForgetAsync` deletes local credentials. Remote revocation is a separate administrative action when supported by the Freebox configuration.

Continue with [authentication and credentials](authentication-and-credentials.md) and [discovery and networking](discovery-and-networking.md).
