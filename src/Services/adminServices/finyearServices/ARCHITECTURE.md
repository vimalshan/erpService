# Architecture & Implementation Guide

## Complete Project Overview

This document provides an in-depth technical explanation of the FinyearAPI architecture, implementation details, and how all components work together.

## Technology Stack

```
┌─────────────────────────────────────────────┐
│         ASP.NET Core 8 Web API              │
├─────────────────────────────────────────────┤
│  Swagger/OpenAPI · Logging · CORS · JSON   │
├─────────────────────────────────────────────┤
│ Controllers (REST Endpoints)                │
├─────────────────────────────────────────────┤
│ Services (Business Logic)                   │
├─────────────────────────────────────────────┤
│ Unit of Work (Transaction Management)       │
├─────────────────────────────────────────────┤
│ Repositories (Data Access)                  │
│  ├─ Entity Framework Core (Full ORM)        │
│  └─ Dapper (High-Performance Queries)       │
├─────────────────────────────────────────────┤
│ SQL Server LocalDB (Data Storage)           │
└─────────────────────────────────────────────┘
```

## Detailed Component Breakdown

### 1. Models Layer

#### FinancialYearMaster.cs
- **Type**: Entity Class
- **Purpose**: Domain model mapped to FINYEAR_MASTER table
- **Key Features**:
  - Data annotations for validation
  - Computed properties (IsActive, DurationInDays)
  - Column mappings for database compatibility

```csharp
[Table("FINYEAR_MASTER")]
public class FinancialYearMaster
{
    [Key]
    [Column("FY_ID")]
    public long FinancialYearId { get; set; }
    
    // Maps to FY_NAME, max 27 characters
    // Database uses VARCHAR(27)
}
```

#### DTOs (Create/Update)
- **Purpose**: API request/response contracts
- **Separation**: Isolate API contracts from entity model
- **Benefits**:
  - Can change API without changing database
  - Selective field exposure
  - Validation at API layer

### 2. Repository Pattern

#### Generic Repository (RepositoryBase<T>)
Provides common CRUD operations:

```
IRepository<T>
├── GetByIdAsync()           // Get single entity
├── GetAllAsync()            // Get all entities
├── AddAsync()               // Create
├── UpdateAsync()            // Modify
├── DeleteAsync()            // Remove
└── ExistsAsync()            // Check existence
```

#### Specialized Repository (IFinancialYearRepository)
Extends generic with business-specific queries:

```
IFinancialYearRepository : IRepository<T>
├── GetCurrentFinancialYearAsync()
├── GetByNameAsync()
├── GetActiveFinancialYearsAsync()
└── GetFinancialYearsByDateRangeAsync()
```

#### Implementation Details

**Entity Framework Core Implementation**:
- Uses DbContext for queries
- Supports async/await for scalability
- Automatic change tracking
- Built-in transaction support

**Dapper Implementation**:
- Direct SQL execution for performance
- Used for:
  - Complex queries
  - Bulk operations
  - Reporting
  - Custom aggregations

### 3. Unit of Work Pattern

**Purpose**: Coordinate multiple repositories in a single transaction

```csharp
public interface IUnitOfWork
{
    IFinancialYearRepository FinancialYearRepository { get; }
    IFinancialYearDapperRepository FinancialYearDapperRepository { get; }
    
    Task<int> SaveChangesAsync();           // Persist all changes
    Task<bool> BeginTransactionAsync();     // Start transaction
    Task<bool> CommitAsync();               // Commit changes
    Task<bool> RollbackAsync();             // Revert changes
}
```

**Transaction Flow**:
```
1. BeginTransactionAsync()      // Start transaction
2. FinancialYearRepository.AddAsync()
3. FinancialYearRepository.UpdateAsync()
4. CommitAsync()                // All-or-nothing
        ↓
   (if error) RollbackAsync()   // Revert all
```

**ACID Guarantees**:
- **Atomicity**: All operations succeed or all fail
- **Consistency**: Database remains in valid state
- **Isolation**: Concurrent operations don't interfere
- **Durability**: Committed data persists

### 4. Service Layer

**Purpose**: Contains business logic and validation

**FinancialYearService**:
```
Service
├── Input Validation
│   └── Check date ranges
├── Business Logic
│   └── Calculate duration
├── Repository Coordination
│   └── Use Unit of Work
├── Error Handling
│   └── Exceptions and logging
└── State Management
    └── User audit trail
```

**Example Service Operation**:
```csharp
public async Task<FinancialYearMaster> CreateFinancialYearAsync(CreateFinancialYearDto dto)
{
    // 1. Validate input
    if (dto.CloseDate <= dto.StartDate)
        throw new ArgumentException("Invalid date range");
    
    // 2. Create entity
    var entity = new FinancialYearMaster { ... };
    
    // 3. Start transaction
    await _unitOfWork.BeginTransactionAsync();
    
    // 4. Persist to database
    await _unitOfWork.FinancialYearRepository.AddAsync(entity);
    
    // 5. Commit or rollback
    await _unitOfWork.CommitAsync();
    
    return entity;
}
```

### 5. Controller Layer

**Purpose**: Handle HTTP requests and responses

```csharp
[ApiController]
[Route("api/[controller]")]
public class FinancialYearController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FinancialYearMaster>>> GetAllFinancialYears()
    
    [HttpGet("{id}")]
    public async Task<ActionResult<FinancialYearMaster>> GetFinancialYearById(long id)
    
    [HttpPost]
    public async Task<ActionResult<FinancialYearMaster>> CreateFinancialYear([FromBody] CreateFinancialYearDto dto)
    
    [HttpPut("{id}")]
    public async Task<ActionResult<FinancialYearMaster>> UpdateFinancialYear(long id, [FromBody] UpdateFinancialYearDto dto)
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFinancialYear(long id)
}
```

**HTTP Methods Mapping**:
| HTTP | Method | Purpose |
|------|--------|---------|
| GET | Select | Retrieve data |
| POST | Insert | Create new |
| PUT | Update | Modify existing |
| DELETE | Delete | Remove record |

**HTTP Status Codes**:
| Code | Meaning |
|------|---------|
| 200 | OK - Request succeeded |
| 201 | Created - Resource created |
| 204 | No Content - Success, no data |
| 400 | Bad Request - Invalid input |
| 404 | Not Found - Resource missing |
| 500 | Internal Error - Server error |

### 6. Database Layer

**Entity Framework Core**:
- **Lazy Loading**: Load related data on demand
- **Change Tracking**: Monitor entity changes
- **Query Translation**: LINQ to SQL
- **Migrations**: Version control for database schema

**DbContext Configuration**:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Table mapping
    entity.ToTable("FINYEAR_MASTER");
    
    // Column mappings
    entity.Property(e => e.FinancialYearId).HasColumnName("FY_ID");
    
    // Key configuration
    entity.HasKey(e => e.FinancialYearId).HasName("PK_FINYEAR_MASTER");
    
    // Index configuration
    entity.HasIndex(e => e.StartDate).HasName("IDX_FINYEAR_STARTDATE");
}
```

**SQL Generation**:
```sql
-- EF Core generates this from LINQ
SELECT 
    [f].[FY_ID] as FinancialYearId,
    [f].[FY_NAME] as FinancialYearName,
    [f].[FY_STARTDATE] as StartDate,
    [f].[FY_CLOSEDATE] as CloseDate
FROM [FINYEAR_MASTER] AS [f]
WHERE [f].[FY_STARTDATE] <= @__now
```

## Data Flow Diagram

### Request Processing Flow
```
1. HTTP Request (Client)
         ↓
2. Controller (Route + Validate)
         ↓
3. Service (Business Logic)
    ├─ Validation
    ├─ Authorization
    └─ Transform DTO to Entity
         ↓
4. Unit of Work (Transaction)
         ↓
5. Repository (Data Access)
    ├─ EF Core (ORM)
    │  └─ DbContext
    │     └─ Change Tracker
    └─ Dapper (Direct SQL)
         ↓
6. Database (SQL Server)
    ├─ Execute Query
    ├─ Validate Constraints
    └─ Return Result
         ↓
7. Service (Format Response)
         ↓
8. Controller (HTTP Status)
         ↓
9. HTTP Response (Client)
```

### Create Operation (Detailed)
```
POST /api/financialyear
{
  "financialYearId": 1,
  "financialYearName": "2024-2025",
  "startDate": "2024-04-01",
  "closeDate": "2025-03-31",
  "updatedBy": 1
}
    ↓
1. FinancialYearController.CreateFinancialYear()
    - Receive DTO
    - Validate ModelState
    ↓
2. FinancialYearService.CreateFinancialYearAsync()
    - Validate dto properties
    - Check date logic: closeDate > startDate
    - Create entity from DTO
    ↓
3. UnitOfWork.BeginTransactionAsync()
    - Start SQL transaction
    ↓
4. FinancialYearRepository.AddAsync()
    - Create entity instance
    - Add to DbContext
    ↓
5. UnitOfWork.CommitAsync()
    - DbContext.SaveChangesAsync()
    - Generate INSERT SQL
    - Execute in transaction
    - Commit transaction
    ↓
6. Response
    201 Created
    Location: /api/financialyear/1
    Body: { FinancialYearId: 1, ... }
```

## Dependency Injection Configuration

**In Program.cs**:
```csharp
// Database
builder.Services.AddDbContext<AdminDbContext>(options =>
    options.UseSqlServer(connectionString));

// Dapper Connection
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(connectionString));

// Repositories
builder.Services.AddScoped<IFinancialYearRepository, FinancialYearRepository>();
builder.Services.AddScoped<IFinancialYearDapperRepository, FinancialYearDapperRepository>();

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IFinancialYearService, FinancialYearService>();
```

**Scope Lifetimes**:
- **Transient**: New instance each time (avoid for DbContext)
- **Scoped**: One instance per request (recommended for repositories)
- **Singleton**: Single instance for application (stateless utilities)

## Error Handling Strategy

### Exception Hierarchy
```
Exception
├── BusinessException
│   ├── ValidationException
│   ├── NotFoundException
│   └── DuplicateException
├── DatabaseException
│   └── TransactionException
└── System Exception
```

### Try-Catch-Rollback Pattern
```csharp
try
{
    await _unitOfWork.BeginTransactionAsync();
    // Multiple operations
    await _unitOfWork.CommitAsync();
}
catch (Exception ex)
{
    await _unitOfWork.RollbackAsync();  // Revert all changes
    _logger.LogError(ex, "Error message");
    throw;  // Propagate to controller
}
```

### Controller Response Mapping
```
Service Exception          →  HTTP Status
─────────────────────────────  ─────────
ValidationException        →  400 Bad Request
NotFoundException          →  404 Not Found
UnauthorizedAccessException → 401 Unauthorized
Exception (Database)        →  500 Internal Error
```

## Query Patterns

### Entity Framework Core
**Use for**:
- Simple CRUD operations
- Relationships/Joins
- Transaction management
- Type-safe queries

```csharp
// Get current financial year
var fy = await _context.FinancialYearMasters
    .Where(f => f.StartDate <= DateTime.Now && f.CloseDate >= DateTime.Now)
    .FirstOrDefaultAsync();
```

### Dapper
**Use for**:
- Complex queries
- Bulk operations
- Performance-critical paths
- Raw SQL performance

```csharp
// Complex aggregation with Dapper
const string sql = @"
SELECT FY_ID, COUNT(*) as TransactionCount
FROM FINYEAR_MASTER
GROUP BY FY_ID
HAVING COUNT(*) > 100
";
var result = await _dapperRepository.QueryAsync<dynamic>(sql);
```

## Performance Optimization

### Caching Strategy
```csharp
// Add distributed caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnectionString("Redis");
});
```

### Query Optimization
1. **Use Indexes**: IDX_FINYEAR_STARTDATE on StartDate
2. **Select Specific Columns**: Don't SELECT *
3. **Eager Loading**: Use .Include() for relationships
4. **Pagination**: SKIP + TAKE for large datasets
5. **Dapper for Bulk**: Batch operations with Dapper

### Connection Pooling
```
MaxPoolSize=10              // Maximum concurrent connections
MinPoolSize=1               // Minimum idle connections
PoolingTimeout=30           // Connection reuse timeout
```

## Logging Architecture

### Logging Levels
```
Trace   (0) - Very detailed, diagnostic info
Debug   (1) - Development debugging
Information (2) - General flow (production default)
Warning (3) - Problematic situations
Error   (4) - Errors requiring attention
Critical (5) - System failures
```

### EF Core SQL Logging
```csharp
// In Development
.LogTo(
    Console.WriteLine,
    new[] { RelationalEventId.CommandExecuted },
    LogLevel.Debug
);
```

## Testing Considerations

### Unit Testing
```csharp
// Mock repository
var mockRepository = new Mock<IFinancialYearRepository>();
mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
    .ReturnsAsync(new FinancialYearMaster { });

var service = new FinancialYearService(mockUnitOfWork, mockLogger);
```

### Integration Testing
```csharp
// Use TestContainer for SQL Server
var container = new MsSqlContainer(image: "mcr.microsoft.com/mssql/server:latest");
await container.StartAsync();
var connectionString = container.GetConnectionString();
```

## Security Considerations

1. **Authentication**: Implement JWT tokens
2. **Authorization**: Role-based access control
3. **Input Validation**: Sanitize all inputs
4. **SQL Injection**: EF Core & Parameterized queries prevent this
5. **HTTPS**: Always use HTTPS in production
6. **Secrets**: Use Azure Key Vault for connection strings
7. **Audit Trail**: Log all data modifications

## Monitoring & Diagnostics

### Health Checks
```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AdminDbContext>();

app.MapHealthChecks("/health");
```

### Application Insights
```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

### Structured Logging
```csharp
_logger.LogInformation(
    "FinancialYear created: {Id}, Name: {Name}, Duration: {Duration} days",
    entity.FinancialYearId,
    entity.FinancialYearName,
    entity.DurationInDays
);
```

## Deployment Considerations

### Production Checklist
- [ ] Connection string uses Production SQL Server
- [ ] Logging level set to Information/Warning
- [ ] HTTPS enforcement enabled
- [ ] CORS configured appropriately
- [ ] Authentication/Authorization implemented
- [ ] Database backups configured
- [ ] Connection pooling optimized
- [ ] Health checks endpoint secured
- [ ] Secrets in Key Vault (not in config files)
- [ ] Database migrations tested in staging

## Conclusion

This architecture provides:
✓ Clean separation of concerns
✓ Testability
✓ Maintainability
✓ Scalability
✓ Performance
✓ Transaction management
✓ Flexibility (EF + Dapper)

The combination of Entity Framework Core and Dapper allows team to balance:
- **Developer Productivity** (EF with LINQ)
- **Performance** (Dapper for critical paths)
- **Transaction Management** (Unit of Work)
