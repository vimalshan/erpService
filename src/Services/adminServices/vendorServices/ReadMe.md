# VendorService Microservice

Vendor management microservice for the ERP system implementing enterprise patterns including CQRS, Domain-Driven Design, GraphQL, and Kubernetes deployment.

## 📋 Table of Contents

- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Build & Run](#build--run)
- [API Endpoints](#api-endpoints)
- [Entity Framework Migrations](#entity-framework-migrations)
- [Docker & Docker Compose](#docker--docker-compose)
- [Kubernetes Deployment](#kubernetes-deployment)
- [Database Setup](#database-setup)

---

## 🏗️ Project Structure

```
src/
├── VendorService.API/              # REST API, GraphQL, Minimal APIs
│   ├── Controllers/                # REST Controllers
│   ├── GraphQL/                    # GraphQL Query/Mutation resolvers
│   ├── MinimalApis/               # Minimal API endpoints
│   ├── Middleware/                # Custom middleware (exception handling, etc.)
│   ├── appsettings.json           # Default configuration
│   ├── appsettings.Development.json
│   └── appsettings.Production.json
├── VendorService.Application/      # Application Layer (CQRS, DTOs)
│   ├── Commands/                   # CQRS Command handlers
│   ├── Queries/                    # CQRS Query handlers
│   ├── DTOs/                       # Data Transfer Objects
│   ├── Mappings/                   # AutoMapper profiles
│   ├── Behaviours/                 # Pipeline behaviours (validation, logging)
│   └── Validators/                 # FluentValidation validators
├── VendorService.Domain/           # Domain Layer (DDD)
│   ├── Entities/                   # Domain entities
│   ├── ValueObjects/               # Value objects
│   ├── Aggregates/                 # Aggregate roots
│   ├── Events/                     # Domain events
│   └── Interfaces/                 # Domain contracts
└── VendorService.Infrastructure/   # Infrastructure Layer
    ├── Repositories/               # Data access repositories
    ├── Data/                       # DbContext and migrations
    ├── Messaging/                  # RabbitMQ messaging
    ├── Resilience/                 # Circuit breaker, retry policies
    └── Storage/                    # Azure Blob storage
```

---

## 📦 Prerequisites

- **SDK**: .NET 10.0 or later
- **Database**: SQL Server 2022 (local or container)
- **Runtime Environment**: Docker & Docker Compose (for containerized setup)
- **Package Manager**: NuGet (bundled with SDK)
- **Tools**:
  - PowerShell 5.1+ or Bash
  - dotnet CLI
  - Visual Studio 2022 or VS Code

### Optional for Kubernetes:
- Docker Desktop with Kubernetes enabled OR Minikube
- kubectl CLI
- Helm (for package management)

---

## 🔨 Build & Run

### Quick Start (Local Development)

#### 1. Restore Dependencies
```powershell
# From project root
cd src/VendorService.API
dotnet restore
cd ../..
```

#### 2. Build Solution
```powershell
# Build all projects
dotnet build

# Or build specific project
dotnet build src/VendorService.API/VendorService.API.csproj
```

> **Troubleshooting MSBuild Error**: If you encounter `MSB1001: Unknown switch`, use:
> ```powershell
> dotnet clean src/VendorService.API/VendorService.API.csproj
> dotnet restore src/VendorService.API/VendorService.API.csproj
> dotnet build src/VendorService.API/VendorService.API.csproj -v minimal
> ```

#### 3. Run the Service
```powershell
# From src/VendorService.API directory
dotnet run --environment Development

# Or with explicit configuration
dotnet run --configuration Release --environment Production
```

The service will start on **http://localhost:5181**

---

## 🔌 API Endpoints

### Health Check
```
GET http://localhost:5181/health
```
**Response**: 200 OK
```json
{
  "status": "Healthy"
}
```

### Swagger UI (REST API Documentation)
```
http://localhost:5181/swagger/index.html
```

**Key REST Endpoints:**
- `GET /api/vendors` - List all vendors
- `GET /api/vendors/{id}` - Get vendor by ID
- `POST /api/vendors` - Create new vendor
- `PUT /api/vendors/{id}` - Update vendor
- `DELETE /api/vendors/{id}` - Delete vendor

**Example:**
```bash
curl -X GET "http://localhost:5181/api/vendors" \
  -H "Content-Type: application/json"
```

### GraphQL Endpoint
```
POST http://localhost:5181/graphql
```

**GraphQL Schema Endpoint**: http://localhost:5181/graphql?sdl

**Sample Query:**
```graphql
{
  vendors {
    id
    name
    email
    address
    liveStatus
  }
}
```

**Sample Mutation:**
```graphql
mutation {
  createVendor(input: {
    catId: 1
    locId: 1
    name: "New Vendor"
    email: "vendor@example.com"
    address: "123 Street"
    liveStatus: "A"
  }) {
    id
    name
    email
  }
}
```

**cURL Example:**
```bash
curl -X POST "http://localhost:5181/graphql" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ vendors { id name email } }"
  }'
```

### Minimal API Endpoints
- `GET /api/v1/vendors` - List vendors (API v1)
- `GET /health/live` - Liveness probe
- `GET /health/ready` - Readiness probe

---

## 📊 Entity Framework Migrations

### Setup Database Context

The VendorService uses Entity Framework Core with SQL Server.

**Connection String** (appsettings.Development.json):
```
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VENDORDB;Integrated Security=True;
```

### Migration Commands

#### 1. Create Initial Migration
```powershell
cd src/VendorService.Infrastructure
dotnet ef migrations add InitialCreate --context VendorDbContext --project ..\VendorService.Infrastructure.csproj
```

#### 2. Update Database (Apply Migrations)
```powershell
dotnet ef database update --context VendorDbContext --project ..\VendorService.Infrastructure.csproj
```

#### 3. Create Stored Procedures
```powershell
# Run SQL scripts (in VENDOR folder)
sqlcmd -S (localdb)\MSSQLLocalDB -d VENDORDB -i ..\..\..\VENDOR\VENDOR-Procedures.sql
```

#### 4. Seed Sample Data
```powershell
# Using EF seed
dotnet ef database update --context VendorDbContext

# Or run SQL script
sqlcmd -S (localdb)\MSSQLLocalDB -d VENDORDB -i ..\..\..\VENDOR\VENDOR-Triggers.sql
```

#### 5. View Migration Script
```powershell
dotnet ef migrations script InitialCreate --context VendorDbContext
```

#### 6. Remove Last Migration (if needed)
```powershell
dotnet ef migrations remove --context VendorDbContext
```

---

## 🐳 Docker & Docker Compose

### Run with Docker Compose

#### 1. Start Services
```bash
# From project root
docker compose up --build

# Or detached mode
docker compose up -d --build
```

**Services Started:**
- **mssql-server** (mssql-vendor-db): SQL Server 2022
- **rabbitmq** (vendor-rabbitmq): RabbitMQ message broker
- **vendor-service**: VendorService API

#### 2. Access Services

| Service         | URL                                  |
|-----------------|-------------------------------------|
| Service Health  | http://localhost:5181/health       |
| Swagger API     | http://localhost:5181/swagger      |
| GraphQL         | http://localhost:5181/graphql      |
| RabbitMQ Mgmt   | http://localhost:15672 (guest/guest) |
| MSSQL Server    | localhost:1433                     |

#### 3. View Logs
```bash
docker compose logs -f vendor-service
docker compose logs -f mssql-server
docker compose logs -f vendor-rabbitmq
```

#### 4. Stop Services
```bash
docker compose down

# Remove volumes (reset database)
docker compose down -v
```

### Build Docker Image

```bash
# Build image
docker build -t vendor-service:latest -f Docker/Dockerfile .

# Run container
docker run -p 5181:5181 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__VendorDb="Server=host.docker.internal;Database=VENDORDB;..." \
  vendor-service:latest
```

---

## ☸️ Kubernetes Deployment

### Prerequisites
```bash
# WSL (Windows Subsystem for Linux)
wsl -d Ubuntu

# Start Minikube
minikube start

# Enable Minikube Docker Registry
kubectl apply -f https://raw.githubusercontent.com/kubernetes/kubernetes/master/cluster/addons/registry/registry-rc.yaml
```

### Deploy to Kubernetes

#### 1. Build and Push Image
```bash
# Configure Docker to use Minikube
eval $(minikube docker-env)

# Build image
docker build -t vendor-service:latest -f Docker/Dockerfile .
```

#### 2. Apply Kubernetes Manifests
```bash
# From k8s directory
kubectl apply -f k8s/

# Or individual files
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secret.yaml
kubectl apply -f k8s/deployment.yaml
kubectl apply -f k8s/service.yaml
kubectl apply -f k8s/ingress.yaml
kubectl apply -f k8s/hpa.yaml
kubectl apply -f k8s/rbac.yaml
```

#### 3. Verify Deployment
```bash
# Check pods
kubectl get pods -n vendor-service

# Check services
kubectl get svc -n vendor-service

# Check deployments
kubectl get deployment -n vendor-service

# Check HPA status
kubectl get hpa -n vendor-service
```

#### 4. Port Forward to Local
```bash
# Forward service port
kubectl port-forward -n vendor-service svc/vendor-service 5181:5181

# Access service
curl http://localhost:5181/health
```

#### 5. View Logs
```bash
# Pod logs
kubectl logs -n vendor-service deployment/vendor-service

# Tail logs
kubectl logs -f -n vendor-service deployment/vendor-service

# Previous logs
kubectl logs --previous -n vendor-service pod/vendor-service-xxx
```

#### 6. Scale Deployment
```bash
# Manual scale
kubectl scale deployment vendor-service -n vendor-service --replicas=5

# HPA will auto-scale between 3-10 replicas based on CPU (70%) and Memory (80%)
```

---

## 💾 Database Setup

### Local Development (LocalDB)

**Connection String:**
```
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VENDORDB;Integrated Security=True;
```

**Create Database:**
```powershell
sqlcmd -S (localdb)\MSSQLLocalDB
> CREATE DATABASE VENDORDB;
> GO
> USE VENDORDB;
> GO
```

### Docker Environment

**Connection String:**
```
Data Source=mssql-server,1433;Initial Catalog=VENDORDB;User ID=sa;Password=YourPassword123!;
```

**Database Initialization:**
- Automatic via `Docker/init-database.sql`
- Runs on container startup via entrypoint script
- Creates tables, stored procedures, triggers, and seeds initial data

### Production (Kubernetes)

**Connection String** (from secret):
```
Server=sql-server.default.svc.cluster.local;Database=VENDORDB;User Id=sa;Password=<from-secret>;
```

**Database Backup:**
```bash
# Backup database
kubectl exec -n vendor-service <pod-name> -- \
  sqlcmd -S localhost -U sa -P $SA_PASSWORD \
  -Q "BACKUP DATABASE VENDORDB TO DISK='/var/opt/mssql/backup/vendordb.bak'"
```

---

## 🚀 Common Tasks

### Run Tests
```powershell
# Unit tests
dotnet test tests/VendorService.UnitTests/VendorService.UnitTests.csproj

# Integration tests
dotnet test tests/VendorService.IntegrationTests/VendorService.IntegrationTests.csproj
```

### Clean Build
```powershell
dotnet clean
dotnet build --configuration Release
```

### Format Code
```powershell
dotnet format src/
```

### Generate API Documentation
```bash
# Swagger JSON
curl http://localhost:5181/swagger/v1/swagger.json > swagger.json

# GraphQL Schema
curl http://localhost:5181/graphql?sdl > schema.graphql
```

---

## 📝 Configuration Reference

### appsettings.json (Development)
```json
{
  "ConnectionStrings": {
    "VendorDb": "Data Source=(localdb)\\MSSQLLocalDB;..."
  },
  "JwtSettings": {
    "SecretKey": "REPLACE_WITH_STRONG_SECRET_KEY_AT_LEAST_32_CHARS",
    "Issuer": "VendorService",
    "Audience": "VendorServiceClients"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "VirtualHost": "/",
    "Username": "guest",
    "Password": "guest"
  }
}
```

### appsettings.Production.json
```json
{
  "ConnectionStrings": {
    "VendorDb": "Data Source=mssql-server,1433;Initial Catalog=VENDORDB;..."
  },
  "JwtSettings": {
    "SecretKey": "SECURE_KEY_FROM_ENV_VARIABLE",
    "Issuer": "VendorService",
    "Audience": "VendorServiceClients"
  },
  "RabbitMQ": {
    "Host": "rabbitmq",
    "Port": 5672,
    "Enabled": true
  },
  "AllowedHosts": "vendor-service.example.com"
}
```

---

## 🐛 Troubleshooting

### MSBuild Error: Unknown Switch
```powershell
dotnet clean --configuration Release
dotnet restore
dotnet build --configuration Release -v minimal
```

### Database Connection Error
```powershell
# Check LocalDB instances
sqllocaldb info

# Start LocalDB if needed
sqllocaldb start mssqllocaldb
```

### GraphQL Not Responding
- Verify service is running: `curl http://localhost:5181/health`
- Check logs: `docker compose logs vendor-service`
- Ensure `HotChocolate.AspNetCore` NuGet package is installed

### Kubernetes Pod Crash Loop
```bash
# Check pod logs
kubectl logs -n vendor-service deployment/vendor-service

# Check pod events
kubectl describe pod -n vendor-service <pod-name>

# Check resource limits
kubectl top pods -n vendor-service
```

---

## 📚 Additional Resources

- [Microsoft Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [HotChocolate GraphQL](https://chillicream.com/docs/chocolatey)
- [Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)
- [CQRS Pattern](https://cqrs.nu/)
- [Domain-Driven Design](https://martinfowler.com/bliki/DDD_Aggregate.html)
- [Kubernetes Best Practices](https://kubernetes.io/docs/concepts/best-practices/)

---

## 📧 Support & Contributing

For issues, feature requests, or contributions, please refer to the main ERP repository documentation.

**Last Updated:** March 10, 2026
