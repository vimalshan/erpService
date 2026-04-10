# ============================================================================
# SPARSH Platform - Docker Compose Deployment Script (PowerShell)
# Usage: .\scripts\deploy-docker.ps1 -Action [up|down|restart|logs|status]
# ============================================================================
param(
    [ValidateSet("up", "down", "restart", "logs", "status")]
    [string]$Action = "up",
    [string]$Service = ""
)

$ErrorActionPreference = "Stop"
$RootDir = Split-Path -Parent $PSScriptRoot
$ComposeFile = Join-Path $RootDir "docker-compose.yml"

# Load .env if present
$envFile = Join-Path $RootDir ".env"
if (Test-Path $envFile) {
    Get-Content $envFile | Where-Object { $_ -match '^\s*[^#]' } | ForEach-Object {
        $parts = $_ -split '=', 2
        if ($parts.Count -eq 2) {
            [Environment]::SetEnvironmentVariable($parts[0].Trim(), $parts[1].Trim(), "Process")
        }
    }
}

switch ($Action) {
    "up" {
        Write-Host "Starting SPARSH platform..."
        docker compose -f $ComposeFile up -d --build
        Write-Host "`nServices starting. Check status with: $($MyInvocation.MyCommand.Name) -Action status"
        Write-Host "API Gateway: http://localhost:5200"
        Write-Host "RabbitMQ UI: http://localhost:15672"
    }
    "down" {
        Write-Host "Stopping SPARSH platform..."
        docker compose -f $ComposeFile down
        Write-Host "Stopped."
    }
    "restart" {
        if ($Service) {
            Write-Host "Restarting $Service..."
            docker compose -f $ComposeFile restart $Service
        } else {
            Write-Host "Restarting all services..."
            docker compose -f $ComposeFile down
            docker compose -f $ComposeFile up -d --build
        }
    }
    "logs" {
        if ($Service) {
            docker compose -f $ComposeFile logs -f $Service
        } else {
            docker compose -f $ComposeFile logs -f
        }
    }
    "status" {
        Write-Host "SPARSH Platform - Service Status"
        Write-Host "================================="
        docker compose -f $ComposeFile ps
    }
}
