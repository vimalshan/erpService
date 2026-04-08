# =============================================================================
# SCI ERP Microservices - Full Deployment Script (PowerShell)
# End-to-end: build images, push to registry, deploy to Kubernetes
# =============================================================================

param(
    [string]$Registry = $env:DOCKER_REGISTRY ?? "sci-erp",
    [string]$Tag = $env:IMAGE_TAG ?? "latest",
    [switch]$SkipBuild,
    [switch]$SkipPush,
    [switch]$SkipK8s,
    [switch]$DockerCompose,
    [switch]$Help
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RootDir = Split-Path -Parent $ScriptDir

if ($Help) {
    Write-Host "Usage: deploy.ps1 [OPTIONS]"
    Write-Host ""
    Write-Host "Options:"
    Write-Host "  -Registry NAME     Docker registry (default: sci-erp)"
    Write-Host "  -Tag TAG           Image tag (default: latest)"
    Write-Host "  -SkipBuild         Skip Docker image build"
    Write-Host "  -SkipPush          Skip pushing images to registry"
    Write-Host "  -SkipK8s           Skip Kubernetes deployment"
    Write-Host "  -DockerCompose     Deploy with docker-compose instead of K8s"
    Write-Host "  -Help              Show this help message"
    exit 0
}

$env:DOCKER_REGISTRY = $Registry
$env:IMAGE_TAG = $Tag

Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "SCI ERP - Full Deployment Pipeline"
Write-Host "Registry: $Registry"
Write-Host "Tag: $Tag"
Write-Host "=============================================" -ForegroundColor Cyan

# Step 1: Build Docker images
if (-not $SkipBuild) {
    Write-Host ""
    Write-Host ">>> Step 1: Building Docker images..." -ForegroundColor Yellow
    & "$ScriptDir\docker-build.ps1" -Registry $Registry -Tag $Tag
} else {
    Write-Host ""
    Write-Host ">>> Step 1: Skipping Docker build" -ForegroundColor DarkGray
}

# Step 2: Push Docker images
if (-not $SkipPush -and -not $DockerCompose) {
    Write-Host ""
    Write-Host ">>> Step 2: Pushing Docker images..." -ForegroundColor Yellow
    $Images = @(
        "api-gateway", "security-service", "vehicle-tracking", "dispatch-planning",
        "order-schedule", "filling-operation", "exim-management", "gst-compliance",
        "inventory-management", "production-management", "mam-allocation", "purchase-sales",
        "master-data", "strategic-stock", "error-logging", "sci-transactional"
    )
    foreach ($Image in $Images) {
        Write-Host "  Pushing $Registry/${Image}:$Tag ..."
        docker push "$Registry/${Image}:$Tag"
    }
} else {
    Write-Host ""
    Write-Host ">>> Step 2: Skipping Docker push" -ForegroundColor DarkGray
}

# Step 3: Deploy
if ($DockerCompose) {
    Write-Host ""
    Write-Host ">>> Step 3: Starting with Docker Compose..." -ForegroundColor Yellow
    Push-Location $RootDir
    docker compose up -d
    Pop-Location
    Write-Host ""
    Write-Host "Services starting... Check status with: docker compose ps" -ForegroundColor Green
} elseif (-not $SkipK8s) {
    Write-Host ""
    Write-Host ">>> Step 3: Deploying to Kubernetes..." -ForegroundColor Yellow
    
    $K8sDir = Join-Path $RootDir "k8s"
    
    Write-Host "[1/5] Creating namespace..."
    kubectl apply -f (Join-Path $K8sDir "namespace.yaml")
    
    Write-Host "[2/5] Applying secrets and configmaps..."
    kubectl apply -f (Join-Path $K8sDir "secrets-configmap.yaml")
    
    Write-Host "[3/5] Deploying infrastructure..."
    kubectl apply -f (Join-Path $K8sDir "infrastructure\sqlserver.yaml")
    kubectl apply -f (Join-Path $K8sDir "infrastructure\rabbitmq.yaml")
    
    Write-Host "Waiting for SQL Server..."
    kubectl rollout status statefulset/sqlserver -n sci-erp --timeout=300s
    
    Write-Host "Waiting for RabbitMQ..."
    kubectl rollout status statefulset/rabbitmq -n sci-erp --timeout=300s
    
    Write-Host "[4/5] Deploying microservices..."
    Get-ChildItem -Path (Join-Path $K8sDir "services") -Filter "*.yaml" | ForEach-Object {
        Write-Host "  Deploying $($_.BaseName)..."
        kubectl apply -f $_.FullName
    }
    
    Write-Host "[5/5] Applying ingress..."
    kubectl apply -f (Join-Path $K8sDir "ingress.yaml")
    
    Write-Host ""
    Write-Host "Check status with:" -ForegroundColor Green
    Write-Host "  kubectl get pods -n sci-erp"
    Write-Host "  kubectl get services -n sci-erp"
    Write-Host "  kubectl get hpa -n sci-erp"
} else {
    Write-Host ""
    Write-Host ">>> Step 3: Skipping deployment" -ForegroundColor DarkGray
}

Write-Host ""
Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "Deployment pipeline complete!" -ForegroundColor Green
Write-Host "=============================================" -ForegroundColor Cyan
