# Compensation Service Microservice

A comprehensive, enterprise-grade microservice for managing employee compensation grades using a Clean Architecture approach with CQRS pattern.

## Project Overview

The Compensation Service is built using .NET 8 with the following technologies and patterns:

- **Architecture**: Clean Architecture with layered design
- **Patterns**: CQRS (Command Query Responsibility Segregation), Repository, Unit of Work
- **Database**: SQL Server (LocalDB)
- **ORM**: Entity Framework Core 8
- **Messaging**: RabbitMQ
- **API Styles**: REST, GraphQL, Minimal APIs
- **Authentication**: JWT Bearer
- **Cloud**: Azure (Blob Storage, Functions)
- **Resilience**: Polly (Circuit Breaker, Retry, Timeout)
- **Logging**: Serilog

## Project Structure

```
CompensationService/
├── CompensationService.sln
├── CompensationService.Domain/              # Domain Layer - Business Logic
│   ├── Entities/                            # Domain Entities (CompensationGrade)
│   ├── ValueObjects/                        # Value Objects (GradeCode, SalaryStructure, etc.)
│   ├── Events/                              # Domain Events
│   ├── Repositories/                        # Repository Interfaces
│   └── Common/                              # Base Classes (Entity, AggregateRoot, ValueObject)
├── CompensationService.Application/         # Application Layer - Use Cases
│   ├── Commands/                            # CQRS Commands
│   ├── Queries/                             # CQRS Queries
│   ├── DTOs/                                # Data Transfer Objects
│   ├── Behaviors/                           # MediatR Behaviors (Validation, Logging)
│   └── Mappings/                            # AutoMapper Profiles
├── CompensationService.Infrastructure/      # Infrastructure Layer
│   ├── Persistence/                         # EF Core DbContext, Migrations
│   ├── Repositories/                        # Repository Implementations
│   ├── ExternalServices/                    # Azure Blob Storage, etc.
│   ├── Messaging/                           # RabbitMQ Implementation
│   └── [ServiceCollectionExtensions]        # DI Configuration
├── CompensationService.API/                 # API Presentation Layer
│   ├── Controllers/                         # REST API Controllers
│   ├── GraphQL/                             # GraphQL Queries & Mutations
│   ├── Middleware/                          # Custom Middleware (Error Handling)
│   ├── Configuration/                       # API Configuration (Auth, Health Checks)
│   ├── Program.cs                           # Entry Point
│   ├── appsettings.json                     # Configuration
│   └── appsettings.Development.json         # Development Configuration
└── CompensationService.AzureFunctions/      # Azure Functions for Background Tasks
    ├── CompensationGradeFunction.cs         # Function Definitions
    ├── host.json                            # Functions Configuration
    └── local.settings.json                  # Local Settings
```

## Database Schema

### COMP_GRADE Table

| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| GRADE_ID | BIGINT | PK, Identity | Unique identifier |
| GRADE_CODE | VARCHAR(50) | NOT NULL, UNIQUE | Grade code (e.g., JR001) |
| GRADE_NAME | VARCHAR(255) | NOT NULL | Grade name (e.g., Junior Executive) |
| GRADE_LEVEL | INT | NOT NULL | Grade level for hierarchy |
| BASE_SALARY | DECIMAL(19,2) | NOT NULL | Base salary amount |
| HRA_PERCENTAGE | DECIMAL(5,2) | | HRA (House Rent Allowance) % |
| DA_PERCENTAGE | DECIMAL(5,2) | | DA (Dearness Allowance) % |
| GRADE_STATUS | CHAR(1) | DEFAULT 'A' | A=Active, I=Inactive |
| EFFECTIVE_FROM | DATE | NOT NULL | Grade effective start date |
| EFFECTIVE_TO | DATE | | Grade effective end date |
| CREATED_BY | BIGINT | NOT NULL | User ID who created |
| CREATED_ON | DATETIME2 | DEFAULT GETDATE() | Creation timestamp |
| UPDATED_BY | BIGINT | | User ID who updated |
| UPDATED_ON | DATETIME2 | | Last update timestamp |

## Connection String

```
Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name="CompensationService";Command Timeout=0
```

## Setup Instructions

### Prerequisites

- .NET 8 SDK
- SQL Server LocalDB (or SQL Server)
- RabbitMQ (optional, for messaging)
- Azure Storage Account (optional, for Blob Storage)
- Visual Studio 2022 or Visual Studio Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd CompensationService
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Apply EF Core migrations**
   ```bash
   cd CompensationService.API
   dotnet ef database update --project ../CompensationService.Infrastructure
   ```

4. **Update appsettings.json**
   - Set correct connection string
   - Configure JWT settings
   - Configure RabbitMQ settings (if needed)
   - Configure Azure Blob Storage (if needed)

5. **Run the API**
   ```bash
   dotnet run --project CompensationService.API
   ```

## API Endpoints

### REST API

#### Base URL
```
https://localhost:7001/api/compensation-grades
```

#### Endpoints

- **GET** `/api/compensation-grades` - Get all grades
- **GET** `/api/compensation-grades/active` - Get active grades only
- **GET** `/api/compensation-grades/{id}` - Get grade by ID
- **POST** `/api/compensation-grades` - Create new grade
- **PUT** `/api/compensation-grades/{id}` - Update grade
- **PATCH** `/api/compensation-grades/{id}/status` - Change grade status

#### Example Request (POST)
```json
{
  "gradeCode": "JR001",
  "gradeName": "Junior Executive",
  "gradeLevel": 1,
  "baseSalary": 25000,
  "hraPercentage": 10,
  "daPercentage": 5,
  "effectiveFrom": "2026-03-15T00:00:00Z"
}
```

### GraphQL

#### URL
```
https://localhost:7001/graphql
```

#### Sample Query
```graphql
query {
  compensationGrades {
    gradeId
    gradeCode
    gradeName
    gradeLevel
    baseSalary
    totalSalary
    status
  }
}
```

#### Sample Mutation
```graphql
mutation {
  createCompensationGrade(
    gradeCode: "SR001"
    gradeName: "Senior Executive"
    gradeLevel: 2
    baseSalary: 40000
    hraPercentage: 15
    daPercentage: 8
    effectiveFrom: "2026-03-15T00:00:00Z"
  ) {
    gradeId
    gradeCode
    gradeName
    totalSalary
  }
}
```

Access GraphQL UI at: `https://localhost:7001/graphql`

### Swagger Documentation

Access Swagger UI at: `https://localhost:7001/swagger/index.html`

## Features

### Authentication & Authorization

- JWT-based authentication
- Bearer token support
- Role-based authorization (can be extended)

### API Styles

1. **REST API** - Traditional REST endpoints in `CompensationGradesController`
2. **GraphQL** - Modern GraphQL queries and mutations
3. Minimal APIs can be added in `Program.cs`

### Domain-Driven Design

- Clear separation of concerns
- Value Objects for type-safety
- Domain Events for state changes
- Aggregate Roots following DDD principles

### CQRS Pattern

- Commands for write operations
- Queries for read operations
- Clear separation of read/write models

### Database

- Entity Framework Core 8 with SQL Server
- Code-first migrations
- Seed data included
- Audit fields (CreatedBy, CreatedOn, UpdatedBy, UpdatedOn)

### Resilience & Reliability

- **Circuit Breaker**: Prevents cascading failures
- **Retry Policy**: Exponential backoff for transient failures
- **Timeout Policy**: Prevents indefinite waiting
- **Health Checks**: Endpoint for health status
  - `/health` - Overall health
  - `/health/ready` - Database readiness

### Azure Integration

- **Blob Storage**: For storing stationery item images
- **Azure Functions**: Background tasks
  
#### Azure Functions

- Timer-triggered: Daily grade update processing
- RabbitMQ-triggered: Event processing
- Blob storage-triggered: Image upload processing

### Extensibility

- RabbitMQ message publishing/subscribing
- Azure Blob Storage operations
- Dapper for complex queries (if needed)
- Plugin architecture through interfaces

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=..."
  },
  "Jwt": {
    "SecureKey": "...",
    "Issuer": "...",
    "Audience": "...",
    "ExpirationMinutes": 60
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest",
    "Port": 5672
  },
  "AzureStorage": {
    "ContainerName": "stationery-images"
  }
}
```

## Building & Deployment

### Build

```bash
dotnet build CompensationService.sln --configuration Release
```

### Publish

```bash
dotnet publish CompensationService.API \
  --configuration Release \
  --output ./publish
```

### Docker (if needed)

Create a Dockerfile in CompensationService.API:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet build "CompensationService.sln" -c Release

RUN dotnet publish "CompensationService.API/CompensationService.API.csproj" \
    -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 443
ENTRYPOINT ["dotnet", "CompensationService.API.dll"]
```

## Development Notes

### Adding New Features

1. **Add domain logic** in `CompensationService.Domain`
2. **Create command/query** in `CompensationService.Application`
3. **Add validation** in `Behaviors/Validators.cs`
4. **Implement handler** in Commands/Queries
5. **Add API endpoint** in Controllers or GraphQL
6. **Create migration** if database changes needed

### Database Migrations

```bash
# Add migration
dotnet ef migrations add MigrationName \
  --project CompensationService.Infrastructure

# Update database
dotnet ef database update \
  --project CompensationService.Infrastructure

# Remove migration
dotnet ef migrations remove \
  --project CompensationService.Infrastructure
```

## Testing

Unit and integration tests can be added to:
- `CompensationService.Domain.Tests`
- `CompensationService.Application.Tests`
- `CompensationService.API.Tests`

## Performance Considerations

- Database indexes on frequently queried columns
- Caching strategies (can be added with Redis)
- Dapper for complex queries
- Async/await throughout

## Security Considerations

- JWT authentication for API
- SQL parameterization (EF Core handles this)
- Input validation via FluentValidation
- Rate limiting (can be added)
- API key authentication (can be added)

## Monitoring & Logging

- Serilog for structured logging
- Application Insights (can be enabled)
- Health check endpoints
- Custom middleware for request/response logging

## Troubleshooting

### Database Connection Issues
- Ensure LocalDB is running: `sqllocaldb info`
- Check connection string in `appsettings.json`
- Run migrations: `dotnet ef database update`

### GraphQL Not Working
- Ensure HotChocolate packages are installed
- Check GraphQL endpoint: `/graphql`
- Use GraphQL playground at `/graphql`

### Dependency Injection Errors
- Check service registration in `Program.cs`
- Verify project references in `.csproj` files
- Check for circular dependencies

## License

[Add your license information here]

## Support

For support, please contact [support email/channel]

## Useful Commands

```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test

# Run API
dotnet run --project CompensationService.API

# Apply migrations
dotnet ef database update --project CompensationService.Infrastructure

# Watch mode (for development)
dotnet watch run --project CompensationService.API
```

## Next Steps / Future Enhancements

1. Add unit and integration tests
2. Implement caching with Redis
3. Add API rate limiting
4. Implement soft deletes
5. Add audit logging for all changes
6. Create comprehensive API documentation
7. Set up CI/CD with Azure DevOps or GitHub Actions
8. Implement distributed tracing
9. Add synthetic monitoring
10. Create admin dashboard for management

---

**Version**: 1.0.0  
**Last Updated**: March 15, 2026  
**Status**: Production Ready
