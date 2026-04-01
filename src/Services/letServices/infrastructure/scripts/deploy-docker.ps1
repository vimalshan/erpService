# ═══════════════════════════════════════════════════════════════════════
# deploy-docker.ps1 — Deploy LET ERP via Docker Compose
# Usage: .\deploy-docker.ps1 [-Action up|down|restart|logs|status|clean]
# ═══════════════════════════════════════════════════════════════════════
param(
    [ValidateSet("up", "down", "restart", "logs", "status", "clean")]
    [string]$Action = "up",
    [string]$Service = ""
)

$ErrorActionPreference = "Stop"
$RootDir = Resolve-Path "$PSScriptRoot\..\.."
$ComposeFile = Join-Path $RootDir "docker-compose.yml"
$EnvFile = Join-Path $RootDir ".env"

# Load .env if exists
if (Test-Path $EnvFile) {
    Write-Host "Loading environment from .env"
    Get-Content $EnvFile | ForEach-Object {
        if ($_ -match '^([^#=]+)=(.*)$') {
            [System.Environment]::SetEnvironmentVariable($Matches[1].Trim(), $Matches[2].Trim(), "Process")
        }
    }
}

switch ($Action) {
    "up" {
        Write-Host "=== Starting LET ERP Stack ===" -ForegroundColor Cyan
        docker compose -f $ComposeFile up -d --build
        Write-Host "`nWaiting for services to be healthy..."
        Start-Sleep -Seconds 10
        docker compose -f $ComposeFile ps
        Write-Host "`n=== Stack is running ===" -ForegroundColor Green
        Write-Host "  API Gateway:     http://localhost:5400"
        Write-Host "  RabbitMQ Mgmt:   http://localhost:15672"
        Write-Host "  SQL Server:      localhost:1433"
    }
    "down" {
        Write-Host "=== Stopping LET ERP Stack ===" -ForegroundColor Yellow
        docker compose -f $ComposeFile down
        Write-Host "Stack stopped."
    }
    "restart" {
        Write-Host "=== Restarting LET ERP Stack ===" -ForegroundColor Yellow
        docker compose -f $ComposeFile down
        docker compose -f $ComposeFile up -d --build
        Write-Host "Stack restarted."
    }
    "logs" {
        if ($Service) {
            docker compose -f $ComposeFile logs -f $Service
        } else {
            docker compose -f $ComposeFile logs -f
        }
    }
    "status" {
        docker compose -f $ComposeFile ps
    }
    "clean" {
        Write-Host "=== Removing all containers, volumes, and images ===" -ForegroundColor Red
        docker compose -f $ComposeFile down -v --rmi local
        Write-Host "Cleaned."
    }
}
