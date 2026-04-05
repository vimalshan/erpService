# ─────────────────────────────────────────────────────────────────────────────
# start-local.ps1
# Starts all 8 microservices + ERP Gateway in separate background processes
# for local development WITHOUT Docker.
# Usage: .\start-local.ps1
#        .\start-local.ps1 -Services audit,risk   (start specific services only)
#        .\start-local.ps1 -Stop                  (kill all previously started processes)
# ─────────────────────────────────────────────────────────────────────────────
param(
    [string[]]$Services,
    [switch]$Stop
)

$root = Resolve-Path "$PSScriptRoot\..\.."
$pidFile = "$PSScriptRoot\.local-pids.txt"

$allServices = @(
    @{ Name = "audit";     Project = "auditServices\src\AuditService.API\AuditService.API.csproj";                             Port = 5037 },
    @{ Name = "batch";     Project = "batchServices\src\BatchService.API\BatchService.API.csproj";                             Port = 5111 },
    @{ Name = "csa";       Project = "csaServices\CSA.Service\src\CSA.Service.API\CSA.Service.API.csproj";                    Port = 5035 },
    @{ Name = "project";   Project = "projectServices\src\ProjectService.API\ProjectService.API.csproj";                       Port = 5290 },
    @{ Name = "risk";      Project = "riskServices\src\RiskService.API\RiskService.API.csproj";                               Port = 5033 },
    @{ Name = "team";      Project = "teamServices\src\TeamServices.API\TeamServices.API.csproj";                             Port = 5183 },
    @{ Name = "timesheet"; Project = "timeSheetServices\src\TimeSheetService.API\TimeSheetService.API.csproj";               Port = 5188 },
    @{ Name = "workorder"; Project = "workorderServices\WorkOrderService\src\WorkOrderService.API\WorkOrderService.API.csproj"; Port = 5138 },
    @{ Name = "gateway";   Project = "Gateway\src\ERPGateway\ERPGateway.csproj";                                              Port = 5000 }
)

# ── Stop mode ────────────────────────────────────────────────────────────────
if ($Stop) {
    if (Test-Path $pidFile) {
        Get-Content $pidFile | ForEach-Object {
            $pid_ = [int]$_
            try { Stop-Process -Id $pid_ -Force -ErrorAction Stop; Write-Host "Stopped PID $pid_" }
            catch { Write-Host "PID $pid_ already gone." }
        }
        Remove-Item $pidFile
    }
    Write-Host "All local services stopped." -ForegroundColor Green
    return
}

# ── Filter requested services ────────────────────────────────────────────────
$toStart = if ($Services) {
    $allServices | Where-Object { $Services -contains $_.Name }
} else {
    $allServices
}

$pids = @()

foreach ($svc in $toStart) {
    $csproj = Join-Path $root $svc.Project
    if (-not (Test-Path $csproj)) {
        Write-Warning "Project not found, skipping: $csproj"
        continue
    }
    Write-Host "Starting $($svc.Name) on port $($svc.Port)..." -ForegroundColor Cyan
    $proc = Start-Process -FilePath "dotnet" `
        -ArgumentList "run --project `"$csproj`" --urls http://localhost:$($svc.Port)" `
        -PassThru -WindowStyle Minimized
    $pids += $proc.Id
    Write-Host "  PID $($proc.Id)"
}

$pids | Out-File $pidFile -Encoding utf8
Write-Host "`nAll services started. PIDs saved to $pidFile" -ForegroundColor Green
Write-Host "Gateway:   http://localhost:5000"
Write-Host "RabbitMQ:  http://localhost:15672  (if running locally)"
Write-Host "`nRun '.\start-local.ps1 -Stop' to stop all services."
