# =============================================================================
# SCI ERP Microservices - Teardown Script (PowerShell)
# Removes all K8s resources or stops Docker Compose
# =============================================================================

param(
    [ValidateSet("k8s", "docker-compose")]
    [string]$Mode = "k8s",
    [switch]$Force
)

$ErrorActionPreference = "Continue"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RootDir = Split-Path -Parent $ScriptDir
$K8sDir = Join-Path $RootDir "k8s"

Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "SCI ERP - Teardown"
Write-Host "Mode: $Mode"
Write-Host "=============================================" -ForegroundColor Cyan

if ($Mode -eq "docker-compose") {
    Write-Host ""
    Write-Host "Stopping Docker Compose services..." -ForegroundColor Yellow
    Push-Location $RootDir
    docker compose down -v
    Pop-Location
    Write-Host "Docker Compose services stopped and volumes removed." -ForegroundColor Green
}
elseif ($Mode -eq "k8s") {
    if (-not $Force) {
        Write-Host ""
        Write-Host "WARNING: This will delete ALL resources in the sci-erp namespace." -ForegroundColor Red
        $Confirm = Read-Host "Are you sure? (y/N)"
        if ($Confirm -notin @("y", "Y")) {
            Write-Host "Aborted."
            exit 0
        }
    }

    Write-Host ""
    Write-Host "Removing ingress..." -ForegroundColor Yellow
    kubectl delete -f (Join-Path $K8sDir "ingress.yaml") --ignore-not-found

    Write-Host "Removing services..." -ForegroundColor Yellow
    Get-ChildItem -Path (Join-Path $K8sDir "services") -Filter "*.yaml" | ForEach-Object {
        kubectl delete -f $_.FullName --ignore-not-found
    }

    Write-Host "Removing infrastructure..." -ForegroundColor Yellow
    kubectl delete -f (Join-Path $K8sDir "infrastructure\rabbitmq.yaml") --ignore-not-found
    kubectl delete -f (Join-Path $K8sDir "infrastructure\sqlserver.yaml") --ignore-not-found

    Write-Host "Removing secrets and configmaps..." -ForegroundColor Yellow
    kubectl delete -f (Join-Path $K8sDir "secrets-configmap.yaml") --ignore-not-found

    if (-not $Force) {
        $DeleteNs = Read-Host "Delete namespace (and all PVCs)? (y/N)"
    } else {
        $DeleteNs = "y"
    }
    
    if ($DeleteNs -in @("y", "Y")) {
        kubectl delete -f (Join-Path $K8sDir "namespace.yaml") --ignore-not-found
        Write-Host "Namespace deleted." -ForegroundColor Green
    } else {
        Write-Host "Namespace preserved." -ForegroundColor Yellow
    }

    Write-Host ""
    Write-Host "Teardown complete." -ForegroundColor Green
}
