param(
  [string]$SolutionPath = '',
  [string]$PackagesDirectory = '',
  [switch]$ForceDownload
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$root = Split-Path -Parent $PSScriptRoot
Set-Location $root

if ([string]::IsNullOrWhiteSpace($SolutionPath)) {
  $SolutionPath = Join-Path $root 'MES.sln'
}

if ([string]::IsNullOrWhiteSpace($PackagesDirectory)) {
  $PackagesDirectory = Join-Path $root 'packages'
}

if (-not (Test-Path $SolutionPath)) {
  Write-Error "Solution not found: $SolutionPath"
  exit 2
}

$nugetCmd = Get-Command nuget -ErrorAction SilentlyContinue
$nugetExe = $null

if ($nugetCmd -and $nugetCmd.Source) {
  $nugetExe = $nugetCmd.Source
} else {
  $toolsDir = Join-Path $root '.tools\nuget'
  $nugetExe = Join-Path $toolsDir 'nuget.exe'

  if ($ForceDownload -and (Test-Path $nugetExe)) {
    Remove-Item -Force $nugetExe
  }

  if (-not (Test-Path $nugetExe)) {
    New-Item -ItemType Directory -Force $toolsDir | Out-Null

    $url = 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe'
    Write-Host "Downloading nuget.exe -> $nugetExe"
    Invoke-WebRequest -Uri $url -OutFile $nugetExe
  }
}

if (-not (Test-Path $nugetExe)) {
  Write-Error "nuget.exe not found: $nugetExe"
  exit 3
}

$restoreArgs = @(
  'restore',
  $SolutionPath,
  '-NonInteractive',
  '-PackagesDirectory',
  $PackagesDirectory
)

Write-Host "Running: $nugetExe $($restoreArgs -join ' ')"
& $nugetExe @restoreArgs
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

