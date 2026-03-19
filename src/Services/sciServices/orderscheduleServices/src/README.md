# Order Scheduling Microservice

A comprehensive microservice for managing order scheduling and fulfillment built with Clean Architecture, CQRS, and Domain-Driven Design.

## Architecture Overview

```
OrderScheduleService/
├── OrderScheduleService.Domain/           # Domain Layer (Entities, Aggregates, Events)
├── OrderScheduleService.Application/      # Application Layer (CQRS, DTOs, Handlers)
├── OrderScheduleService.Infrastructure/   # Infrastructure Layer (EF Core, Repositories)
├── OrderScheduleService.API/             # API Layer (REST, GraphQL, Minimal APIs)
└── OrderScheduleService.IntegrationEvents # Integration Events (RabbitMQ)
```

## Features

✅ **Domain-Driven Design**
- Domain Entities: OrderDetail, ScheduleDetail, OrderActual, Shift
- Aggregate Roots: TiedOrderAggregate, ScheduleAggregate
- Value Objects: OrderQuantity, OrderNumber, TimeRange, OrganizationId
- Domain Events: OrderCreatedEvent, OrderScheduledEvent, OrderCancelledEvent, etc.

✅ **CQRS Pattern**
- Segregated Commands and Queries
- Command Handlers for all write operations
- Query Handlers for all read operations
- MediatR for command/query dispatching

✅ **REST API**
- Full CRUD operations on Orders, Schedules, and Shifts
- Comprehensive error handling
- Request/Response logging middleware
- Swagger/OpenAPI documentation at `/swagger/index.html`

✅ **GraphQL**
- GraphQL Query endpoint at `/graphql`
- Banana Cake Pop support
- Type-safe queries and mutations

✅ **Minimal APIs**
- Alternative lightweight endpoints at `/api/minimal/*`
- OpenAPI documentation included

✅ **Authentication & Authorization**
- JWT token-based authentication
- Role-based authorization (Admin roles)
- Token service for generation and validation

✅ **Database**
- Entity Framework Core 8.0
- SQL Server with LocalDB support
- Automatic migrations on startup
- Seed data included

✅ **Resilience**
- Polly Circuit Breaker policies
- Retry mechanisms
- Health checks at `/health`

✅ **Message Queue**
- RabbitMQ integration (optional)
- Integration event publishing and consuming
- Configurable in appsettings.json

✅ **Cloud Storage**
- Azure Blob Storage support
- File upload/download for stationery images
- Container management

## Database Setup

### Connection String
```
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=OrderScheduleDB;
Integrated Security=True;
Persist Security Info=False;
Pooling=False;
MultipleActiveResultSets=False;
Encrypt=True;
TrustServerCertificate=False;
Application Name=OrderScheduleService;
Command Timeout=0
```

### Schema Tables
- `OS_TIED_ORDER_HEADER` - Order headers
- `OS_TIED_ORDER_DETAILS` - Order line items
- `OS_SCHEDULE_MASTER` - Schedule master records
- `OS_SCHEDULE_DETAILS` - Schedule details
- `OS_ACTUAL_ORDER` - Actual order tracking
- `OS_SHIFT_MASTER` - Shift configuration

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server 2019+ or LocalDB
- Visual Studio 2022 or VS Code
- RabbitMQ (optional)
- Azure Storage Account (optional)

### Installation

1. **Clone and navigate to the project**
```bash
cd OrderScheduleService
```

2. **Restore packages**
```bash
dotnet restore
```

3. **Build the solution**
```bash
dotnet build
```

4. **Run the API**
```bash
cd OrderScheduleService.API
dotnet run
```

The API will start at `https://localhost:5001`

## API Documentation

### Available Endpoints

#### REST API

**Orders Management**
- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `GET /api/orders/customer/{customerCode}` - Get orders by customer
- `POST /api/orders` - Create new order
- `PUT /api/orders/{id}/status` - Update order status
- `DELETE /api/orders/{id}` - Delete order

**Order Details**
- `GET /api/orderdetails/order/{orderId}` - Get order details
- `POST /api/orderdetails/order/{orderId}` - Add detail to order
- `PUT /api/orderdetails/order/{orderId}/detail/{detailId}/schedule` - Schedule detail
- `PUT /api/orderdetails/order/{orderId}/detail/{detailId}/cancel` - Cancel detail

**Schedules**
- `GET /api/schedules/{id}` - Get schedule by ID
- `GET /api/schedules/item/{itemId}` - Get schedules by item
- `GET /api/schedules/date-range` - Get schedules by date range
- `GET /api/schedules/{id}/available-capacity` - Get available capacity
- `POST /api/schedules` - Create schedule
- `PUT /api/schedules/{id}/confirm` - Confirm schedule
- `DELETE /api/schedules/{id}` - Delete schedule

**Shifts** (Admin only)
- `GET /api/shifts` - Get all shifts
- `GET /api/shifts/{shiftCode}/company/{companyUnitId}` - Get shift
- `GET /api/shifts/company/{companyUnitId}` - Get shifts by company
- `POST /api/shifts` - Create shift
- `PUT /api/shifts/{shiftCode}/company/{companyUnitId}` - Update shift
- `DELETE /api/shifts/{shiftCode}/company/{companyUnitId}` - Delete shift

#### GraphQL Endpoint
- `POST /graphql` - GraphQL queries and mutations

#### Minimal APIs
- `GET /api/minimal/orders` - Get all orders
- `GET /api/minimal/orders/{id}` - Get order by ID
- `GET /api/minimal/schedules/{id}` - Get schedule
- Comprehensive swagger docs available

#### Health Checks
- `GET /health` - Health check endpoint with detailed status

### Authentication

1. **Generate JWT Token**
```bash
POST /api/auth/token
{
  "userId": "user123",
  "userName": "John Doe",
  "roles": ["User", "Admin"]
}
```

2. **Use Token in Requests**
```bash
Authorization: Bearer <your_jwt_token>
```

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OrderScheduleDB;..."
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key",
    "Issuer": "OrderScheduleService",
    "Audience": "OrderScheduleServiceAPI",
    "ExpirationMinutes": 60
  },
  "RabbitMq": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest"
  },
  "AzureBlobStorage": {
    "ConnectionString": "DefaultEndpointsProtocol=https;...",
    "ContainerName": "stationery-images"
  }
}
```

## Usage Examples

### Create an Order
```bash
POST /api/orders
Content-Type: application/json

{
  "customerCode": "CUST001",
  "companyUnitId": 1,
  "modifiedUserId": "ADMIN",
  "details": [
    {
      "itemId": 1001,
      "itemName": "Bottled Water 500ML",
      "orderQuantity": 1000,
      "dispatchDate": "2026-03-20T00:00:00",
      "price": 25.00
    }
  ]
}
```

### Schedule an Order Detail
```bash
PUT /api/orderdetails/order/1/detail/1/schedule?scheduledDate=2026-03-20&allocatedQuantity=500
```

### GraphQL Query
```graphql
query {
  orders {
    id
    customerCode
    orderedDate
    details {
      id
      itemName
      orderQuantity
    }
  }
}
```

## Domain Events

The service implements domain events for state changes:

- **OrderCreatedEvent** - When a new order is created
- **OrderDetailAddedEvent** - When details are added to an order
- **OrderScheduledEvent** - When an order is scheduled
- **OrderCancelledEvent** - When an order is cancelled
- **OrderFulfilledEvent** - When an order is fulfilled
- **ScheduleConfirmedEvent** - When a schedule is confirmed
- **CapacityChangedEvent** - When capacity changes

## RabbitMQ Integration

### Configuration
Update `appsettings.json` with your RabbitMQ details:
```json
"RabbitMq": {
  "HostName": "localhost",
  "Port": 5672,
  "UserName": "guest",
  "Password": "guest",
  "QueueName": "order.schedule.events"
}
```

### Publishing Events
Events are automatically published to RabbitMQ when domain events occur.

### Consuming Events
Start the consumer to listen for messages:
```csharp
var consumer = app.Services.GetRequiredService<IRabbitMqConsumer>();
consumer.StartConsuming();
```

## Health Checks

Access health endpoint:
```
GET /health
```

Response:
```json
{
  "status": "Healthy",
  "checks": {
    "Database": "Healthy",
    "API": "Healthy"
  }
}
```

## Error Handling

The service implements global error handling middleware:

- **400 Bad Request** - Invalid input or business logic violations
- **401 Unauthorized** - Missing or invalid authentication
- **403 Forbidden** - Insufficient permissions
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - Unexpected errors

## Logging

Configure logging levels in `appsettings.json`:
```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```

## Testing

### Unit Tests
```bash
dotnet test
```

### Integration Tests
Tests use real database contexts and repositories.

## Deployment

### Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "OrderScheduleService.API.dll"]
```

### Azure
1. Create App Service
2. Connect database
3. Configure environment variables
4. Deploy using Azure DevOps or GitHub Actions

## Performance Optimization

- Entity Framework Core query optimization with includes
- Polly retry policies for resilience
- Circuit breaker for failed dependencies
- Health checks for monitoring
- Async/await patterns throughout

## Security

- JWT token validation
- Role-based access control
- Input validation and sanitization
- HTTPS enforcement in production
- Secure connection strings

## Contributing

1. Follow Clean Architecture principles
2. Use CQRS pattern for new features
3. Add domain events for state changes
4. Write comprehensive tests
5. Update documentation

## License

This project is licensed under the MIT License - see LICENSE file for details.

## Support

For issues and questions, please contact the development team or submit an issue in the repository.

## Changelog

### Version 1.0.0 (March 18, 2026)
- Initial release
- Full CRUD operations
- JWT authentication
- GraphQL support
- RabbitMQ integration
- Azure Blob Storage
- Health checks
- Swagger documentation

## Roadmap

- [ ] Message queue pattern refinement
- [ ] Temporal queries for audit trails
- [ ] Advanced scheduling algorithms
- [ ] Mobile app API
- [ ] Real-time notifications
- [ ] Analytics dashboard
- [ ] Performance caching layer
- [ ] Microservice to microservice communication
