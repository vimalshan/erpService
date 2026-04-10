#!/bin/bash
set -euo pipefail

###############################################################################
# k8s-deploy.sh — Deploy ERP Microservices to Kubernetes
###############################################################################

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/../.." && pwd)"
K8S_DIR="$ROOT_DIR/k8s"
NAMESPACE="erp-microservices"

echo "============================================"
echo "  ERP Microservices — Kubernetes Deployment"
echo "============================================"

# Check kubectl
if ! command -v kubectl &> /dev/null; then
    echo "ERROR: kubectl is not installed or not in PATH"
    exit 1
fi

# Check cluster connection
if ! kubectl cluster-info &> /dev/null; then
    echo "ERROR: Cannot connect to Kubernetes cluster"
    exit 1
fi

ACTION="${1:-deploy}"

deploy() {
    echo ""
    echo "Step 1/4 — Creating namespace & config..."
    kubectl apply -f "$K8S_DIR/namespace.yaml"
    kubectl apply -f "$K8S_DIR/secrets-configmap.yaml"
    echo "  ✓ Namespace and secrets created"

    echo ""
    echo "Step 2/4 — Deploying infrastructure..."
    kubectl apply -f "$K8S_DIR/infrastructure/sqlserver.yaml"
    kubectl apply -f "$K8S_DIR/infrastructure/rabbitmq.yaml"
    echo "  ✓ SQL Server and RabbitMQ deployed"

    echo ""
    echo "  Waiting for infrastructure to be ready..."
    kubectl -n "$NAMESPACE" rollout status statefulset/sqlserver --timeout=120s || true
    kubectl -n "$NAMESPACE" rollout status statefulset/rabbitmq --timeout=120s || true
    echo "  ✓ Infrastructure ready"

    echo ""
    echo "Step 3/4 — Deploying microservices..."
    for manifest in "$K8S_DIR"/services/*.yaml; do
        name=$(basename "$manifest" .yaml)
        echo "  Applying $name..."
        kubectl apply -f "$manifest"
    done
    echo "  ✓ All microservices deployed"

    echo ""
    echo "Step 4/4 — Deploying ingress..."
    if [ -f "$K8S_DIR/ingress.yaml" ]; then
        kubectl apply -f "$K8S_DIR/ingress.yaml"
        echo "  ✓ Ingress deployed"
    else
        echo "  ⊘ No ingress.yaml found, skipping"
    fi

    echo ""
    echo "============================================"
    echo "  Deployment complete"
    echo "============================================"
    echo ""
    echo "Check status:  kubectl -n $NAMESPACE get pods"
    echo "Gateway:       kubectl -n $NAMESPACE get svc api-gateway-service"
}

teardown() {
    echo ""
    read -rp "This will delete ALL resources in namespace '$NAMESPACE'. Continue? [y/N] " answer
    if [[ "$answer" =~ ^[Yy]$ ]]; then
        echo "Removing all resources..."
        kubectl delete namespace "$NAMESPACE" --ignore-not-found
        echo "Namespace $NAMESPACE deleted."
    fi
}

status() {
    echo ""
    echo "--- Pods ---"
    kubectl -n "$NAMESPACE" get pods -o wide
    echo ""
    echo "--- Services ---"
    kubectl -n "$NAMESPACE" get svc
    echo ""
    echo "--- Deployments ---"
    kubectl -n "$NAMESPACE" get deployments
    echo ""
    echo "--- StatefulSets ---"
    kubectl -n "$NAMESPACE" get statefulsets
}

scale() {
    local service="${2:-}"
    local replicas="${3:-}"
    if [ -z "$service" ] || [ -z "$replicas" ]; then
        echo "Usage: $0 scale <deployment-name> <replicas>"
        exit 1
    fi
    kubectl -n "$NAMESPACE" scale deployment "$service" --replicas="$replicas"
    echo "Scaled $service to $replicas replicas"
}

case "$ACTION" in
    deploy)   deploy ;;
    teardown) teardown ;;
    status)   status ;;
    scale)    scale "$@" ;;
    *)
        echo "Usage: $0 {deploy|teardown|status|scale <name> <n>}"
        exit 1
        ;;
esac
