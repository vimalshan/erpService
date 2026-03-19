# 🚀 BUILD AND DEPLOYMENT GUIDE

## Prerequisites Checklist

- [ ] .NET 8.0 SDK installed (`dotnet --version`)
- [ ] SQL Server or LocalDB installed
- [ ] Visual Studio 2022 or VS Code with C# extension
- [ ] Entity Framework Core CLI: `dotnet tool install --global dotnet-ef`
- [ ] RabbitMQ installed (optional, for messaging)
- [ ] Azure Functions Core Tools (for Azure Functions)
- [ ] Git installed (optional)

## Step 1: Environment Setup

### 1.1 Install .NET 8.0

```bash
# Check if installed
dotnet --version

# If not installed, download from https://dotnet.microsoft.com/download/dotnet/8.0
```

### 1.2 Verify SQL Server LocalDB

```bash
# Check if LocalDB is running
sqllocaldb info

# Start LocalDB instance
sqllocaldb start mssqllocaldb

# Create new instance (if needed)
sqllocaldb create mssqllocaldb
```

### 1.3 Install Required Tools

```bash
# Install EF Core tools
dotnet tool install --global dotnet-ef

# Install Azure Functions Core Tools (for Azure Functions development)
choco install azure-functions-core-tools-4 -y
# OR
npm install -g azure-functions-core-tools@4 --unsafe-perm true
```

## Step 2: Database Setup

### 2.1 Create Database Using T-SQL Scripts

**Using SQL Server Management Studio:**

1. Connect to `(localdb)\MSSQLLocalDB`
2. Open `MOD_MobileExpenseManagement_Tables.sql` → Execute (F5)
3. Open `MOD_MobileExpenseManagement_Procedures.sql` → Execute (F5)
4. Open `MobileExpenseManagement_Database_Init.sql` → Execute (F5)
5. Open `MobileExpenseManagement_SampleData.sql` → Execute (F5)

**Using Command Line (sqlcmd):**

```bash
# Navigate to the solution directory
cd e:\ERPMicroservice\src\Services\sparshServices\mobileexpenseServices

# Execute scripts in order
sqlcmd -S (localdb)\MSSQLLocalDB -i MOD_MobileExpenseManagement\MOD_MobileExpenseManagement_Tables.sql

sqlcmd -S (localdb)\MSSQLLocalDB -i MOD_MobileExpenseManagement\MOD_MobileExpenseManagement_Procedures.sql

sqlcmd -S (localdb)\MSSQLLocalDB -i MobileExpenseManagement_Database_Init.sql

sqlcmd -S (localdb)\MSSQLLocalDB -i MobileExpenseManagement_SampleData.sql
```

### 2.2 Verify Database Creation

```sql
-- Execute in SQL Server Management Studio
USE SPARSHDB;

-- Check tables exist
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo';

-- Check sequences exist
SELECT * FROM sys.sequences WHERE schema_id = SCHEMA_ID('dbo');

-- Check sample data
SELECT COUNT(*) As 'Total Expenses' FROM MOBEXP_DET;
SELECT COUNT(*) As 'Total Files' FROM MOBEXP_FILE;
```

## Step 3: Solution Build

### 3.1 Using Visual Studio

1. Open `MobileExpenseManagement.sln`
2. Right-click Solution → **Restore NuGet Packages**
3. Build → **Build Solution** (Ctrl+Shift+B)
   - Verify: 0 errors, 0 warnings
4. Build → **Clean Solution** (optional for clean build)

### 3.2 Using Command Line

```bash
# Navigate to solution directory
cd E:\ERPMicroservice\src\Services\sparshServices\mobileexpenseServices

# Restore packages
dotnet restore MobileExpenseManagement.sln

# Build solution
dotnet build MobileExpenseManagement.sln

# Build with specific configuration
dotnet build MobileExpenseManagement.sln -c Release

# Verbose output (if needed)
dotnet build MobileExpenseManagement.sln --verbosity detailed
```

### 3.3 Expected Build Output

```
Build succeeded.
  0 Warning(s)
  0 Error(s)
  Time Elapsed 00:XX:XX.XX
```

## Step 4: Configuration

### 4.1 Update Connection Strings

Edit `MobileExpenseManagement.API\appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SPARSHDB;Integrated Security=True;"
  }
}
```

### 4.2 Generate JWT Secret Key

```csharp
// In any .NET console app or: https://www.base64encode.org
using System;
using System.Security.Cryptography;
using System.Text;

var key = new byte[64];
using (var generator = RandomNumberGenerator.Create())
    generator.GetBytes(key);

var base64Key = Convert.ToBase64String(key);
Console.WriteLine(base64Key);  // Use this in appsettings.json
```

Update `appsettings.json`:

```json
{
  "Jwt": {
    "SecretKey": "your-generated-base64-key-here",
    "Issuer": "mobileexpensemanagement",
    "Audience": "mobileexpensemanagement-api",
    "ExpirationInMinutes": 60
  }
}
```

### 4.3 Azure Configuration (if using cloud services)

```json
{
  "ConnectionStrings": {
    "BlobStorageConnection": "DefaultEndpointsProtocol=https;AccountName=YOUR_ACCOUNT;AccountKey=YOUR_KEY;EndpointSuffix=core.windows.net"
  },
  "RabbitMQ": {
    "HostName": "rabbitmq.example.com",
    "Port": 5672,
    "Username": "username",
    "Password": "password"
  }
}
```

## Step 5: Run Application

### 5.1 Using Visual Studio

1. **Set Startup Project**: Right-click `MobileExpenseManagement.API` → Set as Startup Project
2. **Start Debugging**: Press `F5` or Debug → Start Debugging
3. **Wait for Launch**: Application builds and launches
4. **Access APIs**:
   - REST API: https://localhost:44301/swagger
   - GraphQL: https://localhost:44301/graphql
   - Health Check: https://localhost:44301/health

### 5.2 Using Command Line

```bash
# Navigate to API project
cd MobileExpenseManagement.API

# Run application
dotnet run

# Run in Release mode
dotnet run --configuration Release

# Specify port (HTTPS)
dotnet run --urls "https://localhost:44301"
```

### 5.3 Verify Application

```bash
# Test health check
curl -k https://localhost:44301/health

# Test API (with JWT token)
curl -k -X GET https://localhost:44301/api/expenses/trip/1001 \
  -H "X-User-Id: 101" \
  -H "Authorization: Bearer <token>"

# Test GraphQL
curl -k -X POST https://localhost:44301/graphql \
  -H "Content-Type: application/json" \
  -d '{"query": "query { getExpenseById(expenseId: 1000) { id } }"}'
```

## Step 6: EF Core Migrations (If Schema Changes)

### 6.1 Create Migration

```bash
cd MobileExpenseManagement.API

# Add migration after model changes
dotnet ef migrations add "MigrationName" \
  --project ..\MobileExpenseManagement.Infrastructure \
  --startup-project .

# Example:
dotnet ef migrations add "AddNewField" \
  --project ..\MobileExpenseManagement.Infrastructure \
  --startup-project .
```

### 6.2 Apply Migration

```bash
# Update database with new migrations
dotnet ef database update \
  --project ..\MobileExpenseManagement.Infrastructure \
  --startup-project .

# Update to specific migration
dotnet ef database update "MigrationName" \
  --project ..\MobileExpenseManagement.Infrastructure \
  --startup-project .
```

### 6.3 Remove Migration

```bash
# Remove last migration (before applying to database)
dotnet ef migrations remove \
  --project ..\MobileExpenseManagement.Infrastructure \
  --startup-project .
```

## Step 7: Azure Functions (Optional)

### 7.1 Install Azure Functions Tools

```bash
# Check if installed
func --version

# Install if needed (Windows with Chocolatey)
choco install azure-functions-core-tools-4 -y

# Or with npm
npm install -g azure-functions-core-tools@4 --unsafe-perm true
```

### 7.2 Run Functions Locally

```bash
cd MobileExpenseManagement.AzureFunctions

# Start function runtime
func start

# Output:
# Azure Functions Core Tools
# Function App started. Press CTRL+C to exit.
# HTTP trigger endpoint: http://localhost:7071/api/ProcessExpenseFile
```

### 7.3 Test Azure Functions

```bash
# Test HTTP-triggered function
curl -X POST http://localhost:7071/api/ProcessExpenseFile \
  -H "Content-Type: application/json" \
  -d '{"fileName": "receipt.pdf"}'

# Check timer-triggered function logs
# Logs appear in console output
```

## Step 8: Docker Build (Optional)

### 8.1 Create Dockerfile

Create file: `Dockerfile` in solution root

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY "MobileExpenseManagement.Domain/MobileExpenseManagement.Domain.csproj" "MobileExpenseManagement.Domain/"
COPY "MobileExpenseManagement.Application/MobileExpenseManagement.Application.csproj" "MobileExpenseManagement.Application/"
COPY "MobileExpenseManagement.Infrastructure/MobileExpenseManagement.Infrastructure.csproj" "MobileExpenseManagement.Infrastructure/"
COPY "MobileExpenseManagement.API/MobileExpenseManagement.API.csproj" "MobileExpenseManagement.API/"
RUN dotnet restore "MobileExpenseManagement.API/MobileExpenseManagement.API.csproj"
COPY . .
RUN dotnet build "MobileExpenseManagement.API/MobileExpenseManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MobileExpenseManagement.API/MobileExpenseManagement.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80 443
ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENTRYPOINT ["dotnet", "MobileExpenseManagement.API.dll"]
```

### 8.2 Build Docker Image

```bash
# Build image
docker build -t mobileexpense:latest .

# List images
docker images | findstr mobileexpense

# Run container
docker run -d \
  -p 5000:80 \
  -e "ConnectionStrings__DefaultConnection=Server=host.docker.internal;Database=SPARSHDB;..." \
  --name mobileexpense-app \
  mobileexpense:latest

# Check logs
docker logs mobileexpense-app

# Stop container
docker stop mobileexpense-app
```

## Step 9: Publish to Azure (Optional)

### 9.1 Prerequisites

```bash
# Install Azure CLI
choco install azure-cli -y

# Login to Azure
az login

# List subscriptions
az account list

# Set default subscription
az account set --subscription "YOUR-SUBSCRIPTION-ID"
```

### 9.2 Create App Service

```bash
# Create resource group
az group create \
  --name "rg-mobileexpense" \
  --location "eastus"

# Create App Service Plan
az appservice plan create \
  --name "plan-expense" \
  --resource-group "rg-mobileexpense" \
  --sku "B2" \
  --is-linux

# Create Web App
az webapp create \
  --name "app-mobileexpense" \
  --resource-group "rg-mobileexpense" \
  --plan "plan-expense" \
  --runtime "DOTNETCORE:8.0"
```

### 9.3 Deploy Application

```bash
# Publish application
dotnet publish -c Release -o ./publish/MobileExpenseManagement.API \
  MobileExpenseManagement.API/MobileExpenseManagement.API.csproj

# Create ZIP file
Compress-Archive -Path ./publish/* -DestinationPath app.zip -Force

# Deploy
az webapp deployment source config-zip \
  --resource-group "rg-mobileexpense" \
  --name "app-mobileexpense" \
  --src-path "./app.zip"

# Configure app settings
az webapp config appsettings set \
  --name "app-mobileexpense" \
  --resource-group "rg-mobileexpense" \
  --settings "ConnectionStrings__DefaultConnection=Server=your-server.database.windows.net,1433;Initial Catalog=SPARSHDB;User ID=admin;Password=P@ssw0rd"
```

### 9.4 Verify Deployment

```bash
# Get application URL
az webapp show \
  --resource-group "rg-mobileexpense" \
  --name "app-mobileexpense" \
  --query "defaultHostName" --output tsv

# Test health endpoint
curl https://app-mobileexpense.azurewebsites.net/health

# View logs
az webapp log tail \
  --name "app-mobileexpense" \
  --resource-group "rg-mobileexpense"
```

## Troubleshooting

### Build Failures

```bash
# Clean build
dotnet clean MobileExpenseManagement.sln
dotnet restore MobileExpenseManagement.sln
dotnet build MobileExpenseManagement.sln

# Check specific project
dotnet build MobileExpenseManagement.API/MobileExpenseManagement.API.csproj -v diag
```

### Database Connection Issues

```sql
-- Verify SQL Server is running
SELECT @@VERSION;

-- Check database exists
SELECT name FROM sys.databases WHERE name = 'SPARSHDB';

-- Check connection with timeout
EXEC sp_executesql N'SELECT 1'
```

### Application Won't Start

1. Check for port conflicts: `netstat -ano | findstr :44301`
2. Verify configuration: `MobileExpenseManagement.API\appsettings.json`
3. Check logs in Visual Studio Output window
4. Restart IIS/Kestrel: `iisreset` or stop/restart application

### EF Migration Issues

```bash
# Reset migrations (development only!)
dotnet ef database drop --project ..\MobileExpenseManagement.Infrastructure --startup-project .
dotnet ef database update --project ..\MobileExpenseManagement.Infrastructure --startup-project .
```

## Deployment Checklist

- [ ] All NuGet packages restored
- [ ] Database created and initialized
- [ ] appsettings.json configured with correct connection strings
- [ ] JWT secret key generated and configured
- [ ] Solution builds without errors
- [ ] All unit tests pass (if applicable)
- [ ] Health check endpoint responds
- [ ] Swagger documentation loads
- [ ] GraphQL playground loads
- [ ] Sample API calls work with valid JWT token
- [ ] Azure Functions tested locally (if using)
- [ ] Docker image builds successfully (if using)
- [ ] Production configuration prepared
- [ ] Environment variables set correctly
- [ ] Monitoring and logging configured

## Performance Tips

1. **Enable HTTPS redirection** (already enabled)
2. **Use connection pooling** (enabled in connection string)
3. **Cache frequently accessed data**
4. **Use async/await throughout**
5. **Implement pagination for large datasets**
6. **Use database indexes on frequently queried columns**
7. **Monitor database query performance**
8. **Use Circuit Breaker for external calls**

## Security Checklist

- [ ] JWT secret key is strong (256-bit)
- [ ] Connection strings don't contain hardcoded credentials
- [ ] HTTPS is enforced  
- [ ] CORS is properly configured
- [ ] Authentication middleware is active
- [ ] Data validation is in place
- [ ] SQL parameters are used (EF Core does this)
- [ ] File uploads are validated
- [ ] Sensitive data is not logged

That's it! Your Mobile Expense Management microservice is ready to deploy!
