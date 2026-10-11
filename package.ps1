param(
    [string]$RimWorldDir,
    [string]$CybranianDir,
    [string]$HarmonyAssembly,
    [string]$Compiler,
    [string]$CecilAssembly
)
$ErrorActionPreference = 'Stop'

# Rebuild and run the documented checks; never package an unchecked old DLL.
$buildOptions = @{}
foreach ($name in @('RimWorldDir', 'CybranianDir', 'HarmonyAssembly', 'Compiler')) {
    if ($PSBoundParameters.ContainsKey($name)) { $buildOptions[$name] = $PSBoundParameters[$name] }
}
& (Join-Path $PSScriptRoot 'build.ps1') @buildOptions
$testOptions = $buildOptions.Clone()
if ($PSBoundParameters.ContainsKey('CecilAssembly')) { $testOptions.CecilAssembly = $CecilAssembly }
& (Join-Path $PSScriptRoot 'test.ps1') @testOptions

$metadata = [xml](Get-Content -Raw -LiteralPath (Join-Path $PSScriptRoot 'About/About.xml'))
$version = [string]$metadata.ModMetaData.modVersion
if ($metadata.ModMetaData.packageId -ne 'Diablood.RimLanguages.IndividualLearning' -or
    $version -notmatch '^\d+\.\d+\.\d+$') { throw 'Unexpected package identity or version.' }
$modName = 'RimLanguages-IndividualLearning'
$packageFiles = @(
    'About/About.xml',
    'About/ModIcon.png',
    'LoadFolders.xml',
    '1.6/Assemblies/RimLanguages.IndividualLearning.dll',
    'Languages/English/Keyed/IndividualLearning.xml',
    'Languages/French/Keyed/IndividualLearning.xml',
    'LICENSE',
    'INSTALLATION.md'
)
$runId = (Get-Date -Format 'yyyyMMdd-HHmmss') + '-' + [Guid]::NewGuid().ToString('N').Substring(0, 8)
$outputRoot = Join-Path $PSScriptRoot 'artifacts/packages'
$stagingRoot = Join-Path $outputRoot ('staging-' + $runId)
$modRoot = Join-Path $stagingRoot $modName
$archivePath = Join-Path $outputRoot ($modName + '-' + $version + '-' + $runId + '.zip')

# A fixed allowlist prevents references, saves, logs and other mods entering the archive.
$expected = @{}
foreach ($relative in $packageFiles) {
    $source = Join-Path $PSScriptRoot $relative
    if (-not (Test-Path -LiteralPath $source -PathType Leaf)) { throw "Package file missing: $relative" }
    $destination = Join-Path $modRoot $relative
    New-Item -ItemType Directory -Path (Split-Path -Parent $destination) -Force | Out-Null
    Copy-Item -LiteralPath $source -Destination $destination
    $hash = (Get-FileHash -LiteralPath $source -Algorithm SHA256).Hash
    if ((Get-FileHash -LiteralPath $destination -Algorithm SHA256).Hash -ne $hash) {
        throw "Staging hash mismatch: $relative"
    }
    $expected[$modName + '/' + $relative] = $hash
}

Add-Type -AssemblyName System.IO.Compression.FileSystem
[IO.Compression.ZipFile]::CreateFromDirectory($stagingRoot, $archivePath)
$archive = [IO.Compression.ZipFile]::OpenRead($archivePath)
try {
    if ($archive.Entries.Count -ne $expected.Count) { throw 'Unexpected archive entry count.' }
    $seen = @{}
    foreach ($entry in $archive.Entries) {
        if (-not $expected.ContainsKey($entry.FullName) -or $seen.ContainsKey($entry.FullName)) {
            throw "Unexpected or duplicate archive entry: $($entry.FullName)"
        }
        $seen[$entry.FullName] = $true
        $stream = $entry.Open()
        $sha = [Security.Cryptography.SHA256]::Create()
        try { $hash = [BitConverter]::ToString($sha.ComputeHash($stream)).Replace('-', '') }
        finally { $sha.Dispose(); $stream.Dispose() }
        if ($hash -ne $expected[$entry.FullName]) { throw "Archive hash mismatch: $($entry.FullName)" }
    }
}
finally { $archive.Dispose() }
Write-Output "PASS: $($expected.Count) archive files verified by SHA256; only the addon DLL included."
Get-Item -LiteralPath $archivePath | Select-Object FullName, Length
Get-FileHash -LiteralPath $archivePath -Algorithm SHA256
