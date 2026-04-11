#!/bin/bash
# =============================================================================
# Tour ERP - Docker Image Build Script
# =============================================================================
set -e

REGISTRY="${DOCKER_REGISTRY:-tour-erp}"
TAG="${IMAGE_TAG:-latest}"

echo "=========================================="
echo "  Tour ERP - Building Docker Images"
echo "  Registry: $REGISTRY"
echo "  Tag: $TAG"
echo "=========================================="

cd "$(dirname "$0")/.."

echo ""
echo "[1/8] Building Admin Service..."
docker build -t "$REGISTRY/admin-service:$TAG" -f adminServices/Dockerfile adminServices/

echo ""
echo "[2/8] Building Booking Service..."
docker build -t "$REGISTRY/booking-service:$TAG" -f bookingServices/BookingService/Dockerfile bookingServices/BookingService/

echo ""
echo "[3/8] Building Config Service..."
docker build -t "$REGISTRY/config-service:$TAG" -f configServices/Dockerfile configServices/

echo ""
echo "[4/8] Building TourPlan Service..."
docker build -t "$REGISTRY/tourplan-service:$TAG" -f tourplanServices/Dockerfile tourplanServices/

echo ""
echo "[5/8] Building Tour Service..."
docker build -t "$REGISTRY/tour-service:$TAG" -f tourServices/Dockerfile tourServices/

echo ""
echo "[6/8] Building Transaction Service..."
docker build -t "$REGISTRY/transaction-service:$TAG" -f transactionServices/Dockerfile transactionServices/

echo ""
echo "[7/8] Building Travel Service..."
docker build -t "$REGISTRY/travel-service:$TAG" -f travelServices/Dockerfile travelServices/

echo ""
echo "[8/8] Building API Gateway..."
docker build -t "$REGISTRY/api-gateway:$TAG" -f apiGateway/Dockerfile apiGateway/

echo ""
echo "=========================================="
echo "  All images built successfully!"
echo "=========================================="
docker images | grep "$REGISTRY"
