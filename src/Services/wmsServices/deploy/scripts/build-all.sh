#!/bin/bash
# =============================================================================
# WMS Microservices - Build All Docker Images
# =============================================================================
set -e

REGISTRY="${REGISTRY:-wms}"
TAG="${IMAGE_TAG:-latest}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/../.." && pwd)"

echo "=============================================="
echo " WMS Microservices - Docker Image Build"
echo " Registry: $REGISTRY"
echo " Tag:      $TAG"
echo "=============================================="

cd "$ROOT_DIR"

SERVICES=(
  "security-service:securityService"
  "warehouse-service:warehousestructureService"
  "racking-service:rackingsystemService"
  "employee-service:emplyeeService"
  "product-service:productService"
  "inventory-service:inventoryService"
  "supplier-service:supplierService"
  "customer-service:customerService"
  "purchaseorder-service:purchaseorderService"
  "receiving-service:receivingService"
  "salesorder-service:salesorderService"
  "shipment-service:shipmentService"
  "order-service:orderService"
  "fleet-service:fleetManagementService"
  "auditlog-service:auditlogService/AuditLogService"
  "transactional-service:wmtransactionalService"
  "api-gateway:apiGateway"
)

FAILED=()
SUCCEEDED=()

for entry in "${SERVICES[@]}"; do
  IFS=':' read -r name context <<< "$entry"
  IMAGE="$REGISTRY/$name:$TAG"
  echo ""
  echo "----------------------------------------------"
  echo " Building: $IMAGE"
  echo " Context:  ./$context"
  echo "----------------------------------------------"

  if docker build -t "$IMAGE" -f "./$context/Dockerfile" "./$context"; then
    SUCCEEDED+=("$name")
    echo " ✓ $name built successfully"
  else
    FAILED+=("$name")
    echo " ✗ $name FAILED"
  fi
done

echo ""
echo "=============================================="
echo " Build Summary"
echo "=============================================="
echo " Succeeded: ${#SUCCEEDED[@]}/${#SERVICES[@]}"
for s in "${SUCCEEDED[@]}"; do echo "   ✓ $s"; done

if [ ${#FAILED[@]} -gt 0 ]; then
  echo " Failed: ${#FAILED[@]}/${#SERVICES[@]}"
  for f in "${FAILED[@]}"; do echo "   ✗ $f"; done
  exit 1
fi

echo ""
echo " All images built successfully!"
echo "=============================================="
