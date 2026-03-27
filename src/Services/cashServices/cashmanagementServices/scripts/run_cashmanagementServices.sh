#!/bin/bash

# Ensure we run from the cashmanagementServices directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR/.." || exit 1

# Stop and remove existing container
echo "Stopping and removing existing container: cash-management-api"
docker rm -f cash-management-api 2>/dev/null || true

# Build image
echo "Building image: cash-management-api"
docker build -t cash-management-api:latest -f Docker/Dockerfile .

# Connect to existing shared infrastructure on erp-network
# (erp-sqlserver on port 1433, erp-rabbitmq on port 5672)
echo "Starting container on port 5249 (using shared erp-network)"
docker run -d -p 5249:5249 --name cash-management-api \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ASPNETCORE_URLS=http://+:5249 \
  -e "ConnectionStrings__DefaultConnection=Data Source=erp-sqlserver,1433;Initial Catalog=CASHDB;User ID=sa;Password=YourStr0ng!Passw0rd;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30" \
  -e RabbitMQ__Host=erp-rabbitmq \
  -e RabbitMQ__Port=5672 \
  -e RabbitMQ__UserName=guest \
  -e RabbitMQ__Password=guest \
  --network erp-network \
  cash-management-api:latest

# Wait for services to become healthy
echo "Waiting for services to become healthy..."
MAX_RETRIES=20
RETRY=0
while [ $RETRY -lt $MAX_RETRIES ]; do
    HEALTH=$(docker inspect --format='{{.State.Health.Status}}' cash-management-api 2>/dev/null)
    if [ "$HEALTH" = "healthy" ]; then
        echo "cash-management-api is healthy!"
        break
    fi
    RETRY=$((RETRY + 1))
    echo "  Attempt $RETRY/$MAX_RETRIES - status: ${HEALTH:-starting}..."
    sleep 10
done

# Check logs
echo ""
echo "Container logs:"
docker logs cash-management-api 2>&1 | tail -20

# Test
echo ""
echo "Testing service at http://localhost:5249/health"
if command -v curl &> /dev/null; then
    curl -s http://localhost:5249/health | python3 -m json.tool 2>/dev/null || curl -s http://localhost:5249/health
    echo ""
else
    echo "curl not found, skipping API check."
fi
