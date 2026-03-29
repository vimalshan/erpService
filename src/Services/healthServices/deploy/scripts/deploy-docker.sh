#!/bin/bash
# ================================================
# Health ERP - Docker Compose Deployment Script
# ================================================
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
DEPLOY_DIR="$(dirname "$SCRIPT_DIR")"
ROOT_DIR="$(dirname "$DEPLOY_DIR")"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

log_info()  { echo -e "${GREEN}[INFO]${NC} $1"; }
log_warn()  { echo -e "${YELLOW}[WARN]${NC} $1"; }
log_error() { echo -e "${RED}[ERROR]${NC} $1"; }

# ------ Pre-checks ------
command -v docker >/dev/null 2>&1 || { log_error "Docker is not installed"; exit 1; }
command -v docker-compose >/dev/null 2>&1 || command -v "docker compose" >/dev/null 2>&1 || { log_error "Docker Compose is not installed"; exit 1; }

# ------ Environment setup ------
ENV_FILE="$DEPLOY_DIR/.env"
if [ ! -f "$ENV_FILE" ]; then
    if [ -f "$DEPLOY_DIR/.env.template" ]; then
        log_warn ".env file not found. Copying from .env.template..."
        cp "$DEPLOY_DIR/.env.template" "$ENV_FILE"
        log_warn "Please update $ENV_FILE with your actual passwords before running in production!"
    else
        log_error ".env file not found and no template available."; exit 1;
    fi
fi

# ------ Parse arguments ------
ACTION="${1:-up}"
PROFILE="${2:-}"

case "$ACTION" in
    up)
        log_info "Starting Health ERP services..."
        cd "$DEPLOY_DIR"
        docker compose --env-file .env up -d --build
        log_info "Waiting for services to start..."
        sleep 10
        log_info "Checking service health..."
        docker compose ps
        echo ""
        log_info "Services are starting up. Check health at:"
        echo "  API Gateway:   http://localhost:5600/health"
        echo "  RabbitMQ Mgmt: http://localhost:15672"
        ;;
    down)
        log_info "Stopping Health ERP services..."
        cd "$DEPLOY_DIR"
        docker compose down
        log_info "All services stopped."
        ;;
    restart)
        log_info "Restarting Health ERP services..."
        cd "$DEPLOY_DIR"
        docker compose down
        docker compose --env-file .env up -d --build
        log_info "Services restarted."
        ;;
    logs)
        SERVICE="${PROFILE:-}"
        cd "$DEPLOY_DIR"
        if [ -n "$SERVICE" ]; then
            docker compose logs -f "$SERVICE"
        else
            docker compose logs -f
        fi
        ;;
    status)
        cd "$DEPLOY_DIR"
        docker compose ps
        ;;
    clean)
        log_warn "This will remove all containers, volumes, and images!"
        read -p "Are you sure? (y/N) " -n 1 -r
        echo
        if [[ $REPLY =~ ^[Yy]$ ]]; then
            cd "$DEPLOY_DIR"
            docker compose down -v --rmi local
            log_info "Cleanup complete."
        else
            log_info "Cleanup cancelled."
        fi
        ;;
    *)
        echo "Usage: $0 {up|down|restart|logs|status|clean} [service-name]"
        echo ""
        echo "Commands:"
        echo "  up       - Start all services (default)"
        echo "  down     - Stop all services"
        echo "  restart  - Restart all services"
        echo "  logs     - View logs (optionally for a specific service)"
        echo "  status   - Show service status"
        echo "  clean    - Remove all containers, volumes, and images"
        exit 1
        ;;
esac
