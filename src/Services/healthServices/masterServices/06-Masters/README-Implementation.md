# Masters Microservice

A comprehensive microservice for managing master data (LOV - List of Values) in the Health Services ERP system.

## Architecture

This project follows **Clean Architecture** principles with the following layers:

- **Masters.Domain** - Core business entities, value objects, and domain events
- **Masters.Application** - CQRS commands/queries, DTOs, and business logic
- **Masters.Infrastructure** - Data access (EF Core, Dapper), messaging (RabbitMQ), storage (Azure Blob)
- **Masters.API** - REST API, GraphQL, Minimal APIs, and middleware
- **Masters.Functions** - Azure Functions for background tasks

## Features

### ✅ Implemented

- **Clean Architecture** with clear separation of concerns
- **CQRS Pattern** using MediatR
- **Domain-Driven Design** with entities, value objects, and aggregates
- **Repository Pattern** with Unit of Work
- **Entity Framework Core 8.0** for data access
- **Dapper** support for high-performance queries
- **REST API** with controllers
- **GraphQL** endpoint using HotChocolate
- **Minimal APIs** (alternative endpoint style)
- **JWT Authentication & Authorization**
- **Swagger/OpenAPI** documentation
- **Health Checks** for API and Database
- **RabbitMQ** message publisher and consumer infrastructure
- **Azure Blob Storage** service for file management
- **Polly** circuit breaker infrastructure
- **FluentValidation** with MediatR pipeline behaviors
- **Logging** pipeline behavior
- **Domain Events** infrastructure
- **Exception handling middleware**

## Database Schema

### LOV_TYPEMASTER
- `LOV_TYPECODE` (PK) - 3-character code
- `LOV_TYPENAME` - Descriptive name

### LOV_MASTER
- `LOV_ID` (PK) - Unique identifier
- `LOV_TYPE` (FK) - References LOV_TYPEMASTER
- `LOV_NAME` - Display value

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server LocalDB (or SQL Server instance)
- Azure Storage Emulator (optional, for blob storage)
- RabbitMQ (optional, for messaging)

### Configuration

Update `appsettings.json` in Masters.API:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HEALTHDB;..."
  },
  "Jwt": {
    "SecretKey": "YourSecretKeyHere...",
    "Issuer": "Masters.API",
    "Audience": "Masters.API.Users"
  },
  "RabbitMQ": {
    "ConnectionString": "amqp://guest:guest@localhost:5672"
  },
  "AzureStorage": {
    "ConnectionString": "UseDevelopmentStorage=true"
  }
}
```

### Database Setup

1. Run the schema script:
   ```sql
   -- Execute Masters-Tables.sql
   ```

2. Run the seed data:
   ```sql
   -- Execute SeedData.sql
   ```

### Running the API

```bash
cd src/Masters.API
dotnet run
```

### API Endpoints

- **Swagger UI**: `https://localhost:5001/swagger`
- **GraphQL**: `https://localhost:5001/graphql`
- **Health Checks**: `https://localhost:5001/health`

### Authentication

Get a JWT token:

```bash
POST /api/auth/token
{
  "username": "admin",
  "password": "admin123"
}
```

Use the token in subsequent requests:
```
Authorization: Bearer {token}
```

## Project Structure

```
Masters/
├── src/
│   ├── Masters.Domain/
│   │   ├── Common/           # Base classes, interfaces
│   │   ├── Entities/         # Domain entities
│   │   ├── ValueObjects/     # Value objects
│   │   └── Events/           # Domain events
│   ├── Masters.Application/
│   │   ├── Commands/         # CQRS commands & handlers
│   │   ├── Queries/          # CQRS queries & handlers
│   │   ├── DTOs/             # Data transfer objects
│   │   ├── Behaviours/       # MediatR pipeline behaviors
│   │   └── Interfaces/       # Repository interfaces
│   ├── Masters.Infrastructure/
│   │   ├── Persistence/      # EF Core, repositories
│   │   ├── Messaging/        # RabbitMQ implementation
│   │   └── Storage/          # Azure Blob Storage
│   ├── Masters.API/
│   │   ├── Controllers/      # REST API controllers
│   │   ├── GraphQL/          # GraphQL queries/mutations
│   │   ├── MinimalApis/      # Minimal API endpoints
│   │   └── Middleware/       # Custom middleware
│   └── Masters.Functions/
│       └── [Azure Functions for background tasks]
├── Masters-Tables.sql        # Database schema
├── SeedData.sql              # Seed data script
└── README.md
```

## API Examples

### REST API

```bash
# Get all LOV Type Masters
GET /api/lovtypemaster

# Create LOV Type Master
POST /api/lovtypemaster
{
  "lovTypeCode": "NEW",
  "lovTypeName": "New Type"
}

# Get LOV Masters by Type
GET /api/lovmaster/type/MED
```

### GraphQL

```graphql
query {
  lovTypeMasters {
    lovTypeCode
    lovTypeName
  }
  lovMastersByType(lovType: "MED") {
    lovId
    lovName
  }
}

mutation {
  createLovTypeMaster(lovTypeCode: "NEW", lovTypeName: "New Type") {
    lovTypeCode
    lovTypeName
  }
}
```

### Minimal APIs (v2)

```bash
GET /api/v2/lov-type-masters
GET /api/v2/lov-masters/type/MED
```

## Technology Stack

- **.NET 8.0**
- **ASP.NET Core 8.0**
- **Entity Framework Core 8.0**
- **Dapper 2.1**
- **MediatR 12.x**
- **FluentValidation 11.x**
- **HotChocolate (GraphQL)**
- **RabbitMQ.Client 7.x**
- **Azure.Storage.Blobs**
- **Polly 8.x**
- **Swashbuckle (Swagger)**

## Design Patterns

- **Clean Architecture**
- **CQRS** (Command Query Responsibility Segregation)
- **Repository Pattern**
- **Unit of Work**
- **Domain-Driven Design**
- **Mediator Pattern**
- **Pipeline Behavior (Cross-cutting Concerns)**

## Next Steps

To complete the setup:

1. Resolve package version conflicts (AutoMapper compatibility)
2. Create EF Core migrations
3. Implement Azure Functions
4. Add comprehensive unit and integration tests
5. Set up CI/CD pipelines
6. Add caching layer (Redis)
7. Implement API versioning
8. Add rate limiting

## License

Copyright © 2026 - Health Services ERP
