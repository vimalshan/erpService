#!/bin/bash
# ─── Push all Docker images to registry ───────────────────────────────────
set -e

REGISTRY="${REGISTRY:?Set REGISTRY environment variable (e.g. myacr.azurecr.io)}"
TAG="${TAG:-latest}"

echo "============================================"
echo "Pushing SSC Service Images to $REGISTRY"
echo "============================================"

IMAGES=(
    "ssc-transactional"
    "batch-and-envelope"
    "category-and-vendor"
    "club-membership"
    "filing-and-archive"
    "hr-document"
    "integration-service"
    "invoice-processing"
    "master-data"
    "menu-and-security"
    "approval-group"
    "user-service"
    "ssc-api-gateway"
)

for IMAGE in "${IMAGES[@]}"; do
    echo "Pushing $REGISTRY/$IMAGE:$TAG ..."
    docker push "$REGISTRY/$IMAGE:$TAG"
    echo "✓ $IMAGE pushed"
done

echo ""
echo "All images pushed successfully!"
