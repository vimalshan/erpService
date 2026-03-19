# Mobile Expense Management Microservice

A comprehensive, production-ready ASP.NET Core 8 microservice for managing mobile/field employee expenses with advanced features.

## 📋 Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Installation & Setup](#installation--setup)
- [Configuration](#configuration)
- [API Endpoints](#api-endpoints)
- [GraphQL API](#graphql-api)
- [Authentication & Authorization](#authentication--authorization)
- [Database](#database)
- [Azure Functions](#azure-functions)
- [Building & Deployment](#building--deployment)
- [Health Checks](#health-checks)
- [Troubleshooting](#troubleshooting)

## ✨ Features

### Core Functionality
- ✅ Create, read, update, and delete mobile expenses
- ✅ File attachment management (receipts, invoices, photos)
- ✅ Multi-currency support
- ✅ Trip/Project-based expense tracking
- ✅ Category-based expense organization

### Advanced Features
- ✅ **CQRS Pattern** - Separate command and query handlers
- ✅ **Domain-Driven Design** - Entities, value objects, aggregations, domain events
- ✅ **REST API** - Comprehensive REST endpoints with Swagger documentation
- ✅ **GraphQL API** - Full GraphQL schema with mutations and queries
- ✅ **Minimal APIs** - Fast, lightweight endpoint configuration
- ✅ **JWT Authentication** - Secure token-based authentication
- ✅ **Role-Based Authorization** - Fine-grained access control
- ✅ **RabbitMQ Messaging** - Asynchronous event processing
- ✅ **Azure Blob Storage** - Secure file storage with lifecycle management
- ✅ **Azure Functions** - Background processing and scheduled tasks
- ✅ **Circuit Breaker Pattern** - Polly-based resilience policies
- ✅ **Health Checks** - Application and database health monitoring
- ✅ **Domain Events** - Domain-driven event sourcing
- ✅ **Entity Framework Core** - ORM with migrations
- ✅ **Dapper Integration** - High-performance data access for complex queries
- ✅ **Validation** - FluentValidation declarative validation
- ✅ **Logging & Tracing** - Structured logging and request tracing
- ✅ **CORS Support** - Cross-origin resource sharing
- ✅ **Exception Handling** - Global middleware-based exception handling

## 🏗️ Architecture

### Layered Architecture

```
┌─────────────────────────────────────────┐
│     API Layer (REST, GraphQL, Minimal)  │
├─────────────────────────────────────────┤
│    Application Layer (CQRS, Services)   │
├─────────────────────────────────────────┤
│       Domain Layer (Entities, Events)   │
├─────────────────────────────────────────┤
│  Infrastructure Layer (EF, Dapper, etc) │
├─────────────────────────────────────────┤
│    External Services (Azure, RabbitMQ)  │
└─────────────────────────────────────────┘
```

### Project Dependencies

```
MobileExpenseManagement.API
  └─ MobileExpenseManagement.Application
      └─ MobileExpenseManagement.Domain
      └─ MobileExpenseManagement.Infrastructure
          └─ MobileExpenseManagement.Domain

MobileExpenseManagement.AzureFunctions
  └─ MobileExpenseManagement.Application
```

## 📋 Prerequisites

- **Framework**: .NET 8.0 or higher
- **Database**: SQL Server 2019+ or LocalDB
- **Queue**: RabbitMQ 3.12+ (optional, for async messaging)
- **Cloud Storage**: Azure Storage Account (for file management)
- **IDE**: Visual Studio 2022 or VS Code with C# Dev Kit
- **Tools**: 
  - Entity Framework Core CLI (`dotnet ef` global tool)
  - Azure Functions Core Tools (for local Azure Functions development)

## 📁 Project Structure

```
MobileExpenseManagement/
├── MobileExpenseManagement.Domain/          # Domain layer
│   ├── Entities/                           # Business entities
│   ├── ValueObjects/                       # Value objects
│   ├── Aggregates/                         # Aggregate roots
│   └── Events/                             # Domain events
├── MobileExpenseManagement.Application/     # Application layer
│   ├── Commands/                           # Command handlers
│   ├── Queries/                            # Query handlers
│   ├── DTOs/                               # Data transfer objects
│   ├── Behaviors/                          # CQRS pipeline behaviors
│   ├── EventHandlers/                      # Domain event handlers
│   └── Common/                             # Interfaces & mappings
├── MobileExpenseManagement.Infrastructure/  # Infrastructure layer
│   ├── Data/                               # DbContext & migrations
│   ├── Repositories/                       # EF & Dapper repositories
│   ├── Messaging/                          # RabbitMQ implementation
│   └── BlobStorage/                        # Azure Blob Storage
├── MobileExpenseManagement.API/             # API layer
│   ├── Controllers/                        # REST controllers
│   ├── GraphQL/                            # GraphQL types
│   ├── Middleware/                         # Custom middleware
│   └── Extensions/                         # DI configuration
└── MobileExpenseManagement.AzureFunctions/ # Background tasks
```

## 🚀 Installation & Setup

### 1. Clone/Copy Repository

```bash
cd e:\ERPMicroservice\src\Services\sparshServices\mobileexpenseServices
```

### 2. Restore NuGet Packages

**Using Visual Studio:**
- Open `MobileExpenseManagement.sln`
- Right-click Solution → Restore NuGet Packages

**Using Command Line:**
```bash
dotnet restore
```

### 3. Create Database

**Using SQL Server Management Studio:**
1. Execute `MOD_MobileExpenseManagement_Tables.sql` first
2. Execute `MOD_MobileExpenseManagement_Procedures.sql`
3. Execute `MobileExpenseManagement_Database_Init.sql`

**Using Entity Framework CLI:**
```bash
cd MobileExpenseManagement.API
dotnet ef database update --startupProject . --project ..\MobileExpenseManagement.Infrastructure
```

### 4. Seed Sample Data

```bash
# Using SQL Server Management Studio
Execute MobileExpenseManagement_SampleData.sql

# OR using SQLCMD
sqlcmd -S (localdb)\MSSQLLocalDB -i MobileExpenseManagement_SampleData.sql -v DatabaseName="SPARSHDB"
```

## ⚙️ Configuration

### appsettings.json

Update the following sections:

#### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SPARSHDB;Integrated Security=True;"
  }
}
```

#### JWT Configuration
```json
{
  "Jwt": {
    "SecretKey": "your-256-bit-secret-key-min-32-chars",
    "Issuer": "mobileexpensemanagement",
    "Audience": "mobileexpensemanagement-api",
    "ExpirationInMinutes": 60
  }
}
```

#### Azure Blob Storage
```json
{
  "ConnectionStrings": {
    "BlobStorageConnection": "DefaultEndpointsProtocol=https;AccountName=youraccountname;AccountKey=..."
  }
}
```

#### RabbitMQ
```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "Username": "guest",
    "Password": "guest"
  }
}
```

### environment.json Files

Create environment-specific files:
- `appsettings.Development.json` - Development settings
- `appsettings.Staging.json` - Staging settings  
- `appsettings.Production.json` - Production settings

## 📡 API Endpoints

### REST API Endpoints

#### Base URL: `https://localhost:44301/api/expenses`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/{expenseId}` | Get expense by ID |
| GET | `/trip/{tripId}` | Get all expenses for a trip |
| GET | `/trip/{tripId}/paginated?pageNumber=1&pageSize=10` | Get paginated expenses |
| GET | `/trip/{tripId}/summary` | Get trip expense summary |
| GET | `/search?startDate=...&endDate=...` | Search by date range |
| GET | `/statistics?startDate=...&endDate=...` | Get expense statistics |
| POST | `/` | Create new expense |
| PUT | `/{expenseId}` | Update expense |
| DELETE | `/{expenseId}` | Delete expense |

### Request Headers
All requests require:
```
X-User-Id: <employee-id>
Authorization: Bearer <jwt-token>
Content-Type: application/json
```

### Example: Create Expense

**Request:**
```bash
curl -X POST https://localhost:44301/api/expenses \
  -H "Content-Type: application/json" \
  -H "X-User-Id: 101" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "tripId": 1001,
    "categoryId": 1,
    "expenseDate": "2024-03-01",
    "comment": "Airfare to Mumbai",
    "amount": 5500,
    "currencyId": 1
  }'
```

**Response:**
```json
{
  "id": 1000,
  "tripId": 1001,
  "categoryId": 1,
  "expenseDate": "2024-03-01T00:00:00",
  "comment": "Airfare to Mumbai",
  "amount": 5500,
  "currencyId": 1,
  "enteredBy": 101,
  "enteredOn": "2024-03-15T10:30:00",
  "files": []
}
```

## 📊 GraphQL API

### Endpoint: `https://localhost:44301/graphql`

### Interactive Tool: Banana Cake Pop
Access GraphQL IDE: `https://localhost:44301/graphql` (auto-loaded in development)

### Example Query

```graphql
query {
  getExpenseById(expenseId: 1000) {
    id
    tripId
    comment
    amount
    enteredOn
    files {
      id
      fileName
      contentType
    }
  }
  
  getTripSummary(tripId: 1001) {
    tripId
    totalExpenseAmount
    expenseCount
    isApproved
    expenses {
      id
      amount
      comment
    }
  }
  
  getExpenseStatistics(startDate: "2024-01-01", endDate: "2024-03-31") {
    totalExpenses
    averageExpense
    expenseCount
  }
}
```

### Example Mutation

```graphql
mutation {
  createExpense(
    tripId: 1001,
    categoryId: 1,
    expenseDate: "2024-03-01",
    comment: "Travel expense",
    amount: 5500,
    currencyId: 1,
    enteredBy: 101
  ) {
    id
    amount
    comment
    enteredOn
  }
}
```

## 🔐 Authentication & Authorization

### JWT Token Generation

In your client application:

```csharp
var tokenHandler = new JwtSecurityTokenHandler();
var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);

var tokenDescriptor = new SecurityTokenDescriptor
{
    Subject = new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Email, email),
        new Claim("EmployeeId", employeeId.ToString())
    }),
    Expires = DateTime.UtcNow.AddHours(1),
    Issuer = configuration["Jwt:Issuer"],
    Audience = configuration["Jwt:Audience"],
    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
};

var token = tokenHandler.CreateToken(tokenDescriptor);
var tokenString = tokenHandler.WriteToken(token);
```

### Using Token in Requests

```bash
curl -H "Authorization: Bearer {token}" https://localhost:44301/api/expenses
```

### Authorization Claims

```csharp
// Access user claims in controller
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
var employeeId = User.FindFirst("EmployeeId")?.Value;
```

## 💾 Database

### Schema

#### MOBEXP_DET (Mobile Expense Details)
| Column | Type | Notes |
|--------|------|-------|
| MOBEXP_ID | decimal(38) | Primary Key, Identity sequence |
| MOBEXP_TPID | decimal(38) | Trip/Project ID (Foreign Key) |
| MOBEXP_CATID | decimal(38) | Category ID |
| MOBEXP_DATE | datetime2(3) | Expense date |
| MOBEXP_COMMENT | varchar(500) | Description |
| MOBEXP_AMOUNT | decimal(19,2) | Amount |
| MOBEXP_CURRID | decimal(38) | Currency ID |
| MOBEXP_ENTEREDBY | decimal(38) | Employee ID |
| MOBEXP_ENTEREDON | datetime2(3) | Created timestamp |
| MOBEXP_ISDELETED | bit | Soft delete flag |

#### MOBEXP_FILE (Expense Files)
| Column | Type | Notes |
|--------|------|-------|
| MOBEXPPHT_ID | decimal(38) | Primary Key |
| MOBEXPPHT_EXPID | decimal(38) | Expense ID (Foreign Key) |
| MOBEXPPHT_FILENAME | varchar(500) | File name |
| MOBEXPPHT_FILEDATA | nvarchar(max) | Base64 or blob path |
| MOBEXPPHT_BLOBPATH | varchar(500) | Azure Blob Storage path |
| MOBEXPPHT_ISDELETED | bit | Soft delete flag |

### Sequences
- `seq_MOBEXP_Id` - Generates expense IDs
- `seq_MOBEXP_File_Id` - Generates file IDs

### Entity Framework Migrations

```bash
# Add migration (after model changes)
dotnet ef migrations add "MigrationName" --project ./MobileExpenseManagement.Infrastructure --startup-project ./MobileExpenseManagement.API

# Apply migrations
dotnet ef database update --project ./MobileExpenseManagement.Infrastructure --startup-project ./MobileExpenseManagement.API

# Remove last migration (if not applied)
dotnet ef migrations remove --project ./MobileExpenseManagement.Infrastructure --startup-project ./MobileExpenseManagement.API

# Generate SQL script
dotnet ef migrations script --project ./MobileExpenseManagement.Infrastructure --startup-project ./MobileExpenseManagement.API --output migration.sql
```

## ☁️ Azure Functions

### Configured Functions

#### 1. ProcessExpenseFile
- **Trigger**: HTTP POST
- **Route**: `/expenses/files/process`
- **Purpose**: Validate and process uploaded expense files
- **Features**: File scanning, optimization, format conversion

#### 2. GenerateExpenseReport
- **Trigger**: Timer (monthly: 0 0 1 * * *)
- **Purpose**: Generate monthly expense reports and statistics
- **Output**: Report files in Blob Storage

#### 3. CleanupOldFiles
- **Trigger**: Timer (daily: 0 0 2 * * *)
- **Purpose**: Delete files older than 90 days
- **Usage**: Comply with retention policies

### Local Testing

```bash
cd MobileExpenseManagement.AzureFunctions

# Install Azure Functions Core Tools
choco install azure-functions-core-tools-4 -y

# Run functions locally
func start

# Test HTTP trigger
curl -X POST http://localhost:7071/api/ProcessExpenseFile

# View logs
# Check console output
```

### Deploy to Azure

```bash
# Login to Azure
az login
az account set --subscription "<subscription-id>"

# Create Function App
az functionapp create --name "expense-functions" \
  --storage-account "storageaccount" \
  --resource-group "rg-sparsh" \
  --runtime dotnet \
  --runtime-version 8

# Deploy
func azure functionapp publish expense-functions
```

## 🔨 Building & Deployment

### Build Solution

**Using Visual Studio:**
1. Open `MobileExpenseManagement.sln`
2. Build → Build Solution (Ctrl+Shift+B)

**Using Command Line:**
```bash
dotnet build MobileExpenseManagement.sln

# Build specific configuration
dotnet build -c Release
```

### Run Locally

**Using Visual Studio:**
1. Set `MobileExpenseManagement.API` as startup project
2. Press F5 or Debug → Start Debugging
3. API launches at `https://localhost:44301`

**Using .NET CLI:**
```bash
cd MobileExpenseManagement.API
dotnet run
```

### Docker Deployment

Create `Dockerfile`:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet build "MobileExpenseManagement.API/MobileExpenseManagement.API.csproj" -c Release

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /src/MobileExpenseManagement.API/bin/Release/net8.0/publish .
EXPOSE 80 443
ENTRYPOINT ["dotnet", "MobileExpenseManagement.API.dll"]
```

Build and run:
```bash
docker build -t mobileexpense:latest .
docker run -p 5000:80 -e ConnectionStrings__DefaultConnection="..." mobileexpense:latest
```

### Publish to Azure App Service

```bash
# Create deployment package
dotnet publish -c Release -o ./publish

# Create App Service
az appservice plan create --name "plan-expense" \
  --resource-group "rg-sparsh" \
  --sku B1 --is-linux

az webapp create --name "mobileexpense-api" \
  --resource-group "rg-sparsh" \
  --plan "plan-expense" \
  --runtime "DOTNETCORE:8.0"

# Deploy
cd publish
az webapp deployment source config-zip --resource-group "rg-sparsh" \
  --name "mobileexpense-api" \
  --src-path ..\publish.zip
```

## 🏥 Health Checks

### Health Check Endpoint

```
GET /health
```

**Response (Healthy):**
```json
{
  "status": "Healthy",
  "checks": {
    "SQL Server": {
      "status": "Healthy",
      "description": "Connected"
    }
  }
}
```

### Monitor Health in Application Insights

```csharp
// In Program.cs, health checks are automatically monitored
app.MapHealthChecks("/health");
```

### Custom Health Check

```csharp
services.AddHealthChecks()
    .AddCheck("Database", async () =>
    {
        try
        {
            // Test database connection
            await dbContext.Database.ExecuteSqlRawAsync("SELECT 1");
            return HealthCheckResult.Healthy();
        }
        catch
        {
            return HealthCheckResult.Unhealthy();
        }
    });
```

## 🆘 Troubleshooting

### Database Connection Issues

**Error**: "Cannot connect to SQL Server"
- Verify connection string in `appsettings.json`
- Ensure LocalDB service is running: `sqllocaldb start mssqllocaldb`
- Check firewall rules for remote databases

### JWT Token Errors

**Error**: "Invalid token", "Token expired"
- Verify secret key matches between token generation and validation
- Check token expiration time
- Ensure Authorization header format: `Bearer <token>`

### RabbitMQ Connection

**Error**: "Cannot connect to RabbitMQ"
- Verify RabbitMQ is running (port 5672)
- Check credentials in appsettings.json
- Windows: `rabbitmq-service.bat install` and `rabbitmq-service.bat start`

### EF Core Migrations

**Error**: "No migrations have been applied"
```bash
# Update database with pending migrations
dotnet ef database update --project ./MobileExpenseManagement.Infrastructure --startup-project ./MobileExpenseManagement.API
```

**Error**: "Cannot find initialization migration"
```bash
# Create initial migration
dotnet ef migrations add InitialCreate --project ./MobileExpenseManagement.Infrastructure --startup-project ./MobileExpenseManagement.API
```

### Azure Functions Debugging

```bash
# Enable debug mode
func start --verbose

# Attach debugger in VS Code
# Set breakpoints and press F5
```

## 📚 Additional Resources

- [Microsoft .NET Documentation](https://docs.microsoft.com/dotnet)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [HotChocolate GraphQL](https://chillicream.com/docs/hotchocolate)
- [Azure Blob Storage](https://docs.microsoft.com/azure/storage/blobs)
- [RabbitMQ Documentation](https://www.rabbitmq.com/documentation.html)
- [Polly Circuit Breaker](https://github.com/App-vNext/Polly)

## 📄 License

This project is part of the Sparsh ERP system. All rights reserved.

## 👥 Support

For issues, questions, or contributions, contact the Sparsh development team at support@sparsh.com
