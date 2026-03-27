#!/bin/bash

# Ensure we run from the emailnotificationServices directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR/.." || exit 1

# Stop and remove existing container
echo "Stopping and removing existing container: email-notification-api"
docker rm -f email-notification-api 2>/dev/null || true

# Rebuild image
echo "Building image: email-notification-api"
docker build -t email-notification-api:latest -f Docker/Dockerfile .

# Connect to existing shared infrastructure on erp-network
echo "Starting container on port 5032 (using shared erp-network)"
docker run -d -p 5032:5032 --name email-notification-api \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ASPNETCORE_URLS=http://+:5032 \
  -e "ConnectionStrings__DefaultConnection=Data Source=erp-sqlserver,1433;Initial Catalog=CASHDB;User ID=sa;Password=YourStr0ng!Passw0rd;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30" \
  -e RabbitMQ__HostName=erp-rabbitmq \
  -e RabbitMQ__Port=5672 \
  -e RabbitMQ__UserName=guest \
  -e RabbitMQ__Password=guest \
  --network erp-network \
  email-notification-api:latest

# Wait for startup
echo "Waiting 10 seconds for service to start..."
sleep 10

# Check logs
echo "Container logs:"
docker logs email-notification-api 2>&1 | tail -20

# Test
echo ""
echo "Testing service at http://localhost:5032/health"
if command -v curl &> /dev/null; then
    curl -s http://localhost:5032/health | python3 -m json.tool 2>/dev/null || curl -s http://localhost:5032/health
    echo ""
else
    echo "curl not found, skipping API check."
fi
