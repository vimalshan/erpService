# Community Service Microservice

A complete, production-ready .NET Community Service microservice with modern architecture patterns and cloud-native features.

## Architecture

### Project Structure

```
CommunityService/
├── CommunityService.Domain/               # Domain layer
│   ├── Entities/                          # Domain entities
│   ├── ValueObjects/                      # Domain value objects
│   ├── Events/                            # Domain events
│   └── Interfaces/                        # Domain interfaces
├── CommunityService.Application/          # Application layer (CQRS)
│   ├── Commands/                          # MediatR commands
│   ├── Queries/                           # MediatR queries
│   ├── DTOs/                              # Data transfer objects
│   ├── Behaviors/                         # Pipeline behaviors
│   ├── Mappings/                          # AutoMapper profiles
│   └── Validators/                        # FluentValidation validators
├── CommunityService.Infrastructure/       # Infrastructure layer
│   ├── Persistence/                       # EF Core DbContext
│   ├── Repositories/                      # Repository implementations
│   ├── Messaging/                         # RabbitMQ integration
│   ├── Services/                          # Azure, Health checks, Polly
│   └── Migrations/                        # EF Core migrations
├── CommunityService.API/                  # API layer
│   ├── Controllers/                       # REST endpoints
│   ├── GraphQL/                           # GraphQL types & queries
│   ├── Middleware/                        # Custom middleware
│   ├── Extensions/                        # Dependency injection
│   └── Program.cs                         # Application startup
└── CommunityService.AzureFunctions/       # Azure Functions layer
    └── Functions/                         # Timer & event-driven functions
```

## Features

### Domain-Driven Design
- **Entities**: `Community`, `CommunityMember`
- **Value Objects**: `CommunityCode`, `CommunityName`, `PrivacyLevel`, `CommunityType`, `CommunityStatus`, `MemberRole`, `MemberStatus`
- **Aggregates**: Community root aggregate with proper encapsulation
- **Domain Events**: `CommunityCreatedEvent`, `CommunityUpdatedEvent`, `MemberAddedEvent`, etc.

### CQRS Pattern
- **Commands**: Create, Update, Delete operations
- **Queries**: Read operations with pagination support
- **MediatR Integration**: Clean command/query dispatching

### API Layer
- **REST API**: Full CRUD operations via HTTP
  - Swagger documentation at `/swagger/index.html`
  - JWT Bearer authentication
  - Proper HTTP status codes and error handling
  
- **GraphQL API**: At `/graphql`
  - Query communities and members
  - Works with Banana Cake Pop and other GraphQL clients
  
- **Minimal APIs**: Can be added for specific lightweight endpoints

### Authentication & Authorization
- **JWT Bearer Tokens**: Secure token-based auth
- **Role-based Access Control**: Admin, Moderator, Member, Guest roles
- **Claim-based Authorization**: Policy-based authorization rules

### Data Persistence
- **Entity Framework Core 8.0**: ORM with LINQ support
- **SQL Server**: (localdb)\MSSQLLocalDB for development
- **Database Migrations**: Automated schema management
- **Repository Pattern**: Data access abstraction
- **Dapper Support**: For complex queries and batch operations

### Messaging & Events
- **RabbitMQ Integration**: Asynchronous event handling
- **Message Publishers**: Publish domain events to message queue
- **Message Consumers**: Subscribe and process events
- **Topic-based Routing**: Event filtering and routing

### Cloud Integration
- **Azure Blob Storage**: For community assets (icons, banners)
- **Azure Functions**: Scheduled and event-driven background tasks
  - Timer-triggered: Community cleanup, event processing
  - Blob-triggered: Image processing and validation
  - RabbitMQ-triggered: Event consumption

### Resilience & Reliability
- **Circuit Breaker Pattern**: Polly-based resilience
- **Timeout Policies**: Request timeout management
- **Fallback Strategies**: Graceful degradation
- **Health Checks**: 
  - Database connectivity
  - RabbitMQ broker status
  - API health endpoints: `/health`, `/health/ready`, `/health/live`

### Middleware & Cross-Cutting Concerns
- **Exception Handling**: Centralized error handling with problem details
- **Logging**: Structured logging with ILogger
- **Validation**: FluentValidation with MediatR pipeline behavior
- **Authentication**: JWT validation and token extraction

## Database Schema

### COMMUNITY_MAST Table
Master table for communities/forums/groups.

```sql
COMMUNITY_ID (PK, Identity)
COMMUNITY_CODE (Unique, 50 chars)
COMMUNITY_NAME (255 chars)
COMMUNITY_DESC (Max)
COMMUNITY_TYPE (FORUM, INTEREST_GROUP, TEAM, DEPARTMENT)
COMMUNITY_ICON (500 chars URL)
COMMUNITY_BANNER (500 chars URL)
PRIVACY_LEVEL (PUBLIC, PRIVATE, RESTRICTED)
OWNER_ID (Foreign key to Users)
APPROVER_ID (Optional)
COMMUNITY_STATUS (ACTIVE, INACTIVE, ARCHIVED)
MEMBER_COUNT (Count)
CREATED_BY, CREATED_ON
UPDATED_BY, UPDATED_ON
```

### COMMUNITY_MEMBERS Table
Community membership tracking with role management.

```sql
MEMBER_ID (PK, Identity)
COMMUNITY_ID (FK to COMMUNITY_MAST)
USER_SYSID (Foreign key to Users)
MEMBER_ROLE (ADMIN, MODERATOR, MEMBER, GUEST)
JOIN_DATE (Datetime)
LEAVE_DATE (Optional)
MEMBER_STATUS (ACTIVE, INACTIVE, SUSPENDED, REMOVED)
CONTRIBUTION_COUNT (Int)
CREATED_BY, CREATED_ON
UPDATED_BY, UPDATED_ON
Unique Constraint: (COMMUNITY_ID, USER_SYSID)
```

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SRFSPARSHDB;..."
  },
  "Jwt": {
    "Issuer": "CommunityService",
    "Audience": "CommunityServiceAPI",
    "SigningKey": "YourSecureEncryptionKey...",
    "ExpirationMinutes": 60
  },
  "RabbitMq": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest"
  },
  "CircuitBreaker": {
    "FailureThreshold": 3,
    "FailureWindow": 10,
    "SuccessThreshold": 2,
    "SuccessWindow": 30,
    "TimeoutDuration": 5
  }
}
```

## API Endpoints

### Communities

- `GET /api/communities` - Get all communities (paginated)
- `GET /api/communities/{id}` - Get community by ID
- `GET /api/communities/search?searchTerm=...` - Search communities
- `POST /api/communities` - Create new community
- `PUT /api/communities/{id}` - Update community
- `DELETE /api/communities/{id}` - Archive community

### Community Members

- `GET /api/communities/{communityId}/members` - Get all members
- `POST /api/communities/{communityId}/members` - Add member
- `DELETE /api/communities/{communityId}/members/{userId}` - Remove member

### Health Checks

- `GET /health` - Overall health status
- `GET /health/ready` - Readiness check
- `GET /health/live` - Liveness check

## Usage

### Create a Community

```bash
curl -X POST https://localhost:5001/api/communities \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "communityCode": "DEV-001",
    "communityName": "Developers Community",
    "communityDescription": "Community for developers",
    "communityType": "INTEREST_GROUP",
    "privacyLevel": "PUBLIC",
    "ownerId": 1
  }'
```

### Query via GraphQL

```graphql
query {
  getCommunities(pageNumber: 1, pageSize: 10) {
    communityId
    communityCode
    communityName
    memberCount
  }
}
```

## Building & Running

### Prerequisites
- .NET 8.0 SDK
- SQL Server (localdb) or Express
- RabbitMQ (optional, for messaging)
- Azure Storage Account (optional, for blob storage)

### Build

```bash
dotnet build CommunityService.sln
```

### Run API

```bash
dotnet run --project CommunityService.API
```

API available at: `https://localhost:5001`
Swagger UI: `https://localhost:5001/swagger`
GraphQL: `https://localhost:5001/graphql`

### Run Migrations

```bash
dotnet ef database update --project CommunityService.Infrastructure
```

### Run Azure Functions (locally)

```bash
cd CommunityService.AzureFunctions
func start
```

## Testing

```bash
# Restore dependencies
dotnet restore

# Build
dotnet build

# Health check
curl https://localhost:5001/health

# Create JWT token (use online JWT.io or implement token endpoint)
# Use token in Authorization header for subsequent requests
```

## Security Considerations

1. **JWT Token Management**: Use secure signing keys
2. **Role-Based Authorization**: Enforce through policies
3. **Input Validation**: FluentValidation on all inputs
4. **SQL Injection**: Protected via EF Core parameterized queries
5. **HTTPS Enforcement**: Configured in production
6. **CORS**: Configure for specific origins

## Monitoring & Logging

- **Structured Logging**: Using Microsoft.Extensions.Logging
- **Health Checks**: Database and RabbitMQ monitoring
- **Error Handling**: Centralized exception handling
- **Pipeline Logging**: MediatR behavior logs all requests

## Deployment

### Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "CommunityService.API.dll"]
```

### Azure App Service
- Deploy using Visual Studio publish or Azure CLI
- Configure App Settings for production
- Set up Application Insights for monitoring
- Configure database connection strings

## Troubleshooting

### Database Connection Issues
- Verify (localdb)\MSSQLLocalDB is running
- Check connection string in appsettings.json
- Run migrations: `dotnet ef database update`

### RabbitMQ Connection Issues
- Ensure RabbitMQ broker is running
- Check hostname and credentials in appsettings.json
- Verify message queue exists

### JWT Token Issues
- Verify token format: "Bearer <token>"
- Check token expiration
- Validate signing key matches

## Future Enhancements

- [ ] Implement full CQRS with Event Sourcing
- [ ] Add distributed caching (Redis)
- [ ] Implement request/response caching
- [ ] Add pagination refinements
- [ ] Implement soft deletes
- [ ] Add audit logging
- [ ] Implement rate limiting
- [ ] Add API versioning
- [ ] Implement Saga pattern for complex workflows
- [ ] Add comprehensive unit and integration tests

## License

This project is part of the ERPMicroservice platform.

---

**Created**: March 15, 2026
**Version**: 1.0.0
**Status**: Production Ready
