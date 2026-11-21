# Native AOT et packaging

## Bibliothèques

`Freenaute.Freebox.Client` et `Freenaute.Freebox.Mapper` ciblent `net11.0` en bibliothèques AnyCPU. Les contrats JSON sont générés à la compilation ; le mode réflexif de `System.Text.Json` est désactivé dans les tests et le smoke AOT. Les contrats additionnels passés au transport utilisent les `JsonTypeInfo<T>` générés.

Les options partagées du mapper sont gelées. L'assembly Mapper est **embarqué** dans le package `Freenaute.Freebox.Client` (`lib/net11.0/`) — il n'existe pas de package Mapper séparé. Les packages d'analyse restent des dépendances privées de build et ne deviennent pas des dépendances consommateur.

## Validation locale

```bash
dotnet restore Freenaute.Freebox.slnx --locked-mode
dotnet build Freenaute.Freebox.slnx -c Release --no-restore -warnaserror
dotnet test --solution Freenaute.Freebox.slnx -c Release --no-build --no-restore

dotnet pack Libs/Freenaute.Freebox.Client/Freenaute.Freebox.Client.csproj -c Release --no-build --no-restore
pwsh .github/scripts/Validate-Packages.ps1

# Native AOT doit être publié sur le système d'exploitation cible correspondant.
dotnet restore Tests/Freenaute.Freebox.AotSmoke/Freenaute.Freebox.AotSmoke.csproj --locked-mode
dotnet publish Tests/Freenaute.Freebox.AotSmoke/Freenaute.Freebox.AotSmoke.csproj -c Release -r linux-x64 --no-restore -warnaserror -o .artifacts/aot/linux-x64
```

Sous Windows, utilisez `win-x64` puis lancez le `.exe`. Exécutez le binaire depuis un dossier de travail propre, sans `DOTNET_ROOT` : aucun JSON de lancement managé ni DLL `Freenaute.Freebox.Client.dll`/`Freenaute.Freebox.Mapper.dll` ne doit être requis à côté.

Le contrôle de package vérifie les paires `.nupkg`/`.snupkg`, `lib/net11.0`, README, métadonnées, dépendances, PDB de symboles et consommation depuis un feed local propre. Les packages sortent dans `.artifacts/nuget/`, les builds/tests dans `.artifacts/build/`.

Native AOT est une barrière de qualité, pas la preuve qu'un handler tiers est compatible. Chaque nouveau package doit publier le smoke sans avertissement trimming/AOT ; une suppression globale d'un avertissement inconnu n'est pas une preuve acceptable.
