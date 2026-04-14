#!/bin/bash
# =============================================================================
# WMS Microservices - Kubernetes Deployment Script
# =============================================================================
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K8S_DIR="$(cd "$SCRIPT_DIR/../k8s" && pwd)"

ACTION="${1:-apply}"
NAMESPACE="wms"

echo "=============================================="
echo " WMS Microservices - Kubernetes Deployment"
echo " Action: $ACTION"
echo "=============================================="

case "$ACTION" in
  apply|deploy)
    echo ""
    echo "[1/7] Creating namespace..."
    kubectl apply -f "$K8S_DIR/00-namespace.yaml"

    echo ""
    echo "[2/7] Creating secrets..."
    kubectl apply -f "$K8S_DIR/01-secrets.yaml"

    echo ""
    echo "[3/7] Creating config maps..."
    kubectl apply -f "$K8S_DIR/02-configmap.yaml"

    echo ""
    echo "[4/7] Deploying SQL Server..."
    kubectl apply -f "$K8S_DIR/03-sqlserver.yaml"
    echo "Waiting for SQL Server to be ready..."
    kubectl wait --for=condition=ready pod -l app=sqlserver -n $NAMESPACE --timeout=120s

    echo ""
    echo "[5/7] Deploying RabbitMQ..."
    kubectl apply -f "$K8S_DIR/04-rabbitmq.yaml"
    echo "Waiting for RabbitMQ to be ready..."
    kubectl wait --for=condition=ready pod -l app=rabbitmq -n $NAMESPACE --timeout=120s

    echo ""
    echo "[6/7] Deploying all microservices..."
    kubectl apply -f "$K8S_DIR/05-services.yaml"

    echo ""
    echo "[7/7] Deploying API Gateway & Ingress..."
    kubectl apply -f "$K8S_DIR/06-api-gateway.yaml"
    kubectl apply -f "$K8S_DIR/07-ingress.yaml"

    echo ""
    echo "=============================================="
    echo " Kubernetes deployment complete!"
    echo "=============================================="
    echo ""
    echo "Waiting for all pods to be ready..."
    kubectl wait --for=condition=ready pod --all -n $NAMESPACE --timeout=300s
    echo ""
    kubectl get pods -n $NAMESPACE
    echo ""
    kubectl get svc -n $NAMESPACE
    ;;

  delete|destroy)
    echo "Deleting all WMS resources..."
    kubectl delete -f "$K8S_DIR/07-ingress.yaml" --ignore-not-found
    kubectl delete -f "$K8S_DIR/06-api-gateway.yaml" --ignore-not-found
    kubectl delete -f "$K8S_DIR/05-services.yaml" --ignore-not-found
    kubectl delete -f "$K8S_DIR/04-rabbitmq.yaml" --ignore-not-found
    kubectl delete -f "$K8S_DIR/03-sqlserver.yaml" --ignore-not-found
    kubectl delete -f "$K8S_DIR/02-configmap.yaml" --ignore-not-found
    kubectl delete -f "$K8S_DIR/01-secrets.yaml" --ignore-not-found
    kubectl delete -f "$K8S_DIR/00-namespace.yaml" --ignore-not-found
    echo "All WMS resources deleted."
    ;;

  status)
    echo "Namespace: $NAMESPACE"
    echo ""
    echo "--- Pods ---"
    kubectl get pods -n $NAMESPACE -o wide
    echo ""
    echo "--- Services ---"
    kubectl get svc -n $NAMESPACE
    echo ""
    echo "--- Deployments ---"
    kubectl get deployments -n $NAMESPACE
    echo ""
    echo "--- Ingress ---"
    kubectl get ingress -n $NAMESPACE
    ;;

  restart)
    SERVICE="${2:-}"
    if [ -n "$SERVICE" ]; then
      echo "Restarting $SERVICE..."
      kubectl rollout restart deployment/$SERVICE -n $NAMESPACE
    else
      echo "Restarting all deployments..."
      kubectl rollout restart deployment -n $NAMESPACE
    fi
    ;;

  logs)
    SERVICE="${2:-api-gateway}"
    kubectl logs -f -l app=$SERVICE -n $NAMESPACE --all-containers --tail=100
    ;;

  *)
    echo "Usage: $0 {apply|delete|status|restart [service]|logs [service]}"
    exit 1
    ;;
esac
