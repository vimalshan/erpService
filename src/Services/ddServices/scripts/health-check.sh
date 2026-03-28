#!/bin/bash
# ──────────────────────────────────────────────────────────────────────────────
# health-check.sh — Check health of all DD ERP services
# Usage: ./scripts/health-check.sh [--gateway-only]
# ──────────────────────────────────────────────────────────────────────────────

set -e

GATEWAY_URL="${GATEWAY_URL:-http://localhost:5200}"
GATEWAY_ONLY=false

for arg in "$@"; do
    case $arg in
        --gateway-only) GATEWAY_ONLY=true ;;
    esac
done

echo "============================================"
echo " DD ERP — Health Check Report"
echo " $(date '+%Y-%m-%d %H:%M:%S')"
echo "============================================"
echo ""

check_health() {
    local name=$1
    local url=$2
    local response
    local http_code

    http_code=$(curl -s -o /dev/null -w "%{http_code}" --connect-timeout 5 --max-time 10 "$url" 2>/dev/null || echo "000")

    if [ "$http_code" = "200" ]; then
        echo "  [✓] $name — Healthy ($url)"
    elif [ "$http_code" = "000" ]; then
        echo "  [✗] $name — Unreachable ($url)"
    else
        echo "  [!] $name — HTTP $http_code ($url)"
    fi
}

# ─── Gateway Health ─────────────────────────────────────────────────────────
echo "── API Gateway ──"
check_health "Gateway Status" "$GATEWAY_URL/gateway/status"
check_health "Gateway Health" "$GATEWAY_URL/health"
echo ""

if [ "$GATEWAY_ONLY" = true ]; then
    exit 0
fi

# ─── Individual Services ────────────────────────────────────────────────────
echo "── Microservices (Direct) ──"

declare -A SERVICES
SERVICES=(
    ["Appraisal"]="http://localhost:5100/health"
    ["Authorization"]="http://localhost:5177/health"
    ["Compensation"]="http://localhost:5000/health"
    ["Competency"]="http://localhost:5261/health"
    ["DemandManagement"]="http://localhost:5210/health"
    ["Document"]="http://localhost:5081/health"
    ["Employee"]="http://localhost:5049/health"
    ["Feedback"]="http://localhost:5101/health"
    ["Learning"]="http://localhost:5102/health"
    ["Objective"]="http://localhost:5258/health"
    ["Other"]="http://localhost:5224/health"
    ["Promotion"]="http://localhost:5103/health/live"
    ["Recruitment"]="http://localhost:5237/health"
    ["Reporting"]="http://localhost:5104/health"
    ["Transaction"]="http://localhost:5178/health"
)

for name in $(echo "${!SERVICES[@]}" | tr ' ' '\n' | sort); do
    check_health "$name" "${SERVICES[$name]}"
done

echo ""

# ─── Infrastructure ─────────────────────────────────────────────────────────
echo "── Infrastructure ──"
check_health "RabbitMQ Management" "http://localhost:15672"
echo ""

# ─── Gateway Downstream Report ──────────────────────────────────────────────
echo "── Gateway Downstream Health ──"
curl -s "$GATEWAY_URL/health/downstream" 2>/dev/null | \
    python3 -m json.tool 2>/dev/null || \
    curl -s "$GATEWAY_URL/health/downstream" 2>/dev/null || \
    echo "  [!] Could not fetch downstream health report"

echo ""
echo "============================================"
echo " Health check complete."
echo "============================================"
