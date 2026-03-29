#!/bin/bash
# ================================================
# Health ERP - Kubernetes Deployment Script
# ================================================
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K8S_DIR="$(dirname "$SCRIPT_DIR")/k8s"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

log_info()  { echo -e "${GREEN}[INFO]${NC} $1"; }
log_warn()  { echo -e "${YELLOW}[WARN]${NC} $1"; }
log_error() { echo -e "${RED}[ERROR]${NC} $1"; }

# ------ Pre-checks ------
command -v kubectl >/dev/null 2>&1 || { log_error "kubectl is not installed"; exit 1; }

REGISTRY="${REGISTRY:-myregistry.azurecr.io}"
TAG="${TAG:-latest}"
ACTION="${1:-apply}"

# ------ Build & Push Images ------
build_images() {
    log_info "Building and pushing Docker images to $REGISTRY..."
    ROOT_DIR="$(dirname "$(dirname "$SCRIPT_DIR")")"

    declare -A SERVICES=(
        ["accident-service"]="accidentmanagementServices/src"
        ["checkup-service"]="healthcheckupServices/src"
        ["insurance-service"]="insurancemanagementServices/src"
        ["masters-service"]="masterServices/src"
        ["medicalvisit-service"]="medicalvisitServices/src"
        ["medicine-service"]="medicinemanagementServices/src"
        ["transaction-service"]="healthTransactionServices/src"
        ["health-gateway"]="apiGateway/src"
    )

    for SVC in "${!SERVICES[@]}"; do
        CONTEXT="${ROOT_DIR}/${SERVICES[$SVC]}"
        log_info "Building $SVC from $CONTEXT..."
        docker build -t "$REGISTRY/$SVC:$TAG" "$CONTEXT"
        docker push "$REGISTRY/$SVC:$TAG"
    done
    log_info "All images built and pushed."
}

# ------ Update image references in manifests ------
update_manifests() {
    log_info "Updating image references in K8s manifests..."
    if [[ "$OSTYPE" == "darwin"* ]]; then
        sed -i '' "s|\${REGISTRY}|$REGISTRY|g" "$K8S_DIR/services.yaml"
    else
        sed -i "s|\${REGISTRY}|$REGISTRY|g" "$K8S_DIR/services.yaml"
    fi
}

# ------ Deploy ------
deploy() {
    log_info "Deploying to Kubernetes..."

    log_info "Creating namespace..."
    kubectl apply -f "$K8S_DIR/namespace.yaml"

    log_info "Applying secrets and config..."
    kubectl apply -f "$K8S_DIR/secrets.yaml"
    kubectl apply -f "$K8S_DIR/configmap.yaml"

    log_info "Deploying infrastructure (SQL Server, RabbitMQ, Redis)..."
    kubectl apply -f "$K8S_DIR/infrastructure.yaml"

    log_info "Waiting for infrastructure to be ready..."
    kubectl -n health-services wait --for=condition=ready pod -l app=sqlserver --timeout=120s 2>/dev/null || log_warn "SQL Server not ready yet, continuing..."
    kubectl -n health-services wait --for=condition=ready pod -l app=rabbitmq --timeout=120s 2>/dev/null || log_warn "RabbitMQ not ready yet, continuing..."
    kubectl -n health-services wait --for=condition=ready pod -l app=redis --timeout=60s 2>/dev/null || log_warn "Redis not ready yet, continuing..."

    log_info "Deploying microservices..."
    kubectl apply -f "$K8S_DIR/services.yaml"

    log_info "Applying ingress rules..."
    kubectl apply -f "$K8S_DIR/ingress.yaml"

    log_info "Deployment complete. Checking status..."
    kubectl -n health-services get pods
}

# ------ Delete ------
delete() {
    log_warn "This will delete all Health ERP resources from Kubernetes!"
    read -p "Are you sure? (y/N) " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        kubectl delete -f "$K8S_DIR/ingress.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/services.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/infrastructure.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/configmap.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/secrets.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/namespace.yaml" --ignore-not-found
        log_info "All resources deleted."
    else
        log_info "Delete cancelled."
    fi
}

# ------ Status ------
status() {
    log_info "Health ERP Kubernetes Status:"
    echo ""
    echo "=== Pods ==="
    kubectl -n health-services get pods -o wide 2>/dev/null || log_warn "Namespace not found"
    echo ""
    echo "=== Services ==="
    kubectl -n health-services get svc 2>/dev/null || true
    echo ""
    echo "=== Ingress ==="
    kubectl -n health-services get ingress 2>/dev/null || true
}

case "$ACTION" in
    build)
        build_images
        ;;
    apply|deploy)
        deploy
        ;;
    build-deploy)
        build_images
        update_manifests
        deploy
        ;;
    delete|destroy)
        delete
        ;;
    status)
        status
        ;;
    *)
        echo "Usage: $0 {build|apply|build-deploy|delete|status}"
        echo ""
        echo "Commands:"
        echo "  build         - Build and push Docker images"
        echo "  apply/deploy  - Deploy K8s manifests"
        echo "  build-deploy  - Build images and deploy"
        echo "  delete        - Remove all K8s resources"
        echo "  status        - Show deployment status"
        echo ""
        echo "Environment variables:"
        echo "  REGISTRY  - Container registry (default: myregistry.azurecr.io)"
        echo "  TAG        - Image tag (default: latest)"
        exit 1
        ;;
esac
