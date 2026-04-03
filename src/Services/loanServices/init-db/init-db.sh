#!/bin/bash
# =============================================================================
#  init-db.sh
#  SQL Server container entrypoint: waits for SQL Server to be ready,
#  then runs all init scripts in order.
# =============================================================================
set -e

SA_PASS="${SA_PASSWORD:-LoanERP_StrongPass!2025}"
SQLCMD="/opt/mssql-tools18/bin/sqlcmd"
SERVER="localhost"
MAX_WAIT=60
WAIT_INTERVAL=3

echo "[init-db] Waiting for SQL Server to be ready..."
for i in $(seq 1 $((MAX_WAIT / WAIT_INTERVAL))); do
    if $SQLCMD -S "$SERVER" -U sa -P "$SA_PASS" -Q "SELECT 1" -No 2>/dev/null; then
        echo "[init-db] SQL Server is ready."
        break
    fi
    echo "[init-db] Attempt $i/$((MAX_WAIT / WAIT_INTERVAL)) — waiting ${WAIT_INTERVAL}s..."
    sleep $WAIT_INTERVAL
done

# Run scripts in numerical order
SCRIPT_DIR="$(dirname "$0")"
for script in "$SCRIPT_DIR"/0*.sql; do
    echo "[init-db] Executing: $script"
    $SQLCMD -S "$SERVER" -U sa -P "$SA_PASS" -i "$script" -No
    echo "[init-db] Completed: $script"
done

echo "[init-db] All initialization scripts completed successfully."
