# Employee Service Microservice

A comprehensive .NET 10 microservice for managing employee salary and increment operations with clean architecture, CQRS, and domain-driven design principles.

## Solution Architecture

### Project Structure

```
EmployeeService/
├── EmployeeService.Domain/          # Domain Layer (Entities, Value Objects, Events)
├── EmployeeService.Application/     # Application Layer (CQRS, DTOs, Behaviors)
├── EmployeeService.Infrastructure/  # Infrastructure Layer (EF Core, Repositories)
├── EmployeeService.API/             # API Layer (REST, GraphQL, Minimal APIs)
└── EmployeeService.Shared/          # Shared utilities and constants
```

## Implemented Features

### 1. Domain Layer
- **Entities**
  - `Employee`: Aggregate root with CTC management, employment status tracking
  - `SalaryIncrementLog`: Maintains history of salary increments
  
- **Value Objects**
  - `Money`: Currency-aware monetary amounts with arithmetic operations
  - `Percentage`: Validated percentage calculations
  
- **Domain Events**
  - `EmployeeCTCIncrementedEvent`: Triggered on salary increment
  - `EmployeeCTCIncrementRejectedEvent`: CTC increment rejection tracking
  - `EmployeeCTCModifiedEvent`: CTC modification events
  
- **Repository Interfaces**
  - `IEmployeeRepository`: CRUD operations for employees
  - `ISalaryIncrementLogRepository`: Increment history management

### 2. Application Layer (CQRs)

#### Commands
- `CreateEmployeeCommand`: Dynamic employee registration with CTC initialization
- `UpdateEmployeeCommand`: Personal information updates
- `ProcessSalaryIncrementCommand`: Salary increment processing
- `ModifyEmployeeCTCCommand`: Direct CTC modifications for special cases
- `TerminateEmployeeCommand`: Employee termination
- `DeleteEmployeeCommand`: Soft delete operations

#### Queries
- `GetEmployeeByIdQuery`: Retrieve single employee details
- `GetAllEmployeesQuery`: Paginated employee listing
- `GetEmployeesByCostCenterQuery`: Filter by cost center
- `GetSalaryIncrementLogsQuery`: Increment history with pagination
- `GetSalaryIncrementLogsByDateRangeQuery`: Range-based increment filtering
- `GetEmployeeCTCHistoryQuery`: Complete CTC change history
- `SearchEmployeesQuery`: Full-text search across employee records

#### DTOs
- `EmployeeDto`: Complete employee information
- `CreateEmployeeDto`: Employee creation payload
- `UpdateEmployeeDto`: Profile update payload
- `SalaryIncrementRequestDto`: Increment request payload
- `SalaryIncrementLogDto`: Increment log response

#### Pipeline Behaviors
- `ValidationBehavior`: FluentValidation integration
- `LoggingBehavior`: Request/response logging
- `ExceptionHandlingBehavior`: Cross-cutting exception handling

#### Validators
- Comprehensive FluentValidation rules for all commands
- Email, date, percentage, and amount validations

### 3. Infrastructure Layer

#### Entity Framework Core
- **DbContext**: `EmployeeDbContext` with:
  - Employee aggregate with complex value object mappings
  - SalaryIncrementLog with owned types
  - Automatic audit field updates
  - Comprehensive indexing strategy

- **Configuration**
  - SQL Server with decimal precision (19,2)
  - Soft delete support via `IsDeleted` flag
  - Automatic `CreatedAt`/`UpdatedAt` timestamp management
  
#### Repositories
- `EmployeeRepository`: EF Core implementation with:
  - GetBySystemIdAsync
  - GetByIdAsync
  - GetAllAsync
  - GetByCostCenterAsync
  - AddAsync, UpdateAsync, DeleteAsync
  - ExistsAsync validation

- `SalaryIncrementLogRepository`: Complete increment history management with:
  - GetByEmployeeIdAsync
  - GetLatestByEmployeeIdAsync
  - GetByStatusAsync
  - GetByDateRangeAsync
  - Pagination support

#### Database Configuration
- Connection string: `Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PAYDB`
- Migration support via EF Core Tools
- Automatic schema generation and seeding

### 4. API Layer

#### REST Endpoints
- **Employees Controller** (`/api/v1/employees`)
  - `GET /` - List all employees
  - `GET /{employeeSystemId}` - Get employee details
  - `POST /` - Create new employee
  - `PUT /{employeeSystemId}` - Update employee
  - `POST /{employeeSystemId}/increment` - Process salary increment
  - `GET /{employeeSystemId}/salary-history` - CTC history
  - `GET /search/find` - Search employees
  - `DELETE /{employeeSystemId}` - Soft delete employee

#### Minimal APIs
- Alternative minimal API endpoints for read operations
- Group-based routing under `/api/v1/minimal/employees`

#### Middleware
- **ExceptionHandlingMiddleware**: Global exception handling with:
  - Validation error responses
  - Unauthorized access handling
  - Business logic error responses
  - Standardized error format

#### Swagger/OpenAPI
- Auto-generated API documentation via Swashbuckle
- Accessible at `/swagger` endpoint

#### Authentication & Authorization
- JWT-based authentication
- Configurable role-based access control:
  - `AdminOnly`: Administrative operations
  - `ManagerOrAdmin`: Manager-level operations
  - `EmployeeAccess`: Employee-level read access
- Claims-based authorization

### 5. Shared Layer
- Common interfaces and utilities
- Configuration abstractions

## Technology Stack

### Core Framework
- **.NET 10** (net10.0 TFM)
- **ASP.NET Core** 10.0.x

### Data Access & ORM
- **Entity Framework Core** 9.0.0
- **SQL Server** with LocalDB support
- **Dapper** 2.1.35 (available for micro-queries if needed)

### Application Architecture
- **MediatR** 12.2.0 for CQRS pattern
- **AutoMapper** 12.0.1 for object mapping
- **FluentValidation** 11.9.1 for input validation

### Authentication & Security
- **Microsoft.AspNetCore.Authentication.JwtBearer** 8.0.0
- **System.IdentityModel.Tokens.Jwt** 7.6.0
- JWT token validation with configurable Claims

### API Documentation
- **Swashbuckle.AspNetCore** 10.1.5 for Swagger/OpenAPI
- Interactive API explorer at `/swagger`

### Logging
- **Serilog.AspNetCore** 9.0.0
- Structured logging with context enrichment

### Message Queue (Pre-configured)
- **RabbitMQ.Client** 6.8.1
- **MassTransit.RabbitMQ** 8.2.3
- Configuration ready in `appsettings.json`

### Cloud & Storage (Pre-configured)
- **Azure.Storage.Blobs** 12.20.0
- **Polly** 8.4.1 for circuit breaker patterns
- Configuration placeholders in `appsettings.json`

### Health Checks
- **AspNetCore.HealthChecks.SqlServer** 9.0.0
- **AspNetCore.HealthChecks.NpgSql** 9.0.0

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;..."
  },
  "JwtSettings": {
    "SecretKey": "your-secure-key-here",
    "Issuer": "EmployeeService",
    "Audience": "EmployeeServiceApi",
    "ExpirationMinutes": 60
  },
  "RabbitMQ": {
    "Hostname": "localhost",
    "Username": "guest",
    "Password": "guest"
  },
  "AzureStorage": {
    "ConnectionString": "...",
    "ContainerName": "employee-documents"
  }
}
```

## API Usage Examples

### Create Employee
```http
POST /api/v1/employees
Content-Type: application/json
Authorization: Bearer {token}

{
  "employeeSystemId": 1001,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "employeeCode": "EMP001",
  "joiningDate": "2023-01-15T00:00:00Z",
  "grossCTC": 500000,
  "basicSalary": 250000,
  "ctcEffectiveDate": "2023-01-15T00:00:00Z"
}
```

### Process Salary Increment
```http
POST /api/v1/employees/1001/increment
Content-Type: application/json
Authorization: Bearer {token}

{
  "incrementPercentage": 10,
  "effectiveDate": "2024-04-01T00:00:00Z"
}
```

### Get CTC History
```http
GET /api/v1/employees/1001/salary-history
Authorization: Bearer {token}
```

## Health Check Endpoints
- `GET /health` - Overall service health
- `GET /health/ready` - Readiness check

## Building & Running

### Build Solution
```powershell
dotnet build EmployeeService.slnx
```

### Create Migrations
```powershell
cd EmployeeService.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../EmployeeService.API
dotnet ef database update --startup-project ../EmployeeService.API
```

### Run API
```powershell
cd EmployeeService.API
dotnet run
```

## Features Ready for Integration

The following features are configured and ready for implementation:

1. **RabbitMQ Message Queue**
   - Configuration in `appsettings.json`
   - Ready for MassTransit integration
   - Event publishing on domain events

2. **Azure Blob Storage**
   - Pre-configured for document/image storage
   - Ready for employee documents/profile pictures

3. **Circuit Breaker Policies**
   - Polly framework configured
   - Can be applied to external API calls

4. **Azure Functions**
   - Infrastructure ready for background job creation
   - Can process async salary increment requests

5. **Advanced Health Checks**
   - SQL Server health check implemented
   - Can add RabbitMQ, Azure Storage checks

6. **Domain Events Propagation**
   - Domain events captured in aggregate roots
   - Ready for async event handlers

## Database Schema

### EMPLOYEE_INCCTC Table
- `Id` (PK, BIGINT)
- `EmployeeSystemId` (BIGINT, Unique)
- `FirstName`, `LastName`, `MiddleName` (VARCHAR)
- `Email` (VARCHAR, Indexed)
- `PhoneNumber` (VARCHAR)
- `EmployeeCode` (VARCHAR, Indexed)
- `GrossCTC` (DECIMAL 19,2)
- `BasicSalary` (DECIMAL 19,2)
- `CTCEffectiveDate` (DATETIME2)
- `EmploymentStatus` (VARCHAR)
- `CostCenterId` (VARCHAR, Indexed)
- `JoiningDate`, `TerminationDate` (DATETIME2)
- `CreatedAt`, `UpdatedAt`, `CreatedBy`, `UpdatedBy` (Audit fields)
- `IsDeleted` (BIT)

### SALARY_INCREMENT_LOG Table
- `Id` (PK, BIGINT)
- `EmployeeSystemId` (BIGINT, Indexed)
- `OldCTC`, `NewCTC` (DECIMAL 19,2)
- `IncrementPercentage` (DECIMAL 5,2)
- `EffectiveDate` (DATETIME2, Indexed)
- `ApprovedBy` (BIGINT)
- `ApprovedOn` (DATETIME2)
- `ApprovalComments` (VARCHAR 500)
- `Status` (VARCHAR 50, Indexed)
- `IsDeleted` (BIT)

## Next Steps

To fully utilize this service, consider implementing:

1. **Message Consumers** for RabbitMQ events
2. **Integration with existing HR systems**
3. **Azure Functions** for background job processing
4. **Advanced audit logging** with Event Store
5. **Caching layer** (Redis) for frequently accessed data
6. **API Gateway** for service orchestration
7. **Unit & Integration Tests** using xUnit
8. **GraphQL** endpoint enhancement with more complex queries

## Project Dependencies

All necessary NuGet packages have been installed. The solution is ready to:
- ✅ Build successfully
- ✅ Connect to SQL Server LocalDB
- ✅ Execute CQRS operations
- ✅ Validate inputs with FluentValidation
- ✅ Map objects with AutoMapper
- ✅ Generate API documentation
- ✅ Handle JWT authentication
- ✅ Integrate with external services (RabbitMQ, Azure, Polly)

## Notes

- JWT Secret Key should be updated to a secure value for production
- SQL Server LocalDB connection is configured by default
- All domain operations include proper validation and error handling
- Event sourcing ready via domain events
- Soft delete enabled for data retention  
- Comprehensive audit trail via CreatedAt/UpdatedAt fields
