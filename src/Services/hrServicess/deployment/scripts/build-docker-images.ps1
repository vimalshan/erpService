param(
    [string]$Registry = "docker.io",
    [string]$ImageTag = "latest",
    [string]$Service = $null,
    [switch]$Push = $false,
    [switch]$NoCache = $false
)

$ErrorActionPreference = "Stop"
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent (Split-Path -Parent $scriptPath)
$dockerfilesPath = Join-Path $scriptPath "..\Dockerfiles"

Write-Host "HR Microservices Docker Build Script" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host "Project Root: $projectRoot" -ForegroundColor Cyan
Write-Host "Registry: $Registry" -ForegroundColor Cyan
Write-Host "Image Tag: $ImageTag`n" -ForegroundColor Cyan

$services = @(
    @{name="AlertsNotifications"; dockerfile="AlertsNotifications.Dockerfile"; context=$projectRoot},
    @{name="CompensationBenefits"; dockerfile="CompensationBenefits.Dockerfile"; context=$projectRoot},
    @{name="EmployeeManagement"; dockerfile="EmployeeManagement.Dockerfile"; context=$projectRoot},
    @{name="EmployeeRelations"; dockerfile="EmployeeRelations.Dockerfile"; context=$projectRoot},
    @{name="ExitManagement"; dockerfile="ExitManagement.Dockerfile"; context=$projectRoot},
    @{name="OrganizationStructure"; dockerfile="OrganizationStructure.Dockerfile"; context=$projectRoot},
    @{name="Recruitment"; dockerfile="Recruitment.Dockerfile"; context=$projectRoot},
    @{name="TimeAttendance"; dockerfile="TimeAttendance.Dockerfile"; context=$projectRoot},
    @{name="TrainingDevelopment"; dockerfile="TrainingDevelopment.Dockerfile"; context=$projectRoot},
    @{name="UserSecurity"; dockerfile="UserSecurity.Dockerfile"; context=$projectRoot},
    @{name="EmployeeTransactions"; dockerfile="EmployeeTransactions.Dockerfile"; context=$projectRoot},
    @{name="ApiGateway"; dockerfile="ApiGateway.Dockerfile"; context=$projectRoot}
)

if ($Service) {
    $services = $services | Where-Object { $_.name -like "*$Service*" }
    if ($services.Count -eq 0) {
        Write-Host "ERROR: Service matching '$Service' not found" -ForegroundColor Red
        exit 1
    }
}

$buildCount = 0
$failCount = 0

foreach ($svc in $services) {
    $imageName = "hr-$($svc.name.ToLower())"
    $imageFullName = "$Registry/$imageName`:$ImageTag"
    $dockerfilePath = Join-Path $dockerfilesPath $svc.dockerfile
    
    if (-not (Test-Path $dockerfilePath)) {
        Write-Host "ERROR: Dockerfile not found: $dockerfilePath" -ForegroundColor Red
        $failCount++
        continue
    }
    
    Write-Host "`nBuilding: $imageName" -ForegroundColor Cyan
    Write-Host "Dockerfile: $svc.dockerfile" -ForegroundColor Yellow
    Write-Host "Image: $imageFullName" -ForegroundColor Yellow
    
    $dockerArgs = @("build", "-t", $imageFullName, "-f", $dockerfilePath, $svc.context)
    
    if ($NoCache) {
        $dockerArgs += "--no-cache"
    }
    
    Try {
        & docker $dockerArgs
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "SUCCESS: Build complete for $imageName" -ForegroundColor Green
            $buildCount++
            
            if ($Push) {
                Write-Host "Pushing image: $imageFullName" -ForegroundColor Cyan
                & docker push $imageFullName
                
                if ($LASTEXITCODE -eq 0) {
                    Write-Host "SUCCESS: Push complete for $imageFullName" -ForegroundColor Green
                } 
                else {
                    Write-Host "ERROR: Push failed for $imageFullName" -ForegroundColor Red
                    $failCount++
                }
            }
        }
        else {
            Write-Host "ERROR: Build failed for $imageName" -ForegroundColor Red
            $failCount++
        }
    }
    Catch {
        Write-Host "ERROR: Exception during build of $imageName : $_" -ForegroundColor Red
        $failCount++
    }
}

Write-Host "`n================================" -ForegroundColor Cyan
Write-Host "Build Summary" -ForegroundColor Green
Write-Host "================================" -ForegroundColor Cyan
Write-Host "Successfully built: $buildCount" -ForegroundColor Green
Write-Host "Failed: $failCount" -ForegroundColor $(if ($failCount -gt 0) { "Red" } else { "Green" })

Write-Host "`nBuilt Docker Images:" -ForegroundColor Cyan
& docker images | grep "hr-"

if ($failCount -gt 0) {
    exit 1
}
