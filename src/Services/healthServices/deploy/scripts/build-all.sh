#!/bin/bash
# ================================================
# Health ERP - Build All Services
# ================================================
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(dirname "$(dirname "$SCRIPT_DIR")")"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

log_info()  { echo -e "${GREEN}[INFO]${NC} $1"; }
log_error() { echo -e "${RED}[ERROR]${NC} $1"; }

FAILED=0
TOTAL=0

build_service() {
    local NAME=$1
    local PATH=$2
    local CSPROJ=$3
    TOTAL=$((TOTAL + 1))

    log_info "Building $NAME..."
    if dotnet build "$ROOT_DIR/$PATH/$CSPROJ" -c Release --nologo -v q 2>&1; then
        echo -e "  ${GREEN}✓ $NAME${NC}"
    else
        echo -e "  ${RED}✗ $NAME${NC}"
        FAILED=$((FAILED + 1))
    fi
}

echo "=========================================="
echo "  Health ERP - Building All Services"
echo "=========================================="
echo ""

build_service "Accident Management" "accidentmanagementServices/src/AccidentManagementService" "AccidentManagementService.csproj"
build_service "Checkup Management"  "healthcheckupServices/src/CheckupManagementService" "CheckupManagementService.csproj"
build_service "Insurance Management" "insurancemanagementServices/src/InsuranceManagement.API" "InsuranceManagement.API.csproj"
build_service "Masters"             "masterServices/src/Masters.API" "Masters.API.csproj"
build_service "Medical Visit"       "medicalvisitServices/src/MedicalVisit.API" "MedicalVisit.API.csproj"
build_service "Medicine Management" "medicinemanagementServices/src/MedicineManagement.API" "MedicineManagement.API.csproj"
build_service "Health Transaction"  "healthTransactionServices/src/HealthTransaction.API" "HealthTransaction.API.csproj"
build_service "API Gateway"         "apiGateway/src/HealthGateway" "HealthGateway.csproj"

echo ""
echo "=========================================="
echo "  Build Summary: $((TOTAL - FAILED))/$TOTAL succeeded"
echo "=========================================="

if [ $FAILED -gt 0 ]; then
    log_error "$FAILED service(s) failed to build."
    exit 1
else
    log_info "All services built successfully!"
fi
