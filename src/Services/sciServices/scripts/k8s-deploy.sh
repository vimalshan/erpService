#!/bin/bash
# =============================================================================
# SCI ERP Microservices - Kubernetes Deployment Script
# Applies all K8s manifests in the correct order
# =============================================================================

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K8S_DIR="$SCRIPT_DIR/../k8s"

echo "============================================="
echo "SCI ERP - Kubernetes Deployment"
echo "============================================="

# Step 1: Create namespace
echo ""
echo "[1/5] Creating namespace..."
kubectl apply -f "$K8S_DIR/namespace.yaml"

# Step 2: Apply secrets and config
echo ""
echo "[2/5] Applying secrets and configmaps..."
kubectl apply -f "$K8S_DIR/secrets-configmap.yaml"

# Step 3: Deploy infrastructure (SQL Server + RabbitMQ)
echo ""
echo "[3/5] Deploying infrastructure..."
kubectl apply -f "$K8S_DIR/infrastructure/sqlserver.yaml"
kubectl apply -f "$K8S_DIR/infrastructure/rabbitmq.yaml"

echo "Waiting for SQL Server to be ready..."
kubectl rollout status statefulset/sqlserver -n sci-erp --timeout=300s

echo "Waiting for RabbitMQ to be ready..."
kubectl rollout status statefulset/rabbitmq -n sci-erp --timeout=300s

# Step 4: Deploy all microservices
echo ""
echo "[4/5] Deploying microservices..."
for SERVICE_FILE in "$K8S_DIR"/services/*.yaml; do
  SERVICE_NAME=$(basename "$SERVICE_FILE" .yaml)
  echo "  Deploying $SERVICE_NAME..."
  kubectl apply -f "$SERVICE_FILE"
done

# Step 5: Apply ingress
echo ""
echo "[5/5] Applying ingress..."
kubectl apply -f "$K8S_DIR/ingress.yaml"

echo ""
echo "============================================="
echo "Deployment complete!"
echo "============================================="
echo ""
echo "Check status with:"
echo "  kubectl get pods -n sci-erp"
echo "  kubectl get services -n sci-erp"
echo "  kubectl get ingress -n sci-erp"
echo "  kubectl get hpa -n sci-erp"
