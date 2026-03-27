#!/bin/bash

# Ensure we run from the loanmanagementServices directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR/.." || exit 1

# Stop and remove existing container
echo "Stopping and removing existing container: loan-management-api"
docker rm -f loan-management-api 2>/dev/null || true

# Rebuild image
echo "Building image: loan-management-api"
docker build -t loan-management-api:latest -f Docker/Dockerfile .

# Connect to existing shared infrastructure on erp-network
echo "Starting container on port 5268 (using shared erp-network)"
docker run -d -p 5268:5268 --name loan-management-api \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ASPNETCORE_URLS=http://+:5268 \
  -e "ConnectionStrings__LoanManagement=Data Source=erp-sqlserver,1433;Initial Catalog=CASHDB;User ID=sa;Password=YourStr0ng!Passw0rd;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30" \
  -e RabbitMQ__HostName=erp-rabbitmq \
  -e RabbitMQ__Port=5672 \
  -e RabbitMQ__UserName=guest \
  -e RabbitMQ__Password=guest \
  --network erp-network \
  loan-management-api:latest

# Wait for startup
echo "Waiting 10 seconds for service to start..."
sleep 10

# Check logs
echo "Container logs:"
docker logs loan-management-api 2>&1 | tail -20

# Test
echo ""
echo "Testing service at http://localhost:5268/health"
if command -v curl &> /dev/null; then
    curl -s http://localhost:5268/health | python3 -m json.tool 2>/dev/null || curl -s http://localhost:5268/health
    echo ""
else
    echo "curl not found, skipping API check."
fi
