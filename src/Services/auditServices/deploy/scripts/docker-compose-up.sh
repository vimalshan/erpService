#!/usr/bin/env bash
# =============================================================================
# docker-compose-up.sh — Start the full stack with docker-compose
# Usage: ./docker-compose-up.sh [--build]
# =============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

cd "$ROOT_DIR"

# Load .env if present
if [[ -f ".env" ]]; then
  set -a
  # shellcheck disable=SC1091
  source .env
  set +a
  echo "Loaded .env"
else
  echo "WARNING: .env not found. Using defaults from docker-compose.yml."
  echo "         Copy .env.example to .env and fill in secrets."
fi

BUILD_FLAG=""
if [[ "${1:-}" == "--build" ]]; then
  BUILD_FLAG="--build"
fi

echo "Starting ERP Microservices stack ..."
docker compose up $BUILD_FLAG -d

echo ""
echo "Services:"
docker compose ps

echo ""
echo "Gateway:       http://localhost:5000"
echo "RabbitMQ UI:   http://localhost:15672"
echo "Seq Logs:      http://localhost:8888"
