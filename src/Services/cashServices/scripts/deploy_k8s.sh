#!/bin/bash
# ============================================================================
# Cash Services - Deploy to Kubernetes
# ============================================================================

set -e

echo "============================================"
echo "  Cash Services - Kubernetes Deployment"
echo "============================================"

# Step 1: Create namespace and RBAC
echo ">>> Creating namespace and RBAC..."
kubectl apply -f k8s/namespace-rbac.yaml

# Step 2: Create secrets and config
echo ">>> Creating secrets and configmaps..."
kubectl apply -f k8s/secrets.yaml
kubectl apply -f k8s/configmap.yaml

# Step 3: Deploy infrastructure
echo ">>> Deploying infrastructure (MSSQL, RabbitMQ)..."
kubectl apply -f k8s/infrastructure.yaml

echo "Waiting for infrastructure to be ready..."
kubectl -n cash-services wait --for=condition=ready pod -l app=mssql-server --timeout=120s 2>/dev/null || echo "MSSQL still starting..."
kubectl -n cash-services wait --for=condition=ready pod -l app=rabbitmq --timeout=120s 2>/dev/null || echo "RabbitMQ still starting..."

# Step 4: Deploy services
echo ">>> Deploying microservices..."
kubectl apply -f k8s/deployments.yaml

# Step 5: Create services
echo ">>> Creating Kubernetes services..."
kubectl apply -f k8s/services.yaml

# Step 6: Setup ingress
echo ">>> Setting up ingress..."
kubectl apply -f k8s/ingress.yaml

# Step 7: Setup HPA
echo ">>> Setting up autoscaling..."
kubectl apply -f k8s/hpa.yaml

echo ""
echo "============================================"
echo "  Deployment Complete"
echo "============================================"
echo ""
echo "Check status:"
echo "  kubectl -n cash-services get pods"
echo "  kubectl -n cash-services get services"
echo "  kubectl -n cash-services get hpa"
echo ""
