#!/bin/bash
# =============================================================================
# SCI ERP Microservices - Teardown Script
# Removes all K8s resources or stops Docker Compose
# =============================================================================

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K8S_DIR="$SCRIPT_DIR/../k8s"

MODE="${1:-k8s}"

echo "============================================="
echo "SCI ERP - Teardown"
echo "Mode: $MODE"
echo "============================================="

if [ "$MODE" = "docker-compose" ] || [ "$MODE" = "compose" ]; then
  echo ""
  echo "Stopping Docker Compose services..."
  cd "$SCRIPT_DIR/.."
  docker compose down -v
  echo "Docker Compose services stopped and volumes removed."

elif [ "$MODE" = "k8s" ] || [ "$MODE" = "kubernetes" ]; then
  echo ""
  echo "WARNING: This will delete ALL resources in the sci-erp namespace."
  read -p "Are you sure? (y/N): " CONFIRM
  
  if [ "$CONFIRM" != "y" ] && [ "$CONFIRM" != "Y" ]; then
    echo "Aborted."
    exit 0
  fi

  echo ""
  echo "Removing ingress..."
  kubectl delete -f "$K8S_DIR/ingress.yaml" --ignore-not-found

  echo "Removing services..."
  for SERVICE_FILE in "$K8S_DIR"/services/*.yaml; do
    kubectl delete -f "$SERVICE_FILE" --ignore-not-found
  done

  echo "Removing infrastructure..."
  kubectl delete -f "$K8S_DIR/infrastructure/rabbitmq.yaml" --ignore-not-found
  kubectl delete -f "$K8S_DIR/infrastructure/sqlserver.yaml" --ignore-not-found

  echo "Removing secrets and configmaps..."
  kubectl delete -f "$K8S_DIR/secrets-configmap.yaml" --ignore-not-found

  echo ""
  read -p "Delete namespace (and all PVCs)? (y/N): " DELETE_NS
  if [ "$DELETE_NS" = "y" ] || [ "$DELETE_NS" = "Y" ]; then
    kubectl delete -f "$K8S_DIR/namespace.yaml" --ignore-not-found
    echo "Namespace deleted."
  else
    echo "Namespace preserved."
  fi

  echo ""
  echo "Teardown complete."
else
  echo "Usage: teardown.sh [k8s|docker-compose]"
  exit 1
fi
