# ==============================================================================
# ERP Travel Services - Docker Compose Deployment Script (Windows PowerShell)
# ==============================================================================

param(
    [Parameter(Position=0)]
    [ValidateSet("up","down","build","start","stop","restart","logs","status","health","init-db","clean","help")]
    [string]$Command = "help"
)

$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot

# Colors via Write-Host
function Write-Step($msg)  { Write-Host "[$(Get-Date -Format 'HH:mm:ss')] $msg" -ForegroundColor Green }
function Write-Warn($msg)  { Write-Host "[WARNING] $msg" -ForegroundColor Yellow }
function Write-Err($msg)   { Write-Host "[ERROR] $msg" -ForegroundColor Red }
function Write-Header($msg) {
    Write-Host ""
    Write-Host "============================================" -ForegroundColor Cyan
    Write-Host "  $msg" -ForegroundColor Cyan
    Write-Host "============================================" -ForegroundColor Cyan
    Write-Host ""
}

function Show-Usage {
    Write-Host @"
Usage: .\deploy.ps1 [COMMAND]

Commands:
  up          Build and start all services
  down        Stop and remove all containers
  build       Build all Docker images
  start       Start existing containers
  stop        Stop running containers
  restart     Restart all services
  logs        Show logs for all services
  status      Show status of all services
  health      Check health of all services
  init-db     Initialize databases
  clean       Remove all containers, images, and volumes
"@
}

function Test-Prerequisites {
    Write-Step "Checking prerequisites..."

    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        Write-Err "Docker is not installed"; exit 1
    }

    $dockerInfo = docker info 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Err "Docker daemon is not running"; exit 1
    }

    Write-Step "Prerequisites OK"
}

function Setup-Env {
    if (-not (Test-Path ".env")) {
        if (Test-Path ".env.example") {
            Copy-Item ".env.example" ".env"
            Write-Warn ".env created from .env.example - review and update values before production use"
        }
    }
}

function Copy-SqlScripts {
    Write-Step "Copying SQL scripts for database initialization..."
    $initDir = "deploy\init-db"

    # Root SQL files
    Copy-Item -Path "TRAVELDB.sql" -Destination $initDir -Force -ErrorAction SilentlyContinue
    Copy-Item -Path "TRAVELDB-procedures.sql" -Destination $initDir -Force -ErrorAction SilentlyContinue

    # Service-specific SQL
    $sqlMappings = @{
        "travelRequestServices\01-TravelRequest"  = "01-TravelRequest"
        "bookingServices\02-Booking"               = "02-Booking"
        "agensService\03-Agency"                   = "03-Agency"
        "expenseServices\04-Expense"               = "04-Expense"
        "financeServices\05-Finance"               = "05-Finance"
        "adminServices\06-Admin"                   = "06-Admin"
        "masterdataServices\07-MasterData"         = "07-MasterData"
        "insuranceServices\08-Insurance"           = "08-Insurance"
    }

    foreach ($src in $sqlMappings.Keys) {
        $dest = Join-Path $initDir $sqlMappings[$src]
        if (Test-Path $src) {
            New-Item -ItemType Directory -Path $dest -Force | Out-Null
            Copy-Item -Path "$src\*.sql" -Destination $dest -Force -ErrorAction SilentlyContinue
        }
    }

    Write-Step "SQL scripts copied"
}

function Invoke-Build {
    Write-Header "Building Docker Images"
    docker compose build --parallel
    Write-Step "All images built successfully"
}

function Invoke-Up {
    Write-Header "Starting ERP Travel Services"
    Test-Prerequisites
    Setup-Env
    Copy-SqlScripts

    Write-Step "Starting infrastructure (SQL Server, RabbitMQ, Azurite)..."
    docker compose up -d sqlserver rabbitmq azurite

    Write-Step "Waiting for SQL Server to be healthy..."
    $retries = 0
    do {
        $retries++
        Start-Sleep -Seconds 3
        $result = docker compose exec -T sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "`$MSSQL_SA_PASSWORD" -C -Q "SELECT 1" 2>&1
    } while ($LASTEXITCODE -ne 0 -and $retries -lt 40)

    if ($retries -ge 40) { Write-Err "SQL Server failed to start"; exit 1 }
    Write-Step "SQL Server is ready"

    Invoke-InitDb

    Write-Step "Starting all microservices..."
    docker compose up -d

    Write-Step "All services started!"
    Invoke-Status
}

function Invoke-Down {
    Write-Header "Stopping ERP Travel Services"
    docker compose down
    Write-Step "All services stopped"
}

function Invoke-Start {
    Write-Header "Starting ERP Travel Services"
    docker compose start
    Write-Step "All services started"
}

function Invoke-Stop {
    Write-Header "Stopping ERP Travel Services"
    docker compose stop
    Write-Step "All services stopped"
}

function Invoke-Restart {
    Write-Header "Restarting ERP Travel Services"
    docker compose restart
    Write-Step "All services restarted"
}

function Invoke-Logs {
    docker compose logs -f --tail=100
}

function Invoke-Status {
    Write-Header "Service Status"
    docker compose ps
}

function Invoke-Health {
    Write-Header "Health Check"

    $services = @(
        @{ Name = "API Gateway";         Port = 5100 },
        @{ Name = "Travel Request";      Port = 5205 },
        @{ Name = "Travel Transaction";  Port = 5082 },
        @{ Name = "Booking";             Port = 5117 },
        @{ Name = "Expense";             Port = 5090 },
        @{ Name = "Finance";             Port = 5294 },
        @{ Name = "Insurance";           Port = 5179 },
        @{ Name = "MasterData";          Port = 5166 },
        @{ Name = "Agency";              Port = 5000 },
        @{ Name = "Admin";               Port = 5001 }
    )

    foreach ($svc in $services) {
        try {
            $resp = Invoke-WebRequest -Uri "http://localhost:$($svc.Port)/health" -TimeoutSec 5 -ErrorAction Stop
            Write-Host "  [OK] $($svc.Name) (port $($svc.Port)) - Healthy" -ForegroundColor Green
        }
        catch {
            Write-Host "  [FAIL] $($svc.Name) (port $($svc.Port)) - Unreachable" -ForegroundColor Red
        }
    }
    Write-Host ""
}

function Invoke-InitDb {
    Write-Step "Initializing databases..."
    docker compose exec -T sqlserver bash /docker-entrypoint-initdb.d/init-databases.sh 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Warn "DB init script returned non-zero - databases may already exist"
    }
}

function Invoke-Clean {
    Write-Header "Cleaning ERP Travel Services"
    Write-Warn "This will remove ALL containers, images, and volumes!"
    $confirm = Read-Host "Are you sure? (y/N)"
    if ($confirm -eq "y" -or $confirm -eq "Y") {
        docker compose down -v --rmi all
        Write-Step "Cleanup complete"
    } else {
        Write-Step "Cleanup cancelled"
    }
}

# Main
switch ($Command) {
    "up"       { Invoke-Up }
    "down"     { Invoke-Down }
    "build"    { Invoke-Build }
    "start"    { Invoke-Start }
    "stop"     { Invoke-Stop }
    "restart"  { Invoke-Restart }
    "logs"     { Invoke-Logs }
    "status"   { Invoke-Status }
    "health"   { Invoke-Health }
    "init-db"  { Invoke-InitDb }
    "clean"    { Invoke-Clean }
    default    { Show-Usage }
}
