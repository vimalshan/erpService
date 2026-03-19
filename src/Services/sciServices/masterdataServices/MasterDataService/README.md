# Master Data Service Microservice

## Overview

The Master Data Service is a comprehensive .NET 8.0 microservice built with clean architecture principles, implementing CQRS pattern, domain-driven design, and event-sourcing patterns. It provides RESTful APIs, GraphQL interfaces, and various enterprise features for managing master data entities.

## 🏗️ Architecture

### Layered Architecture
```
┌─────────────────────────────────┐
│   API Layer (REST, GraphQL)     │
├─────────────────────────────────┤
│   Application Layer (CQRS)      │
├─────────────────────────────────┤
│   Domain Layer (Entities)       │
├─────────────────────────────────┤
│   Infrastructure Layer (EF, DB) │
└─────────────────────────────────┘
```

### Project Structure
- **MasterData.Domain**: Core business entities, aggregates, value objects, domain events
- **MasterData.Application**: CQRS commands/queries, DTOs, validators, handlers, mappings
- **MasterData.Infrastructure**: EF Core, repositories, migrations, messaging services
- **MasterData.API**: REST controllers, GraphQL types, middleware, Swagger/OpenAPI
- **MasterData.Functions**: Azure Functions, background tasks, timers

## 🚀 Features

### 1. **REST API Endpoints**
- CRUD operations for all master data entities
- Comprehensive error handling
- Input validation with FluentValidation
- Swagger/OpenAPI documentation accessible at `/swagger/index.html`
- JWT Bearer token authentication & authorization

### 2. **GraphQL Support**
- Full GraphQL query and mutation support
- Type-safe operations
- Accessible at `/graphql` endpoint
- Compatible with Banana Cake Pop explorer

### 3. **Database & ORM**
- Entity Framework Core 8.0
- SQL Server (LocalDB) database
- Automatic migrations with seed data
- Transaction support with Unit of Work pattern

### 4. **Message Queue Integration**
- RabbitMQ consumer for event processing
- Topic-based message routing (CompanyUnit, Location, Supplier)
- Asynchronous event publishing
- Message durability and reliability

### 5. **Azure Integration**
- Azure Functions for background tasks
- Timer triggers for scheduled operations
- HTTP triggers for image uploads
- Blob Storage integration for stationery item images
- Queue triggers for message processing

### 6. **Resilience & Reliability**
- Polly Circuit Breaker policies
- Retry mechanisms with exponential backoff
- Timeout policies
- Request/response logging middleware
- Global exception handling middleware
- Health checks for database connectivity

### 7. **Security**
- JWT Bearer token authentication
- Role-based authorization (Admin role support)
- Secure password hashing
- CORS configuration

### 8. **Domain-Driven Design**
- Aggregate roots for bounded contexts
- Domain entities with encapsulation
- Value objects (Email, Code, Name, ContactInfo)
- Domain events for state changes
- Event-driven architecture

### 9. **CQRS Pattern**
- Separated read/write operations
- Command handlers for mutations
- Query handlers for data retrieval
- Validation and logging pipelines

## 📊 Domain Entities

### 1. **Company Unit**
- Code (3 chars)
- Name (up to 1000 chars)
- Timestamp tracking
- Soft delete support

### 2. **Location**
- Name (up to 50 chars)
- Timestamp tracking
- Soft delete support

### 3. **Supplier**
- Code (up to 25 chars)
- Name (up to 100 chars)
- Details (optional)
- Entry ID & Entry Number (for audit)
- Timestamp tracking

### 4. **State**
- Code (up to 100 chars)
- Name (up to 200 chars)
- Timestamp tracking

### 5. **City**
- Code (up to 100 chars)
- Name (up to 200 chars)
- State Code association
- Timestamp tracking

## 🔧 Configuration

### Connection String
```
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=MasterDataDB;
Integrated Security=True;
Encrypt=True;
TrustServerCertificate=False
```

### JWT Settings
```json
{
  "JwtSettings": {
    "Secret": "your-super-secret-jwt-key",
    "Issuer": "masterdataservice",
    "Audience": "masterdataapi",
    "ExpirationMinutes": 60
  }
}
```

### RabbitMQ Settings
```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/"
  }
}
```

## 📚 API Endpoints

### REST API

#### Company Units
- `GET /api/companyunits` - Get all company units
- `GET /api/companyunits/{id}` - Get company unit by ID
- `POST /api/companyunits` - Create new company unit (Admin only)
- `PUT /api/companyunits/{id}` - Update company unit (Admin only)
- `DELETE /api/companyunits/{id}` - Delete company unit (Admin only)

#### Suppliers
- `GET /api/suppliers` - Get all suppliers
- `GET /api/suppliers/{code}` - Get supplier by code
- `POST /api/suppliers` - Create new supplier (Admin only)
- `PUT /api/suppliers/{code}` - Update supplier (Admin only)
- `DELETE /api/suppliers/{code}` - Delete supplier (Admin only)

#### States & Cities
- `GET /api/states` - Get all states
- `GET /api/cities` - Get all cities
- `GET /api/cities/state/{stateCode}` - Get cities by state

### GraphQL Queries & Mutations

```graphql
# Queries
query {
  getCompanyUnits {
    id code name createdAt
  }
  suppliers {
    code name details entryId
  }
  citiesByState(stateCode: "NY") {
    code name
  }
}

# Mutations
mutation {
  createCompanyUnit(code: "BR3", name: "Branch 3")
  createSupplier(
    code: "SUP004"
    name: "New Supplier"
    entryId: "USER004"
    entryNumber: 400
  )
}
```

### Health Check
- `GET /health` - Database and API health status

## 🔐 Authentication

### Obtaining JWT Token
```bash
# Send credentials (implement token generation endpoint)
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "password"
}

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Using JWT Token
```bash
GET /api/companyunits
Authorization: Bearer {your_jwt_token}
```

## 🏗️ Building & Running

### Prerequisites
- .NET 8.0 SDK
- SQL Server or LocalDB
- RabbitMQ (optional, for messaging)
- Azure Function Core Tools (for Functions project)

### Build Solution
```bash
dotnet build MasterDataService.sln
```

### Run API
```bash
cd src/MasterData.API
dotnet run
```

The API will start at `https://localhost:7001` (or similar)

### Run Tests
```bash
dotnet test
```

### Deploy to Azure

#### Azure App Service
```bash
dotnet publish -c Release -o ./bin/publish
# Upload bin/publish to App Service
```

#### Azure Functions
```bash
func init --worker-runtime dotnet-isolated
cd src/MasterData.Functions
func start
```

## 📦 NuGet Dependencies

- **Entity Framework Core 8.0**: Database ORM
- **MediatR 12.1.1**: CQRS command/query pattern
- **AutoMapper 12.0.1**: Object mapping
- **FluentValidation 11.8.0**: Input validation
- **HotChocolate 13.5.0**: GraphQL support
- **Polly 8.2.0**: Resilience policies
- **RabbitMQ.Client 6.6.0**: Message queue integration
- **Serilog 8.0.0**: Structured logging
- **Swashbuckle.AspNetCore 6.4.6**: Swagger/OpenAPI
- **Azure.Storage.Blobs 12.19.0**: Blob storage

## 🔄 Data Flow

```
Client Request
    ↓
API Controller (REST/GraphQL)
    ↓
MediatR Handler
    ↓
Validation Pipeline
    ↓
Domain Aggregate
    ↓
Repository/UnitOfWork
    ↓
EF Core DbContext
    ↓
SQL Server
    ↓
Domain Events raised
    ↓
Event Handlers (Publish to RabbitMQ)
    ↓
Response back to Client
```

## 📝 Logging

All logs are written to:
- Console (development)
- File: `logs/masterdata-{date}.txt` (rolling daily)

## 🐛 Troubleshooting

### Connection Issues
- Verify LocalDB is running: `sqllocaldb info`
- Check connection string in appsettings.json

### RabbitMQ Issues
- Verify RabbitMQ service is running
- Check hostname and credentials

### GraphQL Issues
- Visit `/graphql` for Banana Cake Pop explorer
- Check query syntax and type definitions

## 📄 License

This project is part of the ERPMicroservice platform.

## 🤝 Support

For issues and questions, contact the development team.

---

**Last Updated**: March 18, 2026
**Version**: 1.0.0
