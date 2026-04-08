#!/bin/bash
# ============================================
# Deploy PF Services to Kubernetes
# ============================================
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"
K8S_DIR="${PROJECT_DIR}/k8s"

REGISTRY="${REGISTRY:-pfservices}"
TAG="${TAG:-latest}"
NAMESPACE="pf-services"
ACTION="${1:-apply}"

echo "============================================"
echo "PF Services - Kubernetes Deployment"
echo "Action: ${ACTION}"
echo "Registry: ${REGISTRY}"
echo "Tag: ${TAG}"
echo "============================================"

substitute_vars() {
    # Replace ${REGISTRY} and ${TAG} placeholders in YAML files
    sed -e "s|\${REGISTRY}|${REGISTRY}|g" -e "s|\${TAG}|${TAG}|g" "$1"
}

case "$ACTION" in
    apply|deploy)
        echo ""
        echo "--- Creating namespace ---"
        kubectl apply -f "${K8S_DIR}/namespace.yaml"

        echo ""
        echo "--- Applying secrets (update values first!) ---"
        kubectl apply -f "${K8S_DIR}/secrets.yaml"

        echo ""
        echo "--- Applying config ---"
        kubectl apply -f "${K8S_DIR}/configmap.yaml"

        echo ""
        echo "--- Deploying infrastructure ---"
        kubectl apply -f "${K8S_DIR}/infrastructure/"

        echo ""
        echo "--- Waiting for infrastructure to be ready ---"
        kubectl -n ${NAMESPACE} rollout status deployment/sqlserver --timeout=120s || true
        kubectl -n ${NAMESPACE} rollout status deployment/rabbitmq --timeout=120s || true

        echo ""
        echo "--- Deploying services ---"
        for manifest in "${K8S_DIR}/services/"*.yaml; do
            echo "Applying $(basename $manifest)..."
            substitute_vars "$manifest" | kubectl apply -f -
        done

        echo ""
        echo "--- Applying ingress ---"
        kubectl apply -f "${K8S_DIR}/ingress.yaml"

        echo ""
        echo "============================================"
        echo "Deployment complete."
        echo "============================================"
        kubectl -n ${NAMESPACE} get pods
        ;;

    delete|destroy)
        echo "Deleting all PF services from Kubernetes..."
        kubectl delete -f "${K8S_DIR}/ingress.yaml" --ignore-not-found
        kubectl delete -f "${K8S_DIR}/services/" --ignore-not-found
        kubectl delete -f "${K8S_DIR}/infrastructure/" --ignore-not-found
        kubectl delete -f "${K8S_DIR}/configmap.yaml" --ignore-not-found
        kubectl delete -f "${K8S_DIR}/secrets.yaml" --ignore-not-found
        echo "All resources deleted. Namespace preserved."
        ;;

    status)
        echo "Pods:"
        kubectl -n ${NAMESPACE} get pods -o wide
        echo ""
        echo "Services:"
        kubectl -n ${NAMESPACE} get svc
        echo ""
        echo "Ingress:"
        kubectl -n ${NAMESPACE} get ingress
        ;;

    restart)
        echo "Restarting all deployments..."
        for deploy in $(kubectl -n ${NAMESPACE} get deploy -l tier=backend -o name); do
            echo "Restarting ${deploy}..."
            kubectl -n ${NAMESPACE} rollout restart "${deploy}"
        done
        kubectl -n ${NAMESPACE} rollout restart deployment/api-gateway
        echo "All deployments restarted."
        ;;

    logs)
        SERVICE="${2:-}"
        if [ -z "$SERVICE" ]; then
            echo "Usage: $0 logs <service-name>"
            exit 1
        fi
        kubectl -n ${NAMESPACE} logs -f -l app="${SERVICE}" --all-containers
        ;;

    scale)
        SERVICE="${2:-}"
        REPLICAS="${3:-2}"
        if [ -z "$SERVICE" ]; then
            echo "Usage: $0 scale <service-name> <replicas>"
            exit 1
        fi
        kubectl -n ${NAMESPACE} scale deployment "${SERVICE}" --replicas="${REPLICAS}"
        echo "Scaled ${SERVICE} to ${REPLICAS} replicas."
        ;;

    *)
        echo "Usage: $0 {apply|delete|status|restart|logs <service>|scale <service> <replicas>}"
        exit 1
        ;;
esac
