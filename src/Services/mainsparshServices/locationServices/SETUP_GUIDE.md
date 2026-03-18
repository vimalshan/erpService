# Location Service - Complete Setup & Deployment Guide

## 📦 Prerequisites

### Required Software
- **Visual Studio 2022** (v17.8+) or **VS Code**
- **.NET 8.0 SDK** - https://dotnet.microsoft.com/download/dotnet/8.0
- **SQL Server** or **LocalDB** (installed with Visual Studio)
- **Git** - https://git-scm.com/

### Optional But Recommended
- **RabbitMQ** - https://www.rabbitmq.com/download.html (for messaging)
- **Redis** - https://redis.io/download (for caching)
- **Azure Storage Emulator** - For blob storage testing
- **Postman** or **Insomnia** - For API testing
- **Entity Framework Core Power Tools** - VS Extension

---

## 🚀 Step-by-Step Setup

### Step 1: Clone/Setup Repository
```bash
# Navigate to desired directory
cd e:\ERPMicroservice\src\Services\mainsparshServices\locationServices

# Initialize git (if not already done)
git init
git add .
git commit -m "Initial commit: LocationService complete implementation"
```

### Step 2: Open in IDE

#### Visual Studio 2022
1. Open Visual Studio
2. File → Open → Folder
3. Navigate to `LocationService` folder
4. The solution will load automatically

#### VS Code
```bash
# Open in VS Code
code .

# Install C# dev kit (if prompted)
# Install Recommended Extensions
```

### Step 3: Restore NuGet Packages
```bash
# From root directory
dotnet restore

# Or in Package Manager Console
Update-Package -Reinstall
```

### Step 4: Database Setup

#### Option A: Using LocalDB (Recommended for Development)
```bash
# Verify LocalDB instance exists
sqllocaldb info v13.0

# If needed, create instance
sqllocaldb create v13.0

# Navigate to API project
cd LocationService.API

# Check database connection in appsettings.Development.json
# Default: Data Source=(localdb)\MSSQLLocalDB;...

# Apply migrations
dotnet ef database update

# Verify database created
sqllocaldb query v13.0
```

#### Option B: Using SQL Server
1. Modify connection string in `appsettings.Development.json`
2. Use your SQL Server instance name
3. Run migrations: `dotnet ef database update`

### Step 5: Configure Settings

#### Edit appsettings.Development.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;..."
  },
  "JwtSettings": {
    "SecretKey": "your-development-secret-key-min-32-characters-long",
    "Issuer": "LocationServiceApi",
    "Audience": "LocationServiceUsers",
    "ExpiryMinutes": 1440
  },
  "RabbitMq": {
    "Host": "localhost",
    "Port": 5672,
    "User": "guest",
    "Password": "guest"
  }
}
```

### Step 6: Build Solution
```bash
# Clean build
dotnet clean

# Build solution
dotnet build

# Or in Visual Studio: Ctrl+Shift+B
```

### Step 7: Run API
```bash
# Navigate to API project
cd LocationService.API

# Run with development configuration
dotnet run --configuration Development

# Expected output:
# info: Microsoft.Hosting.Lifetime[14]
# Now listening on: https://localhost:7000
# Now listening on: http://localhost:5000
```

### Step 8: Verify API is Running
```bash
# In another terminal
curl http://localhost:5000/health

# Expected response:
# {"status":"Healthy"}
```

### Step 9: Access API Documentation
Open browser and navigate to:
- **Swagger UI**: http://localhost:5000/swagger/index.html
- **GraphQL**: http://localhost:5000/graphql
- **Health Check**: http://localhost:5000/health

---

## 🧪 Testing the API

### Using Swagger UI (Recommended)
1. Open: http://localhost:5000/swagger/index.html
2. Click on any endpoint
3. Click "Try it out"
4. Modify parameters as needed
5. Click "Execute"

### Using cURL

#### Create a Location
```bash
curl -X POST http://localhost:5000/api/locations \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "locationCode": "LOC-TEST",
    "locationName": "Test Location",
    "city": "TestCity",
    "country": "TestCountry"
  }'
```

#### Get All Locations
```bash
curl -X GET http://localhost:5000/api/locations \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

#### Get Location by ID
```bash
curl -X GET http://localhost:5000/api/locations/1 \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### Using Postman

#### Import Collection
1. Download our Postman collection (if available)
2. Import into Postman
3. Set base URL: `http://localhost:5000`
4. Set JWT token in Authorization tab

#### Create Request
1. New → Request
2. Method: GET
3. URL: `http://localhost:5000/api/locations`
4. Headers → Add shared Authorization
5. Send

---

## 🔐 JWT Authentication Setup

### Generate JWT Token for Testing

#### Using .NET CLI
```bash
# In Program.cs, temporarily add an endpoint to generate tokens
[HttpPost("auth/token")]
public string GetToken()
{
    var jwtService = HttpContext.RequestServices.GetService<IJwtTokenService>();
    return jwtService.GenerateToken(1, "admin@test.com", new[] { "Admin" });
}
```

#### Then call it:
```bash
curl -X POST http://localhost:5000/api/auth/token
```

#### For Development: Use Test Token Generator
Create a temporary `TokenGenerator.cs`:
```csharp
// Use in Program.cs to generate a test token on startup
var tokenService = app.Services.GetRequiredService<IJwtTokenService>();
var testToken = tokenService.GenerateToken(1, "test@example.com", new[] { "Admin" });
Console.WriteLine($"Test Token: {testToken}");
```

### Add Token to Requests
```bash
# Method 1: Header
curl -H "Authorization: Bearer eyJhbGc..." http://localhost:5000/api/locations

# Method 2: Swagger UI
# Click "Authorize" button, paste token, click Authorize
```

---

## 🔍 Debugging

### Enable Debug Logging
Add to `Program.cs`:
```csharp
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Optional: Add file logging
builder.Logging.AddFile("logs/app-{Date}.txt");
```

### Attach Debugger (Visual Studio)
1. Set breakpoint in code
2. Press F5 or Debug → Start Debugging
3. Call API endpoint
4. Debugger will pause at breakpoint

### SQL Queries Logging
```csharp
.LogTo(Console.WriteLine, LogLevel.Information)
.EnableSensitiveDataLogging()
```

### RabbitMQ Monitoring
Access RabbitMQ Admin: http://localhost:15672
- Username: guest
- Password: guest

---

## 📊 Database Management

### View Current Database
```sql
-- Open SQL Server Management Studio (SSMS)
-- Connect to (localdb)\MSSQLLocalDB
-- Expand Databases
-- Find SRFSPARSHDB or your configured database

-- Or via command line:
sqlcmd -E -S (localdb)\MSSQLLocalDB
```

### Add New Migration
```bash
cd LocationService.Infrastructure
dotnet ef migrations add MigrationName -p LocationService.Infrastructure -s LocationService.API
```

### Update Database
```bash
# Update to latest
dotnet ef database update -s LocationService.API

# Update to specific migration
dotnet ef database update MigrationName -s LocationService.API

# Rollback last migration
dotnet ef migrations remove -p LocationService.Infrastructure -s LocationService.API
```

### Reset Database
```bash
# Delete and recreate (dev only!)
dotnet ef database drop -s LocationService.API --force
dotnet ef database update -s LocationService.API
```

---

## ☁️ Azure Deployment

### Prerequisites
- Azure subscription
- Azure CLI installed
- Azure Storage account (for blobs)

### Deploy to Azure App Service

#### Step 1: Create Resource Group
```bash
az group create --name LocationServiceRG --location eastus
```

#### Step 2: Create App Service Plan
```bash
az appservice plan create \
  --name LocationServicePlan \
  --resource-group LocationServiceRG \
  --sku B2 --is-linux
```

#### Step 3: Create Web App
```bash
az webapp create \
  --name location-service-app \
  --resource-group LocationServiceRG \
  --plan LocationServicePlan \
  --runtime "DOTNETCORE|8.0"
```

#### Step 4: Configure Database Connection
```bash
az webapp config appsettings set \
  --name location-service-app \
  --resource-group LocationServiceRG \
  --settings ConnectionStrings__DefaultConnection="your-connection-string" \
  JwtSettings__SecretKey="your-secret"
```

#### Step 5: Publish Application
```bash
# Build
dotnet publish -c Release -o ./publish

# Deploy using ZIP
az webapp deployment source config-zip \
  --resource-group LocationServiceRG \
  --name location-service-app \
  --src ".\publish.zip"
```

### Deploy to Docker

#### Create Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LocationService.API/LocationService.API.csproj", "LocationService.API/"]
RUN dotnet restore "LocationService.API/LocationService.API.csproj"
COPY . .
RUN dotnet build "LocationService.API/LocationService.API.csproj" -c Release

FROM build AS publish
RUN dotnet publish "LocationService.API/LocationService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocationService.API.dll"]
```

#### Build and Run Docker Image
```bash
# Build
docker build -t location-service:latest .

# Run locally
docker run -p 5000:80 location-service:latest

# Push to registry
docker tag location-service:latest myregistry.azurecr.io/location-service:latest
docker push myregistry.azurecr.io/location-service:latest
```

---

## 🐛 Troubleshooting Checklist

### API Won't Start
- [ ] .NET 8.0 SDK installed? → `dotnet --version`
- [ ] Correct working directory? → `cd LocationService.API`
- [ ] Database accessible? → Check connection string
- [ ] Ports free? → Check 5000/7000 not in use

### Database Connection Failed
- [ ] LocalDB running? → `sqllocaldb info v13.0`
- [ ] Connection string correct? → Check appsettings
- [ ] Network connectivity? → Ping server
- [ ] Credentials valid? → Verify in SSMS

### JWT Token Invalid
- [ ] Secret key matches? → Check appsettings
- [ ] Token not expired? → Generate new one
- [ ] Header format correct? → `Authorization: Bearer {token}`
- [ ] Token valid JSON? → Check at jwt.io

### Migrations Won't Apply
- [ ] DbContext in correct project? → Should be Infrastructure
- [ ] Startup project correct? → Should be API
- [ ] String value: `-s LocationService.API`
- [ ] No pending changes? → Check `git status`

### RabbitMQ Connection Refused
- [ ] Service running? → Check Windows Services
- [ ] Host/Port correct? → Default localhost:5672
- [ ] Credentials valid? → guest/guest default
- [ ] Firewall blocking? → Check Windows Firewall

---

## 📝 Post-Deployment Checklist

- [ ] API responds to health check
- [ ] Database contains seed data
- [ ] Swagger documentation accessible
- [ ] JWT token generation working
- [ ] RabbitMQ publishing functional
- [ ] Logging configured and working
- [ ] Cache implementation chosen
- [ ] Error handling tested
- [ ] CORS properly configured
- [ ] HTTPS enabled (production)

---

## 🎓 Learning Resources

### Architecture Patterns
- DDD (Domain-Driven Design): https://en.wikipedia.org/wiki/Domain-driven_design
- CQRS: https://www.microsoft.com/en-us/research/publication/cqrs-command-query-responsibility-segregation/
- Event Sourcing: https://martinfowler.com/eaaDev/EventSourcing.html

### .NET & Frameworks
- Entity Framework Core: https://learn.microsoft.com/en-us/ef/core/
- MediatR: https://github.com/jbogard/MediatR
- AutoMapper: https://automapper.org/

### Cloud & DevOps
- Azure App Service: https://azure.microsoft.com/en-us/services/app-service/
- Docker: https://www.docker.com/
- Kubernetes: https://kubernetes.io/

---

## 📞 Support & Reference

### Important Files
- `IMPLEMENTATION_README.md` - Detailed documentation
- `COMPLETION_SUMMARY.md` - Feature summary
- `ARCHITECTURE.md` - Architecture diagrams
- `DEVELOPER_GUIDE.md` - Developer reference
- `appsettings.json` - Configuration template

### Key Endpoints
- REST API: `/api/locations`, `/api/rooms`, `/api/roomresources`
- GraphQL: `/graphql`
- Health: `/health`
- Swagger: `/swagger/index.html`

### Default Credentials (Development Only)
```
UserId: 1
Email: admin@test.com
Roles: Admin, LocationManager
```

---

## ✨ Next Steps

1. **Explore Code**: Review architecture in solution
2. **Test Endpoints**: Use Swagger to test API
3. **Study Patterns**: Understand DDD and CQRS implementation
4. **Add Features**: Implement additional commands/queries
5. **Write Tests**: Add unit and integration tests
6. **Deploy**: Push to development/staging environment

---

**Setup Guide Version**: 1.0  
**Last Updated**: March 15, 2026  
**Framework**: .NET 8.0
