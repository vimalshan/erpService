# Development Configuration & Setup Guide

## Project Setup

### Prerequisites Checklist
- [ ] .NET 8.0 SDK installed
- [ ] Visual Studio 2022 or VS Code
- [ ] SQL Server Express or LocalDB
- [ ] RabbitMQ installed and running
- [ ] Azure Storage Emulator (for local development)
- [ ] Azure Functions Core Tools

## Database Setup

### Create LocalDB Instance
```powershell
sqllocaldb create MasterDataDB
sqllocaldb start MasterDataDB
```

### Apply Migrations
```bash
cd src/MasterData.Infrastructure
dotnet ef database update -s ../MasterData.API/MasterData.API.csproj
```

### Seed Data
Data will be automatically seeded on first run from `DataSeeder.cs`

## Environment Variables

### appsettings.Development.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Debug"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MasterDataDB_Dev;Integrated Security=True;Encrypt=True;TrustServerCertificate=False"
  },
  "JwtSettings": {
    "Secret": "dev-super-secret-jwt-key-longer-than-16-characters-for-256",
    "Issuer": "masterdataservice-dev",
    "Audience": "masterdataapi-dev",
    "ExpirationMinutes": 1440
  }
}
```

## Running the Solution

### Start API Service
```bash
cd src/MasterData.API
dotnet run
```

Open browser: `https://localhost:7001/swagger`

### Start Azure Functions Locally
```bash
cd src/MasterData.Functions
func start
```

## Testing Endpoints

### cURL Examples

#### Get All Company Units
```bash
curl -X GET "https://localhost:7001/api/companyunits" \
  -H "Content-Type: application/json"
```

#### Create Company Unit (Requires Auth)
```bash
curl -X POST "https://localhost:7001/api/companyunits" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "code": "BR3",
    "name": "Branch 3"
  }'
```

#### GraphQL Query
```bash
curl -X POST "https://localhost:7001/graphql" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getCompanyUnits { id code name } }"
  }'
```

## Debugging

### Debug in Visual Studio
1. Set breakpoints in code
2. Press F5 to start debugging
3. Navigate to endpoints to trigger breakpoints

### Debug in VS Code
1. Install C# Dev Kit extension
2. Press F5 to start debugging
3. Confirm launch configuration

## Common Issues & Solutions

### Database Connection Failed
**Solution**: Verify `(localdb)\MSSQLLocalDB` exists
```powershell
sqllocaldb info
```

### Port Already in Use
**Solution**: Change port in launchSettings.json or use
```bash
netstat -ano | findstr :7001
taskkill /PID {PID} /F
```

### RabbitMQ Connection Refused
**Solution**: Start RabbitMQ service
```powershell
# Windows
Start-Service RabbitMQ

# Or access via Docker
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

## Performance Testing

### Using Apache Bench
```bash
ab -n 100 -c 10 "https://localhost:7001/api/companyunits"
```

### Using Bombardier
```bash
bombardier -n 1000 -c 100 "https://localhost:7001/api/companyunits"
```

## Security Checklist

- [ ] Change JWT secret in production
- [ ] Enable HTTPS only in production
- [ ] Configure CORS properly
- [ ] Use strong passwords for RabbitMQ
- [ ] Enable database encryption
- [ ] Set up SSL certificates
- [ ] Configure firewall rules

---

**Last Updated**: March 18, 2026
