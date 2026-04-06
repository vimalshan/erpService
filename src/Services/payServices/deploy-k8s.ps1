<#
.SYNOPSIS
    ERP Microservice - Kubernetes Deploy Script (PowerShell)
.DESCRIPTION
    Deploys all ERP microservices to Kubernetes
.PARAMETER Action
    The action to perform: build, apply, delete, status, logs
.PARAMETER Service
    Service name for logs command
.EXAMPLE
    .\deploy-k8s.ps1 apply
    .\deploy-k8s.ps1 status
    .\deploy-k8s.ps1 logs employee-service
#>

param(
    [ValidateSet("build", "apply", "delete", "status", "logs")]
    [string]$Action = "apply",
    [string]$Service = "api-gateway"
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$K8sDir = Join-Path $ScriptDir "k8s"
$Registry = if ($env:DOCKER_REGISTRY) { $env:DOCKER_REGISTRY } else { "erp" }

function Write-Info($msg)  { Write-Host "[INFO] $msg" -ForegroundColor Blue }
function Write-Ok($msg)    { Write-Host "[OK] $msg" -ForegroundColor Green }
function Write-Warn($msg)  { Write-Host "[WARN] $msg" -ForegroundColor Yellow }
function Write-Err($msg)   { Write-Host "[ERROR] $msg" -ForegroundColor Red }

function Test-Dependencies {
    if (-not (Get-Command kubectl -ErrorAction SilentlyContinue)) {
        Write-Err "kubectl is not installed"; exit 1
    }
    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        Write-Err "Docker is not installed"; exit 1
    }
    Write-Ok "Dependencies verified"
}

function Start-Build {
    Write-Info "Building Docker images..."
    $services = @(
        @{Name="api-gateway"; Context="apiGateway"},
        @{Name="employee-service"; Context="employeeServices"},
        @{Name="hr-service"; Context="hrServices"},
        @{Name="faq-service"; Context="faqServices"},
        @{Name="payroll-service"; Context="payrollServices"},
        @{Name="tax-service"; Context="taxServices"},
        @{Name="paytransactional-service"; Context="payTransactionalServices"}
    )
    foreach ($svc in $services) {
        Write-Info "Building $($svc.Name)..."
        $ctx = Join-Path $ScriptDir $svc.Context
        docker build -t "$Registry/$($svc.Name):latest" -f (Join-Path $ctx "Dockerfile") $ctx
        Write-Ok "$($svc.Name) built"
    }
}

function Start-Apply {
    Write-Info "Deploying to Kubernetes..."

    Write-Info "Creating namespace..."
    kubectl apply -f (Join-Path $K8sDir "namespace.yaml")

    Write-Info "Applying secrets & config..."
    kubectl apply -f (Join-Path $K8sDir "secrets-configmap.yaml")

    Write-Info "Deploying infrastructure..."
    kubectl apply -f (Join-Path $K8sDir "sqlserver.yaml")
    kubectl apply -f (Join-Path $K8sDir "rabbitmq.yaml")

    Write-Info "Waiting for SQL Server..."
    kubectl -n erp-microservices wait --for=condition=ready pod -l app=sqlserver --timeout=120s 2>$null
    if ($LASTEXITCODE -ne 0) { Write-Warn "SQL Server not ready yet" }

    Write-Info "Waiting for RabbitMQ..."
    kubectl -n erp-microservices wait --for=condition=ready pod -l app=rabbitmq --timeout=120s 2>$null
    if ($LASTEXITCODE -ne 0) { Write-Warn "RabbitMQ not ready yet" }

    Write-Info "Deploying microservices..."
    $manifests = @(
        "api-gateway.yaml",
        "employee-service.yaml",
        "hr-service.yaml",
        "faq-service.yaml",
        "payroll-service.yaml",
        "tax-service.yaml",
        "paytransactional-service.yaml"
    )
    foreach ($f in $manifests) {
        kubectl apply -f (Join-Path $K8sDir $f)
    }

    Write-Info "Applying ingress..."
    kubectl apply -f (Join-Path $K8sDir "ingress.yaml")

    Write-Ok "Deployment complete"
    Write-Host ""
    Start-Status
}

function Start-Delete {
    Write-Info "Removing Kubernetes resources..."
    Get-ChildItem -Path $K8sDir -Filter "*.yaml" | ForEach-Object {
        kubectl delete -f $_.FullName --ignore-not-found=true 2>$null
    }
    Write-Ok "All resources removed"
}

function Start-Status {
    Write-Info "Kubernetes resource status:"
    Write-Host ""
    Write-Host "--- Pods ---" -ForegroundColor Cyan
    kubectl -n erp-microservices get pods -o wide
    Write-Host ""
    Write-Host "--- Services ---" -ForegroundColor Cyan
    kubectl -n erp-microservices get svc
    Write-Host ""
    Write-Host "--- Deployments ---" -ForegroundColor Cyan
    kubectl -n erp-microservices get deployments
    Write-Host ""
    Write-Host "--- Ingress ---" -ForegroundColor Cyan
    kubectl -n erp-microservices get ingress
}

function Start-Logs {
    kubectl -n erp-microservices logs -f -l "app=$Service" --all-containers
}

# Main
Test-Dependencies

switch ($Action) {
    "build"  { Start-Build }
    "apply"  { Start-Apply }
    "delete" { Start-Delete }
    "status" { Start-Status }
    "logs"   { Start-Logs }
}
