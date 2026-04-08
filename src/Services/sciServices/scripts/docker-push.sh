#!/bin/bash
# =============================================================================
# SCI ERP Microservices - Docker Push Script
# Pushes all 16 service Docker images to registry
# =============================================================================

set -e

REGISTRY="${DOCKER_REGISTRY:-sci-erp}"
TAG="${IMAGE_TAG:-latest}"

echo "============================================="
echo "SCI ERP - Pushing Docker Images"
echo "Registry: $REGISTRY"
echo "Tag: $TAG"
echo "============================================="

IMAGES=(
  "api-gateway"
  "security-service"
  "vehicle-tracking"
  "dispatch-planning"
  "order-schedule"
  "filling-operation"
  "exim-management"
  "gst-compliance"
  "inventory-management"
  "production-management"
  "mam-allocation"
  "purchase-sales"
  "master-data"
  "strategic-stock"
  "error-logging"
  "sci-transactional"
)

for IMAGE in "${IMAGES[@]}"; do
  echo "Pushing $REGISTRY/$IMAGE:$TAG ..."
  docker push "$REGISTRY/$IMAGE:$TAG"
  echo "  Done."
done

echo ""
echo "All images pushed successfully!"
