<#
.SYNOPSIS
    Deploy ERP Microservices to Kubernetes
.PARAMETER Action
    Action: deploy, teardown, status, scale
.PARAMETER DeploymentName
    Deployment name for scale action
.PARAMETER Replicas
    Number of replicas for scale action
#>
param(
    [ValidateSet("deploy", "teardown", "status", "scale")]
    [string]$Action = "deploy",
    [string]$DeploymentName = "",
    [int]$Replicas = 0
)

$ErrorActionPreference = "Stop"
$RootDir = (Resolve-Path "$PSScriptRoot\..\..").Path
$K8sDir = "$RootDir\k8s"
$Namespace = "erp-microservices"

Write-Host "============================================" -ForegroundColor Yellow
Write-Host "  ERP Microservices - Kubernetes Deployment"
Write-Host "============================================" -ForegroundColor Yellow

# Check kubectl
if (-not (Get-Command kubectl -ErrorAction SilentlyContinue)) {
    Write-Host "ERROR: kubectl is not installed or not in PATH" -ForegroundColor Red
    exit 1
}

switch ($Action) {
    "deploy" {
        Write-Host "`nStep 1/4 - Creating namespace & config..." -ForegroundColor Cyan
        kubectl apply -f "$K8sDir\namespace.yaml"
        kubectl apply -f "$K8sDir\secrets-configmap.yaml"
        Write-Host "  OK Namespace and secrets created" -ForegroundColor Green

        Write-Host "`nStep 2/4 - Deploying infrastructure..." -ForegroundColor Cyan
        kubectl apply -f "$K8sDir\infrastructure\sqlserver.yaml"
        kubectl apply -f "$K8sDir\infrastructure\rabbitmq.yaml"
        Write-Host "  OK SQL Server and RabbitMQ deployed" -ForegroundColor Green

        Write-Host "`n  Waiting for infrastructure to be ready..."
        kubectl -n $Namespace rollout status statefulset/sqlserver --timeout=120s 2>$null
        kubectl -n $Namespace rollout status statefulset/rabbitmq --timeout=120s 2>$null
        Write-Host "  OK Infrastructure ready" -ForegroundColor Green

        Write-Host "`nStep 3/4 - Deploying microservices..." -ForegroundColor Cyan
        Get-ChildItem "$K8sDir\services\*.yaml" | ForEach-Object {
            Write-Host "  Applying $($_.BaseName)..."
            kubectl apply -f $_.FullName
        }
        Write-Host "  OK All microservices deployed" -ForegroundColor Green

        Write-Host "`nStep 4/4 - Deploying ingress..." -ForegroundColor Cyan
        if (Test-Path "$K8sDir\ingress.yaml") {
            kubectl apply -f "$K8sDir\ingress.yaml"
            Write-Host "  OK Ingress deployed" -ForegroundColor Green
        } else {
            Write-Host "  No ingress.yaml found, skipping" -ForegroundColor DarkGray
        }

        Write-Host "`n============================================" -ForegroundColor Yellow
        Write-Host "  Deployment complete"
        Write-Host "============================================" -ForegroundColor Yellow
        Write-Host "`nCheck status:  kubectl -n $Namespace get pods"
        Write-Host "Gateway:       kubectl -n $Namespace get svc api-gateway-service"
    }
    "teardown" {
        $answer = Read-Host "This will delete ALL resources in namespace '$Namespace'. Continue? [y/N]"
        if ($answer -eq "y" -or $answer -eq "Y") {
            Write-Host "Removing all resources..." -ForegroundColor Cyan
            kubectl delete namespace $Namespace --ignore-not-found
            Write-Host "Namespace $Namespace deleted." -ForegroundColor Green
        }
    }
    "status" {
        Write-Host "`n--- Pods ---" -ForegroundColor Cyan
        kubectl -n $Namespace get pods -o wide
        Write-Host "`n--- Services ---" -ForegroundColor Cyan
        kubectl -n $Namespace get svc
        Write-Host "`n--- Deployments ---" -ForegroundColor Cyan
        kubectl -n $Namespace get deployments
        Write-Host "`n--- StatefulSets ---" -ForegroundColor Cyan
        kubectl -n $Namespace get statefulsets
    }
    "scale" {
        if (-not $DeploymentName -or $Replicas -le 0) {
            Write-Host "Usage: .\k8s-deploy.ps1 -Action scale -DeploymentName <name> -Replicas <n>" -ForegroundColor Red
            exit 1
        }
        kubectl -n $Namespace scale deployment $DeploymentName --replicas=$Replicas
        Write-Host "Scaled $DeploymentName to $Replicas replicas" -ForegroundColor Green
    }
}
