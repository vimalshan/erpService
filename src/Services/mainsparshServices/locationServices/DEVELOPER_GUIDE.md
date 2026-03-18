# Location Service - Developer Quick Reference

## 🚀 Quick Commands

### Setup
```bash
# Clone repository and navigate
cd LocationService

# Restore packages
dotnet restore

# Build solution
dotnet build
```

### Database
```bash
# Create migration
dotnet ef migrations add MigrationName -p LocationService.Infrastructure -s LocationService.API

# Update database
dotnet ef database update -s LocationService.API

# Drop database (development only)
dotnet ef database drop -s LocationService.API --force
```

### Running
```bash
# Run API (Development)
cd LocationService.API
dotnet run

# Run with specific configuration
dotnet run --configuration Development

# Run with HTTPS
dotnet run --urls "https://localhost:7000"
```

### Testing
```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "ClassName"

# Run with coverage
dotnet test /p:CollectCoverage=true
```

---

## 📚 Project Navigation

### When to work on each project:
- **Domain** - Adding new entities, value objects, business rules
- **Application** - Adding new commands/queries, handlers, DTOs
- **Infrastructure** - Adding repositories, external services, migrations
- **API** - Adding endpoints, GraphQL types, middleware
- **AzureFunctions** - Adding background tasks, event processors

---

## 🔑 Important Configuration Files

### appsettings.json (Production)
- Database connection string
- JWT secret key
- RabbitMQ credentials
- Blob storage connection

### appsettings.Development.json (Local Dev)
- LocalDB connection string
- Development JWT key (use for testing)
- Localhost RabbitMQ
- Cache settings (memory for dev)

---

## 🛠️ Debugging Tips

### Enable Entity Framework Logging
Add to Program.cs:
```csharp
.LogTo(Console.WriteLine, LogLevel.Information)
```

### Debug RabbitMQ Messages
Monitor RabbitMQ Admin Panel: http://localhost:15672
Default: guest/guest

### Check Health
```bash
curl http://localhost:5000/health
```

### Verify JWT Token
Use: https://jwt.io/

---

## 📋 Common Tasks

### Add New Aggregate
1. Create Aggregate class in Domain/Aggregates
2. Create related domain events
3. Define repository interface in Domain/Entities
4. Create DTOs in Application/DTOs
5. Create Commands in Application/Commands
6. Create Queries in Application/Queries
7. Create Handlers in Application/Handlers
8. Create HTTP endpoints in API/Controllers
9. Add EF mapping in Infrastructure/LocationServiceDbContext.cs
10. Create migration

### Add New External Service
1. Create interface in Infrastructure/ExternalServices
2. Implement the service
3. Register in Program.cs dependency injection
4. Use in handlers or controllers

### Add New Validation Rule
1. Create FluentValidation validator
2. Register in MediatR pipeline
3. Return validation error from handler

---

## 🔐 Security Checklist

- [ ] JWT secret key is strong (32+ characters)
- [ ] Set "Authorization" header in API calls
- [ ] HTTPS enabled in production
- [ ] Database backups scheduled
- [ ] RabbitMQ credentials secured
- [ ] Blob storage SAS tokens used
- [ ] API rate limiting configured
- [ ] Audit logging enabled

---

## 📊 Performance Optimization

### When to use Dapper:
- Complex queries with multiple joins
- High-performance read operations
- Batch operations

### When to enable Redis:
- Frequently accessed data
- High-traffic endpoints
- Aggregated query results

### When to use Polly:
- External API calls
- Intermittent network issues
- Scaling concerns

---

## 🐛 Troubleshooting

### Database Migration Failed
```bash
# Undo last migration
dotnet ef migrations remove -p LocationService.Infrastructure -s LocationService.API

# Or rollback
dotnet ef database update PreviousMigration -s LocationService.API
```

### JWT Token Validation Fails
- Check secret key matches
- Verify token not expired
- Confirm header format: `Authorization: Bearer {token}`

### RabbitMQ Connection Refused
- Start RabbitMQ service
- Verify host/port in appsettings
- Check firewall rules

### EF Core Exception
- Ensure DbContext registered in DI
- Check connection string
- Verify migrations applied

---

## 📞 Getting Help

### Documentation
- See IMPLEMENTATION_README.md for detailed docs
- See COMPLETION_SUMMARY.md for project overview

### Code Comments
- Look for TODO markers for incomplete sections
- Check XML comments on public APIs

### Testing
- Check test projects for usage examples
- Unit tests show expected behavior

---

## 💡 Best Practices

1. **Always use async/await** ✅
2. **Add domain events** when state changes significantly
3. **Validate at command handler level** before persisting
4. **Use DTOs** for API contracts, not entities
5. **Log important operations** using ILogger
6. **Handle exceptions** gracefully in middleware
7. **Keep aggregates small** - split if too large
8. **Use value objects** for complex types
9. **Write clear commit messages**
10. **Code review before merging**

---

## 🎯 Architecture Patterns Reference

### CQRS
```
Request → Command/Query → Handler → Domain → Repository → Database
Response ← DTO ← Handler ← Aggregate
```

### DDD
```
Aggregate → Repository → Entity → Value Object → Domain Event
```

### Event-Driven
```
Entity Change → Domain Event → Event Handler → Side Effects
```

---

## 🚢 Deployment

### Local
```bash
dotnet run --project LocationService.API
```

### Docker
```bash
docker build -t location-service .
docker run -p 5000:5000 location-service
```

### Azure App Service
```bash
az webapp up --resource-group myRG --name myLocationService
```

### Azure Functions
```bash
func azure functionapp publish myFunctionApp
```

---

**Last Updated**: March 15, 2026
**Version**: 1.0
