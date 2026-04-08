# =============================================================================
# SCI ERP Microservices - Docker Build Script (PowerShell)
# Builds all 16 service Docker images
# =============================================================================

param(
    [string]$Registry = $env:DOCKER_REGISTRY ?? "sci-erp",
    [string]$Tag = $env:IMAGE_TAG ?? "latest"
)

$ErrorActionPreference = "Continue"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RootDir = Split-Path -Parent $ScriptDir

Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "SCI ERP - Building Docker Images"
Write-Host "Registry: $Registry"
Write-Host "Tag: $Tag"
Write-Host "=============================================" -ForegroundColor Cyan

$Services = @(
    @{ Name = "api-gateway";            Context = "ApiGateway" },
    @{ Name = "security-service";       Context = "SecurityServices" },
    @{ Name = "vehicle-tracking";       Context = "vechicletrackingServices" },
    @{ Name = "dispatch-planning";      Context = "dispatchplanningServices" },
    @{ Name = "order-schedule";         Context = "orderscheduleServices" },
    @{ Name = "filling-operation";      Context = "fillingoperationServices" },
    @{ Name = "exim-management";        Context = "eximmanagementServices" },
    @{ Name = "gst-compliance";         Context = "gstcomplianceServices" },
    @{ Name = "inventory-management";   Context = "inventorymanagementServices" },
    @{ Name = "production-management";  Context = "productionmanagementServices" },
    @{ Name = "mam-allocation";         Context = "mamallocationServices" },
    @{ Name = "purchase-sales";         Context = "purchasesalesService" },
    @{ Name = "master-data";            Context = "masterdataServices" },
    @{ Name = "strategic-stock";        Context = "strategicstockServices" },
    @{ Name = "error-logging";          Context = "errorloggingServices" },
    @{ Name = "sci-transactional";      Context = "scitransactionalServices" }
)

$Failed = @()
$Succeeded = @()

foreach ($Service in $Services) {
    $ImageName = "$Registry/$($Service.Name):$Tag"
    $ContextPath = Join-Path $RootDir $Service.Context
    $DockerfilePath = Join-Path $ContextPath "Dockerfile"

    Write-Host ""
    Write-Host "---------------------------------------------" -ForegroundColor Yellow
    Write-Host "Building: $ImageName"
    Write-Host "Context:  $ContextPath"
    Write-Host "---------------------------------------------" -ForegroundColor Yellow

    docker build -t $ImageName -f $DockerfilePath $ContextPath
    
    if ($LASTEXITCODE -eq 0) {
        $Succeeded += $Service.Name
        Write-Host "SUCCESS: $($Service.Name)" -ForegroundColor Green
    } else {
        $Failed += $Service.Name
        Write-Host "FAILED: $($Service.Name)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "Build Summary"
Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "Succeeded: $($Succeeded.Count)/$($Services.Count)" -ForegroundColor Green
foreach ($s in $Succeeded) { Write-Host "  + $s" -ForegroundColor Green }

if ($Failed.Count -gt 0) {
    Write-Host ""
    Write-Host "Failed: $($Failed.Count)/$($Services.Count)" -ForegroundColor Red
    foreach ($f in $Failed) { Write-Host "  x $f" -ForegroundColor Red }
    exit 1
}

Write-Host ""
Write-Host "All images built successfully!" -ForegroundColor Green
