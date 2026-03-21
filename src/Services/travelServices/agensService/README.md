# Agency Service Microservice

A comprehensive Travel Agency Management Microservice built with .NET 8, featuring DDD, CQRS, EF Core, GraphQL, and advanced patterns.

## Architecture Overview

```
┌─────────────────────────────────────────────────────────┐
│                    API Layer                            │
│  (REST, GraphQL, Minimal APIs, Middleware, Auth)        │
└─────────────────────────────────────────────────────────┘
                            │
┌─────────────────────────────────────────────────────────┐
│              Application Layer (CQRS)                   │
│        (Commands, Queries, DTOs, Behaviors)             │
└─────────────────────────────────────────────────────────┘
                            │
┌─────────────────────────────────────────────────────────┐
│                  Domain Layer (DDD)                      │
│    (Entities, Aggregates, Value Objects, Events)        │
└─────────────────────────────────────────────────────────┘
                            │
┌─────────────────────────────────────────────────────────┐
│              Infrastructure Layer                        │
│  (EF Core, Repositories, RabbitMQ, Azure, Polly)        │
└─────────────────────────────────────────────────────────┘
```

## Features

### Core Components
- **Domain-Driven Design (DDD)** - Entities, Aggregates, Value Objects
- **CQRS Pattern** - Separated Commands and Queries
- **Event Sourcing** - Domain Events for state changes
- **Repository Pattern** - Data access abstraction

### API Endpoints
- **REST API** - Full CRUD operations with proper HTTP status codes
- **GraphQL** - Query and mutation support via Banana Cake Pop
- **Minimal APIs** - Lightweight endpoint definitions

### Data Access
- **Entity Framework Core** - ORM with migrations
- **Dapper** - Lightweight ORM for complex queries
- **SQL Server (LocalDB)** - Default database

### Advanced Features
- **JWT Authentication** - Secure API access
- **Authorization** - Role-based access control
- **RabbitMQ** - Event-driven architecture
- **Polly Circuit Breaker** - Resilience patterns
- **Azure Blob Storage** - File management
- **Health Checks** - API and database monitoring

### Cloud Integration
- **Azure Functions** - Serverless processing
- **Azure Blob Storage** - File storage
- **Azure Service Bus** - Message queue integration

## Prerequisites

- .NET 8 SDK
- SQL Server 2019+ or SQL Server Express (with LocalDB)
- RabbitMQ (optional, defaults to localhost:5672)
- Visual Studio 2022 or VS Code
- Azure CLI (optional, for Azure deployments)

## Installation

### 1. Clone the Repository
```bash
cd agensService
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Configure Database Connection
Edit `src/API/AgencyService.Api/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;..."
}
```

### 4. Run Database Migrations
```bash
# Navigate to the API project
cd src/API/AgencyService.Api

# Create the database
dotnet ef database update --project ..\\..\\Infrastructure\\AgencyService.Infrastructure
```

### 5. Build the Solution
```bash
dotnet build
```

## Running the Application

### Development Mode
```bash
cd src/API/AgencyService.Api
dotnet run
```

The API will be available at:
- **Swagger UI**: http://localhost:5000/swagger/index.html
- **GraphQL Endpoint**: http://localhost:5000/graphql
- **Health Check**: http://localhost:5000/health

### Using Docker Compose
```bash
docker-compose up -d
```

## API Documentation

### REST Endpoints

#### Agencies
- `GET /api/agencies` - Get all agencies
- `GET /api/agencies/{agencyCode}` - Get agency by code
- `GET /api/agencies/type/{type}` - Get agencies by type (Air, Train, Bus, Cab)
- `POST /api/agencies` - Create new agency
- `PUT /api/agencies/{agencyCode}` - Update agency
- `DELETE /api/agencies/{agencyCode}` - Delete agency

#### Vendors
- `GET /api/vendors` - Get all vendors
- `GET /api/vendors/{vendorId}` - Get vendor by ID
- `GET /api/vendors/category/{categoryType}` - Get vendors by category
- `POST /api/vendors` - Create new vendor
- `PUT /api/vendors/{vendorId}` - Update vendor
- `DELETE /api/vendors/{vendorId}` - Delete vendor

#### Airlines
- `GET /api/airlines` - Get all airlines
- `GET /api/airlines/{code}` - Get airline by code
- `POST /api/airlines` - Create new airline

### Sample Requests

#### Create Agency
```bash
curl -X POST http://localhost:5000/api/agencies \
  -H "Content-Type: application/json" \
  -d '{
    "agencyCode": 1,
    "name": "Global Travel Solutions",
    "type": "Air",
    "email": "info@globaltravel.com",
    "phone": "+1-800-123-4567",
    "address1": "123 Travel Street, New York, NY 10001"
  }'
```

#### Get All Agencies
```bash
curl http://localhost:5000/api/agencies
```

### GraphQL Query Example

```graphql
query {
  getAllAgencies {
    agencyCode
    name
    type
    email
    phone
    address1
  }
}
```

### GraphQL Mutation Example

```graphql
mutation {
  createAgency(
    agencyCode: 1
    name: "Global Travel"
    type: "Air"
    email: "info@globaltravel.com"
    phone: "+1-800-123-4567"
    address1: "123 Travel St"
  ) {
    agencyCode
    name
  }
}
```

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;..."
  },
  "JwtSettings": {
    "Secret": "your-secret-key-minimum-32-chars",
    "Issuer": "AgencyService",
    "Audience": "AgencyServiceClient",
    "ExpirationMinutes": 60
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest",
    "Port": 5672
  },
  "Azure": {
    "BlobStorageConnectionString": "..."
  }
}
```

## Database Schema

### AGENCY_MASTER
- AM_AGN_CODE (BIGINT, PK)
- AM_AGN_NAM (VARCHAR)
- AM_AGN_TYP (VARCHAR)
- AM_EML_ID (VARCHAR)
- AM_PHN_NO (VARCHAR)
- Contact Information & Address

### VENDOR_MASTER
- VM_ID (BIGINT, PK)
- VM_NAME (VARCHAR)
- VM_CAT_TYPE (CHAR) - V (Vendor), H (Hotel)
- VM_PHN_NO (VARCHAR)
- Bank Account Information

### AIRLINE_MAST
- AIR_LIN_COD (CHAR, PK)
- AIR_LIN_NAM (VARCHAR)

## Authentication

The API uses JWT Bearer tokens. To access protected endpoints:

1. Obtain a token (implementation in authentication controller)
2. Include in all requests: `Authorization: Bearer <token>`

## Running Tests

```bash
# Unit Tests
dotnet test

# Run with code coverage
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

## Project Structure

```
src/
├── Domain/
│   └── AgencyService.Domain/
│       ├── Common/
│       ├── Entities/
│       ├── ValueObjects/
│       └── Repositories/
├── Application/
│   └── AgencyService.Application/
│       ├── Commands/
│       ├── Queries/
│       ├── DTOs/
│       └── Common/
├── Infrastructure/
│   └── AgencyService.Infrastructure/
│       ├── Data/
│       ├── Repositories/
│       ├── Messaging/
│       ├── BlobStorage/
│       ├── Resilience/
│       └── AzureFunctions/
└── API/
    └── AgencyService.Api/
        ├── Controllers/
        ├── Endpoints/
        ├── GraphQL/
        ├── Middleware/
        ├── Authentication/
        ├── HealthChecks/
        ├── Program.cs
        └── appsettings.json
```

## NuGet Packages

Key packages used:
- `MediatR` - CQRS implementation
- `EntityFrameworkCore` - ORM
- `Dapper` - Micro ORM
- `RabbitMQ.Client` - Message queue
- `Polly` - Resilience policies
- `Azure.Storage.Blobs` - Blob storage
- `GraphQL` - GraphQL server
- `AspNetCore.Authentication.JwtBearer` - JWT auth

## Environment Variables

```
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://localhost:5000
ConnectionString=Data Source=(localdb)\MSSQLLocalDB;...
JwtSecret=your-secret-key
RabbitMQHost=localhost
```

## Deployment

### Docker
```bash
docker build -t agency-service:latest .
docker run -p 5000:80 agency-service:latest
```

### Azure App Service
```bash
az appservice plan create --name agency-plan --resource-group mygroup --sku B1
az webapp create --resource-group mygroup --plan agency-plan --name agency-service
```

### Kubernetes
```bash
kubectl apply -f k8s-deployment.yaml
```

## Performance Considerations

1. **Connection Pooling** - Enabled by default in EF Core
2. **Caching** - Implement distributed caching (Redis)
3. **Database Indexing** - Indexed on frequently queried columns
4. **Async/Await** - All I/O operations are async
5. **Circuit Breaker** - Polly policies prevent cascading failures

## Monitoring & Logging

- **Logging**: Integrated with ILogger
- **Health Checks**: `/health` endpoint
- **Telemetry**: Application Insights ready
- **Metrics**: Request/Response logging middleware

## Troubleshooting

### Database Connection Issues
```bash
# Verify LocalDB is running
sqllocaldb info
sqllocaldb start mssqllocaldb

# Check connection string in appsettings.json
```

### RabbitMQ Connection Issues
```bash
# Verify RabbitMQ is accessible
telnet localhost 5672
```

### JWT Token Issues
- Ensure secret key is at least 32 characters
- Verify token hasn't expired
- Check issuer and audience configuration

## Contributing

1. Fork the repository
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open Pull Request

## License

This project is licensed under the MIT License.

## Support

For issues and questions:
- GitHub Issues
- Email: support@agencyservice.com
- Documentation: https://docs.agencyservice.com

## Changelog

### Version 1.0.0
- Initial release with DDD, CQRS, REST, GraphQL
- RabbitMQ integration
- JWT Authentication
- Azure integration
- Health checks and resilience patterns
