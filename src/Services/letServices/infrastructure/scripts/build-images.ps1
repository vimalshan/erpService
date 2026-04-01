# ═══════════════════════════════════════════════════════════════════════
# build-images.ps1 — Build and optionally push all Docker images
# Usage: .\build-images.ps1 [-Push] [-Registry <registry>] [-Tag <tag>]
# ═══════════════════════════════════════════════════════════════════════
param(
    [switch]$Push,
    [string]$Registry = $env:REGISTRY ?? "letregistry",
    [string]$Tag = $env:IMAGE_TAG ?? "latest"
)

$ErrorActionPreference = "Stop"
$RootDir = Resolve-Path "$PSScriptRoot\..\.."

$Services = @(
    @{ Name = "api-gateway";            Context = "apiGateway" }
    @{ Name = "leave-service";          Context = "leaveServices" }
    @{ Name = "course-service";         Context = "courseServices" }
    @{ Name = "request-service";        Context = "requestServices" }
    @{ Name = "review-service";         Context = "reviewServices" }
    @{ Name = "development-service";    Context = "developmentServices" }
    @{ Name = "master-service";         Context = "masterServices" }
    @{ Name = "let-transaction-service"; Context = "letTransactionServices" }
)

Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "  Building LET ERP Docker Images"
Write-Host "  Registry: $Registry    Tag: $Tag"
Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan

$Failed = @()
foreach ($svc in $Services) {
    $image = "$Registry/$($svc.Name):$Tag"
    $contextPath = Join-Path $RootDir $svc.Context

    Write-Host "`n--- Building $image ---" -ForegroundColor Yellow
    docker build -t $image $contextPath
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  + $($svc.Name) built successfully" -ForegroundColor Green
        if ($Push) {
            Write-Host "  Pushing $image..."
            docker push $image
            Write-Host "  + $($svc.Name) pushed" -ForegroundColor Green
        }
    } else {
        Write-Host "  x $($svc.Name) FAILED" -ForegroundColor Red
        $Failed += $svc.Name
    }
}

Write-Host "`n═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
if ($Failed.Count -eq 0) {
    Write-Host "  All images built successfully." -ForegroundColor Green
} else {
    Write-Host "  FAILED builds: $($Failed -join ', ')" -ForegroundColor Red
    exit 1
}
