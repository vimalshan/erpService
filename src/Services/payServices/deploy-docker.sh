#!/bin/bash
# ============================================================
# ERP Microservice - Docker Compose Deploy Script
# Usage: ./deploy-docker.sh [up|down|build|restart|logs|status]
# ============================================================

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
COMPOSE_FILE="$SCRIPT_DIR/docker-compose.yml"
ENV_FILE="$SCRIPT_DIR/.env"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

log_info()  { echo -e "${BLUE}[INFO]${NC} $1"; }
log_ok()    { echo -e "${GREEN}[OK]${NC} $1"; }
log_warn()  { echo -e "${YELLOW}[WARN]${NC} $1"; }
log_error() { echo -e "${RED}[ERROR]${NC} $1"; }

check_deps() {
    if ! command -v docker &> /dev/null; then
        log_error "Docker is not installed"
        exit 1
    fi
    if ! command -v docker compose &> /dev/null && ! command -v docker-compose &> /dev/null; then
        log_error "Docker Compose is not installed"
        exit 1
    fi
    log_ok "Dependencies verified"
}

create_env() {
    if [ ! -f "$ENV_FILE" ]; then
        log_warn ".env file not found, creating from .env.example"
        if [ -f "$SCRIPT_DIR/.env.example" ]; then
            cp "$SCRIPT_DIR/.env.example" "$ENV_FILE"
            log_ok "Created .env from .env.example — update secrets before production use"
        else
            log_error ".env.example not found"
            exit 1
        fi
    fi
}

compose_cmd() {
    if command -v docker compose &> /dev/null; then
        docker compose -f "$COMPOSE_FILE" --env-file "$ENV_FILE" "$@"
    else
        docker-compose -f "$COMPOSE_FILE" --env-file "$ENV_FILE" "$@"
    fi
}

cmd_build() {
    log_info "Building all service images..."
    compose_cmd build --parallel
    log_ok "All images built"
}

cmd_up() {
    log_info "Starting ERP Microservice stack..."
    create_env
    compose_cmd up -d
    log_ok "Stack started"
    echo ""
    log_info "Service endpoints:"
    echo "  API Gateway:             http://localhost:5100"
    echo "  Employee Service:        http://localhost:5104"
    echo "  HR Service:              http://localhost:5000"
    echo "  FAQ Service:             http://localhost:5032"
    echo "  Payroll Service:         http://localhost:5002"
    echo "  Tax Service:             http://localhost:5010"
    echo "  PayTransactional Service: http://localhost:5020"
    echo "  RabbitMQ Management:     http://localhost:15672"
    echo ""
    log_info "Health check: http://localhost:5100/health/services"
}

cmd_down() {
    log_info "Stopping ERP Microservice stack..."
    compose_cmd down
    log_ok "Stack stopped"
}

cmd_restart() {
    log_info "Restarting ERP Microservice stack..."
    cmd_down
    cmd_up
}

cmd_logs() {
    local service=${2:-""}
    if [ -n "$service" ]; then
        compose_cmd logs -f "$service"
    else
        compose_cmd logs -f
    fi
}

cmd_status() {
    log_info "Service status:"
    compose_cmd ps
    echo ""
    log_info "Checking health endpoints..."
    services=("5100:API-Gateway" "5104:Employee" "5000:HR" "5032:FAQ" "5002:Payroll" "5010:Tax" "5020:PayTransactional")
    for svc in "${services[@]}"; do
        port="${svc%%:*}"
        name="${svc##*:}"
        status=$(curl -s -o /dev/null -w "%{http_code}" "http://localhost:$port/health" 2>/dev/null || echo "000")
        if [ "$status" = "200" ]; then
            log_ok "$name (port $port): healthy"
        else
            log_warn "$name (port $port): HTTP $status"
        fi
    done
}

cmd_clean() {
    log_info "Stopping and removing all containers, volumes, and images..."
    compose_cmd down -v --rmi local
    log_ok "Clean complete"
}

# Main
check_deps

case "${1:-up}" in
    build)   cmd_build ;;
    up)      cmd_up ;;
    down)    cmd_down ;;
    restart) cmd_restart ;;
    logs)    cmd_logs "$@" ;;
    status)  cmd_status ;;
    clean)   cmd_clean ;;
    *)
        echo "Usage: $0 {build|up|down|restart|logs|status|clean}"
        echo ""
        echo "Commands:"
        echo "  build     Build all Docker images"
        echo "  up        Start entire stack (default)"
        echo "  down      Stop entire stack"
        echo "  restart   Restart stack"
        echo "  logs      View logs (optional: service name)"
        echo "  status    Show status & health checks"
        echo "  clean     Stop, remove volumes & images"
        exit 1
        ;;
esac
