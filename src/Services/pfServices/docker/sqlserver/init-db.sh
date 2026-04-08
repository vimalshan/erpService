#!/bin/bash
# ============================================
# PFDB Database Initialization Script
# Waits for SQL Server, creates DB, runs schema
# ============================================

set -e

SQLSERVER_HOST="sqlserver"
SQLSERVER_PORT="1433"
SA_USER="sa"
MAX_RETRIES=30
RETRY_INTERVAL=5

echo "=========================================="
echo "PF Database Initialization"
echo "=========================================="

# Wait for SQL Server to be ready
echo "Waiting for SQL Server at ${SQLSERVER_HOST}:${SQLSERVER_PORT}..."
retries=0
until /opt/mssql-tools18/bin/sqlcmd -S "${SQLSERVER_HOST},${SQLSERVER_PORT}" -U "${SA_USER}" -P "${SA_PASSWORD}" -C -Q "SELECT 1" -b -o /dev/null 2>/dev/null; do
    retries=$((retries + 1))
    if [ $retries -ge $MAX_RETRIES ]; then
        echo "ERROR: SQL Server not reachable after ${MAX_RETRIES} retries. Exiting."
        exit 1
    fi
    echo "  Retry ${retries}/${MAX_RETRIES}..."
    sleep $RETRY_INTERVAL
done
echo "SQL Server is ready."

# Create the PFDB database if it does not exist
echo "Creating PFDB database..."
/opt/mssql-tools18/bin/sqlcmd -S "${SQLSERVER_HOST},${SQLSERVER_PORT}" -U "${SA_USER}" -P "${SA_PASSWORD}" -C -Q "
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'PFDB')
BEGIN
    CREATE DATABASE PFDB;
    PRINT 'Database PFDB created.';
END
ELSE
    PRINT 'Database PFDB already exists.';
"

# Run table creation scripts
echo "Running PFDB schema (tables)..."
/opt/mssql-tools18/bin/sqlcmd -S "${SQLSERVER_HOST},${SQLSERVER_PORT}" -U "${SA_USER}" -P "${SA_PASSWORD}" -C -d PFDB -i /scripts/PFDB.sql -b || {
    echo "WARNING: Table creation had errors (tables may already exist). Continuing..."
}

# Run stored procedures and functions
echo "Running PFDB procedures and functions..."
/opt/mssql-tools18/bin/sqlcmd -S "${SQLSERVER_HOST},${SQLSERVER_PORT}" -U "${SA_USER}" -P "${SA_PASSWORD}" -C -d PFDB -i /scripts/PFDB-procedures.sql -b || {
    echo "WARNING: Procedure creation had errors. Continuing..."
}

# Run module-specific SQL scripts if present
for module_script in /scripts/modules/*.sql; do
    if [ -f "$module_script" ]; then
        echo "Running module script: $(basename $module_script)..."
        /opt/mssql-tools18/bin/sqlcmd -S "${SQLSERVER_HOST},${SQLSERVER_PORT}" -U "${SA_USER}" -P "${SA_PASSWORD}" -C -d PFDB -i "$module_script" -b || {
            echo "WARNING: $(basename $module_script) had errors. Continuing..."
        }
    fi
done

echo "=========================================="
echo "Database initialization complete."
echo "=========================================="
