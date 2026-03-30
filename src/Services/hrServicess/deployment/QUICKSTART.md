# Quick Start Guide - HR Microservices Deployment

## 🚀 Start Here

### Prerequisites
- Docker Desktop (with Docker Compose) OR Kubernetes cluster
- PowerShell 5.1+
- 30+ GB disk space (for all Docker images)
- 8+ GB RAM recommended

## Option A: Local Development (Docker Compose) - 5 minutes

```powershell
# 1. Navigate to deployment directory
cd .\deployment\scripts

# 2. Build all Docker images (takes 10-15 minutes first time)
.\build-docker-images.ps1 -ImageTag v1.0

# 3. Start all services
.\deploy-docker-compose.ps1 -Action up -Rebuild -Detached:$true

# 4. Wait for services to be healthy (1-2 minutes)
Start-Sleep -Seconds 120

# 5. Check status
.\deploy-docker-compose.ps1 -Action status

# 6. Test API Gateway
$response = Invoke-RestMethod -Uri "http://localhost:5310/" -Method Get
$response | ConvertTo-Json
```

**Expected Output:**
```
200 OK
{
  "name": "HR API Gateway",
  "proxy": "YARP",
  "version": "v1"
}
```

### Service URLs (Docker Compose)
| Service | URL | Port |
|---------|-----|------|
| API Gateway | http://localhost:5310 | 5310 |
| Training | http://localhost:5003 | 5003 |
| Employee Management | http://localhost:5004 | 5004 |
| Compensation | http://localhost:5009 | 5009 |
| Alerts | http://localhost:5154 | 5154 |
| User Security | http://localhost:5140 | 5140 |
| Employee Transactions | http://localhost:5204 | 5204 |
| Employee Relations | http://localhost:5075 | 5075 |
| Organization | http://localhost:5027 | 5027 |
| Recruitment | http://localhost:5265 | 5265 |
| Time Attendance | http://localhost:5235 | 5235 |
| Exit Management | http://localhost:5094 | 5094 |
| RabbitMQ UI | http://localhost:15672 | 15672 |
| SQL Server | localhost:1433 | 1433 |

## Option B: Production (Kubernetes) - 10 minutes

```powershell
# 1. Navigate to deployment directory
cd .\deployment\scripts

# 2. Deploy to Kubernetes cluster
.\deploy-k8s.ps1 -Action deploy -Namespace hr-microservices

# 3. Wait for deployments (2-5 minutes)
.\deploy-k8s.ps1 -Action status

# 4. Get API Gateway address
kubectl get svc api-gateway -n hr-microservices

# 5. Port forward to API Gateway (in separate PowerShell window)
.\deploy-k8s.ps1 -Action port-forward -Service api-gateway

# 6. Test API Gateway
$response = Invoke-RestMethod -Uri "http://localhost:8080/" -Method Get
$response | ConvertTo-Json
```

## 📊 Important Credentials

| Component | Username | Password | URL |
|-----------|----------|----------|-----|
| SQL Server | sa | SafePassword123!@# | Server=sql-server;1433 |
| RabbitMQ | guest | guest | http://localhost:15672 |
| API Gateway Auth | admin | admin123 | POST /gateway/auth/login |

⚠️ **Change these in production!**

## 🔧 Common Commands

### Docker Compose Commands

```powershell
# View logs
.\deploy-docker-compose.ps1 -Action logs -Service training-service

# Restart a service
.\deploy-docker-compose.ps1 -Action restart -Service training-service

# Stop all services
.\deploy-docker-compose.ps1 -Action down

# Rebuild and restart
.\deploy-docker-compose.ps1 -Action up -Rebuild -Detached:$true
```

### Kubernetes Commands

```powershell
# View pod logs
.\deploy-k8s.ps1 -Action logs -Service api-gateway

# Restart a deployment
.\deploy-k8s.ps1 -Action restart -Service api-gateway

# Scale a service
.\deploy-k8s.ps1 -Action scale -Service api-gateway

# Access pod shell
.\deploy-k8s.ps1 -Action shell -Service api-gateway

# Get detailed deployment info
.\deploy-k8s.ps1 -Action describe -Service api-gateway

# Delete all deployments
.\deploy-k8s.ps1 -Action delete
```

## 🧪 Test API Gateway

```powershell
# 1. Get JWT token
$login = Invoke-RestMethod -Uri "http://localhost:5310/gateway/auth/login" `
  -Method Post `
  -ContentType "application/json" `
  -Body (@{username='admin';password='admin123'} | ConvertTo-Json)

$token = $login.accessToken

# 2. Call proxied service
$headers = @{ Authorization = "Bearer $token" }
$response = Invoke-RestMethod -Uri "http://localhost:5310/training/health" `
  -Headers $headers `
  -Method Get

# 3. Check gateway metrics
$metrics = Invoke-RestMethod -Uri "http://localhost:5310/metrics" -Method Get
$metrics | ConvertTo-Json
```

## 📋 Logs and Debugging

### Docker Compose Logs

```powershell
# All services
.\deploy-docker-compose.ps1 -Action logs

# Specific service (follow mode)
.\deploy-docker-compose.ps1 -Action logs -Service api-gateway

# SQL Server logs
docker logs hr-sql-server -f

# RabbitMQ logs
docker logs hr-rabbitmq -f
```

### Kubernetes Logs

```powershell
# Pod logs
.\deploy-k8s.ps1 -Action logs -Service api-gateway

# Previous pod logs (if crashed)
kubectl logs deployment/api-gateway -n hr-microservices --previous

# Events
kubectl get events -n hr-microservices --sort-by='.lastTimestamp'

# Pod status
kubectl get pods -n hr-microservices -o wide
```

## 🔌 Database Access

### SQL Server (Docker Compose)
```sql
Server: localhost,1433
Database: Use each service's database
Username: sa
Password: SafePassword123!@#
Connection String: Server=localhost,1433;Database=TrainingDevelopmentDB;User Id=sa;Password=SafePassword123!@#;TrustServerCertificate=True
```

### Available Databases
- AlertsNotificationsDB
- CompensationBenefitsDB
- EmployeeManagementDB
- EmployeeRelationsDB
- ExitManagementDB
- OrganizationStructureDB
- RecruitmentDB
- TimeAttendanceDB
- TrainingDevelopmentDB
- UserSecurityDB
- EmployeeTransactionsDB

## 🛑 Stop and Clean Up

### Docker Compose
```powershell
# Stop all services (preserves volumes)
.\deploy-docker-compose.ps1 -Action down

# Remove everything including volumes
docker-compose -f .\docker-compose.yml down -v
```

### Kubernetes
```powershell
# Delete all deployments
.\deploy-k8s.ps1 -Action delete -Namespace hr-microservices

# Delete namespace
kubectl delete namespace hr-microservices
```

## 📈 Monitor Services

### Health Check Status
```powershell
# Docker Compose
.\deploy-docker-compose.ps1 -Action status

# Kubernetes
.\deploy-k8s.ps1 -Action status
```

### Check Individual Endpoints
```powershell
# Gateway health
Invoke-RestMethod -Uri "http://localhost:5310/health"

# Service health
Invoke-RestMethod -Uri "http://localhost:5003/health"  # Training

# Gateway metrics
Invoke-RestMethod -Uri "http://localhost:5310/metrics"
```

## 🐛 Troubleshooting

### Services won't start
1. Check ports are available: `netstat -ano | findstr :5310`
2. Check logs: `.\deploy-docker-compose.ps1 -Action logs`
3. Verify SQL Server is healthy: `.\deploy-docker-compose.ps1 -Action status`
4. Wait 30 seconds for SQL Server to initialize

### Database connection errors
1. Verify SQL Server is running: `docker ps | grep mssql`
2. Check connection string in appsettings.json
3. Verify databases exist: Use SQL Server Management Studio to connect

### RabbitMQ connection errors
1. Verify RabbitMQ is running: `docker ps | grep rabbitmq`
2. Check credentials in configuration
3. Access management UI: http://localhost:15672 (guest/guest)

### Kubernetes pod not starting
1. Check pod status: `kubectl get pods -n hr-microservices`
2. Check pod logs: `.\deploy-k8s.ps1 -Action logs -Service <service-name>`
3. Check events: `kubectl describe pod <pod-name> -n hr-microservices`
4. Check resource availability: `kubectl top nodes`

## 📚 Detailed Documentation

For complete setup, configuration, and troubleshooting:
- See [deployment/README.md](./README.md)
- See [deployment/DEPLOYMENT_VERIFICATION.md](./DEPLOYMENT_VERIFICATION.md)

## 🔐 Security Reminders

⚠️ **BEFORE PRODUCTION:**
1. Change all default passwords
2. Update JWT secrets to strong random values
3. Enable HTTPS/TLS on all endpoints
4. Move secrets to Azure Key Vault
5. Configure authentication and authorization
6. Set up network security policies
7. Enable database encryption
8. Configure backup and disaster recovery
9. Set up monitoring and alerting
10. Test security controls

## 🎯 Next Steps

1. ✅ Run `build-docker-images.ps1` to build images
2. ✅ Run `deploy-docker-compose.ps1 -Action up` to start services
3. ✅ Test API Gateway at http://localhost:5310
4. ✅ Review [deployment/README.md](./README.md) for detailed configuration
5. ✅ For production, follow security checklist above

---

**Need Help?** See detailed guides:
- [Deployment README](./README.md)
- [Verification Report](./DEPLOYMENT_VERIFICATION.md)
- [Script Documentation](./README.md#deployment-scripts)
