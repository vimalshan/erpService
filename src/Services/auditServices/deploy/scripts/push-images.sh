#!/usr/bin/env bash
# =============================================================================
# push-images.sh — Tag and push all ERP images to a container registry
# Usage: ./push-images.sh REGISTRY [TAG]
# Example: ./push-images.sh myregistry.azurecr.io 1.2.0
# =============================================================================
set -euo pipefail

REGISTRY="${1:?Usage: $0 REGISTRY [TAG]}"
TAG="${2:-latest}"

IMAGES=(
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

echo "========================================="
echo " Pushing images to $REGISTRY  (tag: $TAG)"
echo "========================================="

for image in "${IMAGES[@]}"; do
  remote="$REGISTRY/$image:$TAG"
  echo ""
  echo ">>> Tagging $image:$TAG -> $remote"
  docker tag "$image:$TAG" "$remote"

  echo "    Pushing $remote ..."
  docker push "$remote"
  echo "    ✓ $remote"
done

echo ""
echo "========================================="
echo " All images pushed successfully."
echo "========================================="
