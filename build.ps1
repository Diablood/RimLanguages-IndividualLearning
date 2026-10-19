param(
    [string]$RimWorldDir,
    [string]$CybranianDir,
    [string]$HarmonyAssembly,
    [string]$Compiler,
    [string]$ManagedReferencesDir
)
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'tools/Resolve-Dependencies.ps1')
$dependencies = Get-RliDependencies -RimWorldDir $RimWorldDir -CybranianDir $CybranianDir -HarmonyAssembly $HarmonyAssembly -Compiler $Compiler
$RimWorldDir = $dependencies.RimWorldDir
$CybranianDir = $dependencies.CybranianDir
$HarmonyAssembly = $dependencies.HarmonyAssembly
$Compiler = $dependencies.Compiler
$managed = if ($ManagedReferencesDir) { (Resolve-Path -LiteralPath $ManagedReferencesDir).Path } else {
    Join-Path $RimWorldDir 'RimWorldWin64_Data\Managed'
}
$references = @('mscorlib.dll','netstandard.dll','System.dll','System.Core.dll','Assembly-CSharp.dll','UnityEngine.CoreModule.dll','UnityEngine.IMGUIModule.dll','UnityEngine.TextRenderingModule.dll') | ForEach-Object { Join-Path $managed $_ }
$references += Join-Path $CybranianDir '1.6\Assemblies\FactionLanguages.dll'
$references += $HarmonyAssembly
foreach ($path in @($Compiler) + $references) {
    if (-not (Test-Path -LiteralPath $path)) { throw "Required dependency missing: $path" }
}
$outDir = Join-Path $PSScriptRoot '1.6\Assemblies'
$metadata = [xml](Get-Content -Raw -LiteralPath (Join-Path $PSScriptRoot 'About\About.xml'))
$version = [string]$metadata.ModMetaData.modVersion
if ($version -notmatch '^\d+\.\d+\.\d+$') { throw 'About.xml modVersion must use major.minor.patch.' }
$artifactDir = Join-Path $PSScriptRoot 'artifacts'
New-Item -ItemType Directory -Path $artifactDir -Force | Out-Null
$versionSource = Join-Path $artifactDir 'AssemblyVersion.g.cs'
@"
using System.Reflection;
[assembly: AssemblyVersion("$version.0")]
[assembly: AssemblyFileVersion("$version.0")]
[assembly: AssemblyInformationalVersion("$version")]
"@ | Set-Content -LiteralPath $versionSource -Encoding UTF8
New-Item -ItemType Directory -Path $outDir -Force | Out-Null
$arguments = @($Compiler, '/nologo', '/target:library', '/nostdlib+', '/optimize+', '/deterministic+', '/warnaserror+', '/utf8output', "/out:$outDir\RimLanguages.IndividualLearning.dll")
$arguments += $references | ForEach-Object { "/reference:$_" }
$arguments += $versionSource
$arguments += Get-ChildItem -LiteralPath (Join-Path $PSScriptRoot 'Source') -Filter '*.cs' | Sort-Object Name | Select-Object -ExpandProperty FullName
& dotnet @arguments
if ($LASTEXITCODE -ne 0) { throw "C# compilation failed: $LASTEXITCODE" }
Write-Output "Built version ${version}: $outDir\RimLanguages.IndividualLearning.dll"
