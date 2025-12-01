Param()

$ErrorActionPreference = "Stop"

Write-Host "=== Pipeline test: RunAllTests.ps1 ==="

# PSScriptRoot = tests/scripts
$scriptDir = $PSScriptRoot
$testsDir  = Split-Path -Parent $scriptDir       # tests
$repoRoot  = Split-Path -Parent $testsDir        # repo root

Write-Host "Repo root: $repoRoot"
Push-Location $repoRoot
try {
    # 1) Reset database (hvis implementeret)
    $resetScript = Join-Path $scriptDir "ResetDatabase.ps1"
    if (Test-Path $resetScript) {
        Write-Host "Kører ResetDatabase.ps1..."
        & $resetScript
    }

    # 2) Kør application unit tests (kan udvides med flere projekter)
    Write-Host "Kører Application unit tests..."
    dotnet test "/PriceRunner.Application.Tests/PriceRunner.Application.Tests.csproj" `
        --configuration Release `
        --no-build
}
finally {
    Pop-Location
}

Write-Host "=== Pipeline test færdig ==="
