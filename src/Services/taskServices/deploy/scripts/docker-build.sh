#!/bin/bash
set -euo pipefail

###############################################################################
# docker-build.sh — Build all ERP Microservice Docker images
###############################################################################

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/../.." && pwd)"

# Load .env if present
if [ -f "$SCRIPT_DIR/../.env" ]; then
    export $(grep -v '^#' "$SCRIPT_DIR/../.env" | xargs)
fi

REGISTRY="${REGISTRY:-}"
TAG="${TAG:-latest}"

# Prefix with registry if set
prefix() {
    if [ -n "$REGISTRY" ]; then
        echo "${REGISTRY}/$1:${TAG}"
    else
        echo "$1:${TAG}"
    fi
}

echo "============================================"
echo "  Building ERP Microservice Docker Images"
echo "  Tag: ${TAG}"
[ -n "$REGISTRY" ] && echo "  Registry: ${REGISTRY}"
echo "============================================"

build_image() {
    local context="$1"
    local dockerfile="$2"
    local name="$3"
    local image
    image=$(prefix "$name")

    echo ""
    echo "--- Building ${name} ---"
    docker build -t "$image" -f "$context/$dockerfile" "$context"
    echo "    ✓ ${image}"
}

# API Gateway
build_image "$ROOT_DIR/apiGateway" "Dockerfile" "erp-api-gateway"

# Lookup Service
build_image "$ROOT_DIR/lookupServices" "Dockerfile" "erp-lookup-api"
build_image "$ROOT_DIR/lookupServices" "Dockerfile.functions" "erp-lookup-functions"

# Task Services
build_image "$ROOT_DIR/taskServices" "Dockerfile" "erp-task-api"
build_image "$ROOT_DIR/taskServices" "Dockerfile.functions" "erp-task-functions"

# Task Transactional
build_image "$ROOT_DIR/taskTransactionalServices" "Dockerfile" "erp-transactional-api"
build_image "$ROOT_DIR/taskTransactionalServices" "Dockerfile.functions" "erp-transactional-functions"

# Complaint Service
build_image "$ROOT_DIR/complaintServices" "Dockerfile" "erp-complaint-api"
build_image "$ROOT_DIR/complaintServices" "Dockerfile.functions" "erp-complaint-functions"

# Energy Service
build_image "$ROOT_DIR/energyServices" "Dockerfile" "erp-energy-api"
build_image "$ROOT_DIR/energyServices" "Dockerfile.functions" "erp-energy-functions"

# Unit Service
build_image "$ROOT_DIR/unitServices" "Dockerfile" "erp-unit-api"
build_image "$ROOT_DIR/unitServices" "Dockerfile.functions" "erp-unit-functions"

echo ""
echo "============================================"
echo "  All 13 images built successfully"
echo "============================================"

# Push if registry is set
if [ -n "$REGISTRY" ]; then
    echo ""
    read -rp "Push all images to ${REGISTRY}? [y/N] " answer
    if [[ "$answer" =~ ^[Yy]$ ]]; then
        echo "Pushing images..."
        for img in \
            erp-api-gateway \
            erp-lookup-api erp-lookup-functions \
            erp-task-api erp-task-functions \
            erp-transactional-api erp-transactional-functions \
            erp-complaint-api erp-complaint-functions \
            erp-energy-api erp-energy-functions \
            erp-unit-api erp-unit-functions; do
            docker push "$(prefix "$img")"
            echo "    ✓ pushed $(prefix "$img")"
        done
        echo "All images pushed."
    fi
fi
