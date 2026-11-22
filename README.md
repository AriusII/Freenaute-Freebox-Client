<p align="center">
  <img src="logo.svg" alt="Freenaute logo" width="120"/>
</p>

<h1 align="center">Freenaute — Freebox Server SDK</h1>

<p align="center">
  Typed <b>C# 15 / .NET 11</b> client for the <a href="https://dev.freebox.fr/sdk/os/">FreeboxOS API</a> —<br/>
  <b>dependency-injection friendly</b>, <b>Native AOT compatible</b>, one clean NuGet package for <b>Windows x64 &amp; Linux x64</b>.
</p>

<p align="center">
  <a href="https://github.com/AriusII/Freenaute-Freebox-Api/actions/workflows/release.yml"><img src="https://github.com/AriusII/Freenaute-Freebox-Api/actions/workflows/release.yml/badge.svg" alt="Release"/></a>
  <a href="https://www.nuget.org/packages/Freenaute.Freebox.Client"><img src="https://img.shields.io/nuget/v/Freenaute.Freebox.Client?color=blue" alt="NuGet"/></a>
  <a href="https://www.nuget.org/packages/Freenaute.Freebox.Client"><img src="https://img.shields.io/nuget/dt/Freenaute.Freebox.Client" alt="Downloads"/></a>
  <img src="https://img.shields.io/badge/.NET-11-512BD4" alt=".NET 11"/>
  <img src="https://img.shields.io/badge/platforms-win--x64%20%7C%20linux--x64-blue" alt="Platforms"/>
</p>

---

## ✨ What is it?

Freenaute is a **strongly typed .NET client** for the Freebox Server (FreeboxOS) REST & WebSocket API:
discovery, pairing/authentication, and typed facades for network, Wi-Fi, files, downloads, calls, contacts,
AirMedia, parental control, system… — all through standard `Microsoft.Extensions.DependencyInjection`.

| ✨ Feature               | Detail                                                                                                    |
|--------------------------|-----------------------------------------------------------------------------------------------------------|
| 💉 DI-native             | `services.AddFreeboxClient(...)`, named/keyed clients, options validation on start                        |
| 🚀 AOT-ready             | `IsAotCompatible`/`IsTrimmable`, source-generated JSON only (zero reflection), NativeAOT smoke gate in CI |
| 🔌 Transports split      | JSON `IFreeboxTransport`, binary `IFreeboxBinaryTransport`, WebSocket `IFreeboxWebSocketTransport`        |
| 🔍 Discovery             | mDNS/DNS-SD `_fbx-api._tcp` with validation, HTTP fallback                                                |
| 🔐 Auth done right       | LCD pairing → `app_token` in a secure `IFreeboxCredentialStore`, HMAC-SHA1 sessions, `X-Fbx-App-Auth`     |
| 🧩 One package           | `Freenaute.Freebox.Client` embeds the Mapper contracts — nothing else to install                          |
| 🎯 Policy-aware coverage | stable APIs plus XML-documented unstable APIs and `[Obsolete]`-marked deprecated APIs                     |

## ✅ API model reconciliation (wave2)

From `docs/api-model.json` against `.Client` + `.Mapper`:

- **Operations**: **222 / 222** documented operations covered (**100%**)
- **Schemas**: **120 / 120** component schemas covered (**100%**)
- Coverage includes 7 documented alias mappings (contact bracket aliases + parental planning resource indirection).

Machine-readable evidence:  
`C:\Users\Arius-dev\.copilot\session-state\a6a9000b-eb02-48ef-a676-4fc1cb95fc0e\files\wave2-global-matrix-docs-coverage.json`

## 📦 Install

```bash
dotnet add package Freenaute.Freebox.Client
```

or in your `.csproj`:

```xml
<PackageReference Include="Freenaute.Freebox.Client" Version="1.0.0" />
```

## 🚀 Quick start

```csharp
using Freenaute.Freebox.Client;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddFreeboxClient(o => o
    .UseApplication("org.example.my-app")
    .WithApplicationVersion("1.0.0"));

using var provider = services.BuildServiceProvider();

// Pairing: the user approves the app once, physically, on the Freebox LCD.
var auth = provider.GetRequiredService<IFreeboxAuthorizationCoordinator>();
await auth.BeginPairingAsync(cancellationToken);
var creds = await auth.WaitForApprovalAsync(cancellationToken: cancellationToken);

var freebox = provider.GetRequiredService<IFreeboxClient>();
var status = await freebox.Connection.GetStatusAsync(cancellationToken);
var hosts  = await freebox.Lan.GetHostsAsync("eth0", cancellationToken);
```

> ⚠️ **Security**: the durable `app_token` must live in an OS-protected/encrypted `IFreeboxCredentialStore` —
> never in config files, environment variables, logs or source. Session tokens stay memory-only.

## 📚 Documentation

- 🇬🇧 [English docs](docs/en/README.md) — quickstart, authentication, DI, discovery, supported API, AOT & packaging
- 🇫🇷 [Documentation française](docs/fr/README.md) — démarrage rapide, authentification, DI, découverte, API supportées,
  AOT & packaging

## 🗂️ Repository layout

| Path                                                                       | What                                                                                                          |
|----------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------|
| [`Libs/Freenaute.Freebox.Client`](Libs/Freenaute.Freebox.Client/README.md) | the published package: client, auth, discovery, transports, domain facades                                    |
| [`Libs/Freenaute.Freebox.Mapper`](Libs/Freenaute.Freebox.Mapper/README.md) | wire contracts + source-gen JSON (embedded in the package)                                                    |
| [`Tests/`](Tests)                                                          | xUnit v3 tests + NativeAOT smoke (`Client.Tests`, `Mapper.Tests`, `AotSmoke`)                                 |
| `.github/workflows/`                                                       | `pr.yml` (tests on MR) · `release.yml` (pack + NuGet on main/tags) · `_build-test.yml` / `_aot.yml` templates |

Build outputs are gitignored under `.artifacts/` (`build/`, `nuget/`, `test/`, `aot/`).

## 🛠️ Build & test

```bash
dotnet restore Freenaute.Freebox.slnx --locked-mode
dotnet build Freenaute.Freebox.slnx -c Release -warnaserror
dotnet test --solution Freenaute.Freebox.slnx -c Release
dotnet pack Libs/Freenaute.Freebox.Client -c Release        # → .artifacts/nuget/
```

Official API reference (protocol authority): <https://dev.freebox.fr/sdk/os/>
