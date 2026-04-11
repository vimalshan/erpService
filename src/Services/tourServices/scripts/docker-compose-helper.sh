#!/bin/bash
# =============================================================================
# Tour ERP - Docker Compose Helpers
# =============================================================================
set -e

SCRIPT_DIR="$(dirname "$0")"
COMPOSE_FILE="$SCRIPT_DIR/../docker-compose.yml"

ACTION="${1:-up}"

case "$ACTION" in
  up)
    echo "Starting Tour ERP services..."
    docker compose -f "$COMPOSE_FILE" up -d --build
    echo ""
    echo "Services started. Access:"
    echo "  API Gateway:  http://localhost:5000"
    echo "  RabbitMQ UI:  http://localhost:15672"
    echo "  SQL Server:   localhost:1433"
    echo ""
    docker compose -f "$COMPOSE_FILE" ps
    ;;
  down)
    echo "Stopping Tour ERP services..."
    docker compose -f "$COMPOSE_FILE" down
    echo "Services stopped."
    ;;
  restart)
    echo "Restarting Tour ERP services..."
    docker compose -f "$COMPOSE_FILE" down
    docker compose -f "$COMPOSE_FILE" up -d --build
    echo "Services restarted."
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
    echo "Stopping and removing all containers, volumes, and networks..."
    docker compose -f "$COMPOSE_FILE" down -v --remove-orphans
    echo "Cleaned up."
    ;;
  *)
    echo "Usage: $0 {up|down|restart|logs [service]|status|clean}"
    exit 1
    ;;
esac
