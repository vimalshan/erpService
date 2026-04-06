<# 
.SYNOPSIS
    ERP Microservice - Docker Compose Deploy Script (PowerShell)
.DESCRIPTION
    Deploys all ERP microservices using Docker Compose
.PARAMETER Action
    The action to perform: build, up, down, restart, logs, status, clean
.EXAMPLE
    .\deploy-docker.ps1 up
    .\deploy-docker.ps1 status
    .\deploy-docker.ps1 logs employee-service
#>

param(
    [ValidateSet("build", "up", "down", "restart", "logs", "status", "clean")]
    [string]$Action = "up",
    [string]$Service = ""
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ComposeFile = Join-Path $ScriptDir "docker-compose.yml"
$EnvFile = Join-Path $ScriptDir ".env"
$EnvExample = Join-Path $ScriptDir ".env.example"

function Write-Info($msg)  { Write-Host "[INFO] $msg" -ForegroundColor Blue }
function Write-Ok($msg)    { Write-Host "[OK] $msg" -ForegroundColor Green }
function Write-Warn($msg)  { Write-Host "[WARN] $msg" -ForegroundColor Yellow }
function Write-Err($msg)   { Write-Host "[ERROR] $msg" -ForegroundColor Red }

function Test-Dependencies {
    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        Write-Err "Docker is not installed"
        exit 1
    }
    Write-Ok "Dependencies verified"
}

function Initialize-Env {
    if (-not (Test-Path $EnvFile)) {
        Write-Warn ".env file not found, creating from .env.example"
        if (Test-Path $EnvExample) {
            Copy-Item $EnvExample $EnvFile
            Write-Ok "Created .env from .env.example - update secrets before production use"
        } else {
            Write-Err ".env.example not found"
            exit 1
        }
    }
}

function Invoke-Compose {
    param([string[]]$Args)
    docker compose -f $ComposeFile --env-file $EnvFile @Args
}

function Start-Build {
    Write-Info "Building all service images..."
    Invoke-Compose @("build", "--parallel")
    Write-Ok "All images built"
}

function Start-Up {
    Write-Info "Starting ERP Microservice stack..."
    Initialize-Env
    Invoke-Compose @("up", "-d")
    Write-Ok "Stack started"
    Write-Host ""
    Write-Info "Service endpoints:"
    Write-Host "  API Gateway:              http://localhost:5100"
    Write-Host "  Employee Service:         http://localhost:5104"
    Write-Host "  HR Service:               http://localhost:5000"
    Write-Host "  FAQ Service:              http://localhost:5032"
    Write-Host "  Payroll Service:          http://localhost:5002"
    Write-Host "  Tax Service:              http://localhost:5010"
    Write-Host "  PayTransactional Service: http://localhost:5020"
    Write-Host "  RabbitMQ Management:      http://localhost:15672"
    Write-Host ""
    Write-Info "Health check: http://localhost:5100/health/services"
}

function Start-Down {
    Write-Info "Stopping ERP Microservice stack..."
    Invoke-Compose @("down")
    Write-Ok "Stack stopped"
}

function Start-Restart {
    Start-Down
    Start-Up
}

function Start-Logs {
    if ($Service) {
        Invoke-Compose @("logs", "-f", $Service)
    } else {
        Invoke-Compose @("logs", "-f")
    }
}

function Start-Status {
    Write-Info "Service status:"
    Invoke-Compose @("ps")
    Write-Host ""
    Write-Info "Checking health endpoints..."
    $services = @(
        @{Port=5100; Name="API-Gateway"},
        @{Port=5104; Name="Employee"},
        @{Port=5000; Name="HR"},
        @{Port=5032; Name="FAQ"},
        @{Port=5002; Name="Payroll"},
        @{Port=5010; Name="Tax"},
        @{Port=5020; Name="PayTransactional"}
    )
    foreach ($svc in $services) {
        try {
            $response = Invoke-WebRequest -Uri "http://localhost:$($svc.Port)/health" -TimeoutSec 5 -ErrorAction SilentlyContinue
            if ($response.StatusCode -eq 200) {
                Write-Ok "$($svc.Name) (port $($svc.Port)): healthy"
            } else {
                Write-Warn "$($svc.Name) (port $($svc.Port)): HTTP $($response.StatusCode)"
            }
        } catch {
            Write-Warn "$($svc.Name) (port $($svc.Port)): unreachable"
        }
    }
}

function Start-Clean {
    Write-Info "Stopping and removing all containers, volumes, and images..."
    Invoke-Compose @("down", "-v", "--rmi", "local")
    Write-Ok "Clean complete"
}

# Main
Test-Dependencies

switch ($Action) {
    "build"   { Start-Build }
    "up"      { Start-Up }
    "down"    { Start-Down }
    "restart" { Start-Restart }
    "logs"    { Start-Logs }
    "status"  { Start-Status }
    "clean"   { Start-Clean }
}
