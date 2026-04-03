# =============================================================================
# deploy.ps1 — Build, tag, and run all Loan ERP services via Docker Compose
# Usage:
#   .\deploy.ps1 [build|up|down|restart|logs|status]
#   $env:REGISTRY="myregistry.io/loan"; .\deploy.ps1 build
# =============================================================================
param(
    [ValidateSet("build","up","down","restart","logs","status")]
    [string]$Command = "up",
    [string]$ServiceName = "",
    [string]$Tag = "latest"
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $ScriptDir

$Registry = $env:REGISTRY
$Services = @{
    "loan-transaction" = "loanTransactionServices"
    "loan-application" = "loanapplicationServices"
    "loan-account"     = "loanaccountServices"
    "loan-definition"  = "loandefinitionServices"
    "document-service" = "documentServices"
    "lov-service"      = "lovServices"
    "utility-service"  = "utilityServices"
    "api-gateway"      = "apiGateway"
}

$Ports = @{
    "LoanTransaction" = 5292
    "LoanApplication" = 5282
    "LoanAccount"     = 5150
    "LoanDefinition"  = 5077
    "DocumentService" = 5280
    "LovService"      = 5008
    "UtilityService"  = 5143
    "ApiGateway"      = 6100
}

function Write-Info  { Write-Host "[INFO]  $args" -ForegroundColor Green }
function Write-Warn  { Write-Host "[WARN]  $args" -ForegroundColor Yellow }
function Write-Err   { Write-Host "[ERROR] $args" -ForegroundColor Red; exit 1 }

function Invoke-Build {
    Write-Info "Building all service images..."
    docker compose build --parallel
    if ($LASTEXITCODE -ne 0) { Write-Err "docker compose build failed" }
    Write-Info "Build complete."

    if ($Registry) {
        Write-Info "Tagging and pushing to $Registry ..."
        foreach ($name in $Services.Keys) {
            $localImage  = "${name}:${Tag}"
            $remoteImage = "${Registry}/${name}:${Tag}"
            docker tag $localImage $remoteImage
            docker push $remoteImage
            Write-Info "  Pushed $remoteImage"
        }
    }
}

function Invoke-Up {
    if (-not (Test-Path .env)) {
        Write-Warn ".env not found — copying .env.example -> .env"
        Copy-Item .env.example .env
    }
    Write-Info "Starting all services..."
    docker compose up -d
    if ($LASTEXITCODE -ne 0) { Write-Err "docker compose up failed" }
    Write-Info "Waiting for health checks (15s)..."
    Start-Sleep -Seconds 15
    Invoke-Status
}

function Invoke-Down {
    Write-Info "Stopping all services..."
    docker compose down
}

function Invoke-Logs {
    if ($ServiceName) {
        docker compose logs -f $ServiceName
    } else {
        docker compose logs -f
    }
}

function Invoke-Status {
    Write-Host ""
    Write-Info "Container status:"
    docker compose ps
    Write-Host ""
    Write-Info "Health endpoints:"
    foreach ($svc in $Ports.Keys) {
        $port = $Ports[$svc]
        try {
            $resp = Invoke-WebRequest -Uri "http://localhost:$port/health" -UseBasicParsing -TimeoutSec 3 -ErrorAction Stop
            Write-Host "  [OK] $svc :$port" -ForegroundColor Green
        } catch {
            Write-Host "  [FAIL] $svc :$port" -ForegroundColor Red
        }
    }
}

switch ($Command) {
    "build"   { Invoke-Build }
    "up"      { Invoke-Build; Invoke-Up }
    "down"    { Invoke-Down }
    "restart" { Invoke-Down; Invoke-Up }
    "logs"    { Invoke-Logs }
    "status"  { Invoke-Status }
}
