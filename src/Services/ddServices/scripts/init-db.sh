#!/bin/bash
# ──────────────────────────────────────────────────────────────────────────────
# init-db.sh — Initialize all databases for DD ERP Microservices
# This script runs inside the SQL Server container after it starts
# ──────────────────────────────────────────────────────────────────────────────

set -e

SA_PASSWORD="${MSSQL_SA_PASSWORD:-YourStrong@Passw0rd}"
SQLCMD="/opt/mssql-tools18/bin/sqlcmd"

echo "============================================"
echo " DD ERP — Database Initialization"
echo "============================================"

# Wait for SQL Server to be ready
echo "[*] Waiting for SQL Server to start..."
for i in {1..60}; do
    if $SQLCMD -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT 1" &>/dev/null; then
        echo "[✓] SQL Server is ready!"
        break
    fi
    echo "    Attempt $i/60 — waiting..."
    sleep 2
done

# ─── Create all databases ───────────────────────────────────────────────────
echo ""
echo "[*] Creating databases..."

$SQLCMD -S localhost -U sa -P "$SA_PASSWORD" -C -Q "
-- Shared database (used by multiple services)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'DDDB')
    CREATE DATABASE [DDDB];
GO

-- Appraisal Service
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'AppraisalDb')
    CREATE DATABASE [AppraisalDb];
GO

-- Authorization Service
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'AuthorizationServiceDb')
    CREATE DATABASE [AuthorizationServiceDb];
GO

-- Compensation Service
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CompensationDb')
    CREATE DATABASE [CompensationDb];
GO

-- Employee Service
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EmployeeServiceDB')
    CREATE DATABASE [EmployeeServiceDB];
GO

-- Learning Service (Todos)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TodosDB')
    CREATE DATABASE [TodosDB];
GO

-- Recruitment Service
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'RecruitmentDb')
    CREATE DATABASE [RecruitmentDb];
GO

-- Reporting Service
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ReportingServiceDb')
    CREATE DATABASE [ReportingServiceDb];
GO

-- Transaction Service
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TransactionServiceDb')
    CREATE DATABASE [TransactionServiceDb];
GO
"

echo "[✓] All databases created successfully!"

# ─── Run shared DDDB schema if SQL files are mounted ────────────────────────
if [ -f "/sql/DDDB.sql" ]; then
    echo "[*] Running DDDB.sql schema..."
    $SQLCMD -S localhost -U sa -P "$SA_PASSWORD" -C -d DDDB -i /sql/DDDB.sql
    echo "[✓] DDDB schema applied."
fi

if [ -f "/sql/DDDB-procedures.sql" ]; then
    echo "[*] Running DDDB-procedures.sql..."
    $SQLCMD -S localhost -U sa -P "$SA_PASSWORD" -C -d DDDB -i /sql/DDDB-procedures.sql
    echo "[✓] DDDB procedures applied."
fi

# ─── Run service-specific SQL files if mounted ──────────────────────────────
declare -A SERVICE_SQL_MAP
SERVICE_SQL_MAP=(
    ["AppraisalDb"]="/sql/appraisal/Appraisal-DDDB.sql"
    ["AuthorizationServiceDb"]="/sql/authorization/Authorization-DDDB.sql"
    ["CompensationDb"]="/sql/compensation/Compensation-DDDB.sql"
    ["DDDB_competency"]="/sql/competency/Competency-DDDB.sql"
    ["DDDB_demandmgmt"]="/sql/demandmanagement/DemandManagement-DDDB.sql"
    ["DDDB_document"]="/sql/document/Document-DDDB.sql"
    ["EmployeeServiceDB"]="/sql/employee/Employee-DDDB.sql"
    ["DDDB_feedback"]="/sql/feedback/Feedback-DDDB.sql"
    ["TodosDB"]="/sql/learning/Learning-DDDB.sql"
    ["DDDB_objective"]="/sql/objective/Objective-DDDB.sql"
    ["DDDB_other"]="/sql/other/Other-DDDB.sql"
    ["DDDB_promotion"]="/sql/promotion/Promotion-DDDB.sql"
    ["RecruitmentDb"]="/sql/recruitment/Recruitment-DDDB.sql"
    ["ReportingServiceDb"]="/sql/reporting/Reporting-DDDB.sql"
    ["TransactionServiceDb"]="/sql/transaction/TransactionService-DDDB.sql"
)

for db_key in "${!SERVICE_SQL_MAP[@]}"; do
    sql_file="${SERVICE_SQL_MAP[$db_key]}"
    # Map the db_key to actual database name
    db_name="${db_key%%_*}"
    if [[ "$db_key" == DDDB_* ]]; then
        db_name="DDDB"
    fi

    if [ -f "$sql_file" ]; then
        echo "[*] Running $sql_file on $db_name..."
        $SQLCMD -S localhost -U sa -P "$SA_PASSWORD" -C -d "$db_name" -i "$sql_file" || \
            echo "[!] Warning: Failed to apply $sql_file (may already exist)"
    fi
done

# ─── Run seed data if available ─────────────────────────────────────────────
if [ -f "/sql/demandmanagement/DemandManagement-Seed.sql" ]; then
    echo "[*] Running DemandManagement seed data..."
    $SQLCMD -S localhost -U sa -P "$SA_PASSWORD" -C -d DDDB -i /sql/demandmanagement/DemandManagement-Seed.sql || true
fi

if [ -f "/sql/demandmanagement/DemandManagement-DDDB-procedures.sql" ]; then
    echo "[*] Running DemandManagement procedures..."
    $SQLCMD -S localhost -U sa -P "$SA_PASSWORD" -C -d DDDB -i /sql/demandmanagement/DemandManagement-DDDB-procedures.sql || true
fi

echo ""
echo "============================================"
echo " [✓] Database initialization complete!"
echo "============================================"
echo ""
echo " Databases created:"
$SQLCMD -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT name FROM sys.databases WHERE name NOT IN ('master','tempdb','model','msdb') ORDER BY name"
