# ==============================================================================
# ERP Travel Services - Kubernetes Deployment Script (Windows PowerShell)
# ==============================================================================

param(
    [Parameter(Position=0)]
    [ValidateSet("build","deploy","delete","status","logs","rollback","help")]
    [string]$Command = "help",

    [Parameter(Position=1)]
    [string]$ServiceName
)

$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot

$K8sDir    = Join-Path $PSScriptRoot "k8s"
$Registry  = if ($env:REGISTRY)  { $env:REGISTRY }  else { "erptravelservices.azurecr.io" }
$ImageTag  = if ($env:IMAGE_TAG) { $env:IMAGE_TAG } else { "latest" }
$Namespace = "erp-travel"

function Write-Step($msg)   { Write-Host "[$(Get-Date -Format 'HH:mm:ss')] $msg" -ForegroundColor Green }
function Write-Warn($msg)   { Write-Host "[WARNING] $msg" -ForegroundColor Yellow }
function Write-Err($msg)    { Write-Host "[ERROR] $msg" -ForegroundColor Red }
function Write-Header($msg) {
    Write-Host "`n============================================" -ForegroundColor Cyan
    Write-Host "  $msg" -ForegroundColor Cyan
    Write-Host "============================================`n" -ForegroundColor Cyan
}

function Show-Usage {
    Write-Host @"
Usage: .\k8s-deploy.ps1 [COMMAND] [SERVICE_NAME]

Commands:
  build       Build and push all Docker images
  deploy      Deploy all services to Kubernetes
  delete      Delete all Kubernetes resources
  status      Show deployment status
  logs        Show logs for a service (requires service name)
  rollback    Rollback a deployment (requires service name)

Environment Variables:
  REGISTRY    Container registry (default: erptravelservices.azurecr.io)
  IMAGE_TAG   Image tag (default: latest)
"@
}

$Services = @(
    @{ Name = "api-gateway";           Context = "ApiGateway";               Dockerfile = "Dockerfile" },
    @{ Name = "travel-request-api";    Context = "travelRequestServices";    Dockerfile = "Dockerfile" },
    @{ Name = "travel-transaction-api"; Context = "traveltransactionServices"; Dockerfile = "Dockerfile" },
    @{ Name = "booking-api";           Context = "bookingServices";          Dockerfile = "Dockerfile" },
    @{ Name = "expense-api";           Context = "expenseServices";          Dockerfile = "Dockerfile" },
    @{ Name = "finance-api";           Context = "financeServices";          Dockerfile = "Dockerfile" },
    @{ Name = "insurance-api";         Context = "insuranceServices";        Dockerfile = "Dockerfile" },
    @{ Name = "masterdata-api";        Context = "masterdataServices";       Dockerfile = "Dockerfile" },
    @{ Name = "agency-api";            Context = "agensService";             Dockerfile = "Dockerfile.production" },
    @{ Name = "admin-api";             Context = "adminServices";            Dockerfile = "Dockerfile" }
)

function Invoke-BuildImages {
    Write-Header "Building & Pushing Docker Images"

    foreach ($svc in $Services) {
        $image = "$Registry/$($svc.Name):$ImageTag"
        Write-Step "Building $($svc.Name)..."
        docker build -t $image -f "$($svc.Context)/$($svc.Dockerfile)" "$($svc.Context)/"
        Write-Step "Pushing $image..."
        docker push $image
    }

    Write-Step "All images built and pushed"
}

function Invoke-Deploy {
    Write-Header "Deploying to Kubernetes"

    if (-not (Get-Command kubectl -ErrorAction SilentlyContinue)) {
        Write-Err "kubectl is not installed"; exit 1
    }

    Write-Step "Creating namespace..."
    kubectl apply -f "$K8sDir\namespace.yaml"

    Write-Step "Applying secrets and config..."
    kubectl apply -f "$K8sDir\secrets.yaml"
    kubectl apply -f "$K8sDir\configmap.yaml"

    Write-Step "Deploying infrastructure..."
    kubectl apply -f "$K8sDir\infrastructure.yaml"

    Write-Step "Waiting for SQL Server..."
    kubectl wait --for=condition=ready pod -l app=sqlserver -n $Namespace --timeout=120s 2>&1 | Out-Null

    Write-Step "Waiting for RabbitMQ..."
    kubectl wait --for=condition=ready pod -l app=rabbitmq -n $Namespace --timeout=120s 2>&1 | Out-Null

    Write-Step "Deploying API Gateway..."
    $env:REGISTRY = $Registry; $env:IMAGE_TAG = $ImageTag
    (Get-Content "$K8sDir\api-gateway.yaml") -replace '\$\{REGISTRY\}', $Registry -replace '\$\{IMAGE_TAG\}', $ImageTag | kubectl apply -f -

    Write-Step "Deploying microservices..."
    (Get-Content "$K8sDir\services.yaml") -replace '\$\{REGISTRY\}', $Registry -replace '\$\{IMAGE_TAG\}', $ImageTag | kubectl apply -f -

    Write-Step "Applying ingress..."
    kubectl apply -f "$K8sDir\ingress.yaml"

    Write-Step "Deployment complete!"
    Invoke-K8sStatus
}

function Invoke-Delete {
    Write-Header "Deleting All K8s Resources"
    Write-Warn "This will delete ALL resources in namespace $Namespace!"
    $confirm = Read-Host "Are you sure? (y/N)"
    if ($confirm -eq "y" -or $confirm -eq "Y") {
        kubectl delete namespace $Namespace --ignore-not-found
        Write-Step "All resources deleted"
    } else {
        Write-Step "Deletion cancelled"
    }
}

function Invoke-K8sStatus {
    Write-Header "Kubernetes Status"
    Write-Host "--- Pods ---"
    kubectl get pods -n $Namespace -o wide
    Write-Host "`n--- Services ---"
    kubectl get svc -n $Namespace
    Write-Host "`n--- HPA ---"
    kubectl get hpa -n $Namespace
    Write-Host "`n--- Ingress ---"
    kubectl get ingress -n $Namespace
}

function Invoke-K8sLogs {
    if (-not $ServiceName) {
        Write-Host "Usage: .\k8s-deploy.ps1 logs <service-name>"
        Write-Host "Services: api-gateway, travel-request-api, travel-transaction-api, booking-api,"
        Write-Host "          expense-api, finance-api, insurance-api, masterdata-api, agency-api, admin-api"
        return
    }
    kubectl logs -f -l "app=$ServiceName" -n $Namespace --tail=100
}

function Invoke-Rollback {
    if (-not $ServiceName) {
        Write-Host "Usage: .\k8s-deploy.ps1 rollback <deployment-name>"; return
    }
    Write-Step "Rolling back $ServiceName..."
    kubectl rollout undo deployment/$ServiceName -n $Namespace
    kubectl rollout status deployment/$ServiceName -n $Namespace
    Write-Step "Rollback complete"
}

switch ($Command) {
    "build"    { Invoke-BuildImages }
    "deploy"   { Invoke-Deploy }
    "delete"   { Invoke-Delete }
    "status"   { Invoke-K8sStatus }
    "logs"     { Invoke-K8sLogs }
    "rollback" { Invoke-Rollback }
    default    { Show-Usage }
}
