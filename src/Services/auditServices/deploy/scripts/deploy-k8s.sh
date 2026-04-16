#!/usr/bin/env bash
# =============================================================================
# deploy-k8s.sh — Apply all K8s manifests and update image tags
# Usage: ./deploy-k8s.sh REGISTRY [TAG]
# Example: ./deploy-k8s.sh myregistry.azurecr.io 1.2.0
# =============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K8S_DIR="$(cd "$SCRIPT_DIR/../k8s" && pwd)"

REGISTRY="${1:?Usage: $0 REGISTRY [TAG]}"
TAG="${2:-latest}"

echo "========================================="
echo " Deploying to Kubernetes"
echo " Registry : $REGISTRY"
echo " Tag      : $TAG"
echo "========================================="

# --- 1. Apply base manifests (namespace, secrets, configmaps, infra) ---------
echo ""
echo ">>> Applying namespace, secrets, configmaps, infrastructure ..."
kubectl apply -f "$K8S_DIR/00-namespace.yaml"
kubectl apply -f "$K8S_DIR/01-secrets.yaml"
kubectl apply -f "$K8S_DIR/02-configmaps.yaml"
kubectl apply -f "$K8S_DIR/03-infrastructure.yaml"

# --- 2. Wait for SQL Server and RabbitMQ to be ready -------------------------
echo ""
echo ">>> Waiting for SQL Server to be ready ..."
kubectl rollout status statefulset/erp-sqlserver -n erp --timeout=180s

echo ">>> Waiting for RabbitMQ to be ready ..."
kubectl rollout status statefulset/erp-rabbitmq -n erp --timeout=120s

# --- 3. Apply microservices with updated image tags --------------------------
SERVICES=(
  "erp-action-service"
  "erp-audit-service"
  "erp-certificate-service"
  "erp-contract-service"
  "erp-finance-service"
  "erp-findings-service"
  "erp-notification-service"
  "erp-schedule-service"
  "erp-settings-service"
)

echo ""
echo ">>> Applying microservices manifests ..."
kubectl apply -f "$K8S_DIR/04-microservices.yaml"

for svc in "${SERVICES[@]}"; do
  deployment="${svc}"
  new_image="$REGISTRY/$svc:$TAG"
  container="${svc#erp-}"      # strip "erp-" prefix for container name

  echo "    Updating $deployment -> $new_image"
  kubectl set image "deployment/$deployment" "$container=$new_image" -n erp
done

# --- 4. Apply gateway and ingress --------------------------------------------
echo ""
echo ">>> Applying gateway and ingress ..."
kubectl apply -f "$K8S_DIR/05-gateway-ingress.yaml"

echo "    Updating erp-api-gateway -> $REGISTRY/erp-api-gateway:$TAG"
kubectl set image deployment/erp-api-gateway "api-gateway=$REGISTRY/erp-api-gateway:$TAG" -n erp

# --- 5. Wait for all rollouts ------------------------------------------------
echo ""
echo ">>> Waiting for all deployments to complete ..."
ALL_DEPLOYMENTS=(
  "erp-api-gateway"
  "erp-action-service"
  "erp-audit-service"
  "erp-certificate-service"
  "erp-contract-service"
  "erp-finance-service"
  "erp-findings-service"
  "erp-notification-service"
  "erp-schedule-service"
  "erp-settings-service"
)

for dep in "${ALL_DEPLOYMENTS[@]}"; do
  echo "    Waiting: $dep ..."
  kubectl rollout status "deployment/$dep" -n erp --timeout=300s
  echo "    ✓ $dep"
done

echo ""
echo "========================================="
echo " Deployment complete!"
echo "========================================="
kubectl get pods -n erp
