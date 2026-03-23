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
#   - Service Principal for GitHub Actions (AZURE_CREDENTIALS secret)
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

CREDENTIALS=$(az ad sp create-for-rbac \
  --name "$SP_NAME" \
  --role contributor \
  --scopes "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RESOURCE_GROUP" \
  --sdk-auth)

echo ""
echo "============================================================"
echo "  IMPORTANT: Copy the JSON below and add it as a GitHub Secret"
echo "  Go to: https://github.com/vimalshan/erpService/settings/secrets/actions"
echo "  Secret name: AZURE_CREDENTIALS"
echo "============================================================"
echo ""
echo "$CREDENTIALS"
echo ""
echo "============================================================"
echo ""
echo "=== Setup complete! ==="
echo ""
echo "Next steps:"
echo "  1. Copy the JSON above"
echo "  2. Go to GitHub repo → Settings → Secrets → Actions"
echo "  3. Add secret: AZURE_CREDENTIALS = (paste the JSON)"
echo "  4. Go to Actions → 'Deploy to Azure Container Apps' → Run workflow"
echo "  5. After deployment, check the workflow summary for public URLs"
echo ""
echo "Estimated Azure cost: ~\$0.50-2.00/day for 9 services (Container Apps free tier covers some usage)"
