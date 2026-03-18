
# AuthProvider Service

A comprehensive Authentication & Authorization microservice implementing modern architectural patterns including API Versioning, CQRS, GraphQL, Domain-Driven Design, and Swagger/OpenAPI documentation.

## Architecture & Patterns

- **API Gateway Pattern** - Centralized entry point for authentication
- **GraphQL** - Types, Queries, Mutations, Subscriptions support via HotChocolate
- **CORS** - Cross-Origin Resource Sharing for multiple clients
- **Minimal APIs** - Lightweight REST endpoints
- **API Versioning** - Version 1.0 and 2.0 endpoints with URL-based versioning
- **CQRS** - Command Query Responsibility Segregation using MediatR
- **RabbitMQ & Message Queues** - Async messaging for event-driven architecture
- **Resilience Patterns** - Circuit Breaker, Retry, and Timeout policies
- **Swagger/OpenAPI** - Comprehensive API documentation
- **Custom Middleware** - Error handling, correlation IDs, request logging
- **Domain-Driven Design** - Entities, Value Objects, Aggregates, Repositories
- **Entity Framework Core** - ORM for data persistence
- **Repository & Unit of Work** - Data access layer patterns
- **Dapper** - High-performance SQL data access
- **Authentication** - JWT Bearer token authentication
- **Authorization** - Role-based access control (RBAC)

## Quick Start

### 1. Build
```bash
cd src
dotnet restore
dotnet build AuthProvider.API/AuthProvider.API.csproj -c Release
```

### 2. Run Locally
```bash
cd src/AuthProvider.API
dotnet run --environment Development
```

Service starts on **http://localhost:5182**

### 3. Access the APIs

**Swagger UI**: http://localhost:5182/swagger/index.html
- v1 Spec: http://localhost:5182/swagger/v1/swagger.json
- v2 Spec: http://localhost:5182/swagger/v2/swagger.json

**GraphQL**: http://localhost:5182/graphql

## API Endpoints

### Authentication
```bash
# Register
POST /api/v1/auth/register
{ "username": "john", "email": "john@example.com", "password": "Pass!123", "firstName": "John", "lastName": "Doe" }

# Login
POST /api/v1/auth/login
{ "email": "john@example.com", "password": "Pass!123" }

# Get Current User
GET /api/v1/auth/me
Authorization: Bearer {jwt_token}

# Verify Token
POST /api/v1/auth/verify
Authorization: Bearer {jwt_token}

# Refresh Token
POST /api/v1/auth/refresh
{ "refreshToken": "{refresh_token}" }
```

### Users
```bash
# List Users
GET /api/v1/users
Authorization: Bearer {jwt_token}

# Get User
GET /api/v1/users/{id}
Authorization: Bearer {jwt_token}

# Update User
PUT /api/v1/users/{id}
Authorization: Bearer {jwt_token}

# Delete User
DELETE /api/v1/users/{id}
Authorization: Bearer {jwt_token}
```

### Health Check
```bash
GET /api/v1/minimal/auth/health
GET /api/v2/minimal/auth/health
```

## GraphQL Examples

### Query Users
```graphql
query {
  users {
    id
    username
    email
    firstName
    lastName
  }
}
```

### Login
```graphql
mutation {
  login(email: "john@example.com", password: "Pass!123") {
    accessToken
    refreshToken
    user {
      id
      username
    }
  }
}
```

### Register
```graphql
mutation {
  register(
    username: "jane"
    email: "jane@example.com"
    password: "Pass!456"
    firstName: "Jane"
    lastName: "Doe"
  ) {
    id
    username
    email
  }
}
```

## Database Setup

### Connection String (LocalDB)
```
Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Database=AuthProviderDb;
```

### Apply Migrations
```bash
cd src/AuthProvider.API
dotnet ef migrations add InitialCreate -p ../AuthProvider.Infrastructure
dotnet ef database update
```

## Docker Deployment

### Build
```bash
docker build -f Docker/Dockerfile -t authprovider:latest .
```

### Run
```bash
docker-compose up -d
```

**Services**:
- Database: localhost:1433
- API: localhost:5182
- RabbitMQ: localhost:5672

## Kubernetes Deployment

```bash
# Start Minikube
wsl -d Ubuntu -e bash -c "minikube start"

# Deploy
kubectl apply -f k8s/

# Check status
kubectl get svc -n authprovider

# Port forward
kubectl port-forward -n authprovider svc/authprovider-service 5182:5182

# View logs
kubectl logs -n authprovider <pod-name> -f
```

## Configuration

### Development (appsettings.Development.json)
```json
{
  "ConnectionStrings": {
    "AuthDb": "Server=(localdb)\\MSSQLLocalDB;Database=AuthProviderDb;Integrated Security=true;"
  },
  "RabbitMQ": {
    "Enabled": false
  },
  "Jwt": {
    "Secret": "your-dev-secret-key-minimum-32-characters-long!",
    "Issuer": "AuthProvider",
    "Audience": "AuthProvider-API",
    "ExpirationMinutes": 60
  }
}
```

## Troubleshooting

### Swagger Definition Error
Ensure `OpenApiInfo` has valid `Version` field. Now fixed with proper OpenAPI 3.0.1 spec.

### RabbitMQ Connection Failed
Set `"RabbitMQ": { "Enabled": false }` to use InMemory transport.

### Database Migration Failed
```bash
sqllocaldb start MSSQLLocalDB
sqllocaldb info MSSQLLocalDB
```

### Port Already in Use
```bash
netstat -ano | findstr :5182
taskkill /PID <pid> /F
```

## Related Services

- **VendorService**: Vendor management (Port 5181)
- **API Gateway**: Central routing

## Project Structure

```
src/
├── AuthProvider.API/          # Controllers, Middleware
├── AuthProvider.Application/  # CQRS, Commands, Queries
├── AuthProvider.Domain/       # Entities, Interfaces
└── AuthProvider.Infrastructure/ # EF Core, Repositories

k8s/                           # Kubernetes manifests
Docker/                        # Container config
tests/                         # Unit & Integration tests
```
