#!/bin/bash
# =============================================================================
# SCI ERP Microservices - Full Deployment Script
# End-to-end: build images, push to registry, deploy to Kubernetes
# =============================================================================

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
export DOCKER_REGISTRY="${DOCKER_REGISTRY:-sci-erp}"
export IMAGE_TAG="${IMAGE_TAG:-latest}"

echo "============================================="
echo "SCI ERP - Full Deployment Pipeline"
echo "Registry: $DOCKER_REGISTRY"
echo "Tag: $IMAGE_TAG"
echo "============================================="

# Parse arguments
SKIP_BUILD=false
SKIP_PUSH=false
SKIP_K8S=false
DOCKER_COMPOSE_ONLY=false

while [[ "$#" -gt 0 ]]; do
  case $1 in
    --skip-build) SKIP_BUILD=true ;;
    --skip-push) SKIP_PUSH=true ;;
    --skip-k8s) SKIP_K8S=true ;;
    --docker-compose) DOCKER_COMPOSE_ONLY=true ;;
    --registry) DOCKER_REGISTRY="$2"; shift ;;
    --tag) IMAGE_TAG="$2"; shift ;;
    -h|--help)
      echo "Usage: deploy.sh [OPTIONS]"
      echo ""
      echo "Options:"
      echo "  --skip-build       Skip Docker image build"
      echo "  --skip-push        Skip pushing images to registry"
      echo "  --skip-k8s         Skip Kubernetes deployment"
      echo "  --docker-compose   Deploy with docker-compose instead of K8s"
      echo "  --registry NAME    Docker registry (default: sci-erp)"
      echo "  --tag TAG          Image tag (default: latest)"
      echo "  -h, --help         Show this help message"
      exit 0
      ;;
    *) echo "Unknown parameter: $1"; exit 1 ;;
  esac
  shift
done

# Step 1: Build Docker images
if [ "$SKIP_BUILD" = false ]; then
  echo ""
  echo ">>> Step 1: Building Docker images..."
  bash "$SCRIPT_DIR/docker-build.sh"
else
  echo ""
  echo ">>> Step 1: Skipping Docker build"
fi

# Step 2: Push Docker images
if [ "$SKIP_PUSH" = false ] && [ "$DOCKER_COMPOSE_ONLY" = false ]; then
  echo ""
  echo ">>> Step 2: Pushing Docker images..."
  bash "$SCRIPT_DIR/docker-push.sh"
else
  echo ""
  echo ">>> Step 2: Skipping Docker push"
fi

# Step 3: Deploy
if [ "$DOCKER_COMPOSE_ONLY" = true ]; then
  echo ""
  echo ">>> Step 3: Starting with Docker Compose..."
  cd "$SCRIPT_DIR/.."
  docker compose up -d
  echo ""
  echo "Services starting... Check status with: docker compose ps"
elif [ "$SKIP_K8S" = false ]; then
  echo ""
  echo ">>> Step 3: Deploying to Kubernetes..."
  bash "$SCRIPT_DIR/k8s-deploy.sh"
else
  echo ""
  echo ">>> Step 3: Skipping deployment"
fi

echo ""
echo "============================================="
echo "Deployment pipeline complete!"
echo "============================================="
