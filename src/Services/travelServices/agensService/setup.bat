@echo off
REM Agency Service Setup Script

echo =====================================
echo Agency Service Setup Script
echo =====================================

REM Check .NET SDK
echo.
echo Checking prerequisites...
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo [ERROR] .NET SDK not found. Please install .NET 8 SDK
    exit /b 1
)
echo [OK] .NET SDK installed

REM Restore dependencies
echo.
echo Restoring NuGet packages...
dotnet restore
if errorlevel 1 (
    echo [ERROR] Failed to restore dependencies
    exit /b 1
)
echo [OK] Dependencies restored

REM Build solution
echo.
echo Building solution...
dotnet build -c Release
if errorlevel 1 (
    echo [ERROR] Build failed
    exit /b 1
)
echo [OK] Solution built successfully

REM Run migrations
echo.
echo Running database migrations...
cd src\API\AgencyService.Api
dotnet ef database update --project ..\..\Infrastructure\AgencyService.Infrastructure
if errorlevel 1 (
    echo [WARNING] Database migrations skipped (ensure database is up to date)
) else (
    echo [OK] Database migrations completed
)
cd ..\..\..

REM Success message
echo.
echo =====================================
echo Setup completed successfully!
echo =====================================

echo.
echo Next steps:
echo 1. (Optional) Start Docker containers: docker-compose up -d
echo 2. Run the API: dotnet run --project src/API/AgencyService.Api
echo 3. Access Swagger: http://localhost:5000/swagger/index.html
echo 4. Access GraphQL: http://localhost:5000/graphql

pause
