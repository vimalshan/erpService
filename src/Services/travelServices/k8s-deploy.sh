#!/bin/bash
# ==============================================================================
# ERP Travel Services - Kubernetes Deployment Script
# ==============================================================================
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K8S_DIR="$SCRIPT_DIR/k8s"
REGISTRY="${REGISTRY:-erptravelservices.azurecr.io}"
IMAGE_TAG="${IMAGE_TAG:-latest}"
NAMESPACE="erp-travel"

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m'

print_header() {
    echo -e "\n${CYAN}============================================${NC}"
    echo -e "${CYAN}  $1${NC}"
    echo -e "${CYAN}============================================${NC}\n"
}

print_step() { echo -e "${GREEN}[$(date +%H:%M:%S)]${NC} $1"; }
print_warn() { echo -e "${YELLOW}[WARNING]${NC} $1"; }
print_error() { echo -e "${RED}[ERROR]${NC} $1"; }

usage() {
    echo "Usage: $0 [COMMAND]"
    echo ""
    echo "Commands:"
    echo "  build       Build and push all Docker images"
    echo "  deploy      Deploy all services to Kubernetes"
    echo "  delete      Delete all Kubernetes resources"
    echo "  status      Show deployment status"
    echo "  logs        Show logs for a service"
    echo "  rollback    Rollback a deployment"
    echo ""
    echo "Environment Variables:"
    echo "  REGISTRY    Container registry (default: erptravelservices.azurecr.io)"
    echo "  IMAGE_TAG   Image tag (default: latest)"
    echo ""
    exit 1
}

SERVICES=(
    "api-gateway:ApiGateway"
    "travel-request-api:travelRequestServices"
    "travel-transaction-api:traveltransactionServices"
    "booking-api:bookingServices"
    "expense-api:expenseServices"
    "finance-api:financeServices"
    "insurance-api:insuranceServices"
    "masterdata-api:masterdataServices"
    "agency-api:agensService"
    "admin-api:adminServices"
)

build_images() {
    print_header "Building & Pushing Docker Images"

    for svc in "${SERVICES[@]}"; do
        NAME="${svc%%:*}"
        CONTEXT="${svc##*:}"
        IMAGE="${REGISTRY}/${NAME}:${IMAGE_TAG}"

        print_step "Building ${NAME}..."
        
        DOCKERFILE="Dockerfile"
        if [ "$NAME" = "agency-api" ]; then
            DOCKERFILE="Dockerfile.production"
        fi

        docker build -t "$IMAGE" -f "${CONTEXT}/${DOCKERFILE}" "${CONTEXT}/"
        
        print_step "Pushing ${IMAGE}..."
        docker push "$IMAGE"
    done

    print_step "All images built and pushed"
}

deploy() {
    print_header "Deploying to Kubernetes"

    # Check kubectl
    if ! command -v kubectl &> /dev/null; then
        print_error "kubectl is not installed"
        exit 1
    fi

    # Apply namespace
    print_step "Creating namespace..."
    kubectl apply -f "$K8S_DIR/namespace.yaml"

    # Apply secrets and config
    print_step "Applying secrets and config..."
    kubectl apply -f "$K8S_DIR/secrets.yaml"
    kubectl apply -f "$K8S_DIR/configmap.yaml"

    # Deploy infrastructure
    print_step "Deploying infrastructure (SQL Server, RabbitMQ)..."
    kubectl apply -f "$K8S_DIR/infrastructure.yaml"

    # Wait for infrastructure
    print_step "Waiting for SQL Server to be ready..."
    kubectl wait --for=condition=ready pod -l app=sqlserver -n "$NAMESPACE" --timeout=120s || {
        print_warn "SQL Server may not be ready yet, continuing..."
    }

    print_step "Waiting for RabbitMQ to be ready..."
    kubectl wait --for=condition=ready pod -l app=rabbitmq -n "$NAMESPACE" --timeout=120s || {
        print_warn "RabbitMQ may not be ready yet, continuing..."
    }

    # Apply service manifests with variable substitution
    print_step "Deploying API Gateway..."
    envsubst < "$K8S_DIR/api-gateway.yaml" | kubectl apply -f -

    print_step "Deploying microservices..."
    envsubst < "$K8S_DIR/services.yaml" | kubectl apply -f -

    # Apply ingress
    print_step "Applying ingress..."
    kubectl apply -f "$K8S_DIR/ingress.yaml"

    print_step "Deployment complete!"
    echo ""
    k8s_status
}

delete_all() {
    print_header "Deleting All K8s Resources"
    print_warn "This will delete ALL resources in namespace ${NAMESPACE}!"
    read -p "Are you sure? (y/N): " confirm
    if [ "$confirm" = "y" ] || [ "$confirm" = "Y" ]; then
        kubectl delete namespace "$NAMESPACE" --ignore-not-found
        print_step "All resources deleted"
    else
        print_step "Deletion cancelled"
    fi
}

k8s_status() {
    print_header "Kubernetes Status"
    echo "--- Pods ---"
    kubectl get pods -n "$NAMESPACE" -o wide
    echo ""
    echo "--- Services ---"
    kubectl get svc -n "$NAMESPACE"
    echo ""
    echo "--- HPA ---"
    kubectl get hpa -n "$NAMESPACE"
    echo ""
    echo "--- Ingress ---"
    kubectl get ingress -n "$NAMESPACE"
}

k8s_logs() {
    if [ -z "$2" ]; then
        echo "Usage: $0 logs <service-name>"
        echo "Services: api-gateway, travel-request-api, travel-transaction-api, booking-api,"
        echo "          expense-api, finance-api, insurance-api, masterdata-api, agency-api, admin-api"
        exit 1
    fi
    kubectl logs -f -l "app=$2" -n "$NAMESPACE" --tail=100
}

rollback() {
    if [ -z "$2" ]; then
        echo "Usage: $0 rollback <deployment-name>"
        exit 1
    fi
    print_step "Rolling back $2..."
    kubectl rollout undo deployment/"$2" -n "$NAMESPACE"
    kubectl rollout status deployment/"$2" -n "$NAMESPACE"
    print_step "Rollback complete"
}

case "${1:-}" in
    build)    build_images ;;
    deploy)   deploy ;;
    delete)   delete_all ;;
    status)   k8s_status ;;
    logs)     k8s_logs "$@" ;;
    rollback) rollback "$@" ;;
    *)        usage ;;
esac
