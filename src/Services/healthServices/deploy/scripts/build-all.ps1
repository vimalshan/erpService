# ================================================
# Health ERP - Build All Services (PowerShell)
# ================================================
$ErrorActionPreference = "Continue"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RootDir = Split-Path -Parent (Split-Path -Parent $ScriptDir)

$Failed = 0
$Total = 0

function Build-Service {
    param($Name, $RelPath, $Csproj)
    $script:Total++
    $fullPath = Join-Path $RootDir "$RelPath/$Csproj"

    Write-Host "[INFO] Building $Name..." -ForegroundColor Green
    $output = dotnet build $fullPath -c Release --nologo -v q 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  OK $Name" -ForegroundColor Green
    } else {
        Write-Host "  FAIL $Name" -ForegroundColor Red
        $output | Write-Host
        $script:Failed++
    }
}

Write-Host "=========================================="
Write-Host "  Health ERP - Building All Services"
Write-Host "=========================================="
Write-Host ""

Build-Service "Accident Management"  "accidentmanagementServices/src/AccidentManagementService" "AccidentManagementService.csproj"
Build-Service "Checkup Management"   "healthcheckupServices/src/CheckupManagementService" "CheckupManagementService.csproj"
Build-Service "Insurance Management" "insurancemanagementServices/src/InsuranceManagement.API" "InsuranceManagement.API.csproj"
Build-Service "Masters"              "masterServices/src/Masters.API" "Masters.API.csproj"
Build-Service "Medical Visit"        "medicalvisitServices/src/MedicalVisit.API" "MedicalVisit.API.csproj"
Build-Service "Medicine Management"  "medicinemanagementServices/src/MedicineManagement.API" "MedicineManagement.API.csproj"
Build-Service "Health Transaction"   "healthTransactionServices/src/HealthTransaction.API" "HealthTransaction.API.csproj"
Build-Service "API Gateway"          "apiGateway/src/HealthGateway" "HealthGateway.csproj"

Write-Host ""
Write-Host "=========================================="
Write-Host "  Build Summary: $($Total - $Failed)/$Total succeeded"
Write-Host "=========================================="

if ($Failed -gt 0) {
    Write-Host "[ERROR] $Failed service(s) failed to build." -ForegroundColor Red
    exit 1
} else {
    Write-Host "[INFO] All services built successfully!" -ForegroundColor Green
}
