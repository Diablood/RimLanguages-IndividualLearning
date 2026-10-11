param(
    [string]$RimWorldDir,
    [string]$Compiler,
    [string]$CybranianDir,
    [string]$HarmonyAssembly,
    [string]$CecilAssembly
)
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'tools/Resolve-Dependencies.ps1')
$dependencies = Get-RliDependencies -RimWorldDir $RimWorldDir -CybranianDir $CybranianDir -HarmonyAssembly $HarmonyAssembly -Compiler $Compiler -CecilAssembly $CecilAssembly -IncludeCecil
$RimWorldDir = $dependencies.RimWorldDir
$CybranianDir = $dependencies.CybranianDir
$HarmonyAssembly = $dependencies.HarmonyAssembly
$Compiler = $dependencies.Compiler
$CecilAssembly = $dependencies.CecilAssembly
$metadata = [xml](Get-Content -Raw -LiteralPath (Join-Path $PSScriptRoot 'About\About.xml'))
$version = [string]$metadata.ModMetaData.modVersion
$versionInfo = [Diagnostics.FileVersionInfo]::GetVersionInfo((Join-Path $PSScriptRoot '1.6\Assemblies\RimLanguages.IndividualLearning.dll'))
if ($version -notmatch '^\d+\.\d+\.\d+$' -or $versionInfo.FileVersion -ne "$version.0" -or $versionInfo.ProductVersion -ne $version) { throw 'Metadata/DLL version mismatch: rebuild the mod.' }
Write-Output "PASS: metadata and DLL version $version."
$managed = Join-Path $RimWorldDir 'RimWorldWin64_Data\Managed'
$artifactDir = Join-Path $PSScriptRoot 'artifacts'
New-Item -ItemType Directory -Path $artifactDir -Force | Out-Null
$arguments = @($Compiler, '/nologo', '/target:library', '/nostdlib+', '/warnaserror+', "/out:$artifactDir\KnowledgeTests.dll")
foreach ($file in @('mscorlib.dll','netstandard.dll','System.dll','System.Core.dll','Assembly-CSharp.dll')) { $arguments += "/reference:$managed\$file" }
$arguments += "/reference:$CybranianDir\1.6\Assemblies\FactionLanguages.dll"
$arguments += "/reference:$HarmonyAssembly"
$arguments += "/reference:Cecil=$CecilAssembly"
$arguments += "/reference:$PSScriptRoot\1.6\Assemblies\RimLanguages.IndividualLearning.dll"
$arguments += Join-Path $PSScriptRoot 'tests\KnowledgeTests.cs'
$arguments += Join-Path $PSScriptRoot 'tests\PrisonerPatchTests.cs'
$arguments += Join-Path $PSScriptRoot 'tests\TradePatchTests.cs'
$arguments += Join-Path $PSScriptRoot 'tests\SocialTests.cs'
$arguments += Join-Path $PSScriptRoot 'tests\LearningHistoryTests.cs'
$arguments += Join-Path $PSScriptRoot 'tests\GenerationPatchTests.cs'
$arguments += Join-Path $PSScriptRoot 'tests\MigrationTests.cs'
$arguments += Join-Path $PSScriptRoot 'tests\TransmissionTests.cs'
$arguments += Join-Path $PSScriptRoot 'tests\ChildLearningTests.cs'
$arguments += Join-Path $PSScriptRoot 'tests\CollectivePresentationTests.cs'
$arguments += "/reference:$managed\UnityEngine.CoreModule.dll"
& dotnet @arguments
if ($LASTEXITCODE -ne 0) { throw 'Test compilation failed.' }
$shellName = if ($PSEdition -eq 'Core') { 'pwsh.exe' } else { 'powershell.exe' }
& (Join-Path $PSHOME $shellName) -NoProfile -NonInteractive -File (Join-Path $PSScriptRoot 'tests\RunKnowledgeTests.ps1') -ProjectRoot $PSScriptRoot -ManagedDir $managed -CybranianDir $CybranianDir -HarmonyAssembly $HarmonyAssembly -CecilAssembly $CecilAssembly
if ($LASTEXITCODE -ne 0) { throw 'Knowledge tests failed.' }
$english = [xml](Get-Content -Raw -LiteralPath (Join-Path $PSScriptRoot 'Languages\English\Keyed\IndividualLearning.xml'))
$french = [xml](Get-Content -Raw -LiteralPath (Join-Path $PSScriptRoot 'Languages\French\Keyed\IndividualLearning.xml'))
$enKeys = @($english.LanguageData.ChildNodes | Where-Object NodeType -eq Element | ForEach-Object Name)
$frKeys = @($french.LanguageData.ChildNodes | Where-Object NodeType -eq Element | ForEach-Object Name)
if (Compare-Object $enKeys $frKeys) { throw 'Translation keys differ.' }
foreach ($file in Get-ChildItem -LiteralPath $PSScriptRoot -Recurse -Filter '*.xml') { $null = [xml](Get-Content -Raw -LiteralPath $file.FullName) }
$assemblies = @(Get-ChildItem -LiteralPath (Join-Path $PSScriptRoot '1.6\Assemblies') -Filter '*.dll')
if ($assemblies.Count -ne 1 -or $assemblies[0].Name -ne 'RimLanguages.IndividualLearning.dll') { throw 'Unexpected redistributed DLL.' }
Write-Output 'PASS: XML parsing, translation key parity, dependency DLL exclusion.'
