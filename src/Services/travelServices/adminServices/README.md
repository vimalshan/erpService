# Admin Service Microservice

A comprehensive .NET 8 microservice for managing travel, accommodation, and meeting-related administrative operations using modern architecture patterns.

## Architecture

This solution follows **Clean Architecture** principles with the following layers:

### Project Structure

```
AdminService.sln
├── AdminService.Shared        # Shared DTOs and enums
├── AdminService.Domain        # Domain models, entities, and business logic
├── AdminService.Application   # Application services, CQRS, DTOs
├── AdminService.Infrastructure # Data access, external services
└── AdminService.API           # REST API, GraphQL, Middleware
```

## Technologies & Frameworks

- **Framework**: .NET 8
- **Database**: SQL Server (LocalDB)
- **ORM**: Entity Framework Core 8
- **CQRS**: MediatR
- **API**: ASP.NET Core REST & HotChocolate GraphQL
- **Authentication**: JWT Bearer
- **Messaging**: RabbitMQ
- **Cloud Storage**: Azure Blob Storage
- **Resilience**: Polly (Circuit Breaker)
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **API Documentation**: Swagger/OpenAPI

## Getting Started

### Prerequisites

- .Net 8 SDK or later
- SQL Server or LocalDB
- RabbitMQ (optional for messaging features)
- Azure Storage Account (optional for blob storage)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd AdminService
   ```

2. **Update Connection Strings**
   Edit `AdminService.API/appsettings.json`:
   
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AdminServiceDb;Integrated Security=True;",
       "AzureBlobStorage": "DefaultEndpointsProtocol=https;AccountName=yourAccount;AccountKey=yourKey;"
     },
     "JwtSettings": {
       "SecretKey": "your-secret-key-min-32-chars",
       "ExpirationMinutes": 60
     },
     "RabbitMQ": {
       "HostName": "localhost",
       "UserName": "guest",
       "Password": "guest",
       "Port": 5672
     }
   }
   ```

3. **Restore NuGet Packages**
   ```bash
   dotnet restore
   ```

4. **Apply Database Migrations**
   ```bash
   dotnet ef database update --project AdminService.Infrastructure --startup-project AdminService.API
   ```

5. **Run the Application**
   ```bash
   cd AdminService.API
   dotnet run
   ```

## API Endpoints

### REST API

#### Admin Units
- `GET /api/adminunits` - Get all admin units
- `GET /api/adminunits/{id}` - Get admin unit by ID
- `GET /api/adminunits/type/{adminType}` - Get by type
- `POST /api/adminunits` - Create admin unit
- `PUT /api/adminunits/{id}` - Update admin unit
- `DELETE /api/adminunits/{id}` - Delete admin unit

#### Finance Units
- `GET /api/financeunits` - Get all finance units
- `GET /api/financeunits/{id}` - Get finance unit by ID
- `POST /api/financeunits` - Create finance unit

### GraphQL

**Endpoint**: `POST /graphql`

**Example Query**:
```graphql
query {
  getAdminUnits {
    id
    adminCode
    name
    adminType
    unitCode
  }
}
```

**Example Mutation**:
```graphql
mutation {
  createAdminUnit(
    adminCode: 1001
    name: "New Unit"
    adminType: "T"
    unitCode: "NEW"
  ) {
    id
    name
  }
}
```

### Swagger/OpenAPI

Access API documentation at: `http://localhost:5000/swagger`

### Health Checks

- `GET /health` - Application health status

## Authentication

JWT Bearer tokens are required for API access:

1. **Generate Token** (requires authentication service)
2. **Include in Request Headers**:
   ```
   Authorization: Bearer <token>
   ```

## Domain Models

### AdminUnit
- AdminCode: Unique identifier
- Name: Unit name
- AdminType: Type (T=Travel, S=Stay, M=Meeting)
- UnitCode: Short code reference
- Image and sorting metadata

### FinanceUnit
- UnitId: Unique identifier
- UnitCode: Short code reference
- Name: Unit name
- OracleCode: Oracle system code
- LocationOption: Location segment code

### Related Entities
- AdminAccess: Access configuration and permissions
- AdminContact: Contact details and communication info
- AreaMaster: Geographic areas
- RouteMaster: Travel routes
- AreaRouteMap: Area-route relationships

## CQRS Implementation

### Commands (Write Operations)
- `CreateAdminUnitCommand`
- `UpdateAdminUnitCommand`
- `DeleteAdminUnitCommand`
- `CreateFinanceUnitCommand`

### Queries (Read Operations)
- `GetAllAdminUnitsQuery`
- `GetAdminUnitByIdQuery`
- `GetAdminUnitsByTypeQuery`
- `GetAllFinanceUnitsQuery`
- `GetFinanceUnitByIdQuery`

## Features

### Authentication & Authorization
- JWT token-based authentication
- Role-based access control
- Token refresh mechanism

### Domain Events
- AdminUnitCreatedEvent
- AdminUnitUpdatedEvent
- AdminUnitDeletedEvent
- FinanceUnitCreatedEvent

### Resilience Patterns
- Circuit Breaker (Polly)
- Retry policies for HTTP calls
- Health checks for dependencies

### Data Persistence
- Soft deletes via IsDeleted flag
- Audit trails (CreatedBy, ModifiedBy)
- Change tracking

### Integration
- RabbitMQ message publishing
- Azure Blob Storage for file management
- Event-driven architecture support

## Configuration

### Database

- **Connection String**: LocalDB or SQL Server
- **Migrations**: Automatic on startup
- **Seed Data**: Initial admin/finance units loaded

### Logging

- Console and file logging
- Serilog integration
- Structured logging throughout

### CORS

- Support for multiple origins
- Configurable in middleware
- Default: AllowAll policy

## Development

### Adding New Features

1. **Create Domain Entity** in `AdminService.Domain/Entities`
2. **Define Repository Interface** in `AdminService.Domain/Interfaces`
3. **Create DTOs** in `AdminService.Application/DTOs`
4. **Define Commands/Queries** in respective folders
5. **Implement Handlers** in `AdminService.Application/Handlers`
6. **Create API Controller** in `AdminService.API/Controllers`
7. **Add Migration** for database changes

### Running Tests

```bash
dotnet test
```

### Code Quality

- Clean Architecture principles
- SOLID design patterns
- Dependency injection
- Inversion of Control

## Deployment

### Build

```bash
dotnet build --configuration Release
```

### Publish

```bash
dotnet publish -c Release -o publish
```

### Docker (Optional)

Create Dockerfile in root:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "AdminService.API.dll"]
```

## Troubleshooting

### Database Connection Issues
- Verify LocalDB is running: `sqllocaldb info`
- Check connection string in appsettings.json
- Ensure user has permissions

### JWT Token Issues
- Secret key must be > 32 characters
- Token format: "Bearer <token>"
- Check token expiration

### RabbitMQ Connection
- Ensure RabbitMQ service is running
- Verify credentials in appsettings.json
- Check VirtualHost settings

## License

This project is licensed under the MIT License.

## Support

For issues or questions, contact the development team.
