#!/bin/bash

# Navigate to the ApiGateway service root (parent of scripts/)
cd "$(dirname "$0")/.." || exit 1

# Stop and remove existing container
echo "Stopping and removing existing container: api-gateway"
docker rm -f api-gateway 2>/dev/null || true

# Rebuild image
echo "Building image: api-gateway"
docker build -t api-gateway:latest -f Docker/Dockerfile .

# Run container
echo "Starting container on port 5000 (using shared erp-network)"
docker run -d -p 5000:5000 --name api-gateway \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ASPNETCORE_URLS=http://+:5000 \
  -e "ConnectionStrings__DefaultConnection=Server=erp-sqlserver;Database=CASHDB;User Id=sa;Password=YourStr0ng!Passw0rd;TrustServerCertificate=True" \
  -e RabbitMQ__HostName=erp-rabbitmq \
  -e RabbitMQ__UserName=guest \
  -e RabbitMQ__Password=guest \
  -e ServiceEndpoints__CashManagement=http://cash-management-api:5249 \
  -e ServiceEndpoints__CurrencyManagement=http://currency-management-api:5031 \
  -e ServiceEndpoints__DealTicketing=http://deal-ticketing-api:5081 \
  -e ServiceEndpoints__LoanManagement=http://loan-management-api:5268 \
  -e ServiceEndpoints__OrganizationSetup=http://organization-setup-api:5099 \
  -e ServiceEndpoints__EmailNotification=http://email-notification-api:5032 \
  -e ReverseProxy__Clusters__cash-cluster__Destinations__cash-primary__Address=http://cash-management-api:5249 \
  -e ReverseProxy__Clusters__currency-cluster__Destinations__currency-primary__Address=http://currency-management-api:5031 \
  -e ReverseProxy__Clusters__deals-cluster__Destinations__deals-primary__Address=http://deal-ticketing-api:5081 \
  -e ReverseProxy__Clusters__loans-cluster__Destinations__loans-primary__Address=http://loan-management-api:5268 \
  -e ReverseProxy__Clusters__organization-cluster__Destinations__organization-primary__Address=http://organization-setup-api:5099 \
  -e ReverseProxy__Clusters__email-cluster__Destinations__email-primary__Address=http://email-notification-api:5032 \
  --network erp-network \
  api-gateway:latest

# Wait for startup
echo "Waiting 10 seconds for service to start..."
sleep 10

# Check logs
echo "Container logs:"
docker logs api-gateway

# Test
echo ""
echo "Testing service at http://localhost:5000/health"
if command -v curl &> /dev/null; then
    STATUS=$(curl -s -o /dev/null -w "%{http_code}" http://localhost:5000/health 2>/dev/null)
    if [ "$STATUS" = "200" ]; then
        echo "Healthy"
    else
        curl -s http://localhost:5000/health 2>/dev/null || echo "Health check failed (HTTP $STATUS)"
    fi
else
    echo "curl not found, skipping API check."
fi
