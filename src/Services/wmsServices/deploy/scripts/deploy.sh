#!/bin/bash
# =============================================================================
# WMS Microservices - Full Deployment Script
# Deploys infrastructure + all services via Docker Compose
# =============================================================================
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/../.." && pwd)"

ACTION="${1:-up}"

echo "=============================================="
echo " WMS Microservices - Deployment"
echo " Action: $ACTION"
echo "=============================================="

cd "$ROOT_DIR"

case "$ACTION" in
  up)
    echo ""
    echo "[1/4] Building all Docker images..."
    docker compose build --parallel

    echo ""
    echo "[2/4] Starting infrastructure (SQL Server + RabbitMQ)..."
    docker compose up -d sqlserver rabbitmq
    echo "Waiting for infrastructure to be healthy..."
    sleep 10

    echo ""
    echo "[3/4] Initializing databases..."
    echo "Waiting for SQL Server to accept connections..."
    for i in $(seq 1 30); do
      if docker exec wms-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT 1" &>/dev/null; then
        echo "SQL Server is ready."
        break
      fi
      echo "  Attempt $i/30 - waiting..."
      sleep 5
    done

    # Run database init script
    if [ -f "$ROOT_DIR/deploy/sql/init-databases.sql" ]; then
      echo "Running database initialization script..."
      docker exec -i wms-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C < "$ROOT_DIR/deploy/sql/init-databases.sql"
      echo "Databases created."
    fi

    echo ""
    echo "[4/4] Starting all microservices..."
    docker compose up -d
    echo ""

    echo "=============================================="
    echo " Deployment complete!"
    echo "=============================================="
    echo ""
    echo " API Gateway:      http://localhost:5000"
    echo " RabbitMQ Console:  http://localhost:15672"
    echo " SQL Server:        localhost:1433"
    echo ""
    echo " Health check:      http://localhost:5000/health"
    echo " Service info:      http://localhost:5000/"
    echo ""
    docker compose ps
    ;;

  down)
    echo "Stopping all services..."
    docker compose down
    echo "All services stopped."
    ;;

  restart)
    echo "Restarting all services..."
    docker compose restart
    echo "All services restarted."
    ;;

  logs)
    SERVICE="${2:-}"
    if [ -n "$SERVICE" ]; then
      docker compose logs -f "$SERVICE"
    else
      docker compose logs -f --tail=100
    fi
    ;;

  status)
    docker compose ps
    echo ""
    echo "Health Checks:"
    echo "  Gateway: $(curl -s -o /dev/null -w "%{http_code}" http://localhost:5000/health 2>/dev/null || echo "DOWN")"
    ;;

  clean)
    echo "WARNING: This will remove all containers, volumes, and images!"
    read -p "Are you sure? (y/N): " confirm
    if [ "$confirm" = "y" ] || [ "$confirm" = "Y" ]; then
      docker compose down -v --rmi local
      echo "Cleaned up all resources."
    else
      echo "Aborted."
    fi
    ;;

  init-db)
    echo "Running database initialization script..."
    source "$ROOT_DIR/.env"
    docker exec -i wms-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C < "$ROOT_DIR/deploy/sql/init-databases.sql"
    echo "Databases initialized."
    ;;

  *)
    echo "Usage: $0 {up|down|restart|logs [service]|status|clean|init-db}"
    exit 1
    ;;
esac
