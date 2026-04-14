# =============================================================================
# WMS Microservices - Kubernetes Deployment Script (PowerShell)
# =============================================================================
param(
    [ValidateSet("apply", "delete", "status", "restart", "logs")]
    [string]$Action = "apply",
    [string]$Service = ""
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$K8sDir = Resolve-Path "$ScriptDir\..\k8s"
$Namespace = "wms"

Write-Host "==============================================" -ForegroundColor Cyan
Write-Host " WMS Microservices - Kubernetes Deployment" -ForegroundColor Cyan
Write-Host " Action: $Action" -ForegroundColor Cyan
Write-Host "==============================================" -ForegroundColor Cyan

switch ($Action) {
    "apply" {
        Write-Host ""
        Write-Host "[1/7] Creating namespace..." -ForegroundColor Yellow
        kubectl apply -f "$K8sDir\00-namespace.yaml"

        Write-Host ""
        Write-Host "[2/7] Creating secrets..." -ForegroundColor Yellow
        kubectl apply -f "$K8sDir\01-secrets.yaml"

        Write-Host ""
        Write-Host "[3/7] Creating config maps..." -ForegroundColor Yellow
        kubectl apply -f "$K8sDir\02-configmap.yaml"

        Write-Host ""
        Write-Host "[4/7] Deploying SQL Server..." -ForegroundColor Yellow
        kubectl apply -f "$K8sDir\03-sqlserver.yaml"
        Write-Host "Waiting for SQL Server to be ready..."
        kubectl wait --for=condition=ready pod -l app=sqlserver -n $Namespace --timeout=120s

        Write-Host ""
        Write-Host "[5/7] Deploying RabbitMQ..." -ForegroundColor Yellow
        kubectl apply -f "$K8sDir\04-rabbitmq.yaml"
        Write-Host "Waiting for RabbitMQ to be ready..."
        kubectl wait --for=condition=ready pod -l app=rabbitmq -n $Namespace --timeout=120s

        Write-Host ""
        Write-Host "[6/7] Deploying all microservices..." -ForegroundColor Yellow
        kubectl apply -f "$K8sDir\05-services.yaml"

        Write-Host ""
        Write-Host "[7/7] Deploying API Gateway & Ingress..." -ForegroundColor Yellow
        kubectl apply -f "$K8sDir\06-api-gateway.yaml"
        kubectl apply -f "$K8sDir\07-ingress.yaml"

        Write-Host ""
        Write-Host "==============================================" -ForegroundColor Cyan
        Write-Host " Kubernetes deployment complete!" -ForegroundColor Green
        Write-Host "==============================================" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "Waiting for all pods to be ready..."
        kubectl wait --for=condition=ready pod --all -n $Namespace --timeout=300s
        Write-Host ""
        kubectl get pods -n $Namespace
        Write-Host ""
        kubectl get svc -n $Namespace
    }

    "delete" {
        Write-Host "Deleting all WMS resources..." -ForegroundColor Red
        kubectl delete -f "$K8sDir\07-ingress.yaml" --ignore-not-found
        kubectl delete -f "$K8sDir\06-api-gateway.yaml" --ignore-not-found
        kubectl delete -f "$K8sDir\05-services.yaml" --ignore-not-found
        kubectl delete -f "$K8sDir\04-rabbitmq.yaml" --ignore-not-found
        kubectl delete -f "$K8sDir\03-sqlserver.yaml" --ignore-not-found
        kubectl delete -f "$K8sDir\02-configmap.yaml" --ignore-not-found
        kubectl delete -f "$K8sDir\01-secrets.yaml" --ignore-not-found
        kubectl delete -f "$K8sDir\00-namespace.yaml" --ignore-not-found
        Write-Host "All WMS resources deleted." -ForegroundColor Green
    }

    "status" {
        Write-Host "Namespace: $Namespace" -ForegroundColor Yellow
        Write-Host ""
        Write-Host "--- Pods ---" -ForegroundColor Cyan
        kubectl get pods -n $Namespace -o wide
        Write-Host ""
        Write-Host "--- Services ---" -ForegroundColor Cyan
        kubectl get svc -n $Namespace
        Write-Host ""
        Write-Host "--- Deployments ---" -ForegroundColor Cyan
        kubectl get deployments -n $Namespace
        Write-Host ""
        Write-Host "--- Ingress ---" -ForegroundColor Cyan
        kubectl get ingress -n $Namespace
    }

    "restart" {
        if ($Service) {
            Write-Host "Restarting $Service..." -ForegroundColor Yellow
            kubectl rollout restart deployment/$Service -n $Namespace
        } else {
            Write-Host "Restarting all deployments..." -ForegroundColor Yellow
            kubectl rollout restart deployment -n $Namespace
        }
    }

    "logs" {
        $target = if ($Service) { $Service } else { "api-gateway" }
        kubectl logs -f -l app=$target -n $Namespace --all-containers --tail=100
    }
}
