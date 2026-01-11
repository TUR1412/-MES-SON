param(
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root

Write-Host "Build solution ($Configuration)..." -ForegroundColor Cyan
& "$root\\build.ps1" -Configuration $Configuration -BuildSolution
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

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
    throw "vstest.console.exe not found. Please install Visual Studio Test Platform."
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

if ($adapterPath) {
    & $vstest $testDll "/TestAdapterPath:$adapterPath"
} else {
    & $vstest $testDll
}
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "All tests passed." -ForegroundColor Green
