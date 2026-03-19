# 🏗️ Architecture & Design Documentation

## Table of Contents

1. [System Architecture](#system-architecture)
2. [Domain-Driven Design](#domain-driven-design)
3. [CQRS Pattern](#cqrs-pattern)
4. [Event-Driven Architecture](#event-driven-architecture)
5. [Design Patterns](#design-patterns)
6. [Data Flow](#data-flow)
7. [Deployment Architecture](#deployment-architecture)

## System Architecture

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    API LAYER                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐   │
│  │  REST API    │  │  GraphQL API │  │ Minimal APIs │   │
│  └──────────────┘  └──────────────┘  └──────────────┘   │
│                         ▼                                │
│              JWT Authentication & CORS                 │
└─────────────────────────────────────────────────────────┘
                          ▼
┌─────────────────────────────────────────────────────────┐
│              APPLICATION LAYER (CQRS)                   │
│  ┌──────────────┐           ┌──────────────┐            │
│  │   Commands   │           │    Queries   │            │
│  │ (Create/Upd) │           │ (Read/Search)│            │
│  └──────────────┘           └──────────────┘            │
│         ▼                           ▼                    │
│  ┌──────────────────────────────────────────┐           │
│  │    MediatR Pipeline Behaviors            │           │
│  │ • Validation  • Logging  • Exception Hdl │           │
│  └──────────────────────────────────────────┘           │
│         ▼                           ▼                    │
│  ┌──────────────┐           ┌──────────────┐            │
│  │  Cmd Handler │           │  Query Handler│           │
│  └──────────────┘           └──────────────┘            │
│         ▼                           ▼                    │
│     Domain Events          Read Models/DTOs            │
└─────────────────────────────────────────────────────────┘
                          ▼
┌─────────────────────────────────────────────────────────┐
│              DOMAIN LAYER                                │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐   │
│  │   Entities   │  │Value Objects │  │   Events     │   │
│  │ • Expense    │  │ • Money      │  │ • Created    │   │
│  │ • ExpFile    │  │ • DateRange  │  │ • Updated    │   │
│  │              │  │ • Category   │  │ • Deleted    │   │
│  └──────────────┘  └──────────────┘  └──────────────┘   │
│                  Business Rules & Logic                 │
└─────────────────────────────────────────────────────────┘
                          ▼
┌─────────────────────────────────────────────────────────┐
│           INFRASTRUCTURE LAYER                           │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐   │
│  │  EF DbContext│  │  Repositories│  │  Unit of Work│   │
│  │              │  │  (Entity Fr) │  │              │   │
│  └──────────────┘  └──────────────┘  └──────────────┘   │
│         ▼                                                │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐   │
│  │ Blob Storage │  │RabbitMQ Bus  │  │   Services   │   │
│  └──────────────┘  └──────────────┘  └──────────────┘   │
└─────────────────────────────────────────────────────────┘
                     ▼
        ┌──────────────────────────────┐
        │   External Services          │
        │ • SQL Server Database         │
        │ • Azure Blob Storage          │
        │ • RabbitMQ Message Broker     │
        │ • Azure Functions             │
        └──────────────────────────────┘
```

## Domain-Driven Design

### Aggregate Root: Expense

The `Expense` entity is the aggregate root for mobile expense management.

```csharp
public class Expense
{
    // Identity
    public decimal Id { get; }
    
    // Value Objects
    public decimal TripId { get; }
    public decimal CategoryId { get; }
    public Money Amount { get; }  // Value Object
    
    // Temporal Data
    public DateTime ExpenseDate { get; }
    public DateTime EnteredOn { get; }
    
    // Entity Collections
    public ICollection<ExpenseFile> Files { get; } = new List<ExpenseFile>();
    
    // Business Methods
    public static Expense Create(...) { /* Factory Method */ }
    public void Update(...) { /* Mutator */ }
    public void Delete(...) { /* Soft Delete */ }
    public void AddFile(ExpenseFile file) { /* Aggregate Operation */ }
    
    // Domain Events
    private readonly List<DomainEvent> _domainEvents = new();
    public void AddDomainEvent(DomainEvent @event) { /* Event Publishing */ }
}
```

### Entities

1. **Expense (Aggregate Root)**
   - Primary business entity
   - Manages its own state and invariants
   - Controls its child collection (ExpenseFiles)
   - Publishes domain events

2. **ExpenseFile (Entity)**
   - Child entity owned by Expense aggregate
   - Represents file attachments
   - Lifecycle managed by parent aggregate

### Value Objects

1. **Money**: Amount + Currency
2. **DateRange**: StartDate + EndDate
3. **ExpenseCategory**: CategoryId + Name + MaxLimit

### Domain Events

- `ExpenseCreatedDomainEvent`
- `ExpenseUpdatedDomainEvent`  
- `ExpenseDeletedDomainEvent`

These events are published after state changes and handled by application services.

## CQRS Pattern

### Command Flow

```
Request → Command → Handler → Domain → Database → Event Publishing
                                           ↓
                      Domain Events → Event Handlers → Side Effects
```

### Query Flow

```
Request → Query Handler → Repository → Database → DTO Response
```

### Benefits

1. **Separation of Concerns**: Read and write logic separated
2. **Scalability**: Read models can be scaled independently
3. **Performance**: Optimized queries and commands
4. **Flexibility**: Different consistency models per use case

### Implementation

#### Commands (Write Operations)

```csharp
// Command Definition
public class CreateExpenseCommand : IRequest<ExpenseDto>
{
    public decimal TripId { get; set; }
    public decimal Amount { get; set; }
    // ... other properties
}

// Command Handler
public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, ExpenseDto>
{
    public async Task<ExpenseDto> Handle(CreateExpenseCommand request, CancellationToken ct)
    {
        // 1. Create aggregate
        var expense = Expense.Create(request.TripId, ...);
        
        // 2. Persist
        await _repository.AddAsync(expense);
        await _unitOfWork.SaveChangesAsync();
        
        // 3. Publish events
        foreach (var evt in expense.DomainEvents)
            await _publisher.Publish(evt);
        
        // 4. Return DTO
        return _mapper.Map<ExpenseDto>(expense);
    }
}
```

#### Queries (Read Operations)

```csharp
// Query Definition
public class GetExpenseByIdQuery : IRequest<ExpenseDto?>
{
    public decimal ExpenseId { get; set; }
}

// Query Handler
public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, ExpenseDto?>
{
    public async Task<ExpenseDto?> Handle(GetExpenseByIdQuery request, CancellationToken ct)
    {
        // Direct read without domain logic
        var expense = await _repository.GetByIdAsync(request.ExpenseId, ct);
        return _mapper.Map<ExpenseDto>(expense);
    }
}
```

## Event-Driven Architecture

### Event Flow

```
1. User performs action (Create/Update/Delete)
                    ↓
2. Command handler executes business logic
                    ↓
3. Domain event is raised in aggregate
                    ↓
4. Event is saved with aggregate
                    ↓
5. After transaction success, event handler processes
                    ↓
6. Side effects: Notifications, RabbitMQ, Logging, etc.
```

### Event Handlers

```csharp
public class ExpenseCreatedEventHandler : INotificationHandler<ExpenseCreatedDomainEvent>
{
    public async Task Handle(ExpenseCreatedDomainEvent @event, CancellationToken ct)
    {
        // Side effects:
        // 1. Publish to RabbitMQ
        await _messageBus.PublishAsync(new ExpenseCreatedMessage
        {
            ExpenseId = @event.ExpenseId,
            Amount = @event.Amount,
            TripId = @event.TripId
        });
        
        // 2. Send notifications
        await _notificationService.NotifyManagerAsync(...);
        
        // 3. Update read models
        await _readModelUpdater.AddExpenseAsync(...);
    }
}
```

## Design Patterns

### 1. Repository Pattern

**Purpose**: Abstract data access logic

```csharp
public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(decimal expenseId);
    Task<List<Expense>> GetByTripIdAsync(decimal tripId);
    Task AddAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(decimal expenseId);
}

public class ExpenseRepository : IExpenseRepository
{
    private readonly ExpenseDbContext _context;
    
    public async Task AddAsync(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
    }
}
```

### 2. Unit of Work Pattern

**Purpose**: Manage database transactions and repositories

```csharp
public interface IUnitOfWork
{
    IExpenseRepository Expenses { get; }
    Task<int> SaveChangesAsync();
    Task<bool> BeginTransactionAsync();
    Task<bool> CommitTransactionAsync();
}

// Usage
var uow = serviceProvider.GetRequiredService<IUnitOfWork>();
await uow.Expenses.AddAsync(expense);
await uow.SaveChangesAsync();
```

### 3. Mediator Pattern

**Purpose**: Decouple request handlers from callers

```csharp
// In controller
var result = await _mediator.Send(new CreateExpenseCommand { ... });

// Internally:
// 1. MediatR finds handler for CreateExpenseCommand
// 2. Executes validation behavior
// 3. Executes logging behavior
// 4. Executes command handler
// 5. Returns result
```

### 4. Pipeline Behavior Pattern

**Purpose**: Add cross-cutting concerns

```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, 
        RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        // Before
        var validationResults = await ValidateAsync(request);
        
        // Request
        var response = await next();
        
        // After
        return response;
    }
}
```

### 5. Factory Pattern

**Purpose**: Create domain entities with validation

```csharp
public static Expense Create(decimal tripId, decimal categoryId, ...)
{
    // Validation
    if (amount <= 0)
        throw new ArgumentException("Invalid amount");
    
    // Creation
    var expense = new Expense { ... };
    
    // Event publishing
    expense.AddDomainEvent(new ExpenseCreatedDomainEvent(...));
    
    return expense;
}
```

### 6. Specification Pattern (Implicit)

Through repository methods:

```csharp
// Specialized queries
await _repository.GetByTripIdAsync(tripId);
await _repository.SearchByDateRangeAsync(start, end);
await _repository.GetByTripIdPaginatedAsync(tripId, page, size);
```

## Data Flow

### Create Expense Flow

```
┌──────────────────────────────────────────────────────────┐
│ 1. HTTP POST /api/expenses                               │
│    Body: { tripId, categoryId, amount, ... }             │
└──────────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────────┐
│ 2. ExpensesController.CreateExpense()                    │
│    • Extract X-User-Id header                            │
│    • Create CreateExpenseCommand                         │
└──────────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────────┐
│ 3. MediatR.Send(CreateExpenseCommand)                    │
│    • Validation Behavior → FluentValidation              │
│    • Logging Behavior → Log request                      │
│    • Exception Handling Behavior → Catch errors          │
└──────────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────────┐
│ 4. CreateExpenseCommandHandler.Handle()                  │
│    • Call Expense.Create() (factory method)              │
│    • Validate business rules                             │
│    • Add to repository                                   │
│    • Save changes to database                            │
└──────────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────────┐
│ 5. Domain Event Publishing                               │
│    • ExpenseCreatedDomainEvent → IPublisher              │
│    • ExpenseCreatedEventHandler processes event          │
│    • RabbitMQ notification sent                          │
│    • Manager notification queued                         │
└──────────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────────┐
│ 6. Response to Client                                    │
│    { id, tripId, amount, ... }  (201 Created)            │
│    Location header: /api/expenses/1000                   │
└──────────────────────────────────────────────────────────┘
```

### Query Expense Flow

```
┌──────────────────────────────────────────────────────────┐
│ 1. HTTP GET /api/expenses/{id}                           │
└──────────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────────┐
│ 2. ExpensesController.GetExpenseById(id)                 │
│    • Create GetExpenseByIdQuery                          │
└──────────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────────┐
│ 3. MediatR.Send(GetExpenseByIdQuery)                     │
│    • Validation Behavior (lightweight)                   │
│    • Logging Behavior                                    │
└──────────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────────┐
│ 4. GetExpenseByIdQueryHandler.Handle()                   │
│    • Call repository.GetByIdAsync()                      │
│    • Map to ExpenseDto                                   │
└──────────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────────┐
│ 5. Database Query                                        │
│    SELECT * FROM MOBEXP_DET WHERE MOBEXP_ID = @id       │
│    AND MOBEXP_ISDELETED = 0                              │
└──────────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────────┐
│ 6. Response to Client                                    │
│    { id, tripId, amount, ... }  (200 OK)                 │
└──────────────────────────────────────────────────────────┘
```

## Deployment Architecture

### On-Premises Deployment

```
┌────────────────────────────────────────────────────────┐
│           Single Server / IIS                           │
├────────────────────────────────────────────────────────┤
│ MobileExpenseManagement.API                            │
│  - Kestrel / HTTP.sys / IIS Integration                │
│  - Port 80 (HTTP) / 443 (HTTPS)                        │
│  - Multiple worker processes for concurrency           │
└────────────────────────────────────────────────────────┘
                        ↓
        ┌──────────────────────────────┐
        │  SQL Server Database         │
        │  - Connection pooling        │
        │  - Automated backups         │
        └──────────────────────────────┘
                        ↓
        ┌──────────────────────────────┐
        │  RabbitMQ Message Broker     │
        │  - Message persistence       │
        │  - Dead-letter queues        │
        └──────────────────────────────┘
                        ↓
        ┌──────────────────────────────┐
        │  File Storage                │
        │  - Shared folder / NAS       │
        │  - Backup strategy           │
        └──────────────────────────────┘
```

### Azure Cloud Deployment

```
┌──────────────────────────────────────────────────────────┐
│                 Azure App Service                        │
│  - Auto-scaling based on CPU/Memory                      │
│  - Multiple instances (2+)                              │
│  - Load Balancer (built-in)                             │
│  - SSL/TLS termination                                  │
└──────────────────────────────────────────────────────────┘
                        ↓
    ┌───────────┬───────────┬───────────┐
    ↓           ↓           ↓
┌─────────┐ ┌─────────┐ ┌─────────┐
│ Azure   │ │ Azure   │ │ Azure   │
│ SQL DB  │ │ Blob    │ │ Service │
│         │ │ Storage │ │ Bus     │
└─────────┘ └─────────┘ └─────────┘
    ↓           ↓           ↓
Connection  File        Message
Pool        Storage      Queue
```

## Security Architecture

### Authentication & Authorization Flow

```
1. Client authenticates (username/password)
                    ↓
2. Generate JWT with claims
   {
     "sub": "101",           // User ID
     "email": "user@...",
     "EmployeeId": "101",
     "iat": 1234567890,      // Issued at
     "exp": 1234571490       // Expiration
   }
                    ↓
3. Client includes in every request
   Authorization: Bearer <jwt>
                    ↓
4. API validates signature with secret key
                    ↓
5. User claims extracted and available in handler
                    ↓
6. Authorization checks performed (if required)
```

### Data Protection

- **In Transit**: HTTPS/TLS encryption
- **At Rest**: SQL Server encryption,  Blob Storage encryption
- **Sensitive Data**: Hashed using SHA-256 (passwords if applicable)

## Performance Considerations

1. **Database Indexing**
   - Indexes on: TripId, CategoryId, EnteredBy, IsDeleted

2. **Pagination**
   - Large result sets paginated (page size: 10-100)

3. **Caching** (Future)
   - Redis for frequently accessed data
   - Cache-aside pattern

4. **Asynchronous Operations**
   - All I/O operations use async/await
   - Non-blocking request handling

5. **Connection Pooling**
   - SQL Server: Min=5, Max=100
   - RabbitMQ: Channel pooling

## Monitoring & Observability

### Structured Logging

```csharp
_logger.LogInformation(
    "Expense created: {ExpenseId} for Trip {TripId} with amount {Amount}",
    expense.Id, expense.TripId, expense.Amount);
```

### Application Insights (Azure)

- Request telemetry
- Performance counters
- Exception tracking
- Custom metrics

### Health Checks

```
GET /health
{
  "status": "Healthy",
  "checks": {
    "SQL Server": { "status": "Healthy" }
  }
}
```

This comprehensive architecture provides scalability, maintainability, and extensibility for the Mobile Expense Management microservice!
