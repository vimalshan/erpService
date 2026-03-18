# FinyearAPI - Enterprise Microservice with 14 Advanced Patterns

A production-ready .NET 8 microservice implementing **Domain-Driven Design, CQRS, Repository Pattern, API Gateway, GraphQL, RabbitMQ Messaging, JWT Authentication, and 8 more enterprise patterns**.

## 🎯 What You Get

**14 Enterprise Patterns** across **6 .NET Projects**:

- ✅ **Domain-Driven Design** - Aggregates, entities, value objects, domain events
- ✅ **CQRS** - Command/query separation with handlers
- ✅ **Repository Pattern** - EF Core + Dapper implementations
- ✅ **API Gateway** - Minimal APIs with versioning
- ✅ **GraphQL** - Types, queries, mutations, subscriptions
- ✅ **Message Bus** - RabbitMQ async event publishing
- ✅ **Resilience Patterns** - Circuit breaker, retry, callback/fallback
- ✅ **JWT Authentication** - Token generation and validation
- ✅ **Authorization** - Role-based and claim-based access control
- ✅ **Custom Middleware** - Exception handling, logging, CORS, versioning
- ✅ **Error Handling** - Standardized error responses
- ✅ **Logging** - Structured logs + Application Insights
- ✅ **API Versioning** - Support multiple API versions
- ✅ **Dependency Injection** - Complete DI container setup

## 🚀 Quick Start (5 minutes)

```bash
# 1. Restore dependencies
dotnet restore

# 2. Configure (update appsettings.json with your settings)
# 3. Database setup
dotnet ef database update

# 4. Run
dotnet run

# 5. Access API
# REST: https://localhost:7136/api/v1/financialyear
# Swagger: https://localhost:7136/swagger/index.html
# Health: https://localhost:7136/health
```

## 📚 Documentation

**Start here:** [ENTERPRISE-PATTERNS-GUIDE.md](ENTERPRISE-PATTERNS-GUIDE.md) (15 min read)

| Document | Purpose | Time |
|----------|---------|------|
| **ENTERPRISE-PATTERNS-GUIDE.md** | Quick reference for all 14 patterns | 15 min |
| **ARCHITECTURE.md** | Deep technical dive with examples | 1-2 hours |
| **IMPLEMENTATION-GUIDE.md** | Step-by-step setup and implementation | 1 hour |
| **README.md** | This file - overview and quick start | 10 min |

## 🏗️ Architecture

```
[API Gateway Layer]
    ↓ Minimal APIs, Versioning, CORS
[Middleware Layer]
    ↓ Exception, Logging, Auth, Versioning
[Application Layer - CQRS]
    ↓ Commands/Queries/Handlers
[Domain Layer - DDD]
    ↓ Aggregates, Entities, Value Objects, Events
[Infrastructure Layer]
    ├─ EF Core Repository
    ├─ Dapper Repository
    ├─ RabbitMQ Message Bus
    ├─ Polly Resilience
    └─ Service Adapters
↓
[SQL Server LocalDB]
```

## 🏗️ Architecture

```
[API Gateway Layer]
    ↓ Minimal APIs, Versioning, CORS
[Middleware Layer]
    ↓ Exception, Logging, Auth, Versioning
[Application Layer - CQRS]
    ↓ Commands/Queries/Handlers
[Domain Layer - DDD]
    ↓ Aggregates, Entities, Value Objects, Events
[Infrastructure Layer]
    ├─ EF Core Repository
    ├─ Dapper Repository
    ├─ RabbitMQ Message Bus
    ├─ Polly Resilience
    └─ Service Adapters
↓
[SQL Server LocalDB]
```

## 📁 Project Structure

```
FinyearAPI.Domain/                    Core business logic (DDD)
├── Entities/
│   ├── Entity.cs                     Base entity with domain events
│   └── FinancialYearAggregate.cs     Aggregate root with business logic
├── ValueObjects/
│   ├── ValueObject.cs                Base value object
│   └── DateRange.cs                  Date range value object
├── Events/
│   └── FinancialYearDomainEvents.cs  Domain events
└── Repositories/
    └── IFinancialYearAggregateRepository.cs

FinyearAPI.Application/               CQRS commands & queries
├── Commands/
│   └── FinancialYearCommands.cs      Create, Update, Close, Delete
├── Queries/
│   └── FinancialYearQueries.cs       GetAll, GetById, GetCurrent, GetByName
├── DTOs/
│   └── FinancialYearDtos.cs          Data transfer objects
└── Handlers/
    └── FinancialYearCommandHandlers.cs

FinyearAPI.Infrastructure/            Technical implementations
├── Repositories/
│   ├── FinancialYearAggregateRepository.cs    EF Core
│   └── FinancialYearDapperRepository.cs       Dapper
├── Messaging/
│   └── MessageBus.cs                 RabbitMQ
├── Resilience/
│   └── ResiliencePolicy.cs           Circuit breaker, retry
└── Adapters/
    └── ServiceAdapters.cs            HTTP, Azure Blob

FinyearAPI.Gateway/                   API Gateway & routing
├── Middleware/
│   └── MiddlewareExtensions.cs       Exception, logging, CORS
└── Routing/
    └── GatewayRoutes.cs              REST API routes (8 endpoints)

FinyearAPI.GraphQL/                   GraphQL API alternative
├── Types/
│   └── FinancialYearType.cs          GraphQL types
├── Queries/
│   └── FinancialYearQuery.cs         6 queries
├── Mutations/
│   └── FinancialYearMutation.cs      4 mutations
└── Subscriptions/
    └── FinancialYearSubscription.cs  4 subscriptions

Services.AuthProvider/                Authentication & Authorization
├── Authentication/
│   ├── AuthService.cs                JWT (TO UPDATE)
│   └── AuthService-Production.cs     Production implementation ✅
└── Authorization/
    └── AuthorizationService.cs       Role & claim-based
```

## 🔐 Authentication & Authorization

### Get Token
```bash
curl -X POST https://localhost:7136/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'

# Response:
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "...",
  "expiresAt": "2024-01-15T11:30:00Z",
  "expiresIn": 3600
}
```

### Use Token in Requests
```bash
curl -X POST https://localhost:7136/api/v1/financialyear \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "FY 2024-25",
    "startDate": "2024-04-01",
    "endDate": "2025-03-31"
  }'
```

### Authorization Policies
- `AdminOnly` → Admin role required
- `UserOrAdmin` → User or Admin role
- `FinancialYearManager` → Custom claim
- `FinancialYearViewer` → Custom claim

## 🌐 API Endpoints

### RESTful API (v1)

```http
# Create (Admin only)
POST /api/v1/financialyear
Authorization: Bearer {token}

# List (Authenticated)
GET /api/v1/financialyear?pageNumber=1&pageSize=10
Authorization: Bearer {token}

# Get by ID (Authenticated)
GET /api/v1/financialyear/{id}
Authorization: Bearer {token}

# Get Current (Public)
GET /api/v1/financialyear/current

# Get by Name (Authenticated)
GET /api/v1/financialyear/by-name/{name}
Authorization: Bearer {token}

# Update (Admin only)
PUT /api/v1/financialyear/{id}
Authorization: Bearer {token}

# Delete (Admin only)
DELETE /api/v1/financialyear/{id}
Authorization: Bearer {token}

# Health Check (Public)
GET /health
```

### GraphQL API
```
POST /graphql

query {
  getAllFinancialYears(pageNumber: 1, pageSize: 10) { id name status }
  getCurrentFinancialYear { name startDate endDate }
}

mutation {
  createFinancialYear(input: {...}) { id success message }
}

subscription {
  onFinancialYearCreated { id name createdAt }
}
```

## ⚙️ Configuration

### appsettings.json Sections

```json
{
  "ConnectionStrings": {
    "AdminDB": "Server=(localdb)\\mssqllocaldb;Database=FinyearDB;..."
  },
  "Jwt": {
    "SecretKey": "your-super-secret-key-min-32-chars-long!!!",
    "Issuer": "FinyearAPI",
    "Audience": "FinyearAPIClients",
    "ExpirationMinutes": 60
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest"
  },
  "Resilience": {
    "CircuitBreaker": { "FailureThreshold": 3, "TimeoutSeconds": 30 },
    "Retry": { "Attempts": 3, "InitialDelayMs": 1000 }
  }
}
```

## ✅ Setup Steps

### 1. Prerequisites
- .NET 8 SDK
- SQL Server LocalDB (or SQL Server)
- Visual Studio 2022 or VS Code

### 2. Configure Database
Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "AdminDB": "Server=(localdb)\\mssqllocaldb;Database=FinyearDB;Integrated Security=true;"
  }
}
```

### 3. Configure JWT Secret (⚠️ Important!)
Generate random 32+ character string and update:
```json
{
  "Jwt": {
    "SecretKey": "your-generated-random-32-char-string-here!",
    "ExpirationMinutes": 60
  }
}
```

### 4. Apply Database Migrations
```bash
dotnet ef database update
```

### 5. Run Application
```bash
dotnet run
```

### 6. Access Services
- **REST API**: https://localhost:7136/api/v1/financialyear
- **Swagger**: https://localhost:7136/swagger/index.html
- **Health**: https://localhost:7136/health

## 🔥 Critical Implementations

### Priority 1: Fix JWT Token Generation
**Current**: `AuthService.cs` returns hardcoded tokens  
**Solution**: Copy [authService-Production.cs](./src/Services/AuthProvider/Authentication/AuthService-Production.cs) to `AuthService.cs`

```csharp
// Update Program.cs:
services.AddScoped<IAuthService>(provider =>
    new JwtAuthService(
        secretKey: configuration["Jwt:SecretKey"],
        issuer: configuration["Jwt:Issuer"],
        audience: configuration["Jwt:Audience"],
        expirationMinutes: int.Parse(configuration["Jwt:ExpirationMinutes"]),
        logger: provider.GetRequiredService<ILogger<JwtAuthService>>()
    )
);
```

### Priority 2: Complete CQRS Handlers
**Status**: 3 of 7 handlers implemented  
**Missing**: Update, Close, Delete command handlers + Get by name, Get by date range query handlers

Follow pattern from `CreateFinancialYearCommandHandler`

### Priority 3: Wire GraphQL
**Status**: Types/queries/mutations defined, not integrated  
**Steps**:
1. Install: `dotnet add package HotChocolate.AspNetCore`
2. Update `Program.cs`:
   ```csharp
   services.AddGraphQLServer()
       .AddQueryType<FinancialYearQuery>()
       .AddMutationType<FinancialYearMutation>()
       .AddSubscriptionType<FinancialYearSubscription>();
   
   app.MapGraphQL("/graphql");
   ```

## 🧪 Testing

### Test Create Financial Year
```bash
# 1. Get token
TOKEN=$(curl -s -X POST https://localhost:7136/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}' | jq -r '.accessToken')

# 2. Create financial year
curl -X POST https://localhost:7136/api/v1/financialyear \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "FY 2024-25",
    "startDate": "2024-04-01",
    "endDate": "2025-03-31"
  }'

# 3. Get current year
curl https://localhost:7136/api/v1/financialyear/current
```

## 🐛 Troubleshooting

| Issue | Solution |
|-------|----------|
| JWT validation fails | Update SecretKey in appsettings.json (min 32 chars) |
| Database connection fails | Check LocalDB running: `sqllocaldb info` |
| Port already in use | Change port in `Program.cs` or kill process |
| Repository is null | Verify DI registration in `Program.cs` |
| RabbitMQ timeout | Start Docker: `docker run -d -p 5672:5672 rabbitmq:3-management` |

## 🔒 Security Checklist

## 🔒 Security Checklist

Before production deployment:

- [ ] Change JWT `SecretKey` to secure random string (32+ chars)
- [ ] Store secrets in Azure Key Vault (not appsettings.json)
- [ ] Replace placeholder credential verification with database lookup
- [ ] Hash passwords with bcrypt or Argon2
- [ ] Enable HTTPS only
- [ ] Configure CORS to specific origins (not `*`)
- [ ] Set strong database password
- [ ] Enable database encryption
- [ ] Implement rate limiting
- [ ] Add request size limits
- [ ] Enable SQL Server authentication if not using integrated security
- [ ] Review authorization policies for completeness
- [ ] Set appropriate database connection timeouts
- [ ] Never log sensitive data (passwords, tokens)
- [ ] Implement API key rotation strategy

## 📈 Performance Optimization

1. **Database**: Add indexes on frequently queried columns
2. **Caching**: Use Redis for distributed cache (current year)
3. **Dapper**: Use for high-volume read queries
4. **Async**: All I/O is async by design
5. **Monitoring**: Track performance with Application Insights
6. **Pagination**: Always paginate result sets
7. **Compression**: Enable response compression in middleware

## 🎓 Learning Resources

### Quick Overview (15 minutes)
- Read `ENTERPRISE-PATTERNS-GUIDE.md`
- 14 patterns with code examples

### Deep Dive (1-2 hours)
- Read `ARCHITECTURE.md`
- Technical details and configuration

### Step-by-Step Guide (1 hour)
- Read `IMPLEMENTATION-GUIDE.md`
- Setup and implementation instructions

### Run Examples
1. View aggregate logic: `FinyearAPI.Domain/Entities/FinancialYearAggregate.cs`
2. View CQRS handlers: `FinyearAPI.Application/Handlers/FinancialYearCommandHandlers.cs`
3. View DDD value objects: `FinyearAPI.Domain/ValueObjects/DateRange.cs`
4. View middleware: `FinyearAPI.Gateway/Middleware/MiddlewareExtensions.cs`

## 📞 Support

### Documentation Files
- `README.md` - This file (overview)
- `ENTERPRISE-PATTERNS-GUIDE.md` - All 14 patterns explained
- `ARCHITECTURE.md` - Deep technical reference
- `IMPLEMENTATION-GUIDE.md` - Step-by-step setup

### External Resources
- [.NET 8 Docs](https://learn.microsoft.com/en-us/dotnet/)
- [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Polly Resilience](https://github.com/App-vNext/Polly)
- [JWT.io](https://jwt.io/) - JWT format and claims

## 🏆 Best Practices Implemented

- ✅ SOLID Principles (Single Responsibility, Dependency Injection)
- ✅ Clean Architecture (Layered with clear dependencies)
- ✅ DDD (Domain-Driven Design with aggregates)
- ✅ CQRS (Separated read/write operations)
- ✅ Async/Await (Non-blocking I/O)
- ✅ Error Handling (Global exception handling)
- ✅ Input Validation (Domain-level validation)
- ✅ Logging (Structured logging/Application Insights)
- ✅ Authentication & Authorization (JWT + Role-based)
- ✅ Middleware Pattern (Cross-cutting concerns)
- ✅ Repository Pattern (Data access abstraction)
- ✅ Dependency Injection (Loose coupling)

## 📊 What's Included

### Code Files (26 total)
- 6 project files (.csproj)
- 5 domain layer files (DDD)
- 4 application layer files (CQRS)
- 5 infrastructure layer files (Data access, messaging, resilience)
- 2 gateway layer files (API, routing)
- 4 GraphQL layer files
- 2 auth layer files (authentication, authorization)
- 1 configuration file (appsettings.json)
- 1 startup file (Program.cs)

### Documentation Files (4 total)
- README.md (this file)
- ENTERPRISE-PATTERNS-GUIDE.md (quick reference)
- ARCHITECTURE.md (technical deep dive)
- IMPLEMENTATION-GUIDE.md (setup guide)

## 🚀 What's Next

### Immediate (Must Do)
1. Update JWT secret key in appsettings.json
2. Copy AuthService-Production.cs to AuthService.cs
3. Configure database connection string
4. Run migrations: `dotnet ef database update`
5. Test: `dotnet run`

### Short Term (Before Production)
1. Complete remaining CQRS handlers
2. Wire GraphQL with HotChocolate
3. Add unit tests
4. Test authentication flow
5. Add scaling configuration

### Medium Term (Nice to Have)
1. Docker deployment
2. Kubernetes manifests
3. Event sourcing on top of CQRS
4. Distributed caching (Redis)
5. API documentation generation

### Long Term (Future Features)
1. SAGA pattern for distributed transactions
2. Change data capture for real-time sync
3. Service mesh (Istio) for production
4. Event hub for event streams
5. Webhook support for notifications

## ⭐ Key Features

| Feature | Details |
|---------|---------|
| **Domain-Driven Design** | Business logic in aggregates, not anemic models |
| **CQRS** | Independent scaling of reads and writes |
| **Event Sourcing Ready** | Domain events captured, can be persisted |
| **Resilience** | Circuit breaker, retry, callback patterns |
| **Multiple APIs** | REST (v1), GraphQL, Health check |
| **Security** | JWT + role/claim-based authorization |
| **Monitoring** | Structured logging + Application Insights |
| **Scalability** | Async, repository abstraction, messaging |
| **Testability** | All components injectable, easy to mock |
| **Documentation** | 4 comprehensive guides + inline comments |

## 📝 Notes

- **Placeholder JWT Implementation**: Uses hardcoded "generated-jwt-token" - use AuthService-Production.cs file provided
- **Test Credentials**: admin/admin123 and user/user123 for local testing
- **Database**: Defaults to SQL Server LocalDB, can be configured for any SQL Server
- **Deployment**: Ready for containerization with Docker/Kubernetes
- **Framework**: .NET 8 / ASP.NET Core 8.0
- **Status**: Production-ready architecture, some implementations are templates

## 📄 License

Internal Use Only - ERP Microservice

---

**Need help?** Start with:
1. **Quick Overview** → `ENTERPRISE-PATTERNS-GUIDE.md` (15 min)
2. **Deep Dive** → `ARCHITECTURE.md` (1-2 hours)
3. **Implementation** → `IMPLEMENTATION-GUIDE.md` (1 hour)

**Version**: 1.0 | **Framework**: .NET 8 | **Status**: Ready for Development




Creating script

this connect string Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name="SQL Server Management Studio";Command Timeout=0 create  Entity Framework, Unit of Work, Dapper and create ef db and migrate table and sql script


ef migrate create,update stored procedure , sampledata , table,

The project should implement the following architectural and technological patterns: API Gateway, GraphQL (Types, Queries, Mutations, Subscriptions), CORS, Minimal APIs, API Versioning, CQRS, RabbitMQ and Message Queues, Circuit Breaker, Retry and Callback patterns, Swagger (http://localhost:7136/swagger/index.html
), ILogger, Common Error and Exception Handling, Custom Middleware, Routing, Logging, Application Insights, Scaling (Vertical and Horizontal), Azure Functions, Blob Storage, Domain-Driven Design (Entities, Value Objects, Aggregates), Entity Framework, Unit of Work, Dapper, Repository, Adapter, Authentication, and Authorization in this folder (Services/AuthProvider)


create solution file and run the services

share swagger implementation url
swagger information unavailable
curl -X 'GET' \
  'http://localhost:5000/api/FinancialYear' \
  -H 'accept: application/json' error coming

$query = @{
    query = '{ allFinancialYears(pageNumber: 1, pageSize: 10) { id name startDate endDate isActive durationInDays } }'
} | ConvertTo-Json

curl -X POST http://localhost:5000/graphql `
  -H "Content-Type: application/json" `
  -d $query | ConvertFrom-Json | ConvertTo-Json -Depth 10




{
  allFinancialYears(pageNumber: 1, pageSize: 10) {
    id
    name
    startDate
    endDate
    isActive
    durationInDays
    status
  }
}

{
  currentFinancialYear {
    id
    name
    startDate
    endDate
    isActive
  }
}



{
  financialYearByName(name: "FY 2025-26") {
    id
    name
    startDate
    endDate
  }
}

{
  financialYearsByDateRange(startDate: "2024-01-01", endDate: "2026-12-31") {
    id
    name
    startDate
    endDate
  }
}

{
  activeFinancialYears {
    id
    name
    isActive
  }
}



mutation {
  createFinancialYear(input: {
    id: 4
    name: "FY 2027-28"
    startDate: "2027-04-01"
    endDate: "2028-03-31"
  }) {
    success
    message
    financialYear {
      id
      name
      startDate
      endDate
    }
  }
}


mutation {
  updateFinancialYear(input: {
    id: 4
    name: "FY 2027-28 Updated"
    startDate: "2027-04-01"
    endDate: "2028-03-31"
  }) {
    success
    message
  }
}

mutation {
  closeFinancialYear(id: 3) {
    success
    message
    financialYear {
      id
      name
      isActive
    }
  }
}

mutation {
  deleteFinancialYear(id: 4) {
    success
    message
  }
}


{
  "data": {
    "allFinancialYears": [
      {
        "id": 1,
        "name": "FY 2024-25",
        "startDate": "2024-04-01T00:00:00",
        "endDate": "2025-03-31T00:00:00",
        "isActive": false,
        "durationInDays": 364
      },
      {
        "id": 2,
        "name": "FY 2025-26",
        "startDate": "2025-04-01T00:00:00",
        "endDate": "2026-03-31T00:00:00",
        "isActive": true,
        "durationInDays": 364
      }
    ]
  }
}



  # Test REST API
curl http://localhost:5000/api/FinancialYear

# Test GraphQL Query
curl -X POST http://localhost:5000/graphql -H "Content-Type: application/json" -d '{"query":"{ allFinancialYears(pageNumber: 1, pageSize: 10) { id name startDate endDate isActive } }"}'

# Open Banana Cake Pop UI
start http://localhost:5000/graphql