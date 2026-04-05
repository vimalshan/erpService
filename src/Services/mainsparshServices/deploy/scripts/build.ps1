# ==========================================
# SRF Sparsh Microservices - Build Script (PowerShell)
# Builds all Docker images
# ==========================================

param(
    [string]$ImageTag = "latest",
    [string]$Registry = "srfsparsh"
)

$ErrorActionPreference = "Stop"
$RootDir = Resolve-Path "$PSScriptRoot\.."

Write-Host "============================================" -ForegroundColor Cyan
Write-Host " SRF Sparsh - Building Docker Images" -ForegroundColor Cyan
Write-Host " Tag: $ImageTag" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan

$services = [ordered]@{
    "api-gateway"              = "apiGateway"
    "approval-service"         = "approvalServices"
    "booking-service"          = "bookingServices"
    "community-service"        = "communityServices"
    "compensation-service"     = "compensationServices"
    "groupmanagement-service"  = "groupmanagementServices"
    "location-service"         = "locationServices"
    "meeting-service"          = "meetingServices"
    "proxy-service"            = "proxyServices"
    "reimbursement-service"    = "reimbursementServices"
    "stipend-service"          = "stipendservices"
    "timesheet-service"        = "timesheetServices"
    "transaction-service"      = "transactionServices"
    "usermanagement-service"   = "usermanagementServices"
    "websitecontent-service"   = "websitecontentServices"
}

$failed = 0
$total = $services.Count

foreach ($svc in $services.GetEnumerator()) {
    $name = $svc.Key
    $context = $svc.Value
    Write-Host "`n--- Building $name from $context ---" -ForegroundColor Yellow

    $contextPath = Join-Path $RootDir $context
    docker build -t "$Registry/${name}:$ImageTag" $contextPath

    if ($LASTEXITCODE -eq 0) {
        Write-Host "[OK] $name built successfully" -ForegroundColor Green
    } else {
        Write-Host "[FAIL] $name build failed!" -ForegroundColor Red
        $failed++
    }
}

Write-Host "`n============================================" -ForegroundColor Cyan
if ($failed -eq 0) {
    Write-Host " All $total images built successfully!" -ForegroundColor Green
} else {
    Write-Host " $failed of $total images failed to build." -ForegroundColor Red
    exit 1
}
Write-Host "============================================" -ForegroundColor Cyan
