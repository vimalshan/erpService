# ==========================================
# SRF Sparsh - Docker Compose Deploy (PowerShell)
# ==========================================

param(
    [ValidateSet("up", "down", "restart", "logs", "status", "infra", "clean")]
    [string]$Action = "up",
    [string]$Service = ""
)

$ErrorActionPreference = "Stop"
$RootDir = Resolve-Path "$PSScriptRoot\.."
Set-Location $RootDir

# Check .env file
if (-not (Test-Path ".env")) {
    Write-Host "Creating .env from .env.example..." -ForegroundColor Yellow
    Copy-Item ".env.example" ".env"
    Write-Host "IMPORTANT: Update .env with production values!" -ForegroundColor Red
}

Write-Host "============================================" -ForegroundColor Cyan
Write-Host " SRF Sparsh - Docker Compose: $Action" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan

switch ($Action) {
    "up" {
        Write-Host "Starting all services..." -ForegroundColor Green
        docker compose up -d --build
        Write-Host "`nWaiting for services..." -ForegroundColor Yellow
        Start-Sleep -Seconds 10
        docker compose ps
        Write-Host "`nGateway: http://localhost:5100" -ForegroundColor Green
        Write-Host "RabbitMQ: http://localhost:15672" -ForegroundColor Green
    }
    "down" {
        Write-Host "Stopping all services..." -ForegroundColor Yellow
        docker compose down
    }
    "restart" {
        Write-Host "Restarting all services..." -ForegroundColor Yellow
        docker compose down
        docker compose up -d --build
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
    "infra" {
        Write-Host "Starting infrastructure only..." -ForegroundColor Green
        docker compose up -d sqlserver rabbitmq redis azurite
    }
    "clean" {
        Write-Host "Stopping and removing all data..." -ForegroundColor Red
        docker compose down -v --remove-orphans
    }
}
