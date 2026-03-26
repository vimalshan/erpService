#!/bin/bash

# Always run from the script's own directory (where Dockerfile lives)
cd "$(dirname "$0")"

# Stop and remove existing container
echo "Stopping and removing existing container: erp-visitor-service"
docker rm -f erp-visitor-service 2>/dev/null || true

# Rebuild image
echo "Building image: visitor-service"
docker build -t visitor-service:latest -f Dockerfile .

# Ensure the shared network exists (created by docker-compose.shared.yml)
docker network create erp-network 2>/dev/null || true

# Run container
echo "Starting container on port 5018"
docker run -d \
  -p 5018:80 \
  --name erp-visitor-service \
  --network erp-network \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ASPNETCORE_URLS=http://+:80 \
  -e ConnectionStrings__VisitorDb="Server=sqlserver;Database=VISITORDB;User Id=sa;Password=YourStr0ng!Passw0rd;TrustServerCertificate=true;" \
  -e ConnectionStrings__AzureStorage="" \
  -e RabbitMQ__Host=rabbitmq \
  -e RabbitMQ__Port=5672 \
  -e RabbitMQ__Username=guest \
  -e RabbitMQ__Password=guest \
  -e RabbitMQ__VirtualHost=/ \
  -e Jwt__Key=CHANGE_ME_TO_A_STRONG_256BIT_SECRET_KEY_IN_PRODUCTION \
  -e Jwt__Issuer=VisitorServices \
  -e Jwt__Audience=VisitorServices \
  visitor-service:latest

# Wait for startup (migrations can take time)
echo "Waiting 20 seconds for service to start and run migrations..."
sleep 20

# Check logs
echo "Container logs:"
docker logs erp-visitor-service

# Test with retry loop
# 200 = healthy, 503 = service up but database unreachable (expected in standalone mode)
# Both mean the API is reachable and running
echo "Testing service at http://localhost:5018/health"
if command -v curl &> /dev/null; then
    MAX_RETRIES=10
    RETRY_INTERVAL=5
    for i in $(seq 1 $MAX_RETRIES); do
        echo "Attempt $i/$MAX_RETRIES..."
        HTTP_STATUS=$(curl -s -o /tmp/health_response.json -w "%{http_code}" http://localhost:5018/health)
        if [ "$HTTP_STATUS" = "200" ] || [ "$HTTP_STATUS" = "503" ]; then
            echo "Service is responding (HTTP $HTTP_STATUS)."
            cat /tmp/health_response.json
            echo ""
            if [ "$HTTP_STATUS" = "503" ]; then
                echo "Note: Database health check is degraded — this is expected when running standalone without SQL Server."
            fi
            break
        fi
        if [ "$i" -eq "$MAX_RETRIES" ]; then
            echo "Service did not respond after $((MAX_RETRIES * RETRY_INTERVAL))s (last HTTP status: ${HTTP_STATUS:-no response}). Check logs:"
            docker logs --tail 30 erp-visitor-service
        else
            echo "No response yet (HTTP ${HTTP_STATUS:-none}), retrying in ${RETRY_INTERVAL}s..."
            sleep $RETRY_INTERVAL
        fi
    done
else
    echo "curl not found, skipping health check."
fi
