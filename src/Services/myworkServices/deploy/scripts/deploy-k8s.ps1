# ─────────────────────────────────────────────────────────────────────────────
# deploy-k8s.ps1
# Applies all Kubernetes manifests to the target cluster.
# Prerequisite: kubectl is configured for the target cluster.
# Usage: .\deploy-k8s.ps1 [-Tag 1.0.0] [-Registry myregistry.azurecr.io]
# ─────────────────────────────────────────────────────────────────────────────
param(
    [string]$Tag      = ($env:IMAGE_TAG ?? "latest"),
    [string]$Registry = ($env:REGISTRY  ?? "erp")
)

$k8sRoot = Resolve-Path "$PSScriptRoot\..\k8s"

function Apply([string]$path) {
    Write-Host "Applying $path ..." -ForegroundColor Cyan
    kubectl apply -f $path
    if ($LASTEXITCODE -ne 0) { Write-Error "kubectl apply failed: $path"; exit 1 }
}

# 1. Namespace
Apply "$k8sRoot\namespace.yaml"

# 2. Secrets
Apply "$k8sRoot\secrets.yaml"

# 3. Infrastructure (SQL Server + RabbitMQ)
Apply "$k8sRoot\infra\sqlserver.yaml"
Apply "$k8sRoot\infra\rabbitmq.yaml"

Write-Host "`nWaiting for SQL Server to become ready..." -ForegroundColor Yellow
kubectl rollout status statefulset/sqlserver -n erp --timeout=180s

Write-Host "Waiting for RabbitMQ to become ready..." -ForegroundColor Yellow
kubectl rollout status deployment/rabbitmq -n erp --timeout=120s

# 4. Patch image tags if a registry is provided
$services = @("audit","batch","csa","project","risk","team","timesheet","workorder","gateway")
foreach ($svc in $services) {
    $deployName = if ($svc -eq "gateway") { "erp-gateway" } else { "$svc-service" }
    $imageName  = if ($svc -eq "gateway") { "erp-gateway" } else { "$svc-service" }
    kubectl set image deployment/$deployName `
        $deployName="$Registry/erp/$($imageName):$Tag" `
        -n erp 2>$null
}

# 5. Microservices
Get-ChildItem "$k8sRoot\services\*.yaml" | ForEach-Object { Apply $_.FullName }

# 6. Rollout status
Write-Host "`nWaiting for all deployments to roll out..." -ForegroundColor Yellow
kubectl rollout status deployment -n erp --timeout=300s

Write-Host "`nDeployment complete." -ForegroundColor Green
kubectl get pods -n erp
