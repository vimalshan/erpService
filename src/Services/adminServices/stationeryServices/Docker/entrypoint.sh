#!/bin/bash
set -e

# Source environment if needed
export SA_PASSWORD=${SA_PASSWORD:-"YourPassword123!"}

echo "======================================================"
echo " Stationery Service - SQL Server Initialisation"
echo "======================================================"
echo "SA_PASSWORD: ${SA_PASSWORD:0:3}***"

# Start SQL Server in the background
echo "Starting SQL Server..."
/opt/mssql/bin/sqlservr &
MSSQL_PID=$!

# Wait for SQL Server to be ready (retry up to 60 times, 2-second intervals = 120 seconds total)
echo "Waiting for SQL Server to start..."
READY=0
for i in $(seq 1 60); do
    # || true prevents set -e from aborting the script while SQL Server is warming up
    if /opt/mssql-tools18/bin/sqlcmd -S 127.0.0.1 -U sa -P "$SA_PASSWORD" -No -Q "SELECT 1" > /dev/null 2>&1; then
        READY=1
        echo "✓ SQL Server is ready after $i attempt(s) (~$((i*2)) seconds)."
        break
    fi
    echo "  Attempt $i/60 - SQL Server not ready yet, waiting 2s..."
    sleep 2
done

if [ $READY -eq 0 ]; then
    echo "ERROR: SQL Server did not become ready in time. Exiting."
    exit 1
fi

# Run the STATIONERYDB initialisation script
echo ""
echo "Running STATIONERYDB init script..."
if /opt/mssql-tools18/bin/sqlcmd -S 127.0.0.1 -U sa -P "$SA_PASSWORD" -No -i /init-database.sql; then
    echo "✓ STATIONERYDB init script completed successfully."
else
    echo "ERROR: STATIONERYDB init script failed."
    exit 1
fi

# Verify STATIONERYDB was created and tables exist
echo ""
echo "Verifying STATIONERYDB setup..."
TABLE_COUNT=$(/opt/mssql-tools18/bin/sqlcmd -S 127.0.0.1 -U sa -P "$SA_PASSWORD" -No -d STATIONERYDB -Q "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';" | grep -oE '^[0-9]+$' | head -1)
echo "✓ Found $TABLE_COUNT tables in STATIONERYDB"

echo ""
echo "======================================================"
echo " ✓ STATIONERYDB initialisation complete"
echo " ✓ Service is ready to accept connections"
echo "======================================================"
echo ""

# Hand off to the SQL Server foreground process
wait $MSSQL_PID
