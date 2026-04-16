#!/usr/bin/env bash
# =============================================================================
# rollback.sh — Roll back a specific deployment or all deployments
# Usage: ./rollback.sh [DEPLOYMENT_NAME]
#   No argument → rolls back ALL deployments
#   With argument → rolls back just that deployment
# Example: ./rollback.sh erp-audit-service
# =============================================================================
set -euo pipefail

NAMESPACE="erp"

ALL_DEPLOYMENTS=(
  "erp-api-gateway"
  "erp-action-service"
  "erp-audit-service"
  "erp-certificate-service"
  "erp-contract-service"
  "erp-finance-service"
  "erp-findings-service"
  "erp-notification-service"
  "erp-schedule-service"
  "erp-settings-service"
)

rollback_one() {
  local dep="$1"
  echo ">>> Rolling back $dep ..."
  kubectl rollout undo "deployment/$dep" -n "$NAMESPACE"
  kubectl rollout status "deployment/$dep" -n "$NAMESPACE" --timeout=120s
  echo "    ✓ $dep rolled back"
}

if [[ -n "${1:-}" ]]; then
  rollback_one "$1"
else
  echo "========================================="
  echo " Rolling back ALL ERP deployments"
  echo "========================================="
  for dep in "${ALL_DEPLOYMENTS[@]}"; do
    rollback_one "$dep"
  done
  echo ""
  echo "========================================="
  echo " All rollbacks complete."
  echo "========================================="
  kubectl get pods -n "$NAMESPACE"
fi
