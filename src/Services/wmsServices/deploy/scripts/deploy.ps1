# =============================================================================
# WMS Microservices - Full Deployment Script (PowerShell)
# Deploys infrastructure + all services via Docker Compose
# =============================================================================
param(
    [ValidateSet("up", "down", "restart", "logs", "status", "clean", "init-db")]
    [string]$Action = "up",
    [string]$Service = ""
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RootDir = Resolve-Path "$ScriptDir\..\.."

Write-Host "==============================================" -ForegroundColor Cyan
Write-Host " WMS Microservices - Deployment" -ForegroundColor Cyan
Write-Host " Action: $Action" -ForegroundColor Cyan
Write-Host "==============================================" -ForegroundColor Cyan

Set-Location $RootDir

# Load .env file
$envFile = Get-Content ".env" -ErrorAction SilentlyContinue
$envVars = @{}
foreach ($line in $envFile) {
    if ($line -match '^\s*([^#][^=]+)=(.*)$') {
        $envVars[$matches[1].Trim()] = $matches[2].Trim()
    }
}

switch ($Action) {
    "up" {
        Write-Host ""
        Write-Host "[1/4] Building all Docker images..." -ForegroundColor Yellow
        docker compose build --parallel

        Write-Host ""
        Write-Host "[2/4] Starting infrastructure (SQL Server + RabbitMQ)..." -ForegroundColor Yellow
        docker compose up -d sqlserver rabbitmq
        Write-Host "Waiting for infrastructure to be healthy..."

        Write-Host ""
        Write-Host "[3/4] Initializing databases..." -ForegroundColor Yellow
        Write-Host "Waiting for SQL Server to accept connections..."
        $ready = $false
        for ($i = 1; $i -le 30; $i++) {
            try {
                $result = docker exec wms-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $envVars["SA_PASSWORD"] -C -Q "SELECT 1" 2>$null
                if ($LASTEXITCODE -eq 0) {
                    Write-Host "SQL Server is ready." -ForegroundColor Green
                    $ready = $true
                    break
                }
            } catch {}
            Write-Host "  Attempt $i/30 - waiting..." -ForegroundColor Gray
            Start-Process -Wait -NoNewWindow -FilePath "timeout" -ArgumentList "/t", "5", "/nobreak" -RedirectStandardOutput "NUL" 2>$null
        }

        # Run database init script
        $initScript = Join-Path $RootDir "deploy\sql\init-databases.sql"
        if (Test-Path $initScript) {
            Write-Host "Running database initialization script..." -ForegroundColor Yellow
            Get-Content $initScript | docker exec -i wms-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $envVars["SA_PASSWORD"] -C
            Write-Host "Databases created." -ForegroundColor Green
        }

        Write-Host ""
        Write-Host "[4/4] Starting all microservices..." -ForegroundColor Yellow
        docker compose up -d

        Write-Host ""
        Write-Host "==============================================" -ForegroundColor Cyan
        Write-Host " Deployment complete!" -ForegroundColor Green
        Write-Host "==============================================" -ForegroundColor Cyan
        Write-Host ""
        Write-Host " API Gateway:      http://localhost:5000" -ForegroundColor White
        Write-Host " RabbitMQ Console:  http://localhost:15672" -ForegroundColor White
        Write-Host " SQL Server:        localhost:1433" -ForegroundColor White
        Write-Host ""
        Write-Host " Health check:      http://localhost:5000/health" -ForegroundColor White
        Write-Host " Service info:      http://localhost:5000/" -ForegroundColor White
        Write-Host ""
        docker compose ps
    }

    "down" {
        Write-Host "Stopping all services..." -ForegroundColor Yellow
        docker compose down
        Write-Host "All services stopped." -ForegroundColor Green
    }

    "restart" {
        Write-Host "Restarting all services..." -ForegroundColor Yellow
        docker compose restart
        Write-Host "All services restarted." -ForegroundColor Green
    }

    "logs" {
        if ($Service) {
            docker compose logs -f $Service
        } else {
            docker compose logs -f --tail=100
        }
    }

    "status" {
        docker compose ps
        Write-Host ""
        Write-Host "Health Checks:" -ForegroundColor Yellow
        try {
            $health = Invoke-WebRequest -Uri "http://localhost:5000/health" -UseBasicParsing -TimeoutSec 5
            Write-Host "  Gateway: $($health.StatusCode) - $($health.Content)" -ForegroundColor Green
        } catch {
            Write-Host "  Gateway: DOWN" -ForegroundColor Red
        }
    }

    "clean" {
        Write-Host "WARNING: This will remove all containers, volumes, and images!" -ForegroundColor Red
        $confirm = Read-Host "Are you sure? (y/N)"
        if ($confirm -eq "y" -or $confirm -eq "Y") {
            docker compose down -v --rmi local
            Write-Host "Cleaned up all resources." -ForegroundColor Green
        } else {
            Write-Host "Aborted." -ForegroundColor Yellow
        }
    }

    "init-db" {
        Write-Host "Running database initialization script..." -ForegroundColor Yellow
        $initScript = Join-Path $RootDir "deploy\sql\init-databases.sql"
        Get-Content $initScript | docker exec -i wms-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $envVars["SA_PASSWORD"] -C
        Write-Host "Databases initialized." -ForegroundColor Green
    }
}
