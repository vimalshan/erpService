# ─── Build all Docker images for SSC Services (PowerShell) ────────────────
param(
    [string]$Registry = "ssc-services",
    [string]$Tag = "latest"
)

$ErrorActionPreference = "Stop"

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Building SSC Service Docker Images"
Write-Host "Registry: $Registry  Tag: $Tag"
Write-Host "============================================" -ForegroundColor Cyan

$RootDir = Split-Path -Parent $PSScriptRoot

$Services = @{
    "ssc-transactional"   = "ssctransactionalServices"
    "batch-and-envelope"  = "batchandenvelopeServices"
    "category-and-vendor" = "categoryandvendorServices"
    "club-membership"     = "clubmembershipServices"
    "filing-and-archive"  = "fillingandarchiveServices"
    "hr-document"         = "hrdocumentServices"
    "integration-service" = "integrationServices\IntegrationService"
    "invoice-processing"  = "invoiceprocessingServices\InvoiceProcessing.Service"
    "master-data"         = "masterdataServices\MasterDataService"
    "menu-and-security"   = "menuandsecurityServices"
    "approval-group"      = "approvalgroupServices"
    "user-service"        = "menuServices\01_USER_MODULE"
    "ssc-api-gateway"     = "apigateway"
}

$Failed = @()

foreach ($ServiceName in $Services.Keys) {
    $Context = $Services[$ServiceName]
    $FullContext = Join-Path $RootDir $Context

    Write-Host ""
    Write-Host "Building: $ServiceName" -ForegroundColor Yellow
    Write-Host "Context:  $Context"

    try {
        docker build -t "$Registry/${ServiceName}:$Tag" -f "$FullContext\Dockerfile" "$FullContext"
        Write-Host "OK $ServiceName built successfully" -ForegroundColor Green
    }
    catch {
        Write-Host "FAIL $ServiceName FAILED" -ForegroundColor Red
        $Failed += $ServiceName
    }
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Build Summary"
Write-Host "Total:  $($Services.Count)"
Write-Host "Failed: $($Failed.Count)"
Write-Host "============================================" -ForegroundColor Cyan

if ($Failed.Count -gt 0) {
    Write-Host "Failed services:" -ForegroundColor Red
    $Failed | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
    exit 1
}

Write-Host "All images built successfully!" -ForegroundColor Green
