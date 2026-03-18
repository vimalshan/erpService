# Organization Setup Microservice

A comprehensive .NET 10 microservice for managing organization roles, user-role mappings, organization parameters, and trade (PP) limits using Clean Architecture, CQRS, and Domain-Driven Design patterns.

## Project Overview

**Database:** CASHDB on `(localdb)\MSSQLLocalDB`

**Architecture Layers:**
- **Domain**: Pure business logic with entities, value objects, aggregates, and domain events
- **Application**: CQRS with MediatR, DTOs, validators, and behaviors
- **Infrastructure**: EF Core DbContext, repositories, external services (Blob, RabbitMQ)
- **API**: REST controllers, Swagger/OpenAPI, JWT authentication, health checks
- **Functions**: Azure Functions for background tasks (scheduled jobs, event processing)

## Solution Structure

```
OrganizationSetupService/
├── src/
│   ├── OrganizationSetup.Domain/            # Entities, Value Objects, Aggregates, Events
│   ├── OrganizationSetup.Application/       # CQRS Commands/Queries, DTOs, Validators, Behaviors
│   ├── OrganizationSetup.Infrastructure/    # EF Core, Repositories, Services, Migrations
│   ├── OrganizationSetup.API/               # REST Controllers, Swagger, Authentication
│   └── OrganizationSetup.Functions/         # Azure Functions Worker
└── OrganizationSetup/                       # Original SQL schema & documentation
```

## Database Schema

### Tables
1. **DEAL_ROLE** - Role master records
2. **DEAL_USERMAP** - User-to-role mappings within organizations
3. **DEAL_ORGPARAMS** - Organization-specific configuration parameters
4. **DEAL_PPLIMIT** - Provisional Prepayment (PP) limit management

All tables have corresponding indexes for optimized querying.

## Key Features

### ✅ Implemented
- **EF Core Code-First** with auto-generated migrations
- **CQRS Pattern** with MediatR for command/query separation
- **Domain-Driven Design** with aggregates and domain events
- **Fluent Validation** for request validation
- **Auto-mapper** for DTO-to-Entity mapping
- **JWT Authentication & Authorization** at API level
- **Health Checks** for API and Database
- **Unit of Work Pattern** for transactional consistency
- **Four REST API Controllers** with authorization
- **Swagger/OpenAPI** documentation
- **Seed Data Scripts** for development

### 🔄 To Be Completed
- RabbitMQ message consumers and publishers
- Azure Functions for background tasks (PP limit alerts, role audits)
- Azure Blob Storage integration for PP certificates
- Entity Framework event publishing/dispatching
- GraphQL endpoint (Hot Chocolate configured)
- Circuit Breaker policies (Polly configured)
- Advanced caching strategies

## API Endpoints

### Roles (`/api/roles`)
- `GET /api/roles` - Get all roles
- `GET /api/roles/{roleId}` - Get role by ID
- `POST /api/roles` - Create new role

### User Maps (`/api/usermaps`)
- `GET /api/usermaps/org/{orgId}` - Get users in organization
- `GET /api/usermaps/employee/{empSysId}` - Get roles for employee
- `POST /api/usermaps` - Map user to role

### Organization Parameters (`/api/orgparams`)
- `GET /api/orgparams/org/{orgId}` - Get all parameters for organization
- `GET /api/orgparams/org/{orgId}/type/{paramType}` - Get specific parameter
- `POST /api/orgparams` - Create parameter
- `PUT /api/orgparams` - Update parameter

### PP Limits (`/api/pplimits`)
- `GET /api/pplimits/{limitId}` - Get limit by ID
- `GET /api/pplimits/org/{orgId}/year/{finYear}` - Get limits by org and year
- `POST /api/pplimits` - Create limit
- `PUT /api/pplimits` - Update limit
- `POST /api/pplimits/{limitId}/certificate` - Upload PP certificate

### Health Checks
- `GET /health` - API and database health status

## Authentication & Authorization

**JWT Bearer Token required for all endpoints (except health check)**

Configuration in `appsettings.json`:
```json
{
  "Jwt": {
    "Secret": "YourSuperSecretKey...",
    "Issuer": "OrganizationSetupAPI",
    "Audience": "OrganizationSetupClients",
    "ExpirationMinutes": 60
  }
}
```

## Getting Started

### 1. Database Setup

Apply EF Core migrations to create schema:

```bash
dotnet ef database update -p src/OrganizationSetup.Infrastructure -s src/OrganizationSetup.API
```

Or manually run the SQL schema:
```bash
sqlcmd -S (localdb)\MSSQLLocalDB -d CASHDB -i OrganizationSetup\05-OrganizationSetup_Create_Schema.sql
```

### 2. Run the API

```bash
dotnet run --project src/OrganizationSetup.API
```

**Swagger UI**: https://localhost:7xxx/swagger

### 3. Generate JWT Token

Create a token using a third-party tool or integrate a token endpoint. Claims should include:
- `sub` / `NameIdentifier` - User ID (long)
- `organizationId` - Organization ID (long)
- `role` - User roles (can be multiple)

## Configuration

**appsettings.json** includes:
- Database connection string
- JWT settings
- RabbitMQ configuration (not yet implemented)
- Azure Blob Storage connection (optional)
- Logging levels

## NuGet Packages

**Core**: MediatR, FluentValidation, AutoMapper  
**Database**: Entity Framework Core, SQL Server provider, Dapper  
**API**: JWT Bearer authentication, Swashbuckle/Swagger  
**Cloud**: Azure Blob Storage, Azure Functions Worker  
**Messaging**: RabbitMQ.Client  
**Resilience**: Polly (circuit breaker patterns)

## Development Notes

### Domain Events
Entities raise domain events when state changes. Currently cleared after SaveChanges but not dispatched. Integration with MediatR in Infrastructure layer is pending.

### Error Handling
Validation errors throw FluentValidation exceptions via the ValidationBehavior pipeline. Custom exception handlers should be added as middleware.

### Transactions
UnitOfWork supports explicit transaction control:
```csharp
await _unitOfWork.BeginTransactionAsync();
try
{
    // ... operations
    await _unitOfWork.CommitAsync();
}
catch
{
    await _unitOfWork.RollbackAsync();
}
```

## Testing

Currently no unit tests included. Recommended:
- Repository pattern tests with in-memory DbContext
- CQRS handler tests with Moq
- Controller integration tests with WebApplicationFactory
- Domain entity tests for business logic

## Deployment

### Docker
Create a Dockerfile for containerized deployment with multi-stage builds.

### Azure
- Deploy API to App Service or Container Instances
- Database to Azure SQL Database  
- Functions to Azure Functions
- Certificates storage in Blob Storage

## Next Steps

1. Implement message consumers for RabbitMQ
2. Create Azure Functions for background tasks
3. Add comprehensive error handling and logging
4. Integrate domain event publishing
5. Add unit and integration tests
6. Configure GraphQL endpoint
7. Implement Polly circuit breaker policies
8. Add data seeding for development environment
9. Implement audit logging for all entity changes
10. Add API rate limiting and throttling

## License

Internal ERP System - Organization Setup Module

---

**Created**: March 12, 2026  
**.NET Version**: 10.0  
**Architecture**: Clean Architecture + CQRS + Domain-Driven Design
