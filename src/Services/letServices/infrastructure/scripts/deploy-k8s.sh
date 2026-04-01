#!/bin/bash
# ═══════════════════════════════════════════════════════════════════════
# deploy-k8s.sh — Deploy LET ERP to Kubernetes
# Usage: ./deploy-k8s.sh [apply|delete|status]
# ═══════════════════════════════════════════════════════════════════════
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K8S_DIR="$(cd "$SCRIPT_DIR/../k8s" && pwd)"
ACTION="${1:-apply}"

case "$ACTION" in
    apply)
        echo "═══ Deploying LET ERP to Kubernetes ═══"
        echo ""

        echo "[1/7] Creating namespace..."
        kubectl apply -f "$K8S_DIR/namespace.yaml"

        echo "[2/7] Creating secrets..."
        kubectl apply -f "$K8S_DIR/secrets.yaml"

        echo "[3/7] Creating config map..."
        kubectl apply -f "$K8S_DIR/configmap.yaml"

        echo "[4/7] Deploying SQL Server..."
        kubectl apply -f "$K8S_DIR/sqlserver.yaml"

        echo "[5/7] Deploying RabbitMQ..."
        kubectl apply -f "$K8S_DIR/rabbitmq.yaml"

        echo "Waiting for infrastructure pods to be ready..."
        kubectl -n let-erp wait --for=condition=ready pod -l app=sqlserver --timeout=120s
        kubectl -n let-erp wait --for=condition=ready pod -l app=rabbitmq --timeout=120s

        echo "[6/7] Deploying microservices + API Gateway..."
        kubectl apply -f "$K8S_DIR/api-gateway.yaml"
        kubectl apply -f "$K8S_DIR/microservices.yaml"

        echo "[7/7] Configuring autoscaling..."
        kubectl apply -f "$K8S_DIR/hpa.yaml"

        echo ""
        echo "═══ Deployment complete ═══"
        kubectl -n let-erp get pods
        ;;
    delete)
        echo "═══ Removing LET ERP from Kubernetes ═══"
        kubectl delete -f "$K8S_DIR/hpa.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/microservices.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/api-gateway.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/rabbitmq.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/sqlserver.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/configmap.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/secrets.yaml" --ignore-not-found
        kubectl delete -f "$K8S_DIR/namespace.yaml" --ignore-not-found
        echo "All resources removed."
        ;;
    status)
        echo "═══ LET ERP Kubernetes Status ═══"
        echo ""
        echo "─── Pods ───"
        kubectl -n let-erp get pods -o wide
        echo ""
        echo "─── Services ───"
        kubectl -n let-erp get svc
        echo ""
        echo "─── HPA ───"
        kubectl -n let-erp get hpa
        echo ""
        echo "─── Ingress ───"
        kubectl -n let-erp get ingress
        ;;
    *)
        echo "Usage: $0 [apply|delete|status]"
        exit 1
        ;;
esac
