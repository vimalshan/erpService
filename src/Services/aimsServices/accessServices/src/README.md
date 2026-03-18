# Access Service - Microservice for User Access Management

A comprehensive .NET 8.0 microservice implementing Domain-Driven Design (DDD), CQRS, and Clean Architecture principles for managing user access, roles, and permissions in the AIMS system.

## Project Structure

### Solution: `AccessService.sln`

#### **1. AccessService.Domain**
- **Purpose**: Core business logic and domain entities
- **Key Components**:
  - Entities: `UserMap`, `UserRole`, `Menu`, `UserMenuMap`, `SPARSHMenu`, `SPARSHMenuAccess`
  - Aggregate Roots: Base `AggregateRoot` class
  - Value Objects: `RoleType`
  - Domain Events: User access state change events
  - Interfaces: Repository contracts

#### **2. AccessService.Application**
- **Purpose**: Application services and business process orchestration
- **Key Components**:
  - CQRS Commands: User map and role management commands
  - CQRS Queries: Data retrieval queries
  - DTOs: Data Transfer Objects for API contracts
  - Behaviors: Validation, logging, performance monitoring
  - Interfaces: Application-level contracts

#### **3. AccessService.Infrastructure**
- **Purpose**: Data access and external service integration
- **Key Components**:
  - Entity Framework DbContext: `AccessServiceDbContext`
  - Repositories: Specialized repositories for aggregates
  - Unit of Work: Transaction management
  - EF Migrations: Database schema management
  - Dapper: Ad-hoc SQL for complex queries
  - External integrations: RabbitMQ, Blob Storage, Azure Functions

#### **4. AccessService.API**
- **Purpose**: RESTful API exposure and request handling
- **Key Components**:
  - Controllers: `UserMapsController`, `UserRolesController`, `MenusController`
  - Middleware: Authentication, error handling, logging
  - Configuration: Dependency injection setup
  - Health Checks: API and database health endpoints
  - Swagger/OpenAPI: API documentation

#### **5. AccessService.Tests**
- **Purpose**: Unit and integration tests
- **Key Components**:
  - Unit Tests: Domain logic and application service tests
  - Integration Tests: Database and API integration tests
  - Test Fixtures: Common test data setup

## Database Schema

The solution is built on the following SQL Server tables:

### Core Tables
- **AIMS_USERMAP**: Employee to application user mappings
- **AIMS_USERROLE**: User role assignments with scope (org, unit, calendar)
- **MENU_MASTER**: Application menu hierarchy
- **AIMS_USERMENUMAP**: Role-menu access mappings
- **SPARSHMENU_MASTER**: SPARSH system menus
- **SPARSHMENU_ACCESS**: Granular menu access by unit/calendar/grade

### Connection String
```
Data Source=(localdb)\MSSQLLocalDB;
Integrated Security=True;
Persist Security Info=False;
Pooling=False;
MultipleActiveResultSets=False;
Encrypt=True;
TrustServerCertificate=False;
Application Name="AccessService";
Command Timeout=0;
Initial Catalog=ACCESSDB;
```

## Technology Stack

- **Framework**: .NET 8.0
- **Database**: SQL Server 2019+ (LocalDB for development)
- **ORM**: Entity Framework Core 8.0
- **AD Hoc SQL**: Dapper
- **Architecture Pattern**: Domain-Driven Design (DDD)
- **Command/Query Pattern**: CQRS
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **API Documentation**: Swagger/OpenAPI
- **Testing**: xUnit, Moq
- **Background Jobs**: Azure Functions
- **Message Queue**: RabbitMQ
- **Blob Storage**: Azure Storage
- **Security**: JWT Bearer Tokens
- **Resilience**: Polly (Circuit Breaker, Retry)
- **Monitoring**: Application Insights

## Key Features

### 1. User Access Management
- Create and manage user to employee system mappings
- Track effective and closure dates for access validity
- Audit trail with modified by/on tracking

### 2. Role-Based Access Control (RBAC)
- Three role types: SuperUser (S), Unit Access (U), Calendar Access (C)
- Scope-based access: Organization, Unit, Calendar
- Menu access levels: All, View Only, Specific

### 3. Menu Hierarchy
- Hierarchical menu structure with parent-child relationships
- Calendar role-specific menu access
- Display order management

### 4. SPARSH System Integration
- Granular access control by unit, calendar, and grade category
- Menu-specific access management for SPARSH subsystem

### 5. Domain Events
- UserMapCreated, UserMapActivated, UserMapDeactivated
- UserRoleAssigned, UserRoleRevoked
- MenuAccessGranted, MenuAccessRevoked

### 6. API Endpoints

#### UserMaps
- `GET /api/usermaps/{employeeSystemId}` - Get user map by employee ID
- `GET /api/usermaps` - Get all user maps (with activeOnly filter)
- `POST /api/usermaps` - Create new user map
- `PUT /api/usermaps/{employeeSystemId}/activate` - Activate user map
- `PUT /api/usermaps/{employeeSystemId}/deactivate` - Deactivate user map

#### UserRoles
- `GET /api/userroles/{roleId}` - Get role by ID
- `GET /api/userroles/employee/{employeeSystemId}` - Get roles for employee
- `GET /api/userroles/type/{roleType}` - Get roles by type
- `POST /api/userroles` - Assign new role
- `PUT /api/userroles/{roleId}` - Update role
- `DELETE /api/userroles/{roleId}` - Revoke role

#### Health Checks
- `GET /health` - API and database health status

#### Swagger/API Documentation
- `GET /swagger` - Swagger UI
- `GET /swagger/v1/swagger.json` - OpenAPI specification

## Configuration

### appsettings.json
Located in `AccessService.API` project:
- Database connection strings
- JWT token settings
- RabbitMQ configuration
- Azure Blob Storage settings
- Health check configuration

### Environment Variables
- `ASPNETCORE_ENVIRONMENT`: Development, Staging, Production
- `ConnectionStrings__DefaultConnection`: Database connection string
- `JwtSettings__Secret`: JWT signing secret

## Development Setup

### Prerequisites
- .NET 8.0 SDK
- SQL Server 2019+ or SQL Server Express with LocalDB
- Visual Studio 2022 or VS Code
- Git

### Installation Steps

1. Clone the repository
```bash
git clone <repository-url>
cd src
```

2. Restore dependencies
```bash
dotnet restore
```

3. Update database (create initial schema)
```bash
cd AccessService.API
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Run the application
```bash
dotnet run
```

5. Access Swagger
```
https://localhost:5001/swagger
```

## Database Migrations

### Create Migration
```bash
cd AccessService.API
dotnet ef migrations add <MigrationName> --project ../AccessService.Infrastructure
```

### Apply Migrations
```bash
cd AccessService.API
dotnet ef database update
```

### Script-based Migrations (SQL files)
See `ACCESSDB-DEPLOYMENT.sql` for database initialization scripts.

## Seed Data

Seed data scripts are located in the `Infrastructure/Persistence/Seeders` folder:
- UserMapSeeder.cs
- UserRoleSeeder.cs
- MenuSeeder.cs
- SPARSHMenuSeeder.cs

## Authentication & Authorization

JWT Bearer token-based authentication:

### Token Claims
- `sub`: Subject (Employee ID)
- `roles`: User roles array
- `email`: User email
- `aud`: Audience (AccessServiceUsers)
- `iss`: Issuer (AccessService)

### Bearer Token Usage
```
Authorization: Bearer <jwt-token>
```

## API Gateway Integration

Register this microservice with API Gateway:
- **Service Name**: AccessService
- **Base URL**: `https://localhost:5001/api`
- **Health Check**: `/health`
- **OpenAPI Spec**: `/swagger/v1/swagger.json`

## Monitoring & Logging

### Application Insights
Configure in `appsettings.json`:
```json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-key-here"
  }
}
```

### Structured Logging
All controllers and services use structured logging with Serilog

## Testing

### Unit Tests
```bash
dotnet test AccessService.Tests --filter "Category=Unit"
```

### Integration Tests
```bash
dotnet test AccessService.Tests --filter "Category=Integration"
```

### All Tests
```bash
dotnet test AccessService.Tests
```

## Deployment

### Docker
```bash
docker build -t access-service:latest .
docker run -p 5001:80 -e ConnectionStrings__DefaultConnection=<conn-string> access-service:latest
```

### Azure App Service
```bash
az webapp create --resource-group <rg> --plan <plan> --name access-service
az webapp deployment source config-zip --resource-group <rg> --name access-service --src publish.zip
```

## Troubleshooting

### Database Connection Issues
1. Verify SQL Server LocalDB is running: `sqllocaldb info`
2. Start LocalDB: `sqllocaldb start MSSQLLocalDB`
3. Check connection string in `appsettings.json`

### Migration Issues
1. Drop and recreate database: `dotnet ef database drop --force`
2. Re-apply migrations: `dotnet ef database update`

### JWT Token Issues
1. Verify JWT secret is set in `appsettings.json`
2. Check token expiry time
3. Validate token at jwt.io

## Contributing

1. Create feature branch: `git checkout -b feature/feature-name`
2. Commit changes: `git commit -am 'Add feature'`
3. Push to branch: `git push origin feature/feature-name`
4. Create Pull Request

## License

Proprietary - AIMS ERP System

## Support

For issues and questions, contact the development team.
