# 🗂️ Project Structure Quick Reference Guide

## Directory Tree Overview

```
ApprovalService/
├── ApprovalService.sln
├── README.md
├── README_COMPREHENSIVE.md
├── QUICK_START.md
├── API_TESTING_GUIDE.md
├── ARCHITECTURE.md
├── IMPLEMENTATION_SUMMARY.md
├── COMPLETE_CHECKLIST.md
├── docker-compose.yml
├── Dockerfile
├── build.ps1
├── build.sh
│
├── src/
│   ├── Domain/
│   │   ├── ApprovalService.Domain.csproj
│   │   ├── Entities/
│   │   │   ├── ApprovalMaster.cs (Aggregate Root)
│   │   │   └── ApproverEmployee.cs (Entity)
│   │   ├── Events/
│   │   │   └── DomainEvents.cs (8 event records)
│   │   ├── Enums/
│   │   │   ├── ApprovalStatus.cs
│   │   │   └── ApproverStatus.cs
│   │   ├── Interfaces/
│   │   │   ├── IApprovalMasterRepository.cs
│   │   │   ├── IApproverEmployeeRepository.cs
│   │   │   ├── IUnitOfWork.cs
│   │   │   └── IDomainEventPublisher.cs
│   │   └── Base/
│   │       ├── Entity.cs
│   │       └── ValueObject.cs
│   │
│   ├── Application/
│   │   ├── ApprovalService.Application.csproj
│   │   ├── Commands/
│   │   │   ├── ApprovalCommands.cs (8 commands)
│   │   │   └── ApproverCommands.cs
│   │   ├── Queries/
│   │   │   ├── ApprovalQueries.cs (9 queries)
│   │   │   └── ApproverQueries.cs
│   │   ├── Handlers/
│   │   │   ├── ApprovalMasterCommandHandlers.cs
│   │   │   ├── ApproverEmployeeCommandHandlers.cs
│   │   │   ├── ApprovalQueryHandlers.cs
│   │   │   └── ApproverQueryHandlers.cs
│   │   ├── DTOs/
│   │   │   ├── ApprovalDtos.cs
│   │   │   ├── ApproverDtos.cs
│   │   │   ├── AuthDtos.cs
│   │   │   └── ResponseDtos.cs
│   │   ├── Validators/
│   │   │   └── Validators.cs (5 validators)
│   │   ├── Behaviors/
│   │   │   └── MediatRBehaviors.cs
│   │   └── Interfaces/
│   │       ├── IMessagePublisher.cs
│   │       ├── IBlobStorageService.cs
│   │       └── ITokenService.cs
│   │
│   ├── Infrastructure/
│   │   ├── ApprovalService.Infrastructure.csproj
│   │   ├── Database/
│   │   │   ├── ApprovalServiceDbContext.cs
│   │   │   ├── DbSeed.cs
│   │   │   └── Migrations/
│   │   │       ├── InitialCreate.cs
│   │   │       └── ApprovalServiceDbContextModelSnapshot.cs
│   │   ├── Repositories/
│   │   │   ├── ApprovalMasterRepository.cs
│   │   │   ├── ApproverEmployeeRepository.cs
│   │   │   └── UnitOfWork.cs
│   │   ├── Services/
│   │   │   ├── JwtTokenService.cs
│   │   │   └── BlobStorageService.cs
│   │   └── Messaging/
│   │       ├── RabbitMqMessagePublisher.cs
│   │       ├── RabbitMqConnectionFactory.cs
│   │       ├── RabbitMqConsumerBase.cs
│   │       ├── ApprovalMasterEventConsumer.cs
│   │       ├── ApproverEmployeeEventConsumer.cs
│   │       └── EventConsumerHost.cs
│   │
│   ├── API/
│   │   ├── ApprovalService.API.csproj
│   │   ├── Controllers/
│   │   │   ├── ApprovalsController.cs (8 endpoints)
│   │   │   ├── ApproversController.cs (7 endpoints)
│   │   │   └── AuthController.cs (3 endpoints)
│   │   ├── Middleware/
│   │   │   ├── GlobalExceptionHandlerMiddleware.cs
│   │   │   └── MiddlewareExtensions.cs
│   │   ├── Program.cs (DI, Configuration, Middleware Setup)
│   │   ├── MappingProfile.cs (AutoMapper config)
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   └── nlog.config
│   │
│   └── Functions/
│       ├── ApprovalService.Functions.csproj
│       ├── ProcessApprovalEvent.cs (ServiceBus trigger)
│       ├── ApprovalBackgroundTaskFunction.cs (Timer trigger)
│       ├── BlobProcessingFunction.cs (Blob trigger)
│       ├── local.settings.json
│       └── host.json
│
└── tests/ (Optional - test projects can be added here)
    ├── ApprovalService.Domain.Tests/
    ├── ApprovalService.Application.Tests/
    ├── ApprovalService.Infrastructure.Tests/
    └── ApprovalService.API.Tests/
```

---

## 🎯 Key File Locations by Use Case

### Starting Development
1. **First time setup**: [QUICK_START.md](QUICK_START.md)
2. **Environment config**: [appsettings.json](src/ApprovalService.API/appsettings.json)
3. **Run services**: [docker-compose.yml](docker-compose.yml)
4. **Build solution**: [build.ps1](build.ps1) or [build.sh](build.sh)

### Understanding Architecture
1. **System design**: [ARCHITECTURE.md](ARCHITECTURE.md)
2. **Implementation details**: [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md)
3. **DDD patterns**: [Domain Layer Entities](src/Domain/Entities)
4. **CQRS pattern**: [Application/Commands](src/Application/Commands) & [Application/Queries](src/Application/Queries)

### Implementing Features
1. **Domain logic**: [ApprovalMaster.cs](src/Domain/Entities/ApprovalMaster.cs)
2. **Business rules**: [Validators.cs](src/Application/Validators/Validators.cs)
3. **API endpoints**: [ApprovalsController.cs](src/API/Controllers/ApprovalsController.cs)
4. **Database access**: [ApprovalMasterRepository.cs](src/Infrastructure/Repositories/ApprovalMasterRepository.cs)

### Testing & Debugging
1. **API testing**: [API_TESTING_GUIDE.md](API_TESTING_GUIDE.md)
2. **Error handling**: [GlobalExceptionHandlerMiddleware.cs](src/API/Middleware/GlobalExceptionHandlerMiddleware.cs)
3. **Logging config**: [nlog.config](src/API/nlog.config)
4. **DI setup**: [Program.cs](src/API/Program.cs) - Lines 1-50

### Deployment
1. **Container config**: [Dockerfile](Dockerfile)
2. **Stack setup**: [docker-compose.yml](docker-compose.yml)
3. **Build automation**: [build.ps1](build.ps1) or [build.sh](build.sh)
4. **Health checks**: [Program.cs](src/API/Program.cs) - Health check section

### Authentication & Security
1. **JWT setup**: [JwtTokenService.cs](src/Infrastructure/Services/JwtTokenService.cs)
2. **Auth endpoints**: [AuthController.cs](src/API/Controllers/AuthController.cs)
3. **Security config**: [Program.cs](src/API/Program.cs) - Authentication section

### Messaging & Events
1. **Event definitions**: [DomainEvents.cs](src/Domain/Events/DomainEvents.cs)
2. **Message publish**: [RabbitMqMessagePublisher.cs](src/Infrastructure/Messaging/RabbitMqMessagePublisher.cs)
3. **Message consume**: [ApprovalMasterEventConsumer.cs](src/Infrastructure/Messaging/ApprovalMasterEventConsumer.cs)

### Database & Data
1. **Schema mapping**: [ApprovalServiceDbContext.cs](src/Infrastructure/Database/ApprovalServiceDbContext.cs)
2. **Sample data**: [DbSeed.cs](src/Infrastructure/Database/DbSeed.cs)
3. **Migrations**: [Migrations/InitialCreate.cs](src/Infrastructure/Database/Migrations/InitialCreate.cs)

---

## 🔗 Common Workflows

### Adding a New Approval Type

1. **Define entity enhancement** in [ApprovalMaster.cs](src/Domain/Entities/ApprovalMaster.cs)
2. **Add domain event** to [DomainEvents.cs](src/Domain/Events/DomainEvents.cs)
3. **Create command** in [ApprovalCommands.cs](src/Application/Commands/ApprovalCommands.cs)
4. **Create handler** in [ApprovalMasterCommandHandlers.cs](src/Application/Handlers/ApprovalMasterCommandHandlers.cs)
5. **Add validator** to [Validators.cs](src/Application/Validators/Validators.cs)
6. **Update DbContext mapping** in [ApprovalServiceDbContext.cs](src/Infrastructure/Database/ApprovalServiceDbContext.cs)
7. **Create migration** with `dotnet ef migrations add YourMigrationName`
8. **Add endpoint** to [ApprovalsController.cs](src/API/Controllers/ApprovalsController.cs)

### Fixing a Bug

1. **Check logs** in `logs/nlog-*.log`
2. **Trace error** using correlation IDs in logs
3. **Reproduce locally** using test case from [API_TESTING_GUIDE.md](API_TESTING_GUIDE.md)
4. **Add validation** if input validation missed
5. **Check repository** logic - [ApprovalMasterRepository.cs](src/Infrastructure/Repositories/ApprovalMasterRepository.cs)
6. **Verify exception handler** - [GlobalExceptionHandlerMiddleware.cs](src/API/Middleware/GlobalExceptionHandlerMiddleware.cs)
7. **Test fix** with curl command from testing guide

### Monitoring Health

1. **Check SQL Server**: `GET http://localhost:5000/health/sql`
2. **Check RabbitMQ**: `GET http://localhost:5000/health/rabbitmq`
3. **Full health check**: `GET http://localhost:5000/health`
4. **Swagger docs**: `https://localhost:5001/swagger`

### Deploying Changes

1. **Build locally**: `.\build.ps1`
2. **Run tests**: Embedded in build script
3. **Build Docker image**: `docker build -t approval-service:latest .`
4. **Start services**: `docker-compose up -d`
5. **Verify deployment**: Health check endpoints
6. **View logs**: `docker compose logs api`

---

## 📦 NuGet Dependencies Overview

| Package | Version | Purpose | Used In |
|---------|---------|---------|---------|
| MediatR | 12.1.1 | CQRS pattern | Domain, Application, API |
| FluentValidation | 11.8.1 | Input validation | Application, API |
| AutoMapper | 13.0.1 | DTO mapping | Application, API |
| EntityFrameworkCore | 8.0.3 | ORM & migrations | Infrastructure, API |
| RabbitMQ.Client | 6.8.1 | Message broker | Infrastructure |
| Azure.Storage.Blobs | 12.20.0 | Cloud storage | Infrastructure, Functions |
| Polly | 8.2.0 | Circuit breaker | Infrastructure, API |
| Serilog | 3.1.1 | Structured logging | API, Infrastructure |
| AspNetCore.HealthChecks | 8.0.1 | Health monitoring | API |
| System.IdentityModel.Tokens.Jwt | 7.1.2 | JWT handling | Infrastructure, API |

---

## 🔑 Configuration Keys Reference

### appsettings.json Sections

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "SQL Server connection string",
    "AzureBlob": "Storage account connection",
    "RabbitMq": "RabbitMQ connection"
  },
  "JwtSettings": {
    "SecretKey": "Minimum 32 characters",
    "Issuer": "ApprovalService",
    "Audience": "ApprovalServiceUsers",
    "ExpirationHours": 24
  },
  "RabbitMq": {
    "HostName": "localhost",
    "Port": 5672,
    "Username": "guest",
    "Password": "guest"
  },
  "Logging": {
    "LogLevel": { "Default": "Information" }
  }
}
```

---

## 🚀 Command Reference

### Build & Run
```powershell
# PowerShell (Windows)
.\build.ps1

# Bash (Linux/Mac)
./build.sh

# Manual build
dotnet build ApprovalService.sln
```

### Database
```powershell
# Create migration
dotnet ef migrations add InitialCreate

# Apply migration
dotnet ef database update

# Drop database
dotnet ef database drop
```

### Docker
```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f api

# Stop all services
docker-compose down
```

### API Testing
```powershell
# Using curl or Postman - see API_TESTING_GUIDE.md
curl -X GET http://localhost:5000/api/approvals
```

---

## ✅ Checklist: Before Going Live

- [ ] Update connection strings for production database
- [ ] Generate new JWT secret key (≥32 chars)
- [ ] Configure Azure services (if applicable)
- [ ] Review appsettings.Production.json
- [ ] Run complete test suite
- [ ] Load test with realistic data volumes
- [ ] Security scan with SonarQube or similar
- [ ] Set up monitoring & alerting
- [ ] Configure backups for SQL Server
- [ ] Document any custom business rules
- [ ] Train team on codebase structure
- [ ] Set up CI/CD pipeline
- [ ] Create runbooks for operational support

---

## 📞 Common Issues & Solutions

| Issue | Solution |
|-------|----------|
| "Connection string invalid" | Check appsettings.json, verify SQL Server running |
| "JWT token invalid" | Verify SecretKey matches in JwtTokenService |
| "RabbitMQ connection failed" | Check docker-compose, verify RabbitMQ container running |
| "Migration not found" | Run `dotnet ef migrations add YourMigration` |
| "Port 5000 already in use" | Change port in Program.cs or kill process on port |
| "Seed data not loading" | Check DbSeed.cs, verify data format matches schema |

---

## 🎓 Learning Path

**New to this codebase?** Follow this path:

1. Read [QUICK_START.md](QUICK_START.md) (5 min)
2. Read [README_COMPREHENSIVE.md](README.md) (15 min)
3. Explore [src/Domain/Entities](src/Domain/Entities) (10 min)
4. Review [ARCHITECTURE.md](ARCHITECTURE.md) (20 min)
5. Study [src/Application/Commands](src/Application/Commands) (15 min)
6. Follow [API_TESTING_GUIDE.md](API_TESTING_GUIDE.md) (30 min)
7. Run one endpoint through debugger (30 min)
8. Try adding a new endpoint (60 min)

**Total time: ~3 hours for solid understanding**

---

## 📋 Support Resources

- **Microsoft .NET Docs**: https://docs.microsoft.com/dotnet/
- **Entity Framework Core**: https://docs.microsoft.com/ef/core/
- **MediatR Pattern**: https://github.com/jbogard/MediatR
- **RabbitMQ**: https://www.rabbitmq.com/getstarted.html
- **Azure SDK**: https://azure.microsoft.com/sdk/
- **Docker**: https://docs.docker.com/

---

**Last Updated:** March 15, 2026
**Maintained By:** Development Team
**Status:** ✅ Active Development

