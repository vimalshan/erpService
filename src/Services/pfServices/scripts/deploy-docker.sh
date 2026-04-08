#!/bin/bash
# ============================================
# Deploy PF Services with Docker Compose
# ============================================
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"
cd "$PROJECT_DIR"

ACTION="${1:-up}"

echo "============================================"
echo "PF Services - Docker Compose Deployment"
echo "Action: ${ACTION}"
echo "============================================"

# Source .env if it exists
if [ -f .env ]; then
    echo "Loading environment from .env"
    set -a
    source .env
    set +a
else
    echo "WARNING: No .env file found. Using defaults."
    echo "Copy .env.example to .env and update values."
fi

case "$ACTION" in
    up)
        echo "Starting infrastructure..."
        docker compose up -d sqlserver rabbitmq
        echo "Waiting for infrastructure to be healthy..."
        docker compose up sqlserver-init

        echo "Starting all services..."
        docker compose up -d

        echo ""
        echo "============================================"
        echo "Services are starting."
        echo "API Gateway: http://localhost:5800"
        echo "RabbitMQ UI: http://localhost:15672"
        echo "============================================"
        echo ""
        echo "Run 'docker compose ps' to check status."
        echo "Run 'docker compose logs -f <service>' to view logs."
        ;;

    down)
        echo "Stopping all services..."
        docker compose down
        echo "All services stopped."
        ;;

    restart)
        echo "Restarting all services..."
        docker compose restart
        ;;

    build)
        echo "Building all images..."
        docker compose build
        ;;

    logs)
        SERVICE="${2:-}"
        if [ -n "$SERVICE" ]; then
            docker compose logs -f "$SERVICE"
        else
            docker compose logs -f
        fi
        ;;

    status)
        docker compose ps
        ;;

    clean)
        echo "Stopping and removing all containers, volumes, and networks..."
        docker compose down -v --remove-orphans
        echo "Cleaned up."
        ;;

    *)
        echo "Usage: $0 {up|down|restart|build|logs [service]|status|clean}"
        exit 1
        ;;
esac
