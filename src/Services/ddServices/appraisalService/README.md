# Appraisal Service Microservice

A comprehensive microservice solution for managing employee appraisals with REST API, GraphQL support, JWT authentication, and Azure integration.

## Project Structure

```
AppraisalService/
├── src/
│   ├── AppraisalService.Domain/              # Domain layer (entities, value objects, events)
│   │   ├── Entities/                         # Domain entities
│   │   ├── ValueObjects/                     # Value objects
│   │   ├── Events/                           # Domain events
│   │   ├── Repositories/                     # Repository interfaces
│   │   └── Entity.cs                         # Base entity class
│   │
│   ├── AppraisalService.Application/         # Application layer (CQRS, DTOs)
│   │   ├── CQRS/
│   │   │   ├── Commands/                     # Command handlers
│   │   │   └── Queries/                      # Query handlers
│   │   ├── DTOs/                             # Data transfer objects
│   │   ├── Behaviors/                        # MediatR pipeline behaviors
│   │   └── MappingProfile.cs                 # AutoMapper configuration
│   │
│   ├── AppraisalService.Infrastructure/      # Infrastructure layer (EF, Repos, External Services)
│   │   ├── Persistence/
│   │   │   ├── Data/                         # DbContext and configurations
│   │   │   └── Repositories/                 # Repository implementations
│   │   ├── Migrations/                       # EF Core migrations
│   │   ├── Messaging/                        # RabbitMQ implementation
│   │   ├── Storage/                          # Azure Blob Storage service
│   │   └── Authentication/                   # JWT token service
│   │
│   ├── AppraisalService.API/                 # API layer (REST, GraphQL, Middleware)
│   │   ├── Controllers/                      # REST API controllers
│   │   ├── GraphQL/                          # GraphQL schema and resolvers
│   │   ├── Middleware/                       # Custom middleware
│   │   ├── Extensions/                       # DI extensions
│   │   ├── Program.cs                        # Main entry point
│   │   ├── appsettings.json                  # Configuration
│   │   └── appsettings.Development.json      # Development configuration
│   │
│   └── AppraisalService.Functions/           # Azure Functions for background tasks
│       ├── AppraisalProcessorFunction.cs     # Scheduled processors
│       └── local.settings.json               # Azure Functions configuration
│
└── AppraisalService.sln                      # Solution file
```

## Architecture & Technology Stack

### Layers:

1. **Domain Layer** - Contains business logic, entities, and domain events
   - Pure C# with no external dependencies (except MediatR for events)
   - Contains validation rules and business rules

2. **Application Layer** - Contains business use cases
   - CQRS pattern for query/command separation
   - DTOs for data transfer
   - MediatR for handler registration
   - AutoMapper for object mapping
   - FluentValidation for input validation

3. **Infrastructure Layer** - Contains external service implementations
   - Entity Framework Core for data persistence
   - Repository pattern for data access
   - RabbitMQ for message publishing/consuming
   - Azure Blob Storage for file management
   - JWT token generation and validation

4. **API Layer** - Contains REST endpoints and GraphQL
   - ASP.NET Core 8.0 controllers for REST API
   - HotChocolate for GraphQL support
   - Swagger/OpenAPI documentation
   - Custom middleware for exception handling and logging
   - Health checks and Polly circuit breakers

5. **Azure Functions** - Background processing
   - Scheduled tasks using TimerTrigger
   - RabbitMQ message processing
   - Manual HTTP-triggered processors

### Technologies:

- **Framework**: .NET 8.0
- **Database**: SQL Server (LocalDB)
- **ORM**: Entity Framework Core 8.0
- **Cache/Message Queue**: RabbitMQ
- **Cloud Services**: Azure Blob Storage, Azure Functions
- **API Options**:
  - REST API with Swagger documentation
  - GraphQL with HotChocolate
  - Minimal APIs
- **Authentication**: JWT Bearer Tokens
- **Resilience**: Polly (Circuit Breaker, Retry policies)
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **CQRS**: MediatR
- **Logging**: Serilog (optional, can be added)

## Prerequisites

- .NET 8.0 SDK
- SQL Server 2019+ or LocalDB
- RabbitMQ 3.11+ (optional, for messaging)
- Azure Storage Account (optional, for blob storage)
- Visual Studio 2022 or VS Code

## Setup Instructions

### 1. Clone and Navigate

```bash
cd E:\ERPMicroservice\src\Services\ddServices\appraisalService
```

### 2. Restore Dependencies

```bash
dotnet restore AppraisalService.sln
```

### 3. Update Database

```bash
# Navigate to API project
cd src/AppraisalService.API

# Apply migrations
dotnet ef database update -p ../AppraisalService.Infrastructure
```

### 4. Configure Settings

Update `appsettings.json` with:
- Database connection string
- JWT secret key (use a strong key in production)
- RabbitMQ credentials (if using messaging)
- Azure Storage credentials (if using blob storage)

### 5. Build Solution

```bash
dotnet build AppraisalService.sln
```

### 6. Run API

```bash
cd src/AppraisalService.API
dotnet run
```

The API will start at:
- REST API: `https://localhost:7001` (default)
- Swagger UI: `https://localhost:7001/swagger/index.html`
- GraphQL: `https://localhost:7001/graphql`
- Health Check: `https://localhost:7001/health`

## API Endpoints

### REST API

#### Appraisals
- `GET /api/appraisals/{requestNumber}` - Get appraisal details
- `GET /api/appraisals/user/{userCode}` - Get appraisal by user code
- `GET /api/appraisals/year/{yearId}` - Get appraisals by year
- `GET /api/appraisals/status/{statusCode}` - Get appraisals by status
- `POST /api/appraisals` - Create new appraisal
- `PUT /api/appraisals/{requestNumber}` - Update appraisal
- `POST /api/appraisals/{requestNumber}/submit` - Submit appraisal
- `POST /api/appraisals/{requestNumber}/cancel` - Cancel appraisal (Admin, HR only)
- `GET /api/appraisals/{requestNumber}/competencies` - Get competency assessments
- `POST /api/appraisals/{requestNumber}/competencies` - Add competency assessment (Appraiser, HR only)

#### Appraisal Bands
- `GET /api/appraisalbands` - Get all bands

### GraphQL Schema

```graphql
type Query {
  getAppraisal(requestNumber: Long!): AppraisalDetailedDto
  getAppraisalByUser(userCode: String!): AppraisalMainDto
  getAppraisalsByYear(yearId: Long!): [AppraisalMainDto]
  getCompetencies(requestNumber: Long!): [CompetencyAssessmentDto]
  getBands: [AppraisalBandDto]
}

type Mutation {
  createAppraisal(input: CreateAppraisalInput!): Long
  updateAppraisal(requestNumber: Long!, input: UpdateAppraisalInput!): Boolean
  submitAppraisal(requestNumber: Long!, finalVtcRating: String): Boolean
}
```

Access GraphQL at: `POST /graphql`

## Authentication

The API uses JWT Bearer token authentication.

### Get Token

```csharp
// Use IJwtTokenService to generate tokens
var token = jwtTokenService.GenerateToken(userId, userName, roles);
```

### Use Token

Add to request headers:
```
Authorization: Bearer <your-jwt-token>
```

## Database Schema

Main tables mapped to existing database structure:

- `DD_APPRAISALMAIN` - Core appraisal records
- `DD_APPRAISALBAND` - Employee band information
- `DD_APPRAISERASSESS` - Competency assessments
- `DD_APPRAISEEGOAL_CUR` - Employee goals
- `DD_APPRAISALDETAILS` - Additional appraisal details

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "AppraisalDb": "Data Source=(localdb)\\MSSQLLocalDB;..."
  },
  "JwtSettings": {
    "Secret": "your-secret-key",
    "Issuer": "AppraisalService",
    "Audience": "AppraisalServiceUsers",
    "ExpirationMinutes": 60
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest"
  },
  "AzureStorage": {
    "ConnectionString": "DefaultEndpointsProtocol=...",
    "ContainerName": "appraisal-documents"
  }
}
```

## Key Features

### ✓ Domain-Driven Design
- Rich domain model with aggregates and value objects
- Domain events for state changes
- Domain validation

### ✓ CQRS Pattern
- Clear separation of reads and writes
- Optimized query and command handlers
- Consistent data through event handling

### ✓ Authentication & Authorization
- JWT Bearer token support
- Role-based access control (RBAC)
- Claim-based authorization

### ✓ API Versioning Ready
- Controller-based routing
- Swagger documentation
- GraphQL support

### ✓ Resilience Patterns
- Circuit breaker with Polly
- Retry policies
- Health checks

### ✓ Message Processing
- RabbitMQ integration
- Domain event publishing
- Async message consumption

### ✓ Cloud Integration
- Azure Blob Storage for documents
- Azure Functions for background tasks
- Configurable for Azure Cosmos DB (migration ready)

### ✓ Logging & Monitoring
- Structured logging ready (Serilog)
- Health check endpoints
- Request/response logging middleware

## Building the Solution

### Debug Build
```bash
dotnet build --configuration Debug AppraisalService.sln
```

### Release Build
```bash
dotnet build --configuration Release AppraisalService.sln
```

## Running Tests (when added)

```bash
dotnet test AppraisalService.sln
```

## Deployment

### To Azure App Service
```bash
dotnet publish -c Release -o ./publish
```

### Using Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY publish .
ENTRYPOINT ["dotnet", "AppraisalService.API.dll"]
```

## Extension Points

1. **Add More Entities** - Add in Domain.Entities
2. **Add Commands/Queries** - Add in Application.CQRS
3. **Add Validators** - FluentValidation in Application
4. **Add Repositories** - Implement in Infrastructure.Persistence
5. **Add Azure Functions** - Add in Functions project
6. **Add GraphQL Types** - Extend GraphQL.AppraisalGraphQL

## Common Tasks

### Adding a New Feature

1. Create domain entity/aggregate in Domain.Entities
2. Create value objects if needed in Domain.ValueObjects
3. Add domain events in Domain.Events
4. Create repository interface in Domain.Repositories
5. Create DTOs in Application.DTOs
6. Create CQRS commands/queries in Application.CQRS
7. Create validations using FluentValidation
8. Implement repository in Infrastructure.Persistence
9. Add API controller in API.Controllers
10. Add GraphQL resolvers if needed

### Adding a Database Migration

```bash
cd src/AppraisalService.Infrastructure
dotnet ef migrations add MigrationName -p ../AppraisalService.API
dotnet ef database update -p ../AppraisalService.API
```

### Configuring RabbitMQ

1. Install RabbitMQ server locally or use Docker:
```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management
```

2. Update `appsettings.json` with connection details
3. Use `IMessagePublisher` to send messages
4. Implement message consumers

### Using Azure Blob Storage

1. Create Azure Storage Account
2. Create container for documents
3. Update connection string in `appsettings.json`
4. Use `IBlobStorageService` for file operations

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in appsettings.json
- Run migrations: `dotnet ef database update`

### RabbitMQ Connection Issues
- Ensure RabbitMQ service is running
- Check credentials in appsettings.json
- Verify network connectivity

### Token Validation Errors
- Verify JWT secret key matches across all instances
- Check token expiration time
- Ensure correct issuer/audience configuration

## Support & Documentation

- [Microsoft Learn - ASP.NET Core](https://learn.microsoft.com/aspnet/core)
- [Entity Framework Core](https://learn.microsoft.com/ef/core)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [HotChocolate GraphQL](https://chillicream.com/docs/hotchocolate)
- [RabbitMQ docs](https://www.rabbitmq.com/documentation.html)

## License

This project is part of the ERP Microservice suite.

---

**Last Updated:** March 12, 2026
**Version:** 1.0.0-alpha
