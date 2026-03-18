#!/bin/bash

# Stop and remove existing container
echo "Stopping and removing existing container: location-services"
docker rm -f location-services 2>/dev/null || true

# Rebuild image
echo "Building image: location-services (lowercase required for Docker)"
docker build -t location-services:latest -f Docker/Dockerfile .

# Run container
echo "Starting container on port 7136"
docker run -d -p 7136:7136 --name location-services -e ASPNETCORE_ENVIRONMENT=Production location-services:latest

# Wait for startup
echo "Waiting 5 seconds for service to start..."
sleep 5

# Check logs
echo "Container logs:"
docker logs location-services

# Test
echo "Testing health endpoint at http://localhost:7136/health"
if command -v curl &> /dev/null; then
    curl -f http://localhost:7136/health || echo "Health check failed. Try http://localhost:7136/swagger/index.html"
else
    echo "curl not found, skipping health check."
fi

echo ""
echo "Service URLs:"
echo "  Swagger : http://localhost:7136/swagger/index.html"
echo "  GraphQL : http://localhost:7136/graphql"
echo "  Health  : http://localhost:7136/health"
