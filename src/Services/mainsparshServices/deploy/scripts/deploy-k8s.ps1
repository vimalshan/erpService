# ==========================================
# SRF Sparsh - Kubernetes Deploy (PowerShell)
# ==========================================

param(
    [ValidateSet("apply", "delete", "status", "logs")]
    [string]$Action = "apply",
    [string]$Deployment = "",
    [string]$Registry = "srfsparsh",
    [string]$ImageTag = "latest"
)

$ErrorActionPreference = "Stop"
$K8sDir = Resolve-Path "$PSScriptRoot\..\k8s"

Write-Host "============================================" -ForegroundColor Cyan
Write-Host " SRF Sparsh - Kubernetes: $Action" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan

switch ($Action) {
    "apply" {
        Write-Host "Creating namespace..." -ForegroundColor Yellow
        kubectl apply -f "$K8sDir\namespace.yaml"

        Write-Host "Creating secrets and config..." -ForegroundColor Yellow
        kubectl apply -f "$K8sDir\secrets.yaml"
        kubectl apply -f "$K8sDir\configmap.yaml"

        Write-Host "Deploying infrastructure..." -ForegroundColor Yellow
        kubectl apply -f "$K8sDir\infrastructure.yaml"

        Write-Host "Waiting for infrastructure..." -ForegroundColor Yellow
        kubectl -n srfsparsh wait --for=condition=available --timeout=120s deployment/sqlserver 2>$null
        kubectl -n srfsparsh wait --for=condition=available --timeout=120s deployment/rabbitmq 2>$null
        kubectl -n srfsparsh wait --for=condition=available --timeout=60s deployment/redis 2>$null

        Write-Host "Deploying microservices..." -ForegroundColor Yellow
        kubectl apply -f "$K8sDir\services.yaml"

        Write-Host "Deploying ingress..." -ForegroundColor Yellow
        kubectl apply -f "$K8sDir\ingress.yaml"

        Write-Host "`nDeployment complete:" -ForegroundColor Green
        kubectl -n srfsparsh get deployments
        kubectl -n srfsparsh get services
    }
    "delete" {
        Write-Host "Deleting all resources..." -ForegroundColor Red
        kubectl delete -f "$K8sDir\ingress.yaml" --ignore-not-found
        kubectl delete -f "$K8sDir\services.yaml" --ignore-not-found
        kubectl delete -f "$K8sDir\infrastructure.yaml" --ignore-not-found
        kubectl delete -f "$K8sDir\secrets.yaml" --ignore-not-found
        kubectl delete -f "$K8sDir\configmap.yaml" --ignore-not-found
        kubectl delete -f "$K8sDir\namespace.yaml" --ignore-not-found
        Write-Host "All resources deleted." -ForegroundColor Green
    }
    "status" {
        kubectl -n srfsparsh get all
    }
    "logs" {
        if ($Deployment) {
            kubectl -n srfsparsh logs -f "deployment/$Deployment" --all-containers
        } else {
            Write-Host "Usage: .\deploy-k8s.ps1 -Action logs -Deployment <name>" -ForegroundColor Yellow
            kubectl -n srfsparsh get deployments -o name
        }
    }
}
