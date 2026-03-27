#!/bin/bash
# ============================================================================
# Cash Services - Stop All Services
# ============================================================================

echo "Stopping all Cash Services..."
docker-compose -f docker-compose.shared.yml -f docker-compose.yml down

echo "Removing network..."
docker network rm cashservices-network 2>/dev/null || true

echo "All services stopped."
