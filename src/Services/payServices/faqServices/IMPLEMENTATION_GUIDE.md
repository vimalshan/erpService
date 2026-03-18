# FAQ Microservice - Implementation Guide

## Project Overview
This is a comprehensive FAQ microservice built with .NET 10.0, following clean architecture principles with clear separation of concerns through Domain, Application, Infrastructure, and API layers.

## Architecture Layers

### 1. **Domain Layer** (FaqServices.Domain)
Core business entities and rules with no dependencies on external frameworks.

**Key Components:**
- **Entities**
  - `FaqGrade` - Represents FAQ categories/grades
  - `FaqQuestion` - Represents FAQ questions
  - `FaqAnswer` - Represents FAQ answers
  
- **Base Classes**
  - `BaseEntity` - Provides audit fields (CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, IsDeleted)
  - `IDomainEvent` - Interface for domain events
  
- **Domain Events**
  - `FaqGradeCreatedEvent`, `FaqGradeUpdatedEvent`
  - `FaqQuestionCreatedEvent`, `FaqQuestionUpdatedEvent`
  - `FaqAnswerCreatedEvent`, `FaqAnswerUpdatedEvent`
  
- **Repository Interfaces**
  - `IUnitOfWork` - Coordinates repositories and transaction management
  - `IFaqGradeRepository` - Grade data access
  - `IFaqQuestionRepository` - Question data access
  - `IFaqAnswerRepository` - Answer data access

### 2. **Infrastructure Layer** (FaqServices.Infrastructure)
Database access, repositories, and external service implementations.

**Key Components:**
- **DbContext**
  - `FaqDbContext` - Entity Framework Core database context with proper configuration
  
- **Repositories**
  - `FaqGradeRepository` - Grade CRUD operations
  - `FaqQuestionRepository` - Question CRUD with grade filtering
  - `FaqAnswerRepository` - Answer CRUD with question filtering
  
- **Unit of Work**
  - `UnitOfWork` - Manages repository coordination and transactions
  
- **Migrations**
  - `20260317000001_InitialCreate` - Creates FAQ_GRADE, FAQ_QUESTION, FAQ_ANSWER tables
  - `FaqDbContextModelSnapshot` - EF Core migration snapshot
  - `DatabaseInitializer` - Handles database migrations and initialization
  
- **Configuration**
  - `ServiceCollectionExtensions` - DI configuration for Infrastructure services

**Database Schema:**
- FAQ_GRADE: Stores FAQ categories (GradeName, Description, SortOrder, IsActive)
- FAQ_QUESTION: Stores questions (QuestionText, GradeId FK, ImageBlobUrl)
- FAQ_ANSWER: Stores answers (AnswerText, QuestionId FK, IsCorrect, ImageBlobUrl)
- All tables have audit fields and soft-delete support

### 3. **Application Layer** (FaqServices.Application)
CQRS pattern implementation with commands, queries, and business logic.

**Commands (Write Operations):**
- Grade Commands
  - `CreateGradeCommand` → `CreateGradeCommandHandler`
  - `UpdateGradeCommand` → `UpdateGradeCommandHandler`
  - `DeleteGradeCommand` → `DeleteGradeCommandHandler`
  
- Question Commands
  - `CreateQuestionCommand` → `CreateQuestionCommandHandler`
  - `UpdateQuestionCommand` → `UpdateQuestionCommandHandler`
  - `DeleteQuestionCommand` → `DeleteQuestionCommandHandler`
  
- Answer Commands
  - `CreateAnswerCommand` → `CreateAnswerCommandHandler`
  - `UpdateAnswerCommand` → `UpdateAnswerCommandHandler`
  - `DeleteAnswerCommand` → `DeleteAnswerCommandHandler`

**Queries (Read Operations):**
- Grade Queries
  - `GetAllGradesQuery` - Get all grades with question count
  - `GetGradeByIdQuery` - Get single grade
  
- Question Queries
  - `GetAllQuestionsQuery` - Get all questions
  - `GetQuestionByIdQuery` - Get question with answers
  - `GetQuestionsByGradeIdQuery` - Get questions by grade
  
- Answer Queries
  - `GetAnswersByQuestionIdQuery` - Get answers for a question
  - `GetAnswerByIdQuery` - Get single answer

**DTOs:**
- `FaqGradeDto` - Grade data transfer object
- `FaqQuestionDto` - Question data transfer object with nested answers
- `FaqAnswerDto` - Answer data transfer object

**Validation:**
- FluentValidation rules for all commands
- Validators ensure data integrity before processing

**AutoMapper:**
- `FaqMappingProfile` - Maps between entities and DTOs

**DI Configuration:**
- `ServiceCollectionExtensions` - Registers MediatR, AutoMapper, and Validators

### 4. **API Layer** (FaqServices.API)
REST API endpoints and middleware configuration.

**Endpoints:**
- `/api/grades` - Grade management
  - GET `/` - Get all grades
  - GET `/{id}` - Get single grade
  - POST `/` - Create grade
  - PUT `/{id}` - Update grade
  - DELETE `/{id}` - Delete grade
  
- `/api/questions` - Question management
  - GET `/` - Get all questions
  - GET `/by-grade/{gradeId}` - Get questions by grade
  - GET `/{id}` - Get single question with answers
  - POST `/` - Create question
  - PUT `/{id}` - Update question
  - DELETE `/{id}` - Delete question
  
- `/api/answers` - Answer management
  - GET `/by-question/{questionId}` - Get answers by question
  - GET `/{id}` - Get single answer
  - POST `/` - Create answer
  - PUT `/{id}` - Update answer
  - DELETE `/{id}` - Delete answer

**Middleware:**
- Authentication (JWT Bearer)
- Authorization
- CORS
- Serilog request logging
- Health checks
- Swagger/OpenAPI documentation

**Services:**
- Health Checks: `/health`
- OpenAPI: `/openapi/v1.json`
- Swagger UI: `/swagger/index.html`

## Key Features Implemented

### ✅ Completed
1. **Database Schema** - SQL Server with proper relationships and indices
2. **Domain Layer** - Entities with soft-delete, audit fields, and domain events
3. **CQRS Pattern** - Complete command/query separation with MediatR
4. **Repository Pattern** - Generic repositories with UnitOfWork
5. **EF Core Migrations** - Automatic database initialization
6. **FluentValidation** - Input validation on all commands
7. **AutoMapper** - DTO mapping configuration
8. **REST API** - Full CRUD endpoints with minimal APIs
9. **JWT Authentication** - Bearer token authentication
10. **Health Checks** - Database and API health monitoring
11. **Swagger/OpenAPI** - API documentation and testing
12. **Serilog** - Structured logging to console and files
13. **Soft-Delete** - Logical deletion with audit trails

### 🔄 Partially Completed
**GraphQL** - Infrastructure ready, queries/mutations need configuration

### ⏳ To Be Implemented
1. **RabbitMQ Integration** - Message publishing and consuming
2. **Azure Functions** - Background job execution
3. **Blob Storage** - Image management
4. **Polly Resilience** - Circuit breaker and retry policies
5. **Domain Event Publishers** - Publishing domain events to message queue
6. **Custom Authentication** - Login/token generation endpoints

## Configuration Files

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PAYDB;..."
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-min-32-characters",
    "Issuer": "FaqServices.API",
    "Audience": "FaqServices.Client",
    "ExpirationInMinutes": 60
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest"
  }
}
```

### Connection String
**Local Development:**
```
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PAYDB;Integrated Security=True;
```

## Running the Application

### Prerequisites
- .NET 10.0 SDK
- SQL Server LocalDB or full SQL Server
- Visual Studio 2024 or VS Code

### Build
```powershell
dotnet build
```

### Database Setup
```powershell
# Apply migrations (automatic on first run via DatabaseInitializer)
# Or manually:
dotnet ef database update -p src/FaqServices.Infrastructure -s src/FaqServices.API
```

### Run
```powershell
dotnet run --project src/FaqServices.API
```

### Test
1. Navigate to `https://localhost:5001/swagger/index.html`
2. Use Swagger UI to test all endpoints
3. Check `/health` endpoint for system status

## Project Structure
```
FaqServices/
├── FAQ/
│   └── FAQ-Module.sql                 # Database creation script
├── src/
│   ├── FaqServices.API/               # Web API
│   │   ├── Endpoints/                 # Minimal API endpoints
│   │   ├── Program.cs                 # Startup configuration
│   │   └── appsettings.json           # Configuration
│   ├── FaqServices.Application/       # Business logic (CQRS)
│   │   ├── Features/                  # Commands/Queries organized by feature
│   │   ├── Common/                    # DTOs, Mappings, Behaviors
│   │   └── Extensions/                # DI configuration
│   ├── FaqServices.Domain/            # Core business entities
│   │   ├── Entities/                  # Domain models
│   │   ├── Interfaces/                # Repository contracts
│   │   ├── Events/                    # Domain events
│   │   └── Common/                    # Base classes
│   ├── FaqServices.Infrastructure/    # Data access
│   │   ├── Data/                      # DbContext and configurations
│   │   ├── Repositories/              # Repository implementations
│   │   ├── Migrations/                # EF Core migrations
│   │   └── Extensions/                # DI configuration
│   └── FaqServices.Functions/         # Azure Functions (future)
└── FaqServices.slnx                   # Solution file
```

## Next Steps

### Immediate
1. Update JWT secret key in appsettings.json
2. Configure database connection string
3. Test all API endpoints via Swagger UI
4. Review and adjust validation rules as needed

### Short-term
1. Implement RabbitMQ message publishing for domain events
2. Create message consumer services
3. Add Azure Blob Storage for image management
4. Implement GraphQL queries and mutations

### Medium-term
1. Add authentication endpoints (login/register)
2. Implement Polly resilience policies
3. Add Azure Functions for background tasks
4. Implement caching strategies

### Long-term
1. Add multi-tenancy support
2. Implement full-text search
3. Add audit logging service
4. Performance optimization and monitoring

## Dependencies Summary

**Domain Layer:**
- No external dependencies (pure C#)

**Application Layer:**
- MediatR 14.1.0 (Command/Query pattern)
- AutoMapper 16.1.1 (Object mapping)
- FluentValidation 12.1.1 (Input validation)

**Infrastructure Layer:**
- EntityFrameworkCore 10.0.5 (ORM)
- EntityFrameworkCore.SqlServer 10.0.5 (SQL Server provider)
- Dapper 2.1.72 (Optional micro-ORM for complex queries)
- Polly 8.6.6 (Resilience policies)
- RabbitMQ.Client 7.2.1 (Message broker)
- Azure.Storage.Blobs 12.27.0 (Blob storage)

**API Layer:**
- Serilog 10.0.0 (Structured logging)
- Swashbuckle.AspNetCore 10.1.5 (Swagger/OpenAPI)
- JWT Bearer Authentication 10.0.5
- HealthChecks 9.0.0 (SQL Server & RabbitMQ monitoring)
- HotChocolate 15.1.12 (GraphQL - ready for configuration)

## Best Practices Implemented

1. **Separation of Concerns** - Clean architecture with distinct layers
2. **CQRS Pattern** - Commands for writes, Queries for reads
3. **Repository Pattern** - Data access abstraction
4. **Unit of Work** - Transaction coordination
5. **Dependency Injection** - Loose coupling via DI
6. **Validation** - FluentValidation for input integrity
7. **Audit Trail** - CreatedAt, UpdatedAt fields on all entities
8. **Soft Delete** - IsDeleted flag instead of hard deletes
9. **Domain Events** - Infrastructure for event-driven architecture
10. **Minimal APIs** - Modern, lightweight endpoint configuration
11. **Structured Logging** - Serilog for production-ready logging
12. **Health Monitoring** - Built-in health check endpoints

## Support & Documentation

- **OpenAPI/Swagger**: Available at `/swagger/index.html`
- **Health Status**: Check at `/health`
- **Logs**: Stored in `logs/` directory (rolling daily)

---

**Last Updated:** March 17, 2026
**Version:** 1.0.0
**Status:** Core functionality implemented, ready for advanced features (RabbitMQ, Azure Functions, GraphQL completion)
