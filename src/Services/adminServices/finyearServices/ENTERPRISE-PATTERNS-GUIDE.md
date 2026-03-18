# FinyearAPI Enterprise Architecture Guide

## Quick Reference

### 14 Enterprise Patterns Implemented

| # | Pattern | File | Purpose |
|---|---------|------|---------|
| 1 | DDD | `Domain/Entities/`, `Domain/ValueObjects/` | Core business logic in domain layer |
| 2 | CQRS | `Application/Commands/`, `Application/Queries/` | Separate read/write operations |
| 3 | Repository | `Infrastructure/Repositories/` | Abstract data access |
| 4 | API Gateway | `Gateway/Routing/GatewayRoutes.cs` | Single entry point for APIs |
| 5 | API Versioning | `Gateway/Middleware/MiddlewareExtensions.cs` | Support multiple API versions |
| 6 | GraphQL | `GraphQL/` (4 files) | Modern query language API |
| 7 | Message Bus | `Infrastructure/Messaging/MessageBus.cs` | RabbitMQ async messaging |
| 8 | Resilience | `Infrastructure/Resilience/ResiliencePolicy.cs` | Circuit breaker, retry, callback |
| 9 | Callback/Fallback | `Infrastructure/Resilience/ResiliencePolicy.cs` | Handle failures gracefully |
| 10 | Middleware | `Gateway/Middleware/MiddlewareExtensions.cs` | Cross-cutting concerns |
| 11 | Authentication | `Services/AuthProvider/Authentication/AuthService.cs` | JWT token management |
| 12 | Authorization | `Services/AuthProvider/Authorization/AuthorizationService.cs` | Role/claim-based access |
| 13 | Error Handling | `Gateway/Middleware/MiddlewareExtensions.cs` | Standardized error responses |
| 14 | Logging | Throughout all layers | Structured logging + App Insights |

---

## Project Structure (6 Projects)

```
FinyearAPI/
├── FinyearAPI.Domain                  (Core DDD - business logic)
│   ├── Entities/
│   │   ├── Entity.cs                 (Base entity with domain events)
│   │   └── FinancialYearAggregate.cs (Aggregate root)
│   ├── ValueObjects/
│   │   ├── ValueObject.cs            (Base value object)
│   │   └── DateRange.cs              (Value object - date range)
│   ├── Events/
│   │   └── FinancialYearDomainEvents.cs (Domain events)
│   └── Repositories/
│       └── IFinancialYearAggregateRepository.cs (Repository interface)
│
├── FinyearAPI.Application            (CQRS - application logic)
│   ├── Commands/
│   │   └── FinancialYearCommands.cs  (4 commands + responses)
│   ├── Queries/
│   │   └── FinancialYearQueries.cs   (4 queries + DTOs)
│   ├── DTOs/
│   │   └── FinancialYearDtos.cs      (Data transfer objects)
│   └── Handlers/
│       └── FinancialYearCommandHandlers.cs (3 handlers implemented)
│
├── FinyearAPI.Infrastructure         (Technical implementations)
│   ├── Repositories/
│   │   ├── FinancialYearAggregateRepository.cs (EF Core)
│   │   └── FinancialYearDapperRepository.cs    (Dapper)
│   ├── Messaging/
│   │   └── MessageBus.cs             (RabbitMQ)
│   ├── Resilience/
│   │   └── ResiliencePolicy.cs       (Polly - Circuit Breaker, Retry)
│   └── Adapters/
│       └── ServiceAdapters.cs        (HTTP, Azure Blob)
│
├── FinyearAPI.Gateway                (API Gateway)
│   ├── Middleware/
│   │   └── MiddlewareExtensions.cs   (5 middleware components)
│   └── Routing/
│       └── GatewayRoutes.cs          (8 API routes + health)
│
├── FinyearAPI.GraphQL                (GraphQL API alternative)
│   ├── Types/
│   │   └── FinancialYearType.cs      (GraphQL types)
│   ├── Queries/
│   │   └── FinancialYearQuery.cs     (6 queries)
│   ├── Mutations/
│   │   └── FinancialYearMutation.cs  (4 mutations)
│   └── Subscriptions/
│       └── FinancialYearSubscription.cs (4 subscriptions)
│
└── Services.AuthProvider             (Authentication & Authorization)
    ├── Authentication/
    │   └── AuthService.cs            (JWT token generation/validation)
    └── Authorization/
        └── AuthorizationService.cs   (Role & claim-based authorization)
```

---

## Configuration Files

```
appsettings-Enhanced.json  (Complete configuration for all patterns)
├── ConnectionStrings      (SQL Server LocalDB, Azure Blob)
├── Jwt                    (Token settings)
├── RabbitMQ               (Message bus)
├── ApplicationInsights    (Monitoring)
├── Swagger                (API documentation)
├── ApiVersioning          (Version settings)
└── Resilience             (Circuit breaker, retry config)

Program-Enhanced.cs        (DI & middleware setup)
├── DbContext registration
├── Dapper registration
├── Service registrations (15+ services)
├── Authentication/Authorization setup
├── Middleware pipeline configuration
├── Database migration
└── GraphQL placeholder (to be integrated)
```

---

## API Endpoints (9 total)

### RESTful Endpoints (8)
```
GET    /api/v{version}/financialyear           [Authorize("UserOrAdmin")]
GET    /api/v{version}/financialyear/{id}      [Authorize("UserOrAdmin")]
GET    /api/v{version}/financialyear/current   [AllowAnonymous]
GET    /api/v{version}/financialyear/by-name/{name} [Authorize("UserOrAdmin")]
POST   /api/v{version}/financialyear           [Authorize("AdminOnly")]
PUT    /api/v{version}/financialyear/{id}      [Authorize("AdminOnly")]
DELETE /api/v{version}/financialyear/{id}      [Authorize("AdminOnly")]
GET    /health                                 [AllowAnonymous]
GET    /api/gateway/info                       [AllowAnonymous]
```

### GraphQL Endpoint (1)
```
POST   /graphql  (to be wired in Program.cs)
```

---

## CQRS Flow Example

### Create Financial Year (Command)
```
1. API Request
   POST /api/v1/financialyear
   { "name": "FY 2024-25", "startDate": "2024-04-01", "endDate": "2025-03-31" }

2. ↓ Handler Layer
   CreateFinancialYearCommandHandler
   ├─ Call repository.AddAsync()
   ├─ Publish domain events
   └─ Return CreateFinancialYearResponse

3. ↓ Domain Layer
   FinancialYearAggregate.Create()
   ├─ Validate business rules
   │  ├─ EndDate > StartDate
   │  └─ Unique name
   ├─ Create aggregate instance
   └─ Raise FinancialYearCreatedEvent

4. ↓ Data Layer
   Repository.AddAsync()
   ├─ Save aggregate to database
   └─ Return saved instance

5. ↑ Response
   { "id": 1, "message": "Created successfully" }
```

### Get Financial Year (Query)
```
1. API Request
   GET /api/v1/financialyear/1

2. ↓ Handler Layer
   GetFinancialYearByIdQueryHandler
   ├─ Call repository.GetByIdAsync(1)
   └─ Map to FinancialYearQueryDto

3. ↓ Data Layer
   Repository.GetByIdAsync(1)
   ├─ Execute query: SELECT * FROM FinancialYears WHERE Id = 1
   └─ Return FinancialYearAggregate

4. ↑ Response
   {
     "id": 1,
     "name": "FY 2024-25",
     "startDate": "2024-04-01T00:00:00Z",
     "endDate": "2025-03-31T00:00:00Z",
     "status": "Open"
   }
```

---

## Resilience Pattern Flow

### Circuit Breaker + Retry Example
```
Request to external service
│
├─ Try 1: Fails (Circuit Breaker: 1/3)
├─ Retry 1: Waits 1s, tries again
├─ Try 2: Fails (Circuit Breaker: 2/3)
├─ Retry 2: Waits 2s, tries again
├─ Try 3: Fails (Circuit Breaker: 3/3 → OPEN)
│
├─ Circuit Breaker OPENS
├─ Subsequent requests immediately rejected
├─ No more calls to external service
│
├─ Waits 30 seconds
│
├─ Circuit Breaker Half-Open
├─ Test call to service
├─ Service responds: Success
├─ Circuit Breaker CLOSES (back to normal)
│
└─ Subsequent requests go through
```

---

## Authentication Flow

```
1. User Login Request
   POST /auth/login
   { "username": "john", "password": "secret" }

2. AuthService.AuthenticateAsync()
   ├─ Verify credentials against database
   ├─ Create AuthUser object
   └─ Call GenerateToken()

3. JwtAuthService.GenerateToken()
   ├─ Create claims (sub, roles, email)
   ├─ Sign with secret key (HS256)
   ├─ Return JWT token (valid for 60 minutes)
   └─ Also return refresh token

4. Client receives
   {
     "accessToken": "eyJhbGciOiJIUzI1NiIs...",
     "refreshToken": "refresh-token-value",
     "expiresAt": "2024-01-15T11:30:00Z",
     "expiresIn": 3600
   }

5. Subsequent API Requests
   GET /api/v1/financialyear
   Authorization: Bearer eyJhbGciOiJIUzI1NiIs...

6. BearerAuthenticationHandler validates JWT
   ├─ Extract token from header
   ├─ Verify signature with secret key
   ├─ Check expiration
   └─ Create ClaimsPrincipal from claims

7. AuthorizationMiddleware checks roles/claims
   ├─ Get user roles from ClaimsPrincipal
   ├─ Match against policy requirements
   ├─ Allow or deny request
   └─ Return 403 Forbidden if denied

8. Handler executes with authenticated user context
```

---

## Authorization Policies

```csharp
// 4 policies defined
AdminOnly            // Requires "Admin" role
UserOrAdmin          // Requires "User" OR "Admin" role
FinancialYearManager // Custom claim-based
FinancialYearViewer  // Custom claim-based

// Applied to routes
[Authorize("AdminOnly")]           → Only admins
[Authorize("UserOrAdmin")]         → Users + admins
[AllowAnonymous]                   → Public access
```

---

## Key Files to Review

### Start Here (Understanding)
1. **[ARCHITECTURE.md](ARCHITECTURE.md)** - Comprehensive 14-pattern documentation
2. **[FinyearAPI.Domain/Entities/FinancialYearAggregate.cs](FinyearAPI.Domain/Entities/FinancialYearAggregate.cs)** - DDD aggregate example
3. **[FinyearAPI.Application/Commands/FinancialYearCommands.cs](FinyearAPI.Application/Commands/FinancialYearCommands.cs)** - CQRS command definitions
4. **[FinyearAPI.Gateway/Routing/GatewayRoutes.cs](FinyearAPI.Gateway/Routing/GatewayRoutes.cs)** - API routes with versioning

### Configuration (Setup)
5. **[appsettings-Enhanced.json](appsettings-Enhanced.json)** - All configuration sections
6. **[Program-Enhanced.cs](Program-Enhanced.cs)** - Dependency injection & middleware

### Domain Layer (Business)
7. **[FinyearAPI.Domain/Entities/Entity.cs](FinyearAPI.Domain/Entities/Entity.cs)** - Base entity
8. **[FinyearAPI.Domain/ValueObjects/DateRange.cs](FinyearAPI.Domain/ValueObjects/DateRange.cs)** - Value object
9. **[FinyearAPI.Domain/Events/FinancialYearDomainEvents.cs](FinyearAPI.Domain/Events/FinancialYearDomainEvents.cs)** - Domain events

### Application Layer (CQRS)
10. **[FinyearAPI.Application/Handlers/FinancialYearCommandHandlers.cs](FinyearAPI.Application/Handlers/FinancialYearCommandHandlers.cs)** - Command handlers
11. **[FinyearAPI.Application/DTOs/FinancialYearDtos.cs](FinyearAPI.Application/DTOs/FinancialYearDtos.cs)** - Data transfer objects

### Infrastructure Layer (Technical)
12. **[FinyearAPI.Infrastructure/Repositories/FinancialYearAggregateRepository.cs](FinyearAPI.Infrastructure/Repositories/FinancialYearAggregateRepository.cs)** - EF Core repository
13. **[FinyearAPI.Infrastructure/Repositories/FinancialYearDapperRepository.cs](FinyearAPI.Infrastructure/Repositories/FinancialYearDapperRepository.cs)** - Dapper repository
14. **[FinyearAPI.Infrastructure/Messaging/MessageBus.cs](FinyearAPI.Infrastructure/Messaging/MessageBus.cs)** - RabbitMQ integration
15. **[FinyearAPI.Infrastructure/Resilience/ResiliencePolicy.cs](FinyearAPI.Infrastructure/Resilience/ResiliencePolicy.cs)** - Polly patterns

### Security & Middleware
16. **[Services/AuthProvider/Authentication/AuthService.cs](Services/AuthProvider/Authentication/AuthService.cs)** - JWT authentication
17. **[Services/AuthProvider/Authorization/AuthorizationService.cs](Services/AuthProvider/Authorization/AuthorizationService.cs)** - Authorization
18. **[FinyearAPI.Gateway/Middleware/MiddlewareExtensions.cs](FinyearAPI.Gateway/Middleware/MiddlewareExtensions.cs)** - Custom middleware

### GraphQL (Alternative API)
19. **[FinyearAPI.GraphQL/Types/FinancialYearType.cs](FinyearAPI.GraphQL/Types/FinancialYearType.cs)** - GraphQL types
20. **[FinyearAPI.GraphQL/Queries/FinancialYearQuery.cs](FinyearAPI.GraphQL/Queries/FinancialYearQuery.cs)** - GraphQL queries
21. **[FinyearAPI.GraphQL/Mutations/FinancialYearMutation.cs](FinyearAPI.GraphQL/Mutations/FinancialYearMutation.cs)** - GraphQL mutations
22. **[FinyearAPI.GraphQL/Subscriptions/FinancialYearSubscription.cs](FinyearAPI.GraphQL/Subscriptions/FinancialYearSubscription.cs)** - Real-time subscriptions

---

## Implementation Status

| Component | Status | Details |
|-----------|--------|---------|
| DDD (Domain Layer) | ✅ Complete | Entities, value objects, aggregates, domain events |
| CQRS | ✅ Complete | Commands, queries, handlers (3/7 handlers implemented) |
| Repository Pattern | ✅ Complete | EF Core + Dapper implementations |
| API Gateway | ✅ Complete | 8 REST routes + health + info endpoints |
| API Versioning | ✅ Complete | Version extraction and routing |
| Custom Middleware | ✅ Complete | Exception, logging, versioning, CORS, auth |
| Authentication | ✅ Partial | Structure complete, JWT token generation needs work |
| Authorization | ✅ Complete | Role + claim-based with 4 policies |
| Message Bus | ✅ Partial | Interface complete, RabbitMQ.Client integration pending |
| Resilience | ✅ Complete | Circuit breaker, retry, callback patterns |
| Error Handling | ✅ Complete | Global exception handling with standardized responses |
| Logging | ✅ Complete | Structured logging + Application Insights integration |
| GraphQL | ✅ Partial | Types/queries/mutations/subscriptions defined, HotChocolate integration pending |
| Configuration | ✅ Complete | All settings in appsettings.json |
| DI & Startup | ✅ Complete | Program.cs fully configured |

---

## Next Steps (Priority Order)

1. **🔴 CRITICAL - JWT Implementation**
   - File: `Services/AuthProvider/Authentication/AuthService.cs`
   - Task: Replace hardcoded tokens with actual JWT generation using `System.IdentityModel.Tokens.Jwt`
   - Impact: Authentication won't work without this

2. **🔴 HIGH - Complete CQRS Handlers**
   - File: `FinyearAPI.Application/Handlers/FinancialYearCommandHandlers.cs`
   - Task: Implement Update, Close, Delete command handlers and Get by name, by date range query handlers
   - Impact: Only 3 of 7 handlers implemented

3. **🟡 MEDIUM - Wire GraphQL**
   - File: `Program-Enhanced.cs`
   - Task: Install HotChocolate, register schema, add subscriptions
   - Impact: Alternative API interface won't be available

4. **🟡 MEDIUM - RabbitMQ Integration**
   - File: `FinyearAPI.Infrastructure/Messaging/MessageBus.cs`
   - Task: Replace placeholder with actual RabbitMQ.Client implementation
   - Impact: Async messaging won't work

6. **🟢 LOW - Add Unit Tests**
   - Task: Create test project with xUnit
   - Impact: Code quality and confidence

---

## Common Commands

### Trust HTTPS Certificate (Development)
```powershell
dotnet dev-certs https --trust
```

### Apply Database Migrations
```bash
dotnet ef database update --project FinyearAPI.Infrastructure --startup-project FinyearAPI
```

### Run Application
```bash
dotnet run --project FinyearAPI
```

### Test API (PowerShell)
```powershell
# Get current financial year
$header = @{ "Authorization" = "Bearer {JWT_TOKEN}" }
Invoke-RestMethod -Uri "http://localhost:5000/api/v1/financialyear/current" -Headers $header

# Create new financial year
$body = @{
    name = "FY 2024-25"
    startDate = "2024-04-01"
    endDate = "2025-03-31"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/v1/financialyear" `
    -Method Post `
    -Headers @{ "Authorization" = "Bearer {JWT_TOKEN}" } `
    -Body $body `
    -ContentType "application/json"
```

### Run RabbitMQ (Docker)
```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

### Verify RabbitMQ
- Management UI: http://localhost:15672 (guest/guest)
- AMQP Port: localhost:5672

---

## Architecture Decisions

### Why EF Core + Dapper?
- **EF Core**: Complex queries, relationships, lazy loading
- **Dapper**: High-performance reads, dashboard queries, control

### Why Polly for Resilience?
- Industry standard .NET resilience library
- Composable policies (circuit breaker + retry)
- Built-in metrics and diagnostics

### Why RabbitMQ?
- Reliable message delivery with acknowledgments
- Fan-out and routing capabilities
- Battle-tested in production systems

### Why DDD?
- Domain logic is business logic (not technical)
- Aggregates enforce business rules at creation
- Domain events provide audit trail

### Why CQRS?
- Independent scaling of reads and writes
- Clear separation of concerns
- Easy to add event sourcing later

### Why GraphQL?
- Single endpoint, no versioning needed
- Client-specified response shape
- Built-in introspection and documentation

---

## Security Checklist

- [ ] Change JWT SecretKey in appsettings.json (minimum 32 characters, random)
- [ ] Store secrets in Azure Key Vault (not in code)
- [ ] Enable HTTPS in production
- [ ] Configure CORS to specific origins (not *)
- [ ] Use parameterized queries (both EF and Dapper do this)
- [ ] Never log passwords or sensitive data
- [ ] Implement rate limiting (future enhancement)
- [ ] Add request size limits to prevent DoS
- [ ] Enable SQL Server encryption
- [ ] Review authorization policies for completeness

---

**Architecture Version**: 1.0  
**Last Updated**: January 2024  
**Framework**: .NET 8 / ASP.NET Core 8.0  
**Team**: Enterprise Architecture
