@echo off
REM Build script for ReferenceService

echo Building ReferenceService...

REM Restore NuGet packages
echo Restoring NuGet packages...
dotnet restore src\ReferenceService.slnx

REM Build solution
echo Building solution...
dotnet build src\ReferenceService.slnx --configuration Release --no-restore

if %ERRORLEVEL% EQU 0 (
    echo Build completed successfully!
    exit /b 0
) else (
    echo Build failed!
    exit /b 1
)
