#!/bin/bash
set -e

echo "======================================================"
echo " SwipeTransactionService - SQL Server Initialisation"
echo "======================================================"

/opt/mssql/bin/sqlservr &
MSSQL_PID=$!

echo "Waiting for SQL Server to start..."
READY=0
for i in $(seq 1 30); do
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

echo "Running SwipeTransactionDb init script..."
if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -No -i /init-database.sql; then
    echo "SwipeTransactionDb init script completed successfully."
else
    echo "ERROR: SwipeTransactionDb init script failed."
    exit 1
fi

echo "Verifying SwipeTransactionDb setup..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -No -d SwipeTransactionDb -Q \
    "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;"
echo "======================================================"
echo " SwipeTransactionDb initialisation complete. Service is ready."
echo "======================================================"

wait $MSSQL_PID
