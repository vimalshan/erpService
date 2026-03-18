# Feedback Microservice

A comprehensive microservice for managing feedback in the ERP system, built with .NET 8, following Domain-Driven Design (DDD) and CQRS patterns.

## Architecture

The solution is organized in a layered architecture:

- **Domain Layer** (`FeedbackService.Domain`) - Business logic, entities, aggregates, and domain events
- **Application Layer** (`FeedbackService.Application`) - CQRS commands/queries, DTOs, validators, and mappings
- **Infrastructure Layer** (`FeedbackService.Infrastructure`) - Data access, Entity Framework Core, repositories, messaging
- **API Layer** (`FeedbackService.API`) - REST API, GraphQL, Swagger, middleware, authentication

## Technology Stack

- **Runtime**: .NET 8.0
- **Database**: SQL Server (LocalDB)
- **ORM**: Entity Framework Core 8.0.3
- **CQRS**: MediatR 12.2.0
- **Messaging**: RabbitMQ
- **APIs**: REST, GraphQL (HotChocolate), Minimal APIs
- **Authentication**: JWT Bearer
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Logging**: Serilog
- **Resilience**: Polly (Circuit Breaker)
- **Cloud**: Azure Blob Storage
- **API Documentation**: Swagger/OpenAPI

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server or LocalDB
- RabbitMQ (optional, for messaging features)
- Visual Studio 2022 or VS Code

### Setup

1. **Clone the repository and navigate to the source directory**

```bash
cd src
```

2. **Restore NuGet packages**

```bash
dotnet restore
```

3. **Update connection strings** in `FeedbackService.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DDDB;Integrated Security=True;..."
  }
}
```

4. **Apply EF Core migrations**

```bash
cd FeedbackService.API
dotnet ef database update --startup-project FeedbackService.API --project ../FeedbackService.Infrastructure
```

5. **Run the application**

```bash
dotnet run
```

The API will be available at `https://localhost:5001` (HTTPS) or `http://localhost:5000` (HTTP).

## API Endpoints

### REST API

#### Authentication
- `POST /api/auth/login` - Generate JWT token
  
#### Feedback Management
- `POST /api/feedback` - Create new feedback
- `GET /api/feedback` - Get all feedback (with pagination)
- `GET /api/feedback/{id}` - Get feedback by ID
- `GET /api/feedback/by-request/{requestNo}` - Get feedback by request number
- `POST /api/feedback/items` - Add item to feedback
- `POST /api/feedback/{feedbackId}/submit` - Submit feedback

### GraphQL

GraphQL endpoint: `https://localhost:5001/graphql`

**Queries:**
```graphql
{
  feedbackById(id: 1) {
    id
    requestNo
    approverSystemId
    status
    items { questionNo answerNo }
  }
  
  feedbacks(pageNumber: 1, pageSize: 10) {
    id
    requestNo
    status
  }
  
  feedbacksByRequestNo(requestNo: 100) {
    id
    remarks
  }
}
```

**Mutations:**
```graphql
mutation {
  createFeedback(
    feedbackId: 1
    requestNo: 100
    approverSystemId: 5
    remarks: "Test feedback"
  ) {
    id
    status
  }
  
  addFeedbackItem(feedbackId: 1, questionNo: 1, answerNo: 101) {
    id
    items { questionNo answerNo }
  }
  
  submitFeedback(feedbackId: 1) {
    id
    status
  }
}
```

### Swagger Documentation

Access Swagger UI at: `https://localhost:5001/swagger/index.html`

## Authentication

The API uses JWT (JSON Web Tokens) for authentication.

### Login

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "password"}'
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600
}
```

### Using Token

Include the token in the Authorization header:

```bash
curl https://localhost:5001/api/feedback \
  -H "Authorization: Bearer <token>"
```

## Features

### Domain-Driven Design (DDD)
- Aggregate roots and entities
- Value objects (FeedbackStatus)
- Domain events (FeedbackCreatedEvent, FeedbackSubmittedEvent)
- Repository pattern

### CQRS Pattern
- Separate read (queries) and write (commands) operations
- MediatR for request/response handling
- Validation pipeline

### Resilience
- Polly circuit breaker for external service calls
- Retry policies
- Health checks for database and RabbitMQ

### Data Persistence
- Entity Framework Core with SQL Server
- Code-first migrations
- Unit of Work pattern
- Dapper support for complex queries

### Messaging
- RabbitMQ integration for async events
- Domain event publishing
- Background event consumer service

### Cloud Integration
- Azure Blob Storage for document management
- Azure Functions support for background tasks

### Logging & Monitoring
- Serilog for structured logging
- Health check endpoints
- Request correlation

### API Features
- REST API with full CRUD operations
- GraphQL interface with Banana Cake Pop support
- Minimal APIs
- OpenAPI/Swagger documentation
- CORS support
- Exception handling middleware

## Configuration

### appsettings.json

Key configuration sections:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "...",
    "AzureBlobStorage": "..."
  },
  "JwtSettings": {
    "SecretKey": "...",
    "Issuer": "FeedbackService",
    "Audience": "FeedbackServiceAPI",
    "ExpirationMinutes": 60
  },
  "RabbitMQ": {
    "Hostname": "localhost",
    "Port": 5672,
    "Username": "guest",
    "Password": "guest"
  }
}
```

## Database Schema

### APP_FEEDBACKMAIN
- FB_FEEDBACKID (decimal) - Primary key
- FB_REQUESTNO (decimal) - Request reference
- FB_APPRSYSID (decimal) - Approver system ID
- FB_STATUS (char) - Status (A=Active, I=Inactive)
- FB_REMARKS (varchar) - Comments
- CREATEDON (datetime2) - Creation timestamp
- UPDATEDON (datetime2) - Last update timestamp

### APP_FEEDBACKSUB
- FB_FEEDBACKID (decimal) - Foreign key
- FB_QTNNO (decimal) - Question number
- FB_ANSNO (decimal) - Answer number
- UPDATEDON (datetime2) - Last update timestamp

### LOV_FEEDBACK
- DD_FEEDBACKID (decimal) - Feedback type ID
- DD_FEEDBACKNAME (nvarchar) - Feedback type name

## Health Checks

- `/health` - General health status
- `/health/live` - Liveness probe
- `/health/ready` - Readiness probe

## Building & Testing

### Build

```bash
dotnet build
```

### Run Tests

```bash
dotnet test
```

### Publish

```bash
dotnet publish -c Release -o ./publish
```

## RabbitMQ Setup

For local development with RabbitMQ:

```bash
# Using Docker
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

# Default credentials: guest/guest
# Management UI: http://localhost:15672
```

## Azure Functions (Background Tasks)

The infrastructure supports Azure Functions for background task processing:

```csharp
public async Task ProcessFeedbackAsync([ServiceBusTrigger("feedback-submissions")] FeedbackSubmittedEvent @event)
{
    // Handle async feedback processing
}
```

## Troubleshooting

### Connection String Issues
- Ensure LocalDB is installed and running
- Update the connection string with your SQL Server instance
- Use SQL Server Object Explorer in Visual Studio to verify

### RabbitMQ Connection Errors
- Verify RabbitMQ is running
- Check connection string in appsettings.json
- Ensure guest user credentials are correct

### JWT Token Errors
- Verify JWT secret key is configured
- Check token expiration time
- Ensure Authorization header format is correct: `Bearer <token>`

### EF Migration Issues
- Delete pending migrations if needed
- Clear the database and re-migrate
- Check database connection string

## Contributing

Follow the established patterns:
- Use CQRS for business logic
- Create domain events for state changes
- Add validators for all commands
- Include XML documentation comments
- Use dependency injection for all dependencies

## License

This project is part of the ERP Microservice architecture.

## Support

For issues or questions, please contact the development team.
