#!/bin/bash
# ─── Deploy SSC Services using Docker Compose ─────────────────────────────
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

cd "$ROOT_DIR"

ACTION="${1:-up}"

case "$ACTION" in
    up)
        echo "============================================"
        echo "Starting SSC Services (Docker Compose)"
        echo "============================================"

        # Copy .env.example if .env doesn't exist
        if [ ! -f .env ]; then
            echo "Creating .env from .env.example..."
            cp .env.example .env
            echo "WARNING: Using default credentials. Update .env for production!"
        fi

        docker compose up -d --build
        echo ""
        echo "Services starting. Check status with:"
        echo "  docker compose ps"
        echo "  docker compose logs -f"
        echo ""
        echo "Service endpoints:"
        echo "  SSC Transactional:   http://localhost:8080"
        echo "  Batch & Envelope:    http://localhost:8081"
        echo "  Category & Vendor:   http://localhost:8082"
        echo "  Club Membership:     http://localhost:8083"
        echo "  Filing & Archive:    http://localhost:8084"
        echo "  HR Document:         http://localhost:8085"
        echo "  Integration:         http://localhost:8086"
        echo "  Invoice Processing:  http://localhost:8087"
        echo "  Master Data:         http://localhost:8088"
        echo "  Menu & Security:     http://localhost:8089"
        echo "  Approval Group:      http://localhost:8090"
        echo "  User Service:        http://localhost:8091"
        echo ""
        echo "  API Gateway:         http://localhost:5000"
        echo "  Gateway Health:      http://localhost:5000/health"
        echo ""
        echo "  RabbitMQ Management: http://localhost:15672"
        echo "  SQL Server:          localhost:1433"
        ;;
    down)
        echo "Stopping SSC Services..."
        docker compose down
        echo "All services stopped."
        ;;
    restart)
        echo "Restarting SSC Services..."
        docker compose down
        docker compose up -d --build
        echo "Services restarted."
        ;;
    logs)
        docker compose logs -f "${@:2}"
        ;;
    status)
        docker compose ps
        ;;
    init-db)
        echo "Initializing database..."
        docker compose exec sqlserver bash /docker-entrypoint-initdb.d/init-db.sh
        echo "Database initialized."
        ;;
    *)
        echo "Usage: $0 {up|down|restart|logs|status|init-db}"
        echo ""
        echo "Commands:"
        echo "  up       - Start all services"
        echo "  down     - Stop all services"
        echo "  restart  - Restart all services"
        echo "  logs     - Follow service logs (optional: service name)"
        echo "  status   - Show service status"
        echo "  init-db  - Run database initialization script"
        exit 1
        ;;
esac
