# Location Service Microservice - Complete Implementation

## Project Overview

This is a comprehensive **Location, Room, and Resource Management Microservice** built using .NET 8 with DDD (Domain-Driven Design), CQRS (Command Query Responsibility Segregation), and event-driven architecture patterns.

## Architecture Layers

### 1. **Domain Layer** (`LocationService.Domain`)
Contains all business logic and entities:
- **Entities**: Base entity classes with domain event support
- **Aggregates**: 
  - `LocationAggregate` - Physical location master
  - `RoomAggregate` - Rooms within location
  - `RoomResourceAggregate` - Equipment/resources in rooms
- **Value Objects**: `Address`, `Contact`, `Status`
- **Domain Events**: Events raised by entities
- **Specifications**: Reusable query patterns
- **Repository Interfaces**: Contracts for data access

### 2. **Application Layer** (`LocationService.Application`)
Implements use cases and business workflows:
- **CQRS Pattern**:
  - `Commands`: Create, Update, Delete operations
  - `Queries`: Read operations
- **DTOs (Data Transfer Objects)**: API contracts
- **MediatR Handlers**: Business logic implementation
- **Behaviors**: Cross-cutting concerns (validation, logging)
- **Event Handlers**: Domain event processing
- **AutoMapper Profiles**: Entity-to-DTO mapping

### 3. **Infrastructure Layer** (`LocationService.Infrastructure`)
External services and data access:
- **EF Core DbContext**: Database modeling and migrations
- **Repositories**: Concrete implementations
- **Unit of Work**: Transaction management
- **RabbitMQ**: Message publishing and consuming
- **Dapper**: Optimized data access (read-heavy queries)
- **Azure Blob Storage**: File management
- **Redis Caching**: Distributed cache
- **Polly Resilience**: Circuit breaker & retry policies

### 4. **API Layer** (`LocationService.API`)
HTTP endpoints and GraphQL interface:
- **REST Controllers**: 
  - LocationsController
  - RoomsController
  - RoomResourcesController
- **GraphQL**: Query and mutation endpoints
- **JWT Authentication**: Secure endpoints
- **Middleware**: Exception handling, logging
- **Health Checks**: System health monitoring
- **Swagger/OpenAPI**: API documentation

### 5. **Azure Functions** (`LocationService.AzureFunctions`)
Background tasks and event processing:
- **RabbitMQ Event Processor**: Process domain events
- **Scheduled Maintenance**: Periodic cleanup tasks
- **Notifications**: Send alerts and notifications

## Database Schema

### Tables
```
LOCATION_CONTACT (LocationAggregate)
├── LOCATION_ID (PK)
├── LOCATION_CODE (Unique)
├── LOCATION_NAME
├── LOCATION_ADDRESS, CITY, STATE, PIN_CODE, COUNTRY
├── PHONE, EMAIL, CONTACT_PERSON
├── LOCATION_STATUS (A/I)
└── Audit columns (CREATED_ON, CREATED_BY, etc.)

ROOM_MAST (RoomAggregate)
├── ROOM_ID (PK)
├── LOCATION_ID (FK)
├── ROOM_CODE (Unique with LocationId)
├── ROOM_NAME
├── ROOM_CAPACITY, ROOM_TYPE, FLOOR_NUMBER
├── ROOM_STATUS (A/I)
└── Audit columns

ROOM_RESOURCE (RoomResourceAggregate)
├── RESOURCE_ID (PK)
├── ROOM_ID (FK)
├── LOCATION_ID (FK)
├── RESOURCE_CODE
├── RESOURCE_NAME, RESOURCE_TYPE
├── RESOURCE_QUANTITY
├── RESOURCE_STATUS (A/I)
└── Audit columns
```

## API Endpoints

### Locations
- `GET /api/locations` - Get all locations
- `GET /api/locations/{id}` - Get location by ID
- `GET /api/locations/code/{code}` - Get location by code
- `GET /api/locations/active` - Get active locations
- `GET /api/locations/search?searchText=...` - Search locations
- `POST /api/locations` - Create location
- `PUT /api/locations/{id}` - Update location
- `DELETE /api/locations/{id}` - Delete location

### Rooms
- `GET /api/rooms/{id}` - Get room by ID
- `GET /api/rooms/location/{locationId}` - Get rooms by location
- `GET /api/rooms/type/{roomType}` - Get rooms by type
- `GET /api/rooms/capacity?locationId=&minCapacity=` - Get rooms by capacity
- `POST /api/rooms` - Create room
- `PUT /api/rooms/{id}` - Update room
- `DELETE /api/rooms/{id}` - Delete room

### Room Resources
- `GET /api/roomresources/{id}` - Get resource by ID
- `GET /api/roomresources/room/{roomId}` - Get resources by room
- `GET /api/roomresources/location/{locationId}` - Get resources by location
- `GET /api/roomresources/type/{resourceType}` - Get resources by type
- `POST /api/roomresources` - Create resource
- `PUT /api/roomresources/{id}` - Update resource
- `DELETE /api/roomresources/{id}` - Delete resource

### Health & Info
- `GET /health` - Health check
- `GET /swagger/index.html` - Swagger UI
- `GET /graphql` - GraphQL endpoint

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;..."
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "Issuer": "LocationServiceApi",
    "Audience": "LocationServiceUsers",
    "ExpiryMinutes": 60
  },
  "RabbitMq": {
    "Host": "localhost",
    "Port": 5672,
    "User": "guest",
    "Password": "guest"
  },
  "CacheSettings": {
    "UseRedis": false
  },
  "BlobStorage": {
    "ConnectionString": "..."
  }
}
```

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server / LocalDB
- RabbitMQ (optional for messaging)
- Redis (optional for caching)
- Azure Storage Account (optional for blob storage)

### 1. Database Setup
```bash
# Navigate to API project
cd LocationService.API

# Create database and run migrations
dotnet ef database update -s LocationService.API -p LocationService.Infrastructure

# Or manually run the SQL script
# Scripts\LocationModule_Schema.sql
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Build Solution
```bash
dotnet build
```

### 4. Run API
```bash
cd LocationService.API
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:7000
- Swagger UI: http://localhost:5000/swagger
- GraphQL: http://localhost:5000/graphql

## Authentication

### Generate JWT Token
Include in your request:
```bash
curl -X GET http://localhost:5000/api/locations \
  -H "Authorization: Bearer <your-jwt-token>"
```

### Default Credentials (Development)
```
UserId: 1
Email: admin@locationservice.com
Roles: Admin, LocationManager
```

## Features Implemented

### ✅ Core Architecture
- [x] Domain Layer with DDD principles
- [x] CQRS pattern with MediatR
- [x] Event Sourcing support
- [x] Repository pattern
- [x] Unit of Work pattern
- [x] Specification pattern

### ✅ API Features
- [x] REST API with async/await
- [x] GraphQL queries and mutations
- [x] Swagger/OpenAPI documentation
- [x] JWT Authentication & Authorization
- [x] CORS support
- [x] Error handling middleware

### ✅ Data Access
- [x] Entity Framework Core 8.0
- [x] SQL Server integration
- [x] EF Migrations support
- [x] Dapper for optimized queries
- [x] Connection pooling

### ✅ Messaging
- [x] RabbitMQ integration
- [x] Message publishing
- [x] Message consumers (async)
- [x] Domain event publishing

### ✅ Performance
- [x] Redis caching (optional)
- [x] Memory caching (development)
- [x] Distributed caching support
- [x] Query optimization

### ✅ Resilience
- [x] Polly Circuit Breaker
- [x] Retry policies with backoff
- [x] Timeout policies
- [x] Combined policies

### ✅ Cloud Integration
- [x] Azure Blob Storage support
- [x] Azure Functions scaffold
- [x] Health checks
- [x] Logging integration

### ✅ Maintenance
- [x] Seed data scripts
- [x] Database migrations
- [x] Entity audit columns
- [x] Structured logging

## Testing

### Unit Tests (Pending)
```bash
cd LocationService.UnitTests
dotnet test
```

### Integration Tests (Pending)
```bash
cd LocationService.IntegrationTests
dotnet test
```

## Deployment

### Local Development
```bash
dotnet run --project LocationService.API
```

### Docker
```dockerfile
# Build
docker build -t location-service:latest .

# Run
docker run -p 5000:5000 location-service:latest
```

### Azure App Service
```bash
# Publish
dotnet publish -c Release -o ./publish

# Deploy to Azure
az webapp up --resource-group myResourceGroup --name myLocationService
```

### Azure Container Instances
```bash
# Deploy
az container create \
  --resource-group myResourceGroup \
  --name location-service \
  --image myacr.azurecr.io/location-service:latest \
  --ports 5000 \
  --environment-variables \
    "ConnectionStrings__DefaultConnection=..." \
    "JwtSettings__SecretKey=..."
```

## Monitoring

### Health Checks
```bash
curl http://localhost:5000/health
```

### Application Insights (Optional)
Configure in appsettings.json:
```json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-key"
  }
}
```

## Troubleshooting

### Database Connection Issues
1. Verify LocalDB is running: `sqllocaldb info v13.0`
2. Check connection string in appsettings.json
3. Run migrations again: `dotnet ef database update`

### RabbitMQ Issues
1. Verify RabbitMQ is running on localhost:5672
2. Update credentials in appsettings.json
3. Check message queue status in RabbitMQ admin panel

### JWT Token Issues
1. Verify secret key is at least 32 characters
2. Check token expiry time
3. Ensure Authorization header format: `Bearer <token>`

## Future Enhancements

- [ ] Add comprehensive unit tests
- [ ] Add integration tests
- [ ] Implement advanced caching strategies
- [ ] Add real-time notifications via SignalR
- [ ] Implement API versioning
- [ ] Add request/response logging middleware
- [ ] Implement rate limiting
- [ ] Add OpenTelemetry tracing
- [ ] Multi-tenant support
- [ ] Comprehensive audit logging

## Project Structure

```
LocationService/
├── LocationService.Domain/
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Aggregates/
│   ├── DomainEvents/
│   ├── Specifications/
│   └── Exceptions/
├── LocationService.Application/
│   ├── Commands/
│   ├── Queries/
│   ├── Handlers/
│   ├── DTOs/
│   ├── Behaviors/
│   ├── EventHandlers/
│   └── Mappings/
├── LocationService.Infrastructure/
│   ├── Persistence/
│   ├── Repositories/
│   ├── Migrations/
│   ├── ExternalServices/
│   ├── Messaging/
│   └── Caching/
├── LocationService.API/
│   ├── Controllers/
│   ├── Middleware/
│   ├── Security/
│   ├── GraphQL/
│   ├── Program.cs
│   └── Properties/
└── LocationService.AzureFunctions/
    └── Functions/
```

## Contributing

1. Follow DDD principles
2. Add domain events for significant changes
3. Use CQRS separation
4. Add appropriate logging
5. Include XML comments for public APIs

## License

Internal - SPARSH ERP System

## Contact

For issues or questions, contact the Architecture team.

---

**Created**: March 15, 2026
**Version**: 1.0
**Framework**: .NET 8.0
