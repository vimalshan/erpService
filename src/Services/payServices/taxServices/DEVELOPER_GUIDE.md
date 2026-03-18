# Tax Service - Developer Quick Start Guide

## Getting Started (5 minutes)

### 1. Clone/Open the Project
```bash
cd e:\ERPMicroservice\src\Services\payServices\taxServices
```

### 2. Build Solution
```bash
dotnet build
```

### 3. Run API
```bash
cd src/TaxService.API
dotnet run
```

You should see:
```
Now listening on: https://localhost:5001
Application started. Press Ctrl+C to exit.
```

### 4. Test the API
```bash
# Health check (no auth required)
curl https://localhost:5001/health

# List active payees (requires JWT)
curl -X GET "https://localhost:5001/api/conditionalmasters/active" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## Common Tasks

### Add a New Command

1. **Create Command in Application Layer**:
```csharp
// src/TaxService.Application/Commands/TaxCommands.cs
public record UpdateTaxMarginalDetailCommand(
    long Id, 
    decimal GrossIncome) 
    : IRequest<Result<TaxMarginalDetailDto>>;
```

2. **Create Handler in Infrastructure Layer**:
```csharp
// src/TaxService.Infrastructure/CommandHandlers/TaxCommandHandlers.cs
public class UpdateTaxMarginalDetailCommandHandler 
    : IRequestHandler<UpdateTaxMarginalDetailCommand, Result<TaxMarginalDetailDto>>
{
    public async Task<Result<TaxMarginalDetailDto>> Handle(
        UpdateTaxMarginalDetailCommand request,
        CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

3. **Create Validator**:
```csharp
// src/TaxService.Application/Validators/TaxValidators.cs
public class UpdateTaxMarginalDetailCommandValidator 
    : AbstractValidator<UpdateTaxMarginalDetailCommand>
{
    public UpdateTaxMarginalDetailCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.GrossIncome).GreaterThanOrEqualTo(0);
    }
}
```

4. **Add Controller Endpoint**:
```csharp
// src/TaxService.API/Controllers/TaxMarginalDetailsController.cs
[HttpPut("{id}")]
public async Task<ActionResult<TaxMarginalDetailDto>> Update(
    long id, 
    [FromBody] decimal grossIncome)
{
    var command = new UpdateTaxMarginalDetailCommand(id, grossIncome);
    var result = await _mediator.Send(command);
    
    if (!result.IsSuccess)
        return BadRequest(new { message = result.Error });
    
    return Ok(result.Data);
}
```

### Add a New Query

1. **Create Query**:
```csharp
// src/TaxService.Application/Queries/TaxQueries.cs
public record GetTaxDetailsByYearQuery(int FinancialYear) 
    : IRequest<Result<IEnumerable<TaxMarginalDetailDto>>>;
```

2. **Create Handler**:
```csharp
public class GetTaxDetailsByYearQueryHandler 
    : IRequestHandler<GetTaxDetailsByYearQuery, Result<IEnumerable<TaxMarginalDetailDto>>>
{
    private readonly ITaxMarginalDetailRepository _repository;
    private readonly IMapper _mapper;

    public async Task<Result<IEnumerable<TaxMarginalDetailDto>>> Handle(
        GetTaxDetailsByYearQuery request,
        CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

### Add Domain Event

1. **Create Event in Domain Layer**:
```csharp
// src/TaxService.Domain/Entities/TaxMarginalDetail.cs
public sealed class TaxExemptionAddedEvent : DomainEvent
{
    public long TaxMarginalDetailId { get; }
    public string ExemptionCode { get; }

    public TaxExemptionAddedEvent(long taxId, string code)
    {
        TaxMarginalDetailId = taxId;
        ExemptionCode = code;
    }
}
```

2. **Raise Event from Aggregate**:
```csharp
public void AddExemption(TaxExemption exemption)
{
    Exemptions.Add(exemption);
    _domainEvents.Add(new TaxExemptionAddedEvent(Id, exemption.Code));
}
```

3. **Handle Event** (Future: In background worker):
```csharp
// When event sourcing is implemented
public class TaxExemptionAddedEventHandler 
    : INotificationHandler<TaxExemptionAddedEvent>
{
    public async Task Handle(
        TaxExemptionAddedEvent notification, 
        CancellationToken cancellationToken)
    {
        // Send notification, audit log, etc.
    }
}
```

### Modify Database Schema

1. **Make Changes to Entity** (e.g., add property):
```csharp
// src/TaxService.Domain/Entities/TaxMarginalDetail.cs
public string TaxCalculationMethod { get; set; }
```

2. **Update DbContext Configuration**:
```csharp
// src/TaxService.Infrastructure/Data/TaxServiceDbContext.cs
entity.Property(e => e.TaxCalculationMethod)
    .HasMaxLength(50);
```

3. **Create Migration**:
```bash
cd src/TaxService.Infrastructure
dotnet ef migrations add AddTaxCalculationMethod -s ../TaxService.API
```

4. **Update Database**:
```bash
dotnet ef database update -s ../TaxService.API
```

### Add Authentication/Authorization

Currently uses JWT Bearer tokens. To add role-based authorization:

1. **Add Role Claim to Token**:
```csharp
var tokenDescriptor = new SecurityTokenDescriptor
{
    Subject = new ClaimsIdentity(new[] {
        new Claim("sub", userId),
        new Claim(ClaimTypes.Role, "TaxAdmin")  // Add role
    }),
    // ... rest of configuration
};
```

2. **Protect Endpoint**:
```csharp
[Authorize(Roles = "TaxAdmin")]
[HttpPost]
public async Task<ActionResult<ConditionalMasterDto>> Create(...)
{
    // Only users with TaxAdmin role can access
}
```

---

## Debugging

### Enable Logging
Add to `Program.cs`:
```csharp
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.SetMinimumLevel(LogLevel.Debug);
});
```

### Database Query Logging
Add to DbContext configuration:
```csharp
services.AddDbContext<TaxServiceDbContext>(options =>
    options
        .UseSqlServer(connectionString)
        .EnableSensitiveDataLogging()  // Log parameter values
        .LogTo(Console.WriteLine));     // Log to console
```

### View Current SQL
```bash
# In SSMS or SQL Server Management Tools
SELECT * FROM ConditionalMasters;
SELECT * FROM TaxMarginalDetails;
```

### Check Migrations Applied
```bash
# Returns last applied migration batch number
SELECT MAX(MigrationId) FROM __EFMigrationsHistory;
```

---

## Common Issues & Solutions

### Issue: "Unable to trace EF Core change tracking"
**Solution**: 
```bash
cd src/TaxService.Infrastructure
dotnet ef database drop
dotnet ef database update
```

### Issue: JWT Token Invalid
**Solution**: 
- Verify token hasn't expired
- Check secret key in appsettings.json matches token generation
- Ensure token includes all required claims

### Issue: CORS Error
**Solution**: 
Check that origin is allowed in `Program.cs`:
```csharp
options.AddPolicy("AllowAll", corsPolicyBuilder =>
{
    corsPolicyBuilder
        .WithOrigins("https://localhost:3000")  // Add your origin
        .AllowAnyMethod()
        .AllowAnyHeader();
});
```

### Issue: Database Lock
**Solution**:
```bash
# Kill connections to database
KILL CONNECTION SPID_NUMBER;

# Or restart SqlLocalDB
sqllocaldb stop MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
```

---

## Performance Tips

1. **Use Indexes**: Already configured on common query columns
2. **Async All the Way**: All I/O operations are async
3. **Lazy Loading Disabled**: Load related entities explicitly
4. **Query Optimization**: Use repository methods instead of raw queries
5. **Caching**: Can be added at repository layer
6. **Monitoring**: Use Application Insights in production

---

## Code Organization

### Follow These Patterns

✅ **DO**:
- Keep business logic in domain layer
- Use dependency injection
- Return Result<T> from handlers
- Make domain entities immutable where possible
- Use value objects for type safety
- Document public methods

❌ **DON'T**:
- Put business logic in controllers
- Use static classes
- Reference UI from domain layer
- Create circular dependencies
- Use raw SQL queries in application layer

---

## Running Tests (When Implemented)

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test src/TaxService.Application.Tests

# Run with coverage
dotnet test /p:CollectCoverage=true
```

---

## Useful Commands Reference

```bash
# Build
dotnet build

# Run
dotnet run

# Publish
dotnet publish -c Release

# Add package
dotnet add package PackageName

# Remove package
dotnet remove package PackageName

# Update package
dotnet package update PackageName

# List packages
dotnet list package

# EF Migrations
dotnet ef migrations add MigrationName -s ../TaxService.API
dotnet ef database update -s ../TaxService.API
dotnet ef migrations remove -s ../TaxService.API

# Clean build
dotnet clean
dotnet build
```

---

## Further Learning

- **MediatR**: https://github.com/jbogard/MediatR
- **EF Core**: https://docs.microsoft.com/ef/core/
- **FluentValidation**: https://fluentvalidation.net/
- **AutoMapper**: https://automapper.org/
- **JWT**: https://jwt.io/

---

**Happy coding! 🚀**
