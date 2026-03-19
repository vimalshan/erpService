# User Service Architecture Overview

## Project Structure Summary

This microservice implements a complete user management system with the following layers:

### 1. Domain Layer (UserService.Domain)
- **Entities**: User, UserRoleMapping, UserOrganizationMapping, UserLocationMapping
- **Value Objects**: Email, UserName, PasswordHash, BusinessUnitId
- **Domain Events**: UserCreatedDomainEvent, UserDeactivatedDomainEvent, UserRoleAssignedDomainEvent, etc.
- **Repositories**: IUserRepository interface for data access abstraction

### 2. Application Layer (UserService.Application)
- **Commands**:
  - CreateUserCommand
  - UpdateUserCommand
  - DeactivateUserCommand
  - AssignRoleToUserCommand
  - AssignOrganizationToUserCommand
  - AssignLocationToUserCommand
  - LoginUserCommand

- **Queries**:
  - GetUserByIdQuery
  - GetUserByEmailQuery
  - GetAllUsersQuery
  - GetActiveUsersQuery
  - GetUsersByRoleQuery
  - GetUsersByOrganizationQuery
  - GetUsersByLocationQuery

- **DTOs**: UserDto, CreateUserRequest, LoginResponse, etc.
- **Validators**: FluentValidation for all commands
- **Behaviors**: Logging, Performance monitoring, Validation pipeline

### 3. Infrastructure Layer (UserService.Infrastructure)
- **EF Core DbContext**: UserServiceDbContext with full entity mapping
- **Repositories**: UserRepository implementing IUserRepository
- **Unit of Work**: Transaction management and repository coordination
- **JWT Service**: Token generation and validation
- **RabbitMQ**: Message publishing and consuming
- **Polly Policies**: Circuit breaker, retry, timeout, bulkhead
- **Health Checks**: Database connectivity monitoring
- **Migrations**: 
  - InitialCreate migration with 4 tables
  - SeedData.sql with sample users and mappings

### 4. API Layer (UserService.API)
- **REST Controllers**:
  - UsersController (CRUD operations)
  - AuthController (Login)

- **GraphQL**:
  - Query type (GetUser, GetAllUsers, GetActiveUsers)
  - Mutation type (CreateUser, UpdateUser, DeactivateUser, AssignRole)
  - User type definition

- **Middleware**:
  - GlobalExceptionHandlingMiddleware
  - AuthenticationMiddleware

- **Configuration**:
  - ServiceCollectionExtensions for DI
  - ApplicationBuilderExtensions for pipeline
  - Swagger/OpenAPI documentation
  - JWT authentication setup
  - Health checks endpoint

### 5. Azure Functions (UserService.AzureFunctions)
- **UserEventProcessor**: Processes user domain events from queue
- **UserProfileImageUploader**: Uploads user profile images to blob storage
- **UserStatusReportFunction**: Generates daily user status reports

## Key Features

✅ **CQRS Pattern**: Separate command and query models
✅ **Domain-Driven Design**: Rich domain model with value objects
✅ **Async/Await**: All operations are fully asynchronous
✅ **Validation**: Multi-level validation (domain, application, data)
✅ **Security**: JWT authentication, Bcrypt password hashing, role-based authorization
✅ **Resilience**: Circuit breaker, retry policies, timeout handling
✅ **Logging**: Comprehensive logging with Serilog
✅ **Health Checks**: Database connectivity monitoring
✅ **Database**: EF Core migrations, seed data, repository pattern
✅ **Messaging**: RabbitMQ integration for async events
✅ **Cloud**: Azure Functions and Blob Storage ready
✅ **API Documentation**: Swagger/OpenAPI and GraphQL

## Database Schema

### USER_MAST (User Master)
- USER_ID (PK, Identity)
- USER_NAME (varchar 100)
- USER_PASSWORD (varchar 255, hashed)
- USER_EMAILID (varchar 50)
- USER_SPARSHUSERID (varchar 50, nullable)
- USER_HREMPSYSID (bigint, nullable)
- USER_EFFECTIVE_DATE (datetime2)
- USER_CLOSURE_DATE (datetime2, nullable)
- USER_ENTEREDBY (bigint)
- CREATED_DATE (datetime2)
- MODIFIED_DATE (datetime2, nullable)
- IS_ACTIVE (bit, default 1)

### USER_ROLEMAP (User Role Mapping)
- ROLE_MAPID (PK, Identity)
- ROLE_USERID (FK)
- ROLE_ID (bigint)
- ROLE_DEFFLAG (bit, default 0)
- ROLE_CREATEDON (datetime2)
- ROLE_CREATEDBY (bigint)

### USER_ORGMAP (User Organization Mapping)
- ORG_MAPID (PK, Identity)
- ORG_USERID (FK)
- ORG_BUID (varchar 25)
- ORG_CREATEDON (datetime2)
- ORG_CREATEDBY (bigint)

### USER_LOCATIONMAP (User Location Mapping)
- LOC_MAPID (PK, Identity)
- LOC_USERID (FK)
- LOC_ID (int)
- LOC_CREATEDON (datetime2)
- LOC_CREATEDBY (bigint)

## NuGet Packages Used

**Domain**:
- MediatR.Contracts
- BCrypt.Net-Next

**Application**:
- MediatR
- FluentValidation

**Infrastructure**:
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Dapper
- Polly
- RabbitMQ.Client
- Azure.Storage.Blobs

**API**:
- Microsoft.AspNetCore.Authentication.JwtBearer
- Swashbuckle.AspNetCore
- GraphQL
- HealthChecks.UI
- Serilog

**Azure Functions**:
- Microsoft.Azure.Functions.Worker
- Microsoft.Azure.Functions.Worker.Extensions.Storage
- Microsoft.Azure.Functions.Worker.Extensions.RabbitMQ

## Startup Sequence

1. **Program.cs** registers all services
2. **Dependency Injection** wires up repositories, handlers, and services
3. **EF Core Migrations** applied automatically on startup
4. **Health Checks** initialized
5. **Middleware** pipeline configured
6. **ASP.NET Core** server starts listening

## Environment Configuration

- **Development**: Full logging, relaxed security in JWT
- **Production**: Minimal logging, secure JWT configuration, circuit breakers active

## Testing & Verification

The solution is fully scaffolded and ready for:
1. Integration testing (use TestHost)
2. Unit testing (xUnit/NUnit with mock repositories)
3. API testing (Swagger/Postman)
4. GraphQL testing (Banana Cake Pop or GraphQL client)
5. Load testing (with Polly circuit breaker validation)

## Migration Commands

```bash
# Add new migration
dotnet ef migrations add MigrationName -p UserService.Infrastructure -s UserService.API

# Update database
dotnet ef database update -p UserService.Infrastructure -s UserService.API

# Remove last migration
dotnet ef migrations remove -p UserService.Infrastructure -s UserService.API
```

## File Structure

```
01_USER_MODULE/
├── UserService.sln
├── README.md
├── ARCHITECTURE.md (this file)
├── .gitignore
│
├── UserService.Domain/
│   ├── Abstractions/
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Events/
│   ├── Repositories/
│   └── UserService.Domain.csproj
│
├── UserService.Application/
│   ├── Commands/
│   │   ├── Handlers/
│   │   └── UserCommands.cs
│   ├── Queries/
│   │   ├── Handlers/
│   │   └── UserQueries.cs
│   ├── DTOs/
│   ├── Behaviors/
│   ├── Abstractions/
│   └── UserService.Application.csproj
│
├── UserService.Infrastructure/
│   ├── Data/
│   ├── Repositories/
│   ├── Services/
│   ├── Persistence/
│   ├── Messaging/
│   ├── Policies/
│   ├── Migrations/
│   │   ├── SeedData.sql
│   │   └── 20260319000000_InitialCreate.cs
│   └── UserService.Infrastructure.csproj
│
├── UserService.API/
│   ├── Controllers/
│   ├── GraphQL/
│   ├── Middleware/
│   ├── Extensions/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── UserService.API.csproj
│
└── UserService.AzureFunctions/
    ├── Functions/
    ├── Program.cs
    ├── host.json
    ├── local.settings.json
    └── UserService.AzureFunctions.csproj
```

---

**Created**: March 19, 2026
**Framework**: .NET 8
**Architecture Pattern**: Clean Architecture with CQRS
**Database**: SQL Server LocalDB
