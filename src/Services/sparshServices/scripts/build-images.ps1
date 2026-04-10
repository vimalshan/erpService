# ============================================================================
# SPARSH Platform - Docker Build & Push Script (PowerShell)
# Usage: .\scripts\build-images.ps1 [-Push] [-Registry <reg>] [-Tag <tag>]
# ============================================================================
param(
    [switch]$Push,
    [string]$Registry = $env:DOCKER_REGISTRY ?? "sparsh",
    [string]$Tag = $env:IMAGE_TAG ?? "latest"
)

$ErrorActionPreference = "Stop"
$RootDir = Split-Path -Parent $PSScriptRoot

Write-Host "============================================"
Write-Host "SPARSH Platform - Building Docker Images"
Write-Host "Registry: $Registry"
Write-Host "Tag:      $Tag"
Write-Host "Push:     $Push"
Write-Host "============================================"

$services = @{
    "sparsh-api-gateway"         = "apigateway\SparshApiGateway"
    "sparsh-employee-pride-api"  = "employeepridemanagementServices"
    "sparsh-mobile-app-api"      = "mobileappmanagementServices"
    "sparsh-mobile-expense-api"  = "mobileexpenseServices"
    "sparsh-problem-api"         = "problemmanagementServices\ProblemManagement"
    "sparsh-transactional-api"   = "sparshtransactionalServices\SparshTransactional"
}

$failed = @()

foreach ($entry in $services.GetEnumerator()) {
    $serviceName = $entry.Key
    $contextDir = Join-Path $RootDir $entry.Value
    $imageName = "$Registry/${serviceName}:$Tag"

    Write-Host "`n--- Building $serviceName ---"
    Write-Host "Context: $contextDir"

    try {
        docker build -t $imageName $contextDir
        Write-Host "[OK] $serviceName built" -ForegroundColor Green

        if ($Push) {
            Write-Host "Pushing $imageName..."
            docker push $imageName
            Write-Host "[OK] $serviceName pushed" -ForegroundColor Green
        }
    }
    catch {
        Write-Host "[FAIL] $serviceName - $_" -ForegroundColor Red
        $failed += $serviceName
    }
}

Write-Host "`n============================================"
if ($failed.Count -eq 0) {
    Write-Host "All images built successfully!" -ForegroundColor Green
} else {
    Write-Host "FAILURES:" -ForegroundColor Red
    $failed | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
    exit 1
}
