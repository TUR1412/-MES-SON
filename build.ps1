param(
  [ValidateSet('Debug', 'Release')]
  [string]$Configuration = 'Debug',

  [switch]$BuildSolution
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$root = Split-Path -Parent $PSCommandPath
Set-Location $root

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
  throw 'dotnet CLI not found. Please install .NET SDK (required for dotnet msbuild).'
}

$nugetCmd = Get-Command nuget -ErrorAction SilentlyContinue
$nugetExe = $null
if ($nugetCmd) {
  $nugetExe = 'nuget'
} else {
  # 兼容本机未安装 nuget.exe 的情况：临时下载到 %TEMP%
  $nugetExe = Join-Path $env:TEMP 'nuget.exe'
  if (-not (Test-Path $nugetExe)) {
    $nugetUrl = 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe'
    Write-Host "Downloading NuGet CLI: $nugetUrl"
    Invoke-WebRequest -Uri $nugetUrl -OutFile $nugetExe
  }
}

$restoreTarget = 'MES.sln'
Write-Host "Restoring packages: $restoreTarget"
& $nugetExe restore $restoreTarget -NonInteractive

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
