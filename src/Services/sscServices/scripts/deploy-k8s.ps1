# ─── Deploy SSC Services to Kubernetes (PowerShell) ───────────────────────
param(
    [Parameter(Mandatory)]
    [string]$Registry,
    [string]$Tag = "latest"
)

$ErrorActionPreference = "Stop"
$K8sDir = Join-Path (Split-Path -Parent $PSScriptRoot) "k8s"

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Deploying SSC Services to Kubernetes"
Write-Host "Registry: $Registry  Tag: $Tag"
Write-Host "============================================" -ForegroundColor Cyan

# Step 1: Namespace
Write-Host "`n── Creating namespace ──" -ForegroundColor Yellow
kubectl apply -f "$K8sDir\namespace.yaml"

# Step 2: Secrets & ConfigMap
Write-Host "`n── Applying secrets and configmap ──" -ForegroundColor Yellow
kubectl apply -f "$K8sDir\secrets.yaml"
kubectl apply -f "$K8sDir\configmap.yaml"

# Step 3: Infrastructure
Write-Host "`n── Deploying SQL Server ──" -ForegroundColor Yellow
kubectl apply -f "$K8sDir\sqlserver.yaml"

Write-Host "`n── Deploying RabbitMQ ──" -ForegroundColor Yellow
kubectl apply -f "$K8sDir\rabbitmq.yaml"

Write-Host "`n── Waiting for infrastructure ──" -ForegroundColor Yellow
kubectl -n ssc-services rollout status statefulset/sqlserver --timeout=120s 2>$null
kubectl -n ssc-services rollout status statefulset/rabbitmq --timeout=120s 2>$null

# Step 4: Application services
Write-Host "`n── Deploying application services ──" -ForegroundColor Yellow
$deployContent = Get-Content "$K8sDir\deployments.yaml" -Raw
$deployContent = $deployContent -replace '\$\{REGISTRY\}', $Registry
$deployContent = $deployContent -replace ':latest', ":$Tag"
$deployContent | kubectl apply -f -

# Step 5: Ingress
Write-Host "`n── Deploying ingress ──" -ForegroundColor Yellow
kubectl apply -f "$K8sDir\ingress.yaml"

# Step 6: Status
Write-Host "`n============================================" -ForegroundColor Cyan
Write-Host "Deployment Status" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
kubectl -n ssc-services get deployments
Write-Host ""
kubectl -n ssc-services get services
Write-Host ""
kubectl -n ssc-services get pods

Write-Host "`nDeployment complete!" -ForegroundColor Green
Write-Host "Monitor with: kubectl -n ssc-services get pods -w"
