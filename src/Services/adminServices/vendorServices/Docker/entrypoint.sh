#!/bin/bash
set -e

echo "======================================================"
echo " Vendor Service - SQL Server Initialisation"
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

# Run the VENDORDB initialisation script
echo "Running VENDORDB init script..."
if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -No -i /init-database.sql; then
    echo "VENDORDB init script completed successfully."
else
    echo "ERROR: VENDORDB init script failed."
    exit 1
fi

# Verify VENDORDB was created and tables exist
echo "Verifying VENDORDB setup..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -No -d VENDORDB -Q \
    "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;"
echo "======================================================"
echo " VENDORDB initialisation complete. Service is ready."
echo "======================================================"

# Hand off to the SQL Server foreground process
wait $MSSQL_PID
