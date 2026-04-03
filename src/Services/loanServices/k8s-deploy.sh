#!/usr/bin/env bash
# =============================================================================
# k8s-deploy.sh — Deploy Loan ERP services to Kubernetes via Kustomize
# Usage:
#   ./k8s-deploy.sh [dev|prod]          # apply overlay
#   ./k8s-deploy.sh [dev|prod] delete   # teardown
#   ./k8s-deploy.sh status              # show pod/svc status
# =============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

OVERLAY="${1:-dev}"
ACTION="${2:-apply}"
REGISTRY="${REGISTRY:-}"
TAG="${TAG:-latest}"

GREEN='\033[0;32m'; YELLOW='\033[1;33m'; RED='\033[0;31m'; NC='\033[0m'
info()  { echo -e "${GREEN}[INFO]${NC}  $*"; }
warn()  { echo -e "${YELLOW}[WARN]${NC}  $*"; }
error() { echo -e "${RED}[ERROR]${NC} $*"; exit 1; }

# ── Verify prerequisites ──────────────────────────────────────────────────────
command -v kubectl  >/dev/null 2>&1 || error "kubectl not found in PATH"
command -v kustomize >/dev/null 2>&1 || warn "kustomize binary not found — using 'kubectl kustomize'"

KUSTOMIZE_PATH="k8s/overlays/${OVERLAY}"
[[ -d "$KUSTOMIZE_PATH" ]] || error "Overlay not found: $KUSTOMIZE_PATH"

# ── Optional: update image tags using kustomize ───────────────────────────────
update_images() {
  if [[ -n "$REGISTRY" ]]; then
    info "Setting image tags to ${REGISTRY}/<name>:${TAG} ..."
    cd "$KUSTOMIZE_PATH"
    kustomize edit set image \
      "loan-transaction=${REGISTRY}/loan-transaction:${TAG}" \
      "loan-application=${REGISTRY}/loan-application:${TAG}" \
      "loan-account=${REGISTRY}/loan-account:${TAG}" \
      "loan-definition=${REGISTRY}/loan-definition:${TAG}" \
      "document-service=${REGISTRY}/document-service:${TAG}" \
      "lov-service=${REGISTRY}/lov-service:${TAG}" \
      "utility-service=${REGISTRY}/utility-service:${TAG}" \
      "api-gateway=${REGISTRY}/api-gateway:${TAG}"
    cd "$SCRIPT_DIR"
  fi
}

# ── Apply ─────────────────────────────────────────────────────────────────────
cmd_apply() {
  update_images
  info "Deploying overlay: $OVERLAY ..."
  kubectl apply -k "$KUSTOMIZE_PATH"
  info "Waiting for rollout..."
  kubectl rollout status deployment -n loan-services --timeout=300s
  cmd_status
}

# ── Delete ────────────────────────────────────────────────────────────────────
cmd_delete() {
  warn "Deleting overlay: $OVERLAY — this will remove all resources in loan-services namespace"
  read -rp "Are you sure? (yes/no): " confirm
  [[ "$confirm" == "yes" ]] || { info "Aborted."; exit 0; }
  kubectl delete -k "$KUSTOMIZE_PATH" --ignore-not-found
}

# ── Status ────────────────────────────────────────────────────────────────────
cmd_status() {
  echo ""
  info "Pods:"
  kubectl get pods -n loan-services -o wide
  echo ""
  info "Services:"
  kubectl get svc -n loan-services
  echo ""
  info "Ingress:"
  kubectl get ingress -n loan-services
}

case "$ACTION" in
  apply|"")    cmd_apply ;;
  delete)      cmd_delete ;;
  status)      cmd_status ;;
  *)           error "Unknown action: $ACTION. Use: apply|delete|status" ;;
esac
