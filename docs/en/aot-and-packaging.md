# Native AOT and packaging

## Libraries

`Freenaute.Freebox.Client` and `Freenaute.Freebox.Mapper` target `net11.0` as AnyCPU libraries. JSON contracts are source generated; reflection-based `System.Text.Json` is disabled in the tests and AOT smoke. Consumers must use the generated `JsonTypeInfo<T>` metadata for additional transport contracts.

The mapper's shared options are frozen. The client package depends on Mapper. Analyzer packages are build-only private assets and do not become consumer dependencies.

## Local validation

```bash
dotnet restore Freenaute.Freebox.slnx --locked-mode
dotnet build Freenaute.Freebox.slnx -c Release --no-restore -warnaserror
dotnet test --solution Freenaute.Freebox.slnx -c Release --no-build --no-restore

dotnet pack Libs/Freenaute.Freebox.Mapper/Freenaute.Freebox.Mapper.csproj -c Release --no-build --no-restore
dotnet pack Libs/Freenaute.Freebox.Client/Freenaute.Freebox.Client.csproj -c Release --no-build --no-restore
pwsh .github/scripts/Validate-Packages.ps1

# Native AOT must be published on the matching operating system.
dotnet restore Tests/Freenaute.Freebox.AotSmoke/Freenaute.Freebox.AotSmoke.csproj --locked-mode
dotnet publish Tests/Freenaute.Freebox.AotSmoke/Freenaute.Freebox.AotSmoke.csproj -c Release -r linux-x64 --no-restore -warnaserror -o .artifacts/aot/linux-x64
```

On Windows use `win-x64` and run the resulting `.exe`. Run the published executable from a clean working directory without `DOTNET_ROOT`; neither managed launcher JSON file nor `Freenaute.Freebox.Client.dll`/`Freenaute.Freebox.Mapper.dll` should be required beside it.

The package gate checks both `.nupkg` and `.snupkg`, `lib/net11.0`, README, metadata, dependencies, symbol PDB, and consumption from a clean local-feed application. Package output is `.artifacts/nuget/`; build and test output is under `.artifacts/build/`.

Native AOT is a gate, not a promise that a third-party handler is compatible. New packages must publish the smoke with zero trimming/AOT warnings. Suppressing an unknown warning globally is not an acceptable proof.
