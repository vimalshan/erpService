#!/bin/bash
# ============================================================================
# SPARSH Platform - Docker Build & Push Script
# Usage: ./scripts/build-images.sh [--push] [--registry <registry>] [--tag <tag>]
# ============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

# Defaults
PUSH=false
REGISTRY="${DOCKER_REGISTRY:-sparsh}"
TAG="${IMAGE_TAG:-latest}"

# Parse arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --push) PUSH=true; shift ;;
        --registry) REGISTRY="$2"; shift 2 ;;
        --tag) TAG="$2"; shift 2 ;;
        *) echo "Unknown option: $1"; exit 1 ;;
    esac
done

echo "============================================"
echo "SPARSH Platform - Building Docker Images"
echo "Registry: $REGISTRY"
echo "Tag:      $TAG"
echo "Push:     $PUSH"
echo "============================================"

declare -A SERVICES=(
    ["sparsh-api-gateway"]="apigateway/SparshApiGateway"
    ["sparsh-employee-pride-api"]="employeepridemanagementServices"
    ["sparsh-mobile-app-api"]="mobileappmanagementServices"
    ["sparsh-mobile-expense-api"]="mobileexpenseServices"
    ["sparsh-problem-api"]="problemmanagementServices/ProblemManagement"
    ["sparsh-transactional-api"]="sparshtransactionalServices/SparshTransactional"
)

FAILED=()

for SERVICE_NAME in "${!SERVICES[@]}"; do
    CONTEXT_DIR="${SERVICES[$SERVICE_NAME]}"
    IMAGE_NAME="$REGISTRY/$SERVICE_NAME:$TAG"

    echo ""
    echo "--- Building $SERVICE_NAME ---"
    echo "Context: $ROOT_DIR/$CONTEXT_DIR"

    if docker build -t "$IMAGE_NAME" "$ROOT_DIR/$CONTEXT_DIR"; then
        echo "[OK] $SERVICE_NAME built successfully"
        
        if [ "$PUSH" = true ]; then
            echo "Pushing $IMAGE_NAME..."
            if docker push "$IMAGE_NAME"; then
                echo "[OK] $SERVICE_NAME pushed"
            else
                echo "[FAIL] Push failed for $SERVICE_NAME"
                FAILED+=("$SERVICE_NAME (push)")
            fi
        fi
    else
        echo "[FAIL] Build failed for $SERVICE_NAME"
        FAILED+=("$SERVICE_NAME (build)")
    fi
done

echo ""
echo "============================================"
if [ ${#FAILED[@]} -eq 0 ]; then
    echo "All images built successfully!"
else
    echo "FAILURES:"
    for f in "${FAILED[@]}"; do echo "  - $f"; done
    exit 1
fi
