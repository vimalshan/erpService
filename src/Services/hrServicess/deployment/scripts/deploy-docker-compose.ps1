# Docker Compose Deployment Script
# Builds and deploys all services using Docker Compose

param(
    [string]$Action = "up",  # up, down, restart, logs, status
    [string]$Service = $null,
    [switch]$Rebuild = $false,
    [switch]$Detached = $true
)

$ErrorActionPreference = "Stop"
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent (Split-Path -Parent $scriptPath)
$dockerComposeFile = Join-Path $scriptPath "docker-compose.yml"

Write-Host "HR Microservices Docker Compose Deployment" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host "Action: $Action" -ForegroundColor Cyan
Write-Host "Project Root: $projectRoot`n" -ForegroundColor Cyan

# Validate docker-compose file exists
if (-not (Test-Path $dockerComposeFile)) {
    Write-Host "ERROR: docker-compose.yml not found at $dockerComposeFile" -ForegroundColor Red
    exit 1
}

# Build arguments
$composeArgs = @("-f", $dockerComposeFile)
if ($Service) { $composeArgs += $Service }

switch ($Action) {
    "up" {
        Write-Host "Starting all services..." -ForegroundColor Cyan
        $buildArgs = if ($Rebuild) { @("--build") } else { @() }
        $detachArg = if ($Detached) { @("-d") } else { @() }
        
        & docker-compose @composeArgs up @detachArg @buildArgs
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Services started successfully!" -ForegroundColor Green
            Write-Host "Waiting 10 seconds for services to initialize..." -ForegroundColor Yellow
            Start-Sleep -Seconds 10
            
            # Display service status
            & docker-compose @composeArgs ps
        }
    }
    
    "down" {
        Write-Host "Stopping all services..." -ForegroundColor Cyan
        & docker-compose @composeArgs down
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Services stopped successfully!" -ForegroundColor Green
        }
    }
    
    "restart" {
        Write-Host "Restarting services..." -ForegroundColor Cyan
        & docker-compose @composeArgs restart
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Services restarted successfully!" -ForegroundColor Green
        }
    }
    
    "logs" {
        Write-Host "Displaying logs (press Ctrl+C to stop)..." -ForegroundColor Cyan
        $tailArg = @("-f", "--tail=100")
        & docker-compose @composeArgs logs @tailArg
    }
    
    "status" {
        Write-Host "Checking service status..." -ForegroundColor Cyan
        & docker-compose @composeArgs ps
        Write-Host "`nChecking health endpoints..." -ForegroundColor Cyan
        
        $services = @(
            @{name="API Gateway"; port=5310; path="/health"},
            @{name="AlertsNotifications"; port=5154; path="/health"},
            @{name="CompensationBenefits"; port=5009; path="/health"},
            @{name="EmployeeManagement"; port=5004; path="/health"},
            @{name="EmployeeRelations"; port=5075; path="/health"},
            @{name="ExitManagement"; port=5094; path="/health"},
            @{name="OrganizationStructure"; port=5027; path="/health"},
            @{name="Recruitment"; port=5265; path="/health"},
            @{name="TimeAttendance"; port=5235; path="/health"},
            @{name="TrainingDevelopment"; port=5003; path="/health"},
            @{name="UserSecurity"; port=5140; path="/health"},
            @{name="EmployeeTransactions"; port=5204; path="/health"}
        )
        
        foreach ($service in $services) {
            try {
                $response = Invoke-RestMethod -Uri "http://localhost:$($service.port)$($service.path)" -Method Get -ErrorAction SilentlyContinue
                Write-Host "✓ $($service.name) : HEALTHY" -ForegroundColor Green
            }
            catch {
                Write-Host "✗ $($service.name) : UNHEALTHY or UNAVAILABLE" -ForegroundColor Red
            }
        }
    }
    
    "build" {
        Write-Host "Building all Docker images..." -ForegroundColor Cyan
        & docker-compose @composeArgs build
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "All images built successfully!" -ForegroundColor Green
        }
    }
    
    default {
        Write-Host "ERROR: Unknown action '$Action'" -ForegroundColor Red
        Write-Host "Valid actions: up, down, restart, logs, status, build" -ForegroundColor Yellow
        exit 1
    }
}

Write-Host "`nDeployment script completed." -ForegroundColor Green
