#!/bin/bash

# Always run from the script's own directory (where Dockerfile lives)
cd "$(dirname "$0")"

# Stop and remove existing container
echo "Stopping and removing existing container: erp-aimstransaction-service"
docker rm -f erp-aimstransaction-service 2>/dev/null || true

# Rebuild image
echo "Building image: aimstransaction-service"
docker build -t aimstransaction-service:latest -f Dockerfile .

# Ensure the shared network exists (created by docker-compose.shared.yml)
docker network create erp-network 2>/dev/null || true

# Run container
echo "Starting container on port 5019"
docker run -d \
  -p 5019:80 \
  --name erp-aimstransaction-service \
  --network erp-network \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ASPNETCORE_URLS=http://+:80 \
  -e ConnectionStrings__AimsTransactionDb="Server=sqlserver;Database=AIMSDB;User Id=sa;Password=YourStr0ng!Passw0rd;TrustServerCertificate=true;" \
  -e ConnectionStrings__AzureStorage="" \
  -e RabbitMQ__Host=rabbitmq \
  -e RabbitMQ__Port=5672 \
  -e RabbitMQ__Username=guest \
  -e RabbitMQ__Password=guest \
  -e RabbitMQ__VirtualHost=/ \
  -e Jwt__Key=CHANGE_ME_TO_A_STRONG_256BIT_SECRET_KEY_IN_PRODUCTION \
  -e Jwt__Issuer=AimsTransactionService \
  -e Jwt__Audience=AimsTransactionService \
  aimstransaction-service:latest

# Wait for startup (migrations can take time)
echo "Waiting 20 seconds for service to start and run migrations..."
sleep 20
