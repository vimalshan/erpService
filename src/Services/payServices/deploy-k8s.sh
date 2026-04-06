#!/bin/bash
# ============================================================
# ERP Microservice - Kubernetes Deploy Script
# Usage: ./deploy-k8s.sh [apply|delete|status]
# ============================================================

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K8S_DIR="$SCRIPT_DIR/k8s"
REGISTRY="${DOCKER_REGISTRY:-erp}"

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

log_info()  { echo -e "${BLUE}[INFO]${NC} $1"; }
log_ok()    { echo -e "${GREEN}[OK]${NC} $1"; }
log_warn()  { echo -e "${YELLOW}[WARN]${NC} $1"; }
log_error() { echo -e "${RED}[ERROR]${NC} $1"; }

check_deps() {
    if ! command -v kubectl &> /dev/null; then
        log_error "kubectl is not installed"
        exit 1
    fi
    if ! command -v docker &> /dev/null; then
        log_error "Docker is not installed"
        exit 1
    fi
    log_ok "Dependencies verified"
}

build_images() {
    log_info "Building Docker images..."
    
    local services=(
        "api-gateway:apiGateway"
        "employee-service:employeeServices"
        "hr-service:hrServices"
        "faq-service:faqServices"
        "payroll-service:payrollServices"
        "tax-service:taxServices"
        "paytransactional-service:payTransactionalServices"
    )

    for svc in "${services[@]}"; do
        name="${svc%%:*}"
        context="${svc##*:}"
        log_info "Building $name..."
        docker build -t "$REGISTRY/$name:latest" -f "$SCRIPT_DIR/$context/Dockerfile" "$SCRIPT_DIR/$context"
        log_ok "$name built"
    done
}

cmd_apply() {
    log_info "Deploying to Kubernetes..."

    log_info "Creating namespace..."
    kubectl apply -f "$K8S_DIR/namespace.yaml"

    log_info "Applying secrets & config..."
    kubectl apply -f "$K8S_DIR/secrets-configmap.yaml"

    log_info "Deploying infrastructure..."
    kubectl apply -f "$K8S_DIR/sqlserver.yaml"
    kubectl apply -f "$K8S_DIR/rabbitmq.yaml"

    log_info "Waiting for SQL Server to be ready..."
    kubectl -n erp-microservices wait --for=condition=ready pod -l app=sqlserver --timeout=120s || log_warn "SQL Server not ready yet"

    log_info "Waiting for RabbitMQ to be ready..."
    kubectl -n erp-microservices wait --for=condition=ready pod -l app=rabbitmq --timeout=120s || log_warn "RabbitMQ not ready yet"

    log_info "Deploying microservices..."
    kubectl apply -f "$K8S_DIR/api-gateway.yaml"
    kubectl apply -f "$K8S_DIR/employee-service.yaml"
    kubectl apply -f "$K8S_DIR/hr-service.yaml"
    kubectl apply -f "$K8S_DIR/faq-service.yaml"
    kubectl apply -f "$K8S_DIR/payroll-service.yaml"
    kubectl apply -f "$K8S_DIR/tax-service.yaml"
    kubectl apply -f "$K8S_DIR/paytransactional-service.yaml"

    log_info "Applying ingress..."
    kubectl apply -f "$K8S_DIR/ingress.yaml"

    log_ok "Deployment complete"
    echo ""
    cmd_status
}

cmd_delete() {
    log_info "Removing Kubernetes resources..."
    kubectl delete -f "$K8S_DIR/" --ignore-not-found=true
    log_ok "All resources removed"
}

cmd_status() {
    log_info "Kubernetes resource status:"
    echo ""
    echo "--- Pods ---"
    kubectl -n erp-microservices get pods -o wide
    echo ""
    echo "--- Services ---"
    kubectl -n erp-microservices get svc
    echo ""
    echo "--- Deployments ---"
    kubectl -n erp-microservices get deployments
    echo ""
    echo "--- Ingress ---"
    kubectl -n erp-microservices get ingress
}

cmd_logs() {
    local service="${2:-api-gateway}"
    kubectl -n erp-microservices logs -f -l "app=$service" --all-containers
}

# Main
check_deps

case "${1:-apply}" in
    build)  build_images ;;
    apply)  cmd_apply ;;
    delete) cmd_delete ;;
    status) cmd_status ;;
    logs)   cmd_logs "$@" ;;
    *)
        echo "Usage: $0 {build|apply|delete|status|logs}"
        echo ""
        echo "Commands:"
        echo "  build     Build all Docker images for k8s"
        echo "  apply     Deploy to Kubernetes (default)"
        echo "  delete    Remove all Kubernetes resources"
        echo "  status    Show pod/service/deployment status"
        echo "  logs      View logs (optional: service name)"
        exit 1
        ;;
esac
