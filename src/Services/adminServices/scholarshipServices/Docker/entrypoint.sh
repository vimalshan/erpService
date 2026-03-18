#!/bin/bash
set -e

echo "======================================================"
echo " Scholarship Service - SQL Server Initialisation"
echo "======================================================"

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &
MSSQL_PID=$!

# Wait for SQL Server to be ready (retry up to 30 times, 2-second intervals)
echo "Waiting for SQL Server to start..."
READY=0
for i in $(seq 1 30); do
    # || true prevents set -e from aborting the script while SQL Server is warming up
    if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -No -Q "SELECT 1" > /dev/null 2>&1; then
        READY=1
        echo "SQL Server is ready after $i attempt(s)."
        break
    fi
    echo "  Attempt $i/30 - SQL Server not ready yet, waiting 2s..."
    sleep 2
done

if [ $READY -eq 0 ]; then
    echo "ERROR: SQL Server did not become ready in time. Exiting."
    exit 1
fi

# Note: ADMINDB will be created by Entity Framework migrations during app startup
echo "======================================================"
echo " ADMINDB will be initialised by Entity Framework."
echo "======================================================"

# Hand off to the SQL Server foreground process
wait $MSSQL_PID
