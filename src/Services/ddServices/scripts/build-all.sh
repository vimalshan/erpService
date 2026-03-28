#!/bin/bash
# ──────────────────────────────────────────────────────────────────────────────
# build-all.sh — Build all DD ERP Docker images
# Usage: ./scripts/build-all.sh [--no-cache] [--parallel]
# ──────────────────────────────────────────────────────────────────────────────

set -e
cd "$(dirname "$0")/.."

REGISTRY="${DOCKER_REGISTRY:-dderp}"
TAG="${IMAGE_TAG:-latest}"
NO_CACHE=""
PARALLEL=false

for arg in "$@"; do
    case $arg in
        --no-cache) NO_CACHE="--no-cache" ;;
        --parallel) PARALLEL=true ;;
    esac
done

echo "============================================"
echo " DD ERP — Building All Docker Images"
echo " Registry: $REGISTRY | Tag: $TAG"
echo "============================================"

declare -A SERVICES
SERVICES=(
    ["api-gateway"]="apiGateway"
    ["appraisal-service"]="appraisalService"
    ["authorization-service"]="authorizationServices"
    ["compensation-service"]="compensationServices"
    ["competency-service"]="competencyServices"
    ["demandmanagement-service"]="demandmanagementServices"
    ["document-service"]="documentServices"
    ["employee-service"]="employeeServices"
    ["feedback-service"]="feedbackServices"
    ["learning-service"]="learningServices"
    ["objective-service"]="objectiveServices"
    ["other-service"]="OtherServices"
    ["promotion-service"]="promotionServices"
    ["recruitment-service"]="recruitmentServices"
    ["reporting-service"]="reportingServices"
    ["transaction-service"]="transactionServices"
)

build_service() {
    local name=$1
    local dir=$2
    echo ""
    echo "─── Building $name ───"
    docker build $NO_CACHE -t "$REGISTRY/$name:$TAG" -f "$dir/Dockerfile" "$dir"
    echo "[✓] $name built successfully"
}

if [ "$PARALLEL" = true ]; then
    echo "[*] Building in parallel..."
    pids=()
    for name in "${!SERVICES[@]}"; do
        build_service "$name" "${SERVICES[$name]}" &
        pids+=($!)
    done
    for pid in "${pids[@]}"; do
        wait $pid || { echo "[✗] A build failed!"; exit 1; }
    done
else
    for name in "${!SERVICES[@]}"; do
        build_service "$name" "${SERVICES[$name]}"
    done
fi

echo ""
echo "============================================"
echo " [✓] All images built successfully!"
echo "============================================"
docker images | grep "$REGISTRY"
