<#
.SYNOPSIS
    Smoke test all ERP Microservice endpoints via the API Gateway
.PARAMETER GatewayUrl
    Base URL of the API Gateway (default: http://localhost:5000)
#>
param(
    [string]$GatewayUrl = "http://localhost:5000"
)

$ErrorActionPreference = "Stop"

Write-Host "============================================" -ForegroundColor Yellow
Write-Host "  ERP Microservices - Smoke Test"
Write-Host "  Gateway: $GatewayUrl"
Write-Host "============================================" -ForegroundColor Yellow

$passed = 0
$failed = 0

function Test-Endpoint([string]$Name, [string]$Url, [int]$ExpectedStatus = 200) {
    try {
        $response = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec 10 -ErrorAction Stop
        if ($response.StatusCode -eq $ExpectedStatus) {
            Write-Host "  PASS  $Name ($($response.StatusCode))" -ForegroundColor Green
            $script:passed++
        } else {
            Write-Host "  FAIL  $Name (expected $ExpectedStatus, got $($response.StatusCode))" -ForegroundColor Red
            $script:failed++
        }
    }
    catch {
        $status = $_.Exception.Response.StatusCode.value__
        if ($status -eq $ExpectedStatus) {
            Write-Host "  PASS  $Name ($status)" -ForegroundColor Green
            $script:passed++
        } else {
            Write-Host "  FAIL  $Name ($($_.Exception.Message))" -ForegroundColor Red
            $script:failed++
        }
    }
}

Write-Host "`n--- Gateway ---" -ForegroundColor Cyan
Test-Endpoint "Gateway Home"        "$GatewayUrl/"
Test-Endpoint "Gateway Health"      "$GatewayUrl/health"

Write-Host "`n--- Lookup Service ---" -ForegroundColor Cyan
Test-Endpoint "Lookup Health"       "$GatewayUrl/api/lookup/health"

Write-Host "`n--- Task Service ---" -ForegroundColor Cyan
Test-Endpoint "Task Health"         "$GatewayUrl/api/task/health"

Write-Host "`n--- Transactional Service ---" -ForegroundColor Cyan
Test-Endpoint "Transactional Health" "$GatewayUrl/api/transactional/health"

Write-Host "`n--- Complaint Service ---" -ForegroundColor Cyan
Test-Endpoint "Complaint Health"    "$GatewayUrl/api/complaint/health"

Write-Host "`n--- Energy Service ---" -ForegroundColor Cyan
Test-Endpoint "Energy Health"       "$GatewayUrl/api/energy/health"

Write-Host "`n--- Unit Service ---" -ForegroundColor Cyan
Test-Endpoint "Unit Health"         "$GatewayUrl/api/unit/health"

Write-Host "`n============================================" -ForegroundColor Yellow
Write-Host "  Results: $passed passed, $failed failed"
Write-Host "============================================" -ForegroundColor Yellow

if ($failed -gt 0) { exit 1 }
