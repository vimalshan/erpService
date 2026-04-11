#!/bin/bash
# ==========================================
# TOURDB Database Initialization Script
# For Docker SQL Server container
# ==========================================

set -e

SA_PASSWORD="${SA_PASSWORD:-TourERP@Str0ngP@ss!}"
DB_NAME="TOURDB"
MAX_RETRIES=30
RETRY_INTERVAL=5

echo "============================================"
echo "  Tour ERP Database Initialization"
echo "============================================"

# Wait for SQL Server to be ready
echo "[INFO] Waiting for SQL Server to be ready..."
for i in $(seq 1 $MAX_RETRIES); do
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT 1" > /dev/null 2>&1
    if [ $? -eq 0 ]; then
        echo "[INFO] SQL Server is ready."
        break
    fi
    echo "[INFO] SQL Server not ready yet... attempt $i/$MAX_RETRIES"
    sleep $RETRY_INTERVAL
done

# Check if we connected
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT 1" > /dev/null 2>&1
if [ $? -ne 0 ]; then
    echo "[ERROR] Could not connect to SQL Server after $MAX_RETRIES attempts. Exiting."
    exit 1
fi

# Create database if it doesn't exist
echo "[INFO] Creating database $DB_NAME if not exists..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -Q "
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'$DB_NAME')
BEGIN
    CREATE DATABASE [$DB_NAME];
    PRINT 'Database $DB_NAME created.';
END
ELSE
    PRINT 'Database $DB_NAME already exists.';
"

# Run schema creation
echo "[INFO] Running schema creation (tables)..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -d "$DB_NAME" -i /docker-entrypoint-initdb.d/01-tables.sql
echo "[INFO] Tables created successfully."

# Run stored procedures
echo "[INFO] Running stored procedures..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -d "$DB_NAME" -i /docker-entrypoint-initdb.d/02-procedures.sql
echo "[INFO] Stored procedures created successfully."

# Run seed data
if [ -f /docker-entrypoint-initdb.d/03-seed-data.sql ]; then
    echo "[INFO] Running seed data..."
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -d "$DB_NAME" -i /docker-entrypoint-initdb.d/03-seed-data.sql
    echo "[INFO] Seed data inserted successfully."
fi

echo "============================================"
echo "  Database initialization complete!"
echo "============================================"
