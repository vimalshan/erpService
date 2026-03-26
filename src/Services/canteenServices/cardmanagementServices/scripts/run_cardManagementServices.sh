#!/bin/bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(dirname "$SCRIPT_DIR")"
cd "$PROJECT_ROOT"

FRESH=false
for arg in "$@"; do
  case "$arg" in --fresh) FRESH=true ;; esac
done

echo "================================================"
echo " CardManagement Microservice — Docker Run"
echo "================================================"

docker rm -f card-management-service mssql-card-management-db card-management-rabbitmq 2>/dev/null || true
docker compose down 2>/dev/null || true

if [ "$FRESH" = true ]; then
    echo "[fresh] Removing volumes..."
    docker compose down -v 2>/dev/null || true
fi

echo "Building and starting containers..."
docker compose up --build -d

echo "Waiting for service to become healthy..."
for i in $(seq 1 18); do
    STATUS=$(docker inspect --format='{{.State.Health.Status}}' card-management-service 2>/dev/null || echo "starting")
    [ "$STATUS" = "healthy" ] && echo "Service is healthy!" && break
    echo "    ($((i * 5))s) status: $STATUS"; sleep 5
done

echo ""
echo "Container logs (last 40 lines):"
docker compose logs --tail=40 card-management-service

echo ""
echo "Health check:"
curl -sf http://localhost:5191/health && echo " OK" || echo " FAILED"

echo ""
echo "================================================"
echo " CardManagement Service URLs:"
echo "   Swagger    : http://localhost:5191/swagger"
echo "   GraphQL    : http://localhost:5191/graphql"
echo "   Health     : http://localhost:5191/health"
echo "   RabbitMQ   : http://localhost:15677 (guest/guest)"
echo "================================================"
