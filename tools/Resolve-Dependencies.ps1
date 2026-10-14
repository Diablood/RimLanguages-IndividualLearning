# Shared dependency discovery. Explicit parameters override environment and Steam discovery.
function Get-RliSteamLibraries {
    $candidates = @()
    if ($env:STEAM_DIR) { $candidates += $env:STEAM_DIR }
    if ($env:OS -eq 'Windows_NT') {
        $steam = Get-ItemProperty -LiteralPath 'HKCU:\Software\Valve\Steam' -ErrorAction SilentlyContinue
        if ($steam.SteamPath) { $candidates += $steam.SteamPath }
        if (${env:ProgramFiles(x86)}) { $candidates += Join-Path ${env:ProgramFiles(x86)} 'Steam' }
    }
    foreach ($candidate in @($candidates)) {
        $vdf = Join-Path $candidate 'steamapps/libraryfolders.vdf'
        if (Test-Path -LiteralPath $vdf -ErrorAction SilentlyContinue) {
            $content = Get-Content -Raw -LiteralPath $vdf
            foreach ($match in [regex]::Matches($content, '"path"\s+"([^"]+)"')) {
                $candidates += $match.Groups[1].Value.Replace('\\', '\')
            }
        }
    }
    @($candidates | Where-Object { Test-Path -LiteralPath $_ -PathType Container -ErrorAction SilentlyContinue } | Select-Object -Unique)
}

function Find-RliPath {
    param([string]$Explicit, [string]$EnvironmentValue, [string[]]$Candidates, [string]$Name)
    $chosen = if ($Explicit) { $Explicit } elseif ($EnvironmentValue) { $EnvironmentValue } else {
        $Candidates | Where-Object { Test-Path -LiteralPath $_ -ErrorAction SilentlyContinue } | Select-Object -First 1
    }
    if (-not $chosen -or -not (Test-Path -LiteralPath $chosen)) {
        throw "Dependency missing: $Name. Supply -$Name explicitly (see docs/BUILDING.md)."
    }
    (Resolve-Path -LiteralPath $chosen).Path
}

function Get-RliDependencies {
    param([string]$RimWorldDir, [string]$CybranianDir, [string]$HarmonyAssembly,
        [string]$Compiler, [string]$CecilAssembly, [switch]$IncludeCecil, [switch]$GameOnly)
    $libraries = @(Get-RliSteamLibraries)
    $game = Find-RliPath $RimWorldDir $env:RIMWORLD_DIR @($libraries | ForEach-Object {
        Join-Path $_ 'steamapps/common/RimWorld'
    }) 'RimWorldDir'
    $result = @{ RimWorldDir = $game }
    if ($GameOnly) { return $result }
    # Include the library containing the explicitly chosen game.
    $libraries = @((Split-Path -Parent (Split-Path -Parent (Split-Path -Parent $game)))) + $libraries
    $result.CybranianDir = Find-RliPath $CybranianDir $env:CYBRANIAN_DIR @($libraries | ForEach-Object {
        Join-Path $_ 'steamapps/workshop/content/294100/3450332059'
    }) 'CybranianDir'
    $gameVersion = '1.6'
    $versionFile = Join-Path $game 'Version.txt'
    if (Test-Path -LiteralPath $versionFile) {
        $versionText = Get-Content -Raw -LiteralPath $versionFile
        if ($versionText -match '^(\d+\.\d+)') { $gameVersion = $Matches[1] }
    }
    $result.HarmonyAssembly = Find-RliPath $HarmonyAssembly $env:HARMONY_ASSEMBLY @($libraries | ForEach-Object {
        Join-Path $_ "steamapps/workshop/content/294100/2009463077/$gameVersion/Assemblies/0Harmony.dll"
        Join-Path $_ 'steamapps/workshop/content/294100/2009463077/Current/Assemblies/0Harmony.dll'
    }) 'HarmonyAssembly'
    $compilers = @()
    if (-not $Compiler -and -not $env:DOTNET_CSC -and (Get-Command dotnet -ErrorAction SilentlyContinue)) {
        $sdkLines = & dotnet --list-sdks
        if ($LASTEXITCODE -ne 0) { throw 'Cannot list .NET SDKs; supply -Compiler.' }
        $sdks = @($sdkLines | ForEach-Object {
            if ($_ -match '^(\d+\.\d+\.\d+)[^ ]*\s+\[(.+)\]$') {
                [pscustomobject]@{ Version = [version]$Matches[1]; Path = Join-Path $Matches[2] ($_ -split ' ')[0] }
            }
        } | Sort-Object Version -Descending)
        $compilers = @($sdks | ForEach-Object { Join-Path $_.Path 'Roslyn/bincore/csc.dll' })
    }
    $result.Compiler = Find-RliPath $Compiler $env:DOTNET_CSC $compilers 'Compiler'
    if ($IncludeCecil) {
        $packageRoot = if ($env:NUGET_PACKAGES) { $env:NUGET_PACKAGES } else { Join-Path $env:USERPROFILE '.nuget/packages' }
        $package = Join-Path $packageRoot 'microsoft.net.illink.tasks'
        $cecilCandidates = @()
        if (Test-Path -LiteralPath $package) {
            $cecilCandidates = @(Get-ChildItem -LiteralPath $package -Directory | Sort-Object Name -Descending |
                ForEach-Object { Join-Path $_.FullName 'tools/netframework/Mono.Cecil.dll' })
        }
        $result.CecilAssembly = Find-RliPath $CecilAssembly $env:CECIL_ASSEMBLY $cecilCandidates 'CecilAssembly'
    }
    $result
}
