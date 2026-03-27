#!/bin/bash
# ============================================================================
# Cash Services - Remove from Kubernetes
# ============================================================================

echo "Removing Cash Services from Kubernetes..."

kubectl delete -f k8s/hpa.yaml 2>/dev/null || true
kubectl delete -f k8s/ingress.yaml 2>/dev/null || true
kubectl delete -f k8s/services.yaml 2>/dev/null || true
kubectl delete -f k8s/deployments.yaml 2>/dev/null || true
kubectl delete -f k8s/infrastructure.yaml 2>/dev/null || true
kubectl delete -f k8s/configmap.yaml 2>/dev/null || true
kubectl delete -f k8s/secrets.yaml 2>/dev/null || true
kubectl delete -f k8s/namespace-rbac.yaml 2>/dev/null || true

echo "All Cash Services resources removed from Kubernetes."
