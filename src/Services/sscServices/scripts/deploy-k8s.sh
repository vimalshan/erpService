#!/bin/bash
# ─── Deploy SSC Services to Kubernetes ────────────────────────────────────
set -e

REGISTRY="${REGISTRY:?Set REGISTRY environment variable (e.g. myacr.azurecr.io)}"
TAG="${TAG:-latest}"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K8S_DIR="$(cd "$SCRIPT_DIR/../k8s" && pwd)"

echo "============================================"
echo "Deploying SSC Services to Kubernetes"
echo "Registry: $REGISTRY  Tag: $TAG"
echo "============================================"

# Step 1: Create namespace
echo ""
echo "── Creating namespace ──"
kubectl apply -f "$K8S_DIR/namespace.yaml"

# Step 2: Deploy secrets and configmap
echo ""
echo "── Applying secrets and configmap ──"
kubectl apply -f "$K8S_DIR/secrets.yaml"
kubectl apply -f "$K8S_DIR/configmap.yaml"

# Step 3: Deploy infrastructure (SQL Server + RabbitMQ)
echo ""
echo "── Deploying SQL Server ──"
kubectl apply -f "$K8S_DIR/sqlserver.yaml"

echo ""
echo "── Deploying RabbitMQ ──"
kubectl apply -f "$K8S_DIR/rabbitmq.yaml"

echo ""
echo "── Waiting for infrastructure to be ready ──"
kubectl -n ssc-services rollout status statefulset/sqlserver --timeout=120s || true
kubectl -n ssc-services rollout status statefulset/rabbitmq --timeout=120s || true

# Step 4: Deploy application services (substitute registry)
echo ""
echo "── Deploying application services ──"
sed "s|\${REGISTRY}|$REGISTRY|g; s|:latest|:$TAG|g" "$K8S_DIR/deployments.yaml" | kubectl apply -f -

# Step 5: Deploy ingress
echo ""
echo "── Deploying ingress ──"
kubectl apply -f "$K8S_DIR/ingress.yaml"

# Step 6: Show deployment status
echo ""
echo "============================================"
echo "Deployment Status"
echo "============================================"
kubectl -n ssc-services get deployments
echo ""
kubectl -n ssc-services get services
echo ""
kubectl -n ssc-services get pods

echo ""
echo "Deployment complete!"
echo "Monitor with: kubectl -n ssc-services get pods -w"
