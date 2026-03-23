@echo off
REM ============================================================
REM ERP Microservices - Docker Compose Deploy Script
REM ============================================================

set ACTION=%1
if "%ACTION%"=="" set ACTION=up

if "%ACTION%"=="up" (
    echo Starting ERP infrastructure and services...
    docker compose -f docker-compose.shared.yml -f docker-compose.yml up -d --build
    echo.
    echo Services are starting. Check status with:
    echo   docker compose -f docker-compose.shared.yml -f docker-compose.yml ps
    echo.
    echo Service endpoints:
    echo   API Gateway:             http://localhost:5020
    echo   Access Service:          http://localhost:5010
    echo   Attendance Service:      http://localhost:5011
    echo   Bus Service:             http://localhost:5012
    echo   Calendar Service:        http://localhost:5013
    echo   Employee Service:        http://localhost:5014
    echo   Group Incentive Service: http://localhost:5015
    echo   Leave Service:           http://localhost:5016
    echo   Reference Service:       http://localhost:5017
    echo   Visitor Service:         http://localhost:5018
    echo   RabbitMQ Management:     http://localhost:15672
    goto :eof
)

if "%ACTION%"=="down" (
    echo Stopping all services...
    docker compose -f docker-compose.shared.yml -f docker-compose.yml down
    goto :eof
)

if "%ACTION%"=="down-v" (
    echo Stopping all services and removing volumes...
    docker compose -f docker-compose.shared.yml -f docker-compose.yml down -v
    goto :eof
)

if "%ACTION%"=="ps" (
    docker compose -f docker-compose.shared.yml -f docker-compose.yml ps
    goto :eof
)

if "%ACTION%"=="logs" (
    set SVC=%2
    if "%SVC%"=="" (
        docker compose -f docker-compose.shared.yml -f docker-compose.yml logs -f --tail=100
    ) else (
        docker compose -f docker-compose.shared.yml -f docker-compose.yml logs -f --tail=100 %SVC%
    )
    goto :eof
)

if "%ACTION%"=="infra" (
    echo Starting infrastructure only (SQL Server, RabbitMQ, Azurite^)...
    docker compose -f docker-compose.shared.yml up -d
    goto :eof
)

if "%ACTION%"=="build" (
    echo Building all service images...
    docker compose -f docker-compose.shared.yml -f docker-compose.yml build
    goto :eof
)

echo Usage: deploy-compose.cmd [up^|down^|down-v^|ps^|logs^|infra^|build] [service-name]
echo   up      - Start all services (default)
echo   down    - Stop all services
echo   down-v  - Stop all services and remove volumes
echo   ps      - Show service status
echo   logs    - Show logs (optionally for a specific service)
echo   infra   - Start infrastructure only
echo   build   - Build all Docker images
