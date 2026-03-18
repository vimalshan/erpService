# Compensation Service - Quick Start Guide

## 5-Minute Quick Start

### 1. Clone and Navigate
```bash
cd e:\ERPMicroservice\src\Services\mainsparshServices\compensationServices
```

### 2. Restore and Build
```bash
dotnet restore
dotnet build CompensationService.sln
```

### 3. Apply Migrations
```bash
cd CompensationService.API
dotnet ef database update --project ../CompensationService.Infrastructure
```

### 4. Run Application
```bash
dotnet run
```

### 5. Access APIs
- **REST API**: https://localhost:7001/api/compensation-grades
- **GraphQL**: https://localhost:7001/graphql
- **Swagger**: https://localhost:7001/swagger
- **Health**: https://localhost:7001/health

---

## Project Structure Overview

```
CompensationService/
├── Domain/           # Business logic & rules
├── Application/      # Use cases & commands
├── Infrastructure/   # Database & external services
├── API/             # REST, GraphQL, Middleware
└── AzureFunctions/   # Background tasks
```

---

## Common Commands

```bash
# Build
dotnet build

# Run API
dotnet run --project CompensationService.API

# Database migration
dotnet ef database update --project CompensationService.Infrastructure

# Add new migration
dotnet ef migrations add MigrationName --project CompensationService.Infrastructure

# Test endpoints
curl https://localhost:7001/api/compensation-grades

# View health
curl https://localhost:7001/health
```

---

## API Examples

### Get All Grades
```bash
curl https://localhost:7001/api/compensation-grades
```

### Get Active Grades Only
```bash
curl https://localhost:7001/api/compensation-grades/active
```

### Create New Grade
```bash
curl -X POST https://localhost:7001/api/compensation-grades \
  -H "Content-Type: application/json" \
  -d '{
    "gradeCode":"NEW001",
    "gradeName":"New Grade",
    "gradeLevel":6,
    "baseSalary":150000,
    "hraPercentage":30,
    "daPercentage":15,
    "effectiveFrom":"2026-03-15"
  }'
```

### GraphQL Query
```bash
curl -X POST https://localhost:7001/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{
      compensationGrades {
        gradeId
        gradeCode
        gradeName
        totalSalary
      }
    }"
  }'
```

---

## File Structure

| Path | Purpose |
|---|---|
| `Domain/Entities/` | CompensationGrade aggregate |
| `Domain/ValueObjects/` | GradeCode, SalaryStructure, Status |
| `Domain/Events/` | Domain events |
| `Application/Commands/` | Create, Update, ChangeStatus |
| `Application/Queries/` | GetAll, GetActive, GetById |
| `Infrastructure/Persistence/` | DbContext, Migrations |
| `API/Controllers/` | REST endpoints |
| `API/GraphQL/` | GraphQL types |
| `API/Middleware/` | Error handling |

---

## Key Technologies

- **.NET**: 8.0
- **Database**: SQL Server (LocalDB)
- **ORM**: Entity Framework Core 8
- **API**: REST, GraphQL
- **Auth**: JWT
- **Messaging**: RabbitMQ
- **Cloud**: Azure (Blob Storage, Functions)
- **Resilience**: Polly
- **Logging**: Serilog

---

## Database Schema

Single table: `COMP_GRADE`
- 15 columns including audit fields
- 4 indexes for performance
- Seed data with 5 initial grades

---

## Features Implemented

✅ Clean Architecture
✅ CQRS Pattern
✅ Domain-Driven Design
✅ REST API
✅ GraphQL
✅ JWT Authentication
✅ Entity Framework Core
✅ Migrations & Seed Data
✅ RabbitMQ Integration
✅ Azure Blob Storage
✅ Azure Functions
✅ Health Checks
✅ Polly Resilience
✅ Serilog Logging
✅ Minimal APIs
✅ FluentValidation
✅ AutoMapper

---

## Troubleshooting

| Issue | Solution |
|---|---|
| Database not found | Run: `dotnet ef database update` |
| Port 7001 in use | Change in `launchSettings.json` |
| Build fails | Run: `dotnet clean && dotnet restore && dotnet build` |
| Migration issues | Run: `dotnet ef migrations remove --project Infrastructure` |
| RabbitMQ not responding | Ensure RabbitMQ service is running |

---

## Next Steps

1. ✅ **Review** the codebase structure
2. ✅ **Test** all API endpoints
3. ✅ **Explore** GraphQL queries
4. ✅ **Check** database schema
5. ✅ **Read** the detailed README.md
6. ✅ **Follow** DEPLOYMENT.md for production setup
7. ✅ **Add** business logic as needed
8. ✅ **Create** unit tests
9. ✅ **Deploy** to production

---

## Documentation Links

- [README.md](./README.md) - Comprehensive documentation
- [DEPLOYMENT.md](./DEPLOYMENT.md) - Build & deployment guide
- [Schema SQL](./CompensationModule/CompensationModule_Schema.sql) - Database schema

---

## Support

For issues or questions:
1. Check the comprehensive README.md
2. Review DEPLOYMENT.md for setup issues
3. Check application logs in `./logs` directory
4. Verify database connection string

---

**Version**: 1.0.0  
**Status**: Ready for Development  
**Last Updated**: March 15, 2026
