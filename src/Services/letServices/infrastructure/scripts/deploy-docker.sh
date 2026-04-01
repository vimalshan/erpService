#!/bin/bash
# ═══════════════════════════════════════════════════════════════════════
# deploy-docker.sh — Deploy LET ERP via Docker Compose
# Usage: ./deploy-docker.sh [up|down|restart|logs|status]
# ═══════════════════════════════════════════════════════════════════════
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/../.." && pwd)"
COMPOSE_FILE="$ROOT_DIR/docker-compose.yml"
ENV_FILE="$ROOT_DIR/.env"

# Load .env if exists
if [ -f "$ENV_FILE" ]; then
    echo "Loading environment from .env"
    set -a; source "$ENV_FILE"; set +a
fi

ACTION="${1:-up}"

case "$ACTION" in
    up)
        echo "═══ Starting LET ERP Stack ═══"
        docker compose -f "$COMPOSE_FILE" up -d --build
        echo ""
        echo "Waiting for services to be healthy..."
        sleep 10
        docker compose -f "$COMPOSE_FILE" ps
        echo ""
        echo "═══ Stack is running ═══"
        echo "  API Gateway:     http://localhost:5400"
        echo "  RabbitMQ Mgmt:   http://localhost:15672"
        echo "  SQL Server:      localhost:1433"
        ;;
    down)
        echo "═══ Stopping LET ERP Stack ═══"
        docker compose -f "$COMPOSE_FILE" down
        echo "Stack stopped."
        ;;
    restart)
        echo "═══ Restarting LET ERP Stack ═══"
        docker compose -f "$COMPOSE_FILE" down
        docker compose -f "$COMPOSE_FILE" up -d --build
        echo "Stack restarted."
        ;;
    logs)
        SERVICE="${2:-}"
        if [ -n "$SERVICE" ]; then
            docker compose -f "$COMPOSE_FILE" logs -f "$SERVICE"
        else
            docker compose -f "$COMPOSE_FILE" logs -f
        fi
        ;;
    status)
        docker compose -f "$COMPOSE_FILE" ps
        ;;
    clean)
        echo "═══ Removing all containers, volumes, and images ═══"
        docker compose -f "$COMPOSE_FILE" down -v --rmi local
        echo "Cleaned."
        ;;
    *)
        echo "Usage: $0 [up|down|restart|logs|status|clean]"
        exit 1
        ;;
esac
