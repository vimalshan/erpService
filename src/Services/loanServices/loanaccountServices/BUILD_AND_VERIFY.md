# Build & Verification Guide

## Complete Build Process

### Step 1: Verify Prerequisites
```bash
# Check .NET version
dotnet --version

# Check SQL Server LocalDB installation
sqllocaldb i

# Verify SQL Server instance
sqllocaldb s
```

### Step 2: Restore NuGet Packages
```bash
# Navigate to solution root
cd LoanAccountService

# Restore all packages
dotnet restore
```

### Step 3: Build Solution
```bash
# Full build
dotnet build

# Build with specific configuration
dotnet build -c Release
```

### Step 4: Create Database & Apply Migrations
```bash
# Navigate to API project
cd src/LoanAccount.API

# Optional: Drop existing database
dotnet ef database drop

# Create database and apply migrations
dotnet ef database update

# Verify migration status
dotnet ef migrations list
```

### Step 5: Run Tests
```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true

# Run specific test project
dotnet test tests/LoanAccount.Tests
```

### Step 6: Start the API
```bash
# Run in development mode
dotnet run

# Run with watch mode (auto-restart on file change)
dotnet watch run
```

### Step 7: Verify API is Running
```bash
# Check health endpoint
curl https://localhost:5001/health

# Check Swagger UI
# Visit: https://localhost:5001/swagger

# Check GraphQL endpoint
curl https://localhost:5001/graphql
```

## Verification Checklist

### Database Verification
- [ ] Database created: `LoanAccountDb`
- [ ] Tables created:
  - [ ] LoanMains
  - [ ] LoanInstallments
  - [ ] LoanEmployeeInterestRates
  - [ ] LoanLedgers
  - [ ] LoanSettlements
- [ ] Indexes created on foreign keys
- [ ] Seed data populated (3 sample loans)

### API Endpoints Verification

#### Authentication
```bash
# Test login
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"demo","password":"demo"}'

# Expected response: JWT token
```

#### Loan Operations
```bash
# Get all active loans
curl -X GET https://localhost:5001/api/loans/active \
  -H "Authorization: Bearer <YOUR_TOKEN>"

# Get loan details
curl -X GET https://localhost:5001/api/loans/1001/details \
  -H "Authorization: Bearer <YOUR_TOKEN>"

# Get loan installments
curl -X GET https://localhost:5001/api/loans/1001/installments \
  -H "Authorization: Bearer <YOUR_TOKEN>"

# Get loan ledger
curl -X GET https://localhost:5001/api/loans/1001/ledger \
  -H "Authorization: Bearer <YOUR_TOKEN>"
```

#### Health Checks
```bash
# Overall health
curl https://localhost:5001/health

# Detailed health check
curl https://localhost:5001/health/live

# UI/ready status
curl https://localhost:5001/health/ready
```

### Swagger UI Verification
1. Navigate to: `https://localhost:5001/swagger`
2. Verify all endpoints are listed:
   - Authentication endpoints (POST /api/auth/*)
   - Loan endpoints (GET/POST /api/loans/*)
   - Health check endpoints
3. Test endpoints directly from Swagger UI:
   - Click "Try it out" on any endpoint
   - Provide required parameters
   - Execute and verify response

### GraphQL Verification
1. Navigate to: `https://localhost:5001/graphql`
2. Copy sample query:
```graphql
query {
  activeLoans {
    loanNo
    employeeId
    principalAmount
    outstandingAmount
    status
  }
}
```
3. Execute and verify response

### Logging Verification
1. Check log files in: `bin/Debug/net8.0/logs/`
2. Verify entries:
   - Application startup messages
   - Request/response logging
   - Database operation logs
   - Error logs (if any)

## Project Structure Verification

```
✓ Solution structure created
  ├─ LoanAccount.Domain
  │  ├─ Common (Entity, ValueObject, DomainEvent)
  │  ├─ Entities (LoanMain, LoanInstallment, etc.)
  │  ├─ Events (Domain events)
  │  ├─ Interfaces (Repository interfaces)
  │  └─ ValueObjects (Money, InterestRate, etc.)
  │
  ├─ LoanAccount.Application
  │  ├─ Commands (CQRS commands)
  │  ├─ Queries (CQRS queries)
  │  ├─ DTOs (Data transfer objects)
  │  ├─ Validators (FluentValidation)
  │  ├─ Handlers (Command/Query handlers)
  │  ├─ Services (Application services)
  │  └─ Mapping (AutoMapper profiles)
  │
  ├─ LoanAccount.Infrastructure
  │  ├─ Persistence (DbContext, Configurations)
  │  ├─ Repositories (Repository implementations)
  │  ├─ UnitOfWork (Unit of Work pattern)
  │  ├─ Services (Azure Blob Storage service)
  │  ├─ Messaging (RabbitMQ publisher/consumer)
  │  ├─ HealthChecks (Custom health checks)
  │  ├─ EventPublishing (Domain event publishing)
  │  ├─ Resilience (Polly policies)
  │  ├─ Extensions (DI configuration)
  │  ├─ Migrations (EF migrations)
  │  └─ Seed (Database seeding)
  │
  ├─ LoanAccount.API
  │  ├─ Controllers (REST controllers)
  │  ├─ GraphQL (Query/Mutation types)
  │  ├─ Security (JWT token service)
  │  ├─ Middleware (Global exception handling)
  │  ├─ Extensions (Service registration)
  │  ├─ appsettings.json (Configuration)
  │  ├─ Program.cs (Startup)
  │  └─ Dockerfile (Optional)
  │
  ├─ LoanAccount.Functions
  │  ├─ LoanFunctions.cs (Timer/Blob triggers)
  │  ├─ Program.cs (Configuration)
  │  ├─ host.json (Azure Functions config)
  │  └─ local.settings.json (Local configuration)
  │
  └─ LoanAccount.Tests
     ├─ Unit Tests
     └─ Integration Tests
```

## Performance Baseline

### Expected Response Times (Development)
- GET /api/loans/active: ~100-200ms
- GET /api/loans/{id}/details: ~150-300ms
- POST /api/loans (Create): ~200-400ms
- POST /api/loans/{id}/payment: ~200-400ms

### Database Queries
- Active loans query: ~50ms
- Loan with details (includes installments): ~100ms
- Ledger entries for loan: ~75ms

## Troubleshooting During Verification

### Issue: Database Connection Failed
**Solution:**
```bash
# Verify SQL Server is running
sqllocaldb start

# Check connection string in appsettings.json
# Format: Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LoanAccountDb;...
```

### Issue: Migration Failed
**Solution:**
```bash
# Remove last migration
dotnet ef migrations remove

# Check migration history
dotnet ef migrations list

# Reapply migrations
dotnet ef database update
```

### Issue: Port 5001 Already in Use
**Solution:**
```bash
# Find process using port
netstat -ano | findstr :5001

# Kill process
taskkill /PID <PID> /F

# Or run on different port
dotnet run --urls="https://localhost:5002"
```

### Issue: JWT Token Validation Failed
**Solution:**
- Verify secret key in appsettings.json (minimum 32 characters)
- Check token format: `Bearer <token>`
- Verify token hasn't expired
- Check issuer and audience match configuration

### Issue: GraphQL Query Returns Null
**Solution:**
- Ensure seed data was populated
- Check database contains records
- Verify query syntax matches schema
- Check authorization header is provided

## Performance Optimization Checklist

- [ ] Database indexes on LoanNo, EmpSysId, UnitId
- [ ] Connection pooling configured in EF Core
- [ ] Query projections used to avoid loading unnecessary data
- [ ] Pagination implemented for large result sets
- [ ] Caching strategy defined (Redis optional)
- [ ] Async/await used throughout
- [ ] Logging configured appropriately
- [ ] Error handling middleware in place

## Post-Deployment Checklist

- [ ] Environment-specific appsettings configured
- [ ] Database migrations applied
- [ ] Seed data loaded
- [ ] JWT secret key changed for production
- [ ] CORS configured for frontend domains
- [ ] Health checks accessible
- [ ] Logging aggregation configured
- [ ] Monitoring and alerts set up
- [ ] Rate limiting configured
- [ ] API documentation updated

## Additional Resources

- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/)
- [HotChocolate GraphQL](https://chillicream.com/docs/hotchocolate)
- [MediatR CQRS Pattern](https://github.com/jbogard/MediatR)
- [Polly Resilience](https://github.com/App-vNext/Polly)
- [RabbitMQ Documentation](https://www.rabbitmq.com/documentation.html)

---

**Last Updated**: March 14, 2026
