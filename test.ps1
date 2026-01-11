param(
    [string]$Configuration = "Debug",
    [switch]$SkipBuild,
    [switch]$SkipRestore,
    [string]$ResultsDirectory = ""
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root

function IsPathRooted([string]$path) {
    if ([string]::IsNullOrWhiteSpace($path)) { return $false }
    return [System.IO.Path]::IsPathRooted($path)
}

if (-not $SkipBuild) {
    Write-Host "Build solution ($Configuration)..." -ForegroundColor Cyan

    $buildParams = @{
        Configuration = $Configuration
        BuildSolution = $true
    }
    if ($SkipRestore) { $buildParams.SkipRestore = $true }

    & "$root\\build.ps1" @buildParams
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
} else {
    Write-Host "Skip build; run tests only." -ForegroundColor Yellow
}

$testDll = Join-Path $root "tests\\MES.UnitTests\\bin\\$Configuration\\MES.UnitTests.dll"
if (-not (Test-Path $testDll)) { throw "Test assembly not found: $testDll" }

$candidates = @(
    "$env:ProgramFiles\\Microsoft Visual Studio\\2022\\Community\\Common7\\IDE\\Extensions\\TestPlatform\\vstest.console.exe",
    "$env:ProgramFiles(x86)\\Microsoft Visual Studio\\2022\\Community\\Common7\\IDE\\Extensions\\TestPlatform\\vstest.console.exe",
    "$env:ProgramFiles\\Microsoft Visual Studio\\2022\\BuildTools\\Common7\\IDE\\Extensions\\TestPlatform\\vstest.console.exe",
    "$env:ProgramFiles(x86)\\Microsoft Visual Studio\\2022\\BuildTools\\Common7\\IDE\\Extensions\\TestPlatform\\vstest.console.exe"
)

$vstest = $null
foreach ($p in $candidates) {
    if (Test-Path $p) { $vstest = $p; break }
}

if (-not $vstest) {
    $vswhere = Join-Path $env:ProgramFiles(x86) "Microsoft Visual Studio\\Installer\\vswhere.exe"
    if (Test-Path $vswhere) {
        try {
            $found = & $vswhere -latest -products * -requires Microsoft.VisualStudio.Component.VSTest -find '**\vstest.console.exe' 2>$null
            $found = $found | Select-Object -First 1
            if ($found -and (Test-Path $found)) { $vstest = $found }
        } catch {
            $vstest = $null
        }
    }
}

if (-not $vstest) {
    throw "vstest.console.exe not found. Please install Visual Studio Test Platform (or Build Tools)."
}

Write-Host "Run unit tests..." -ForegroundColor Cyan
$adapterPath = $null
try {
    $adapterPkg = Get-ChildItem (Join-Path $root "packages") -Directory -Filter "MSTest.TestAdapter.*" |
        Sort-Object Name -Descending |
        Select-Object -First 1

    if ($adapterPkg) {
        $common = Join-Path $adapterPkg.FullName "build\\_common"
        $net46 = Join-Path $adapterPkg.FullName "build\\net46"
        $net45 = Join-Path $adapterPkg.FullName "build\\net45"

        if (Test-Path $common) { $adapterPath = $common }
        elseif (Test-Path $net46) { $adapterPath = $net46 }
        elseif (Test-Path $net45) { $adapterPath = $net45 }
    }
} catch {
    $adapterPath = $null
}

$resultsDir = $null
if (-not [string]::IsNullOrWhiteSpace($ResultsDirectory)) {
    $resultsDir = if (IsPathRooted $ResultsDirectory) { $ResultsDirectory } else { Join-Path $root $ResultsDirectory }
    New-Item -ItemType Directory -Force $resultsDir | Out-Null
}

$vstestArgs = @($testDll)
if ($adapterPath) { $vstestArgs += "/TestAdapterPath:$adapterPath" }
if ($resultsDir) {
    $vstestArgs += "/ResultsDirectory:$resultsDir"
    $vstestArgs += "/Logger:trx;LogFileName=MES.UnitTests.trx"
}

& $vstest @vstestArgs
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "All tests passed." -ForegroundColor Green
