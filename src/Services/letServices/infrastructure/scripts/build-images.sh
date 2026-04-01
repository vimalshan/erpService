#!/bin/bash
# ═══════════════════════════════════════════════════════════════════════
# build-images.sh — Build and optionally push all Docker images
# Usage: ./build-images.sh [--push] [--registry <registry>] [--tag <tag>]
# ═══════════════════════════════════════════════════════════════════════
set -euo pipefail

REGISTRY="${REGISTRY:-letregistry}"
TAG="${IMAGE_TAG:-latest}"
PUSH=false
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/../.." && pwd)"

while [[ $# -gt 0 ]]; do
    case $1 in
        --push)    PUSH=true; shift ;;
        --registry) REGISTRY="$2"; shift 2 ;;
        --tag)     TAG="$2"; shift 2 ;;
        *)         echo "Unknown option: $1"; exit 1 ;;
    esac
done

SERVICES=(
    "api-gateway:apiGateway"
    "leave-service:leaveServices"
    "course-service:courseServices"
    "request-service:requestServices"
    "review-service:reviewServices"
    "development-service:developmentServices"
    "master-service:masterServices"
    "let-transaction-service:letTransactionServices"
)

echo "═══════════════════════════════════════════════════════════════"
echo "  Building LET ERP Docker Images"
echo "  Registry: $REGISTRY    Tag: $TAG"
echo "═══════════════════════════════════════════════════════════════"

FAILED=()
for entry in "${SERVICES[@]}"; do
    IFS=':' read -r name context <<< "$entry"
    IMAGE="$REGISTRY/$name:$TAG"
    echo ""
    echo "─── Building $IMAGE ───"
    if docker build -t "$IMAGE" "$ROOT_DIR/$context"; then
        echo "  ✓ $name built successfully"
        if [ "$PUSH" = true ]; then
            echo "  Pushing $IMAGE..."
            docker push "$IMAGE"
            echo "  ✓ $name pushed"
        fi
    else
        echo "  ✗ $name FAILED"
        FAILED+=("$name")
    fi
done

echo ""
echo "═══════════════════════════════════════════════════════════════"
if [ ${#FAILED[@]} -eq 0 ]; then
    echo "  All images built successfully."
else
    echo "  FAILED builds: ${FAILED[*]}"
    exit 1
fi
echo "═══════════════════════════════════════════════════════════════"
