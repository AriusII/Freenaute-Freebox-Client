# Freenaute — Freebox Server SDK

Typed .NET 11 libraries for the Freebox Server API, with standard dependency injection, in-memory session coordination,
source-generated JSON, and Native AOT gates.

Packages:

- `Freenaute.Freebox.Client` — HTTP, WebSocket, discovery, authentication, DI, and domain facades.
- `Freenaute.Freebox.Mapper` — typed wire contracts and generated `System.Text.Json` metadata.

[English documentation](docs/en/README.md) · [Documentation française](docs/fr/README.md)

## Important security requirement

Initial pairing always requires the user to **physically approve the application on the Freebox**. The durable
`app_token` belongs in an explicit secure credential store. Session tokens, challenges, and HMAC-derived values remain
memory-only and must never be logged or committed. The built-in in-memory store is for development/tests and is not a
secure persistence fallback.

## Install

Prerequisites: .NET 11 SDK and, for initial pairing, a Freebox Server reachable on the local network.

```bash
dotnet add package Freenaute.Freebox.Client
```

## Minimal dependency injection

```csharp
using Freenaute.Freebox.Client;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddFreeboxClient(options => options
    .UseApplication("org.example.my-app")
    .WithApplicationVersion("1.0.0"));

using var provider = services.BuildServiceProvider();
var coordinator = provider.GetRequiredService<IFreeboxAuthorizationCoordinator>();
var registration = await coordinator.BeginPairingAsync(cancellationToken);

// Tell the user to approve this request physically on the Freebox.
var credentials = await coordinator.WaitForApprovalAsync(cancellationToken: cancellationToken);

var freebox = provider.GetRequiredService<IFreeboxClient>();
var version = await freebox.Discovery.GetApiVersionAsync(cancellationToken);
```

In production, register an `IFreeboxCredentialStore` backed by user-scoped OS protection or an application-owned
encrypted/external secret system. Do not put a real app token in ordinary configuration, environment variables, samples,
or logs. See [authentication and credentials](docs/en/authentication-and-credentials.md).

Named clients are registered by passing a name to `AddFreeboxClient` and resolved through keyed DI. Named clients own
isolated session state. The method returns `IHttpClientBuilder` for handler configuration. Resilience is opt-in;
mutations are not replayed automatically. Details: [dependency injection](docs/en/dependency-injection.md).

## Supported scope

The SDK provides typed discovery/authentication, AirMedia, camera, notification, network, service, file, system, and
modern WebSocket facades. It deliberately excludes RRD, storage/RAID/VM, PVR, VPN, download trackers/peers/blacklist,
and undocumented Home APIs, plus explicitly unstable Wi-Fi fields. See [supported API](docs/en/supported-api.md).

Discovery prefers DNS-SD/mDNS when available and uses HTTP fallback subject to origin, UID, HTTPS, and endpoint
validation. The initial pairing is LAN-only policy; containers, VPNs, firewalls, and multiple interfaces can affect
multicast. See [discovery and networking](docs/en/discovery-and-networking.md).

## Build, test, pack, and AOT

The repository uses `Freenaute.Freebox.slnx`, .NET 11 MTP tests, locked restores, and `.artifacts/` output:

```bash
dotnet restore Freenaute.Freebox.slnx --locked-mode
dotnet build Freenaute.Freebox.slnx -c Release --no-restore -warnaserror
dotnet test --solution Freenaute.Freebox.slnx -c Release --no-build --no-restore

dotnet pack Libs/Freenaute.Freebox.Mapper/Freenaute.Freebox.Mapper.csproj -c Release --no-build --no-restore
dotnet pack Libs/Freenaute.Freebox.Client/Freenaute.Freebox.Client.csproj -c Release --no-build --no-restore
pwsh .github/scripts/Validate-Packages.ps1

# Publish on the matching OS; use win-x64 on Windows.
dotnet restore Tests/Freenaute.Freebox.AotSmoke/Freenaute.Freebox.AotSmoke.csproj --locked-mode
dotnet publish Tests/Freenaute.Freebox.AotSmoke/Freenaute.Freebox.AotSmoke.csproj -c Release -r linux-x64 --no-restore -warnaserror -o .artifacts/aot/linux-x64
.artifacts/aot/linux-x64/Freenaute.Freebox.AotSmoke
```

The libraries remain AnyCPU; Native AOT executables are RID-specific. CI validates managed tests, package
contents/symbols/local-feed consumption, and native smoke artifacts on both `linux-x64` and `win-x64`. No NuGet
publishing occurs automatically.

Official Freebox API documentation is the runtime and protocol authority: <https://dev.freebox.fr/sdk/os/>. More
details: [AOT and packaging](docs/en/aot-and-packaging.md).
