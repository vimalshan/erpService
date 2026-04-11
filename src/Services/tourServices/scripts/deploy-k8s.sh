#!/bin/bash
# =============================================================================
# Tour ERP - Deploy to Kubernetes
# =============================================================================
set -e

NAMESPACE="tour-erp"
SCRIPT_DIR="$(dirname "$0")"
K8S_DIR="$SCRIPT_DIR/../k8s"

echo "=========================================="
echo "  Tour ERP - Kubernetes Deployment"
echo "=========================================="

# 1. Create namespace
echo ""
echo "[1/6] Creating namespace..."
kubectl apply -f "$K8S_DIR/namespace.yaml"

# 2. Apply ConfigMap and Secrets
echo ""
echo "[2/6] Applying ConfigMap and Secrets..."
kubectl apply -f "$K8S_DIR/configmap-secrets.yaml"

# 3. Deploy infrastructure (SQL Server, RabbitMQ)
echo ""
echo "[3/6] Deploying SQL Server..."
kubectl apply -f "$K8S_DIR/sqlserver.yaml"
echo "Waiting for SQL Server to be ready..."
kubectl rollout status statefulset/sqlserver -n "$NAMESPACE" --timeout=180s

echo ""
echo "[4/6] Deploying RabbitMQ..."
kubectl apply -f "$K8S_DIR/rabbitmq.yaml"
echo "Waiting for RabbitMQ to be ready..."
kubectl rollout status statefulset/rabbitmq -n "$NAMESPACE" --timeout=120s

# 4. Deploy all microservices
echo ""
echo "[5/6] Deploying microservices..."
for SVC_FILE in "$K8S_DIR"/services/*.yaml; do
  echo "  Applying $(basename "$SVC_FILE")..."
  kubectl apply -f "$SVC_FILE"
done

# Wait for deployments
echo ""
echo "Waiting for deployments to be ready..."
DEPLOYMENTS=(
  "admin-service"
  "booking-service"
  "config-service"
  "tourplan-service"
  "tour-service"
  "transaction-service"
  "travel-service"
  "api-gateway"
)
for DEPLOY in "${DEPLOYMENTS[@]}"; do
  echo "  Waiting for $DEPLOY..."
  kubectl rollout status deployment/"$DEPLOY" -n "$NAMESPACE" --timeout=120s
done

# 5. Apply Ingress and HPA
echo ""
echo "[6/6] Applying Ingress and HPA..."
kubectl apply -f "$K8S_DIR/ingress.yaml"
kubectl apply -f "$K8S_DIR/hpa.yaml"

echo ""
echo "=========================================="
echo "  Deployment complete!"
echo "=========================================="
echo ""
echo "Verify with:"
echo "  kubectl get all -n $NAMESPACE"
echo "  kubectl get ingress -n $NAMESPACE"
