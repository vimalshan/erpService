# ─────────────────────────────────────────────────────────────────────────────
# build-push.ps1
# Builds all Docker images and pushes them to the container registry.
# Usage: .\build-push.ps1 [-Registry myregistry.azurecr.io] [-Tag 1.0.0]
# ─────────────────────────────────────────────────────────────────────────────
param(
    [string]$Registry = $env:REGISTRY,
    [string]$Tag      = ($env:IMAGE_TAG ?? "latest")
)

if (-not $Registry) {
    Write-Error "Registry not set. Pass -Registry or set REGISTRY env var."
    exit 1
}

$root = Resolve-Path "$PSScriptRoot\..\.."

$services = @(
    @{ Name = "audit-service";     Context = "auditServices";                        Dockerfile = "auditServices\Dockerfile" },
    @{ Name = "batch-service";     Context = "batchServices";                        Dockerfile = "batchServices\Dockerfile" },
    @{ Name = "csa-service";       Context = "csaServices\CSA.Service";              Dockerfile = "csaServices\CSA.Service\Dockerfile" },
    @{ Name = "project-service";   Context = "projectServices";                      Dockerfile = "projectServices\Dockerfile" },
    @{ Name = "risk-service";      Context = "riskServices";                         Dockerfile = "riskServices\Dockerfile" },
    @{ Name = "team-service";      Context = "teamServices";                         Dockerfile = "teamServices\Dockerfile" },
    @{ Name = "timesheet-service"; Context = "timeSheetServices";                    Dockerfile = "timeSheetServices\Dockerfile" },
    @{ Name = "workorder-service"; Context = "workorderServices\WorkOrderService";   Dockerfile = "workorderServices\WorkOrderService\Dockerfile" },
    @{ Name = "erp-gateway";       Context = "Gateway";                              Dockerfile = "Gateway\Dockerfile" }
)

foreach ($svc in $services) {
    $image   = "$Registry/erp/$($svc.Name):$Tag"
    $context = Join-Path $root $svc.Context
    $df      = Join-Path $root $svc.Dockerfile
    Write-Host "`n==> Building $image" -ForegroundColor Cyan
    docker build -t $image -f $df $context
    if ($LASTEXITCODE -ne 0) { Write-Error "Build failed for $($svc.Name)"; exit 1 }

    Write-Host "==> Pushing $image" -ForegroundColor Cyan
    docker push $image
    if ($LASTEXITCODE -ne 0) { Write-Error "Push failed for $($svc.Name)"; exit 1 }
}

Write-Host "`nAll images built and pushed successfully." -ForegroundColor Green
