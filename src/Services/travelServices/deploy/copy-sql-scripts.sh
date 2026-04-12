#!/bin/bash
# ==============================================================================
# Copy SQL scripts into the init-db folder for Docker volume mount
# Run this BEFORE docker-compose up
# ==============================================================================

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
INIT_DIR="$SCRIPT_DIR/deploy/init-db"

echo "Copying SQL scripts to $INIT_DIR ..."

# Root-level TRAVELDB scripts
cp -f "$SCRIPT_DIR/TRAVELDB.sql" "$INIT_DIR/" 2>/dev/null || true
cp -f "$SCRIPT_DIR/TRAVELDB-procedures.sql" "$INIT_DIR/" 2>/dev/null || true

# Service-specific SQL folders
mkdir -p "$INIT_DIR/01-TravelRequest"
cp -f "$SCRIPT_DIR/travelRequestServices/01-TravelRequest/"*.sql "$INIT_DIR/01-TravelRequest/" 2>/dev/null || true

mkdir -p "$INIT_DIR/02-Booking"
cp -f "$SCRIPT_DIR/bookingServices/02-Booking/"*.sql "$INIT_DIR/02-Booking/" 2>/dev/null || true

mkdir -p "$INIT_DIR/03-Agency"
cp -f "$SCRIPT_DIR/agensService/03-Agency/"*.sql "$INIT_DIR/03-Agency/" 2>/dev/null || true

mkdir -p "$INIT_DIR/04-Expense"
cp -f "$SCRIPT_DIR/expenseServices/04-Expense/"*.sql "$INIT_DIR/04-Expense/" 2>/dev/null || true

mkdir -p "$INIT_DIR/05-Finance"
cp -f "$SCRIPT_DIR/financeServices/05-Finance/"*.sql "$INIT_DIR/05-Finance/" 2>/dev/null || true

mkdir -p "$INIT_DIR/06-Admin"
cp -f "$SCRIPT_DIR/adminServices/06-Admin/"*.sql "$INIT_DIR/06-Admin/" 2>/dev/null || true

mkdir -p "$INIT_DIR/07-MasterData"
cp -f "$SCRIPT_DIR/masterdataServices/07-MasterData/"*.sql "$INIT_DIR/07-MasterData/" 2>/dev/null || true

mkdir -p "$INIT_DIR/08-Insurance"
cp -f "$SCRIPT_DIR/insuranceServices/08-Insurance/"*.sql "$INIT_DIR/08-Insurance/" 2>/dev/null || true

echo "SQL scripts copied successfully!"
echo ""
echo "Contents of $INIT_DIR:"
find "$INIT_DIR" -name "*.sql" -o -name "*.sh" | sort
