#!/bin/bash
# ============================================================================
# SPARSH Platform - Docker Compose Deployment Script
# Usage: ./scripts/deploy-docker.sh [up|down|restart|logs|status]
# ============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

# Load .env if present
if [ -f "$ROOT_DIR/.env" ]; then
    set -a; source "$ROOT_DIR/.env"; set +a
fi

ACTION="${1:-up}"

case "$ACTION" in
    up)
        echo "Starting SPARSH platform..."
        docker compose -f "$ROOT_DIR/docker-compose.yml" up -d --build
        echo ""
        echo "Services starting. Check status with: $0 status"
        echo "API Gateway: http://localhost:5200"
        echo "RabbitMQ UI: http://localhost:15672"
        ;;
    down)
        echo "Stopping SPARSH platform..."
        docker compose -f "$ROOT_DIR/docker-compose.yml" down
        echo "Stopped."
        ;;
    restart)
        SERVICE="${2:-}"
        if [ -n "$SERVICE" ]; then
            echo "Restarting $SERVICE..."
            docker compose -f "$ROOT_DIR/docker-compose.yml" restart "$SERVICE"
        else
            echo "Restarting all services..."
            docker compose -f "$ROOT_DIR/docker-compose.yml" down
            docker compose -f "$ROOT_DIR/docker-compose.yml" up -d --build
        fi
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
        echo "SPARSH Platform - Service Status"
        echo "================================="
        docker compose -f "$ROOT_DIR/docker-compose.yml" ps
        ;;
    *)
        echo "Usage: $0 [up|down|restart|logs|status]"
        echo "  up              Start all services"
        echo "  down            Stop all services"
        echo "  restart [svc]   Restart all or specific service"
        echo "  logs [svc]      Tail logs for all or specific service"
        echo "  status          Show service status"
        exit 1
        ;;
esac
