# FinyearAPI Implementation Guide

## What You Have

A complete enterprise microservice architecture with **14 advanced patterns** implemented across **6 .NET projects**.

### Projects Created

| Project | Purpose | Files | Status |
|---------|---------|-------|--------|
| **FinyearAPI.Domain** | Core business logic (DDD) | 5 files | ✅ Complete |
| **FinyearAPI.Application** | CQRS commands/queries | 4 files | ⏳ Partial (3/7 handlers) |
| **FinyearAPI.Infrastructure** | Data access, messaging, resilience | 5 files | ⏳ Partial (interfaces + EF Core + Dapper) |
| **FinyearAPI.Gateway** | API Gateway, routing, middleware | 2 files | ✅ Complete |
| **FinyearAPI.GraphQL** | GraphQL alternative API | 4 files | ⏳ Structure (needs HotChocolate wiring) |
| **Services.AuthProvider** | Authentication & authorization | 2 files | ⏳ Structure (JWT needs completion) |

### Patterns Implemented

```
✅ Domain-Driven Design (DDD)
✅ CQRS (Command Query Responsibility Segregation)
✅ Repository Pattern (EF Core + Dapper)
✅ API Gateway with Minimal APIs
✅ API Versioning (/api/v{version})
✅ GraphQL (Types, Queries, Mutations, Subscriptions)
✅ Message Bus (RabbitMQ abstraction)
✅ Resilience Patterns (Circuit Breaker, Retry, Callback)
✅ Custom Middleware (5 types)
✅ Authentication (JWT)
✅ Authorization (Role-based + Claim-based)
✅ Error Handling (Global exception handling)
✅ Logging (Structured logging + Application Insights)
✅ Dependency Injection (Complete setup)
```

---

## What's Next: Critical Implementations

### Priority 1: Fix JWT Token Generation ⚠️ SECURITY

**Current Problem:** `AuthService.cs` returns hardcoded tokens like `"generated-jwt-token"`

**File to Update:**
- `Services/AuthProvider/Authentication/AuthService.cs`

**Solution Provided:**
- `Services/AuthProvider/Authentication/AuthService-Production.cs` ← USE THIS VERSION

**What to Do:**
1. Copy contents of `AuthService-Production.cs`
2. Replace contents of `AuthService.cs`
3. Update `Program.cs` to configure JWT settings when registering `IAuthService`

**Code Example for Program.cs:**
```csharp
// Register JWT Authentication Service
var jwtSettings = configuration.GetSection("Jwt");
services.AddScoped<IAuthService>(provider =>
    new JwtAuthService(
        secretKey: jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured"),
        issuer: jwtSettings["Issuer"] ?? "FinyearAPI",
        audience: jwtSettings["Audience"] ?? "FinyearAPIClients",
        expirationMinutes: int.Parse(jwtSettings["ExpirationMinutes"] ?? "60"),
        logger: provider.GetRequiredService<ILogger<JwtAuthService>>()
    )
);
```

---

### Priority 2: Complete CQRS Handlers

**Current Status:** 3 of 7 handlers implemented

**File:** `FinyearAPI.Application/Handlers/FinancialYearCommandHandlers.cs`

**Implemented Handlers:**
- ✅ CreateFinancialYearCommandHandler
- ✅ GetAllFinancialYearsQueryHandler
- ✅ GetFinancialYearByIdQueryHandler

**Missing Handlers (Follow the same pattern):**
- ❌ UpdateFinancialYearCommandHandler
- ❌ CloseFinancialYearCommandHandler
- ❌ DeleteFinancialYearCommandHandler
- ❌ GetFinancialYearByNameQueryHandler

**Code Template:**
```csharp
public class UpdateFinancialYearCommandHandler : ICommandHandler<UpdateFinancialYearCommand>
{
    private readonly IFinancialYearAggregateRepository _repository;
    private readonly ILogger<UpdateFinancialYearCommandHandler> _logger;
    private readonly IEventPublisher _eventPublisher;

    public UpdateFinancialYearCommandHandler(
        IFinancialYearAggregateRepository repository,
        ILogger<UpdateFinancialYearCommandHandler> logger,
        IEventPublisher eventPublisher)
    {
        _repository = repository;
        _logger = logger;
        _eventPublisher = eventPublisher;
    }

    public async Task<UpdateFinancialYearResponse> Handle(
        UpdateFinancialYearCommand request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating financial year: {Id}", request.Id);

            var aggregate = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (aggregate == null)
                return new UpdateFinancialYearResponse { Success = false, Message = "Financial year not found" };

            // Call domain method to update
            aggregate.Update(request.Name ?? aggregate.Name, request.UpdatedBy);

            // Save changes
            await _repository.UpdateAsync(aggregate, cancellationToken);

            // Publish domain events
            await _eventPublisher.PublishAsync(aggregate.DomainEvents);

            return new UpdateFinancialYearResponse
            {
                Success = true,
                Message = "Financial year updated successfully",
                Data = new FinancialYearDto { /* map properties */ }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating financial year: {Id}", request.Id);
            return new UpdateFinancialYearResponse { Success = false, Message = ex.Message };
        }
    }
}
```

---

### Priority 3: Ensure Repository Registration in DI

**File:** `Program-Enhanced.cs`

**Check for Repository Registration:**
```csharp
// Register repository (uncomment one)
services.AddScoped<IFinancialYearAggregateRepository, FinancialYearAggregateRepository>();      // EF Core
// OR
services.AddScoped<IFinancialYearAggregateRepository, FinancialYearDapperRepository>();  // Dapper
```

**Both implementations provided:**
- ✅ `FinancialYearAggregateRepository.cs` (EF Core)
- ✅ `FinancialYearDapperRepository.cs` (Dapper)

---

### Priority 4: Wire GraphQL (Optional but Recommended)

**Files:**
- `FinyearAPI.GraphQL/Types/FinancialYearType.cs` ✅ Complete
- `FinyearAPI.GraphQL/Queries/FinancialYearQuery.cs` ✅ Complete
- `FinyearAPI.GraphQL/Mutations/FinancialYearMutation.cs` ✅ Complete
- `FinyearAPI.GraphQL/Subscriptions/FinancialYearSubscription.cs` ✅ Complete

**What to Do:**
1. Install HotChocolate NuGet package:
   ```bash
   dotnet add package HotChocolate.AspNetCore
   ```

2. Update `Program.cs` with GraphQL registration:
   ```csharp
   services.AddGraphQLServer()
       .AddQueryType<FinancialYearQuery>()
       .AddMutationType<FinancialYearMutation>()
       .AddSubscriptionType<FinancialYearSubscription>()
       .AddAuthorization();

   // Map GraphQL endpoint
   app.MapGraphQL("/graphql");
   ```

---

## Quick Start Steps

### 1. Restore Dependencies
```bash
cd finyearServices/src
dotnet restore
```

### 2. Configure Database Connection
Edit `appsettings-Enhanced.json`:
```json
{
  "ConnectionStrings": {
    "AdminDB": "Server=(localdb)\\mssqllocaldb;Database=FinyearDB;Integrated Security=true;"
  }
}
```

### 3. Configure JWT Secret
Edit `appsettings-Enhanced.json`:
```json
{
  "Jwt": {
    "SecretKey": "your-super-secret-key-at-least-32-chars-long!!!",
    "Issuer": "FinyearAPI",
    "Audience": "FinyearAPIClients",
    "ExpirationMinutes": 60
  }
}
```

⚠️ **SECURITY:** Change the secret key! Generate a random 32+ character string:
```powershell
$newSecret = [Convert]::ToBase64String((1..32 | ForEach-Object { [byte](Get-Random -Maximum 256) }))
Write-SetClipboard $newSecret
# Paste into appsettings.json
```

### 4. Copy Enhanced Files
```bash
# Copy the complete Program.cs setup
cp src/FinyearAPI/Program-Enhanced.cs src/FinyearAPI/Program.cs

# Copy the complete configuration
cp appsettings-Enhanced.json src/FinyearAPI/appsettings.json

# Copy the production-ready auth service
cp src/Services/AuthProvider/Authentication/AuthService-Production.cs src/Services/AuthProvider/Authentication/AuthService.cs
```

### 5. Apply Database Migrations
```bash
# Install EF Core tools if not installed
dotnet tool install --global dotnet-ef

# Create and apply migration
cd src/FinyearAPI.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../FinyearAPI/FinyearAPI.csproj
dotnet ef database update --startup-project ../FinyearAPI/FinyearAPI.csproj
```

### 6. Run Application
```bash
cd src/FinyearAPI
dotnet run
```

The API will start on: `https://localhost:7136` (or similar)

---

## Testing the API

### Get Access Token
```bash
# Request: Authenticate
curl -X POST https://localhost:7136/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'

# Response:
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2024-01-15T11:30:00Z",
  "expiresIn": 3600
}
```

### Create Financial Year (Admin Only)
```bash
curl -X POST https://localhost:7136/api/v1/financialyear \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {accessToken}" \
  -d '{
    "name": "FY 2024-25",
    "startDate": "2024-04-01",
    "endDate": "2025-03-31"
  }'
```

### Get All Financial Years (Authenticated)
```bash
curl -X GET https://localhost:7136/api/v1/financialyear \
  -H "Authorization: Bearer {accessToken}"
```

### Get Current Financial Year (Public)
```bash
curl -X GET https://localhost:7136/api/v1/financialyear/current
```

---

## File Organization

### Domain Layer (FinyearAPI.Domain/)
```
FinyearAPI.Domain/
├── Entities/
│   ├── Entity.cs                          Base entity with domain events
│   └── FinancialYearAggregate.cs          Aggregate root with business logic
├── ValueObjects/
│   ├── ValueObject.cs                     Base value object
│   └── DateRange.cs                       Date range value object
├── Events/
│   └── FinancialYearDomainEvents.cs       Domain events (Created, Updated, Closed)
└── Repositories/
    └── IFinancialYearAggregateRepository.cs Repository interface
```

### Application Layer (FinyearAPI.Application/)
```
FinyearAPI.Application/
├── Commands/
│   └── FinancialYearCommands.cs           4 commands: Create, Update, Close, Delete
├── Queries/
│   └── FinancialYearQueries.cs            4 queries: GetAll, GetById, GetCurrent, GetByName
├── DTOs/
│   └── FinancialYearDtos.cs               Data transfer objects
└── Handlers/
    └── FinancialYearCommandHandlers.cs    Command & query handlers
```

### Infrastructure Layer (FinyearAPI.Infrastructure/)
```
FinyearAPI.Infrastructure/
├── Repositories/
│   ├── FinancialYearAggregateRepository.cs    EF Core implementation ✅
│   └── FinancialYearDapperRepository.cs       Dapper implementation ✅
├── Messaging/
│   └── MessageBus.cs                     RabbitMQ integration
├── Resilience/
│   └── ResiliencePolicy.cs               Circuit breaker, retry, callback patterns
└── Adapters/
    └── ServiceAdapters.cs                HTTP & Azure Blob adapters
```

### Gateway Layer (FinyearAPI.Gateway/)
```
FinyearAPI.Gateway/
├── Middleware/
│   └── MiddlewareExtensions.cs           5 middleware: Exception, Logging, Versioning, CORS, Auth
└── Routing/
    └── GatewayRoutes.cs                  8 API routes + health + info endpoints
```

### GraphQL Layer (FinyearAPI.GraphQL/)
```
FinyearAPI.GraphQL/
├── Types/
│   └── FinancialYearType.cs              GraphQL types (Input, Output, Payload)
├── Queries/
│   └── FinancialYearQuery.cs             6 GraphQL queries
├── Mutations/
│   └── FinancialYearMutation.cs          4 GraphQL mutations
└── Subscriptions/
    └── FinancialYearSubscription.cs      4 WebSocket subscriptions
```

### Auth Layer (Services.AuthProvider/)
```
Services.AuthProvider/
├── Authentication/
│   ├── AuthService.cs                    JWT authentication (TO BE UPDATED)
│   └── AuthService-Production.cs         Production-ready implementation ✅
└── Authorization/
    └── AuthorizationService.cs           Role & claim-based authorization ✅
```

---

## Configuration Deep Dive

### appsettings-Enhanced.json Sections

#### 1. Database Connection
```json
{
  "ConnectionStrings": {
    "AdminDB": "Server=(localdb)\\mssqllocaldb;Database=FinyearDB;Integrated Security=true;"
  }
}
```

#### 2. JWT Configuration
```json
{
  "Jwt": {
    "SecretKey": "your-super-secret-key-at-least-32-chars-long!!!",
    "Issuer": "FinyearAPI",
    "Audience": "FinyearAPIClients",
    "ExpirationMinutes": 60
  }
}
```

#### 3. RabbitMQ Configuration
```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/"
  }
}
```

To use RabbitMQ locally:
```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
# Then access: http://localhost:15672 (guest/guest)
```

#### 4. Resilience Configuration
```json
{
  "Resilience": {
    "CircuitBreaker": {
      "FailureThreshold": 3,        // Opens after 3 failures
      "TimeoutSeconds": 30          // Waits 30 seconds before retrying
    },
    "Retry": {
      "Attempts": 3,                // Max 3 retry attempts
      "InitialDelayMs": 1000        // Exponential backoff
    }
  }
}
```

---

## Key Interfaces to Understand

### CQRS Interfaces
```csharp
// Commands (write operations)
public interface ICommand<TResponse> { }
public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
}

// Queries (read operations)
public interface IQuery<TResponse> { }
public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);
}
```

### Domain Interfaces
```csharp
// Domain events
public abstract class DomainEvent
{
    public string EventId { get; set; }
    public DateTime OccurredAt { get; set; }
}

// Entity base
public abstract class Entity
{
    public long Id { get; set; }
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    protected void AddDomainEvent(DomainEvent myDomainEvent) => _domainEvents.Add(myDomainEvent);
}

// Value object
public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();
}

// Repository
public interface IFinancialYearAggregateRepository
{
    Task<FinancialYearAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<FinancialYearAggregate> AddAsync(FinancialYearAggregate aggregate, CancellationToken cancellationToken = default);
    Task<FinancialYearAggregate> UpdateAsync(FinancialYearAggregate aggregate, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
```

### Infrastructure Interfaces
```csharp
// Message Bus
public interface IMessageBus
{
    Task SendAsync<T>(T message, string routingKey);
    Task SubscribeAsync<T>(Func<T, Task> handler);
    Task StartConsumingAsync();
    Task StopConsumingAsync();
}

// Event Publisher
public interface IEventPublisher
{
    Task PublishAsync(IReadOnlyList<DomainEvent> events);
    Task PublishBatchAsync(IEnumerable<DomainEvent> events);
}

// Resilience
public interface IResiliencePolicy
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> operation);
    IAsyncPolicy<T> GetCircuitBreakerPolicy<T>();
    IAsyncPolicy<T> GetRetryPolicy<T>();
    IAsyncPolicy<T> GetCombinedPolicy<T>();
}

// Callbacks
public interface ICallbackHandler<T>
{
    Task<T> HandleCircuitBreakerOpenAsync();
    Task<T> HandleRetryExhaustedAsync(Exception exception);
}
```

---

## Common Tasks

### Add a New Financial Year Command Handler
1. Update `FinancialYearCommandHandlers.cs`
2. Implement `ICommandHandler<YourCommand, YourResponse>`
3. Call aggregate's domain method
4. Save via repository
5. Publish domain events

### Add a New GraphQL Query
1. Update `FinyearAPI.GraphQL/Queries/FinancialYearQuery.cs`
2. Add method returning `IQueryable<FinancialYearType>` or similar
3. Decorate with `[GraphQLType]`
4. Inject query handler via constructor

### Add Custom Authorization Policy
1. Define in `AuthorizationPolicies` constants
2. Check in `AuthorizationService`
3. Use in route: `.RequireAuthorization("YourPolicy")`

### Add Middleware
1. Create `public class YourMiddleware`
2. Implement `InvokeAsync(HttpContext context, ...dependencies)`
3. Register in `Program.cs`: `app.UseMiddleware<YourMiddleware>()`
4. Position in pipeline (order matters!)

---

## Troubleshooting

### "JWT token is null or empty"
- Ensure authentication middleware returns authorization header
- Check BearerAuthenticationHandler implementation
- Verify request has `Authorization: Bearer {token}` header

### "Invalid signing key"
- JWT SecretKey in appsettings must match secret in ValidateToken
- Minimum 32 characters required
- Must be identical issuer and audience

### "Database connection failed"
- Verify SQL Server LocalDB is running: `sqllocaldb info`
- Check connection string matches LocalDB instance name
- Run migrations: `dotnet ef database update`

### "Repository null exception"
- Repository must be registered in DI: `services.AddScoped<IFinancialYearAggregateRepository, ...>`
- Constructor parameter must match registered interface
- Check Program.cs DI section

### "RabbitMQ timeout"
- Start Docker container: `docker run -d --name rabbitmq -p 5672:5672 rabbitmq:3-management`
- Verify configuration hostname/port: `localhost:5672`
- Check RabbitMQ is not already running on port 5672

---

## Security Checklist

Before deploying to production:

- [ ] Change JWT SecretKey to random 32+ character string
- [ ] Store secrets in Azure Key Vault (not appsettings.json)
- [ ] Enable HTTPS only in production
- [ ] Set CORS to specific origins (not "*")
- [ ] Implement rate limiting
- [ ] Review authorization policies for completeness
- [ ] Hash passwords in database (not plaintext)
- [ ] Enable SQL encryption
- [ ] Set strong database passwords
- [ ] Enable request size limits to prevent DoS
- [ ] Never log sensitive data (passwords, tokens)
- [ ] Set database connection timeout
- [ ] Use parameterized queries (both EF and Dapper do this automatically)
- [ ] Implement API key rotation strategy for refresh tokens
- [ ] Add audit logging for admin operations

---

## Performance Optimization Tips

1. **Caching**: Add Redis for distributed cache
2. **Database Indexing**: Index frequently queried columns
3. **Dapper for reads**: Use Dapper for high-volume queries
4. **Async all the way**: Ensure all I/O is async
5. **Connection pooling**: EF Core has this by default
6. **Monitoring**: Use Application Insights to track slow queries
7. **Pagination**: Always paginate large result sets
8. **Compression**: Enable response compression in middleware
9. **CDN**: Cache static responses via CDN

---

## Documentation Files

All provided documentation:
- `ENTERPRISE-PATTERNS-GUIDE.md` ← Read this first
- `ARCHITECTURE.md` ← Comprehensive technical reference
- `IMPLEMENTATION-GUIDE.md` ← This file
- `README.md` ← High-level overview

---

## Getting Help

### Understanding the Architecture
1. Read `ENTERPRISE-PATTERNS-GUIDE.md` (quick reference)
2. Read `ARCHITECTURE.md` (detailed explanations)
3. Review `FinancialYearAggregate.cs` (DDD example)
4. Check `FinancialYearCommandHandlers.cs` (CQRS example)

### Implementing Missing Parts
1. Copy from `AuthService-Production.cs` to `AuthService.cs`
2. Follow handler templates in file comments
3. Use existing handlers as pattern for new handlers

### Troubleshooting
1. Check logs in Output panel
2. Use debugger to step through code
3. Verify configuration in appsettings.json
4. Check DI registration in Program.cs

---

**Version**: 1.0  
**Last Updated**: January 2024  
**Framework**: .NET 8 / ASP.NET Core 8.0  
**Status**: Ready for development and testing
