#!/bin/bash
# Start SQL Server in the background
/opt/mssql/bin/sqlservr &
MSSQL_PID=$!

# Wait for SQL Server to be ready (retry up to 30 times, 2-second intervals)
echo "Waiting for SQL Server to start..."
for i in $(seq 1 30); do
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -No -Q "SELECT 1" > /dev/null 2>&1
    if [ $? -eq 0 ]; then
        echo "SQL Server is ready. Running init script..."
        /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -No -i /init-database.sql
        echo "Init script completed."
        break
    fi
    echo "  Attempt $i/30 - SQL Server not ready yet, waiting 2s..."
    sleep 2
done

# Hand off to the SQL Server foreground process
wait $MSSQL_PID
