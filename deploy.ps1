param([string]$RimWorldDir, [string]$RimWorldModsDir)
$ErrorActionPreference = 'Stop'
$sourceRoot = $PSScriptRoot
. (Join-Path $PSScriptRoot 'tools/Resolve-Dependencies.ps1')
if (-not $RimWorldModsDir) {
    $dependencies = Get-RliDependencies -RimWorldDir $RimWorldDir -GameOnly
    $RimWorldModsDir = Join-Path $dependencies.RimWorldDir 'Mods'
}
if (-not (Test-Path -LiteralPath $RimWorldModsDir -PathType Container)) { throw 'RimWorld Mods directory not found.' }
$modsRoot = (Resolve-Path -LiteralPath $RimWorldModsDir).Path.TrimEnd('\', '/')
$targetRoot = [IO.Path]::GetFullPath((Join-Path $modsRoot 'RimLanguages-IndividualLearning'))
if (-not $targetRoot.StartsWith($modsRoot + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) { throw 'Unsafe mod directory.' }
$files = @('About\About.xml', 'About\ModIcon.png', 'About\Preview.png', 'LoadFolders.xml', '1.6\Assemblies\RimLanguages.IndividualLearning.dll', 'Languages\French\Keyed\IndividualLearning.xml', 'Languages\English\Keyed\IndividualLearning.xml', 'LICENSE', 'INSTALLATION.md')
if (Get-Process -Name RimWorldWin64,RimWorld -ErrorAction SilentlyContinue) { throw 'Close RimWorld before updating the test build.' }
$installedAbout = Join-Path $targetRoot 'About\About.xml'
if (Test-Path -LiteralPath $installedAbout) {
    $metadata = [xml](Get-Content -Raw -LiteralPath $installedAbout)
    if ($metadata.ModMetaData.packageId -ne 'Diablood.RimLanguages.IndividualLearning') { throw 'Unexpected destination mod identity.' }
} elseif (Test-Path -LiteralPath $targetRoot) { throw 'Existing destination has no valid mod metadata.' }
$sourceMetadata = [xml](Get-Content -Raw -LiteralPath (Join-Path $sourceRoot 'About/About.xml'))
$version = [string]$sourceMetadata.ModMetaData.modVersion
$versionInfo = [Diagnostics.FileVersionInfo]::GetVersionInfo((Join-Path $sourceRoot '1.6/Assemblies/RimLanguages.IndividualLearning.dll'))
if ($sourceMetadata.ModMetaData.packageId -ne 'Diablood.RimLanguages.IndividualLearning' -or $version -notmatch '^\d+\.\d+\.\d+$' -or $versionInfo.ProductVersion -ne $version -or $versionInfo.FileVersion -ne "$version.0") { throw 'Source identity/version mismatch. Build and test before deploying.' }
$backupRoot = Join-Path $sourceRoot (('work\runtime-before-' + $version + '-') + (Get-Date -Format 'yyyyMMdd-HHmmss-fff'))
foreach ($relative in $files) {
    $destination = [IO.Path]::GetFullPath((Join-Path $targetRoot $relative))
    if (-not $destination.StartsWith($targetRoot + '\', [StringComparison]::OrdinalIgnoreCase)) { throw 'Unsafe destination.' }
    if (-not (Test-Path -LiteralPath (Join-Path $sourceRoot $relative))) { throw "Source missing: $relative" }
    if (Test-Path -LiteralPath $destination) {
        $backup = Join-Path $backupRoot $relative
        New-Item -ItemType Directory -Path (Split-Path -Parent $backup) -Force | Out-Null
        Copy-Item -LiteralPath $destination -Destination $backup
    }
}
foreach ($relative in $files) {
    $destination = Join-Path $targetRoot $relative
    New-Item -ItemType Directory -Path (Split-Path -Parent $destination) -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $sourceRoot $relative) -Destination $destination -Force
    if ((Get-FileHash -LiteralPath $destination).Hash -ne (Get-FileHash -LiteralPath (Join-Path $sourceRoot $relative)).Hash) { throw "Hash mismatch: $relative" }
}
Write-Output "Version $version deployed: $($files.Count) files verified by SHA256."
if (Test-Path -LiteralPath $backupRoot) { Write-Output "Previous runtime backup: $backupRoot" }
else { Write-Output 'First installation: no previous runtime to back up.' }
Get-FileHash -LiteralPath (Join-Path $targetRoot '1.6\Assemblies\RimLanguages.IndividualLearning.dll') | Format-List Hash
