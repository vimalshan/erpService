# =============================================================================
# WMS Microservices - Build All Docker Images (PowerShell)
# =============================================================================
param(
    [string]$Registry = "wms",
    [string]$Tag = "latest"
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RootDir = Resolve-Path "$ScriptDir\..\.."

Write-Host "==============================================" -ForegroundColor Cyan
Write-Host " WMS Microservices - Docker Image Build" -ForegroundColor Cyan
Write-Host " Registry: $Registry" -ForegroundColor Cyan
Write-Host " Tag:      $Tag" -ForegroundColor Cyan
Write-Host "==============================================" -ForegroundColor Cyan

Set-Location $RootDir

$services = @(
    @{ Name = "security-service";       Context = "securityService" },
    @{ Name = "warehouse-service";      Context = "warehousestructureService" },
    @{ Name = "racking-service";        Context = "rackingsystemService" },
    @{ Name = "employee-service";       Context = "emplyeeService" },
    @{ Name = "product-service";        Context = "productService" },
    @{ Name = "inventory-service";      Context = "inventoryService" },
    @{ Name = "supplier-service";       Context = "supplierService" },
    @{ Name = "customer-service";       Context = "customerService" },
    @{ Name = "purchaseorder-service";  Context = "purchaseorderService" },
    @{ Name = "receiving-service";      Context = "receivingService" },
    @{ Name = "salesorder-service";     Context = "salesorderService" },
    @{ Name = "shipment-service";       Context = "shipmentService" },
    @{ Name = "order-service";          Context = "orderService" },
    @{ Name = "fleet-service";          Context = "fleetManagementService" },
    @{ Name = "auditlog-service";       Context = "auditlogService\AuditLogService" },
    @{ Name = "transactional-service";  Context = "wmtransactionalService" },
    @{ Name = "api-gateway";            Context = "apiGateway" }
)

$succeeded = @()
$failed = @()

foreach ($svc in $services) {
    $image = "$Registry/$($svc.Name):$Tag"
    $context = ".\$($svc.Context)"

    Write-Host ""
    Write-Host "----------------------------------------------" -ForegroundColor Yellow
    Write-Host " Building: $image" -ForegroundColor Yellow
    Write-Host " Context:  $context" -ForegroundColor Yellow
    Write-Host "----------------------------------------------" -ForegroundColor Yellow

    try {
        docker build -t $image -f "$context\Dockerfile" $context
        $succeeded += $svc.Name
        Write-Host " [OK] $($svc.Name) built successfully" -ForegroundColor Green
    }
    catch {
        $failed += $svc.Name
        Write-Host " [FAIL] $($svc.Name) FAILED: $_" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host " Build Summary" -ForegroundColor Cyan
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host " Succeeded: $($succeeded.Count)/$($services.Count)" -ForegroundColor Green
foreach ($s in $succeeded) { Write-Host "   [OK] $s" -ForegroundColor Green }

if ($failed.Count -gt 0) {
    Write-Host " Failed: $($failed.Count)/$($services.Count)" -ForegroundColor Red
    foreach ($f in $failed) { Write-Host "   [FAIL] $f" -ForegroundColor Red }
    exit 1
}

Write-Host ""
Write-Host " All images built successfully!" -ForegroundColor Green
Write-Host "==============================================" -ForegroundColor Cyan
