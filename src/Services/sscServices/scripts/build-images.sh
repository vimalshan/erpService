#!/bin/bash
# ─── Build all Docker images for SSC Services ─────────────────────────────
set -e

REGISTRY="${REGISTRY:-ssc-services}"
TAG="${TAG:-latest}"

echo "============================================"
echo "Building SSC Service Docker Images"
echo "Registry: $REGISTRY  Tag: $TAG"
echo "============================================"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

declare -A SERVICES
SERVICES["ssc-transactional"]="ssctransactionalServices"
SERVICES["batch-and-envelope"]="batchandenvelopeServices"
SERVICES["category-and-vendor"]="categoryandvendorServices"
SERVICES["club-membership"]="clubmembershipServices"
SERVICES["filing-and-archive"]="fillingandarchiveServices"
SERVICES["hr-document"]="hrdocumentServices"
SERVICES["integration-service"]="integrationServices/IntegrationService"
SERVICES["invoice-processing"]="invoiceprocessingServices/InvoiceProcessing.Service"
SERVICES["master-data"]="masterdataServices/MasterDataService"
SERVICES["menu-and-security"]="menuandsecurityServices"
SERVICES["approval-group"]="approvalgroupServices"
SERVICES["user-service"]="menuServices/01_USER_MODULE"
SERVICES["ssc-api-gateway"]="apigateway"

FAILED=()

for SERVICE_NAME in "${!SERVICES[@]}"; do
    CONTEXT="${SERVICES[$SERVICE_NAME]}"
    echo ""
    echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
    echo "Building: $SERVICE_NAME"
    echo "Context:  $CONTEXT"
    echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

    if docker build \
        -t "$REGISTRY/$SERVICE_NAME:$TAG" \
        -f "$ROOT_DIR/$CONTEXT/Dockerfile" \
        "$ROOT_DIR/$CONTEXT"; then
        echo "✓ $SERVICE_NAME built successfully"
    else
        echo "✗ $SERVICE_NAME FAILED"
        FAILED+=("$SERVICE_NAME")
    fi
done

echo ""
echo "============================================"
echo "Build Summary"
echo "============================================"
echo "Total:  ${#SERVICES[@]}"
echo "Failed: ${#FAILED[@]}"

if [ ${#FAILED[@]} -gt 0 ]; then
    echo ""
    echo "Failed services:"
    for F in "${FAILED[@]}"; do
        echo "  - $F"
    done
    exit 1
fi

echo ""
echo "All images built successfully!"
