@echo off
REM Run migration script for ReferenceService

echo Applying database migrations...

REM Change to API project directory
cd src\API\ReferenceService.API

REM Apply migrations
dotnet ef database update --project ..\..\..\Infrastructure\ReferenceService.Infrastructure\ReferenceService.Infrastructure.csproj --context ReferenceDbContext --startup-project .

if %ERRORLEVEL% EQU 0 (
    echo Database migrations applied successfully!
    
    REM Run seed script
    echo Applying seed data...
    sqlcmd -S "(localdb)\MSSQLLocalDB" -i ..\..\..\..\SeedData.sql
    
    echo Setup completed successfully!
    exit /b 0
) else (
    echo Migration failed!
    exit /b 1
)
