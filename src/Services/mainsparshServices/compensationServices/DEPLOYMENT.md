# Compensation Service - Build & Deployment Guide

## Prerequisites

- .NET 8 SDK (latest version)
- SQL Server or SQL Server LocalDB
- (Optional) Visual Studio 2022 Professional/Community or VS Code
- (Optional) RabbitMQ for messaging
- (Optional) Azure Storage Account for Blob Storage

## Step 1: Build the Solution

### Using .NET CLI

```bash
# Navigate to the solution directory
cd e:\ERPMicroservice\src\Services\mainsparshServices\compensationServices

# Restore NuGet packages
dotnet restore

# Build the solution
dotnet build CompensationService.sln --configuration Release

# Verify build output
# Should see: "Build succeeded" message
```

### Using Visual Studio

1. Open `CompensationService.sln` in Visual Studio 2022
2. Right-click on Solution > Build Solution
3. Verify no errors in the Error List window

## Step 2: Database Setup

### Create Database and Apply Migrations

```bash
# Navigate to the API project
cd CompensationService.API

# Apply migrations to database
dotnet ef database update --project ../CompensationService.Infrastructure

# Expected output: Applying migration '20260315000000_InitialCreate'
```

### Verify Database Creation

```bash
# Using SQL Server Management Studio or sqlcmd
# Connect to: (localdb)\MSSQLLocalDB

# Run this query to verify the table exists:
SELECT * FROM COMP_GRADE;

# Should return: 5 seed records (JR001, SR001, MG001, SEN001, DIR001)
```

## Step 3: Configure Application Settings

### Update appsettings.json (if needed)

Edit `CompensationService.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"CompensationService\";Command Timeout=0"
  },
  "Jwt": {
    "SecureKey": "CompensationServiceSecureKeyWithMinimumLength32Characters",
    "Issuer": "CompensationServiceIssuer",
    "Audience": "CompensationServiceAudience",
    "ExpirationMinutes": 60
  }
}
```

### For RabbitMQ (Optional)

Update `RabbitMQ` section if RabbitMQ server is running on different host:

```json
"RabbitMQ": {
  "HostName": "localhost",
  "UserName": "guest",
  "Password": "guest",
  "Port": 5672
}
```

### For Azure Blob Storage (Optional)

Update connection string in `appsettings.json` or use Azure Key Vault:

```json
"ConnectionStrings": {
  "AzureBlobStorage": "DefaultEndpointsProtocol=https;AccountName=YOUR_ACCOUNT;AccountKey=YOUR_KEY;EndpointSuffix=core.windows.net"
}
```

## Step 4: Run the Application

### Development Mode

```bash
# From CompensationService.API directory
dotnet run

# Expected output:
# info: Microsoft.Hosting.Lifetime[14]
#   Now listening on: https://localhost:7001
#   Now listening on: http://localhost:5001
```

### Using Visual Studio

1. Set `CompensationService.API` as startup project
2. Press F5 or Debug > Start Debugging
3. Application will launch at `https://localhost:7001`

### Using Docker (Optional)

```bash
# Build Docker image
docker build -t compensation-service:latest -f CompensationService.API/Dockerfile .

# Run container
docker run -p 443:443 -p 80:80 compensation-service:latest
```

## Step 5: Verify Installation

### Check API Health

```bash
# Health check endpoint
curl https://localhost:7001/health
# Expected: {"status":"Healthy","details":{...}}

# Ready check (database)
curl https://localhost:7001/health/ready
# Expected: {"status":"Healthy"}
```

### Test Swagger Documentation

```
Open browser: https://localhost:7001/swagger/index.html
```

### Test GraphQL

```
Open browser: https://localhost:7001/graphql
```

### Test REST Endpoint

```bash
# Get all grades
curl https://localhost:7001/api/compensation-grades \
  -H "Content-Type: application/json"

# Expected response:
# [
#   {
#     "gradeId": 1,
#     "gradeCode": "JR001",
#     "gradeName": "Junior Executive",
#     ...
#   },
#   ...
# ]
```

## Step 6: Test Endpoints

### Create New Grade

```bash
curl -X POST https://localhost:7001/api/compensation-grades \
  -H "Content-Type: application/json" \
  -d '{
    "gradeCode": "MGR001",
    "gradeName": "Manager",
    "gradeLevel": 3,
    "baseSalary": 50000,
    "hraPercentage": 15,
    "daPercentage": 8,
    "effectiveFrom": "2026-03-15"
  }'
```

### Get Specific Grade

```bash
curl https://localhost:7001/api/compensation-grades/1 \
  -H "Content-Type: application/json"
```

### Update Grade

```bash
curl -X PUT https://localhost:7001/api/compensation-grades/1 \
  -H "Content-Type: application/json" \
  -d '{
    "gradeName": "Senior Executive Updated",
    "baseSalary": 45000,
    "hraPercentage": 15,
    "daPercentage": 8
  }'
```

### Change Grade Status

```bash
curl -X PATCH https://localhost:7001/api/compensation-grades/1/status \
  -H "Content-Type: application/json" \
  -d '{
    "newStatus": "I"
  }'
```

### GraphQL Query

```bash
curl -X POST https://localhost:7001/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ compensationGrades { gradeId gradeCode gradeName baseSalary } }"
  }'
```

## Step 7: Verify Advanced Features

### Health Checks

- Overall Health: `https://localhost:7001/health`
- Ready Check: `https://localhost:7001/health/ready`

### Minimal APIs

- Minimal endpoints at: `https://localhost:7001/api/minimal/grades`

### Swagger/OpenAPI

- Documentation: `https://localhost:7001/swagger/index.html`

### GraphQL Playground

- GraphQL UI: `https://localhost:7001/graphql`

## Troubleshooting

### Build Issues

```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

### Database Connection Issues

```bash
# Check LocalDB is running
sqllocaldb info

# Start LocalDB if not running
sqllocaldb start MSSQLLocalDB

# Check connection string in appsettings.json
```

### Port Already in Use

```bash
# If port 7001 is in use, change in launchSettings.json
# Or use:
dotnet run --project CompensationService.API -- --urls="https://localhost:7002"
```

### EF Core Migration Issues

```bash
# Check current migrations
dotnet ef migrations list --project CompensationService.Infrastructure

# Remove last migration if needed
dotnet ef migrations remove --project CompensationService.Infrastructure

# Reapply migrations
dotnet ef database update --project CompensationService.Infrastructure
```

## Production Deployment

### Prerequisites

- SQL Server 2019 or higher
- .NET 8 Runtime
- IIS 10 or higher (for Windows hosting)
- Or use Azure App Service

### Deployment Steps

1. **Publish Release Build**
   ```bash
   dotnet publish CompensationService.API \
     --configuration Release \
     --output ./publish
   ```

2. **Create Database**
   - Deploy database using EF Core migrations
   - Or use SQL scripts in `CompensationService.Infrastructure\Migrations`

3. **Configure Production Settings**
   - Update `appsettings.Production.json`
   - Set secure JWT secrets
   - Configure Azure resources
   - Set environment variables

4. **Deploy to IIS**
   - Install .NET Hosting Bundle
   - Create IIS Application Pool (.NET 8)
   - Copy published files to wwwroot
   - Configure binding and SSL

5. **Deploy to Azure**
   ```bash
   # Using Azure CLI
   az webapp deployment source config-zip \
     --resource-group <group> \
     --name <app-name> \
     --src publish.zip
   ```

## Monitoring & Logging

### View Logs

```bash
# Logs are stored in ./logs directory
# Check latest log file:
tail -f ./logs/compensation-service-*.txt
```

### Application Insights (Optional)

```bash
# Add Application Insights to appsettings.json
"ApplicationInsights": {
  "InstrumentationKey": "YOUR_KEY"
}
```

## Performance Testing

### Using Apache AB

```bash
# Load testing
ab -n 1000 -c 10 https://localhost:7001/api/compensation-grades
```

### Using k6

```javascript
import http from 'k6/http';
import { check } from 'k6';

export let options = {
  vus: 10,
  duration: '30s',
};

export default function () {
  let response = http.get('https://localhost:7001/api/compensation-grades');
  check(response, {
    'status is 200': (r) => r.status === 200,
  });
}
```

## Maintenance

### Backup Database

```bash
# SQL Server backup
BACKUP DATABASE SRFSPARSHDB TO DISK = 'C:\Backups\compensation_service.bak';
```

### Clean Old Logs

```bash
# Remove logs older than 30 days
Get-ChildItem ./logs -Filter *.txt | Where-Object { $_.LastWriteTime -lt (Get-Date).AddDays(-30) } | Remove-Item
```

## Success Criteria

✅ Solution builds without errors
✅ Database migrations apply successfully
✅ Application starts without errors
✅ Health checks return Healthy status
✅ REST API endpoints respond with correct data
✅ GraphQL queries execute successfully
✅ Swagger documentation is accessible
✅ Authentication/Authorization working
✅ All seed data is present in database
✅ Minimal APIs are accessible

## Next Steps

1. Deploy Azure Functions
2. Configure RabbitMQ message consumers
3. Set up monitoring and alerts
4. Implement API authentication tokens
5. Add automated tests
6. Configure CI/CD pipeline
7. Set up backup and disaster recovery
8. Monitor performance and optimize

---

**Support**: For issues or questions, refer to the main README.md file
