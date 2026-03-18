# Payroll Microservice - Complete Solution

## Overview
This is a comprehensive microservice solution for payroll management, built with .NET 8, following Domain-Driven Design (DDD) and CQRS patterns.

## Architecture

### Layered Architecture

```
┌─────────────────────────────────────────────┐
│         Presentation Layer (API)            │
│     REST API | GraphQL | Minimal APIs       │
└─────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────┐
│      Application Layer (CQRS)               │
│  Commands | Queries | DTOs | Validators    │
└─────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────┐
│          Domain Layer (DDD)                 │
│  Entities | Aggregates | Value Objects     │
│         Events | Interfaces                │
└─────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────┐
│     Infrastructure Layer                    │
│   EF Core | Repositories | External Svcs   │
└─────────────────────────────────────────────┘
```

### Project Structure

```
PayrollServices/
├── PayrollServices.Domain/              # Domain Layer
│   ├── Entities/                        # Business entities
│   ├── ValueObjects/                    # Value objects
│   ├── Events/                          # Domain events
│   └── Interfaces/                      # Domain contracts
│
├── PayrollServices.Application/         # Application Layer
│   ├── Commands/                        # CQRS Commands
│   ├── Queries/                         # CQRS Queries
│   ├── DTOs/                           # Data Transfer Objects
│   ├── Validators/                      # FluentValidation
│   ├── Mappings/                        # AutoMapper profiles
│   └── Services/                        # Command/Query handlers
│
├── PayrollServices.Infrastructure/      # Infrastructure Layer
│   ├── Data/                           # EF Core DbContext
│   ├── Repositories/                    # Data access layer
│   ├── Messaging/                       # RabbitMQ
│   ├── ExternalServices/                # Azure Blob Storage
│   └── Services/                        # Polly policies
│
├── PayrollServices.API/                 # Presentation Layer
│   ├── Controllers/                     # REST endpoints
│   ├── GraphQL/                        # GraphQL queries/mutations
│   ├── Middleware/                      # Custom middleware
│   └── Extensions/                      # DI setup
│
├── PayrollServices.Functions/           # Azure Functions
│   ├── ProcessPayrollFunction/          # Timer-triggered
│   ├── DisbursePayrollQueueFunction/    # Queue-triggered
│   └── ProcessPayrollDocumentFunction/  # Blob-triggered
│
└── PayrollServices.Tests/               # Unit Tests
```

## Technology Stack

### Core Framework
- **.NET 8.0** - Latest LTS version
- **C# 12** - Latest language features
- **ASP.NET Core 8** - Web API framework

### Data Access
- **Entity Framework Core 8** - ORM for SQL Server
- **SQL Server 2019+** - Relational database
- **Dapper** - Micro-ORM for complex queries

### API & Communication
- **Swagger/OpenAPI** - API documentation
- **GraphQL (Hot Chocolate)** - Alternative query language
- **RabbitMQ** - Message broker
- **WebSockets** - Real-time updates (optional)

### Authentication & Security
- **JWT (JSON Web Tokens)** - Token-based authentication
- **HTTPS** - Secure communication
- **Azure Key Vault** - Secrets management (optional)

### Cloud Services
- **Azure Functions** - Serverless computing
- **Azure Blob Storage** - File storage
- **Application Insights** - Monitoring

### Patterns & Libraries
- **CQRS** - Command Query Responsibility Segregation
- **Domain-Driven Design** - Business-focused architecture
- **Event Sourcing** - Change tracking
- **MediatR** - Mediator pattern implementation
- **AutoMapper** - Object mapping
- **FluentValidation** - Data validation
- **Polly** - Resilience and fault handling

### Testing
- **xUnit** - Testing framework
- **Moq** - Mocking library

## Database Schema

### Tables

#### PAYROLL_BATCH
```sql
BatchId (PK)
BatchMonth (VARCHAR(7))
BatchStatus (CHAR(1))
CreatedBy (BIGINT)
CreatedOn (DATETIME)
UpdatedOn (DATETIME)
UpdatedBy (BIGINT)
```

#### PAY_TRANDET
```sql
TrnId (PK)
TrnEmpSysId (BIGINT)
TrnBatchId (FK)
TrnMonth (VARCHAR(7))
TrnGross (DECIMAL)
TrnDeductions (DECIMAL)
TrnNet (DECIMAL)
TrnStatus (CHAR(1))
TrnCreatedBy (BIGINT)
TrnCreatedOn (DATETIME)
```

#### PAY_ARR
```sql
ArId (PK)
PayEmpSysId (BIGINT)
ArAmount (DECIMAL)
ArType (CHAR(1))
ArDate (DATETIME)
ArDescription (VARCHAR(500))
ArCreatedBy (BIGINT)
ArCreatedOn (DATETIME)
```

## API Endpoints

### REST API

#### Batch Management
- `GET /api/payrollbatches/{batchId}` - Get batch by ID
- `GET /api/payrollbatches/by-month/{month}` - Get batch by month
- `GET /api/payrollbatches` - Get all batches
- `POST /api/payrollbatches` - Create batch
- `POST /api/payrollbatches/process-monthly-salary` - Process monthly salary

#### Transactions
- `GET /api/payrolltransactions/batch/{batchId}` - Get batch transactions
- `GET /api/payrolltransactions/employee/{employeeId}` - Get employee payroll
- `POST /api/payrolltransactions` - Create transaction
- `PUT /api/payrolltransactions/{transactionId}/disburse` - Disburse payroll

#### Adjustments
- `POST /api/adjustments` - Create adjustment (allowance/deduction)

#### Health
- `GET /health` - Health check

### GraphQL Endpoints

**Query Endpoint:** `/graphql` (POST)

```graphql
query {
  getBatch(batchId: 1) {
    batchId
    batchMonth
    status
  }
  getAllBatches {
    batchId
    batchMonth
  }
}

mutation {
  processMonthlySalary(monthYear: "2024-01", processedBy: 1) {
    batchId
    success
    message
  }
}
```

**GraphQL UI:** `http://localhost:5000/graphql` (Banana Cake Pop)

## Authentication

### JWT Token

Generate token with claims:
- `sub` - User ID
- `name` - Username
- `email` - Email address
- `roles` - User roles

**Usage:**
```bash
curl -H "Authorization: Bearer <token>" \
     http://localhost:5000/api/payrollbatches
```

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PAYDB;..."
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-32-chars-minimum",
    "Issuer": "PayrollService",
    "Audience": "PayrollServiceUsers",
    "ExpirationMinutes": 60
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest"
  },
  "AzureStorage": {
    "ConnectionString": "DefaultEndpointsProtocol=https;..."
  }
}
```

## Running the Application

### Prerequisites
- .NET 8.0 SDK
- SQL Server 2019+ or LocalDB
- RabbitMQ (optional)
- Azure Storage Account (optional)

### Setup Database

```bash
# Navigate to API project
cd PayrollServices.API

# Create initial migration
dotnet ef migrations add InitialCreate --project ../PayrollServices.Infrastructure

# Update database
dotnet ef database update
```

### Run API

```bash
dotnet run
```

Access:
- **Swagger:** http://localhost:5000/swagger/index.html
- **GraphQL:** http://localhost:5000/graphql
- **Health:** http://localhost:5000/health

### Run Azure Functions

```bash
cd PayrollServices.Functions
func start
```

## Features Implemented

✅ **Domain Layer**
- Entity definitions with validation
- Value objects for domain concepts
- Domain events
- Repository interfaces

✅ **Application Layer**
- CQRS pattern (Commands & Queries)
- MediatR integration
- DTOs for data transfer
- FluentValidation for input validation
- AutoMapper for object mapping

✅ **Infrastructure Layer**
- Entity Framework Core with SQL Server
- Repository pattern with UnitOfWork
- RabbitMQ publisher & consumer base
- Azure Blob Storage integration
- Polly circuit breaker policies

✅ **API Layer**
- REST controllers with Swagger documentation
- GraphQL support with Hot Chocolate
- JWT authentication
- Global exception handling middleware
- Health checks

✅ **Azure Functions**
- Timer-triggered payroll processing
- Queue-triggered disbursement
- Blob-triggered document processing

✅ **Security & Resilience**
- JWT token-based authentication
- Polly circuit breaker with retry policies
- Input validation
- Exception handling
- Health checks

## Deployment

### Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY --from=build /app .
ENTRYPOINT ["dotnet", "PayrollServices.API.dll"]
```

### Azure
1. Create App Service
2. Configure SQL Server connection
3. Set environment variables in Key Vault
4. Deploy using Azure DevOps or GitHub Actions

## Testing

Run unit tests:
```bash
dotnet test PayrollServices.Tests
```

Coverage includes:
- Entity creation and state transitions
- Value object validation
- Business logic

## Future Enhancements

- [ ] Event sourcing
- [ ] SAGA pattern for distributed transactions
- [ ] Multi-tenant support
- [ ] Advanced reporting
- [ ] Mobile app integration
- [ ] Real-time banking integration
- [ ] Machine learning for anomaly detection
- [ ] Audit logging and compliance

## Contact & Support

For issues, feature requests, or contributions, please open an issue on GitHub.

---

**Version:** 1.0.0  
**Last Updated:** January 2024
