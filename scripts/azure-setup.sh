#!/bin/bash
# ──────────────────────────────────────────────────────────────────────────────
# One-time Azure setup for ERP Admin Services
# Run this script ONCE from Azure Cloud Shell or local Azure CLI
#
# Prerequisites:
#   - Azure CLI installed (az --version)
#   - Logged in (az login)
#   - An active Azure subscription
#
# What this creates:
#   - Resource Group: erp-adminservices-rg
#   - Container Apps Environment: erp-admin-env
#   - Service Principal for GitHub Actions (4 secrets)
# ──────────────────────────────────────────────────────────────────────────────

set -e

RESOURCE_GROUP="erp-adminservices-rg"
LOCATION="eastus"
ENVIRONMENT="erp-admin-env"
SP_NAME="erp-github-deploy"

echo "=== Step 1: Create Resource Group ==="
az group create --name "$RESOURCE_GROUP" --location "$LOCATION"

echo ""
echo "=== Step 2: Create Container Apps Environment ==="
az containerapp env create \
  --name "$ENVIRONMENT" \
  --resource-group "$RESOURCE_GROUP" \
  --location "$LOCATION"

echo ""
echo "=== Step 3: Create Service Principal for GitHub Actions ==="
SUBSCRIPTION_ID=$(az account show --query id --output tsv)

SP_OUTPUT=$(az ad sp create-for-rbac \
  --name "$SP_NAME" \
  --role contributor \
  --scopes "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RESOURCE_GROUP" \
  --output json)

CLIENT_ID=$(echo "$SP_OUTPUT" | python3 -c "import sys,json; print(json.load(sys.stdin)['appId'])")
CLIENT_SECRET=$(echo "$SP_OUTPUT" | python3 -c "import sys,json; print(json.load(sys.stdin)['password'])")
TENANT_ID=$(echo "$SP_OUTPUT" | python3 -c "import sys,json; print(json.load(sys.stdin)['tenant'])")

echo ""
echo "============================================================"
echo "  Add these 4 GitHub Secrets:"
echo "  Go to: https://github.com/vimalshan/erpService/settings/secrets/actions"
echo "============================================================"
echo ""
echo "  AZURE_CLIENT_ID       = $CLIENT_ID"
echo "  AZURE_TENANT_ID       = $TENANT_ID"
echo "  AZURE_SUBSCRIPTION_ID = $SUBSCRIPTION_ID"
echo "  AZURE_CLIENT_SECRET   = $CLIENT_SECRET"
echo ""
echo "============================================================"
echo ""
echo "=== Setup complete! ==="
echo ""
echo "Next steps:"
echo "  1. Go to GitHub repo → Settings → Secrets and variables → Actions"
echo "  2. Add these 4 secrets (names and values above)"
echo "  3. Go to Actions → 'Deploy to Azure Container Apps' → Run workflow"
echo "  4. After deployment, check the workflow summary for public URLs"
echo ""
echo "Estimated Azure cost: ~\$0.50-2.00/day for 9 services (Container Apps free tier covers some usage)"
