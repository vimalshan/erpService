#!/bin/bash
# ==========================================
# SRF Sparsh - Docker Compose Deploy Script
# ==========================================

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
export IMAGE_TAG="${IMAGE_TAG:-latest}"

echo "============================================"
echo " SRF Sparsh - Docker Compose Deployment"
echo "============================================"

# Check .env file
if [ ! -f "$ROOT_DIR/.env" ]; then
    echo "Creating .env from .env.example..."
    cp "$ROOT_DIR/.env.example" "$ROOT_DIR/.env"
    echo "IMPORTANT: Update $ROOT_DIR/.env with production values!"
fi

cd "$ROOT_DIR"

case "${1:-up}" in
    up)
        echo "Starting all services..."
        docker compose up -d --build
        echo ""
        echo "Waiting for services to become healthy..."
        sleep 10
        echo ""
        echo "Service status:"
        docker compose ps
        echo ""
        echo "Gateway: http://localhost:5100"
        echo "RabbitMQ Management: http://localhost:15672"
        ;;
    down)
        echo "Stopping all services..."
        docker compose down
        ;;
    restart)
        echo "Restarting all services..."
        docker compose down
        docker compose up -d --build
        ;;
    logs)
        docker compose logs -f "${2:-}"
        ;;
    status)
        docker compose ps
        ;;
    infra)
        echo "Starting infrastructure only..."
        docker compose up -d sqlserver rabbitmq redis azurite
        ;;
    clean)
        echo "Stopping and removing all data..."
        docker compose down -v --remove-orphans
        ;;
    *)
        echo "Usage: $0 {up|down|restart|logs|status|infra|clean}"
        exit 1
        ;;
esac
