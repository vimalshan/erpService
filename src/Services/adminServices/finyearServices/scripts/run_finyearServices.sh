#!/bin/bash

# Stop and remove existing container
echo "Stopping and removing existing container: finyear-api"
docker rm -f finyear-api 2>/dev/null || true

# Rebuild image
echo "Building image: finyear-api (lowercase required for Docker)"
docker build -t finyear-api:latest -f Docker/Dockerfile .

# Run container
echo "Starting container on port 5000"
docker run -d -p 5000:5000 -p 5001:5001 --name finyear-api -e ASPNETCORE_ENVIRONMENT=Production finyear-api:latest

# Wait for startup
echo "Waiting 5 seconds for service to start..."
sleep 5

# Check logs
echo "Container logs:"
docker logs finyear-api

# Test
echo "Testing service at http://localhost:5000/api/FinancialYear"
if command -v curl &> /dev/null; then
    curl -f http://localhost:5000/api/FinancialYear || echo "API check failed or endpoint not ready. Try http://localhost:5000/swagger"
else
    echo "curl not found, skipping API check."
fi
