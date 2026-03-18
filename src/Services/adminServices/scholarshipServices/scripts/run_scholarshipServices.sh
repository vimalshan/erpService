#!/bin/bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(dirname "$SCRIPT_DIR")"

cd "$PROJECT_ROOT"

# ── Parse flags ───────────────────────────────────────────────────────────────
FRESH=false
for arg in "$@"; do
  case "$arg" in
    --fresh) FRESH=true ;;
  esac
done

# ── Tear down existing stack ───────────────────────────────────────────────────
echo "==> Stopping existing containers..."
# Force-remove any orphan containers created outside of compose (e.g. by docker run)
docker rm -f scholarship-service mssql-scholarship-db scholarship-rabbitmq 2>/dev/null || true
docker compose down 2>/dev/null || true

if [ "$FRESH" = true ]; then
  echo "==> --fresh: removing volumes (database will be re-initialised)..."
  docker compose down -v 2>/dev/null || true
fi

# ── Build and start full stack ────────────────────────────────────────────────
echo "==> Building and starting full stack (SQL Server + RabbitMQ + ScholarshipService)..."
docker compose up --build -d

# ── Wait for scholarship-service to become healthy ────────────────────────────────────
echo "==> Waiting for scholarship-service to become healthy (up to 90 s)..."
for i in $(seq 1 18); do
  STATUS=$(docker inspect --format='{{.State.Health.Status}}' scholarship-service 2>/dev/null || echo "starting")
  if [ "$STATUS" = "healthy" ]; then
    echo "    scholarship-service is healthy!"
    break
  fi
  echo "    ($((i * 5))s) status: $STATUS"
  sleep 5
done

# ── Show recent logs ──────────────────────────────────────────────────────────
echo ""
echo "==> Container logs (last 40 lines):"
docker compose logs --tail=40 scholarship-service

# ── Health check ──────────────────────────────────────────────────────────────
echo ""
echo "==> Testing health endpoint..."
if command -v curl &> /dev/null; then
    curl -sf http://localhost:5166/health && echo "" || echo "Health check failed — service may still be starting."
else
    echo "curl not found, skipping health check."
fi

echo ""
echo "========================================="
echo "  Service URLs"
echo "========================================="
echo "  Swagger  : http://localhost:5166/swagger/index.html"
echo "  GraphQL  : http://localhost:5166/graphql"
echo "  Health   : http://localhost:5166/health"
echo "  RabbitMQ : http://localhost:15672  (guest/guest)"
echo "========================================="
echo ""
echo "Tip: use --fresh to wipe the database volume and re-run init-database.sql"
