# HR Service - Quick Start Guide

## Quick Setup (5 minutes)

### 1. Clone/Open Solution
```bash
cd HRService
dotnet restore
```

### 2. Update Database
```powershell
# In Package Manager Console
Update-Database
```

### 3. Run API
```bash
cd HRService.API
dotnet run
```

### 4. Access Swagger
- **URL**: `https://localhost:7001/swagger`
- **Health Check**: `https://localhost:7001/health`

## Environment Variables

```
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PAYDB;Integrated Security=true;
Jwt__SecretKey=your-secret-key-at-least-32-characters
RabbitMQ__Host=localhost
```

## Docker Quick Start

```bash
# Build image
docker build -t hrservice:latest .

# Run container
docker run -p 5001:443 \
  -e ASPNETCORE_URLS=https://+:443 \
  -e Jwt__SecretKey=your-secret \
  hrservice:latest
```

## Common Commands

```bash
# Build
dotnet build

# Test
dotnet test

# Run
dotnet run --project HRService.API

# Add Migration
dotnet ef migrations add MigrationName --project HRService.Infrastructure --startup-project HRService.API

# View Logs
tail -f logs/hrservice*.txt
```

## API Examples

### Create Employee
```bash
curl -X POST https://localhost:7001/api/employees \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "employeeCode": "EMP001",
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@company.com",
    "dateOfBirth": "1990-01-01",
    "departmentId": "{dept-guid}",
    "positionId": "{pos-guid}",
    "siteId": "{site-guid}",
    "joinDate": "2024-01-01",
    "employmentType": "Permanent"
  }'
```

### Request Leave
```bash
curl -X POST https://localhost:7001/api/leaves \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "employeeId": "{emp-guid}",
    "leaveTypeId": "{leave-type-guid}",
    "startDate": "2024-04-01",
    "endDate": "2024-04-05",
    "reason": "Vacation"
  }'
```

## Troubleshooting

### Database Connection Failed
```bash
# Check SQL Server is running
sqlcmd -L  # List available servers

# Create new database
sqlcmd -S (localdb)\MSSQLLocalDB -Q "CREATE DATABASE PAYDB"
```

### Port Already in Use
```bash
# Run on different port
dotnet run --project HRService.API -- --urls "https://localhost:7002"
```

### Migration Failed
```powershell
# Rollback and retry
Update-Database -Migration 0
Update-Database
```

## Next Steps

1. Read [README.md](./README.md) for full documentation
2. Review [MIGRATIONS_GUIDE.md](./MIGRATIONS_GUIDE.md) for database setup
3. Check [API Documentation](./API_DOCUMENTATION.md)
4. Review [Architecture](./ARCHITECTURE.md)

---

For detailed instructions, see [README.md](./README.md)
