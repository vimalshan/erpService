#!/bin/bash
# ==============================================================================
# ERP Travel Services - Database Initialization Script
# This script runs inside the SQL Server container to set up all databases
# ==============================================================================

set -e

SQLCMD="/opt/mssql-tools18/bin/sqlcmd"
SERVER="localhost"
USER="sa"
PASSWORD="${SA_PASSWORD:-Erp@Travel2024!}"
COMMON_ARGS="-S $SERVER -U $USER -P $PASSWORD -C -b"

echo "============================================"
echo "ERP Travel Services - Database Initialization"
echo "============================================"

# Wait for SQL Server to be ready
echo "[1/6] Waiting for SQL Server to be ready..."
for i in {1..60}; do
    if $SQLCMD $COMMON_ARGS -Q "SELECT 1" > /dev/null 2>&1; then
        echo "  SQL Server is ready!"
        break
    fi
    echo "  Attempt $i/60 - SQL Server not ready yet..."
    sleep 2
done

# Create all databases
echo ""
echo "[2/6] Creating databases..."
$SQLCMD $COMMON_ARGS -Q "
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TRAVELDB')
    CREATE DATABASE TRAVELDB;
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'FinanceServiceDb')
    CREATE DATABASE FinanceServiceDb;
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'MasterDataDB')
    CREATE DATABASE MasterDataDB;
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'AgencyServiceDb')
    CREATE DATABASE AgencyServiceDb;
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'AdminServiceDb')
    CREATE DATABASE AdminServiceDb;
GO
PRINT 'All databases created successfully';
"

# Run TRAVELDB tables (shared by TravelRequest, TravelTransaction, Booking, Expense, Insurance)
echo ""
echo "[3/6] Creating TRAVELDB tables..."
if [ -f /docker-entrypoint-initdb.d/TRAVELDB.sql ]; then
    $SQLCMD $COMMON_ARGS -d TRAVELDB -i /docker-entrypoint-initdb.d/TRAVELDB.sql
    echo "  TRAVELDB tables created."
else
    echo "  WARNING: TRAVELDB.sql not found, skipping..."
fi

# Run TRAVELDB stored procedures
echo ""
echo "[4/6] Creating TRAVELDB stored procedures..."
if [ -f /docker-entrypoint-initdb.d/TRAVELDB-procedures.sql ]; then
    $SQLCMD $COMMON_ARGS -d TRAVELDB -i /docker-entrypoint-initdb.d/TRAVELDB-procedures.sql
    echo "  TRAVELDB procedures created."
else
    echo "  WARNING: TRAVELDB-procedures.sql not found, skipping..."
fi

# Run service-specific SQL scripts
echo ""
echo "[5/6] Creating service-specific tables and procedures..."

# Finance Service
for f in /docker-entrypoint-initdb.d/05-Finance/*.sql; do
    if [ -f "$f" ]; then
        echo "  Running $(basename $f) on FinanceServiceDb..."
        $SQLCMD $COMMON_ARGS -d FinanceServiceDb -i "$f"
    fi
done

# MasterData Service
for f in /docker-entrypoint-initdb.d/07-MasterData/*.sql; do
    if [ -f "$f" ]; then
        echo "  Running $(basename $f) on MasterDataDB..."
        $SQLCMD $COMMON_ARGS -d MasterDataDB -i "$f"
    fi
done

# Agency Service
for f in /docker-entrypoint-initdb.d/03-Agency/*.sql; do
    if [ -f "$f" ]; then
        echo "  Running $(basename $f) on AgencyServiceDb..."
        $SQLCMD $COMMON_ARGS -d AgencyServiceDb -i "$f"
    fi
done

# Admin Service
for f in /docker-entrypoint-initdb.d/06-Admin/*.sql; do
    if [ -f "$f" ]; then
        echo "  Running $(basename $f) on AdminServiceDb..."
        $SQLCMD $COMMON_ARGS -d AdminServiceDb -i "$f"
    fi
done

# TRAVELDB sub-service scripts (Booking, Expense, Insurance, TravelRequest, TravelTransaction)
for dir in 01-TravelRequest 02-Booking 04-Expense 08-Insurance; do
    for f in /docker-entrypoint-initdb.d/$dir/*.sql; do
        if [ -f "$f" ]; then
            echo "  Running $(basename $f) on TRAVELDB..."
            $SQLCMD $COMMON_ARGS -d TRAVELDB -i "$f"
        fi
    done
done

# Create application login
echo ""
echo "[6/6] Creating application login..."
$SQLCMD $COMMON_ARGS -Q "
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'erp_app')
BEGIN
    CREATE LOGIN erp_app WITH PASSWORD = '${SA_PASSWORD:-Erp@Travel2024!}';
END

USE TRAVELDB;
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'erp_app')
BEGIN
    CREATE USER erp_app FOR LOGIN erp_app;
    ALTER ROLE db_datareader ADD MEMBER erp_app;
    ALTER ROLE db_datawriter ADD MEMBER erp_app;
    GRANT EXECUTE TO erp_app;
END

USE FinanceServiceDb;
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'erp_app')
BEGIN
    CREATE USER erp_app FOR LOGIN erp_app;
    ALTER ROLE db_datareader ADD MEMBER erp_app;
    ALTER ROLE db_datawriter ADD MEMBER erp_app;
    GRANT EXECUTE TO erp_app;
END

USE MasterDataDB;
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'erp_app')
BEGIN
    CREATE USER erp_app FOR LOGIN erp_app;
    ALTER ROLE db_datareader ADD MEMBER erp_app;
    ALTER ROLE db_datawriter ADD MEMBER erp_app;
    GRANT EXECUTE TO erp_app;
END

USE AgencyServiceDb;
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'erp_app')
BEGIN
    CREATE USER erp_app FOR LOGIN erp_app;
    ALTER ROLE db_datareader ADD MEMBER erp_app;
    ALTER ROLE db_datawriter ADD MEMBER erp_app;
    GRANT EXECUTE TO erp_app;
END

USE AdminServiceDb;
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'erp_app')
BEGIN
    CREATE USER erp_app FOR LOGIN erp_app;
    ALTER ROLE db_datareader ADD MEMBER erp_app;
    ALTER ROLE db_datawriter ADD MEMBER erp_app;
    GRANT EXECUTE TO erp_app;
END

PRINT 'Application login created successfully';
"

echo ""
echo "============================================"
echo "Database initialization completed!"
echo "============================================"
echo ""
echo "Databases created:"
$SQLCMD $COMMON_ARGS -Q "SELECT name FROM sys.databases WHERE name NOT IN ('master','tempdb','model','msdb') ORDER BY name" -h -1
