#!/bin/bash

# Stop and remove existing container
echo "Stopping and removing existing container: auth-provider"
docker rm -f auth-provider 2>/dev/null || true

# Rebuild image
echo "Building image: auth-provider (lowercase required for Docker)"
docker build -t auth-provider:latest -f Docker/Dockerfile .

# Run container
echo "Starting container on port 5200"
docker run -d -p 5200:5200 --name auth-provider -e ASPNETCORE_ENVIRONMENT=Production auth-provider:latest

# Wait for startup
echo "Waiting 5 seconds for service to start..."
sleep 5

# Check logs
echo "Container logs:"
docker logs auth-provider

# Test
echo "Testing health endpoint at http://localhost:5200/api/v1/minimal/auth/health"
if command -v curl &> /dev/null; then
    curl -f http://localhost:5200/api/v1/minimal/auth/health || echo "Health check failed. Try http://localhost:5200/swagger/index.html"
else
    echo "curl not found, skipping health check."
fi

echo ""
echo "Service URLs:"
echo "  Swagger : http://localhost:5200/swagger/index.html"
echo "  GraphQL : http://localhost:5200/graphql"
echo "  Health  : http://localhost:5200/api/v1/minimal/auth/health"
