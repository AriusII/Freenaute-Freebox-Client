param(
    [string] $PackagePath = ".artifacts/nuget",
    [string] $WorkspacePath = ".artifacts/package-validation"
)

$ErrorActionPreference = "Stop"
$expected = @("Freenaute.Freebox.Client", "Freenaute.Freebox.Mapper")
$root = (Resolve-Path $PackagePath).Path
if (Test-Path $WorkspacePath) {
    Remove-Item $WorkspacePath -Recurse -Force
}
New-Item -ItemType Directory -Path $WorkspacePath | Out-Null
$workspace = (Resolve-Path $WorkspacePath).Path

$validated = @{}
foreach ($id in $expected) {
    $packages = @(Get-ChildItem $root -Filter "$id.*.nupkg" -File)
    $symbols = @(Get-ChildItem $root -Filter "$id.*.snupkg" -File)
    if ($packages.Count -ne 1 -or $symbols.Count -ne 1) {
        throw "Expected one package and one symbol package for $id."
    }

    $packageArchive = [System.IO.Compression.ZipFile]::OpenRead($packages[0].FullName)
    try { $entries = @($packageArchive.Entries.FullName) }
    finally { $packageArchive.Dispose() }
    if (-not ($entries | Where-Object { $_ -match "^lib/net11\.0/.+\.dll$" })) {
        throw "$id does not contain a net11.0 library."
    }
    if (-not ($entries | Where-Object { $_ -ieq "README.md" })) {
        throw "$id does not contain README.md."
    }
    if ($entries | Where-Object { $_ -match '(^|/)(Tests|Samples|Tools)/' -or $_ -match '\.(cs|csproj)$' }) {
        throw "$id contains source or excluded project content."
    }

    $nuspecName = $entries | Where-Object { $_ -match '\.nuspec$' } | Select-Object -First 1
    $archive = [System.IO.Compression.ZipFile]::OpenRead($packages[0].FullName)
    try {
        $nuspecEntry = $archive.GetEntry($nuspecName)
        $reader = [System.IO.StreamReader]::new($nuspecEntry.Open())
        try { [xml] $nuspec = $reader.ReadToEnd() } finally { $reader.Dispose() }
    }
    finally { $archive.Dispose() }

    $metadata = $nuspec.package.metadata
    if ($metadata.id -ne $id -or $metadata.license.type -ne "expression" -or $metadata.license.'#text' -ne "MIT" -or
        [string]::IsNullOrWhiteSpace([string]$metadata.description) -or [string]::IsNullOrWhiteSpace([string]$metadata.repository.url)) {
        throw "$id has incomplete or unexpected package metadata."
    }
    if ($metadata.dependencies.dependency.id -contains "Meziantou.Analyzer" -or
        $metadata.dependencies.group.dependencies.dependency.id -contains "Meziantou.Analyzer" -or
        $metadata.dependencies.dependency.id -contains "ZeroAlloc.Analyzers" -or
        $metadata.dependencies.group.dependencies.dependency.id -contains "ZeroAlloc.Analyzers") {
        throw "$id unexpectedly exposes a build-quality analyzer as a package dependency."
    }

    $symbolArchive = [System.IO.Compression.ZipFile]::OpenRead($symbols[0].FullName)
    try {
        if (-not ($symbolArchive.Entries.FullName | Where-Object { $_ -match "^lib/net11\.0/.+\.pdb$" })) {
            throw "$id symbol package does not contain the net11.0 PDB."
        }
    }
    finally { $symbolArchive.Dispose() }

    $validated[$id] = $metadata.version
}

$consumer = Join-Path $workspace "PackageConsumer"
dotnet new console --framework net11.0 --no-restore --output $consumer
if ($LASTEXITCODE -ne 0) { throw "dotnet new failed with exit code $LASTEXITCODE." }
@"
<Project>
</Project>
"@ | Set-Content (Join-Path $consumer "Directory.Build.props")
@"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net11.0</TargetFramework>
    <ManagePackageVersionsCentrally>false</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Freenaute.Freebox.Client" Version="$($validated['Freenaute.Freebox.Client'])" />
  </ItemGroup>
</Project>
"@ | Set-Content (Join-Path $consumer "PackageConsumer.csproj")
@"
using Freenaute.Freebox.Client;

System.Console.WriteLine(typeof(IFreeboxClient).FullName);
"@ | Set-Content (Join-Path $consumer "Program.cs")
@"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="local-candidates" value="$root" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
"@ | Set-Content (Join-Path $consumer "NuGet.Config")

dotnet restore $consumer --configfile (Join-Path $consumer "NuGet.Config")
if ($LASTEXITCODE -ne 0) { throw "Package consumer restore failed with exit code $LASTEXITCODE." }
dotnet run --project $consumer --no-restore
if ($LASTEXITCODE -ne 0) { throw "Package consumer run failed with exit code $LASTEXITCODE." }

Write-Output "Validated package/symbol pairs, metadata, contents, and local-feed consumption."
