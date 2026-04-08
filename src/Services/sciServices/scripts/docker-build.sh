#!/bin/bash
# =============================================================================
# SCI ERP Microservices - Docker Build Script
# Builds all 16 service Docker images
# =============================================================================

set -e

REGISTRY="${DOCKER_REGISTRY:-sci-erp}"
TAG="${IMAGE_TAG:-latest}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "============================================="
echo "SCI ERP - Building Docker Images"
echo "Registry: $REGISTRY"
echo "Tag: $TAG"
echo "============================================="

SERVICES=(
  "api-gateway:ApiGateway"
  "security-service:SecurityServices"
  "vehicle-tracking:vechicletrackingServices"
  "dispatch-planning:dispatchplanningServices"
  "order-schedule:orderscheduleServices"
  "filling-operation:fillingoperationServices"
  "exim-management:eximmanagementServices"
  "gst-compliance:gstcomplianceServices"
  "inventory-management:inventorymanagementServices"
  "production-management:productionmanagementServices"
  "mam-allocation:mamallocationServices"
  "purchase-sales:purchasesalesService"
  "master-data:masterdataServices"
  "strategic-stock:strategicstockServices"
  "error-logging:errorloggingServices"
  "sci-transactional:scitransactionalServices"
)

FAILED=()
SUCCEEDED=()

for SERVICE_ENTRY in "${SERVICES[@]}"; do
  IFS=':' read -r IMAGE_NAME BUILD_CONTEXT <<< "$SERVICE_ENTRY"
  
  echo ""
  echo "---------------------------------------------"
  echo "Building: $REGISTRY/$IMAGE_NAME:$TAG"
  echo "Context:  $SCRIPT_DIR/$BUILD_CONTEXT"
  echo "---------------------------------------------"
  
  if docker build \
    -t "$REGISTRY/$IMAGE_NAME:$TAG" \
    -f "$SCRIPT_DIR/$BUILD_CONTEXT/Dockerfile" \
    "$SCRIPT_DIR/$BUILD_CONTEXT"; then
    SUCCEEDED+=("$IMAGE_NAME")
    echo "SUCCESS: $IMAGE_NAME"
  else
    FAILED+=("$IMAGE_NAME")
    echo "FAILED: $IMAGE_NAME"
  fi
done

echo ""
echo "============================================="
echo "Build Summary"
echo "============================================="
echo "Succeeded: ${#SUCCEEDED[@]}/${#SERVICES[@]}"
for s in "${SUCCEEDED[@]}"; do echo "  ✓ $s"; done

if [ ${#FAILED[@]} -gt 0 ]; then
  echo ""
  echo "Failed: ${#FAILED[@]}/${#SERVICES[@]}"
  for f in "${FAILED[@]}"; do echo "  ✗ $f"; done
  exit 1
fi

echo ""
echo "All images built successfully!"
