# HR Microservices Deployment Guide

This directory contains all deployment configurations for the HR microservices platform.

## Directory Structure

```
deployment/
├── Dockerfiles/           # Individual Dockerfile for each service
├── docker-compose.yml     # Docker Compose configuration for local development
├── k8s/                   # Kubernetes manifests
│   ├── 00-infrastructure.yaml   # Databases, RabbitMQ, ConfigMaps, Secrets
│   ├── 01-api-gateway.yaml      # API Gateway deployment
│   └── 02-microservices.yaml    # All 11 microservices
├── scripts/
│   ├── init-db.sql                    # Database initialization script
│   ├── build-docker-images.ps1        # Build all Docker images
│   ├── deploy-docker-compose.ps1      # Deploy using Docker Compose
│   └── deploy-k8s.ps1                 # Deploy to Kubernetes
└── README.md              # This file
```

## Quick Start

### Prerequisites
- Docker Desktop (or Docker Engine + Docker Compose)
- .NET 10.0 SDK
- PowerShell 5.1+
- (For K8s) kubectl and a working Kubernetes cluster

### Option 1: Docker Compose (Development/Testing)

```powershell
# Build all Docker images
.\scripts\build-docker-images.ps1

# Start all services
.\scripts\deploy-docker-compose.ps1 -Action up -Rebuild -Detached:$true

# Check service status
.\scripts\deploy-docker-compose.ps1 -Action status

# Stop services
.\scripts\deploy-docker-compose.ps1 -Action down
```

#### Service URLs (Docker Compose)
- API Gateway: http://localhost:5310
- AlertsNotifications: http://localhost:5154
- CompensationBenefits: http://localhost:5009
- EmployeeManagement: http://localhost:5004
- EmployeeRelations: http://localhost:5075
- ExitManagement: http://localhost:5094
- OrganizationStructure: http://localhost:5027
- Recruitment: http://localhost:5265
- TimeAttendance: http://localhost:5235
- TrainingDevelopment: http://localhost:5003
- UserSecurity: http://localhost:5140
- EmployeeTransactions: http://localhost:5204

#### RabbitMQ Management UI
- URL: http://localhost:15672
- Username: guest
- Password: guest

#### SQL Server
- Host: localhost,1433
- Username: sa
- Password: SafePassword123!@#
- Driver: ODBC Driver 17 for SQL Server (or higher)

### Option 2: Kubernetes (Production)

```powershell
# Deploy to Kubernetes cluster
.\scripts\deploy-k8s.ps1 -Action deploy

# Check deployment status
.\scripts\deploy-k8s.ps1 -Action status

# View logs
.\scripts\deploy-k8s.ps1 -Action logs -Service api-gateway

# Port forward to API Gateway
.\scripts\deploy-k8s.ps1 -Action port-forward -Service api-gateway

# Delete deployment
.\scripts\deploy-k8s.ps1 -Action delete
```

## Configuration

### Environment Variables

All services use environment variables for configuration, typically set via:

1. **Docker Compose**: Defined in `docker-compose.yml`
2. **Kubernetes**: Defined in ConfigMaps and Secrets in `k8s/00-infrastructure.yaml`
3. **Local Development**: Use `appsettings.json` or `appsettings.Development.json`
4. **Production**: Use `appsettings.Production.json` with environment variable overrides

### Key Configuration Areas

#### Database Connections
- SQL Server: `Server=sql-server;Database=<ServiceDB>;User=sa;Password=<PASSWORD>;TrustServerCertificate=True`
- Each service gets its own database

#### RabbitMQ
- Host: `rabbitmq` (Docker Compose) or `rabbitmq` (Kubernetes)
- Port: `5672` (AMQP)
- Management UI: `15672`
- Default Credentials: guest/guest

#### JWT Settings
- Each service has its own JWT secret key
- Gateway JWT secret: `SuperSecureGatewaySecretKey2026$#@!`
- Service JWT keys: Check `appsettings.Production.json` in each service

### Security Considerations

⚠️ **IMPORTANT FOR PRODUCTION:**

1. **Secrets Management**
   - Replace hardcoded secrets with Azure Key Vault or HashiCorp Vault
   - Update `SA_PASSWORD`, JWT secrets, RabbitMQ credentials
   - Use Kubernetes Secrets instead of stringData

2. **SSL/TLS**
   - Enable HTTPS on all services
   - Use cert-manager for Kubernetes TLS certificates
   - Update Ingress configuration in `01-api-gateway.yaml`

3. **Database Hardening**
   - Change default SQL Server password
   - Implement SQL Server authentication best practices
   - Use managed identities in Azure

4. **RabbitMQ Security**
   - Change default guest credentials
   - Enable authentication and authorization
   - Use TLS for AMQP connections

5. **Network Policies**
   - Implement Kubernetes NetworkPolicies
   - Restrict inter-service communication
   - Isolate database and message broker access

## Deployment Scripts

### build-docker-images.ps1

Builds Docker images for all services.

```powershell
# Build all services
.\scripts\build-docker-images.ps1 -ImageTag v1.0

# Build specific service
.\scripts\build-docker-images.ps1 -Service TrainingDevelopment -ImageTag v1.0

# Push to registry
.\scripts\build-docker-images.ps1 -ImageTag v1.0 -Registry myregistry.azurecr.io -Push

# Rebuild without cache
.\scripts\build-docker-images.ps1 -NoCache
```

### deploy-docker-compose.ps1

Manages Docker Compose deployments.

```powershell
# Start services (detached)
.\scripts\deploy-docker-compose.ps1 -Action up -Detached:$true

# Start with rebuild
.\scripts\deploy-docker-compose.ps1 -Action up -Rebuild

# View logs (follow mode)
.\scripts\deploy-docker-compose.ps1 -Action logs

# Restart specific service
.\scripts\deploy-docker-compose.ps1 -Action restart -Service training-service

# Check health status
.\scripts\deploy-docker-compose.ps1 -Action status

# Stop services
.\scripts\deploy-docker-compose.ps1 -Action down
```

### deploy-k8s.ps1

Manages Kubernetes deployments.

```powershell
# Deploy all services
.\scripts\deploy-k8s.ps1 -Action deploy

# Check status
.\scripts\deploy-k8s.ps1 -Action status

# View logs
.\scripts\deploy-k8s.ps1 -Action logs -Service api-gateway

# Port forward
.\scripts\deploy-k8s.ps1 -Action port-forward -Service api-gateway

# Restart deployment
.\scripts\deploy-k8s.ps1 -Action restart -Service training-service

# Scale deployment
.\scripts\deploy-k8s.ps1 -Action scale -Service api-gateway

# Delete deployment
.\scripts\deploy-k8s.ps1 -Action delete
```

## Database Migration

The services include Entity Framework Core migrations. Apply migrations:

```powershell
# From service directory, using dotnet CLI
cd <service>/src/<Service>.API

# Apply migrations to production database
dotnet ef database update --configuration Release

# Or via Docker Compose init script (automatic)
# The init-db.sql is automatically run during container startup
```

## Monitoring and Debugging

### Docker Compose

```powershell
# View logs
docker-compose -f deployment/docker-compose.yml logs -f api-gateway

# Execute command in container
docker-compose -f deployment/docker-compose.yml exec training-service dotnet --help

# Get container status
docker-compose -f deployment/docker-compose.yml ps
```

### Kubernetes

```powershell
# View logs
kubectl logs deployment/api-gateway -n hr-microservices -f

# Execute command in pod
kubectl exec -it <pod-name> -n hr-microservices -- /bin/sh

# Describe deployment
kubectl describe deployment api-gateway -n hr-microservices

# View events
kubectl get events -n hr-microservices --sort-by='.lastTimestamp'
```

## Health Check Endpoints

Each service exposes:
- `/health` - Health check status
- `/health/live` - Liveness probe
- `/health/ready` - Readiness probe

Gateway also exposes:
- `/metrics` - Aggregated metrics
- `/gateway/auth/login` - JWT token endpoint

## Common Issues

### Services not starting
- Check logs: `deploy-docker-compose.ps1 -Action logs`
- Verify SQL Server is ready (30 second startup delay)
- Check port availability

### Database connection failures
- Verify SQL Server container is healthy
- Check connection string in environment variables
- Ensure sa password matches in all configs

### RabbitMQ connection issues
- Verify RabbitMQ container is running
- Check default credentials (guest/guest)
- Verify network connectivity between services

## Production Checklist

- [ ] Update all default passwords
- [ ] Configure Azure Key Vault for secrets
- [ ] Enable SSL/TLS on all endpoints
- [ ] Set up proper logging and monitoring
- [ ] Configure database backups
- [ ] Implement CI/CD pipeline
- [ ] Set resource limits and requests
- [ ] Configure autoscaling policies
- [ ] Set up ingress and load balancing
- [ ] Test disaster recovery procedures
- [ ] Review and test security policies
- [ ] Configure alerting and notifications

## References

- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [Kubernetes Documentation](https://kubernetes.io/docs/)
- [YARP Documentation](https://microsoft.github.io/reverse-proxy/)
- [Entity Framework Core Docs](https://learn.microsoft.com/en-us/ef/core/)
- [RabbitMQ Management Plugin](https://www.rabbitmq.com/management.html)

## Support

For issues or questions about deployment:
1. Check logs in respective containers
2. Review configuration in appsettings files
3. Verify service dependencies (SQL Server, RabbitMQ)
4. Check network connectivity between services
5. Review Kubernetes events and pod descriptions
