<#
.SYNOPSIS
    Deploy ERP Microservices with Docker Compose
.PARAMETER Action
    Action to perform: up, down, restart, logs, status, clean
.PARAMETER Service
    Service name for logs command (optional)
#>
param(
    [ValidateSet("up", "down", "restart", "logs", "status", "clean")]
    [string]$Action = "up",
    [string]$Service = ""
)

$ErrorActionPreference = "Stop"
$RootDir = (Resolve-Path "$PSScriptRoot\..\..").Path
$EnvFile = "$PSScriptRoot\..\.env"
$ComposeFile = "$RootDir\docker-compose.yml"

Write-Host "============================================" -ForegroundColor Yellow
Write-Host "  ERP Microservices - Docker Compose Deploy"
Write-Host "============================================" -ForegroundColor Yellow

# Check .env
if (-not (Test-Path $EnvFile)) {
    Write-Host "ERROR: .env file not found at $EnvFile" -ForegroundColor Red
    Write-Host "Copy .env.example and fill in values:"
    Write-Host "  Copy-Item deploy\.env.example deploy\.env"
    exit 1
}

# Validate required vars
$envContent = Get-Content $EnvFile | Where-Object { $_ -match '=' -and $_ -notmatch '^\s*#' }
$envVars = @{}
foreach ($line in $envContent) {
    $parts = $line -split '=', 2
    $envVars[$parts[0].Trim()] = $parts[1].Trim()
}

$required = @("SQL_SA_PASSWORD", "RABBITMQ_DEFAULT_USER", "RABBITMQ_DEFAULT_PASS", "JWT_SECRET_KEY")
foreach ($var in $required) {
    if (-not $envVars.ContainsKey($var) -or [string]::IsNullOrWhiteSpace($envVars[$var])) {
        Write-Host "ERROR: $var is not set in .env" -ForegroundColor Red
        exit 1
    }
}
Write-Host "  OK Environment variables validated" -ForegroundColor Green

switch ($Action) {
    "up" {
        Write-Host "`nStarting all services..." -ForegroundColor Cyan
        docker compose -f $ComposeFile --env-file $EnvFile up -d --build
        Write-Host "`nServices started. Checking status..." -ForegroundColor Green
        Start-Sleep -Seconds 3
        docker compose -f $ComposeFile ps
        Write-Host "`nGateway: http://localhost:5000"
        Write-Host "RabbitMQ: http://localhost:15672"
    }
    "down" {
        Write-Host "`nStopping all services..." -ForegroundColor Cyan
        docker compose -f $ComposeFile --env-file $EnvFile down
        Write-Host "All services stopped." -ForegroundColor Green
    }
    "restart" {
        Write-Host "`nRestarting all services..." -ForegroundColor Cyan
        docker compose -f $ComposeFile --env-file $EnvFile down
        docker compose -f $ComposeFile --env-file $EnvFile up -d --build
        Write-Host "All services restarted." -ForegroundColor Green
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
        $answer = Read-Host "This will remove all containers, volumes, and images. Continue? [y/N]"
        if ($answer -eq "y" -or $answer -eq "Y") {
            docker compose -f $ComposeFile --env-file $EnvFile down -v --rmi local
            Write-Host "Cleaned up." -ForegroundColor Green
        }
    }
}
