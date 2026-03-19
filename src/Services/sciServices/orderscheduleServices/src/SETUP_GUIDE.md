# Order Scheduling Microservice - Setup Guide

## Quick Start

### Prerequisites
- .NET 8.0 SDK installed
- SQL Server LocalDB or Docker
- Visual Studio 2022 / VS Code
- RabbitMQ (optional, can use Docker)

### Development Setup (LocalDB)

1. **Install SQL Server LocalDB**
   - Download from: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Choose Express edition with LocalDB

2. **Navigate to project directory**
   ```bash
   cd OrderScheduleService
   ```

3. **Restore packages**
   ```bash
   dotnet restore
   ```

4. **Build solution**
   ```bash
   dotnet build
   ```

5. **Run migrations (automatic on startup)**
   - The database will be created and seeded automatically on first run

6. **Start the API**
   ```bash
   cd OrderScheduleService.API
   dotnet run
   ```

7. **Access the API**
   - Swagger UI: https://localhost:5001/swagger/index.html
   - GraphQL: https://localhost:5001/graphql
   - Health: https://localhost:5001/health

### Docker Setup

1. **Build and run with Docker Compose**
   ```bash
   docker-compose up -d
   ```

2. **Seed initial data**
   ```bash
   docker-compose exec api dotnet ef database update --project OrderScheduleService.Infrastructure
   ```

3. **Access services**
   - API: http://localhost:5000
   - RabbitMQ Management: http://localhost:15672 (guest/guest)
   - SQL Server: Server=localhost,1433; User=sa; Password=YourPassword123!

## Configuration Changes

### Change Database Connection
Edit `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Your-Connection-String"
}
```

### Change JWT Secret
Update in `appsettings.json`:
```json
"JwtSettings": {
  "SecretKey": "your-new-secret-key-minimum-32-characters"
}
```

### Configure Azure Blob Storage
```json
"AzureBlobStorage": {
  "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=account;AccountKey=key;EndpointSuffix=core.windows.net",
  "ContainerName": "your-container"
}
```

### Configure RabbitMQ
```json
"RabbitMq": {
  "HostName": "localhost",
  "Port": 5672,
  "UserName": "guest",
  "Password": "guest",
  "QueueName": "order.schedule.events"
}
```

## Troubleshooting

### Issue: Database connection failed
**Solution**: Check connection string and ensure SQL Server is running
```bash
# For LocalDB
SqlLocalDB info mssqllocaldb
```

### Issue: JWT token validation fails
**Solution**: Ensure JWT secret in appsettings matches what's configured
```bash
# Generate new secret (must be 32+ characters)
openssl rand -base64 32
```

### Issue: RabbitMQ connection refused
**Solution**: Ensure RabbitMQ is running
```bash
docker-compose up -d rabbitmq
```

### Issue: Port already in use
**Solution**: Change ports in docker-compose.yml or launchSettings.json

## First Request Example

### 1. Generate Token
```bash
curl -X POST "https://localhost:5001/api/authentication/token" \
  -H "Content-Type: application/json" \
  -d '{"userId":"user1","userName":"John Doe","roles":["User"]}'
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "tokenType": "Bearer"
}
```

### 2. Create Order
```bash
curl -X POST "https://localhost:5001/api/orders" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "customerCode": "CUST001",
    "companyUnitId": 1,
    "modifiedUserId": "admin",
    "details": [{
      "itemId": 1001,
      "itemName": "Product A",
      "orderQuantity": 100,
      "price": 25.00
    }]
  }'
```

### 3. Query in GraphQL
Open https://localhost:5001/graphql

```graphql
query {
  getOrders {
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

## Database Migrations

### Create new migration
```bash
cd OrderScheduleService.Infrastructure
dotnet ef migrations add MigrationName
```

### Update database
```bash
dotnet ef database update
```

### Remove last migration
```bash
dotnet ef migrations remove
```

## Project Structure

```
OrderScheduleService/
├── OrderScheduleService.Domain/
│   ├── Aggregates/          # TiedOrderAggregate, ScheduleAggregate
│   ├── Common/              # Entity, AggregateRoot, ValueObject, DomainEvent
│   ├── Entities/            # OrderDetail, ScheduleDetail, etc.
│   ├── Events/              # Domain events
│   ├── ValueObjects/        # OrderQuantity, OrderNumber, etc.
│   └── Interfaces/          # Repository interfaces
│
├── OrderScheduleService.Application/
│   ├── Commands/            # CQRS commands
│   ├── Queries/             # CQRS queries
│   ├── CommandHandlers/     # Command handlers
│   ├── QueryHandlers/       # Query handlers
│   ├── DTOs/                # Data transfer objects
│   └── Mapping/             # AutoMapper profiles
│
├── OrderScheduleService.Infrastructure/
│   ├── Persistence/         # DbContext, seeding
│   ├── Repositories/        # Repository implementations
│   ├── Migrations/          # EF Core migrations
│   └── InfrastructureServiceExtensions.cs
│
├── OrderScheduleService.API/
│   ├── Controllers/         # REST API controllers
│   ├── GraphQL/             # GraphQL schema
│   ├── Services/            # JWT, RabbitMQ, Blob Storage
│   ├── Middleware/          # Error handling, logging
│   ├── Program.cs           # Startup configuration
│   ├── appsettings.json     # Configuration
│   └── Dockerfile           # Docker build
│
├── OrderScheduleService.IntegrationEvents/
│   └── IntegrationEvents.cs # RabbitMQ events
│
└── docker-compose.yml       # Docker Compose for dev environment
```

## Useful Commands

### Run tests
```bash
dotnet test
```

### Build release
```bash
dotnet build -c Release
```

### Publish for production
```bash
dotnet publish -c Release -o ./publish
```

### Format code
```bash
dotnet format
```

### Analyze code
```bash
dotnet analyzers
```

## Performance Tips

1. Use include patterns in queries to avoid N+1
2. Enable query caching for read-heavy operations
3. Use async/await patterns
4. Configure Polly retry policies appropriately
5. Monitor database indexes
6. Use connection pooling

## Security Checklist

- [ ] Change default JWT secret
- [ ] Update SQL Server password
- [ ] Configure HTTPS in production
- [ ] Enable CORS only for trusted origins
- [ ] Validate all user inputs
- [ ] Use parameterized queries (EF Core does this)
- [ ] Implement rate limiting
- [ ] Set secure headers
- [ ] Enable audit logging
- [ ] Regular security updates

## Deployment

### To Azure App Service
```bash
# Install Azure CLI
az login
az group create --name MyResourceGroup --location eastus
az appservice plan create --name MyServicePlan --resource-group MyResourceGroup --sku B1
az webapp create --resource-group MyResourceGroup --plan MyServicePlan --name MyApiName
az webapp deployment source config-zip --resource-group MyResourceGroup --name MyApiName --src ./publish.zip
```

### To Docker Hub
```bash
docker build -t myrepo/orderschedule:latest -f OrderScheduleService.API/Dockerfile .
docker push myrepo/orderschedule:latest
```

## Support & Resources

- Official Docs: https://learn.microsoft.com/dotnet
- Entity Framework: https://learn.microsoft.com/ef/core
- MediatR: https://github.com/jbogard/MediatR
- Hot Chocolate (GraphQL): https://chillicream.com/
- Polly (Resilience): https://github.com/App-vNext/Polly

## Feedback

For issues, improvements, or feature requests, please contact the development team.

---

Last Updated: March 18, 2026
