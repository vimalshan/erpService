@echo off
REM ============================================================
REM ERP Microservices - Docker Build Script
REM Builds all service Docker images
REM ============================================================

set REGISTRY=%1
if "%REGISTRY%"=="" set REGISTRY=erp
set TAG=%2
if "%TAG%"=="" set TAG=latest

echo ============================================================
echo Building ERP Microservice Docker Images
echo Registry: %REGISTRY%  Tag: %TAG%
echo ============================================================

echo.
echo [1/10] Building API Gateway...
docker build -t %REGISTRY%/api-gateway:%TAG% ./ApiGateway
if %ERRORLEVEL% neq 0 (echo FAILED: api-gateway & exit /b 1)

echo.
echo [2/10] Building Access Service...
docker build -t %REGISTRY%/access-service:%TAG% ./accessServices
if %ERRORLEVEL% neq 0 (echo FAILED: access-service & exit /b 1)

echo.
echo [3/10] Building Attendance Service...
docker build -t %REGISTRY%/attendance-service:%TAG% ./attendanceServices
if %ERRORLEVEL% neq 0 (echo FAILED: attendance-service & exit /b 1)

echo.
echo [4/10] Building Bus Service...
docker build -t %REGISTRY%/bus-service:%TAG% ./busServices
if %ERRORLEVEL% neq 0 (echo FAILED: bus-service & exit /b 1)

echo.
echo [5/10] Building Calendar Service...
docker build -t %REGISTRY%/calendar-service:%TAG% ./calendarServices
if %ERRORLEVEL% neq 0 (echo FAILED: calendar-service & exit /b 1)

echo.
echo [6/10] Building Employee Service...
docker build -t %REGISTRY%/employee-service:%TAG% ./employeeServices
if %ERRORLEVEL% neq 0 (echo FAILED: employee-service & exit /b 1)

echo.
echo [7/10] Building Group Incentive Service...
docker build -t %REGISTRY%/groupincentive-service:%TAG% ./groupincentiveServices
if %ERRORLEVEL% neq 0 (echo FAILED: groupincentive-service & exit /b 1)

echo.
echo [8/10] Building Leave Service...
docker build -t %REGISTRY%/leave-service:%TAG% ./leaveServices
if %ERRORLEVEL% neq 0 (echo FAILED: leave-service & exit /b 1)

echo.
echo [9/10] Building Reference Service...
docker build -t %REGISTRY%/reference-service:%TAG% ./referenceServies
if %ERRORLEVEL% neq 0 (echo FAILED: reference-service & exit /b 1)

echo.
echo [10/10] Building Visitor Service...
docker build -t %REGISTRY%/visitor-service:%TAG% ./visitorServices
if %ERRORLEVEL% neq 0 (echo FAILED: visitor-service & exit /b 1)

echo.
echo ============================================================
echo All 10 service images built successfully!
echo ============================================================
echo.
echo To deploy with Docker Compose:
echo   deploy-compose.cmd up
echo.
echo To run individually:
echo   docker run -d -p 5020:80 %REGISTRY%/api-gateway:%TAG%
echo ============================================================
