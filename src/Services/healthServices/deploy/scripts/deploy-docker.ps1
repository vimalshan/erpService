# ================================================
# Health ERP - Docker Compose Deployment Script (PowerShell)
# ================================================
param(
    [ValidateSet("up", "down", "restart", "logs", "status", "clean")]
    [string]$Action = "up",
    [string]$Service = ""
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$DeployDir = Split-Path -Parent $ScriptDir
$RootDir = Split-Path -Parent $DeployDir

function Write-Info  { param($msg) Write-Host "[INFO] $msg" -ForegroundColor Green }
function Write-Warn  { param($msg) Write-Host "[WARN] $msg" -ForegroundColor Yellow }
function Write-Err   { param($msg) Write-Host "[ERROR] $msg" -ForegroundColor Red }

# Pre-checks
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Err "Docker is not installed"; exit 1
}

# .env setup
$envFile = Join-Path $DeployDir ".env"
if (-not (Test-Path $envFile)) {
    $template = Join-Path $DeployDir ".env.template"
    if (Test-Path $template) {
        Write-Warn ".env file not found. Copying from .env.template..."
        Copy-Item $template $envFile
        Write-Warn "Please update $envFile with your actual passwords before running in production!"
    } else {
        Write-Err ".env file not found and no template available."; exit 1
    }
}

Push-Location $DeployDir
try {
    switch ($Action) {
        "up" {
            Write-Info "Starting Health ERP services..."
            docker compose --env-file .env up -d --build
            Start-Sleep -Seconds 10
            Write-Info "Checking service health..."
            docker compose ps
            Write-Host ""
            Write-Info "Services are starting up. Check health at:"
            Write-Host "  API Gateway:   http://localhost:5600/health"
            Write-Host "  RabbitMQ Mgmt: http://localhost:15672"
        }
        "down" {
            Write-Info "Stopping Health ERP services..."
            docker compose down
            Write-Info "All services stopped."
        }
        "restart" {
            Write-Info "Restarting Health ERP services..."
            docker compose down
            docker compose --env-file .env up -d --build
            Write-Info "Services restarted."
        }
        "logs" {
            if ($Service) {
                docker compose logs -f $Service
            } else {
                docker compose logs -f
            }
        }
        "status" {
            docker compose ps
        }
        "clean" {
            $confirm = Read-Host "This will remove all containers, volumes, and images! Are you sure? (y/N)"
            if ($confirm -eq "y" -or $confirm -eq "Y") {
                docker compose down -v --rmi local
                Write-Info "Cleanup complete."
            } else {
                Write-Info "Cleanup cancelled."
            }
        }
    }
} finally {
    Pop-Location
}
