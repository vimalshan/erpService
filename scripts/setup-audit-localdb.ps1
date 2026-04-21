param(
    [string]$LocalDbInstance = "MSSQLLocalDB",
    [switch]$ApplySqlAssets,
    [switch]$SkipMigrations
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$auditRoot = Join-Path $repoRoot "src/Services/auditServices"

$services = @(
    @{
        Name = "actionapiServices"
        Database = "ERPActionDB"
        Project = "ActionService.csproj"
        DbContext = "ActionDbContext"
        SqlRoots = @("Database/Tables", "Database/Insert-Scripts", "Database/Stored-Procedures")
    },
    @{
        Name = "auditapiServices"
        Database = "ERPAuditDB"
        Project = "AuditService.csproj"
        DbContext = "AuditDomainDbContext"
        SqlRoots = @("tables", "insert-scripts", "Stored-procedure")
    },
    @{
        Name = "certificateapiServices"
        Database = "ERPCertificateDB"
        Project = "CertificateService.csproj"
        DbContext = "CertificateDomainDbContext"
        SqlRoots = @("tables", "insert-scripts", "Stored-procedure")
    },
    @{
        Name = "contractapiServices"
        Database = "ERPContractDB"
        Project = "ContractService.csproj"
        DbContext = "ContractDomainDbContext"
        SqlRoots = @()
    },
    @{
        Name = "financeapiServices"
        Database = "ERPFinanceDB"
        Project = "FinanceService.csproj"
        DbContext = "FinanceDomainDbContext"
        SqlRoots = @()
    },
    @{
        Name = "findingsapiServices"
        Database = "ERPFindingsDB"
        Project = "FindingsAPI.Gateway.csproj"
        DbContext = "FindingsDomainDbContext"
        SqlRoots = @("tables", "insert-scripts", "Stored-procedure")
    },
    @{
        Name = "notificationapiServices"
        Database = "ERPNotificationDB"
        Project = "NotificationService.csproj"
        DbContext = "NotificationDomainDbContext"
        SqlRoots = @("tables", "insert-scripts", "Stored-procedure")
    },
    @{
        Name = "scheduleapiServices"
        Database = "ERPScheduleDB"
        Project = "ScheduleService.csproj"
        DbContext = "ScheduleDomainDbContext"
        SqlRoots = @("tables", "insert-scripts", "Stored-procedure")
    },
    @{
        Name = "settingsapiServices"
        Database = "ERPSettingsDB"
        Project = "SettingsService.csproj"
        DbContext = "SettingsDomainDbContext"
        SqlRoots = @("tables", "insert-scripts", "Stored-procedure")
    }
)

function Invoke-Checked {
    param(
        [string]$FilePath,
        [string[]]$ArgumentList,
        [string]$WorkingDirectory
    )

    Push-Location $WorkingDirectory
    try {
        & $FilePath @ArgumentList
        if ($LASTEXITCODE -ne 0) {
            throw "Command failed: $FilePath $($ArgumentList -join ' ')"
        }
    }
    finally {
        Pop-Location
    }
}

function Get-SqlFiles {
    param(
        [string]$RootPath
    )

    if (-not (Test-Path $RootPath)) {
        return @()
    }

    return @(Get-ChildItem -Path $RootPath -Recurse -Filter *.sql -File |
        Sort-Object FullName |
        Select-Object -ExpandProperty FullName)
}

function New-DatabaseIfMissing {
    param(
        [string]$SqlcmdPath,
        [string]$ServerName,
        [string]$DatabaseName
    )

    $query = "IF DB_ID(N'$DatabaseName') IS NULL CREATE DATABASE [$DatabaseName];"
    Invoke-Checked -FilePath $SqlcmdPath -ArgumentList @("-S", $ServerName, "-d", "master", "-Q", $query, "-b") -WorkingDirectory $repoRoot
}

Write-Host "Ensuring LocalDB instance '$LocalDbInstance' is running..."
& sqllocaldb start $LocalDbInstance | Out-Null
if ($LASTEXITCODE -ne 0) {
    throw "Failed to start LocalDB instance '$LocalDbInstance'."
}

$sqlcmd = Get-Command sqlcmd -ErrorAction SilentlyContinue
$serverName = "(localdb)\$LocalDbInstance"

if ($ApplySqlAssets -and -not $sqlcmd) {
    throw "sqlcmd was not found in PATH. Install sqlcmd or rerun without -ApplySqlAssets."
}

foreach ($service in $services) {
    $servicePath = Join-Path $auditRoot $service.Name
    $projectPath = Join-Path $servicePath $service.Project

    Write-Host ""
    Write-Host "=== $($service.Name) -> $($service.Database) ==="

    if ($sqlcmd) {
        New-DatabaseIfMissing -SqlcmdPath $sqlcmd.Source -ServerName $serverName -DatabaseName $service.Database
    }

    if (-not $SkipMigrations) {
        Write-Host "Applying EF migrations for $($service.DbContext)..."
        Invoke-Checked -FilePath "dotnet" -ArgumentList @(
            "ef", "database", "update",
            "--project", $projectPath,
            "--startup-project", $projectPath,
            "--context", $service.DbContext
        ) -WorkingDirectory $repoRoot
    }

    if ($ApplySqlAssets) {
        $sqlFiles = @()
        foreach ($relativeRoot in $service.SqlRoots) {
            $sqlFiles += Get-SqlFiles -RootPath (Join-Path $servicePath $relativeRoot)
        }

        if ($sqlFiles.Count -gt 0) {
            Write-Host "Applying $($sqlFiles.Count) SQL asset files..."
            foreach ($sqlFile in $sqlFiles) {
                Invoke-Checked -FilePath $sqlcmd.Source -ArgumentList @(
                    "-S", $serverName,
                    "-d", $service.Database,
                    "-i", $sqlFile,
                    "-b"
                ) -WorkingDirectory $repoRoot
            }
        }
        else {
            Write-Host "No SQL asset files found."
        }
    }
}

Write-Host ""
Write-Host "Audit LocalDB setup completed. Restart the audit services before re-running GraphQL checks."