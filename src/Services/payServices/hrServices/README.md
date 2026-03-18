# HR Microservice - Setup & Building Guide

## Overview

This is a comprehensive Human Resources Management Microservice built with cutting-edge architecture patterns and technologies including:

- **Clean Architecture**: Separated layers (Domain, Application, Infrastructure, API)
- **CQRS Pattern**: Command Query Responsibility Segregation
- **Event-Driven Architecture**: Domain events and integration events
- **Entity Framework Core 8**: ORM with migrations
- **MediatR**: Mediator pattern for reducing dependencies
- **JWT Authentication**: Secure API endpoints
- **RabbitMQ**: Asynchronous messaging
- **Health Checks**: Application and database health monitoring
- **Polly**: Resilience patterns (Circuit Breaker, Retry)
- **Azure Functions**: Background task processing
- **Swagger/OpenAPI**: API documentation

## Database Setup

### Connection String
```
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PAYDB;Integrated Security=true;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name="HRService";Command Timeout=0
```

### Create Initial Database

1. **Open Visual Studio Package Manager Console**
   - Tools → NuGet Package Manager → Package Manager Console

2. **Create Initial Migration**
   ```powershell
   Add-Migration InitialCreate -Project HRService.Infrastructure
   ```

3. **Update Database**
   ```powershell
   Update-Database -Project HRService.Infrastructure
   ```

### Run SQL Script
Alternatively, execute the SQL script in [HR-Module.sql](./HR/HR-Module.sql) directly in SQL Server Management Studio:

```sql
USE [PAYDB]
GO
-- Run the entire HR-Module.sql script
```

## Building the Solution

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 (or VS Code with necessary extensions)

### Build Steps

1. **Restore NuGet Packages**
   ```bash
   dotnet restore HRService.sln
   ```

2. **Build Solution**
   ```bash
   dotnet build HRService.sln
   ```

3. **Run Database Migrations**
   ```powershell
   # In Package Manager Console
   Update-Database
   ```

4. **Run the API**
   ```bash
   cd HRService.API
   dotnet run
   ```

The API will be available at `https://localhost:5001`

## API Documentation

### Swagger UI
- **Endpoint**: `https://localhost:5001/swagger/index.html`
- Explore and test all REST endpoints
- Try-it-out feature for testing

### Health Check
- **Endpoint**: `https://localhost:5001/health`
- Returns application and database health status

### Authentication

#### Get JWT Token
1. Navigate to Swagger UI
2. Authenticate with valid credentials (implementation specific)
3. Copy the JWT token
4. Click "Authorize" button
5. Enter: `Bearer {token}`
6. Click "Authorize"

#### Sample JWT Structure
```json
{
  "nameid": "employee-id",
  "email": "employee@company.com",
  "unique_name": "Employee Name",
  "role": ["Manager", "HR"]
}
```

## Project Structure

```
HRService/
├── HRService.Domain/              # Domain layer (entities, aggregates, events)
│   ├── Common/                    # Base classes
│   ├── Entities/                  # Domain entities
│   ├── ValueObjects/              # Value objects
│   ├── Events/                    # Domain events
│   └── Exceptions/                # Domain exceptions
│
├── HRService.Application/         # Application layer (CQRS, DTOs, services)
│   ├── DTOs/                      # Data transfer objects
│   ├── Commands/                  # CQRS commands
│   ├── Queries/                   # CQRS queries
│   ├── Handlers/                  # Command & query handlers
│   ├── Services/                  # Application services
│   ├── Validators/                # Validation rules
│   └── Mappings/                  # AutoMapper profiles
│
├── HRService.Infrastructure/      # Infrastructure layer (EF, Repositories, external services)
│   ├── Data/                      # DbContext and configurations
│   ├── Repositories/              # Repository implementations
│   ├── MessageBroker/             # RabbitMQ integration
│   └── Logging/                   # Logging configuration
│
├── HRService.Common/              # Shared utilities
│   ├── Security/                  # JWT services
│   └── Resilience/                # Polly policies
│
├── HRService.API/                 # API layer (REST, Controllers, Middleware)
│   ├── Controllers/               # API controllers
│   ├── Middleware/                # Custom middleware
│   ├── GraphQL/                   # GraphQL schema (future)
│   └── Extensions/                # Extension methods
│
├── HRService.Functions/           # Azure Functions (background tasks)
│   ├── EmployeeProcessing/
│   ├── SalaryCalculation/
│   └── ReportGeneration/
│
└── HRService.Tests/               # Unit tests

```

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PAYDB;..."
  },
  "Jwt": {
    "SecretKey": "your-secret-key-minimum-32-characters",
    "Issuer": "HRService",
    "Audience": "HRServiceAPI",
    "ExpirationMinutes": 60
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Port": 5672,
    "Username": "guest",
    "Password": "guest",
    "VirtualHost": "/",
    "Enabled": true
  },
  "AzureStorage": {
    "ConnectionString": "DefaultEndpointsProtocol=https;...",
    "BlobContainerName": "stationery-images",
    "Enabled": false
  }
}
```

## Key Features Implemented

### ✅ Domain Layer
- [x] Employee aggregate root with full lifecycle management
- [x] Leave management (request, approval, rejection)
- [x] Attendance tracking
- [x] Salary management
- [x] Performance review system
- [x] Value objects (Email, PhoneNumber, Money, EmployeeCode)
- [x] Domain events

### ✅ Application Layer
- [x] CQRS pattern with commands and queries
- [x] DTOs for data transfer
- [x] FluentValidation for input validation
- [x] AutoMapper for entity-DTO mapping
- [x] Service interfaces for business logic

### ✅ Infrastructure Layer
- [x] Entity Framework Core 8 DbContext
- [x] Entity configurations with Fluent API
- [x] Repository pattern with generic base
- [x] Unit of Work pattern for transaction management
- [x] RabbitMQ message broker integration

### ✅ API Layer
- [x] REST API with standard HTTP verbs
- [x] JWT authentication and authorization
- [x] Swagger/OpenAPI documentation
- [x] Global exception handling middleware
- [x] CORS configuration
- [x] Health check endpoints

### ✅ Cross-Cutting Concerns
- [x] Serilog for structured logging
- [x] Polly for circuit breaker and retry policies
- [x] Domain event handlers
- [x] Validation pipeline

## REST API Endpoints

### Employees
- `POST /api/employees` - Create employee
- `GET /api/employees/{id}` - Get employee by ID
- `GET /api/employees` - Get all employees (paginated)
- `POST /api/employees/{id}/terminate` - Terminate employee
- `POST /api/employees/{id}/suspend` - Suspend employee
- `POST /api/employees/{id}/resume` - Resume employee

### Leave Management
- `POST /api/leaves` - Request leave
- `GET /api/leaves/{id}` - Get leave details
- `POST /api/leaves/{id}/approve` - Approve leave
- `POST /api/leaves/{id}/reject` - Reject leave

### Attendance
- `POST /api/attendance` - Mark attendance
- `GET /api/attendance/employee/{employeeId}` - Get employee attendance

### Salary
- `POST /api/salary` - Create/update salary
- `GET /api/salary/employee/{employeeId}` - Get employee salary

## Running with Docker

### Build Docker Image
```bash
docker build -t hrservice:latest .
```

### Run Container
```bash
docker run -p 5001:443 -e ASPNETCORE_URLS=https://+:443 hrservice:latest
```

## Testing

### Run Unit Tests
```bash
dotnet test HRService.Tests
```

### Test Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

## Deployment

### Azure Deployment
1. Create Azure SQL Database
2. Create App Service
3. Update connection string
4. Deploy using Visual Studio or Azure DevOps

### Environment Variables Required
```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=<production-db>
Jwt__SecretKey=<production-secret-key>
RabbitMQ__Host=<rabbitmq-host>
AzureStorage__ConnectionString=<azure-storage-connection>
```

## Troubleshooting

### Database Connection Issues
1. Verify SQL Server is running
2. Check connection string format
3. Run migrations: `Update-Database`

### JWT Token Errors
1. Verify secret key is set correctly
2. Check token hasn't expired
3. Ensure token format is: `Bearer {token}`

### RabbitMQ Issues
1. Verify RabbitMQ service is running (localhost:15672 for admin)
2. Check credentials in appsettings.json
3. Verify exchange and queue names

## Future Enhancements

- [ ] GraphQL API implementation
- [ ] Batch operations for payroll
- [ ] Advanced reporting and analytics
- [ ] Machine learning for salary recommendations
- [ ] Mobile app for employee self-service
- [ ] Integration with Active Directory
- [ ] Advanced audit trail and compliance
- [ ] Real-time notifications

## Contributing

Follow these guidelines:
1. Create feature branches
2. Follow SOLID principles
3. Write unit tests for new features
4. Update documentation
5. Create pull requests for review

## License

Internal - Proprietary

## Support

For issues or questions:
1. Check documentation
2. Review logs in `logs/` directory
3. Contact development team

---

**Last Updated**: March 2026
**Version**: 1.0.0
