@echo off
REM ============================================================================
REM Migration Setup Script for HR Microservice
REM Usage: Run this script from the repository root directory
REM Purpose: Automates database migration and seeding
REM ============================================================================

setlocal enabledelayedexpansion

cls
echo.
echo ============================================================================
echo HR Microservice - Database Migration Setup
echo ============================================================================
echo.

REM Check if .NET SDK is installed
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK not found. Please install .NET 8.0 or later.
    echo Download from: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo ✓ .NET SDK found: 
dotnet --version
echo.

REM Check if EF CLI is installed
dotnet ef --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Installing Entity Framework CLI tools...
    dotnet tool install --global dotnet-ef
    if %errorlevel% neq 0 (
        echo ERROR: Failed to install EF CLI
        pause
        exit /b 1
    )
)

echo ✓ EF CLI tools found:
dotnet ef --version
echo.

REM Verify project structure
if not exist "HRService.sln" (
    echo ERROR: HRService.sln not found
    echo Make sure you run this script from the repository root
    pause
    exit /b 1
)

echo ✓ Solution file found: HRService.sln
echo.

REM Build solution
echo Building solution...
dotnet build HRService.sln
if %errorlevel% neq 0 (
    echo ERROR: Solution build failed
    pause
    exit /b 1
)

echo ✓ Solution built successfully
echo.

REM Create migration
echo Creating initial database migration...
dotnet ef migrations add InitialCreate --project HRService.Infrastructure --startup-project HRService.API

if %errorlevel% neq 0 (
    echo ERROR: Failed to create migration
    pause
    exit /b 1
)

echo ✓ Migration created successfully
echo.

REM Apply migration
echo Applying migration to database...
dotnet ef database update --project HRService.Infrastructure --startup-project HRService.API

if %errorlevel% neq 0 (
    echo ERROR: Failed to apply migration
    pause
    exit /b 1
)

echo ✓ Migration applied successfully
echo.

REM Verification
echo Verifying database...
sqlcmd -S "(localdb)\MSSQLLocalDB" -d PAYDB -Q "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='dbo' ORDER BY TABLE_NAME" 2>nul

if %errorlevel% equ 0 (
    echo ✓ Database verification successful
) else (
    echo WARNING: Could not verify database (sqlcmd not found)
)

echo.
echo ============================================================================
echo Setup Complete!
echo ============================================================================
echo.
echo Database: PAYDB
echo Location: (localdb)\MSSQLLocalDB
echo.
echo Next steps:
echo  1. Start the API: dotnet run --project HRService.API
echo  2. Access Swagger: https://localhost:7001/swagger
echo  3. Check health: https://localhost:7001/health
echo.
echo To revert migration:
echo  dotnet ef database update --migration 0 --project HRService.Infrastructure
echo.
pause
