#!/bin/bash
# ──────────────────────────────────────────────────────────────────────────────
# DD ERP — Kubernetes Deployment Script
# ──────────────────────────────────────────────────────────────────────────────
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K8S_DIR="$SCRIPT_DIR/../k8s"

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m'

log()   { echo -e "${CYAN}[K8S]${NC} $1"; }
ok()    { echo -e "${GREEN}[OK]${NC} $1"; }
warn()  { echo -e "${YELLOW}[WARN]${NC} $1"; }
err()   { echo -e "${RED}[ERROR]${NC} $1"; }

usage() {
    cat << EOF
Usage: $(basename "$0") <command>

Commands:
  apply       Apply all K8s manifests (namespace → secrets → config → infra → services → ingress)
  delete      Delete entire dd-erp namespace and all resources
  status      Show status of all pods, services, deployments
  logs <svc>  Show logs for a service (e.g., logs api-gateway)
  restart     Restart all deployments (rolling restart)
  scale <n>   Scale all service deployments to n replicas

EOF
    exit 1
}

cmd_apply() {
    log "Applying Kubernetes manifests..."

    log "1/6 Creating namespace..."
    kubectl apply -f "$K8S_DIR/namespace.yaml"
    ok "Namespace dd-erp created"

    log "2/6 Applying secrets..."
    kubectl apply -f "$K8S_DIR/secrets.yaml"
    ok "Secrets applied"

    log "3/6 Applying configmap..."
    kubectl apply -f "$K8S_DIR/configmap.yaml"
    ok "ConfigMap applied"

    log "4/6 Deploying SQL Server..."
    kubectl apply -f "$K8S_DIR/sqlserver.yaml"
    ok "SQL Server StatefulSet applied"

    log "5/6 Deploying RabbitMQ..."
    kubectl apply -f "$K8S_DIR/rabbitmq.yaml"
    ok "RabbitMQ StatefulSet applied"

    log "Waiting for infrastructure to be ready..."
    kubectl -n dd-erp wait --for=condition=Ready pod -l app=sqlserver --timeout=120s 2>/dev/null || warn "SQL Server not ready yet"
    kubectl -n dd-erp wait --for=condition=Ready pod -l app=rabbitmq --timeout=120s 2>/dev/null || warn "RabbitMQ not ready yet"

    log "6/6 Deploying all microservices..."
    kubectl apply -f "$K8S_DIR/services.yaml"
    ok "All 16 microservice deployments applied"

    if [ -f "$K8S_DIR/ingress.yaml" ]; then
        log "Applying ingress..."
        kubectl apply -f "$K8S_DIR/ingress.yaml"
        ok "Ingress applied"
    fi

    echo ""
    ok "All manifests applied successfully!"
    echo ""
    cmd_status
}

cmd_delete() {
    warn "This will delete the ENTIRE dd-erp namespace and all resources!"
    read -p "Are you sure? (y/N): " confirm
    if [ "$confirm" = "y" ] || [ "$confirm" = "Y" ]; then
        log "Deleting namespace dd-erp..."
        kubectl delete namespace dd-erp --grace-period=30
        ok "Namespace dd-erp deleted"
    else
        log "Cancelled."
    fi
}

cmd_status() {
    log "=== Deployments ==="
    kubectl -n dd-erp get deployments -o wide 2>/dev/null || warn "No deployments found"
    echo ""
    log "=== StatefulSets ==="
    kubectl -n dd-erp get statefulsets -o wide 2>/dev/null || warn "No statefulsets found"
    echo ""
    log "=== Pods ==="
    kubectl -n dd-erp get pods -o wide 2>/dev/null || warn "No pods found"
    echo ""
    log "=== Services ==="
    kubectl -n dd-erp get services -o wide 2>/dev/null || warn "No services found"
    echo ""
    log "=== Ingress ==="
    kubectl -n dd-erp get ingress -o wide 2>/dev/null || warn "No ingress found"
}

cmd_logs() {
    local svc=$1
    if [ -z "$svc" ]; then
        err "Specify a service name. E.g.: $(basename "$0") logs api-gateway"
        exit 1
    fi
    kubectl -n dd-erp logs -l app="$svc" --tail=100 -f
}

cmd_restart() {
    log "Rolling restart of all deployments..."
    DEPLOYMENTS=$(kubectl -n dd-erp get deployments -o name 2>/dev/null)
    for dep in $DEPLOYMENTS; do
        kubectl -n dd-erp rollout restart "$dep"
        ok "Restarted $dep"
    done
    ok "All deployments restarting"
}

cmd_scale() {
    local replicas=$1
    if [ -z "$replicas" ]; then
        err "Specify replica count. E.g.: $(basename "$0") scale 3"
        exit 1
    fi
    log "Scaling all service deployments to $replicas replicas..."
    DEPLOYMENTS=$(kubectl -n dd-erp get deployments -o name 2>/dev/null | grep -v sqlserver | grep -v rabbitmq)
    for dep in $DEPLOYMENTS; do
        kubectl -n dd-erp scale "$dep" --replicas="$replicas"
        ok "Scaled $dep to $replicas"
    done
}

# ── Main ─────────────────────────────────────────────────────────────────────
case "${1:-}" in
    apply)   cmd_apply ;;
    delete)  cmd_delete ;;
    status)  cmd_status ;;
    logs)    cmd_logs "$2" ;;
    restart) cmd_restart ;;
    scale)   cmd_scale "$2" ;;
    *)       usage ;;
esac
