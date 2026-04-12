#!/bin/bash
# ==============================================================================
# ERP Travel Services - Docker Compose Deployment Script
# ==============================================================================
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m'

print_header() {
    echo ""
    echo -e "${CYAN}============================================${NC}"
    echo -e "${CYAN}  $1${NC}"
    echo -e "${CYAN}============================================${NC}"
    echo ""
}

print_step() {
    echo -e "${GREEN}[$(date +%H:%M:%S)]${NC} $1"
}

print_warn() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Usage
usage() {
    echo "Usage: $0 [COMMAND]"
    echo ""
    echo "Commands:"
    echo "  up          Build and start all services"
    echo "  down        Stop and remove all containers"
    echo "  build       Build all Docker images"
    echo "  start       Start existing containers"
    echo "  stop        Stop running containers"
    echo "  restart     Restart all services"
    echo "  logs        Show logs for all services"
    echo "  status      Show status of all services"
    echo "  health      Check health of all services"
    echo "  init-db     Initialize databases"
    echo "  clean       Remove all containers, images, and volumes"
    echo ""
    exit 1
}

# Check prerequisites
check_prereqs() {
    print_step "Checking prerequisites..."

    if ! command -v docker &> /dev/null; then
        print_error "Docker is not installed"
        exit 1
    fi

    if ! command -v docker compose &> /dev/null && ! command -v docker-compose &> /dev/null; then
        print_error "Docker Compose is not installed"
        exit 1
    fi

    if ! docker info &> /dev/null; then
        print_error "Docker daemon is not running"
        exit 1
    fi

    print_step "Prerequisites OK"
}

# Copy .env if not exists
setup_env() {
    if [ ! -f .env ]; then
        if [ -f .env.example ]; then
            cp .env.example .env
            print_warn ".env created from .env.example - review and update values before production use"
        fi
    fi
}

# Copy SQL scripts for DB init
copy_sql_scripts() {
    print_step "Copying SQL scripts for database initialization..."
    if [ -f deploy/copy-sql-scripts.sh ]; then
        bash deploy/copy-sql-scripts.sh
    else
        print_warn "copy-sql-scripts.sh not found, skipping SQL copy"
    fi
}

# Build images
build() {
    print_header "Building Docker Images"
    docker compose build --parallel
    print_step "All images built successfully"
}

# Start infrastructure first, then services
up() {
    print_header "Starting ERP Travel Services"
    check_prereqs
    setup_env
    copy_sql_scripts

    print_step "Starting infrastructure (SQL Server, RabbitMQ, Azurite)..."
    docker compose up -d sqlserver rabbitmq azurite

    print_step "Waiting for infrastructure to be healthy..."
    docker compose exec -T sqlserver bash -c '
        for i in $(seq 1 60); do
            /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "SELECT 1" > /dev/null 2>&1 && exit 0
            sleep 2
        done
        exit 1
    ' || {
        print_error "SQL Server failed to start"
        exit 1
    }
    print_step "SQL Server is ready"

    # Initialize databases
    init_db

    print_step "Starting all microservices..."
    docker compose up -d

    print_step "All services started!"
    echo ""
    status
}

# Stop
down() {
    print_header "Stopping ERP Travel Services"
    docker compose down
    print_step "All services stopped"
}

# Start
start() {
    print_header "Starting ERP Travel Services"
    docker compose start
    print_step "All services started"
}

# Stop
stop() {
    print_header "Stopping ERP Travel Services"
    docker compose stop
    print_step "All services stopped"
}

# Restart
restart() {
    print_header "Restarting ERP Travel Services"
    docker compose restart
    print_step "All services restarted"
}

# Logs
logs() {
    docker compose logs -f --tail=100 "$@"
}

# Status
status() {
    print_header "Service Status"
    docker compose ps --format "table {{.Name}}\t{{.Status}}\t{{.Ports}}"
}

# Health check
health() {
    print_header "Health Check"
    
    SERVICES=(
        "API Gateway:5100"
        "Travel Request:5205"
        "Travel Transaction:5082"
        "Booking:5117"
        "Expense:5090"
        "Finance:5294"
        "Insurance:5179"
        "MasterData:5166"
        "Agency:5000"
        "Admin:5001"
    )

    for svc in "${SERVICES[@]}"; do
        NAME="${svc%%:*}"
        PORT="${svc##*:}"
        STATUS=$(curl -s -o /dev/null -w "%{http_code}" --max-time 5 "http://localhost:${PORT}/health" 2>/dev/null || echo "000")
        if [ "$STATUS" = "200" ]; then
            echo -e "  ${GREEN}✓${NC} ${NAME} (port ${PORT}) - Healthy"
        else
            echo -e "  ${RED}✗${NC} ${NAME} (port ${PORT}) - HTTP ${STATUS}"
        fi
    done
    echo ""
}

# Initialize databases
init_db() {
    print_step "Initializing databases..."
    if [ -f deploy/init-db/init-databases.sh ]; then
        docker compose exec -T sqlserver bash /docker-entrypoint-initdb.d/init-databases.sh || {
            print_warn "DB init script failed - databases may already exist"
        }
    else
        print_warn "init-databases.sh not found"
    fi
}

# Clean everything
clean() {
    print_header "Cleaning ERP Travel Services"
    print_warn "This will remove ALL containers, images, and volumes!"
    read -p "Are you sure? (y/N): " confirm
    if [ "$confirm" = "y" ] || [ "$confirm" = "Y" ]; then
        docker compose down -v --rmi all
        print_step "Cleanup complete"
    else
        print_step "Cleanup cancelled"
    fi
}

# Main
case "${1:-}" in
    up)       up ;;
    down)     down ;;
    build)    build ;;
    start)    start ;;
    stop)     stop ;;
    restart)  restart ;;
    logs)     shift; logs "$@" ;;
    status)   status ;;
    health)   health ;;
    init-db)  init_db ;;
    clean)    clean ;;
    *)        usage ;;
esac
