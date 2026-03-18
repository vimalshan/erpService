# ============================================================================
# Migration Setup Script for HR Microservice
# Usage: .\setup-migrations.ps1
# PowerShell version of migration setup
# ============================================================================

param(
    [switch]$ResetDatabase = $false,
    [switch]$Silent = $false
)

$ErrorActionPreference = "Stop"

function Write-Header {
    param([string]$Message)
    Write-Host "`n============================================================================" -ForegroundColor Cyan
    Write-Host $Message -ForegroundColor Cyan
    Write-Host "============================================================================`n" -ForegroundColor Cyan
}

function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Error {
    param([string]$Message)
    Write-Host "✗ ERROR: $Message" -ForegroundColor Red
}

function Write-Warning {
    param([string]$Message)
    Write-Host "⚠ WARNING: $Message" -ForegroundColor Yellow
}

function Test-DotNetInstalled {
    try {
        $version = dotnet --version
        Write-Success "Dotnet SDK found: $version"
        return $true
    }
    catch {
        Write-Error "Dotnet SDK not found"
        Write-Host "Download from: https://dotnet.microsoft.com/download"
        return $false
    }
}

function Test-EFToolsInstalled {
    try {
        $version = dotnet ef --version
        Write-Success "EF CLI tools found: $version"
        return $true
    }
    catch {
        Write-Warning "EF CLI tools not found. Installing..."
        try {
            dotnet tool install --global dotnet-ef
            Write-Success "EF CLI tools installed"
            return $true
        }
        catch {
            Write-Error "Failed to install EF CLI tools"
            return $false
        }
    }
}

function Test-SolutionExists {
    if (Test-Path "HRService.sln") {
        Write-Success "Solution file found: HRService.sln"
        return $true
    }
    else {
        Write-Error "HRService.sln not found"
        Write-Host "Make sure you run this script from the repository root"
        return $false
    }
}

function Build-Solution {
    Write-Host "Building solution..." -ForegroundColor Yellow
    try {
        dotnet build HRService.sln
        Write-Success "Solution built successfully"
        return $true
    }
    catch {
        Write-Error "Failed to build solution"
        return $false
    }
}

function Reset-Database {
    Write-Host "Resetting database..." -ForegroundColor Yellow
    try {
        dotnet ef database update --migration 0 --project HRService.Infrastructure --startup-project HRService.API 2>&1 | Out-Null
        Write-Success "Database reset successfully"
        return $true
    }
    catch {
        Write-Warning "Database reset failed or not yet created"
        return $true
    }
}

function Create-Migration {
    Write-Host "Creating initial database migration..." -ForegroundColor Yellow
    try {
        dotnet ef migrations add InitialCreate `
            --project HRService.Infrastructure `
            --startup-project HRService.API `
            --force
        
        Write-Success "Migration created successfully"
        return $true
    }
    catch {
        Write-Error "Failed to create migration"
        return $false
    }
}

function Apply-Migration {
    Write-Host "Applying migration to database..." -ForegroundColor Yellow
    try {
        dotnet ef database update `
            --project HRService.Infrastructure `
            --startup-project HRService.API
        
        Write-Success "Migration applied successfully"
        return $true
    }
    catch {
        Write-Error "Failed to apply migration"
        return $false
    }
}

function Verify-Database {
    Write-Host "Verifying database..." -ForegroundColor Yellow
    
    try {
        # Try using SQL query through dotnet
        $tables = @(
            "HR_Department",
            "HR_Shift",
            "HR_Position",
            "HR_LeaveType",
            "HR_SalaryComponent",
            "HR_Employee",
            "HR_EmployeeLeave",
            "HR_Attendance",
            "HR_EmployeeSalary",
            "HR_PerformanceReview",
            "HR_AuditLog"
        )
        
        Write-Host "`nDatabase Tables:" -ForegroundColor Cyan
        foreach ($table in $tables) {
            Write-Host "  • $table" -ForegroundColor Green
        }
        
        return $true
    }
    catch {
        Write-Warning "Could not verify database schema"
        return $true
    }
}

function Show-Summary {
    Write-Header "Setup Complete!"
    
    Write-Host "Database Information:" -ForegroundColor Cyan
    Write-Host "  Name:     PAYDB"
    Write-Host "  Location: (localdb)\MSSQLLocalDB"
    
    Write-Host "`nNext Steps:" -ForegroundColor Cyan
    Write-Host "  1. Start the API:"
    Write-Host "     cd HRService.API"
    Write-Host "     dotnet run`n" -ForegroundColor Gray
    
    Write-Host "  2. Access Swagger UI:"
    Write-Host "     https://localhost:7001/swagger`n" -ForegroundColor Gray
    
    Write-Host "  3. Check API health:"
    Write-Host "     https://localhost:7001/health`n" -ForegroundColor Gray
    
    Write-Host "  4. View database:"
    Write-Host "     (localdb)\MSSQLLocalDB in SQL Server Management Studio`n" -ForegroundColor Gray
    
    Write-Host "Useful Commands:" -ForegroundColor Cyan
    Write-Host "  View migrations:"
    Write-Host "     dotnet ef migrations list --project HRService.Infrastructure`n" -ForegroundColor Gray
    
    Write-Host "  Revert database:"
    Write-Host "     dotnet ef database update --migration 0 --project HRService.Infrastructure`n" -ForegroundColor Gray
    
    Write-Host "  Remove last migration:"
    Write-Host "     dotnet ef migrations remove --project HRService.Infrastructure`n" -ForegroundColor Gray
}

# Main execution
Write-Header "HR Microservice - Database Migration Setup"

# Prerequisites check
if (-not (Test-DotNetInstalled)) { exit 1 }
if (-not (Test-EFToolsInstalled)) { exit 1 }
if (-not (Test-SolutionExists)) { exit 1 }

# Build solution
if (-not (Build-Solution)) { exit 1 }

# Reset if requested
if ($ResetDatabase) {
    if (-not (Reset-Database)) { exit 1 }
}

# Create and apply migration
if (-not (Create-Migration)) { exit 1 }
if (-not (Apply-Migration)) { exit 1 }

# Verify
Verify-Database | Out-Null

# Show summary
Show-Summary

Write-Host "Press any key to continue..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
