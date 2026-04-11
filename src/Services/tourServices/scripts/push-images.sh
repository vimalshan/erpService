#!/bin/bash
# =============================================================================
# Tour ERP - Push Docker Images to Registry
# =============================================================================
set -e

REGISTRY="${DOCKER_REGISTRY:-tour-erp}"
TAG="${IMAGE_TAG:-latest}"

echo "=========================================="
echo "  Tour ERP - Pushing Docker Images"
echo "  Registry: $REGISTRY"
echo "  Tag: $TAG"
echo "=========================================="

SERVICES=(
  "admin-service"
  "booking-service"
  "config-service"
  "tourplan-service"
  "tour-service"
  "transaction-service"
  "travel-service"
  "api-gateway"
)

for SERVICE in "${SERVICES[@]}"; do
  echo ""
  echo "Pushing $REGISTRY/$SERVICE:$TAG ..."
  docker push "$REGISTRY/$SERVICE:$TAG"
done

echo ""
echo "=========================================="
echo "  All images pushed successfully!"
echo "=========================================="
