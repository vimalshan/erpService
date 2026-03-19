# User Service Microservice

A complete, production-ready microservice for user authentication, authorization, and role management built with .NET 8, following clean architecture principles with CQRS pattern.

## Architecture Overview

```
UserService.Domain/              Domain layer - Core business logic
├── Entities/                    Domain entities and aggregates
├── ValueObjects/                Value objects for domain
├── Events/                       Domain events
├── Repositories/                Repository interfaces
└── Abstractions/                Base classes and interfaces

UserService.Application/         Application layer - Use cases
├── Commands/                    Commands for mutations
├── Queries/                     Queries for reads
├── DTOs/                        Data transfer objects
├── Behaviors/                   MediatR pipeline behaviors
└── Abstractions/                Application abstractions

UserService.Infrastructure/      Infrastructure layer - Technical concerns
├── Data/                        EF Core DbContext
├── Repositories/                Repository implementations
├── Services/                    External services (JWT, Health checks)
├── Messaging/                   RabbitMQ integration
├── Persistence/                 Unit of Work pattern
├── Policies/                    Polly circuit breaker policies
└── Migrations/                  EF Core migrations

UserService.API/                 Presentation layer - API endpoints
├── Controllers/                 REST API controllers
├── GraphQL/                     GraphQL schema and resolvers
├── Middleware/                  Custom middleware
├── Extensions/                  Service registration extensions
└── Program.cs                   Startup configuration

UserService.AzureFunctions/      Background tasks
├── Functions/                   Azure Functions
└── Program.cs                   Azure Functions startup
```

## Technology Stack

- **Framework**: .NET 8
- **Database**: SQL Server (LocalDB)
- **ORM**: Entity Framework Core 8.0
- **Query Pattern**: CQRS with MediatR
- **Validation**: FluentValidation
- **API**: REST (OpenAPI/Swagger) + GraphQL
- **Authentication**: JWT Bearer
- **Messaging**: RabbitMQ
- **Resilience**: Polly (Circuit Breaker, Retry, Timeout)
- **Cloud**: Azure Functions, Azure Blob Storage
- **Logging**: Serilog
- **Health Checks**: AspNetCore.HealthChecks

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full edition)
- RabbitMQ (optional for messaging)
- Azure Storage Account (optional for blob storage)
- Visual Studio 2022 or VS Code

## Installation & Setup

### 1. Database Setup

Create the database using the SQL Server connection string:

```bash
# Connection string (configured in appsettings.json)
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SSCDB;Integrated Security=True;
```

### 2. Apply Migrations

From the project root:

```bash
# Install EF Core CLI (if not already installed)
dotnet tool install --global dotnet-ef

# Navigate to API project
cd UserService.API

# Apply migrations
dotnet ef database update
```

Or run the migration file directly:

```sql
-- Read and execute the migration SQL from Migrations/20260319000000_InitialCreate.cs
```

### 3. Seed Sample Data

```bash
# Execute seed script against SSCDB database
sqlcmd -S (localdb)\MSSQLLocalDB -d SSCDB -i UserService.Infrastructure/Migrations/SeedData.sql
```

### 4. Configure Settings

Update `appsettings.json` (or `appsettings.Development.json` for dev):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SSCDB;Integrated Security=True;..."
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-at-least-32-chars",
    "Issuer": "UserService",
    "Audience": "UserServiceAPI",
    "ExpirationMinutes": 60
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest"
  }
}
```

### 5. Build Solution

```bash
dotnet restore
dotnet build
```

### 6. Run the API

```bash
cd UserService.API
dotnet run
```

The API will start at `https://localhost:5001` (or `http://localhost:5000`)

## API Documentation

### REST Endpoints

**Authentication**
- `POST /api/auth/login` - Login user

**Users**
- `POST /api/users` - Create user
- `GET /api/users/{id}` - Get user by ID
- `GET /api/users/email/{email}` - Get user by email
- `GET /api/users` - Get all users
- `GET /api/users/active` - Get active users
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Deactivate user
- `POST /api/users/{id}/roles` - Assign role to user
- `POST /api/users/{id}/organizations` - Assign organization to user
- `POST /api/users/{id}/locations` - Assign location to user

### Swagger UI
- Access at: `https://localhost:5001/swagger/index.html`

### GraphQL
- Endpoint: `https://localhost:5001/graphql`
- GraphQL UI (Banana Cake Pop): `https://localhost:5001/graphql`

Example GraphQL Query:
```graphql
query GetUser {
  getUser(userId: 1) {
    userId
    userName
    emailId
    isActive
  }
}
```

Example GraphQL Mutation:
```graphql
mutation CreateUser {
  createUser(
    userName: "newuser"
    password: "password123"
    emailId: "newuser@company.com"
    enteredBy: 1
  )
}
```

### Health Checks
- Endpoint: `https://localhost:5001/health`

## Features Implemented

✅ **Domain Layer**
- Clean domain entities with aggregate roots
- Domain events for state changes
- Value objects with validation
- Repository pattern interfaces

✅ **Application Layer**
- CQRS pattern with Commands and Queries
- MediatR for request handling
- FluentValidation for input validation
- DTOs for API contracts
- Pipeline behaviors (logging, performance monitoring)

✅ **Infrastructure Layer**
- Entity Framework Core 8.0 with migrations
- Repository implementations
- Unit of Work pattern
- JWT token service
- RabbitMQ messaging
- Polly circuit breaker policies
- Health checks

✅ **API Layer**
- REST API with OpenAPI/Swagger documentation
- GraphQL Schema with queries and mutations
- JWT authentication & authorization
- Middleware for error handling
- CORS support
- Minimal APIs support

✅ **Security**
- JWT Bearer authentication
- Role-based access control (RBAC)
- Bcrypt password hashing
- Input validation
- Error handling middleware

✅ **Resilience**
- Circuit breaker pattern (Polly)
- Retry policies with exponential backoff
- Timeout handling
- Bulkhead isolation
- Health checks

✅ **Messaging**
- RabbitMQ integration
- Domain event publishing
- Message consumer pattern
- Async processing

✅ **Azure Functions**
- Background task processing
- Blob storage integration
- Timer-triggered functions
- Queue-triggered functions

✅ **Logging & Monitoring**
- Serilog integration
- Health checks endpoint
- Performance monitoring
- Error logging

## Running Tests

```bash
# Tests can be added in a separate UserService.Tests project
dotnet test
```

## Building for Production

```bash
# Build release version
dotnet build -c Release

# Publish
dotnet publish -c Release -o ./publish
```

## Docker Support (Optional)

Create a Dockerfile for containerization:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY publish .
EXPOSE 5000
CMD ["dotnet", "UserService.API.dll"]
```

## Database Connection String Reference

```
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=SSCDB;
Integrated Security=True;
Persist Security Info=False;
Pooling=False;
MultipleActiveResultSets=False;
Encrypt=True;
TrustServerCertificate=False;
Application Name="User Service";
Command Timeout=0
```

## Key Design Patterns

1. **Domain-Driven Design (DDD)**: Bounded context with domain entities and value objects
2. **CQRS**: Separate read and write models via Commands and Queries
3. **Repository Pattern**: Data access abstraction
4. **Unit of Work**: Transaction management
5. **Pipeline Pattern**: MediatR behaviors for cross-cutting concerns
6. **Circuit Breaker**: Polly policies for resilience
7. **Value Objects**: Domain-driven validation and immutability

## Configuration

### JWT Settings
- Secret key should be at least 32 characters
- Issuer and Audience must match in validation
- Token expiration is configurable

### Database
- Migrations automatically applied on startup
- Seed data can be loaded manually
- Connection pooling disabled for development

### RabbitMQ
- Default connection: localhost:5672
- Guest/guest credentials (change in production)
- Topic exchange with durable queues

## Common Operations

### Create a New User
```bash
curl -X POST https://localhost:5001/api/users \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "john.doe",
    "password": "SecurePassword123!",
    "emailId": "john.doe@company.com",
    "enteredBy": 1
  }'
```

### Login User
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "userEmail": "john.doe@company.com",
    "password": "SecurePassword123!"
  }'
```

### Get User (Requires JWT Token)
```bash
curl -X GET https://localhost:5001/api/users/1 \
  -H "Authorization: Bearer {JWT_TOKEN}"
```

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure SSCDB database exists

### JWT Token Issues
- Verify secret key is configured correctly
- Check token hasn't expired
- Ensure Authorization header format: `Bearer {token}`

### RabbitMQ Issues
- Ensure RabbitMQ is running (if using messaging)
- Check credentials match configuration
- Verify required exchanges and queues exist

## Contributing

Follow these guidelines:
1. Maintain clean architecture principles
2. Write meaningful commit messages
3. Add unit tests for new features
4. Follow C# naming conventions
5. Use async/await for I/O operations

## License

Internal Use Only

## Support

For issues or questions, contact the development team.

---

**Last Updated**: March 19, 2026
**Framework**: .NET 8
**Database**: SQL Server LocalDB
