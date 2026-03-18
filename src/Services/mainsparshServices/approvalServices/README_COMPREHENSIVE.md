# Approval Service Microservice

A comprehensive .NET 8 microservice for managing approval workflows with support for multiple approval levels, employee assignments, and notifications.

## Features

### Core Features
- ✅ Approval Master Management (PER, DDP, LET modules)
- ✅ Approver Employee Assignment with effective date ranges
- ✅ Multi-level approval workflows
- ✅ Status tracking (Active/Inactive)
- ✅ Audit trails with CreatedBy/UpdatedBy

### Architecture Patterns
- ✅ **Domain-Driven Design (DDD)**: Clear separation of concerns with aggregates and value objects
- ✅ **CQRS (Command Query Responsibility Segregation)**: Separate read and write operations
- ✅ **Repository Pattern**: Data access abstraction
- ✅ **Unit of Work Pattern**: Transaction management
- ✅ **MediatR**: Request/response pipeline
- ✅ **Dependency Injection**: Loosely coupled components

### API Capabilities
- ✅ **REST API**: Full CRUD operations with OpenAPI/Swagger documentation
- ✅ **GraphQL**: Query and mutation support (Banana Cake Pop compatible)
- ✅ **Minimal APIs**: Lightweight endpoint configuration
- ✅ **JWT Authentication**: Secure token-based authentication
- ✅ **Role-Based Authorization**: Fine-grained access control

### Infrastructure
- ✅ **Entity Framework Core**: ORM with SQL Server support
- ✅ **Dapper**: High-performance queries for read operations
- ✅ **RabbitMQ**: Asynchronous messaging and event publishing
- ✅ **Azure Blob Storage**: Document and image storage
- ✅ **Azure Functions**: Background task processing
- ✅ **Polly Circuit Breaker**: Resilience and fault handling
- ✅ **Health Checks**: Database and dependency monitoring

### Cross-Cutting Concerns
- ✅ **Structured Logging**: Serilog with file and database outputs
- ✅ **Global Exception Handling**: Centralized error management
- ✅ **Validation**: FluentValidation with MediatR behaviors
- ✅ **CORS**: Cross-origin resource sharing
- ✅ **Domain Events**: Event-driven architecture support

## Project Structure

```
ApprovalService/
├── src/
│   ├── ApprovalService.Domain/           # Domain layer (Entities, Value Objects, Events)
│   │   ├── Entities/                     # ApprovalMaster, ApproverEmployee
│   │   ├── Events/                       # Domain events
│   │   ├── Interfaces/                   # Repository interfaces
│   │   ├── ValueObjects/                 # Value objects
│   │   └── Common/                       # Base classes
│   ├── ApprovalService.Application/      # Application layer (CQRS, DTOs)
│   │   ├── CQRS/
│   │   │   ├── Commands/                 # Create, Update, Activate, Deactivate
│   │   │   ├── Queries/                  # Get, List, Filter
│   │   │   └── Handlers/                 # Command and Query handlers
│   │   ├── DTOs/                         # Data transfer objects
│   │   ├── Behaviors/                    # MediatR behaviors (Validation, Logging)
│   │   └── Interfaces/                   # Application service interfaces
│   ├── ApprovalService.Infrastructure/   # Infrastructure layer (EF, Repositories, Services)
│   │   ├── Persistence/                  # DbContext, Migrations, DbSeed
│   │   ├── Repositories/                 # Implementation of repositories and UoW
│   │   ├── Messaging/                    # RabbitMQ publishers and consumers
│   │   └── External/                     # JWT, Blob Storage, External services
│   ├── ApprovalService.API/              # API layer (Controllers, Middleware)
│   │   ├── Controllers/                  # REST endpoints
│   │   ├── GraphQL/                      # GraphQL types and resolvers
│   │   ├── Middleware/                   # Global exception handling, correlation ID
│   │   ├── Program.cs                    # Application configuration
│   │   ├── appsettings.json              # Configuration
│   │   └── MappingProfile.cs             # AutoMapper profiles
│   └── ApprovalService.Functions/        # Azure Functions
│       ├── Functions.cs                  # Timer-triggered, Service Bus, Blob
│       └── local.settings.json           # Local function settings
└── ApprovalService.sln                   # Solution file
```

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server or SQL Server Express
- RabbitMQ 3.x+
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd ApprovalService
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Configure connection string**
   Edit `src/ApprovalService.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ApprovalServiceDb;Integrated Security=True;"
   }
   ```

4. **Create and seed database**
   ```bash
   cd src/ApprovalService.API
   dotnet ef database update
   ```

5. **Start RabbitMQ** (if not already running)
   ```bash
   docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
   ```

6. **Run the API**
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:5001`

## API Endpoints

### Approval Masters

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/approvals` | Get all approvals | ✅ |
| GET | `/api/approvals/{id}` | Get approval by ID | ✅ |
| GET | `/api/approvals/code/{code}` | Get approval by code | ✅ |
| GET | `/api/approvals/module/{module}` | Get approvals by module | ✅ |
| POST | `/api/approvals` | Create approval | ✅ |
| PUT | `/api/approvals/{id}` | Update approval | ✅ |
| PUT | `/api/approvals/{id}/activate` | Activate approval | ✅ |
| PUT | `/api/approvals/{id}/deactivate` | Deactivate approval | ✅ |

### Approver Employees

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/approvers/{id}` | Get approver by ID | ✅ |
| GET | `/api/approvers/approval/{approvalMasterId}` | Get approvers by approval | ✅ |
| GET | `/api/approvers/employee/{employeeId}` | Get approvers by employee | ✅ |
| POST | `/api/approvers` | Create approver assignment | ✅ |
| PUT | `/api/approvers/{id}` | Update approver | ✅ |
| PUT | `/api/approvers/{id}/activate` | Activate approver | ✅ |
| PUT | `/api/approvers/{id}/deactivate` | Deactivate approver | ✅ |

## Authentication

### Get JWT Token
Replace `{username}` and `{password}` with actual credentials:

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"{username}","password":"{password}"}'
```

Response:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 86400
}
```

### Use Token in Requests
```bash
curl -X GET https://localhost:5001/api/approvals \
  -H "Authorization: Bearer {accessToken}"
```

## Database Schema

### APPR_MAST (Approval Master)
```sql
CREATE TABLE APPR_MAST (
    APPR_ID BIGINT PRIMARY KEY IDENTITY(1,1),
    APPR_CODE VARCHAR(50) NOT NULL UNIQUE,
    APPR_NAME VARCHAR(255) NOT NULL,
    APPR_MODULE VARCHAR(100) NOT NULL,  -- PER, DDP, LET
    APPR_LEVEL INT NOT NULL DEFAULT 1,
    APPR_STATUS CHAR(1) DEFAULT 'A',   -- A=Active, I=Inactive
    CREATED_BY BIGINT NOT NULL,
    CREATED_ON DATETIME2(3) DEFAULT GETDATE(),
    UPDATED_BY BIGINT,
    UPDATED_ON DATETIME2(3)
);
```

### APPROVER_EMP (Approver Employee)
```sql
CREATE TABLE APPROVER_EMP (
    APPROVER_ID BIGINT PRIMARY KEY IDENTITY(1,1),
    APPR_ID BIGINT NOT NULL,
    EMP_SYSID BIGINT NOT NULL,
    APPROVER_LEVEL INT NOT NULL,
    APPROVER_STATUS CHAR(1) DEFAULT 'A',
    EFFECTIVE_FROM DATE NOT NULL,
    EFFECTIVE_TO DATE,
    CREATED_BY BIGINT NOT NULL,
    CREATED_ON DATETIME2(3) DEFAULT GETDATE(),
    UPDATED_BY BIGINT,
    UPDATED_ON DATETIME2(3),
    FOREIGN KEY (APPR_ID) REFERENCES APPR_MAST(APPR_ID)
);
```

## Configuration

### appsettings.json

```json
{
  "Jwt": {
    "SecretKey": "your-secret-key-here",
    "Issuer": "ApprovalService",
    "Audience": "ApprovalServiceUsers",
    "ExpirationHours": 24
  },
  "RabbitMq": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest"
  }
}
```

## Running Tests

```bash
# Run unit tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

## Building and Publishing

```bash
# Build solution
dotnet build

# Publish for deployment
dotnet publish -c Release -o ./publish

# Create Docker image
docker build -t approval-service:latest .
```

## Health Checks

Access health check endpoint:
```
GET https://localhost:5001/health
```

## Logging

Logs are output to:
- Console (Development)
- File: `logs/nlog-*.log`
- Database: `Log` table (if configured)

## RabbitMQ Integration

### Publishing Events
The service automatically publishes domain events to RabbitMQ:
- `approval.master.created`
- `approval.master.updated`
- `approval.master.statuschanged`
- `approver.employee.created`
- `approver.employee.updated`

### Consuming Events
Message consumers can subscribe to these topics:
```csharp
var connection = RabbitMqConnectionFactory.CreateConnection(configuration);
var consumer = new ApprovalMasterEventConsumer(connection, logger);
consumer.Start("approval-master-queue", "approval.master.*");
```

## Azure Integration

### Blob Storage
```csharp
var url = await blobStorageService.UploadAsync(
    "stationery-items", 
    "item-1.jpg", 
    imageStream);
```

### Azure Functions
Configured triggers:
- Timer: Background cleanup tasks (every 5 minutes)
- Service Bus: Event processing
- Blob Storage: Image processing on upload

## Troubleshooting

### Database Connection Issues
- Verify connection string in `appsettings.json`
- Check SQL Server is running: `sqlcmd -S (localdb)\MSSQLLocalDB`
- Run migrations: `dotnet ef database update`

### RabbitMQ Connection Issues
- Verify RabbitMQ is running on localhost:5672
- Check credentials (default: guest/guest)
- Access management console: `http://localhost:15672`

### JWT Token Issues
- Verify SecretKey is set and >= 32 characters
- Check token expiration hasn't occurred
- Validate token format: `Bearer {token}`

## Performance Optimization

- **Dapper**: Use for high-volume read queries
- **Circuit Breaker**: Automatic failover with Polly
- **Health Checks**: Monitor service health
- **Connection Pooling**: Configured in connection string
- **Async/Await**: Non-blocking I/O operations

## Security Considerations

- ✅ JWT tokens for stateless authentication
- ✅ HTTPS only in production
- ✅ Input validation with FluentValidation
- ✅ SQL injection prevention via EF Core/Dapper parameterized queries
- ✅ CORS policy configured
- ✅ Exception details hidden in Production

## Contributing

1. Create feature branch: `git checkout -b feature/your-feature`
2. Commit changes: `git commit -am 'Add your feature'`
3. Push to branch: `git push origin feature/your-feature`
4. Submit pull request

## License

This project is licensed under the MIT License.

## Support

For issues, questions, or suggestions, please create an issue in the repository.
