# Group Management Microservice

A comprehensive microservice solution for managing user groups and menu-based access control built with .NET and modern architectural patterns.

## Project Structure

```
GroupManagementService/
├── GroupManagementService.Domain
│   ├── Entities/
│   │   ├── BaseEntity.cs
│   │   ├── Group.cs
│   │   └── GroupMenuMap.cs
│   ├── ValueObjects/
│   │   ├── MenuPermissions.cs
│   │   └── GroupStatus.cs
│   ├── Events/
│   │   └── DomainEvents.cs
│   └── Repositories/
│       └── IGroupRepository.cs
│
├── GroupManagementService.Application
│   ├── DTOs/
│   │   └── GroupDtos.cs
│   ├── Commands/
│   │   └── GroupCommands.cs
│   ├── Queries/
│   │   └── GroupQueries.cs
│   ├── Handlers/
│   │   ├── CommandHandlers.cs
│   │   └── QueryHandlers.cs
│   ├── Behaviors/
│   │   └── PipelineBehaviors.cs
│   └── Profiles/
│       └── MappingProfile.cs
│
├── GroupManagementService.Infrastructure
│   ├── Persistence/
│   │   ├── GroupManagementDbContext.cs
│   │   └── Configurations/
│   │       └── EntityConfigurations.cs
│   ├── Repositories/
│   │   └── GroupRepository.cs
│   ├── Seeds/
│   │   └── GroupManagementSeeds.cs
│   └── DependencyInjection.cs
│
├── GroupManagementService.API
│   ├── Controllers/
│   │   ├── GroupsController.cs
│   │   └── MenuMapsController.cs
│   ├── GraphQL/
│   │   ├── GroupQuery.cs
│   │   └── GroupMutation.cs
│   ├── Security/
│   │   └── JwtTokenGenerator.cs
│   ├── Middleware/
│   │   └── ExceptionHandlingMiddleware.cs
│   ├── Configuration/
│   │   ├── RabbitMqConfig.cs
│   │   └── HealthCheckConfiguration.cs
│   ├── Program.cs
│   └── appsettings.json
│
├── GroupManagementService.BackgroundTasks
│   └── Services/
│       └── GroupExportService.cs
│
└── GroupManagementService.Tests/
```

## Architecture Overview

### Domain Layer
Contains core business logic, entities, value objects, and domain events.

**Key Components:**
- `Group` - Aggregate root for group management
- `GroupMenuMap` - Value object for menu mappings
- `MenuPermissions` - Value object for access control
- Domain Events for state changes

### Application Layer
Implements CQRS pattern with Commands and Queries, DTOs, and pipeline behaviors.

**Key Components:**
- **Commands**: CreateGroup, UpdateGroup, ActivateGroup, DeactivateGroup, AddMenuMap, RemoveMenuMap, UpdateMenuPermissions
- **Queries**: GetGroupById, GetGroupByCode, GetAllGroups, GetGroupsByStatus, SearchGroups, GetAdminGroups
- **Behaviors**: Logging, Validation, Exception Handling, Performance Monitoring
- **AutoMapper Profiles**: DTOEntity mappings

### Infrastructure Layer
Implements data access using Entity Framework Core and repositories.

**Key Components:**
- `GroupManagementDbContext` - EF Core DbContext
- `GroupRepository` - Implementation of IGroupRepository
- Database migrations and seed data
- SQL Server as the primary data store

### API Layer
Provides multiple API interfaces: REST (JSON), GraphQL, and Minimal APIs.

**Endpoints:**

#### REST API
- `GET /api/v1/groups` - Get all groups
- `GET /api/v1/groups/{id}` - Get group by ID
- `GET /api/v1/groups/code/{code}` - Get group by code
- `POST /api/v1/groups` - Create group
- `PUT /api/v1/groups/{id}` - Update group
- `POST /api/v1/groups/{id}/activate` - Activate group
- `POST /api/v1/groups/{id}/deactivate` - Deactivate group
- `GET /api/v1/groups/search` - Search groups
- `POST /api/v1/groups/{groupId}/menumaps` - Add menu mapping
- `DELETE /api/v1/groups/{groupId}/menumaps/{menuCode}` - Remove menu mapping
- `PUT /api/v1/groups/{groupId}/menumaps/{menuCode}/permissions` - Update permissions

#### GraphQL Endpoint
- `/graphql` - Access to GraphQL Playground (Banana Cake Pop)

**Queries:**
- `getGroupById` - Get group by ID
- `getGroupByCode` - Get group by code
- `getAllGroups` - Get all groups
- `searchGroups` - Search with filters and pagination
- `getAdminGroups` - Get admin groups
- `getGroupsByStatus` - Get groups by status

**Mutations:**
- `createGroup` - Create new group
- `updateGroup` - Update group details
- `activateGroup` - Activate a group
- `deactivateGroup` - Deactivate a group
- `addMenuMap` - Add menu to group
- `removeMenuMap` - Remove menu from group
- `updateMenuPermissions` - Update menu permissions

#### Minimal APIs
- `POST /api/v1/groups-minimal/` - Create group using minimal API

### Background Tasks Layer
Implements background processing for exports and async operations.

**Services:**
- `GroupExportService` - Exports groups to Azure Blob Storage

## Key Features

### 1. **Authentication & Authorization**
- JWT Bearer token authentication
- Role-based authorization
- Token generation with configurable expiration
- Secure endpoints with [Authorize] attributes

### 2. **CQRS Pattern**
- Clear separation of Commands (mutations) and Queries (read operations)
- MediatR for command/query dispatching
- Handlers for business logic implementation

### 3. **Domain-Driven Design**
- Aggregate roots (Group)
- Value objects (MenuPermissions)
- Domain events for state changes
- Repository pattern for data access

### 4. **Health Checks**
- SQL Server database health check
- RabbitMQ messaging health check
- Dedicated health check endpoints:
  - `/health` - Full health report
  - `/api/health` - API health endpoint

### 5. **RabbitMQ Messaging**
- Configuration for rabbitmq connectivity
- Message publishing service
- Queue management
- Automatic recovery and reconnection

### 6. **Circuit Breaker Pattern**
- Polly integration for resilience
- Configurable retry and circuit-breaker policies
- External HTTP client configuration

### 7. **Azure Integration**
- Blob Storage support for file uploads/exports
- Azure Identity integration
- Background tasks for async operations

### 8. **API Documentation**
- Swagger/OpenAPI integration
- Automatic API documentation
- Interactive API testing via Swagger UI
- GraphQL schema documentation

### 9. **Error Handling**
- Global exception handling middleware
- Structured error responses
- Logging with ILogger interface

### 10. **Pipeline Behaviors**
- Logging behavior for all requests
- Validation behavior for cross-cutting concerns
- Exception handling behavior
- Performance monitoring with slow request warnings

## Database Schema

### GROUP_MAST Table
```sql
CREATE TABLE [GROUP_MAST] (
    [GROUP_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [GROUP_CODE] VARCHAR(50) NOT NULL UNIQUE,
    [GROUP_NAME] VARCHAR(255) NOT NULL,
    [GROUP_DESC] NVARCHAR(MAX),
    [GROUP_STATUS] CHAR(1) DEFAULT 'A',
    [IS_ADMIN] CHAR(1) DEFAULT 'N',
    [CREATED_BY] BIGINT NOT NULL,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [UPDATED_BY] BIGINT,
    [UPDATED_ON] DATETIME2(3)
);
```

### GROUP_MENUMAP Table
```sql
CREATE TABLE [GROUP_MENUMAP] (
    [MENUMAP_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [GROUP_ID] BIGINT NOT NULL,
    [MENU_CODE] VARCHAR(50) NOT NULL,
    [MENU_NAME] VARCHAR(255) NOT NULL,
    [CAN_VIEW] CHAR(1) DEFAULT 'Y',
    [CAN_CREATE] CHAR(1) DEFAULT 'N',
    [CAN_EDIT] CHAR(1) DEFAULT 'N',
    [CAN_DELETE] CHAR(1) DEFAULT 'N',
    [CAN_APPROVE] CHAR(1) DEFAULT 'N',
    [MENU_SEQUENCE] INT,
    [CREATED_BY] BIGINT NOT NULL,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [UPDATED_BY] BIGINT,
    [UPDATED_ON] DATETIME2(3),
    CONSTRAINT [FK_GROUP_MENUMAP_GROUP] FOREIGN KEY ([GROUP_ID]) REFERENCES [GROUP_MAST]([GROUP_ID])
);
```

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GroupManagementDb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"GroupManagementService\";Command Timeout=0",
    "AzureBlobStorage": ""
  },
  "Jwt": {
    "SecretKey": "your-super-secret-key-min-32-characters-long-here",
    "Issuer": "GroupManagementService",
    "Audience": "GroupManagementService",
    "ExpiresInMinutes": 60
  },
  "RabbitMq": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/"
  }
}
```

## Getting Started

### Prerequisites
- .NET 8.0 or higher
- SQL Server (localdb or Express)
- RabbitMQ (optional, for messaging)
- Visual Studio 2022 or VS Code

### Setup Steps

1. **Clone/Open the Solution**
   ```bash
   cd GroupManagementService
   ```

2. **Update appsettings.json**
   - Set the correct JWT secret key (minimum 32 characters)
   - Update RabbitMQ configuration if needed
   - Configure Azure Blob Storage connection string if using

3. **Build the Solution**
   ```bash
   dotnet build
   ```

4. **Apply Database Migrations**
   ```bash
   dotnet ef database update --project GroupManagementService.Infrastructure --startup-project GroupManagementService.API
   ```

5. **Run the API**
   ```bash
   dotnet run --project GroupManagementService.API
   ```

6. **Access the APIs**
   - **Swagger UI**: http://localhost:5000/swagger
   - **GraphQL**: http://localhost:5000/graphql
   - **Health Check**: http://localhost:5000/health

## API Examples

### REST API - Create Group
```bash
curl -X POST https://localhost:5001/api/v1/groups \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {JWT_TOKEN}" \
  -d '{
    "code": "ADMIN",
    "name": "Administrator Group",
    "description": "Full system access",
    "createdBy": 1,
    "isAdmin": true
  }'
```

### REST API - Add Menu Mapping
```bash
curl -X POST https://localhost:5001/api/v1/groups/1/menumaps \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {JWT_TOKEN}" \
  -d '{
    "menuCode": "USER_MNGMT",
    "menuName": "User Management",
    "permissions": {
      "canView": true,
      "canCreate": true,
      "canEdit": true,
      "canDelete": false,
      "canApprove": false
    },
    "createdBy": 1,
    "menuSequence": 1
  }'
```

### GraphQL - Query Groups
```graphql
query {
  getAllGroups {
    id
    code
    name
    status
    isAdmin
    menuMaps {
      menuCode
      menuName
      permissions {
        canView
        canCreate
        canEdit
        canDelete
        canApprove
      }
    }
  }
}
```

### GraphQL - Mutation: Create Group
```graphql
mutation {
  createGroup(
    code: "MANAGER"
    name: "Manager Group"
    description: "Manager access"
    createdBy: 1
    isAdmin: false
  ) {
    id
    code
    name
    status
  }
}
```

## Testing

Run unit tests using:
```bash
dotnet test
```

## Seed Data

The application automatically seeds initial data on startup:
- **ADMIN** group with full permissions
- **USER** group with read-only access
- **MANAGER** group with approval permissions

## Logging

Logs are written to the configured output using ILogger. Configure in appsettings.json:

```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```

## Security Considerations

1. **JWT Secret**: Change the default secret key in production
2. **HTTPS**: Always use HTTPS in production
3. **Database**: Use strong connection strings and managed identities
4. **CORS**: Configure appropriate CORS policies for your use case
5. **Rate Limiting**: Consider implementing rate limiting for production

## Deployment

### Docker Deployment
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "GroupManagementService.API.dll"]
```

### Azure App Service
1. Create App Service in Azure
2. Configure Application Settings with database connection string
3. Deploy using Visual Studio Publish or Azure DevOps

## Future Enhancements

- [ ] EF Core migrations versioning
- [ ] Advanced GraphQL subscriptions
- [ ] Kafka integration for event streaming
- [ ] OpenTelemetry observability
- [ ] Redis caching layer
- [ ] Advanced search with Elasticsearch
- [ ] Audit logging and compliance
- [ ] Multi-tenancy support

## Contributing

Follow these guidelines:
1. Create feature branches from main
2. Write unit tests for new features
3. Follow SOLID principles
4. Submit pull requests with clear descriptions

## License

[Your License Here]

## Support

For issues and questions, contact the development team.

---

**Last Updated**: March 15, 2026
**Version**: 1.0.0
