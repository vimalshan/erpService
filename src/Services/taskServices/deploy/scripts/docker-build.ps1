<#
.SYNOPSIS
    Build all ERP Microservice Docker images
.PARAMETER Tag
    Image tag (default: latest)
.PARAMETER Registry
    Docker registry prefix (optional)
.PARAMETER Push
    Push images to registry after building
#>
param(
    [string]$Tag = "latest",
    [string]$Registry = "",
    [switch]$Push
)

$ErrorActionPreference = "Stop"
$RootDir = (Resolve-Path "$PSScriptRoot\..\..").Path

function Get-ImageName([string]$Name) {
    if ($Registry) { return "${Registry}/${Name}:${Tag}" }
    return "${Name}:${Tag}"
}

function Build-Image([string]$Context, [string]$Dockerfile, [string]$Name) {
    $image = Get-ImageName $Name
    Write-Host "`n--- Building $Name ---" -ForegroundColor Cyan
    docker build -t $image -f "$Context\$Dockerfile" $Context
    if ($LASTEXITCODE -ne 0) { throw "Failed to build $Name" }
    Write-Host "    OK $image" -ForegroundColor Green
}

Write-Host "============================================" -ForegroundColor Yellow
Write-Host "  Building ERP Microservice Docker Images"
Write-Host "  Tag: $Tag"
if ($Registry) { Write-Host "  Registry: $Registry" }
Write-Host "============================================" -ForegroundColor Yellow

$images = @(
    @{ Context = "$RootDir\apiGateway";                   Dockerfile = "Dockerfile";           Name = "erp-api-gateway" },
    @{ Context = "$RootDir\lookupServices";               Dockerfile = "Dockerfile";           Name = "erp-lookup-api" },
    @{ Context = "$RootDir\lookupServices";               Dockerfile = "Dockerfile.functions"; Name = "erp-lookup-functions" },
    @{ Context = "$RootDir\taskServices";                 Dockerfile = "Dockerfile";           Name = "erp-task-api" },
    @{ Context = "$RootDir\taskServices";                 Dockerfile = "Dockerfile.functions"; Name = "erp-task-functions" },
    @{ Context = "$RootDir\taskTransactionalServices";    Dockerfile = "Dockerfile";           Name = "erp-transactional-api" },
    @{ Context = "$RootDir\taskTransactionalServices";    Dockerfile = "Dockerfile.functions"; Name = "erp-transactional-functions" },
    @{ Context = "$RootDir\complaintServices";            Dockerfile = "Dockerfile";           Name = "erp-complaint-api" },
    @{ Context = "$RootDir\complaintServices";            Dockerfile = "Dockerfile.functions"; Name = "erp-complaint-functions" },
    @{ Context = "$RootDir\energyServices";               Dockerfile = "Dockerfile";           Name = "erp-energy-api" },
    @{ Context = "$RootDir\energyServices";               Dockerfile = "Dockerfile.functions"; Name = "erp-energy-functions" },
    @{ Context = "$RootDir\unitServices";                 Dockerfile = "Dockerfile";           Name = "erp-unit-api" },
    @{ Context = "$RootDir\unitServices";                 Dockerfile = "Dockerfile.functions"; Name = "erp-unit-functions" }
)

foreach ($img in $images) {
    Build-Image -Context $img.Context -Dockerfile $img.Dockerfile -Name $img.Name
}

Write-Host "`n============================================" -ForegroundColor Yellow
Write-Host "  All 13 images built successfully"
Write-Host "============================================" -ForegroundColor Yellow

if ($Push -and $Registry) {
    Write-Host "`nPushing images to $Registry..." -ForegroundColor Cyan
    foreach ($img in $images) {
        $image = Get-ImageName $img.Name
        docker push $image
        if ($LASTEXITCODE -ne 0) { throw "Failed to push $image" }
        Write-Host "    OK pushed $image" -ForegroundColor Green
    }
    Write-Host "All images pushed." -ForegroundColor Green
}
