#!/bin/bash
# ============================================
# Push Docker Images to Container Registry
# ============================================
set -e

REGISTRY="${REGISTRY:-pfservices}"
TAG="${TAG:-latest}"

echo "============================================"
echo "Pushing PF Services Docker Images"
echo "Registry: ${REGISTRY}"
echo "Tag: ${TAG}"
echo "============================================"

SERVICES=(
    "api-gateway"
    "accounting-service"
    "bank-service"
    "contribution-service"
    "investment-service"
    "loan-service"
    "masterdata-service"
    "member-service"
    "pftransactional-service"
    "settlement-service"
    "trust-service"
)

for name in "${SERVICES[@]}"; do
    echo "Pushing ${REGISTRY}/${name}:${TAG}..."
    docker push "${REGISTRY}/${name}:${TAG}"
done

echo ""
echo "All images pushed successfully."
