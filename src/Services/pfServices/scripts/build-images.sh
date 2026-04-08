#!/bin/bash
# ============================================
# Build Docker Images for All PF Services
# ============================================
set -e

REGISTRY="${REGISTRY:-pfservices}"
TAG="${TAG:-latest}"

echo "============================================"
echo "Building PF Services Docker Images"
echo "Registry: ${REGISTRY}"
echo "Tag: ${TAG}"
echo "============================================"

SERVICES=(
    "api-gateway:apiGateway"
    "accounting-service:accountingServices"
    "bank-service:bankServices/BankService"
    "contribution-service:contributionServices/ContributionService"
    "investment-service:investmentServices/InvestmentService"
    "loan-service:loanServices"
    "masterdata-service:masterdataServices/MasterDataService"
    "member-service:memberServices"
    "pftransactional-service:pftransactionalServices"
    "settlement-service:settlementServices"
    "trust-service:trustServices/TrustService"
)

FAILED=()

for entry in "${SERVICES[@]}"; do
    IFS=':' read -r name context <<< "$entry"
    echo ""
    echo "--- Building ${name} ---"
    if docker build -t "${REGISTRY}/${name}:${TAG}" -f "${context}/Dockerfile" "${context}"; then
        echo "[OK] ${name}"
    else
        echo "[FAIL] ${name}"
        FAILED+=("$name")
    fi
done

echo ""
echo "============================================"
echo "Build Summary"
echo "============================================"
if [ ${#FAILED[@]} -eq 0 ]; then
    echo "All images built successfully."
else
    echo "Failed builds:"
    for f in "${FAILED[@]}"; do
        echo "  - ${f}"
    done
    exit 1
fi
