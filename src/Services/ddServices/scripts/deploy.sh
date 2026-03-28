#!/bin/bash
# ──────────────────────────────────────────────────────────────────────────────
# deploy.sh — Deploy DD ERP Microservices with Docker Compose
# Uses existing infrastructure (erp-sqlserver, erp-rabbitmq, erp-azurite)
# on the erp-network. Use --with-infra to also start DD-specific infra.
# Usage: ./scripts/deploy.sh [up|down|restart|logs|status|clean] [--with-infra]
# ──────────────────────────────────────────────────────────────────────────────

set -e
cd "$(dirname "$0")/.."

ACTION="${1:-up}"
WITH_INFRA=false

for arg in "$@"; do
    case $arg in
        --with-infra) WITH_INFRA=true ;;
    esac
done

if [ "$WITH_INFRA" = true ]; then
    COMPOSE_FILES="-f docker-compose.shared.yml -f docker-compose.yml"
else
    COMPOSE_FILES="-f docker-compose.yml"
fi

# Load .env if exists
if [ -f .env ]; then
    export $(grep -v '^#' .env | xargs)
fi

case "$ACTION" in
    up)
        echo "============================================"
        echo " DD ERP — Starting All Services"
        echo "============================================"
        echo ""

        if [ "$WITH_INFRA" = true ]; then
            echo "[1/3] Starting DD infrastructure (SQL Server, RabbitMQ, Azurite)..."
            docker compose -f docker-compose.shared.yml up -d
            echo ""
            echo "[2/3] Waiting for infrastructure to be healthy..."
            echo "      Waiting for SQL Server..."
            until docker compose -f docker-compose.shared.yml exec -T sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "${DB_PASSWORD:-YourStrong@Passw0rd}" -C -Q "SELECT 1" &>/dev/null; do
                sleep 3
            done
            echo "      [✓] SQL Server is ready"
            echo "      Waiting for RabbitMQ..."
            until docker compose -f docker-compose.shared.yml exec -T rabbitmq rabbitmq-diagnostics -q ping &>/dev/null; do
                sleep 3
            done
            echo "      [✓] RabbitMQ is ready"
        else
            echo "[1/2] Checking existing infrastructure on erp-network..."
            if docker ps --filter name=erp-sqlserver --format '{{.Names}}' | grep -q erp-sqlserver; then
                echo "      [✓] erp-sqlserver is running"
            else
                echo "      [✗] erp-sqlserver NOT found! Use --with-infra or start it manually."
                exit 1
            fi
            if docker ps --filter name=erp-rabbitmq --format '{{.Names}}' | grep -q erp-rabbitmq; then
                echo "      [✓] erp-rabbitmq is running"
            else
                echo "      [✗] erp-rabbitmq NOT found! Use --with-infra or start it manually."
                exit 1
            fi
        fi

        echo ""
        STEP=$( [ "$WITH_INFRA" = true ] && echo "3/3" || echo "2/2" )
        echo "[$STEP] Starting microservices and API Gateway..."
        docker compose $COMPOSE_FILES up -d --build
        echo ""
        echo "============================================"
        echo " [✓] All services started!"
        echo "============================================"
        echo ""
        echo " Gateway:    http://localhost:5200"
        echo " RabbitMQ:   http://localhost:15672"
        echo " Health:     http://localhost:5200/health"
        echo " Services:   http://localhost:5200/gateway/services"
        ;;

    down)
        echo "[*] Stopping all services..."
        docker compose $COMPOSE_FILES down
        echo "[✓] All services stopped."
        ;;

    restart)
        echo "[*] Restarting all services..."
        docker compose $COMPOSE_FILES restart
        echo "[✓] All services restarted."
        ;;

    logs)
        SERVICE="${2:-}"
        if [ -n "$SERVICE" ]; then
            docker compose $COMPOSE_FILES logs -f "$SERVICE"
        else
            docker compose $COMPOSE_FILES logs -f --tail=50
        fi
        ;;

    status)
        echo "============================================"
        echo " DD ERP — Service Status"
        echo "============================================"
        docker compose $COMPOSE_FILES ps
        ;;

    clean)
        echo "[!] Stopping and removing all containers, volumes, and networks..."
        docker compose $COMPOSE_FILES down -v --remove-orphans
        echo "[✓] Cleaned up."
        ;;

    *)
        echo "Usage: $0 [up|down|restart|logs|status|clean] [--with-infra]"
        echo ""
        echo "  up       Start all services (default — uses existing erp-* infra)"
        echo "  down     Stop all services"
        echo "  restart  Restart all services"
        echo "  logs     View logs (optionally: logs <service-name>)"
        echo "  status   Show service status"
        echo "  clean    Stop and remove everything including volumes"
        echo ""
        echo "  --with-infra  Also start DD-specific SQL Server, RabbitMQ, Azurite"
        exit 1
        ;;
esac
