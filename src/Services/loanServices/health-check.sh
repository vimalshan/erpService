#!/usr/bin/env bash
# =============================================================================
# health-check.sh — Verify all Loan ERP service health endpoints
# Usage:
#   ./health-check.sh              # check local ports (docker compose)
#   BASE_URL=http://loans.example.com ./health-check.sh  # check via ingress
# =============================================================================
set -euo pipefail

BASE_URL="${BASE_URL:-}"

GREEN='\033[0;32m'; RED='\033[0;31m'; YELLOW='\033[1;33m'; NC='\033[0m'

declare -A SERVICES=(
  ["LoanTransaction"]="5292:/health"
  ["LoanApplication"]="5282:/health"
  ["LoanAccount"]="5150:/health"
  ["LoanDefinition"]="5077:/health"
  ["DocumentService"]="5280:/health"
  ["LovService"]="5008:/health"
  ["UtilityService"]="5143:/health"
  ["ApiGateway"]="6100:/health"
)

PASS=0; FAIL=0

for svc in "${!SERVICES[@]}"; do
  IFS=':' read -r port path <<< "${SERVICES[$svc]}"

  if [[ -n "$BASE_URL" ]]; then
    url="${BASE_URL}${path}"
  else
    url="http://localhost:${port}${path}"
  fi

  http_code=$(curl -s -o /dev/null -w "%{http_code}" --connect-timeout 5 --max-time 10 "$url" 2>/dev/null || echo "000")

  if [[ "$http_code" == "200" ]]; then
    echo -e "  ${GREEN}✓ HEALTHY${NC}   ${svc} → ${url} (HTTP ${http_code})"
    ((PASS++))
  else
    echo -e "  ${RED}✗ UNHEALTHY${NC} ${svc} → ${url} (HTTP ${http_code})"
    ((FAIL++))
  fi
done

echo ""
echo -e "Results: ${GREEN}${PASS} healthy${NC} / ${RED}${FAIL} unhealthy${NC}"

if [[ "$FAIL" -gt 0 ]]; then
  echo -e "${YELLOW}[WARN]${NC} Some services are unhealthy. Check logs: docker compose logs <service>"
  exit 1
fi
