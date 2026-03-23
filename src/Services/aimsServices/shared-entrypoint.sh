#!/bin/bash
set -e

echo "======================================================"
echo " AIMS Services - Shared SQL Server Initialisation"
echo "======================================================"

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &
MSSQL_PID=$!

# Wait for SQL Server to be ready
echo "Waiting for SQL Server to start..."
READY=0
for i in $(seq 1 60); do
    if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -No -Q "SELECT 1" > /dev/null 2>&1; then
        READY=1
        echo "SQL Server is ready after $i attempt(s)."
        break
    fi
    echo "  Attempt $i/60 - SQL Server not ready yet, waiting 2s..."
    sleep 2
done

if [ $READY -eq 0 ]; then
    echo "ERROR: SQL Server did not become ready in time. Exiting."
    exit 1
fi

# Run the combined init script
echo "Running combined database init script..."
if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -No -i /init-all-databases.sql; then
    echo "Database init completed successfully."
else
    echo "WARNING: Database init script had errors (non-fatal)."
fi

echo "======================================================"
echo " All databases initialised. SQL Server is ready."
echo "======================================================"

# Hand off to the SQL Server foreground process
wait $MSSQL_PID
