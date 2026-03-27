#!/bin/bash

# Ensure we run from the transactionServices directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR/.." || exit 1

# Stop and remove existing container
echo "Stopping and removing existing container: transaction-processing-api"
docker rm -f transaction-processing-api 2>/dev/null || true

# Rebuild image
echo "Building image: transaction-processing-api"
docker build -t transaction-processing-api:latest -f Docker/Dockerfile .

# Connect to existing shared infrastructure on erp-network
echo "Starting container on port 5100 (using shared erp-network)"
docker run -d -p 5100:5100 --name transaction-processing-api \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ASPNETCORE_URLS=http://+:5100 \
  -e "ConnectionStrings__DefaultConnection=Data Source=erp-sqlserver,1433;Initial Catalog=CASHDB;User ID=sa;Password=YourStr0ng!Passw0rd;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30" \
  -e RabbitMQ__HostName=erp-rabbitmq \
  -e RabbitMQ__Port=5672 \
  -e RabbitMQ__UserName=guest \
  -e RabbitMQ__Password=guest \
  -e RabbitMQ__Enabled=true \
  --network erp-network \
  transaction-processing-api:latest

# Wait for startup
echo "Waiting 10 seconds for service to start..."
sleep 10

# Check logs
echo "Container logs:"
docker logs transaction-processing-api 2>&1 | tail -20

# Test
echo ""
echo "Testing service at http://localhost:5100/health"
if command -v curl &> /dev/null; then
    curl -s http://localhost:5100/health | python3 -m json.tool 2>/dev/null || curl -s http://localhost:5100/health
    echo ""
else
    echo "curl not found, skipping API check."
fi
