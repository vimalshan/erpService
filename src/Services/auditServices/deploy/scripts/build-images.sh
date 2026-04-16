#!/usr/bin/env bash
# =============================================================================
# build-images.sh — Build all ERP Docker images
# Usage: ./build-images.sh [TAG]
# Example: ./build-images.sh 1.2.0
# =============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

TAG="${1:-latest}"

SERVICES=(
  "apigateway:erp-api-gateway"
  "actionapiServices:erp-action-service"
  "auditapiServices:erp-audit-service"
  "certificateapiServices:erp-certificate-service"
  "contractapiServices:erp-contract-service"
  "financeapiServices:erp-finance-service"
  "findingsapiServices:erp-findings-service"
  "notificationapiServices:erp-notification-service"
  "scheduleapiServices:erp-schedule-service"
  "settingsapiServices:erp-settings-service"
)

echo "========================================="
echo " Building ERP images  (tag: $TAG)"
echo "========================================="

for entry in "${SERVICES[@]}"; do
  folder="${entry%%:*}"
  image="${entry##*:}"
  context="$ROOT_DIR/$folder"

  echo ""
  echo ">>> Building $image:$TAG from $folder ..."
  docker build \
    --build-arg BUILD_CONFIGURATION=Release \
    -t "$image:$TAG" \
    -t "$image:latest" \
    "$context"
  echo "    ✓ $image:$TAG"
done

echo ""
echo "========================================="
echo " All images built successfully."
echo "========================================="
