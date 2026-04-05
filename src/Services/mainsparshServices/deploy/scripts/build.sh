#!/bin/bash
# ==========================================
# SRF Sparsh Microservices - Build Script
# Builds all Docker images
# ==========================================

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
IMAGE_TAG="${IMAGE_TAG:-latest}"
REGISTRY="${REGISTRY:-srfsparsh}"

echo "============================================"
echo " SRF Sparsh - Building Docker Images"
echo " Tag: $IMAGE_TAG"
echo "============================================"

declare -A SERVICES=(
    ["api-gateway"]="apiGateway"
    ["approval-service"]="approvalServices"
    ["booking-service"]="bookingServices"
    ["community-service"]="communityServices"
    ["compensation-service"]="compensationServices"
    ["groupmanagement-service"]="groupmanagementServices"
    ["location-service"]="locationServices"
    ["meeting-service"]="meetingServices"
    ["proxy-service"]="proxyServices"
    ["reimbursement-service"]="reimbursementServices"
    ["stipend-service"]="stipendservices"
    ["timesheet-service"]="timesheetServices"
    ["transaction-service"]="transactionServices"
    ["usermanagement-service"]="usermanagementServices"
    ["websitecontent-service"]="websitecontentServices"
)

FAILED=0
for SERVICE in "${!SERVICES[@]}"; do
    CONTEXT="${SERVICES[$SERVICE]}"
    echo ""
    echo "--- Building $SERVICE from $CONTEXT ---"
    if docker build -t "$REGISTRY/$SERVICE:$IMAGE_TAG" "$ROOT_DIR/$CONTEXT"; then
        echo "[OK] $SERVICE built successfully"
    else
        echo "[FAIL] $SERVICE build failed!"
        FAILED=$((FAILED + 1))
    fi
done

echo ""
echo "============================================"
if [ $FAILED -eq 0 ]; then
    echo " All ${#SERVICES[@]} images built successfully!"
else
    echo " $FAILED of ${#SERVICES[@]} images failed to build."
    exit 1
fi
echo "============================================"
