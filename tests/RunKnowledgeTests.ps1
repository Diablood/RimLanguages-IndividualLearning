param(
    [Parameter(Mandatory = $true)][string]$ProjectRoot,
    [Parameter(Mandatory = $true)][string]$ManagedDir,
    [Parameter(Mandatory = $true)][string]$CybranianDir,
    [Parameter(Mandatory = $true)][string]$HarmonyAssembly,
    [Parameter(Mandatory = $true)][string]$CecilAssembly
)
$ErrorActionPreference = 'Stop'
try {
    # Reuse the calling PowerShell host and its policy, without any policy override.
    # A short-lived process releases the loaded DLL before the next build.
    $assembly = [Reflection.Assembly]::LoadFrom((Join-Path $ProjectRoot 'artifacts\KnowledgeTests.dll'))
    $entry = $assembly.GetType('KnowledgeTests').GetMethod('Main', [Reflection.BindingFlags]'Public,Static')
    $testArguments = [string[]]@($ProjectRoot, $ManagedDir, (Join-Path $CybranianDir '1.6\Assemblies'), (Split-Path -Parent $HarmonyAssembly), (Split-Path -Parent $CecilAssembly))
    $invokeArguments = New-Object 'System.Object[]' 1
    $invokeArguments[0] = $testArguments
    $result = $entry.Invoke($null, $invokeArguments)
    exit ([int]$result)
}
catch {
    Write-Error $_
    exit 1
}
