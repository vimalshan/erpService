# Reference Service Microservice

A comprehensive .NET 10 microservice built with Domain-Driven Design (DDD) and CQRS patterns for managing reference data (Lists of Values, Permissions, Leave Flags, etc.)

## Overview

The Reference Service provides a scalable, maintainable solution for managing reference data with:
- **Domain-Driven Design**: Clear separation of concerns with aggregates, value objects, and domain events
- **CQRS Pattern**: Segregated command and query operations via MediatR
- **Event-Driven Architecture**: RabbitMQ integration for domain event publishing
- **Multiple API Layers**: REST (Swagger), GraphQL (Banana Cake Pop), and Minimal APIs
- **Advanced Features**: JWT Authentication, Health Checks, Circuit Breaker patterns, Background Functions

## Architecture

### Project Structure

```
src/
├── Domain/ReferenceService.Domain
│   ├── Entities (LovType, LovValue, PermissionRule, LeaveFlag)
│   ├── ValueObjects (EntityStatus, AuditInfo)
│   ├── Events (Domain Events)
│   └── Interfaces (Repository, UnitOfWork, DomainEventPublisher)
│
├── Application/ReferenceService.Application
│   ├── Commands (Create, Update, Delete LOV Types & Values)
│   ├── Queries (Get LOV Types, Values, Permissions)
│   ├── DTOs (Data Transfer Objects)
│   ├── Validators (FluentValidation)
│   ├── Behaviors (MediatR Pipeline Behaviors)
│   └── Mappings (AutoMapper)
│
├── Infrastructure/ReferenceService.Infrastructure
│   ├── Persistence (EF Core DbContext, Migrations)
│   ├── Repositories (Generic & Specific Repository Implementations)
│   ├── DomainEventPublisher (RabbitMQ Implementation)
│   └── Resilience (Polly Circuit Breaker)
│
├── API/ReferenceService.API
│   ├── Controllers (REST API Endpoints)
│   ├── GraphQL (Schema, Types, Resolvers)
│   ├── Auth (JWT Token Service)
│   ├── Middleware (Exception Handling, Logging)
│   └── HealthChecks (Database, API Readiness)
│
└── Functions/ReferenceService.Functions
    └── Azure Functions (Timer-triggered, HTTP-triggered)
```

## Key Technologies

- **.NET 10**: Latest .NET version
- **Entity Framework Core 10**: ORM for database access
- **Dapper**: For complex queries and stored procedure calls
- **MediatR 12.4**: CQRS implementation
- **FluentValidation 11.9**: Input validation
- **AutoMapper 13**: Object-to-object mapping
- **HotChocolate 15**: GraphQL server
- **Swashbuckle 6.9**: Swagger/OpenAPI documentation
- **RabbitMQ.Client 7.1**: Message broker
- **Polly 8.4**: Resilience patterns (Circuit Breaker, Retry)
- **Microsoft.AspNetCore.Authentication.JwtBearer**: JWT authentication
- **Azure Functions Worker**: Serverless background tasks
- **Azure.Storage.Blobs**: Cloud storage for images
- **Health Checks**: Service health monitoring

## Features

### 1. REST API
- **Endpoints**:
  - `GET /api/lovtypes` - Get all LOV Types (paginated)
  - `GET /api/lovtypes/{id}` - Get LOV Type by ID
  - `POST /api/lovtypes` - Create LOV Type
  - `PUT /api/lovtypes/{id}` - Update LOV Type
  - `DELETE /api/lovtypes/{id}` - Deactivate LOV Type
  - `GET /api/lovvalues/by-type/{typeId}` - Get LOV Values by Type
  - `POST /api/lovvalues` - Create LOV Value

- **Documentation**: Swagger UI at `/swagger/index.html`

### 2. GraphQL API
- Available at `/graphql`
- Supports queries and mutations for reference data
- Compatible with Banana Cake Pop explorer
- Schema auto-generated with HotChocolate

### 3. Authentication & Authorization
- **JWT Token Service**
  - Token generation with user claims and roles
  - Token validation and expiration handling
  - Default expiration: 60 minutes
  
- **API Protection**
  - Bearer token authentication
  - Role-based authorization
  - Claim-based authorization

Example token request:
```csharp
POST /api/auth/token
{
  "userId": "user123",
  "email": "user@example.com",
  "roles": ["admin", "manager"]
}
```

### 4. Domain Events
- Auto-published on entity state changes
- RabbitMQ integration for event distribution
- Events:
  - `LovTypeCreatedEvent`
  - `LovTypeUpdatedEvent`
  - `LovTypeDeactivatedEvent`
  - `LovValueCreatedEvent`
  - `LovValueUpdatedEvent`
  - `LovValueDeactivatedEvent`

### 5. Health Checks
- **Endpoints**:
  - `GET /health` - Overall health status
  - `GET /health/ready` - Readiness probe (K8s)
  - `GET /health/live` - Liveness probe (K8s)

- **Health Checks**:
  - Database connectivity
  - API readiness

### 6. Azure Functions
- **DataCleanup** (Timer-triggered, daily 2 AM UTC)
  - Archive old records
  - Clean up temporary data
  
- **SyncData** (HTTP-triggered)
  - Manual data synchronization
  - Endpoint: `POST /api/reference-data/sync`

### 7. Middleware
- **Exception Handling**: Global error response formatting
- **Request/Response Logging**: Detailed logs for debugging
- **CORS**: Cross-Origin Resource Sharing
- **HTTPS Redirection**: Force secure connections

### 8. Entity Framework Migrations
- Auto-applied database schema creation
- Seed data scripts for initial load
- Migration history tracked in `__EFMigrationsHistory`

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server LocalDB or SQL Server 2019+
- RabbitMQ 3.12+ (optional, for event publishing)
- Visual Studio 2022 or VSCode

### Installation

1. **Clone and navigate to project**
```bash
cd src/Services/aimsServices/referenceServies
```

2. **Restore dependencies**
```bash
dotnet restore src/ReferenceService.slnx
```

3. **Update database connection string**
Edit `src/API/ReferenceService.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=REFERENCEDB;Integrated Security=True;"
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest"
  }
}
```

4. **Apply database migrations**
```bash
# From referenceServies directory
.\setup-db.cmd
```

5. **Build solution**
```bash
dotnet build src/ReferenceService.slnx --configuration Release
```

6. **Run API**
```bash
cd src/API/ReferenceService.API
dotnet run
```

API will be available at: `https://localhost:5001`

## API Usage Examples

### Get All LOV Types
```bash
curl -X GET "https://localhost:5001/api/lovtypes?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer {token}" \
  -H "Accept: application/json"
```

Response:
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "items": [
      {
        "id": 1,
        "typeName": "DEPARTMENT",
        "description": "Department Master",
        "sequence": 1,
        "status": "Active",
        "values": [
          {
            "id": 1,
            "typeId": 1,
            "code": "IT",
            "description": "Information Technology"
          }
        ]
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalCount": 3,
    "totalPages": 1,
    "hasPreviousPage": false,
    "hasNextPage": false
  }
}
```

### Create LOV Type
```bash
curl -X POST "https://localhost:5001/api/lovtypes" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "typeName": "COMPANY",
    "description": "Company Master",
    "sequence": 4,
    "modifiedBy": 1
  }'
```

### GraphQL Query Example
```graphql
query {
  getLovTypes(pageNumber: 1, pageSize: 10) {
    items {
      id
      typeName
      description
      values {
        code
        description
      }
    }
  }
}
```

## Database Schema

### Tables
- **LOV_TYPEMAST**: List of Values Type Master
- **LOV_MAST**: List of Values Master
- **PERMISSION_RULES**: Permission Rule Definitions
- **LEAVEFLAG**: Leave Classification Reference
- **PROGRAMLOV_MAST**: Program-Specific List of Values (future)

### Key Relationships
- LOV_MAST has Foreign Key to LOV_TYPEMAST
- Cascading delete on type deletion
- Unique constraints on code combinations
- Indexes for performance optimization

## Configuration

### JWT Configuration
```json
{
  "Jwt": {
    "SecretKey": "your-secret-key-must-be-at-least-32-characters-long!",
    "Issuer": "reference-service",
    "Audience": "reference-service-api",
    "ExpirationMinutes": 60
  }
}
```

### RabbitMQ Configuration
```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/"
  }
}
```

## Security Considerations

1. **JWT Token Management**
   - Always use strong secret keys (minimum 32 characters)
   - Rotate keys periodically
   - Set appropriate expiration times

2. **Database Security**
   - Use Integrated Security or encrypted connection strings
   - Implement row-level security where needed
   - Regular backups

3. **API Security**
   - HTTPS enforcement
   - Request rate limiting (implement with Polly)
   - CORS restrictions
   - Input validation

4. **Event Publication**
   - RabbitMQ authentication
   - Message encryption for sensitive data
   - Dead-letter queues for failed events

## Logging & Monitoring

- **Application Insights Integration** (Azure Functions)
- **Structured Logging** with ILogger
- **Health Check Endpoints** for monitoring
- **Request/Response Logging Middleware**
- **Exception Logging** for debugging

## Performance Optimization

1. **Database**
   - Indexed queries for fast retrieval
   - Connection pooling
   - EF Core lazy loading control

2. **Caching**
   - Consider Redis for LOV Types/Values
   - Cache invalidation on updates

3. **API**
   - Pagination for large result sets
   - Async/await throughout
   - Compression middleware

4. **Resilience**
   - Polly Circuit Breaker for external calls
   - Automatic retries with exponential backoff
   - Timeout handling

## Deployment

### Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY bin/Release/net10.0/publish .
ENTRYPOINT ["dotnet", "ReferenceService.API.dll"]
```

### Kubernetes
- Health check probes configured
- Readiness/Liveness probes available
- Environment-based configuration

### Azure App Service
- Connection string from environment variables
- Health checks for auto-heal
- Application Insights integration

## Testing

### Unit Tests (Recommended Structure)
```csharp
[Fact]
public async Task CreateLovType_WithValidData_ShouldSucceed()
{
    // Arrange
    var command = new CreateLovTypeCommand("TEST", "Test Type", 1, 1);
    
    // Act
    var result = await handler.Handle(command, CancellationToken.None);
    
    // Assert
    Assert.True(result.Success);
    Assert.NotEqual(0, result.Id);
}
```

### Integration Tests
- Test database operations
- Test API endpoints end-to-end
- Test RabbitMQ event publishing

## Troubleshooting

### Database Connection Issues
```bash
# Check LocalDB instance
SqlLocalDB info

# Create instance if needed
SqlLocalDB create REFERENCEDB

# Start instance
SqlLocalDB start REFERENCEDB
```

### RabbitMQ Issues
```bash
# Check RabbitMQ service status
rabbitmqctl status

# List connections
rabbitmqctl list_connections

# Clear queues if needed
rabbitmqctl purge_queue domain-events
```

### Migration Issues
```bash
# Remove last migration
dotnet ef migrations remove

# Reapply migrations
dotnet ef database update
```

## Contributing Guidelines

1. Follow DDD principles
2. Use meaningful commit messages
3. Add tests for new features
4. Keep domain logic in Domain layer
5. Use CQRS for application logic
6. Document public APIs

## License

[Your License Here]

## Support & Contact

For issues, feature requests, or questions:
- Project Board: [Link]
- Email: [Your Email]
- Wiki: [Link]

## Roadmap

- [ ] Add Redis caching layer
- [ ] Implement Saga pattern for distributed transactions
- [ ] Add advanced filtering/search
- [ ] Implement audit trail
- [ ] Add bulk import/export operations
- [ ] Implement data versioning
- [ ] Add workflow engine integration
- [ ] Implement multi-tenancy

---

**Last Updated**: March 11, 2026  
**Version**: 1.0.0  
**Status**: Production Ready
