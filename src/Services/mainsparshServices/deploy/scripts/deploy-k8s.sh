#!/bin/bash
# ==========================================
# SRF Sparsh - Kubernetes Deploy Script
# ==========================================

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K8S_DIR="$SCRIPT_DIR/../k8s"
REGISTRY="${REGISTRY:-srfsparsh}"
IMAGE_TAG="${IMAGE_TAG:-latest}"

echo "============================================"
echo " SRF Sparsh - Kubernetes Deployment"
echo " Registry: $REGISTRY"
echo " Tag: $IMAGE_TAG"
echo "============================================"

case "${1:-apply}" in
    apply)
        echo "Creating namespace..."
        kubectl apply -f "$K8S_DIR/namespace.yaml"

        echo "Creating secrets and config..."
        kubectl apply -f "$K8S_DIR/secrets.yaml"
        kubectl apply -f "$K8S_DIR/configmap.yaml"

        echo "Deploying infrastructure..."
        kubectl apply -f "$K8S_DIR/infrastructure.yaml"

        echo "Waiting for infrastructure to be ready..."
        kubectl -n srfsparsh wait --for=condition=available --timeout=120s deployment/sqlserver || true
        kubectl -n srfsparsh wait --for=condition=available --timeout=120s deployment/rabbitmq || true
        kubectl -n srfsparsh wait --for=condition=available --timeout=60s deployment/redis || true

        echo "Deploying microservices..."
        kubectl apply -f "$K8S_DIR/services.yaml"

        echo "Deploying ingress..."
        kubectl apply -f "$K8S_DIR/ingress.yaml"

        echo ""
        echo "Deployment complete. Checking status..."
        kubectl -n srfsparsh get deployments
        kubectl -n srfsparsh get services
        ;;
    delete)
        echo "Deleting all resources..."
        kubectl delete -f "$K8S_DIR/ingress.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/services.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/infrastructure.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/secrets.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/configmap.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/namespace.yaml" --ignore-not-found
        echo "All resources deleted."
        ;;
    status)
        echo "Namespace: srfsparsh"
        kubectl -n srfsparsh get all
        ;;
    logs)
        if [ -n "$2" ]; then
            kubectl -n srfsparsh logs -f "deployment/$2" --all-containers
        else
            echo "Usage: $0 logs <deployment-name>"
            echo "Available deployments:"
            kubectl -n srfsparsh get deployments -o name
        fi
        ;;
    *)
        echo "Usage: $0 {apply|delete|status|logs <deployment>}"
        exit 1
        ;;
esac
