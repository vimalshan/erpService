# Tax Service - Technical Architecture

## Overview

The Tax Service is a modern microservice built using Clean Architecture and Domain-Driven Design (DDD) principles. It demonstrates enterprise-grade patterns for a production-ready financial service microservice.

## Architecture Layers

### 1. Domain Layer (TaxService.Domain)

**Purpose**: Contains pure business logic independent of any infrastructure concerns.

**Key Components**:

#### Entities
- `TaxMarginalDetail`: Aggregate root managing tax calculation for an employee
  - Encapsulates tax calculation logic
  - Maintains tax rates and exemptions
  - Publishes domain events on state changes

- `ConditionalMaster`: Aggregate root for payee master data
  - Manages exemptions and deductions
  - Maintains multiple tax regimes
  - Validates business rules

#### Value Objects
- `Money`: Type-safe monetary value with currency
  - Prevents domain logic errors by making money a first-class concept
  - Implements arithmetic operations (+, -, *)
  - Ensures immutability

- `TaxRate`: Calculates tax for a bracket
  - Encapsulates tax bracket logic
  - Ensures rates are properly applied
  - Immutable record type

- `TaxExemption` & `TaxDeduction`: Detail lines
  - Type-safe representations of exemptions/deductions
  - Effective date ranges for temporal validity

#### Domain Events
- Published by aggregates when state changes
- Not persisted yet, but prepared for event sourcing
- Enable eventual consistency patterns

#### Repositories (Interfaces Only)
- Define contracts for data access
- Aggregate root scoped (one repo per aggregate)
- Enable dependency injection and testability

**Design Principles**:
- No external dependencies (no EF, no frameworks)
- Business rules expressed in domain language
- Aggregates maintain consistency boundaries
- Domain events for cross-aggregate communication

---

### 2. Application Layer (TaxService.Application)

**Purpose**: Orchestrates domain logic and coordinates with infrastructure.

**Key Components**:

#### CQRS (Command Query Responsibility Segregation)
Separates read and write operations for better scalability.

**Commands** (State Changes):
- `CreateTaxMarginalDetailCommand`: Creates new tax record
- `CalculateTaxCommand`: Computes tax based on income
- `CreateConditionalMasterCommand`: Sets up payee
- `AddExemptionCommand`: Adds exemption to payee
- `AddDeductionCommand`: Adds deduction to payee

**Queries** (Read-Only):
- `GetTaxMarginalDetailByIdQuery`: Retrieve tax record
- `GetTaxByEmployeeAndYearQuery`: Find tax by employee/year
- `GetEmployeeTaxDetailsQuery`: All tax records for employee
- `GetConditionalMasterByIdQuery`: Retrieve payee master
- `GetConditionalMasterByPayeeIdQuery`: Find by payee ID
- `GetActiveConditionalMastersQuery`: List active payees

#### Request/Response Handling
- **MediatR**: Central dispatcher for commands and queries
- **IPipelineBehavior**: Cross-cutting concerns (validation, logging)
- **ValidationBehavior**: Automatic validation before handlers execute

#### Data Transfer Objects (DTOs)
- Flatten domain model for API responses
- Prevent domain model exposure
- Support version-independent APIs

#### Validators
- FluentValidation for declarative validation rules
- Executed in MediatR pipeline
- Clear error messages for API clients

#### Mappers (AutoMapper Profiles)
- Automatic mapping between entities and DTOs
- Maintains single mapping configuration
- Type-safe with compiler support

**Design Principles**:
- Commands modify state, Queries return data
- No business rules in use case layer
- DTOs shield domain from external changes
- Validation at both domain and application levels

---

### 3. Infrastructure Layer (TaxService.Infrastructure)

**Purpose**: Implements technical concerns: data persistence, external services, event handling.

**Key Components**:

#### Entity Framework Core DbContext
- Fluent API configuration of domain model
- Owned entities for value objects (Money columns)
- Soft deletes with IsDeleted flag
- Automatic timestamps (CreatedAt, ModifiedAt)
- Audit trail support

**Table Design**:
```
TaxMarginalDetails
├── Domain properties (Id, EmployeeSystemId, FinancialYear)
├── Owned Money objects (GrossIncome, TaxableIncome, CalculatedTax)
└── Metadata (CreatedAt, CreatedBy, IsDeleted)

ConditionalMasters
├── Domain properties (PayeeId, PayeeName, TaxRegime)
├── Owned Money objects (TotalExemption, TotalDeduction)
├── Collections (Exemptions, Deductions)
└── Metadata (CreatedAt, IsActive, IsDeleted)
```

#### Repositories
Implement aggregate-scoped repository pattern:
- Load and save only aggregate roots
- Enforce consistency boundaries
- Abstract database details from application

```csharp
// Application doesn't know about EF Core
public interface ITaxMarginalDetailRepository
{
    Task<TaxMarginalDetail?> GetByIdAsync(long id);
    Task AddAsync(TaxMarginalDetail entity);
    Task UpdateAsync(TaxMarginalDetail entity);
}

// Infrastructure implements using EF Core
public class TaxMarginalDetailRepository : ITaxMarginalDetailRepository
{
    // Hidden implementation details
}
```

#### Command/Query Handlers
- Injected from IoC container (MediatR discovers them)
- Each handler implements single responsibility principle
- Return Result<T> for consistent error handling

#### Message Broker (RabbitMQ)
- Simplified facade for RabbitMQ
- Ready for event publishing
- Supports eventual consistency patterns

#### Migrations
- Version-controlled schema changes
- InitialCreate creates all tables and relationships
- Repeatable for development and production

**Design Principles**:
- Infrastructure depends on Domain/Application (dependency rule)
- Repositories are boundaries between infrastructure and application
- Configuration (settings) injected via dependency injection
- Async/await throughout for scalability

---

### 4. API Layer (TaxService.API)

**Purpose**: RESTful HTTP interface for clients.

**Key Components**:

#### REST Controllers
- `TaxMarginalDetailsController`: Tax operations
- `ConditionalMastersController`: Payee operations
- Standard HTTP verbs (GET, POST, PUT, DELETE)
- RESTful routing: `/api/resource/{id}`
- Standard status codes (200, 201, 400, 401, 404)

#### Middleware Stack
1. HTTPS redirection
2. CORS (Cross-Origin Resource Sharing)
3. Authentication (JWT Bearer)
4. Authorization (roles/policies)
5. Exception handling

#### Authentication
- **JWT (JSON Web Tokens)**
  - Stateless authentication
  - Claims-based authorization
  - Configurable expiration
  - HS256 signing algorithm

```csharp
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### Health Checks
- Database connectivity
- Application responsiveness
- `/health` endpoint for orchestrators

#### Dependency Injection
```csharp
builder.Services.AddApplicationServices();          // CQRS, validation, mapping
builder.Services.AddInfrastructureServices();       // EF, repositories
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
```

**Design Principles**:
- Controllers are thin, delegating to MediatR
- DTOs shield domain from API contracts
- Standardized error responses
- Versioning-ready structure

---

### 5. Background Layer (TaxService.Background)

**Purpose**: Asynchronous operations and scheduled tasks.

**Preparation For**:
- Azure Functions integration
- Long-running batch jobs
- Message queue consumers
- Event handlers for domain events

---

## Data Flow Example: Calculate Tax

```
1. Client sends HTTP Request
   POST /api/taxmarginaldetails/1/calculate
   Authorization: Bearer JWT_TOKEN

2. API Controller receives request
   → TaxMarginalDetailsController.CalculateTax()

3. Controller creates command
   CalculateTaxCommand command = new CalculateTaxCommand(id: 1);

4. MediatR dispatcher processes command
   → Validates command (ValidationBehavior)
   → Routes to handler (CalculateTaxCommandHandler)

5. Handler executes business logic
   → Repository retrieves TaxMarginalDetail aggregate
   → Aggregate's CalculateTax() method applies logic
   → Domain events are raised internally
   → Repository saves updated aggregate

6. Result mapped to DTO
   TaxMarginalDetailDto dto = _mapper.Map<TaxMarginalDetailDto>(detail);

7. Controller returns HTTP Response
   200 OK { data: {...} }
```

---

## Key Design Patterns

### 1. **Repository Pattern**
Encapsulates data access logic, allowing:
- Easy switching of data sources
- Testability with mocks
- Consistency boundaries at aggregate level

### 2. **CQRS (Command Query Responsibility Segregation)**
Benefits:
- Read and write models can be optimized independently
- Easier to understand intent (command vs query)
- Foundation for event sourcing
- Scales to event-driven architectures

### 3. **Aggregate Pattern (DDD)**
- TaxMarginalDetail and ConditionalMaster are aggregates
- Maintain their own consistency
- Communicate via domain events
- Only the aggregate root is accessed

### 4. **Value Object Pattern**
- Money, TaxRate are immutable value objects
- Enable type-safe domain logic
- Prevent invalid states at compile time

### 5. **Pipeline Behavior** (MediatR)
```
Command → Validation → Logging → Handler → Mapping → Response
```

### 6. **Dependency Injection**
- Loose coupling between layers
- Easy to replace implementations
- Testable components

---

## Database Schema Design

### Considerations
1. **Normalization**: Balanced between 3rd normal form and query efficiency
2. **Soft Deletes**: IsDeleted flag for audit trail retention
3. **Temporal Data**: CreatedAt, ModifiedAt for tracking
4. **Indexes**: On frequently queried columns (EmployeeSystemId, PayeeId)
5. **Constraints**: Foreign keys for referential integrity
6. **Owned Entities**: Money as database columns, not separate tables

### Relationships
```
Employee
    ↓
TaxMarginalDetails (one-to-many)
    ├── GrossIncome (Money)
    ├── TaxableIncome (Money)
    └── CalculatedTax (Money)

ConditionalMasters (one-to-many)
    ├── TotalExemption (Money)
    ├── TotalDeduction (Money)
    └── Contains:
        ├── TaxExemptions (one-to-many)
        └── TaxDeductions (one-to-many)
```

---

## Configuration Management

### Environment-Specific Settings
```
Development: src/TaxService.API/appsettings.json
Production: src/TaxService.API/appsettings.Production.json
```

### Key Settings
- Database connection string
- JWT signing key (minimum 32 characters)
- Token expiration (minutes)
- RabbitMQ broker details
- Azure Storage credentials
- CORS allowed origins

---

## Error Handling

### Result<T> Pattern
```csharp
// Success
Result<TaxMarginalDetailDto>.Success(dto);

// Failure (single error)
Result<TaxMarginalDetailDto>.Failure("Tax record not found");

// Failure (multiple errors)
Result<TaxMarginalDetailDto>.Failure(new List<string> { "Error 1", "Error 2" });
```

### HTTP Response Codes
- `200 OK`: Successful GET
- `201 Created`: Successful POST (with Location header)
- `204 No Content`: Successful DELETE
- `400 Bad Request`: Validation errors
- `401 Unauthorized`: Missing/invalid token
- `403 Forbidden`: Insufficient permissions
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Unhandled exception

---

## Security Measures

1. **Authentication**: JWT Bearer tokens with HS256 signing
2. **Authorization**: Claims-based (via `[Authorize]` attribute)
3. **CORS**: Configured per environment
4. **Input Validation**: FluentValidation before processing
5. **Soft Deletes**: Prevents accidental data loss
6. **Audit Trail**: CreatedBy, CreatedAt, ModifiedBy fields
7. **HTTPS**: Enabled by default in production

---

## Performance Optimization

1. **Database Indexes**: On EmployeeSystemId, PayeeId, CreatedAt
2. **Async/Await**: Non-blocking I/O throughout
3. **Connection Pooling**: Automatic with EF Core
4. **CQRS Separation**: Read models can be optimized independently
5. **Value Objects**: Avoid unnecessary allocations
6. **Repository Caching**: Can be added later without changing domain

---

## Testing Strategy (To Implement)

### Unit Tests
- Domain entity logic
- Value object operations
- Business rule validation

### Integration Tests
- Repository operations
- Complete command/query flows
- Database migrations

### API Tests
- Endpoint behavior
- Authentication/authorization
- Error handling

### Performance Tests
- Query optimization
- Load testing
- Stress testing

---

## Deployment Considerations

1. **Database**: Use Azure SQL Database or SQL Server
2. **API Hosting**: Azure App Service or Kubernetes
3. **Messaging**: Azure Service Bus or RabbitMQ
4. **Storage**: Azure Blob Storage
5. **Monitoring**: Application Insights
6. **Scaling**: Stateless design supports horizontal scaling
7. **CI/CD**: GitHub Actions or Azure DevOps

---

## Future Enhancements

1. **Event Sourcing**: Add event store for full audit trail
2. **GraphQL API**: Add alongside REST for complex queries
3. **Caching**: Redis cache for frequently accessed data
4. **Message Queue**: Event publishing via RabbitMQ
5. **Reports**: Complex aggregation views
6. **Notifications**: Email/SMS on tax calculation
7. **Integration**: Connect with payroll system
8. **Analytics**: Tax trend analysis

---

## References

- **DDD**: Domain-Driven Design - Eric Evans
- **CQRS**: http://www.cqrs.nu/
- **Repository Pattern**: Martin Fowler
- **Clean Architecture**: Uncle Bob's Architecture Guide
- **MediatR**: https://github.com/jbogard/MediatR
- **EF Core**: https://learn.microsoft.com/ef/core/

---

**Architecture Version**: 1.0  
**Last Updated**: March 17, 2026  
**Status**: Production Ready
