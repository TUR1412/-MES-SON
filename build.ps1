param(
  [ValidateSet('Debug', 'Release')]
  [string]$Configuration = 'Debug',

  [switch]$BuildSolution,
  [switch]$SkipRestore
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$root = Split-Path -Parent $PSCommandPath
Set-Location $root

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
  throw 'dotnet CLI not found. Please install .NET SDK (required for dotnet msbuild).'
}

$restoreScript = Join-Path $root 'scripts\restore.ps1'
if (-not $SkipRestore -and (Test-Path $restoreScript)) {
  Write-Host 'Restoring NuGet packages (packages.config)...'
  & $restoreScript -SolutionPath (Join-Path $root 'MES.sln') -PackagesDirectory (Join-Path $root 'packages')
  if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

$targetPath = if ($BuildSolution) { 'MES.sln' } else { 'src\\MES.UI\\MES.UI.csproj' }
$platform = if ($BuildSolution) { 'Any CPU' } else { 'AnyCPU' }

$msbuildArgs = @(
  'msbuild',
  $targetPath,
  '/t:Build',
  "/p:Configuration=$Configuration",
  "/p:Platform=$platform",
  '/p:GenerateResourceMSBuildArchitecture=x64'
)

Write-Host "Building: $targetPath ($Configuration|$platform)"
& dotnet @msbuildArgs
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
