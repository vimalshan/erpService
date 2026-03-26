#!/bin/bash

# Always run from the script's own directory (where Dockerfile lives)
cd "$(dirname "$0")"

# Stop and remove existing container
echo "Stopping and removing existing container: erp-calendar-service"
docker rm -f erp-calendar-service 2>/dev/null || true

# Rebuild image
echo "Building image: calendar-service (lowercase required for Docker)"
docker build -t calendar-service:latest -f Dockerfile .

# Ensure the shared network exists (created by docker-compose.shared.yml)
docker network create erp-network 2>/dev/null || true

# Run container
echo "Starting container on port 5013"
docker run -d \
  -p 5013:80 \
  --name erp-calendar-service \
  --network erp-network \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ASPNETCORE_URLS=http://+:80 \
  -e ConnectionStrings__CalendarDb="Server=sqlserver;Database=CALENDARDB;User Id=sa;Password=YourStr0ng!Passw0rd;TrustServerCertificate=true;" \
  -e RabbitMQ__Host=rabbitmq \
  -e RabbitMQ__Port=5672 \
  -e RabbitMQ__VirtualHost=/ \
  -e RabbitMQ__Username=guest \
  -e RabbitMQ__Password=guest \
  -e RabbitMQ__CalendarCreatedQueue=calendar-created \
  -e RabbitMQ__HolidayCreatedQueue=holiday-created \
  -e RabbitMQ__ShiftCreatedQueue=shift-created \
  -e Jwt__Key=CalendarServiceSuperSecretKey_ChangeInProd_256bit! \
  -e Jwt__Issuer=CalendarService \
  -e Jwt__Audience=CalendarService \
  calendar-service:latest

# Wait for startup (migrations can take time)
echo "Waiting 20 seconds for service to start and run migrations..."
sleep 20

# Check logs
echo "Container logs:"
docker logs erp-calendar-service

# Test with retry loop
# 200 = healthy, 503 = service up but some optional checks (AzureBlob/RabbitMQ) degraded
# Both mean the API is reachable and running
echo "Testing service at http://localhost:5013/health"
if command -v curl &> /dev/null; then
    MAX_RETRIES=10
    RETRY_INTERVAL=5
    for i in $(seq 1 $MAX_RETRIES); do
        echo "Attempt $i/$MAX_RETRIES..."
        HTTP_STATUS=$(curl -s -o /tmp/health_response.json -w "%{http_code}" http://localhost:5013/health)
        if [ "$HTTP_STATUS" = "200" ] || [ "$HTTP_STATUS" = "503" ]; then
            echo "Service is responding (HTTP $HTTP_STATUS)."
            cat /tmp/health_response.json
            echo ""
            if [ "$HTTP_STATUS" = "503" ]; then
                echo "Note: Some optional health checks (AzureBlob/RabbitMQ) are degraded — this is expected in local dev."
            fi
            break
        fi
        if [ "$i" -eq "$MAX_RETRIES" ]; then
            echo "Service did not respond after $((MAX_RETRIES * RETRY_INTERVAL))s (last HTTP status: ${HTTP_STATUS:-no response}). Check logs:"
            docker logs --tail 30 erp-calendar-service
        else
            echo "No response yet (HTTP ${HTTP_STATUS:-none}), retrying in ${RETRY_INTERVAL}s..."
            sleep $RETRY_INTERVAL
        fi
    done
else
    echo "curl not found, skipping health check."
fi
