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
  throw '未检测到 dotnet CLI：请先安装 .NET SDK（用于 dotnet msbuild）。'
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

Write-Host "构建目标: $targetPath ($Configuration|$platform)"
& dotnet @msbuildArgs
