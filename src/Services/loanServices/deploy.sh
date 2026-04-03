#!/usr/bin/env bash
# =============================================================================
# deploy.sh — Build, tag, and run all Loan ERP services via Docker Compose
# Usage:
#   ./deploy.sh [up|build|down|restart|logs|status]
#   REGISTRY=myregistry.io/loan ./deploy.sh build  # push to remote registry
# =============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

REGISTRY="${REGISTRY:-}"
TAG="${TAG:-latest}"

# ── Colour helpers ────────────────────────────────────────────────────────────
GREEN='\033[0;32m'; YELLOW='\033[1;33m'; RED='\033[0;31m'; NC='\033[0m'
info()    { echo -e "${GREEN}[INFO]${NC}  $*"; }
warn()    { echo -e "${YELLOW}[WARN]${NC}  $*"; }
error()   { echo -e "${RED}[ERROR]${NC} $*"; exit 1; }

# ── Services to build ─────────────────────────────────────────────────────────
declare -A SERVICES=(
  ["loan-transaction"]="loanTransactionServices"
  ["loan-application"]="loanapplicationServices"
  ["loan-account"]="loanaccountServices"
  ["loan-definition"]="loandefinitionServices"
  ["document-service"]="documentServices"
  ["lov-service"]="lovServices"
  ["utility-service"]="utilityServices"
  ["api-gateway"]="apiGateway"
)

cmd_build() {
  info "Building all service images..."
  docker compose build --parallel
  info "Build complete."

  if [[ -n "$REGISTRY" ]]; then
    info "Tagging and pushing to $REGISTRY ..."
    for name in "${!SERVICES[@]}"; do
      local_image="${name}:${TAG}"
      remote_image="${REGISTRY}/${name}:${TAG}"
      docker tag "$local_image" "$remote_image"
      docker push "$remote_image"
      info "  Pushed $remote_image"
    done
  fi
}

cmd_up() {
  if [[ ! -f .env ]]; then
    warn ".env not found — copying .env.example → .env"
    cp .env.example .env
  fi
  info "Starting all services..."
  docker compose up -d
  info "Waiting for health checks..."
  sleep 15
  cmd_status
}

cmd_down() {
  info "Stopping all services..."
  docker compose down
}

cmd_restart() {
  cmd_down
  cmd_up
}

cmd_logs() {
  local service="${2:-}"
  if [[ -n "$service" ]]; then
    docker compose logs -f "$service"
  else
    docker compose logs -f
  fi
}

cmd_status() {
  echo ""
  info "Container status:"
  docker compose ps
  echo ""
  info "Health endpoints:"
  declare -A PORTS=(
    ["LoanTransaction"]="5292"
    ["LoanApplication"]="5282"
    ["LoanAccount"]="5150"
    ["LoanDefinition"]="5077"
    ["DocumentService"]="5280"
    ["LovService"]="5008"
    ["UtilityService"]="5143"
    ["ApiGateway"]="6100"
  )
  for svc in "${!PORTS[@]}"; do
    port="${PORTS[$svc]}"
    if curl -sf "http://localhost:${port}/health" -o /dev/null 2>&1; then
      echo -e "  ${GREEN}✓${NC} ${svc} :${port}"
    else
      echo -e "  ${RED}✗${NC} ${svc} :${port}"
    fi
  done
}

CMD="${1:-up}"
case "$CMD" in
  build)   cmd_build ;;
  up)      cmd_build && cmd_up ;;
  down)    cmd_down ;;
  restart) cmd_restart ;;
  logs)    cmd_logs "$@" ;;
  status)  cmd_status ;;
  *)       error "Unknown command: $CMD. Use: build|up|down|restart|logs|status" ;;
esac
