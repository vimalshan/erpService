#!/bin/bash
# ============================================================================
# SPARSH Platform - Kubernetes Deployment Script
# Usage: ./scripts/deploy-k8s.sh [apply|delete|status] [--registry <reg>] [--tag <tag>]
# ============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
K8S_DIR="$ROOT_DIR/k8s"

REGISTRY="${DOCKER_REGISTRY:-sparsh}"
TAG="${IMAGE_TAG:-latest}"
ACTION="${1:-apply}"
shift || true

# Parse optional args
while [[ $# -gt 0 ]]; do
    case $1 in
        --registry) REGISTRY="$2"; shift 2 ;;
        --tag) TAG="$2"; shift 2 ;;
        *) echo "Unknown option: $1"; exit 1 ;;
    esac
done

substitute_vars() {
    local file="$1"
    sed -e "s|\${DOCKER_REGISTRY}|$REGISTRY|g" \
        -e "s|\${IMAGE_TAG}|$TAG|g" \
        "$file"
}

case "$ACTION" in
    apply)
        echo "============================================"
        echo "Deploying SPARSH to Kubernetes"
        echo "Registry: $REGISTRY | Tag: $TAG"
        echo "============================================"

        echo "--- Applying namespace & config ---"
        kubectl apply -f "$K8S_DIR/namespace.yaml"
        kubectl apply -f "$K8S_DIR/configmap.yaml"
        kubectl apply -f "$K8S_DIR/secrets.yaml"

        echo "--- Deploying infrastructure ---"
        kubectl apply -f "$K8S_DIR/infrastructure/"

        echo "--- Waiting for SQL Server readiness ---"
        kubectl -n sparsh rollout status deployment/sqlserver --timeout=120s || true
        kubectl -n sparsh rollout status deployment/rabbitmq --timeout=90s || true

        echo "--- Creating DB init ConfigMap ---"
        kubectl -n sparsh create configmap db-init-scripts \
            --from-file="$ROOT_DIR/docker/db-init/01_init_databases.sql" \
            --dry-run=client -o yaml | kubectl apply -f -

        echo "--- Running DB init job ---"
        kubectl delete job db-init -n sparsh --ignore-not-found=true
        kubectl apply -f "$K8S_DIR/jobs/db-init.yaml"

        echo "--- Deploying application services ---"
        for svc_file in "$K8S_DIR/services/"*.yaml; do
            echo "Applying $(basename "$svc_file")..."
            substitute_vars "$svc_file" | kubectl apply -f -
        done

        echo "--- Applying ingress ---"
        kubectl apply -f "$K8S_DIR/ingress.yaml"

        echo ""
        echo "============================================"
        echo "Deployment complete!"
        echo "Check status: $0 status"
        echo "============================================"
        ;;

    delete)
        echo "Removing SPARSH from Kubernetes..."
        kubectl delete namespace sparsh --ignore-not-found=true
        echo "Deleted."
        ;;

    status)
        echo "SPARSH Platform - Kubernetes Status"
        echo "===================================="
        echo ""
        echo "--- Pods ---"
        kubectl -n sparsh get pods -o wide 2>/dev/null || echo "Namespace 'sparsh' not found"
        echo ""
        echo "--- Services ---"
        kubectl -n sparsh get svc 2>/dev/null || true
        echo ""
        echo "--- Ingress ---"
        kubectl -n sparsh get ingress 2>/dev/null || true
        echo ""
        echo "--- Jobs ---"
        kubectl -n sparsh get jobs 2>/dev/null || true
        ;;

    *)
        echo "Usage: $0 [apply|delete|status] [--registry <reg>] [--tag <tag>]"
        echo "  apply   Deploy all resources to K8s"
        echo "  delete  Remove entire sparsh namespace"
        echo "  status  Show current state"
        exit 1
        ;;
esac
