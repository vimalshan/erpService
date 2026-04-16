#!/usr/bin/env bash
# =============================================================================
# init-db.sh — Run the init.sql against a running SQL Server container
# (Docker Compose workflow only — K8s uses the db-init Job in compose)
# Usage: ./init-db.sh
# =============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
INIT_SQL="$(cd "$SCRIPT_DIR/../database" && pwd)/init.sql"

SA_PASSWORD="${SQL_SA_PASSWORD:-ErpStr0ng!Pass}"
CONTAINER="erp-sqlserver"

echo "========================================="
echo " Running DB init script"
echo " Container: $CONTAINER"
echo "========================================="

docker exec -i "$CONTAINER" \
  /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "$SA_PASSWORD" \
    -i /dev/stdin \
    -C -b \
  < "$INIT_SQL"

echo ""
echo "Database initialisation complete."
