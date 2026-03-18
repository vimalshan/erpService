# Approval Service - Build and Test Script

## Prerequisites Setup
$prerequisites = @{
    "dotnet --version" = ".NET 8 SDK"
    "docker --version" = "Docker"
    "docker-compose --version" = "Docker Compose"
}

Write-Host "Checking prerequisites..." -ForegroundColor Green
foreach ($command in $prerequisites.Keys) {
    try {
        Invoke-Expression $command | Out-Null
        Write-Host "✓ $($prerequisites[$command]) - Installed" -ForegroundColor Green
    }
    catch {
        Write-Host "✗ $($prerequisites[$command]) - NOT installed" -ForegroundColor Red
        exit 1
    }
}

## Start Dependencies
Write-Host "`nStarting dependencies..." -ForegroundColor Green
docker-compose up -d

Write-Host "Waiting for services to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

## Build Solution
Write-Host "`nBuilding solution..." -ForegroundColor Green
dotnet build ApprovalService.sln

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "Build successful!" -ForegroundColor Green

## Run Tests
Write-Host "`nRunning tests..." -ForegroundColor Green
$testProjects = Get-ChildItem -Path "src" -Filter "*.Tests.csproj" -Recurse

if ($testProjects.Count -eq 0) {
    Write-Host "No test projects found" -ForegroundColor Yellow
}
else {
    foreach ($project in $testProjects) {
        dotnet test $project.FullName --logger "console;verbosity=minimal"
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Tests failed for $($project.Name)!" -ForegroundColor Red
            exit 1
        }
    }
}

## Run API
Write-Host "`nStarting API..." -ForegroundColor Green
$apiProjectPath = "src/ApprovalService.API/ApprovalService.API.csproj"

if (Test-Path $apiProjectPath) {
    dotnet run --project $apiProjectPath
}
else {
    Write-Host "API project not found!" -ForegroundColor Red
    exit 1
}
