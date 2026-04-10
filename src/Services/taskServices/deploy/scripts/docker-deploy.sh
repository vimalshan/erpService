#!/bin/bash
set -euo pipefail

###############################################################################
# docker-deploy.sh — Deploy ERP Microservices with Docker Compose
###############################################################################

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/../.." && pwd)"
ENV_FILE="$SCRIPT_DIR/../.env"

echo "============================================"
echo "  ERP Microservices — Docker Compose Deploy"
echo "============================================"

# Check .env
if [ ! -f "$ENV_FILE" ]; then
    echo "ERROR: .env file not found at $ENV_FILE"
    echo "Copy .env.example and fill in values:"
    echo "  cp deploy/.env.example deploy/.env"
    exit 1
fi

# Validate required vars
required_vars=("SQL_SA_PASSWORD" "RABBITMQ_DEFAULT_USER" "RABBITMQ_DEFAULT_PASS" "JWT_SECRET_KEY")
source "$ENV_FILE"
for var in "${required_vars[@]}"; do
    if [ -z "${!var:-}" ]; then
        echo "ERROR: $var is not set in $ENV_FILE"
        exit 1
    fi
done
echo "  ✓ Environment variables validated"

ACTION="${1:-up}"

case "$ACTION" in
    up)
        echo ""
        echo "Starting all services..."
        docker compose -f "$ROOT_DIR/docker-compose.yml" --env-file "$ENV_FILE" up -d --build
        echo ""
        echo "Services started. Checking health..."
        sleep 5
        docker compose -f "$ROOT_DIR/docker-compose.yml" ps
        echo ""
        echo "Gateway: http://localhost:5000"
        echo "RabbitMQ: http://localhost:15672"
        ;;
    down)
        echo ""
        echo "Stopping all services..."
        docker compose -f "$ROOT_DIR/docker-compose.yml" --env-file "$ENV_FILE" down
        echo "All services stopped."
        ;;
    restart)
        echo ""
        echo "Restarting all services..."
        docker compose -f "$ROOT_DIR/docker-compose.yml" --env-file "$ENV_FILE" down
        docker compose -f "$ROOT_DIR/docker-compose.yml" --env-file "$ENV_FILE" up -d --build
        echo "All services restarted."
        ;;
    logs)
        SERVICE="${2:-}"
        if [ -n "$SERVICE" ]; then
            docker compose -f "$ROOT_DIR/docker-compose.yml" logs -f "$SERVICE"
        else
            docker compose -f "$ROOT_DIR/docker-compose.yml" logs -f
        fi
        ;;
    status)
        docker compose -f "$ROOT_DIR/docker-compose.yml" ps
        ;;
    clean)
        echo ""
        read -rp "This will remove all containers, volumes, and images. Continue? [y/N] " answer
        if [[ "$answer" =~ ^[Yy]$ ]]; then
            docker compose -f "$ROOT_DIR/docker-compose.yml" --env-file "$ENV_FILE" down -v --rmi local
            echo "Cleaned up."
        fi
        ;;
    *)
        echo "Usage: $0 {up|down|restart|logs [service]|status|clean}"
        exit 1
        ;;
esac
