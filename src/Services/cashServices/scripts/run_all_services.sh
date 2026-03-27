#!/bin/bash
# ============================================================================
# Cash Services - Build and Run All Services
# ============================================================================

set -e

echo "============================================"
echo "  Cash Services - Full Stack Deployment"
echo "============================================"

# Create shared network if not exists
docker network create cashservices-network 2>/dev/null || true

# Step 1: Start infrastructure
echo ""
echo ">>> Step 1: Starting shared infrastructure (MSSQL, RabbitMQ, Azurite)..."
docker-compose -f docker-compose.shared.yml up -d

echo "Waiting 30 seconds for infrastructure to be healthy..."
sleep 30

# Step 2: Build and start all services
echo ""
echo ">>> Step 2: Building and starting all microservices..."
docker-compose -f docker-compose.shared.yml -f docker-compose.yml up -d --build

echo "Waiting 15 seconds for services to start..."
sleep 15

# Step 3: Health checks
echo ""
echo "============================================"
echo "  Service Health Checks"
echo "============================================"

services=(
  "API Gateway:5000"
  "Organization Setup:5099"
  "Currency Management:5031"
  "Deal Ticketing:5081"
  "Loan Management:5268"
  "Cash Management:5249"
  "Email Notification:5032"
  "Transaction Processing:5100"
)

for svc in "${services[@]}"; do
  name="${svc%%:*}"
  port="${svc##*:}"
  if curl -sf "http://localhost:${port}/health" > /dev/null 2>&1; then
    echo "  ✓ ${name} (port ${port}) - HEALTHY"
  else
    echo "  ✗ ${name} (port ${port}) - UNHEALTHY or starting..."
  fi
done

echo ""
echo "============================================"
echo "  Infrastructure"
echo "============================================"
echo "  MSSQL Server:    localhost:1433"
echo "  RabbitMQ AMQP:   localhost:5672"
echo "  RabbitMQ Mgmt:   http://localhost:15672"
echo "  Azurite Blob:    localhost:10000"
echo ""
echo "  API Gateway:     http://localhost:5000"
echo "  Swagger URLs:"
echo "    http://localhost:5099/swagger  (Organization Setup)"
echo "    http://localhost:5031/swagger  (Currency Management)"
echo "    http://localhost:5081/swagger  (Deal Ticketing)"
echo "    http://localhost:5268/swagger  (Loan Management)"
echo "    http://localhost:5249/swagger  (Cash Management)"
echo "    http://localhost:5032/swagger  (Email Notification)"
echo "    http://localhost:5100/swagger  (Transaction Processing)"
echo "============================================"
