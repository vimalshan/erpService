# ─── Deploy SSC Services using Docker Compose (PowerShell) ────────────────
param(
    [ValidateSet("up", "down", "restart", "logs", "status", "init-db")]
    [string]$Action = "up",
    [string]$ServiceName
)

$ErrorActionPreference = "Stop"
$RootDir = Split-Path -Parent $PSScriptRoot
Push-Location $RootDir

try {
    switch ($Action) {
        "up" {
            Write-Host "============================================" -ForegroundColor Cyan
            Write-Host "Starting SSC Services (Docker Compose)"
            Write-Host "============================================" -ForegroundColor Cyan

            if (-not (Test-Path ".env")) {
                Write-Host "Creating .env from .env.example..."
                Copy-Item ".env.example" ".env"
                Write-Host "WARNING: Using default credentials. Update .env for production!" -ForegroundColor Yellow
            }

            docker compose up -d --build

            Write-Host ""
            Write-Host "Service endpoints:" -ForegroundColor Green
            Write-Host "  SSC Transactional:   http://localhost:8080"
            Write-Host "  Batch & Envelope:    http://localhost:8081"
            Write-Host "  Category & Vendor:   http://localhost:8082"
            Write-Host "  Club Membership:     http://localhost:8083"
            Write-Host "  Filing & Archive:    http://localhost:8084"
            Write-Host "  HR Document:         http://localhost:8085"
            Write-Host "  Integration:         http://localhost:8086"
            Write-Host "  Invoice Processing:  http://localhost:8087"
            Write-Host "  Master Data:         http://localhost:8088"
            Write-Host "  Menu & Security:     http://localhost:8089"
            Write-Host "  Approval Group:      http://localhost:8090"
            Write-Host "  User Service:        http://localhost:8091"
            Write-Host ""
            Write-Host "  API Gateway:         http://localhost:5000"
            Write-Host "  Gateway Health:      http://localhost:5000/health"
            Write-Host ""
            Write-Host "  RabbitMQ Management: http://localhost:15672"
            Write-Host "  SQL Server:          localhost:1433"
        }
        "down" {
            Write-Host "Stopping SSC Services..."
            docker compose down
            Write-Host "All services stopped." -ForegroundColor Green
        }
        "restart" {
            Write-Host "Restarting SSC Services..."
            docker compose down
            docker compose up -d --build
            Write-Host "Services restarted." -ForegroundColor Green
        }
        "logs" {
            if ($ServiceName) {
                docker compose logs -f $ServiceName
            } else {
                docker compose logs -f
            }
        }
        "status" {
            docker compose ps
        }
        "init-db" {
            Write-Host "Initializing database..."
            docker compose exec sqlserver bash /docker-entrypoint-initdb.d/init-db.sh
            Write-Host "Database initialized." -ForegroundColor Green
        }
    }
}
finally {
    Pop-Location
}
